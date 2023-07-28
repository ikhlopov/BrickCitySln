namespace BrickCity.Models
{
    public class LinReg
    {
        // Принимает список точек "Consumption-Msrmnt", рассчитывает коэффициенты парной регрессии, выдает спрогнозированное потребление как Consumption = predict(Msrmnt)

        public LinReg(List<TrendGraph.Point> points, string Name)
        {
            _points = points;
            this.Name = Name;
            foreach (var point in points)
            {
                msrmntSum += point.Msrmnt;
                msrmntSqSum += point.Msrmnt * point.Msrmnt;
                msrmntConsumptionSum += point.Msrmnt * point.Consumption;
                consumptionSum += point.Consumption;
            }
        }

        public string Name { get; set; } = "";
        private List<TrendGraph.Point> _points { get; set; } = new List<TrendGraph.Point> { };
        private float msrmntSum = 0;
        private float msrmntSqSum = 0;
        private float msrmntConsumptionSum = 0;
        private float consumptionSum = 0;
        private float a
        {
            get
            {
                float res =
                    ((_points.Count * msrmntConsumptionSum) - (msrmntSum * consumptionSum))
                    / ((_points.Count * msrmntSqSum) - (msrmntSum * msrmntSum));

                return res;
            }
        }

        private float b
        {
            get 
            {
                float res =
                    (consumptionSum - a * msrmntSum) / _points.Count;
                return res;
            }
        }

        public float predict(float Msrmnt)
        {
            return Msrmnt * a + b;
        }
    }
}
