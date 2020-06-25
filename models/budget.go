package models

import (
	"fmt"

	u "github.com/chakian/expense-tracker-web-api/utils"
)

// Budget ...
type Budget struct {
	BaseAuditableModel
	BudgetID   uint         `json:"budgetId" gorm:"primary_key;column:budget_id"`
	BudgetName string       `json:"budgetName" gorm:"column:budget_name"`
	Users      []BudgetUser `gorm:"many2many:budget_user;association_foreignkey:budget_id;foreignkey:budget_id"`
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

	budgetUser := &BudgetUser{UserID: userid, BudgetID: budget.BudgetID, UserApprovedFlag: 1}
	if budgetUser.Create(userid) == false {
		resp := u.Message(false, "budget User creation failed")
		return resp
	}

	resp := u.Message(true, "success")
	resp["budget"] = budget
	return resp
}

// GetBudget ...
func GetBudget(budgetid uint, userid uint) *Budget {
	budget := &Budget{}
	err := GetDB().Table("budget").Where("budget_id = ? AND active_flag = ?", budgetid, 1).First(budget).Error
	if err != nil || DoesBudgetBelongToUser(budget.BudgetID, userid) == false {
		return nil
	}
	return budget
}

// GetBudgets ...
func GetBudgets(userid uint) []*Budget {
	budgets := make([]*Budget, 0)

	//SELECT * FROM budget INNER JOIN budget_user ON budget.budget_id = budget_user.budget_id WHERE budget_user.user_id = 1
	err := GetDB().Table("budget").Joins("JOIN budget_user ON budget.budget_id = budget_user.budget_id").Where("budget_user.user_id = ? AND budget.active_flag = ? AND budget_user.active_flag = ? AND budget_user.user_approved_flag = ?", userid, 1, 1, 1).Find(&budgets).Error
	if err != nil {
		fmt.Println(err)
		return nil
	}

	return budgets
}

// Update ...
func (budget *Budget) Update(userid uint) map[string]interface{} {
	if resp, ok := budget.Validate(); !ok {
		return resp
	}

	if DoesBudgetBelongToUser(budget.BudgetID, userid) == false {
		return nil
	}

	SetAuditValuesForUpdate(&budget.BaseAuditableModel, 1, userid)

	GetDB().Model(&budget).Update(budget)

	resp := u.Message(true, "success")
	resp["budget"] = budget
	return resp
}
