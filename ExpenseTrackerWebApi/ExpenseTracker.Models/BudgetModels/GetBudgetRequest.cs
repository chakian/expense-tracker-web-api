﻿using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class GetBudgetRequest : BaseAuthenticatedRequest
    {
        public int BudgetId { get; set; }
    }
}