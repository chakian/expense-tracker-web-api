package controllers

import (
	"encoding/json"
	"expense-tracker-web-api/app"
	"expense-tracker-web-api/models"
	u "expense-tracker-web-api/utils"
	"net/http"
)

// CreateBudget ...
var CreateBudget = func(w http.ResponseWriter, r *http.Request) {
	user := r.Context().Value("user").(uint)
	budget := &models.Budget{}

	err := json.NewDecoder(r.Body).Decode(budget)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := budget.Create(user)
	u.Respond(w, resp)
}

// GetBudgetsOfUser ...
var GetBudgetsOfUser = func(w http.ResponseWriter, r *http.Request) {
	// params := mux.Vars(r)
	userid := app.GetUserID(r)

	data := models.GetBudgets(userid)
	resp := u.Message(true, "ok")
	resp["data"] = data
	u.Respond(w, resp)
}
