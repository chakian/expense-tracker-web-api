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
	ActiveFlag   uint8      `json:"active_flag" gorm:"column:active_flag"`
	InsertUserID uint       `json:"insert_user_id" gorm:"column:insert_user_id"`
	InsertTime   time.Time  `json:"insert_time" gorm:"column:insert_time"`
	UpdateUserID *uint      `json:"update_user_id" gorm:"column:update_user_id"`
	UpdateTime   *time.Time `json:"update_time" gorm:"column:update_time"`
}

// SetAuditValuesForInsert ...
func SetAuditValuesForInsert(model *BaseAuditableModel, isActive uint8, user uint) {
	model.ActiveFlag = isActive
	model.InsertUserID = user
	model.InsertTime = time.Now().UTC()
	model.UpdateUserID = nil
	model.UpdateTime = nil
}

// SetAuditValuesForUpdate ...
func SetAuditValuesForUpdate(model BaseAuditableModel, isActive uint8, user uint) {
	model.ActiveFlag = isActive
	model.UpdateUserID = &user
	currentTime := time.Now().UTC()
	model.UpdateTime = &currentTime
}

var db *gorm.DB

func init() {
	fmt.Println("started MODELS -> BASE init")

	// u.ReadCredentials()

	// fmt.Println(u.Credentials.DbHost)
	// fmt.Println(u.Credentials.DbName)
	// fmt.Println(u.Credentials.DbUser)
	// fmt.Println(u.Credentials.DbType)

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
