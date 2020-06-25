package controllers

import (
	"log"
	"net/http"
)

// GetTransactionList ...
var GetTransactionList = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.GetTransactionList")
	log.Print("Finished: transactionController.GetTransactionList")
}

// CreateTransaction ...
var CreateTransaction = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.CreateTransaction")
	log.Print("Finished: transactionController.CreateTransaction")
}

// UpdateTransaction ...
var UpdateTransaction = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.UpdateTransaction")
	log.Print("Finished: transactionController.UpdateTransaction")
}

// DeleteTransaction ...
var DeleteTransaction = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.DeleteTransaction")
	log.Print("Finished: transactionController.DeleteTransaction")
}
