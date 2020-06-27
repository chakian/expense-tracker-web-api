package models

import (
	"fmt"
	"os"
	"strings"
	"time"

	c "expense-tracker-web-api/configutils"
	u "expense-tracker-web-api/utils"

	_ "github.com/go-sql-driver/mysql" //we do the db operations in this package. This comment is mandatory for lint
	"github.com/jinzhu/gorm"
)

// BaseAuditableModel ...
type BaseAuditableModel struct {
	ActiveFlag   uint8      `json:"isActive" gorm:"column:active_flag"`
	InsertUserID uint       `json:"insertUserId" gorm:"column:insert_user_id"`
	InsertTime   time.Time  `json:"insertTime" gorm:"column:insert_time"`
	UpdateUserID *uint      `json:"updateUserId" gorm:"column:update_user_id"`
	UpdateTime   *time.Time `json:"updateTime" gorm:"column:update_time"`
}

// SetAuditValuesForInsert ...
func SetAuditValuesForInsert(model *BaseAuditableModel, isActive uint8, userid uint) {
	model.ActiveFlag = isActive
	model.InsertUserID = userid
	model.InsertTime = time.Now().UTC()
	model.UpdateUserID = nil
	model.UpdateTime = nil
}

// SetAuditValuesForUpdate ...
func SetAuditValuesForUpdate(model *BaseAuditableModel, isActive uint8, userid uint) {
	model.ActiveFlag = isActive
	model.UpdateUserID = &userid
	currentTime := time.Now().UTC()
	model.UpdateTime = &currentTime
}

var db *gorm.DB

func isInTests() bool {
	for _, arg := range os.Args {
		if strings.HasPrefix(arg, "-test.") {
			return true
		}
	}
	return false
}

func init() {
	fmt.Println("initializing db")

	if isInTests() {
		fmt.Print("TEST ENVIRONMENT. Skipping Db Initialization")
		return
	}

	// if os.Getenv("GO_ENV") == "TEST" {
	// 	fmt.Print("TEST ENVIRONMENT. Skipping Db Initialization")
	// 	return
	// }

	c.Initialize()

	dbURI := getDBURI()
	conn, err := gorm.Open("mysql", dbURI)
	u.CheckAndLog(err)

	db = conn
	if c.Config.Server.Environment == "LOCAL" {
		db.LogMode(true)
		db.Debug() //.AutoMigrate(&Account{}, &Contact{}) //Database migration
	} else {
		db.Begin()
	}

	fmt.Println("db initialization completed")
}

func getDBURI() string {
	var (
		username               = c.Config.Database.DbUser
		password               = c.Config.Database.DbPass
		dbName                 = c.Config.Database.DbName
		dbHost                 = c.Config.Database.DbHost
		dbPort                 = c.Config.Database.DbPort
		instanceConnectionName = c.Config.Database.InstanceName
	)

	var dbURI string

	if c.Config.Server.Environment == "LOCAL" {
		dbURI = fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?parseTime=true", username, password, dbHost, dbPort, dbName)
	} else {
		dbURI = fmt.Sprintf("%s:%s@unix(/cloudsql/%s)/%s?parseTime=true", username, password, instanceConnectionName, dbName)
	}

	fmt.Printf("DBString: '%s\n", dbURI)

	return dbURI
}

// GetDB ... returns a handle to the DB object
func GetDB() *gorm.DB {
	return db
}
