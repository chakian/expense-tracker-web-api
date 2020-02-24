package controllers

import (
	"encoding/json"
	"expense-tracker-web-api/models"
	u "expense-tracker-web-api/utils"
	"net/http"
)

// CreateUser ...
var CreateUser = func(w http.ResponseWriter, r *http.Request) {

	user := &models.User{}
	err := json.NewDecoder(r.Body).Decode(user)
	if err != nil {
		u.Respond(w, u.Message(false, "Invalid request"))
		return
	}

	resp := user.Create() //Create user

	if resp["status"] == "true" {
		userid := user.ID
		budget := &models.Budget{BudgetName: "Bütçem"}
		budget.Create(userid)
		// resp := budget.Create(userid)
	}

	u.Respond(w, resp)
}

// Authenticate ...
var Authenticate = func(w http.ResponseWriter, r *http.Request) {

	user := &models.User{}
	err := json.NewDecoder(r.Body).Decode(user)
	if err != nil {
		u.Respond(w, u.Message(false, "Invalid request"))
		return
	}

	resp := models.Login(user.Email, user.Password)
	u.Respond(w, resp)
}
