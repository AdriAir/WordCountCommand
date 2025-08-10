using System.Text;
using System.Text.RegularExpressions;
using challenge_01_wc_tool_dotnet.Domain.Enums;

namespace challenge_01_wc_tool_dotnet.Domain;

public class WordCounter
{
    private readonly List<string> Arguments;
    private readonly Dictionary<Option, bool> Options;
    private readonly List<string> Filenames;
    private readonly List<Result> FileResult;

    public WordCounter(string[] args)
    {
        Arguments = args.ToList();
        Options = GetOptions();
        Filenames = GetFilenames();

        FileResult = ProcessOptions();
    }

    private Dictionary<Option, bool> GetOptions()
    {
        var optionStates = new Dictionary<Option, bool>()
        {
            { Option.BYTES, false },
            { Option.CHARS, false },
            { Option.WORDS, false },
            { Option.LINES, false },
            { Option.MAX_LINE_LENGTH, false },
        };

        foreach (var argument in Arguments.ToList())
        {
            //Si se detecta el separador de opciones, se terminan las opciones
            if (argument.Equals("--"))
            {
                Arguments.Remove(argument);
                break;
            }

            switch (argument)
            {
                case ShortValidOption.BYTES:
                case LargeValidOptions.BYTES:

                    optionStates[Option.BYTES] = true;
                    Arguments.Remove(argument);
                    break;

                case ShortValidOption.CHARS:
                case LargeValidOptions.CHARS:

                    optionStates[Option.CHARS] = true;
                    Arguments.Remove(argument);
                    break;

                case ShortValidOption.WORDS:
                case LargeValidOptions.WORDS:

                    optionStates[Option.WORDS] = true;
                    Arguments.Remove(argument);
                    break;

                case ShortValidOption.LINES:
                case LargeValidOptions.LINES:

                    optionStates[Option.LINES] = true;
                    Arguments.Remove(argument);
                    break;

                case ShortValidOption.MAX_LINE_LENGTH:
                case LargeValidOptions.MAX_LINE_LENGTH:

                    optionStates[Option.MAX_LINE_LENGTH] = true;
                    Arguments.Remove(argument);
                    break;
            }
        }

        if (optionStates.Any(option => option.Value)) return optionStates;
        
        //Default values (no options found)
        optionStates[Option.CHARS] = true;
        optionStates[Option.WORDS] = true;
        optionStates[Option.LINES] = true;

        return optionStates;
    }

    private List<string> GetFilenames()
    {

        if (Arguments.Count <= 0)
            throw new ArgumentException($"No files given");
        
        var filenames = new List<string>();

        foreach (var filename in Arguments)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }

            filenames.Add(filename);
        }

        return filenames;
    }

    private List<Result> ProcessOptions()
    {
        var results = new List<Result>();

        foreach (var filename in Filenames)
        {
            
            Result result = new Result
            {
                Filename = filename,
                Values = new Dictionary<Option, long>()
            };
            
            Encoding encoding = GetEncoding(filename);
            
            string content = File.ReadAllText(filename, encoding);
            string[]  lines = File.ReadAllLines(filename, encoding);
            FileInfo fileInfo = new FileInfo(filename);

            if (Options[Option.BYTES])
            {
                result.Values.Add(Option.BYTES, CountBytes(fileInfo));
            }

            if (Options[Option.CHARS])
            {
                result.Values.Add(Option.CHARS, CountChars(content));
            }

            if (Options[Option.WORDS])
            {
                result.Values.Add(Option.WORDS, CountWords(content));
            }

            if (Options[Option.LINES])
            {
                result.Values.Add(Option.LINES, CountLines(lines));
            }

            if (Options[Option.MAX_LINE_LENGTH])
            {
                result.Values.Add(Option.MAX_LINE_LENGTH, CountCharsOfLargestLine(lines));
            }

            results.Add(result);
        }

        return results;
    }

    private long CountBytes(FileInfo fileInfo)
    {
        return fileInfo.Length;
    }

    private long CountChars(string content)
    {
        return content.Length;
    }

    private long CountWords(string content)
    {
        MatchCollection matches = Regex.Matches(content, @"\b\w+\b");
        return matches.Count;
    }

    private long CountLines(string[] lines)
    {
        return lines.Length;
    }

    private long CountCharsOfLargestLine(string[] lines)
    {
        return GetLargestLine(lines).Length;
    }

    private string GetLargestLine(string[] lines)
    {
        return lines.OrderByDescending(line => line.Length).FirstOrDefault() ?? string.Empty;
    }

    public override string ToString()
    {
        var formattedData = new StringBuilder();
        foreach (var result in FileResult)
        {
            if (Options[Option.LINES])
            {
                formattedData.Append($"{result.Values?[Option.LINES]} ");
            }

            if (Options[Option.WORDS])
            {
                formattedData.Append($"{result.Values?[Option.WORDS]} ");
            }
            
            if (Options[Option.CHARS])
            {
                formattedData.Append($"{result.Values?[Option.CHARS]} ");
            }

            if (Options[Option.BYTES])
            {
                formattedData.Append($"{result.Values?[Option.BYTES]} ");
            }

            if (Options[Option.MAX_LINE_LENGTH])
            {
                formattedData.Append($"{result.Values?[Option.MAX_LINE_LENGTH]}\t");
            }

            formattedData.AppendLine(result.Filename);
        }

        return formattedData.ToString();
    }
    
    private Encoding GetEncoding(string filename)
    {
        // Leer los primeros bytes para detectar BOM
        byte[] bom = new byte[4];
        using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            file.Read(bom, 0, 4);
        }

        // UTF-8 BOM
        if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
            return Encoding.UTF8;

        // UTF-16 LE BOM
        if (bom[0] == 0xFF && bom[1] == 0xFE)
            return Encoding.Unicode; // UTF-16 LE

        // UTF-16 BE BOM
        if (bom[0] == 0xFE && bom[1] == 0xFF)
            return Encoding.BigEndianUnicode; // UTF-16 BE

        // UTF-32 LE BOM
        if (bom[0] == 0xFF && bom[1] == 0xFE && bom[2] == 0x00 && bom[3] == 0x00)
            return Encoding.UTF32;

        // UTF-32 BE BOM
        if (bom[0] == 0x00 && bom[1] == 0x00 && bom[2] == 0xFE && bom[3] == 0xFF)
            return new UTF32Encoding(true, true);

        // Si no hay BOM, se asume UTF-8
        return Encoding.UTF8;
    }

}