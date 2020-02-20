package models

import (
	"fmt"
	"os"
	"time"

	_ "github.com/go-sql-driver/mysql" //we do the db operations in this package. This comment is mandatory for lint
	"github.com/jinzhu/gorm"
	"github.com/joho/godotenv"
)

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

	e := godotenv.Load() //Load .env file
	if e != nil {
		fmt.Print(e)
	}

	dbURI := getDBURI()
	conn, err := gorm.Open("mysql", dbURI)
	if err != nil {
		fmt.Print(err)
	}

	db = conn
	db.LogMode(true)
	db.Debug() //.AutoMigrate(&Account{}, &Contact{}) //Database migration
}

func getDBURI() string {
	// LOCAL
	// username := os.Getenv("DB_USER")
	// password := os.Getenv("DB_PASS")
	// dbNameLocal := os.Getenv("DB_NAME")
	// dbHost := os.Getenv("db_host")
	// dbPort := os.Getenv("db_port")
	// dbURI := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?parseTime=true", username, password, dbHost, dbPort, dbNameLocal)

	// GCP TEST
	var (
		dbUser                 = os.Getenv("DB_USER")
		dbPwd                  = os.Getenv("DB_PASS")
		instanceConnectionName = os.Getenv("INSTANCE_CONNECTION_NAME")
		dbName                 = os.Getenv("DB_NAME")
	)
	dbURI := fmt.Sprintf("%s:%s@unix(/cloudsql/%s)/%s?parseTime=true", dbUser, dbPwd, instanceConnectionName, dbName)

	// GCP PROD

	fmt.Printf("DBString: '%s", dbURI)
	return dbURI
}

// GetDB ... returns a handle to the DB object
func GetDB() *gorm.DB {
	return db
}
