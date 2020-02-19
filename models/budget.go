package models

import (
	u "expense-tracker-web-api/utils"
	"fmt"
	"time"
)

// Budget ...
type Budget struct {
	BudgetID     uint       `json:"budget_id" gorm:"primary_key;column:budget_id"`
	BudgetName   string     `json:"budget_name" gorm:"column:budget_name"`
	ActiveFlag   uint       `json:"active_flag" gorm:"column:active_flag"`
	InsertUserID uint       `json:"insert_user_id" gorm:"column:insert_user_id"`
	InsertTime   time.Time  `json:"insert_time" gorm:"column:insert_time"`
	UpdateUserID *uint      `json:"update_user_id" gorm:"column:update_user_id"`
	UpdateTime   *time.Time `json:"update_time" gorm:"column:update_time"`
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
func (budget *Budget) Create() map[string]interface{} {
	if resp, ok := budget.Validate(); !ok {
		return resp
	}

	budget.ActiveFlag = 1
	budget.InsertTime = time.Now().UTC()
	budget.UpdateUserID = nil
	budget.UpdateTime = nil

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
