using Reports.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reports.Domain.Models
{
    public class Report
	{
		[Required(ErrorMessage = "CompanyId is required")]
		public int CompanyId { get; set; }

		[Required(ErrorMessage = "CompanyName is required")]
		public string CompanyName { get; set; }

		public int? TrackerId { get; set; }

		[Required(ErrorMessage = "Tracker name is required")]
		public string TrackerName { get; set; }

		[DataType(DataType.DateTime)]
		[UntilNow]
		public System.DateTime? FirstCrumbDtm { get; set; }

		[DataType(DataType.DateTime)]
		[UntilNow]
		public System.DateTime? LastCrumbDtm { get; set; }

		public int? TempCount { get; set; }

		[Range(-100, 1000, ErrorMessage = "Average temperature must be between -100 and 1000")]
		public double? AvgTemp { get; set; }

		public int? HumidityCount { get; set; }

		[Range(1, 100, ErrorMessage = "Average humidty must be between 1 and 100")]
		public double? AvgHumdity { get; set; }
    }
}
