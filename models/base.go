package models

import (
	"fmt"

	_ "github.com/go-sql-driver/mysql" //we do the db operations here
	"github.com/jinzhu/gorm"
	"github.com/joho/godotenv"
)

var db *gorm.DB //database

func init() {

	e := godotenv.Load() //Load .env file
	if e != nil {
		fmt.Print(e)
	}

	// username := os.Getenv("db_user")
	// password := os.Getenv("db_pass")
	// dbName := os.Getenv("db_name")
	// dbHost := os.Getenv("db_host")
	username := "root"
	password := "123456"
	dbName := "expense_tracker"
	dbHost := "127.0.0.1"

	dbURI := fmt.Sprintf("%s:%s@tcp(%s:3306)/%s", username, password, dbHost, dbName) //Build connection string
	fmt.Println(dbURI)

	//db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")

	conn, err := gorm.Open("mysql", dbURI)
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
