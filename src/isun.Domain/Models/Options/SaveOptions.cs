namespace isun.Domain.Models.Options;

public class SaveOptions
{
    private string _directory = "data";

    public string Directory
    {
        get => _directory;
        set => _directory = value.Replace("/", @"\");
    }
}
