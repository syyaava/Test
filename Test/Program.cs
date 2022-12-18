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

if (!Directory.Exists(path))
{
    Console.WriteLine("This directory does not exists.");
    Console.ReadLine();
    return;
}

var files = Directory.GetFiles(path);

foreach(var file in files)
{
    var config = configHelper.GetConfigFromFile(file);
    if(config.Status == OperationResult<Configuration>.StatusCode.Error)
    {
        ErrorOutput(config);
        continue;
    }
    configurations.Add(config.Result);
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

static void ErrorOutput(OperationResult<Configuration> config)
{
    Console.WriteLine("--------------------");
    Console.WriteLine($"Configuration reading error. {config.Exception}");
    Console.WriteLine("--------------------");
}