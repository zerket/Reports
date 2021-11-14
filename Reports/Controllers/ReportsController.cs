using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Reports.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IMediator _mediator;

        public ReportsController(ILogger<ReportsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string request)
        {
            var uniqReportId = Guid.NewGuid();
            _logger.LogInformation($"Received report at {DateTime.Now:s}. ReportId: {uniqReportId}");

            var parsedReports = await _mediator.Send(new MediatR.ParseReportCommand
            {
                Data = request
            });

            _logger.LogInformation($"Parse report OK. ReportId: {uniqReportId}");

            try
            {
                var result = await _mediator.Send(new MediatR.SaveReportCommand
                {
                    Reports = parsedReports,
                    ReportId = uniqReportId
                });

                _logger.LogInformation($"Save report OK. ReportId: {uniqReportId}");
                return Ok();
            }
            catch (Exception)
            {
                _logger.LogInformation($"Save report FAILED. ReportId: {uniqReportId}");
                return BadRequest();
            }
        }
    }
}
