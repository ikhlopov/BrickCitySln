// See https://aka.ms/new-console-template for more information
using BrickCitySandbox;

StreamReader sr = new StreamReader("C:\\Users\\ikhWORK\\Nextcloud\\Work\\T+\\Brick City\\data2019.json");
var data = City.GetCityFromJSON(sr.ReadToEnd());
Console.WriteLine("Hello, World!");
