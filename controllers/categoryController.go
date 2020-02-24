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

	params := mux.Vars(r)
	category := &models.Category{}
	err := json.NewDecoder(r.Body).Decode(category)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}
	categoryid, err := strconv.ParseUint(params["category_id"], 0, 32)
	if err != nil {
		u.Respond(w, u.Message(false, "there was a problem with 'category_id' parameter"))
		return
	}
	category.BudgetCategoryID = uint(categoryid)
	userid := app.GetUserID(r)

	resp := category.Update(userid)
	u.Respond(w, resp)

	log.Print("Finished: categoryController.UpdateCategory")
}
