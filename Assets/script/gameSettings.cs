using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class gameSettings : MonoBehaviour
{
	public OptionsData Options = null;

	public bool isLoaded = false;

	// Start is called before the first frame update
	void Start()
	{
		var json = ReadJsonFromFile("settings.json");

		if (!string.IsNullOrEmpty(json))
		{
			Options = JsonUtility.FromJson<OptionsData>(json);
			isLoaded = true;
			//if (Options != null)
			//{
			//	Debug.Log("Deserialization successful.");

			//	foreach (Option option in Options.options)
			//	{
			//		Debug.Log("Option Display Title: " + option.displayTitle);
			//		Debug.Log("MultiSelectionEnabled: " + option.multiSelectionEnabled);
			//		Debug.Log("Value Type: " + option.valueType);

			//		foreach (OptionValue value in option.values)
			//		{
			//			Debug.Log("Value ID: " + value.id);
			//			Debug.Log("Value Title: " + value.title);
			//			Debug.Log("Value: " + value.value);
			//		}

			//		Debug.Log("Selected IDs: " + string.Join(", ", option.selected));
			//	}
			//}
			//else
			//{
			//	Debug.LogError("Failed to deserialize JSON.");
			//}
		}
		else
		{
			Debug.LogError("Failed to read JSON from file.");
		}
	}

	string ReadJsonFromFile(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}
			else
			{
				//Debug.LogError("File not found at path: " + path);

				var opt = new OptionsData();
				opt.options.Add(new Option
				{
					displayTitle = "Main",
					multiSelectionEnabled = false,
					selected = new List<int> { 0 },
					valueType = "number",
					values = new List<OptionValue>
					{
						new OptionValue{id = 1, title = "Play time", value = "90"}
					}
				});

				var res = JsonUtility.ToJson(opt, true);

				File.WriteAllText("settings.json", res);

				return res;
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError("Error reading file: " + e.Message);
			return null;
		}
	}
}

[System.Serializable]
public class OptionValue
{
	public int id;
	public string title;
	public string value; // Can be string or number (int, float, etc.)
}

[System.Serializable]
public class Option
{
	public string displayTitle;
	public bool multiSelectionEnabled;
	public string valueType; // Possible values: "string" or "number"
	public List<OptionValue> values;
	public List<int> selected;
}

[System.Serializable]
public class OptionsData
{
	public List<Option> options = new List<Option>();
}