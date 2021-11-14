using MediatR;
using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.MediatR
{
    public class SaveReportCommand : IRequest<bool>
    {
        public List<Report> Reports { get; set; }
        public Guid ReportId { get; set; }
    }

    public class SaveReportCommandHandler : IRequestHandler<SaveReportCommand, bool>
    {
        public SaveReportCommandHandler() { }

        public async Task<bool> Handle(SaveReportCommand request, CancellationToken cancellationToken)
        {
            var fileName = $"{request.ReportId}_{request.Reports.First().CompanyName}_{DateTime.Now:yyyyMMddHHmmss}.json";
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string json = JsonSerializer.Serialize(request.Reports);

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException($"Could not serializq report");

            try
            {
                File.WriteAllText($"{filePath}\\{fileName}", json);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
