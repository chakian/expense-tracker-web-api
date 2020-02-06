namespace ExpenseTracker.Common.Constants
{
    public partial class ErrorCodes
    {
        public const string REGISTER_EMAIL_EXISTS = "ET-001-001";
        public const string REGISTER_EMAIL_INVALID = "ET-001-002";
        public const string REGISTER_EMAIL_EMPTY = "ET-001-003";
        public const string REGISTER_PASSWORD_EMPTY = "ET-001-004";
        public const string REGISTER_PASSWORD_NOT_EQUAL = "ET-001-005";
        public const string REGISTER_PASSWORD_NOT_SAFE = "ET-001-006";
        public const string REGISTER_NAME_EMPTY = "ET-001-007";
        public const string LOGIN_EMAIL_NOT_FOUND = "ET-001-008";
        public const string LOGIN_WRONG_PASSWORD = "ET-001-009";
        public const string TOKEN_EMPTY = "ET-001-010";
    }
}
