package models

import (
	u "expense-tracker-web-api/utils"
)

// Category ...
type Category struct {
	BaseAuditableModel
	BudgetCategoryID uint   `json:"budgetCategoryId" gorm:"primary_key;column:budget_category_id"`
	BudgetID         uint   `json:"budgetId" gorm:"column:budget_id"`
	CategoryName     string `json:"categoryName" gorm:"column:category_name"`
	ParentCategoryID *uint  `json:"parentCategoryId" gorm:"column:parent_category_id"`
}

// TableName ...
func (Category) TableName() string {
	return "budget_category"
}

// Create ...
func (category *Category) Create(userid uint) map[string]interface{} {
	if DoesBudgetBelongToUser(category.BudgetID, userid) == false {
		return nil
	}

	SetAuditValuesForInsert(&category.BaseAuditableModel, 1, userid)

	GetDB().Create(category)

	resp := u.Message(true, "success")
	resp["category"] = category
	return resp
}
