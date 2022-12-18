using System.Text.Json;
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
    try
    {
        var config = configHelper.GetConfigFromFile(file);
        configurations.Add(config);
        LogToConsole();
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("********************");
        Console.WriteLine(ex.Message);
        Console.WriteLine("********************");
    }
    catch (DeserializeException ex)
    {
        Console.WriteLine("--------------------");
        Console.WriteLine(ex.Message);
        Console.WriteLine("--------------------");
    }
}

Console.ReadLine();

void LogToConsole()
{
    Console.WriteLine("//////////Configuration//////////");
    foreach (var config in configurations)
        Console.WriteLine(config);
    Console.WriteLine("////////End Configuration////////");
}