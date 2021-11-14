using Reports.Infrastructure;
using Reports.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Reports.Dto
{
    public class Foo1 : IReport
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public List<Tracker> Trackers { get; set; }

        public List<Report> GetReports()
        {
            return this.Trackers.Select(x => new Report
            {
                CompanyId = this.PartnerId,
                CompanyName = this.PartnerName,
                TrackerId = x.Id,
                TrackerName = x.Model,
                FirstCrumbDtm = x.FirstCrumbDtm,
                LastCrumbDtm = x.LastCrumbDtm,
                TempCount = x.TempCount,
                AvgTemp = x.AvgTemp,
                HumidityCount = x.HumidityCount,
                AvgHumdity = x.AvgHumdity
            }).ToList();
        }
    }

    public class Tracker : Foo1
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string ShipmentStartDtm { get; set; }
        public List<Sensor> Sensors { get; set; }

        public DateTime? FirstCrumbDtm
        {
            get
            {
                return this.Sensors.SelectMany(x => x.Crumbs)
                                   .Select(x => DateTime.Parse(x.CreatedDtm))
                                   .OrderBy(x => x)
                                   .First();
            }
        }
        public DateTime? LastCrumbDtm
        {
            get
            {
                return this.Sensors.SelectMany(x => x.Crumbs)
                                   .Select(x => DateTime.Parse(x.CreatedDtm))
                                   .OrderBy(x => x)
                                   .Last();
            }
        }
        public int? TempCount
        {
            get
            {
                return this.Sensors.Where(x => x.Name == "Temperature")
                                    .SelectMany(x => x.Crumbs)
                                    .Count();
            }
        }
        public double? AvgTemp
        {
            get
            {
                var values = this.Sensors.Where(x => x.Name == "Temperature")
                                    .SelectMany(x => x.Crumbs)
                                    .Select(x => x.Value)
                                    .ToList();
                return values.Any() ? values.Average() : null;
            }
        }
        public int? HumidityCount
        {
            get
            {
                return this.Sensors.Where(x => x.Name == "Humidty")
                                    .SelectMany(x => x.Crumbs)
                                    .Count();
            }
        }
        public double? AvgHumdity
        {
            get
            {
                var values = this.Sensors.Where(x => x.Name == "Humidty")
                                    .SelectMany(x => x.Crumbs)
                                    .Select(x => x.Value)
                                    .ToList();
                return values.Any() ? values.Average() : null;
            }
        }
    }

    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Crumb> Crumbs { get; set; }
    }

    public class Crumb
    {
        public string CreatedDtm { get; set; }
        public double Value { get; set; }
    }
}
