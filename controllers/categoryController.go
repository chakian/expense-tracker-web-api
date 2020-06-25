package controllers

import (
	"encoding/json"
	"log"
	"net/http"
	"strconv"

	"github.com/chakian/expense-tracker-web-api/app"
	"github.com/chakian/expense-tracker-web-api/models"
	u "github.com/chakian/expense-tracker-web-api/utils"

	"github.com/gorilla/mux"
)

// GetCategoriesOfBudget ...
var GetCategoriesOfBudget = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: categoryController.GetCategoriesOfBudget")

	userid := app.GetUserID(r)
	log.Print("userid : ", userid)

	params := mux.Vars(r)
	budgetid, err := strconv.ParseUint(params["budget_id"], 0, 32)
	if err != nil {
		u.Respond(w, u.Message(false, "there was a problem with 'budget_id' parameter"))
		return
	}

	data := models.GetCategoryListByBudgetID(uint(budgetid), userid)
	resp := u.Message(true, "ok")
	resp["data"] = data
	u.Respond(w, resp)

	log.Print("Finished: categoryController.GetCategoriesOfBudget")
}

// CreateCategory ...
var CreateCategory = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: categoryController.CreateCategory")

	userid := app.GetUserID(r)
	category := &models.Category{}

	err := json.NewDecoder(r.Body).Decode(category)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	resp := category.Create(userid)
	u.Respond(w, resp)

	log.Print("Finished: categoryController.CreateCategory")
}

// UpdateCategory ...
var UpdateCategory = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: categoryController.UpdateCategory")

	category := &models.Category{}
	err := json.NewDecoder(r.Body).Decode(category)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	category.BudgetCategoryID = uint(category.BudgetCategoryID)
	userid := app.GetUserID(r)

	resp := category.Update(userid)
	u.Respond(w, resp)

	log.Print("Finished: categoryController.UpdateCategory")
}
