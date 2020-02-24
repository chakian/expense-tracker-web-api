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

// CreateBudget ...
var CreateBudget = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: budgetController.CreateBudget")

	userid := app.GetUserID(r)
	budget := &models.Budget{}

	err := json.NewDecoder(r.Body).Decode(budget)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := budget.Create(userid)
	u.Respond(w, resp)

	log.Print("Finished: budgetController.CreateBudget")
}

// GetBudgetsOfUser ...
var GetBudgetsOfUser = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: budgetController.GetBudgetsOfUser")

	userid := app.GetUserID(r)

	data := models.GetBudgets(userid)
	resp := u.Message(true, "ok")
	resp["data"] = data
	u.Respond(w, resp)

	log.Print("Finished: budgetController.GetBudgetsOfUser")
}

// GetBudgetByID ...
var GetBudgetByID = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: budgetController.GetBudgetByID")

	params := mux.Vars(r)
	budgetid, err := strconv.ParseUint(params["id"], 0, 32)
	if err != nil {
		u.Respond(w, u.Message(false, "there was a problem with 'budget_id' parameter"))
		return
	}
	userid := app.GetUserID(r)

	data := models.GetBudget(uint(budgetid), userid)
	resp := u.Message(true, "ok")
	resp["data"] = data
	u.Respond(w, resp)

	log.Print("Finished: budgetController.GetBudgetByID")
}

// UpdateBudget ...
var UpdateBudget = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: budgetController.UpdateBudget")

	params := mux.Vars(r)
	budget := &models.Budget{}
	err := json.NewDecoder(r.Body).Decode(budget)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}
	budgetid, err := strconv.ParseUint(params["id"], 0, 32)
	if err != nil {
		u.Respond(w, u.Message(false, "there was a problem with 'budget_id' parameter"))
		return
	}
	budget.BudgetID = uint(budgetid)
	userid := app.GetUserID(r)

	resp := budget.Update(userid)
	u.Respond(w, resp)

	log.Print("Finished: budgetController.UpdateBudget")
}

// AddUserToBudget ...
var AddUserToBudget = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: budgetController.AddUserToBudget")

	budgetUser := &models.BudgetUser{}
	err := json.NewDecoder(r.Body).Decode(budgetUser)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}
	userid := app.GetUserID(r)

	resp := budgetUser.AddUserToBudget(userid)
	u.Respond(w, resp)

	log.Print("Finished: budgetController.AddUserToBudget")
}
