namespace ExpenseTracker.Common.Constants
{
    public partial class ErrorCodes
    {
        public const string BUDGET_EXISTS_WITH_SAME_NAME = "ET-002-001";
        public const string BUDGET_EXISTS_ON_ANOTHER_USER_WITH_SAME_NAME = "ET-002-002";
        public const string BUDGET_DOESNT_BELONG_TO_MODIFYING_USER = "ET-002-003";
        public const string BUDGET_DOESNT_BELONG_TO_USER = "ET-002-004";
        public const string BUDGET_USER_IS_NOT_AUTHORIZED_TO_MODIFY_BUDGET = "ET-002-006";
        public const string BUDGET_USER_IS_NOT_AUTHORIZED_TO_DELETE_BUDGET = "ET-002-007";
        public const string BUDGET_USER_IS_NOT_AUTHORIZED_TO_WRITE = "ET-002-008";
    }
}
