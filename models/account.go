package models

import (
	u "expense-tracker-web-api/utils"
	"fmt"
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

// GetAccountListByBudgetID ...
func GetAccountListByBudgetID(budgetid uint, userid uint) []*Account {
	if DoesBudgetBelongToUser(budgetid, userid) == false {
		return nil
	}

	accounts := make([]*Account, 0)

	err := GetDB().Table("budget_account").Where("active_flag = ? AND budget_id = ? ", 1, budgetid).Find(&accounts).Error
	if err != nil {
		fmt.Println(err)
		return nil
	}

	return accounts
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
func (account *Account) Update(userid uint) map[string]interface{} {
	if DoesBudgetBelongToUser(account.BudgetID, userid) == false {
		return nil
	}

	SetAuditValuesForUpdate(&account.BaseAuditableModel, 1, userid)

	GetDB().Model(&account).Update(account)

	resp := u.Message(true, "success")
	resp["account"] = account
	return resp
}
