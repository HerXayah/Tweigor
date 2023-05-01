using System;

namespace Twigor;

internal class Program
{
	public static void Main(string[] args)
	{
		var json = twigorhelper.ReadJson("test.json");

		Console.WriteLine("Starting loop...");
		int count = 0;

		foreach (var line in json)
		{
			// count how many times we looped
			count++;
			Console.WriteLine("Looped " + count + " times");

			if (line.ignore == "true")
			{
				Console.WriteLine("Ignored: " + line.key);
			} else if (line.ignore == "false" || string.IsNullOrEmpty(line.ignore))
			{
				// write to registry
				twigorhelper.WriteToReg(line.hive, line.key, line.path, line.name, line.value, line.type, line.ignore,
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
}

// json tree
public record struct RegTweaks(string hive, string path, string type, string value, string key, string name,
	string ignore, string folder);