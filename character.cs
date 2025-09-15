// Character.cs
public class Character
{
    public ulong Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Species { get; set; } = "";
    public string FirstAppearance { get; set; } = "";
    public int YearCreated { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}\nName: {Name}\nDescription: {Description}\nSpecies: {Species}\nFirst Appearance: {FirstAppearance}\nYear Created: {YearCreated}\n";
    }
}
