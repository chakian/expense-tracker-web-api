package models

import (
	"fmt"

	u "github.com/chakian/expense-tracker-web-api/utils"
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

// GetCategoryListByBudgetID ...
func GetCategoryListByBudgetID(budgetid uint, userid uint) []*Category {
	if DoesBudgetBelongToUser(budgetid, userid) == false {
		return nil
	}

	categories := make([]*Category, 0)

	//SELECT * FROM budget INNER JOIN budget_user ON budget.budget_id = budget_user.budget_id WHERE budget_user.user_id = 1
	err := GetDB().Table("budget_category").Where("active_flag = ? AND budget_id = ? ", 1, budgetid).Find(&categories).Error
	if err != nil {
		fmt.Println(err)
		return nil
	}

	return categories
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

// Update ...
func (category *Category) Update(userid uint) map[string]interface{} {
	if DoesBudgetBelongToUser(category.BudgetID, userid) == false {
		return nil
	}

	SetAuditValuesForUpdate(&category.BaseAuditableModel, 1, userid)

	GetDB().Model(&category).Update(category)

	resp := u.Message(true, "success")
	resp["category"] = category
	return resp
}
