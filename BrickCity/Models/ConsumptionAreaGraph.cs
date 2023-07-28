using BrickCity.Data;
using BrickCity.Models.EFEntity;
using Highsoft.Web.Mvc.Charts;

namespace BrickCity.Models
{
    public class ConsumptionAreaGraph
    {
        // Класс принимает в качестве параметров границы дипазона дат и сущность файла, в результате формирует Highchart объект графика
        public FileModel SelectedFile { get; set; }
        private DateTime fromDate;
        private DateTime toDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                if (SelectedFile!= null)
                {
                    if (value > SelectedFile.MinRecordDate && value < SelectedFile.MaxRecordDate) fromDate = value;
                    else fromDate = SelectedFile.MinRecordDate;
                }
                else fromDate = DateTime.MinValue;
            }
        }
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                if (SelectedFile != null)
                {
                    if (value > SelectedFile.MinRecordDate && value < SelectedFile.MaxRecordDate) toDate = value;
                    else toDate = SelectedFile.MaxRecordDate;
                }
                else toDate = DateTime.MinValue;
            }
        }
        public string FromDateString { get { return fromDate.ToString("yyyy-MM-dd"); } }
        public string ToDateString { get { return toDate.ToString("yyyy-MM-dd"); } }
        public IEnumerable<Consumer> Consumers {get; set; } = Enumerable.Empty<Consumer>();
        public Highcharts? Graph {
            get
            {
                return GetConsumptionGraphModel();
            }
        }
        
        public ConsumptionAreaGraph() { }

        public Highcharts GetConsumptionGraphModel()
        {
            Highcharts resultModel = new Highcharts();
            resultModel.Title = new Title { Text = "Распределение потребления" };
            resultModel.Chart = new Chart
            {
                Type = ChartType.Area,
                ZoomType = ChartZoomType.X,
                Panning = new ChartPanning { Enabled = true },
                PanKey = ChartPanKey.Shift

            };
            resultModel.XAxis = new List<XAxis> { new XAxis { Type = "datetime" } };
            resultModel.YAxis = new List<YAxis> { new YAxis { Title = new YAxisTitle { Text = "кВт" } } };
            resultModel.PlotOptions = new PlotOptions
            {
                Area = new PlotOptionsArea
                {
                    Stacking = PlotOptionsAreaStacking.Normal
                }
            };
            resultModel.Series = GetSeriesForAllConsumers();
            resultModel.ID = "Consumption";
            return resultModel;
        }
        private List<Series> GetSeriesForAllConsumers()
        {
            List<Series> series = new List<Series>();

            foreach (var consumer in Consumers)
            {
                series.Add(consumer.GetAreaSeries(FromDate,ToDate));
            }
            return series;

        }
    }
}
