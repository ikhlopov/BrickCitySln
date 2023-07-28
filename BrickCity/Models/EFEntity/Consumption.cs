using BrickCity.Data;
using Highsoft.Web.Mvc.Charts;
using System;

namespace BrickCity.Models.EFEntity
{
    public class Consumption
    {

        public int ConsumptionID { get; set; }
        public int ConsumerID { get; set; }
        public int MsrmntID { get; set; }
        public DateTime Date { get; set; }
        public float Value { get; set; }

        public virtual Msrmnt Msrmnt { get; set; }
        public virtual Consumer Consumer { get; set; }


        //---------------------------------   Функции взаимодействия с сущностью потребления   --------------------------------//


        // Преобразование еденицы потребления в точку на графике потребления
        public AreaSeriesData getAreaSeriesPoint()
        {
            var res = new AreaSeriesData();
            res.X = (Date - DateTime.UnixEpoch).TotalMilliseconds;
            res.Y = Value;

            return res;
        }


        
        public bool DateIsInRange(DateTime from, DateTime to)
        {
            return Date <= to && Date >= from;
        }
    }



}
