package models

import (
	u "expense-tracker-web-api/utils"
)

// Account ...
type Account struct {
	BaseAuditableModel
	BudgetAccountID uint   `json:"budgetAccountId" gorm:"primary_key;column:budget_account_id"`
	BudgetID        uint   `json:"budgetId" gorm:"column:budget_id"`
	AccountName     string `json:"accountName" gorm:"column:account_name"`
	AccountTypeID   *uint  `json:"accountTypeId" gorm:"column:account_type_id"`
}

// TableName ...
func (Account) TableName() string {
	return "budget_account"
}

// Create ...
func (account *Account) Create(userid uint) map[string]interface{} {
	if DoesBudgetBelongToUser(account.BudgetID, userid) == false {
		return nil
	}

	SetAuditValuesForInsert(&account.BaseAuditableModel, 1, userid)

	GetDB().Create(account)

	resp := u.Message(true, "success")
	resp["account"] = account
	return resp
}

// Update ...
// func (account *Account) Update(userid uint) map[string]interface{} {
// 	if DoesBudgetBelongToUser(account.BudgetID, userid) == false {
// 		return nil
// 	}

// 	SetAuditValuesForUpdate(&account.BaseAuditableModel, 1, userid)

// 	GetDB().Model(&account).Update(account)

// 	resp := u.Message(true, "success")
// 	resp["account"] = account
// 	return resp
// }
