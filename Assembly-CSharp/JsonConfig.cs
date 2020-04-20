using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class JsonConfig
{
	public JsonConfig()
	{
		this.EnablePostProcess = true;
		this.EnablePatches = true;
		this.HostName = NetUtil.GetHostName().ToLower();
		int num = this.HostName.IndexOf('.');
		if (num >= 0)
		{
			this.HostName = this.HostName.Substring(0, num);
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
		this.SystemUserName = text.ToLower();
		int num2 = this.SystemUserName.IndexOf('\\');
		if (num2 != -1)
		{
			this.SystemUserName = this.SystemUserName.Substring(num2 + 1);
		}
		this.LoadedFileInfo = new List<FileInfo>();
	}

	[JsonIgnore]
	public bool EnablePostProcess { get; set; }

	[JsonIgnore]
	public bool EnablePatches { get; set; }

	[JsonIgnore]
	public string HostName { get; private set; }

	[JsonIgnore]
	public string ConfigPath { get; set; }

	[JsonIgnore]
	public string SystemUserName { get; private set; }

	[JsonIgnore]
	public List<FileInfo> LoadedFileInfo { get; private set; }

	public List<string> Patches { get; set; }

	public virtual void Load(string data)
	{
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			ObjectCreationHandling = ObjectCreationHandling.Replace
		};
		JsonConvert.PopulateObject(data, this, settings);
		if (this.EnablePostProcess)
		{
			this.PostProcess();
		}
		if (this.EnablePatches)
		{
			this.ApplyPatches();
		}
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonConverter[]
		{
			new StringEnumConverter()
		});
	}

	public virtual void LoadFromFile(string fileName, bool failIfNotFound = true)
	{
		this.ConfigPath = Path.GetDirectoryName(fileName);
		string data = "{}";
		FileInfo fileInfo = new FileInfo(fileName);
		if (!fileInfo.Exists)
		{
			if (!failIfNotFound)
			{
				goto IL_4C;
			}
		}
		data = File.ReadAllText(fileName);
		IL_4C:
		this.LoadedFileInfo.Add(fileInfo);
		this.Load(data);
	}

	public virtual void SaveToFile(string fileName)
	{
		string contents = this.SaveToString();
		File.WriteAllText(fileName, contents);
	}

	public bool HasBeenModified()
	{
		using (List<FileInfo>.Enumerator enumerator = this.LoadedFileInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FileInfo fileInfo = enumerator.Current;
				if (fileInfo.HasBeenModified(null))
				{
					return true;
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
		foreach (FieldInfo fieldInfo in fields)
		{
			if ((fieldInfo.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.PrivateScope)
			{
				if (fieldInfo.GetCustomAttributes(typeof(SensitiveDataAttribute), false).Length > 0)
				{
					object defaultValue = fieldInfo.FieldType.GetDefaultValue();
					fieldInfo.SetValue(obj, defaultValue);
				}
			}
		}
	}
}
