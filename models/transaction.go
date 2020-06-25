package models

import (
	u "expense-tracker-web-api/utils"
	"time"
)

// TransactionHeader ...
type TransactionHeader struct {
	BaseAuditableModel
	TransactionHeaderID uint      `json:"transactionHeaderId" gorm:"primary_key;column:transaction_header_id"`
	BudgetID            uint      `json:"budgetId" gorm:"column:budget_id"`
	SourceAccountID     uint      `json:"sourceAccountId" gorm:"column:source_account_id"`
	TargetAccountID     *uint     `json:"targetAccountId" gorm:"column:target_account_id"`
	TransactionDate     time.Time `json:"transactionDate" gorm:"column:transaction_date"`
	CategoryID          *uint     `json:"categoryId" gorm:"column:category_id"`
	MultipleItemFlag    bool      `json:"multipleItemFlag" gorm:"column:multiple_item_flag"`
	Amount              uint      `json:"amount" gorm:"column:amount"`
	Description         *string   `json:"description" gorm:"column:description"`
}

// TableName ...
func (TransactionHeader) TableName() string {
	return "transaction_header"
}

// Create ...
func (transactionHeader *TransactionHeader) Create(userid uint) map[string]interface{} {
	if DoesBudgetBelongToUser(transactionHeader.BudgetID, userid) == false {
		return nil
	}

	SetAuditValuesForInsert(&transactionHeader.BaseAuditableModel, 1, userid)

	GetDB().Create(transactionHeader)

	resp := u.Message(true, "success")
	resp["transactionHeader"] = transactionHeader
	return resp
}
