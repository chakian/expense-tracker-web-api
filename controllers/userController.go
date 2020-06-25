package controllers

import (
	"encoding/json"
	"expense-tracker-web-api/app"
	"expense-tracker-web-api/models"
	u "expense-tracker-web-api/utils"
	"log"
	"net/http"
)

// SearchUsersByEmailRequest ...
type SearchUsersByEmailRequest struct {
	Email string `json:"email"`
}

// SearchUsersByEmail ...
var SearchUsersByEmail = func(w http.ResponseWriter, r *http.Request) {
	log.Print("Started: userController.SearchUsersByEmail")

	userid := app.GetUserID(r)

	req := &SearchUsersByEmailRequest{}
	err := json.NewDecoder(r.Body).Decode(req)
	if err != nil {
		u.Respond(w, u.Message(false, "Error while decoding request body"))
		return
	}

	data := models.SearchUsersByEmail(req.Email, userid)
	resp := u.Message(true, "ok")
	resp["data"] = data
	u.Respond(w, resp)

	log.Print("Finished: userController.SearchUsersByEmail")
}
