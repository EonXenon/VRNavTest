using System.IO;
using System.Threading.Tasks;

public class DataOutput
{
    public static async Task Write(string s, string f)
    {
        using StreamWriter file = new StreamWriter(f, append: true);
        await file.WriteLineAsync(s);
    }

}
