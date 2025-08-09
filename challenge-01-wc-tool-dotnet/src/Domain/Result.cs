using challenge_01_wc_tool_dotnet.Domain.Enums;

namespace challenge_01_wc_tool_dotnet.Domain;

public class Result
{
    public string Filename { get; set; }
    public Dictionary<Option, long> Values { get; set; }
}