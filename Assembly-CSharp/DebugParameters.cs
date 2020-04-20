using System;
using System.Collections.Generic;

public class DebugParameters
{
	private static DebugParameters s_instance;

	private Dictionary<string, string> m_parameters = new Dictionary<string, string>();

	private DebugParameters()
	{
	}

	public static DebugParameters Get()
	{
		return DebugParameters.s_instance;
	}

	public static void Instantiate()
	{
		DebugParameters.s_instance = new DebugParameters();
	}

	~DebugParameters()
	{
		DebugParameters.s_instance = null;
	}

	public void SetParameter(string key, string value)
	{
		this.m_parameters[key] = value;
	}

	public void SetParameter<T>(string key, T value) where T : IFormattable
	{
		this.m_parameters[key] = value.ToString();
	}

	public void SetParameter(string key, bool value)
	{
		this.m_parameters[key] = ((!value) ? "0" : "1");
	}

	public string GetParameter(string key)
	{
		if (this.m_parameters.ContainsKey(key))
		{
			return this.m_parameters[key];
		}
		return string.Empty;
	}

	public float GetParameterAsFloat(string key)
	{
		if (this.m_parameters.ContainsKey(key))
		{
			return Convert.ToSingle(this.m_parameters[key]);
		}
		return 0f;
	}

	public bool GetParameterAsBool(string key)
	{
		if (this.m_parameters.ContainsKey(key))
		{
			return Convert.ToInt32(this.m_parameters[key]) == 1;
		}
		return false;
	}

	public int GetParameterAsInt(string key)
	{
		if (this.m_parameters.ContainsKey(key))
		{
			return Convert.ToInt32(this.m_parameters[key]);
		}
		return 0;
	}

	public T GetParameterAs<T>(string key) where T : IConvertible
	{
		if (this.m_parameters.ContainsKey(key))
		{
			return (T)((object)Convert.ChangeType(this.m_parameters[key], typeof(T)));
		}
		return (T)((object)Convert.ChangeType(0, typeof(T)));
	}
}
