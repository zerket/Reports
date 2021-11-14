using Reports.Domain.Models;
using Reports.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports.Dto
{
    public class Foo2 : IReport
    {
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public List<Device> Devices { get; set; }

        public List<Report> GetReports()
        {
            return this.Devices.Select(x => new Report
            {
                CompanyId = this.CompanyId,
                CompanyName = this.Company,
                TrackerId = x.DeviceID,
                TrackerName = x.Name,
                FirstCrumbDtm = x.FirstCrumbDtm,
                LastCrumbDtm = x.LastCrumbDtm,
                TempCount = x.TempCount,
                AvgTemp = x.AvgTemp,
                HumidityCount = x.HumidityCount,
                AvgHumdity = x.AvgHumdity
            }).ToList();
        }
    }

    public class Device : Foo2
    {
        public int DeviceID { get; set; }
        public string Name { get; set; }
        public string StartDateTime { get; set; }
        public List<SensorData> SensorData { get; set; }

        public DateTime? FirstCrumbDtm
        {
            get
            {
                return this.SensorData.Select(x => DateTime.Parse(x.DateTime))
                                      .OrderBy(x => x)
                                      .First();
            }
        }
        public DateTime? LastCrumbDtm
        {
            get
            {
                return this.SensorData.Select(x => DateTime.Parse(x.DateTime))
                                      .OrderBy(x => x)
                                      .Last();
            }
        }
        public int? TempCount
        {
            get
            {
                return this.SensorData.Where(x => x.SensorType == "TEMP")
                                      .Count();
            }
        }
        public double? AvgTemp
        {
            get
            {
                var values = this.SensorData.Where(x => x.SensorType == "TEMP")
                                            .Select(x => x.Value)
                                            .ToList();
                return values.Any() ? values.Average() : null;
            }
        }
        public int? HumidityCount
        {
            get
            {
                return this.SensorData.Where(x => x.SensorType == "HUM")
                                      .Count();
            }
        }
        public double? AvgHumdity
        {
            get
            {
                var values = this.SensorData.Where(x => x.SensorType == "HUM")
                                            .Select(x => x.Value)
                                            .ToList();
                return values.Any() ? values.Average() : null;
            }
        }
    }

    public class SensorData
    {
        public string SensorType { get; set; }
        public string DateTime { get; set; }
        public double Value { get; set; }
    }
}
