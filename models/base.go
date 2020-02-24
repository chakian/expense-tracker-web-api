package models

import (
	"fmt"
	"time"

	"expense-tracker-web-api/utils"
	u "expense-tracker-web-api/utils"

	_ "github.com/go-sql-driver/mysql" //we do the db operations in this package. This comment is mandatory for lint
	"github.com/jinzhu/gorm"
)

// "github.com/joho/godotenv"

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

func init() {
	fmt.Println("initializing db")

	dbURI := getDBURI()
	conn, err := gorm.Open("mysql", dbURI)
	u.CheckAndLog(err)

	db = conn
	if utils.Config.Server.Environment == "LOCAL" {
		db.LogMode(true)
		db.Debug() //.AutoMigrate(&Account{}, &Contact{}) //Database migration
	} else {
		db.Begin()
	}

	fmt.Println("db initialization completed")
}

func getDBURI() string {
	var (
		username               = u.Config.Database.DbUser
		password               = u.Config.Database.DbPass
		dbName                 = u.Config.Database.DbName
		dbHost                 = u.Config.Database.DbHost
		dbPort                 = u.Config.Database.DbPort
		instanceConnectionName = u.Config.Database.InstanceName
	)

	var dbURI string

	if u.Config.Server.Environment == "LOCAL" {
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
