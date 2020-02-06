﻿namespace ExpenseTracker.Business.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int ValidDays { get; set; }
    }
}
