package tests

import (
	"expense-tracker-web-api/controllers"
	"fmt"
	"testing"
)

func TestMe(t *testing.T) {
	fmt.Println("Started Test: TestMe")
	controllers.GetAccountsOfBudget(nil, nil)
}
