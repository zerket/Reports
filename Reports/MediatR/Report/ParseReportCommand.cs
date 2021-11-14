using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reports.Domain.Models;
using Reports.Infrastructure;
using Reports.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.MediatR
{
    public class ParseReportCommand : IRequest<List<Report>>
    {
        public string Data { get; set; }
    }

    public class ParseReportCommandHandler : IRequestHandler<ParseReportCommand, List<Report>>
    {
        public ParseReportCommandHandler() { }

        public async Task<List<Report>> Handle(ParseReportCommand request, CancellationToken cancellationToken)
        {
            var types = Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(x => x.IsClass && x.Namespace == "Reports.Dto");

            var result = new List<Report>();

            foreach (var type in types)
            {
                object[] arguments = new object[] { request.Data, null };

                var methodInfo = typeof(ParserService).GetMethod(nameof(ParserService.Process));
                methodInfo = methodInfo.MakeGenericMethod(type);

                if ((bool)methodInfo.Invoke(null, arguments))
                {
                    result = ((IReport)arguments[1]).GetReports();
                    return result;
                }
            }

            return result;
        }
    }
}
