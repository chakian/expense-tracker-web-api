package models

import (
	u "github.com/chakian/expense-tracker-web-api/utils"
)

// BudgetUser ...
type BudgetUser struct {
	BaseAuditableModel
	BudgetUserID     uint  `json:"budgetUserId" gorm:"primary_key;column:budget_user_id"`
	BudgetID         uint  `json:"budgetId"`
	UserID           uint  `json:"userId"`
	UserApprovedFlag uint8 `json:"userApproved"`
}

// TableName ...
func (BudgetUser) TableName() string {
	return "budget_user"
}

// Create ...
func (budgetUser *BudgetUser) Create(userid uint) bool {
	SetAuditValuesForInsert(&budgetUser.BaseAuditableModel, 1, userid)

	GetDB().Create(budgetUser)

	return true
}

//DoesBudgetBelongToUser ...
func DoesBudgetBelongToUser(budgetid uint, userid uint) bool {
	budgetUser := &BudgetUser{}
	err := GetDB().Where(&BudgetUser{BudgetID: budgetid, UserID: userid}).Where("active_flag = ? AND budget_user.user_approved_flag = ?", 1, 1).First(budgetUser).Error
	if err != nil || budgetUser.BudgetUserID <= 0 {
		return false
	}
	return true
}

// AddUserToBudget ...
func (budgetUser *BudgetUser) AddUserToBudget(auditUserID uint) map[string]interface{} {
	SetAuditValuesForInsert(&budgetUser.BaseAuditableModel, 1, auditUserID)

	GetDB().Create(budgetUser)

	resp := u.Message(true, "success")
	resp["budgetUser"] = budgetUser
	return resp
}

// Update ...
func (budgetUser *BudgetUser) Update(userid uint) map[string]interface{} {
	SetAuditValuesForUpdate(&budgetUser.BaseAuditableModel, 1, userid)

	GetDB().Model(&budgetUser).Update(budgetUser)

	resp := u.Message(true, "success")
	resp["budgetUser"] = budgetUser
	return resp
}
