using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Principal;

public class JsonConfig
{
	[JsonIgnore]
	public bool EnablePostProcess
	{
		get;
		set;
	}

	[JsonIgnore]
	public bool EnablePatches
	{
		get;
		set;
	}

	[JsonIgnore]
	public string HostName
	{
		get;
		private set;
	}

	[JsonIgnore]
	public string ConfigPath
	{
		get;
		set;
	}

	[JsonIgnore]
	public string SystemUserName
	{
		get;
		private set;
	}

	[JsonIgnore]
	public List<FileInfo> LoadedFileInfo
	{
		get;
		private set;
	}

	public List<string> Patches
	{
		get;
		set;
	}

	public JsonConfig()
	{
		EnablePostProcess = true;
		EnablePatches = true;
		HostName = NetUtil.GetHostName().ToLower();
		int num = HostName.IndexOf('.');
		if (num >= 0)
		{
			HostName = HostName.Substring(0, num);
		}
		string text = null;
		try
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			if (current != null)
			{
				text = current.Name;
			}
		}
		catch (Exception)
		{
		}
		if (text == null)
		{
			text = "unknown";
		}
		SystemUserName = text.ToLower();
		int num2 = SystemUserName.IndexOf('\\');
		if (num2 != -1)
		{
			SystemUserName = SystemUserName.Substring(num2 + 1);
		}
		LoadedFileInfo = new List<FileInfo>();
	}

	public virtual void Load(string data)
	{
		JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
		jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
		JsonSerializerSettings settings = jsonSerializerSettings;
		JsonConvert.PopulateObject(data, this, settings);
		if (EnablePostProcess)
		{
			PostProcess();
		}
		if (!EnablePatches)
		{
			return;
		}
		while (true)
		{
			ApplyPatches();
			return;
		}
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
	}

	public virtual void LoadFromFile(string fileName, bool failIfNotFound = true)
	{
		ConfigPath = Path.GetDirectoryName(fileName);
		string data = "{}";
		FileInfo fileInfo = new FileInfo(fileName);
		if (!fileInfo.Exists)
		{
			if (!failIfNotFound)
			{
				goto IL_004c;
			}
		}
		data = File.ReadAllText(fileName);
		goto IL_004c;
		IL_004c:
		LoadedFileInfo.Add(fileInfo);
		Load(data);
	}

	public virtual void SaveToFile(string fileName)
	{
		string contents = SaveToString();
		File.WriteAllText(fileName, contents);
	}

	public bool HasBeenModified()
	{
		using (List<FileInfo>.Enumerator enumerator = LoadedFileInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FileInfo current = enumerator.Current;
				if (current.HasBeenModified())
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public virtual void PostProcess()
	{
	}

	public virtual void ApplyPatches()
	{
	}

	public static void StripSensitiveData(object obj)
	{
		FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if ((fieldInfo.Attributes & FieldAttributes.NotSerialized) == 0)
			{
				if (fieldInfo.GetCustomAttributes(typeof(SensitiveDataAttribute), false).Length > 0)
				{
					object defaultValue = fieldInfo.FieldType.GetDefaultValue();
					fieldInfo.SetValue(obj, defaultValue);
				}
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
