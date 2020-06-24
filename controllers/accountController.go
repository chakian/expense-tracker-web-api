package controllers

import (
	"encoding/json"
	"expense-tracker-web-api/app"
	"expense-tracker-web-api/models"
	u "expense-tracker-web-api/utils"
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
)

// GetAccountsOfBudget ...
var GetAccountsOfBudget = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: accountController.GetAccountsOfBudget")
	log.Print("Finished: accountController.GetAccountsOfBudget")
}

// CreateAccount ...
var CreateAccount = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: accountController.CreateAccount")

	userid := app.GetUserID(r)
	account := &models.Account{}

	err := json.NewDecoder(r.Body).Decode(account)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := account.Create(userid)
	u.Respond(w, resp)

	log.Print("Finished: accountController.CreateAccount")
}

// UpdateAccount ...
var UpdateAccount = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: accountController.UpdateAccount")

	params := mux.Vars(r)
	account := &models.Account{}
	err := json.NewDecoder(r.Body).Decode(account)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}
	accountid, err := strconv.ParseUint(params["account_id"], 0, 32)
	if err != nil {
		u.Respond(w, u.Message(false, "there was a problem with 'account_id' parameter"))
		return
	}
	account.BudgetAccountID = uint(accountid)
	userid := app.GetUserID(r)

	resp := account.Update(userid)
	u.Respond(w, resp)

	log.Print("Finished: accountController.UpdateAccount")
}
