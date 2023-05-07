using FluentValidation;
using Hackathon.Application.DTOs;
using Hackathon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Application.Validations;

public class CreditApplicationValidation : AbstractValidator<CreditDto>
{
    public CreditApplicationValidation()
    {
        RuleFor(p => p.Age)
            .NotNull()
            .Must(age => age >= 18 && age <= 150);

        RuleFor(p => p.HomeOwnership)
            .NotNull()
            .Must(ownership => ownership.ToUpper() == "RENT" || ownership.ToUpper() == "MORTGAGE");

        RuleFor(p => p.LoanPurposes)
            .NotNull()
            .Must(ownership => ownership.ToUpper() == "EDUCATION" || ownership.ToUpper() == "MEDICAL" ||ownership.ToUpper() == "HOMEIMPROVEMENT");

        RuleFor(p => p.EmploymentTime)
            .NotNull();
        
        RuleFor(p => p.Salary)
            .NotNull()
            .Must(salary => salary > 0);

        RuleFor(p => p.CreditAmount)
            .NotNull();
    }
}
