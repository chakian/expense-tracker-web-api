package main

import (
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
)

var startupTime time.Time

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

	transactions = append(transactions, Transaction{ID: "1", Title: "Sigara", Category: "Hobi"})
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
