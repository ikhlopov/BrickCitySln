using Highsoft.Web.Mvc.Charts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BrickCity.Models.EFEntity
{
    public enum Consumer_Type
    {
        None,
        House,
        Plant
    }
    public class Consumer
    {
        public int ConsumerID { get; set; }
        public string Name { get; set; }
        public int FileModelID { get; set; }
        public Consumer_Type TypeOfConsumer { get; set; }

        public virtual List<Consumption> Consumptions { get; set; }
        public virtual List<Price> Prices { get; set; }
        public virtual FileModel File { get; set; }


        //---------------------------------   Функции взаимодействия с сущностью потребителя   ---------------------------------//


        // Возвращает список точек типа "Consumption-Msrmnt", используется для построения графика и расчета регресии для прогноза
        public List<TrendGraph.Point> GetTrendPoints()
        {
            List<TrendGraph.Point> result = new List<TrendGraph.Point>();
            foreach (var consumption in Consumptions)
            {
                var pointToAdd = new TrendGraph.Point();
                pointToAdd.Consumption = consumption.Value;
                var msrmnt = consumption.Msrmnt;
                switch (msrmnt.Msrmnt_Type)
                {
                    case Msrmnt_Type.Price:
                        pointToAdd.Msrmnt = msrmnt.Price.Value;
                        break;
                    case Msrmnt_Type.Weather:
                        pointToAdd.Msrmnt = msrmnt.Weather.Value;
                        break;
                }
                result.Add(pointToAdd);
            }
            return result;
        }

        // Создает серию точек для график потребленияя на основе всех вложенных сущностей потребления входящих в диапазон
        public AreaSeries GetAreaSeries(DateTime fromDate, DateTime toDate)
        {
            var result = new AreaSeries();
            result.Name = Name;
            result.Data = new List<AreaSeriesData>();
            var consumptions = Consumptions.Where(c => c.DateIsInRange(fromDate, toDate));
            foreach (var consumption in consumptions)
            {
                result.Data.Add(consumption.getAreaSeriesPoint());
            }
            return result;
        }

    }
}
