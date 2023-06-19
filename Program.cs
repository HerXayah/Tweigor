using System;

namespace Twigor;

internal class Program
{

    public static void Main(string[] args)
    {
        var restorePoint = twigorhelper.CreateRestorePoint("Twigor System Restore Point", 0, 100);
        var json = twigorhelper.ReadJson("test.json");
        
        
        if (restorePoint && json != null)
        {
            Console.WriteLine("System Restore Point was created!");
            Console.WriteLine("Starting loop...");
            var count = 0;

            foreach (var line in json)
            {
                // count how many times we looped
                count++;
                Console.WriteLine("Looped " + count + " times");

                if (line.ignore.Equals("true")) {
                    Console.WriteLine("Ignored: " + line.key);
                } else if (line.ignore.Equals("false") || string.IsNullOrEmpty(line.ignore)) {
                    // write to registry
                    twigorhelper.WriteToReg(line.hive, line.key, line.path, line.name, line.value, line.type,
                        line.ignore,
                        line.folder, response =>
                        {
                            // if callback contains error, print error, else print success
                            Console.WriteLine(response.Contains("Error")
                                ? response
                                : "Wrote to registry: " + line.key + " in " + line.hive + " with name " + line.name);
                        });
                }
            }
        }
        else
        {
            Console.WriteLine("Creating System Restore Point has failed!");
            Console.WriteLine("Regedit was not changed!");
        }
    }
}

// json tree
public record struct RegTweaks(string hive, string key, string path, string name, string value, string type,
    string ignore, string folder);