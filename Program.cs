using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Twigor;

internal class Program
{
	public static void Main(string[] args)
	{
		var json = twigorhelper.ReadJson("test.json");

		Console.WriteLine("Writing to registry...");

		foreach (var line in json)
		{
			if (string.IsNullOrEmpty(line.ignore) || bool.TryParse(line.ignore, out bool ignore) && ignore)
			{
				Console.WriteLine("Ignored: " + line.key);
			} else
			{
				twigorhelper.WriteToReg(line.hive, line.key, line.name, response =>
				{
					Console.WriteLine(response.Contains("Error")
						? response
						: "Wrote to registry: " + line.key + " in " + line.hive + " with name " + line.name);
				});
			}
		}
	}

	private static void HandleError(string errorMessage)
	{
		Console.WriteLine(errorMessage);
	}
}

public record struct RegTweaks(string hive, string path, string type, string value, string key, string name,
	string ignore);