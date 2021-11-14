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
    public class SaveReportCommandTests
    {
        private string _jsonResult;

        [SetUp]
        public void SetUp()
        {
            var asmb = Assembly.GetExecutingAssembly();

            var json_result_path = $"Reports.Tests.Mock.CompiledData2.json";

            using (Stream stream = asmb.GetManifestResourceStream(json_result_path))
            using (StreamReader reader = new StreamReader(stream))
            {
                _jsonResult = reader.ReadToEnd();
            }
        }

        [Test]
        public async Task SaveReportCommand_ValidReportList_Success()
        {
            var request = new SaveReportCommand
            {
                Reports = JsonConvert.DeserializeObject<List<Report>>(_jsonResult),
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
