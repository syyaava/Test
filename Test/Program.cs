using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Test;

string path = "Configurations/";
List<Configuration> configurations = new List<Configuration>();

var configHelper = new ConfigurationReaderHelper(new List<IConfigReader>()
{
    new XMLConfigReader(),
    new JSONConfigReader(),
    new CSVConfigReader(),
});

//using (var fs = new FileStream(path + "configs.json", FileMode.Create, FileAccess.Write))
//{
//    JsonSerializer.Serialize(fs, new List<Configuration>()
//    {
//        new Configuration() { Name = "List config 1", Description = "List config"},
//        new Configuration() { Name = "List config 2", Description = "List config"},
//        new Configuration() { Name = "List config 3", Description = "List config"},
//        new Configuration() { Name = "List config 4", Description = "List config"},
//        new Configuration() { Name = "List config 5", Description = "List config"},
//    });
//}

if (!Directory.Exists(path))
{
    Console.WriteLine("This directory does not exists.");
    Console.ReadLine();
    return;
}

var files = Directory.GetFiles(path);

foreach(var file in files)
{
    var configs = configHelper.GetConfigFromFile(file);
    if(configs.Status == OperationResult<IEnumerable<Configuration>>.StatusCode.Error)
    {
        ErrorOutput(configs);
        continue;
    }
    configurations.AddRange(configs.Result);
    LogToConsole();
}

Console.ReadLine();

void LogToConsole()
{
    Console.WriteLine("//////////Configuration//////////");
    foreach (var config in configurations)
        Console.WriteLine(config);
    Console.WriteLine("////////End Configuration////////");
}

static void ErrorOutput(OperationResult<IEnumerable<Configuration>> config)
{
    Console.WriteLine("--------------------");
    Console.WriteLine($"Configurations reading error. {config.Exception}");
    Console.WriteLine("--------------------");
}