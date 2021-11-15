using FluentValidation.TestHelper;
using NUnit.Framework;
using Reports.Domain.Models;
using Reports.Infrastructure.Validators;
using System.Threading.Tasks;

namespace Reports.Tests
{
    [TestFixture]
    public class ReportModelsValidationTests
    {
        [Test]
        public void Validate_ModelEmpty_Throws()
        {
            // Arrange
            var model = new Report();
            var sut = new ReportModelValidator();

            // Act
            var actual = sut.TestValidate(model);

            actual.ShouldHaveValidationErrorFor(x => x.CompanyId);
            actual.ShouldHaveValidationErrorFor(x => x.CompanyName);
            actual.ShouldHaveValidationErrorFor(x => x.TrackerName);
        }

        [Test]
        public void Validate_InvalidHumidity_Throws()
        {
            // Arrange
            var model = new Report()
            {
                AvgHumdity = 0,
                CompanyId = 1,
                CompanyName = "New",
                TrackerName =  "Tracker"
            };
            var sut = new ReportModelValidator();

            // Act
            var actual = sut.TestValidate(model);

            actual.ShouldNotHaveValidationErrorFor(x => x.CompanyId);
            actual.ShouldNotHaveValidationErrorFor(x => x.CompanyName);
            actual.ShouldNotHaveValidationErrorFor(x => x.TrackerName);
            actual.ShouldHaveValidationErrorFor(x => x.AvgHumdity);
        }
    }
}
