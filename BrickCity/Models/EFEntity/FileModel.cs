using BrickCity.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickCity.Models.EFEntity
{
    public class FileModel
    {

        public int FileModelID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public virtual List<Consumer> Consumers { get; set; }


        //---------------------------------   Свойства для расчета параметров файла   ---------------------------------//

        [NotMapped]
        public DateTime MinRecordDate { get { return Consumers.Min(c => c.Consumptions.MinBy(consumption => consumption.Date).Date); } }

        [NotMapped]
        public DateTime MaxRecordDate { get { return Consumers.Max(c => c.Consumptions.MaxBy(consumption => consumption.Date).Date); } }

        [NotMapped]
        public int CountConsumers { get { return Consumers.Count; } }

        [NotMapped]
        public int CountConsumptions
        {
            get
            {
                int count = 0;
                foreach (var consumer in Consumers) count += consumer.Consumptions.Count;
                return count;
            }
        }

        [NotMapped]
        public virtual string Content { get; set; }

    }
}
