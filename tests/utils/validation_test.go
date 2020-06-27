package utils_test

import (
	"expense-tracker-web-api/utils"
	"testing"
)

func TestEmail(t *testing.T) {
	mail := "asdfg@tyur.coo"
	utils.ValidateEmail(mail)
}
