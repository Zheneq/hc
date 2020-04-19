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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(JsonConfig..ctor()).MethodHandle;
			}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			text = "unknown";
		}
		this.SystemUserName = text.ToLower();
		int num2 = this.SystemUserName.IndexOf('\\');
		if (num2 != -1)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(JsonConfig.Load(string)).MethodHandle;
			}
			this.PostProcess();
		}
		if (this.EnablePatches)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(JsonConfig.LoadFromFile(string, bool)).MethodHandle;
			}
			if (!failIfNotFound)
			{
				goto IL_4C;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(JsonConfig.HasBeenModified()).MethodHandle;
					}
					return true;
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(JsonConfig.StripSensitiveData(object)).MethodHandle;
				}
				if (fieldInfo.GetCustomAttributes(typeof(SensitiveDataAttribute), false).Length > 0)
				{
					object defaultValue = fieldInfo.FieldType.GetDefaultValue();
					fieldInfo.SetValue(obj, defaultValue);
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}
}
