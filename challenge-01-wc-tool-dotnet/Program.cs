using challenge_01_wc_tool_dotnet.Domain;

namespace challenge_01_wc_tool_dotnet;

static class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length <= 0)
                throw new ArgumentException("No arguments passed");
            
            var wordCounter = new WordCounter(args);
            Console.WriteLine(wordCounter.ToString());
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
            Environment.Exit(1);
        }
    }
}