package models

import (
	"fmt"
	"os"
	"time"

	u "expense-tracker-web-api/utils"

	"github.com/dgrijalva/jwt-go"
	"github.com/jinzhu/gorm"
	"golang.org/x/crypto/bcrypt"
)

//User ... A struct to represent user account
type User struct {
	ID              uint      `json:"userId" gorm:"primary_key;column:user_id"`
	Email           string    `json:"email"`
	Password        string    `json:"password"`
	Name            string    `json:"name" gorm:"column:user_name"`
	RegisterDate    time.Time `json:"-" gorm:"column:register_date"`
	Token           string    `json:"token" gorm:"-"`
	DefaultBudgetID uint      `json:"defaultBudgetID" gorm:"column:default_budget_id"`
}

// TableName ...
func (User) TableName() string {
	return "user"
}

/*
Token ... JWT claims struct
*/
type Token struct {
	UserID uint
	jwt.StandardClaims
}

//Validate incoming user details...
func (user *User) Validate() (map[string]interface{}, bool) {

	if !u.ValidateEmail(user.Email) {
		return u.Message(false, "Invalid email address"), false
	}

	if len(user.Password) < 6 {
		return u.Message(false, "Password is shorter than 6 characters"), false
	}

	//Email must be unique
	temp := &User{}

	// check for errors and duplicate emails
	err := GetDB().Table("user").Where("email = ?", user.Email).First(temp).Error
	if err != nil && err != gorm.ErrRecordNotFound {
		return u.Message(false, "Connection error. Please retry"), false
	}
	if user.Email == temp.Email {
		return u.Message(false, "Email address already in use by another user."), false
	}

	return u.Message(false, "Requirement passed"), true
}

// Create ...
func (user *User) Create() map[string]interface{} {

	if resp, ok := user.Validate(); !ok {
		return resp
	}

	hashedPassword, _ := bcrypt.GenerateFromPassword([]byte(user.Password), bcrypt.DefaultCost)
	user.Password = string(hashedPassword)
	user.RegisterDate = time.Now().UTC()

	GetDB().Create(user)

	if user.ID <= 0 {
		return u.Message(false, "Failed to create user, connection error.")
	}

	//Create new JWT token for the newly registered account
	tk := &Token{UserID: user.ID}
	token := jwt.NewWithClaims(jwt.GetSigningMethod("HS256"), tk)
	tokenString, _ := token.SignedString([]byte(os.Getenv("token_password")))
	user.Token = tokenString

	user.Password = "" //delete password

	response := u.Message(true, "User account has been created")
	response["user"] = user
	return response
}

// Login ...
func Login(email, password string) map[string]interface{} {

	user := &User{}
	err := GetDB().Table("user").Where("email = ?", email).First(user).Error
	if err != nil {
		if err == gorm.ErrRecordNotFound {
			return u.Message(false, "Email address not found")
		}
		return u.Message(false, "Connection error. Please retry")
	}

	err = bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(password))
	if err != nil && err == bcrypt.ErrMismatchedHashAndPassword { //Password does not match!
		return u.Message(false, "Invalid login credentials. Please try again")
	}
	//Worked! Logged In
	user.Password = ""

	//Create JWT token
	tk := &Token{UserID: user.ID}
	token := jwt.NewWithClaims(jwt.GetSigningMethod("HS256"), tk)
	tokenString, _ := token.SignedString([]byte(os.Getenv("token_password")))
	user.Token = tokenString //Store the token in the response

	resp := u.Message(true, "Logged In")
	resp["user"] = user
	return resp
}

// // GetUser ...
// func GetUser(u uint) *User {

// 	user := &User{}
// 	GetDB().Table("user").Where("id = ?", u).First(user)
// 	if user.Email == "" { //User not found!
// 		return nil
// 	}

// 	user.Password = ""
// 	return user
// }

// SearchUsersByEmail ...
func SearchUsersByEmail(email string, userid uint) []*User {
	users := make([]*User, 0)

	//SELECT * FROM budget INNER JOIN budget_user ON budget.budget_id = budget_user.budget_id WHERE budget_user.user_id = 1
	err := GetDB().Table("user").Where("user.email = ? AND user.active_flag = ?", email, 1).Find(&users).Error
	if err != nil {
		fmt.Println(err)
		return nil
	}

	for i := range users {
		users[i].Password = ""
	}

	return users
}
