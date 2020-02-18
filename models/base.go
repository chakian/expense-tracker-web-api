package models

import (
	"fmt"
	"os"

	_ "github.com/go-sql-driver/mysql" //we do the db operations in this package. This comment is mandatory for lint
	"github.com/jinzhu/gorm"
	"github.com/joho/godotenv"
)

var db *gorm.DB

func init() {

	e := godotenv.Load() //Load .env file
	if e != nil {
		fmt.Print(e)
	}

	dbType := os.Getenv("db_type")
	username := os.Getenv("db_user")
	password := os.Getenv("db_pass")
	dbName := os.Getenv("db_name")
	dbHost := os.Getenv("db_host")
	dbPort := os.Getenv("db_port")
	dbURI := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?parseTime=true", username, password, dbHost, dbPort, dbName) //Build connection string

	conn, err := gorm.Open(dbType, dbURI)
	if err != nil {
		fmt.Print(err)
	}

	db = conn
	db.Debug() //.AutoMigrate(&Account{}, &Contact{}) //Database migration
}

// GetDB ... returns a handle to the DB object
func GetDB() *gorm.DB {
	return db
}
