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
		return s_instance;
	}

	public static void Instantiate()
	{
		s_instance = new DebugParameters();
	}

	~DebugParameters()
	{
		s_instance = null;
	}

	public void SetParameter(string key, string value)
	{
		m_parameters[key] = value;
	}

	public void SetParameter<T>(string key, T value) where T : IFormattable
	{
		m_parameters[key] = value.ToString();
	}

	public void SetParameter(string key, bool value)
	{
		m_parameters[key] = ((!value) ? "0" : "1");
	}

	public string GetParameter(string key)
	{
		if (m_parameters.ContainsKey(key))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_parameters[key];
				}
			}
		}
		return string.Empty;
	}

	public float GetParameterAsFloat(string key)
	{
		if (m_parameters.ContainsKey(key))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return Convert.ToSingle(m_parameters[key]);
				}
			}
		}
		return 0f;
	}

	public bool GetParameterAsBool(string key)
	{
		if (m_parameters.ContainsKey(key))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return Convert.ToInt32(m_parameters[key]) == 1;
				}
			}
		}
		return false;
	}

	public int GetParameterAsInt(string key)
	{
		if (m_parameters.ContainsKey(key))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return Convert.ToInt32(m_parameters[key]);
				}
			}
		}
		return 0;
	}

	public T GetParameterAs<T>(string key) where T : IConvertible
	{
		if (m_parameters.ContainsKey(key))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return (T)Convert.ChangeType(m_parameters[key], typeof(T));
				}
			}
		}
		return (T)Convert.ChangeType(0, typeof(T));
	}
}
