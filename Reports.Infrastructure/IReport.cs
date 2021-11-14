using System.Collections.Generic;
using Reports.Domain.Models;

namespace Reports.Infrastructure
{
    public interface IReport
    {
        List<Report> GetReports();
    }
}
