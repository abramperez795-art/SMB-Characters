string path = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config");

var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
logger.Info("Program started");

string file = "SMB.csv";

if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
    Console.WriteLine("Data file does not exist!");
}
else
{
    List<UInt64> Ids = new();
    List<string> Names = new();
    List<string> Descriptions = new();
    List<string> Species = new();
    List<string> FirstAppearances = new();
    List<int> YearsCreated = new();
}

  