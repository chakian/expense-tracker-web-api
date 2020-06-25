package controllers

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/chakian/expense-tracker-web-api/app"
	"github.com/chakian/expense-tracker-web-api/models"
	u "github.com/chakian/expense-tracker-web-api/utils"
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
