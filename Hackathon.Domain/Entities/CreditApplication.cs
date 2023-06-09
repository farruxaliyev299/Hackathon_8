﻿using Hackathon.Domain.Common;
using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Entities;

public class CreditApplication : BaseEntity
{
    public int Age { get; set; }
    public float Salary { get; set; }
    public string HomeOwnership { get; set; }
    public float EmploymentTime { get; set; }
    public string LoanPurposes { get; set; }
    public float CreditAmount { get; set; }
    public float? LoanRate { get; set; }
    public  int? CreditStatus { get; set; }
    public float? LoanPercentage { get; set; }
    public int? PaymentHistory { get; set; }
    public int? CreditHistoryLength { get; set; }
}
