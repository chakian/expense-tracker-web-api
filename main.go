package main

import (
	"io/ioutil"
	"net/http"

	"encoding/json"

	"math/rand"
	"strconv"

	"fmt"

	"github.com/gorilla/mux"

	"context"
	"log"
	"os"
	"time"

	"database/sql"

	_ "github.com/go-sql-driver/mysql"
)

var startupTime time.Time

// Transaction ... Yes
type Transaction struct {
	ID       string `json:"id"`
	Title    string `json:"title"`
	Category string `json:"category"`
}

// User ...
type User struct {
	ID       int    `json:"id"`
	Email    string `json:"email"`
	Username string `json:"username"`
	password string
}

var db *sql.DB
var err error

var transactions []Transaction

func getTransactions(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(transactions)
}

func getTransaction(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	params := mux.Vars(r)

	for index, item := range transactions {
		fmt.Printf(item.ID)
		if item.ID == params["id"] {
			json.NewEncoder(w).Encode(&transactions[index])
			break
		}
	}
}

func addTransaction(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	var transaction Transaction
	_ = json.NewDecoder(r.Body).Decode(&transaction)
	transaction.ID = strconv.Itoa(rand.Intn(1000000))
	transactions = append(transactions, transaction)
	json.NewEncoder(w).Encode(&transaction)
}

func updateTransaction(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)
	for index, item := range transactions {
		if item.ID == params["id"] {
			transactions = append(transactions[:index], transactions[index+1:]...)
			var transaction Transaction
			_ = json.NewDecoder(r.Body).Decode(&transaction)
			transaction.ID = params["id"]
			transactions = append(transactions, transaction)
			json.NewEncoder(w).Encode(&transaction)
			return
		}
	}
	json.NewEncoder(w).Encode(transactions)
}

func deleteTransaction(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)
	for index, item := range transactions {
		if item.ID == params["id"] {
			transactions = append(transactions[:index], transactions[index+1:]...)
			break
		}
	}
	json.NewEncoder(w).Encode(transactions)
}

func main() {

	if err := setup(context.Background()); err != nil {
		log.Fatalf("setup: %v", err)
	}

	router := mux.NewRouter()

	transactions = append(transactions, Transaction{ID: "1", Title: "Sigara", Category: "Deneme"})
	transactions = append(transactions, Transaction{ID: "2", Title: "Roberts", Category: "Cafe"})

	// Log when an appengine warmup request is used to create the new instance.
	// Warmup steps are taken in setup for consistency with "cold start" instances.
	router.HandleFunc("/_ah/warmup", func(w http.ResponseWriter, r *http.Request) {
		log.Println("warmup done")
	})
	router.HandleFunc("/", indexHandler)

	router.HandleFunc("/transactions", getTransactions).Methods("GET")
	router.HandleFunc("/transactions", addTransaction).Methods("POST")
	router.HandleFunc("/transactions/{id}", getTransaction).Methods("GET")
	router.HandleFunc("/transactions/{id}", updateTransaction).Methods("PUT")
	router.HandleFunc("/transactions/{id}", deleteTransaction).Methods("DELETE")

	router.HandleFunc("/register", registerUser).Methods("POST")
	router.HandleFunc("/listUsers", listUsers).Methods("GET")
	router.HandleFunc("/user/{id}", getUser).Methods("GET")
	router.HandleFunc("/user/{id}", updateUser).Methods("PUT")
	router.HandleFunc("/user/{id}", deleteUser).Methods("DELETE")

	apiTest := router.PathPrefix("/api/vTest").Subrouter()
	apiTest.HandleFunc("/testParams/{someID}", testParams).Methods("GET")

	port := os.Getenv("PORT")
	if port == "" {
		port = "8000"
		log.Printf("Defaulting to port %s", port)
	}

	log.Printf("Listening on port %s", port)
	if err := http.ListenAndServe(":"+port, router); err != nil {
		log.Fatal(err)
	}
}

func testParams(w http.ResponseWriter, r *http.Request) {
	pathParams := mux.Vars(r)
	w.Header().Set("Content-Type", "application/json")

	someID := -1
	var err error
	if val, ok := pathParams["someID"]; ok {
		someID, err = strconv.Atoi(val)
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			w.Write([]byte(`{"message": "need a number"}`))
			return
		}
	}

	query := r.URL.Query()
	location := query.Get("location")

	w.Write([]byte(fmt.Sprintf(`{"someID": %d, "location": "%s" }`, someID, location)))
}

// setup executes per-instance one-time warmup and initialization actions.
func setup(ctx context.Context) error {
	// Store the startup time of the server.
	startupTime = time.Now()

	return nil
}

// indexHandler responds to requests with our greeting.
func indexHandler(w http.ResponseWriter, r *http.Request) {
	if r.URL.Path != "/" {
		http.NotFound(w, r)
		return
	}
	uptime := time.Since(startupTime).Seconds()
	fmt.Fprintf(w, "Hello, World! Uptime: %.2fs\n", uptime)
}

func registerUser(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	stmt, err := db.Prepare("INSERT INTO user(email, username, password) VALUES(?,?,?)")
	if err != nil {
		panic(err.Error())
	}

	body, err := ioutil.ReadAll(r.Body)
	if err != nil {
		panic(err.Error())
	}

	keyVal := make(map[string]string)
	json.Unmarshal(body, &keyVal)

	email := keyVal["email"]
	username := keyVal["username"]
	password := keyVal["password"]

	_, err = stmt.Exec(email, username, password)
	if err != nil {
		panic(err.Error())
	}

	fmt.Fprintf(w, "OK")
}

func listUsers(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	var users []User

	result, err := db.Query("SELECT id, email, username, password FROM user")
	if err != nil {
		panic(err.Error())
	}

	for result.Next() {
		var user User

		err := result.Scan(&user.ID, &user.Email, &user.Username, &user.password)
		if err != nil {
			panic(err.Error())
		}

		users = append(users, user)
	}

	json.NewEncoder(w).Encode(users)
}

func getUser(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	params := mux.Vars(r)

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	fmt.Println("ID parametremizin değeri:" + params["id"])
	result, err := db.Query("SELECT id, email, username, password FROM user WHERE id = ?", params["id"])
	if err != nil {
		panic(err.Error())
	}
	defer result.Close()

	var user User
	for result.Next() {
		err := result.Scan(&user.ID, &user.Email, &user.Username, &user.password)
		if err != nil {
			panic(err.Error())
		}
		fmt.Println("looping")
	}

	json.NewEncoder(w).Encode(user)
}

func updateUser(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	stmt, err := db.Prepare("UPDATE user SET password = ? WHERE id = ?")
	if err != nil {
		panic(err.Error())
	}

	body, err := ioutil.ReadAll(r.Body)
	if err != nil {
		panic(err.Error())
	}

	keyVal := make(map[string]string)
	json.Unmarshal(body, &keyVal)
	newPassword := keyVal["password"]

	_, err = stmt.Exec(newPassword, params["id"])
	if err != nil {
		panic(err.Error())
	}

	fmt.Fprintf(w, "Password has been updated for user with ID = %s", params["id"])
}

func deleteUser(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)

	db, err := sql.Open("mysql", "root:123456@tcp(127.0.0.1:3306)/expense_tracker")
	if err != nil {
		panic(err.Error())
	}
	defer db.Close()

	stmt, err := db.Prepare("DELETE FROM user WHERE id = ?")
	if err != nil {
		panic(err.Error())
	}

	_, err = stmt.Exec(params["id"])
	if err != nil {
		panic(err.Error())
	}

	fmt.Fprintf(w, "User with ID = %s is deleted", params["id"])
}
