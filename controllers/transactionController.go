package controllers

import (
	"encoding/json"
	"expense-tracker-web-api/app"
	"expense-tracker-web-api/models"
	u "expense-tracker-web-api/utils"
	"log"
	"net/http"
	"time"
)

type transactionFilter struct {
	BudgetID  uint      `json:"budgetId"`
	StartDate time.Time `json:"startDate"`
	EndDate   time.Time `json:"endDate"`
}

// GetTransactionList ...
var GetTransactionList = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.GetTransactionList")

	userid := app.GetUserID(r)
	filter := &transactionFilter{}

	err := json.NewDecoder(r.Body).Decode(filter)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}
	if filter.BudgetID <= 0 {
		u.Respond(w, u.Message(false, "budgetId field is required"))
		return
	}
	// TODO: More validations... StartDate can't be later than EndDate, they can't be empty...

	data := models.GetFilteredTransactionList(filter.BudgetID, filter.StartDate, filter.EndDate, userid)
	resp := u.Message(true, "ok")
	resp["data"] = data
	u.Respond(w, resp)

	log.Print("Finished: transactionController.GetTransactionList")
}

// CreateTransaction ...
var CreateTransaction = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.CreateTransaction")

	userid := app.GetUserID(r)
	transaction := &models.TransactionHeader{}

	err := json.NewDecoder(r.Body).Decode(transaction)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := transaction.Create(userid)
	u.Respond(w, resp)

	log.Print("Finished: transactionController.CreateTransaction")
}

// UpdateTransaction ...
var UpdateTransaction = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.UpdateTransaction")

	userid := app.GetUserID(r)
	transaction := &models.TransactionHeader{}

	err := json.NewDecoder(r.Body).Decode(transaction)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := transaction.Update(userid)
	u.Respond(w, resp)

	log.Print("Finished: transactionController.UpdateTransaction")
}

// DeleteTransaction ...
var DeleteTransaction = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: transactionController.DeleteTransaction")
	log.Print("Finished: transactionController.DeleteTransaction")
}
