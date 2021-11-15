using FluentValidation;
using Reports.Domain.Models;
using System;

namespace Reports.Infrastructure.Validators
{
    public class ReportModelValidator : AbstractValidator<Report>
    {
        public ReportModelValidator()
        {
            RuleFor(model => model.CompanyId).NotEmpty().GreaterThan(0);
            RuleFor(model => model.CompanyName).NotEmpty();

            RuleFor(model => model.TrackerName).NotEmpty();

            RuleFor(model => model.LastCrumbDtm).LessThan(DateTime.Now);
            RuleFor(model => model.FirstCrumbDtm).LessThan(DateTime.Now);

            RuleFor(model => model.AvgHumdity).GreaterThan(0).LessThanOrEqualTo(100);
            RuleFor(model => model.AvgTemp).LessThan(1000);
        }
    }
}
