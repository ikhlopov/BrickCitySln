using BrickCity.Models.EFEntity;
using Highsoft.Web.Mvc.Charts;

namespace BrickCity.Models
{
    public class TrendGraph
    {
        public FileModel SelectedFile { get; set; }
        private IEnumerable<Consumer> plants
        {
            get
            {
                if (SelectedFile == null) return Enumerable.Empty<Consumer>();
                return SelectedFile.Consumers.Where(consumer => consumer.TypeOfConsumer is Consumer_Type.Plant);
            }
        }

        private IEnumerable<Consumer> houses
        {
            get
            {
                if (SelectedFile == null) return Enumerable.Empty<Consumer>();
                return SelectedFile.Consumers.Where(consumer => consumer.TypeOfConsumer is Consumer_Type.House);
            }
        }

        public Highcharts GetLineTrendGraph(Consumer_Type type)
        {
            IEnumerable<Consumer> consumers = Enumerable.Empty<Consumer>();
            string plotTitle = "", XaxText = "", ID = "";

            switch (type) 
            {
                case Consumer_Type.House:
                    plotTitle = "Зависимость потребления жилых домов от температуры";
                    XaxText = "Температура, град.";
                    consumers = houses;
                    ID = "housesTrend";
                    break;

                case Consumer_Type.Plant:
                    plotTitle = "Зависимость потребления заводов от цены на кирпич";
                    XaxText = "Цена кирпича";
                    consumers = plants;
                    ID = "plantsTrend";
                    break;
            }

            Highcharts resultModel = new Highcharts();
            resultModel.Title = new Title { Text = plotTitle };
            resultModel.Chart = new Chart
            {
                Type = ChartType.Scatter,
                ZoomType = ChartZoomType.Xy,
                Panning = new ChartPanning { Enabled = true },
                PanKey = ChartPanKey.Shift

            };
            resultModel.XAxis = new List<XAxis> { new XAxis { Title = new XAxisTitle { Text = XaxText } } };
            resultModel.YAxis = new List<YAxis> { new YAxis { Title = new YAxisTitle { Text = "Потребление, кВт" } } };

            resultModel.Series = GetSeriesList(consumers);
            resultModel.ID = ID;
            return resultModel;
        }

        private List<Series> GetSeriesList(IEnumerable<Consumer> consumers)
        {
            List<Series> series = new List<Series>();
            foreach (var consumer in consumers)
            {
                List<Point> points = consumer.GetTrendPoints();
                var Records = new ScatterSeries();
                Records.Data = new List<ScatterSeriesData>();
                foreach (var point in points) 
                    Records.Data.Add(new ScatterSeriesData { X = point.Msrmnt, Y = point.Consumption });
                Records.Name = consumer.Name;
                series.Add(Records);
            }
            return series;
        }


        public struct Point
        {
            public float Msrmnt; public float Consumption;
        }
    }
}
