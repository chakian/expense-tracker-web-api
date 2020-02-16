package models

import (
	"database/sql"
	"os"

	u "expense-tracker-web-api/utils"

	"github.com/dgrijalva/jwt-go"
	"golang.org/x/crypto/bcrypt"
)

/*
Token ... JWT claims struct
*/
type Token struct {
	UserID uint
	jwt.StandardClaims
}

//User ... A struct to represent user account
type User struct {
	//gorm.Model
	ID       uint   `json:"id"`
	Email    string `json:"email"`
	Password string `json:"password"`
	Username string `json:"username"`
	Token    string `json:"token";sql:"-"`
}

//Validate incoming user details...
func (user *User) Validate() (map[string]interface{}, bool) {

	// if !strings.Contains(user.Email, "@") {
	// 	return u.Message(false, "Email address is required"), false
	// }

	if len(user.Password) < 6 {
		return u.Message(false, "Password is required"), false
	}

	//Email must be unique
	temp := &User{}

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	result, err := db.Query("SELECT id, email, username, password FROM users WHERE email = ? OR username = ?", temp.Email, temp.Username)
	if err != nil {
		panic(err.Error())
	}
	defer result.Close()

	var existingUser User
	for result.Next() {
		err := result.Scan(&existingUser.ID, &existingUser.Email, &existingUser.Username, &existingUser.Password)
		if err != nil {
			panic(err.Error())
		}
	}

	if existingUser.Email == temp.Email {
		return u.Message(false, "Email address already in use by another user."), false
	}

	if existingUser.Username == temp.Username {
		return u.Message(false, "Username already in use by another user."), false
	}

	// //check for errors and duplicate emails
	// err := GetDB().Table("accounts").Where("email = ?", account.Email).First(temp).Error
	// if err != nil && err != gorm.ErrRecordNotFound {
	// 	return u.Message(false, "Connection error. Please retry"), false
	// }
	// if temp.Email != "" {
	// 	return u.Message(false, "Email address already in use by another user."), false
	// }

	return u.Message(false, "Requirement passed"), true
}

// Create ...
func (user *User) Create() map[string]interface{} {

	if resp, ok := user.Validate(); !ok {
		return resp
	}

	hashedPassword, _ := bcrypt.GenerateFromPassword([]byte(user.Password), bcrypt.DefaultCost)
	user.Password = string(hashedPassword)

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	stmt, err := db.Prepare("INSERT INTO users(email, username, password) VALUES(?,?,?)")
	if err != nil {
		panic(err.Error())
	}

	_, err = stmt.Exec(user.Email, user.Username, user.Password)
	if err != nil {
		panic(err.Error())
	}

	// GetDB().Create(account)

	// if account.ID <= 0 {
	// 	return u.Message(false, "Failed to create account, connection error.")
	// }

	//Create new JWT token for the newly registered account
	tk := &Token{UserID: user.ID}
	token := jwt.NewWithClaims(jwt.GetSigningMethod("HS256"), tk)
	tokenString, _ := token.SignedString([]byte(os.Getenv("token_password")))
	user.Token = tokenString

	user.Password = "" //delete password

	response := u.Message(true, "Account has been created")
	response["user"] = user
	return response
}

/* func Login(email, password string) map[string]interface{} {

	account := &Account{}
	err := GetDB().Table("accounts").Where("email = ?", email).First(account).Error
	if err != nil {
		if err == gorm.ErrRecordNotFound {
			return u.Message(false, "Email address not found")
		}
		return u.Message(false, "Connection error. Please retry")
	}

	err = bcrypt.CompareHashAndPassword([]byte(account.Password), []byte(password))
	if err != nil && err == bcrypt.ErrMismatchedHashAndPassword { //Password does not match!
		return u.Message(false, "Invalid login credentials. Please try again")
	}
	//Worked! Logged In
	account.Password = ""

	//Create JWT token
	tk := &Token{UserId: account.ID}
	token := jwt.NewWithClaims(jwt.GetSigningMethod("HS256"), tk)
	tokenString, _ := token.SignedString([]byte(os.Getenv("token_password")))
	account.Token = tokenString //Store the token in the response

	resp := u.Message(true, "Logged In")
	resp["account"] = account
	return resp
}

func GetUser(u uint) *Account {

	acc := &Account{}
	GetDB().Table("accounts").Where("id = ?", u).First(acc)
	if acc.Email == "" { //User not found!
		return nil
	}

	acc.Password = ""
	return acc
}
*/
