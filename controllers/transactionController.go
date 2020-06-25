package controllers

import (
	"encoding/json"
	"expense-tracker-web-api/app"
	"expense-tracker-web-api/models"
	u "expense-tracker-web-api/utils"
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

	userid := app.GetUserID(r)
	transactionHeader := &models.TransactionHeader{}

	err := json.NewDecoder(r.Body).Decode(transactionHeader)
	if err != nil {
		log.Print(err)
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := transactionHeader.Create(userid)
	u.Respond(w, resp)

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
