package main

import (
	"net/http"

	"encoding/json"

	"math/rand"
	"strconv"

	"github.com/gorilla/mux"
)

// Transaction ... Yes
type Transaction struct {
	ID       string `json:"id"`
	Title    string `json:"title"`
	Category string `json:"category"`
}

var transactions []Transaction

func getTransactions(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(transactions)
}

func getTransaction(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)
	for _, item := range transactions {
		if item.ID == params["id"] {
			json.NewEncoder(w).Encode(item)
			break
		}
		return
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
	router := mux.NewRouter()

	transactions = append(transactions, Transaction{ID: "1", Title: "Sigara", Category: "Hobi"})
	transactions = append(transactions, Transaction{ID: "2", Title: "Roberts", Category: "Cafe"})

	router.HandleFunc("/transactions", getTransactions).Methods("GET")
	router.HandleFunc("/transactions", addTransaction).Methods("POST")
	router.HandleFunc("/transactions/{id}", getTransaction).Methods("GET")
	router.HandleFunc("/transactions/{id}", updateTransaction).Methods("PUT")
	router.HandleFunc("/transactions/{id}", deleteTransaction).Methods("DELETE")

	http.ListenAndServe(":8000", router)
}
