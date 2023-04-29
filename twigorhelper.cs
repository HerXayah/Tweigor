using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Twigor;

public class twigorhelper
{
	public delegate void ShootyShootyBangBang(string response);

	// write to registry but check node first where to put it
	public static void WriteToReg(string hive, string key, string ignore, ShootyShootyBangBang callback)
	{
		try
		{
			// c# 8 switch expression
			var node = hive switch
			{
				"HKEY_CURRENT_USER" => Registry.CurrentUser,
				"HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
				"HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
				"HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
				"HKEY_USERS" => Registry.Users,
				_ => throw new Exception("Invalid hive")
			};
			Console.WriteLine("Writing to registry... in node: " + node);

			// write to registry
			node.SetValue(key, ignore);

			// call success callback
			callback("Wrote to registry: " + key + " in " + hive + " with name " + node);
		} catch (Exception e)
		{
			// call error callback
			callback("Error: " + e.Message);
		}
	}

	public static List<RegTweaks>? ReadJson(string fileName)
	{
		using (var r = new StreamReader(fileName))
		{
			var tw = r.ReadToEnd();
			return JsonConvert.DeserializeObject<List<RegTweaks>>(tw);
		}
	}

	// convert JSON to Indented String
	public static string JsonToIndentedString(string jsonpath)
	{
		if (string.IsNullOrEmpty(jsonpath)) return string.Empty;

		var obj = JsonConvert.DeserializeObject(jsonpath);
		return JsonConvert.SerializeObject(obj, Formatting.Indented);
	}
}