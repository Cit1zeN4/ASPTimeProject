using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft.Json;

namespace ASPTime.Models
{
    public class Chart
    {
        public Chart()
        {
            Data = new ChartData();
        }

        [JsonProperty(PropertyName = "type")]
        public string ChartType { get; set; }

        [JsonProperty(PropertyName = "data")]
        public ChartData Data { get; set; }
    }

    public class ChartData
    {
        public ChartData()
        {
            Labels = new List<string>();
            DataSets = new List<ChartDataSets>();
        }

        [JsonProperty(PropertyName = "labels")]
        public List<string> Labels { get; set; }

        [JsonProperty(PropertyName = "datasets")]
        public List<ChartDataSets> DataSets { get; set; }
    }

    public class ChartDataSets
    {
        public ChartDataSets()
        {
            Data = new List<double>();
            BackgroundColor = new List<string>();
        }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<double> Data { get; set; }

        [JsonProperty(PropertyName = "backgroundColor")]
        public List<string> BackgroundColor { get; set; }
    }

    public class ChartPieValue
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public string BackgroundColor { get; set; }
    }

    public class ChartDataAdapter
    {
        public string[] BackgroundColor = new string[]
        {
            "#F0A0B0",
            "#284068",
            "#88B8B0",
            "#A8E8E0",
        };

        public Chart GetChart(List<ChartPieValue> values, string chartType = "pie")
        {
            Chart chart = new Chart
            {
                ChartType = chartType
            };

            chart.Data.Labels = new List<string>(values.Select(o => o.Label).ToArray());
            chart.Data.DataSets.Add(new ChartDataSets
            {
                Data = new List<double>(values.Select(o => o.Value).ToArray()),
                BackgroundColor = new List<string>(values.Select(o => o.BackgroundColor).ToArray())
            });
            return chart;
        }
    }
}