package models

import (
	u "expense-tracker-web-api/utils"
	"fmt"
)

// Budget ...
type Budget struct {
	BaseAuditableModel
	BudgetID   uint   `json:"budget_id" gorm:"primary_key;column:budget_id"`
	BudgetName string `json:"budget_name" gorm:"column:budget_name"`
}

// TableName ...
func (Budget) TableName() string {
	return "budget"
}

// Validate ...
func (budget *Budget) Validate() (map[string]interface{}, bool) {
	if budget.BudgetName == "" {
		return u.Message(false, "Budget name cannot be empty"), false
	}

	return u.Message(true, "success"), true
}

// Create ...
func (budget *Budget) Create(userid uint) map[string]interface{} {
	if resp, ok := budget.Validate(); !ok {
		return resp
	}

	SetAuditValuesForInsert(&budget.BaseAuditableModel, 1, userid)

	GetDB().Create(budget)

	resp := u.Message(true, "success")
	resp["budget"] = budget
	return resp
}

// GetBudget ...
func GetBudget(budgetid uint, userid uint) *Budget {
	budget := &Budget{}
	err := GetDB().Table("budget").Where("budget_id = ?", budgetid).First(budget).Error
	if err != nil {
		return nil
	}
	return budget
}

// GetBudgets ...
func GetBudgets(userid uint) []*Budget {
	budgets := make([]*Budget, 0)
	err := GetDB().Table("budget").Where("insert_user_id = ?", userid).Find(&budgets).Error
	if err != nil {
		fmt.Println(err)
		return nil
	}
	return budgets
}
