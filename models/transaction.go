package models

import (
	"fmt"
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
	HasMultipleItems    bool      `json:"hasMultipleItems" gorm:"column:multiple_item_flag"`
	Amount              float64   `json:"amount" gorm:"column:amount"`
	Description         string    `json:"description" gorm:"column:description"`
}

// TableName ...
func (TransactionHeader) TableName() string {
	return "transaction_header"
}

// GetFilteredTransactionList ...
func GetFilteredTransactionList(budgetid uint, startDate time.Time, endDate time.Time, userid uint) []*TransactionHeader {
	if DoesBudgetBelongToUser(budgetid, userid) == false {
		return nil
	}

	transactions := make([]*TransactionHeader, 0)

	// SELECT * FROM transaction_header WHERE active_flag=1 AND budget_id=14 AND (transaction_date between '2020-06-10' and '2020-06-21');
	err := GetDB().Table("transaction_header").Where("active_flag = ? AND budget_id = ? AND (transaction_date between ? and ?)", 1, budgetid, startDate, endDate).Find(&transactions).Error
	if err != nil {
		fmt.Println(err)
		return nil
	}

	return transactions
}
