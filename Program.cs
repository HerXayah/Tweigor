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

			// check if we should ignore this line by checking if the ignore property is empty | true or false
			if (string.IsNullOrEmpty(line.ignore) || bool.TryParse(line.ignore, out bool ignore) && ignore)
			{
				Console.WriteLine("Ignored: " + line.key);
			} else
			{
				// write to registry
				twigorhelper.WriteToReg(line.hive, line.key, line.name, response =>
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

public record struct RegTweaks(string hive, string path, string type, string value, string key, string name,
	string ignore);