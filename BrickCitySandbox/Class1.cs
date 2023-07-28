using System.Text.Json;

namespace BrickCitySandbox
{
    public class City
    {
        public List<Consumer>? houses { get; set; }
        public List<Consumer>? plants { get; set; }
        public static City GetCityFromJSON(string jsonString)
        {

            return JsonSerializer.Deserialize<City>(jsonString);
        }
        public IEnumerable<Consumer> all { get { return Enumerable.Concat(houses,plants).ToList(); } }
    }

    public class Consumer
    {
        public int ConsumerId { get; set; }
        public string Name { get; set; }
        public List<UltConsumption> consumptions { get; set; }
    }
    public class UltConsumption
    {
        public DateTime Date { get; set; }
        public float? Weather { get; set; } = null;
        public float? Price { get; set; } = null;
        public float Consumption { get; set; }
    }

}