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
    if (choice == "1")
    {
        Console.WriteLine("Enter new character name: ");
        string? Name = Console.ReadLine();

        if (!string.IsNullOrEmpty(Name))
        {
            var LowerCaseNames = Names.ConvertAll(n => n.ToLower());
            if (LowerCaseNames.Contains(Name.ToLower()))
            {
                logger.Info("Duplicate name attempted: {Name}", Name);
                Console.WriteLine("Name already exists.");
            }
            else
            {
                UInt64 Id = Ids.Count > 0 ? Ids.Max() + 1 : 1;
                Console.WriteLine("Enter description:");
                string? Description = Console.ReadLine();
                Console.WriteLine("Enter species:");
                string? SpeciesValue = Console.ReadLine();
                Console.WriteLine("Enter first appearance:");
                string? FirstAppearance = Console.ReadLine();
                Console.WriteLine("Enter year created:");
                bool validYear = int.TryParse(Console.ReadLine(), out int YearCreated);

                if (!validYear)
                {
                    logger.Error("Invalid year entered");
                    Console.WriteLine("Invalid year. Character not added.");
                }
                else
                {
                    try
                    {
                        using StreamWriter sw = new(file, true);
                        sw.WriteLine($"{Id},{Name},{Description},{SpeciesValue},{FirstAppearance},{YearCreated}");

                        // Add to memory
                        Ids.Add(Id);
                        Names.Add(Name);
                        Descriptions.Add(Description ?? "");
                        Species.Add(SpeciesValue ?? "");
                        FirstAppearances.Add(FirstAppearance ?? "");
                        YearsCreated.Add(YearCreated);

                        logger.Info("Character added: {Id}, {Name}", Id, Name);
                        Console.WriteLine("Character added!");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Failed to write new character to file");
                    }
                }
            }
        }
        else
        {
            logger.Error("Empty name entered");
            Console.WriteLine("You must enter a name.");
        }
    }
else if (choice == "2")
{
    for (int i = 0; i < Ids.Count; i++)
    {
        Console.WriteLine($"Id: {Ids[i]}");
        Console.WriteLine($"Name: {Names[i]}");
        Console.WriteLine($"Description: {Descriptions[i]}");
        Console.WriteLine($"Species: {Species[i]}");
        Console.WriteLine($"First Appearance: {FirstAppearances[i]}");
        Console.WriteLine($"Year Created: {YearsCreated[i]}");
        Console.WriteLine();
    }
    logger.Info("Displayed all characters.");
}
