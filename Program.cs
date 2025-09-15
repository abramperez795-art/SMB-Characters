using nLog;

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
try
{
    using StreamReader sr = new(file);
    sr.ReadLine(); // Skip header

    string? line;
    while ((line = sr.ReadLine()) != null)
    {
        if (!string.IsNullOrEmpty(line))
        {
            string[] details = line.Split(',').Select(d => d.Trim()).ToArray();

            if (details.Length == 6)
            {
                if (UInt64.TryParse(details[0], out var id) && int.TryParse(details[5], out var year))
                {
                    // Add values to each list
                    Ids.Add(id);
                    Names.Add(details[1]);
                    Descriptions.Add(details[2]);
                    Species.Add(details[3]);
                    FirstAppearances.Add(details[4]);
                    YearsCreated.Add(year);
                }
                else
                {
                    logger.Error("Could not parse Id or Year: {line}", line);
                }
            }
            else
            {
                logger.Error("Malformed line: {line}", line);
            }
        }
    }
}
catch (Exception ex)
{
    logger.Error(ex, "Error reading mario.csv");
}

  string? choice;
do
{
    Console.WriteLine("1) Add Character");
    Console.WriteLine("2) Display All Characters");
    Console.WriteLine("Enter to quit");
    choice = Console.ReadLine();
    logger.Info("User choice: {Choice}", choice);
