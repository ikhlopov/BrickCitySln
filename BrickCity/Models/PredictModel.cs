using BrickCity.Models.EFEntity;

namespace BrickCity.Models
{
    public class PredictModel
    {
        public PredictModel(FileModel selectedFile, DateTime PredictDate)
        {
           _predictDate = PredictDate;
            predictedWeather = WeatherPredict(selectedFile.Consumers.First());
            SelectedFile = selectedFile;
        }
        public DateTime _predictDate { get; }
        private List<Consumer> _plants = new List<Consumer>();
        private List<Consumer> _houses = new List<Consumer>();
        public List<Consumer> Plants { get 
            {
                if(_plants.Count ==0) _plants = SelectedFile.Consumers.Where(c => c.TypeOfConsumer == Consumer_Type.Plant).ToList(); 
                return _plants;
            } 
        }
        public List<Consumer> Houses
        {
            get
            {
                if (_houses.Count == 0) _houses = SelectedFile.Consumers.Where(c => c.TypeOfConsumer == Consumer_Type.House).ToList();
                return _houses;
            }
        }
        private float predictedWeather;

        public FileModel SelectedFile { get; set; }


        // Предсказание Msrmnt самое спорное, выдает предсказанное значение как среднее предыдущих 7 значений.
        private float WeatherPredict(Consumer consumer)
        {
            IEnumerable<Consumption> activeRecords = consumer.Consumptions.Where(consump => consump.DateIsInRange(_predictDate.AddDays(-7), _predictDate));
            var activeWeathers = activeRecords.Select(c => c.Msrmnt.Weather).ToList().Select(w => w.Value);
            float sumOfActiveWeathers = 0;
            foreach (var w in activeWeathers) sumOfActiveWeathers += w;
            return sumOfActiveWeathers/activeWeathers.Count();
        }

        private float PricePredict(Consumer consumer)
        {
            IEnumerable<Consumption> activeRecords = consumer.Consumptions.Where(consump => consump.DateIsInRange(_predictDate.AddDays(-7), _predictDate));
            var activePrices = activeRecords.Select(c => c.Msrmnt.Price).ToList().Select(w => w.Value);
            float sumOfActivePrices = 0;
            foreach (var w in activePrices) sumOfActivePrices += w;
            return sumOfActivePrices / activePrices.Count();
        }

        public float plant_predict(Consumer plant)
        {
            var LR = new LinReg(plant.GetTrendPoints(), plant.Name);
            return LR.predict(PricePredict(plant));
        }

        public float house_predict(Consumer house)
        {
            var LR = new LinReg(house.GetTrendPoints(), house.Name);
            return LR.predict(predictedWeather);
        }
    }
}
