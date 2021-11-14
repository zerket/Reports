using Newtonsoft.Json;
using NUnit.Framework;
using Reports.Domain.Models;
using Reports.MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.Tests
{
    [TestFixture]
    public class ParseReportCommandTests
    {
        private string _json1;
        private string _json2;
        private string _jsonResult1;
        private string _jsonResult2;

        [SetUp]
        public void SetUp()
        {
            var asmb = Assembly.GetExecutingAssembly();

            var json_path_1 = $"Reports.Tests.Mock.TrackerDataFoo1.json";
            var json_path_2 = $"Reports.Tests.Mock.TrackerDataFoo2.json";
            var json_result_path_1 = $"Reports.Tests.Mock.CompiledData1.json";
            var json_result_path_2 = $"Reports.Tests.Mock.CompiledData2.json";

            using (Stream stream = asmb.GetManifestResourceStream(json_path_1))
            using (StreamReader reader = new StreamReader(stream))
            {
                _json1 = reader.ReadToEnd();
            }

            using (Stream stream = asmb.GetManifestResourceStream(json_path_2))
            using (StreamReader reader = new StreamReader(stream))
            {
                _json2 = reader.ReadToEnd();
            }

            using (Stream stream = asmb.GetManifestResourceStream(json_result_path_1))
            using (StreamReader reader = new StreamReader(stream))
            {
                _jsonResult1 = reader.ReadToEnd();
            }

            using (Stream stream = asmb.GetManifestResourceStream(json_result_path_2))
            using (StreamReader reader = new StreamReader(stream))
            {
                _jsonResult2 = reader.ReadToEnd();
            }
        }

        [Test]
        public async Task ParseReportCommand_InvalidModel_ThrowException()
        {
            // Arrange
            var request = new ParseReportCommand
            {
                Data = { }
            };

            var sut = new ParseReportCommandHandler();

            //// Act
            try
            {
                var actual = await sut.Handle(request, CancellationToken.None);
                Assert.Fail("An exception should have been thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'value')", ex.InnerException.Message);
            }
        }

        [Test]
        public async Task ParseReportCommand_StandartModel_ReturnValue()
        {
            // Arrange
            var request = new ParseReportCommand
            {
                Data = _json1
            };

            var sut = new ParseReportCommandHandler();

            //// Act
            var actual = await sut.Handle(request, CancellationToken.None);
            Assert.IsTrue(actual.Any());
        }

        [Test]
        public async Task ParseReportCommand_StandartModel_ValidParseFirstTracker()
        {
            var request = new ParseReportCommand
            {
                Data = _json1
            };

            var sut = new ParseReportCommandHandler();

            var act1 = await sut.Handle(request, CancellationToken.None);
            var act2 = new List<Report> { JsonConvert.DeserializeObject<Report>(_jsonResult1) };

            Assert.AreEqual(act1.Count, act2.Count);
            Assert.AreEqual(act1[0].CompanyName, act2[0].CompanyName);
            Assert.AreEqual(act1[0].TrackerName, act2[0].TrackerName);
            Assert.AreEqual(act1[0].AvgTemp, act2[0].AvgTemp);
        }

        [Test]
        public async Task ParseReportCommand_StandartModel_ValidParseSecondTracker()
        {
            var request = new ParseReportCommand
            {
                Data = _json2
            };

            var sut = new ParseReportCommandHandler();

            var act1 = await sut.Handle(request, CancellationToken.None);
            var act2 = JsonConvert.DeserializeObject<List<Report>>(_jsonResult2);

            Assert.IsTrue(act1.Count == 2);
            Assert.AreEqual(act1.Count, act2.Count);
            Assert.AreEqual(act1[0].CompanyName, act2[0].CompanyName);
            Assert.AreEqual(act1[1].TrackerName, act2[1].TrackerName);
            Assert.AreEqual(act1[1].AvgTemp, act2[1].AvgTemp);
        }

        [Test]
        public async Task SaveReportCommand_ValidReportList_Success()
        {
            var request = new SaveReportCommand
            {
                Reports = JsonConvert.DeserializeObject<List<Report>>(_jsonResult2),
                ReportId = Guid.NewGuid()
            };

            var sut = new SaveReportCommandHandler();

            var actual = await sut.Handle(request, CancellationToken.None);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task SaveReportCommand_EmptyReportList_Fail()
        {
            var request = new SaveReportCommand
            {
                Reports = new List<Report>(),
                ReportId = Guid.NewGuid()
            };

            var sut = new SaveReportCommandHandler();

            try
            {
                var actual = await sut.Handle(request, CancellationToken.None);
                Assert.Fail("An exception should have been thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Sequence contains no elements", ex.Message);
            }
        }
    }
}
