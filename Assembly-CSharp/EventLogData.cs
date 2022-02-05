// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Serialization;
using System.Text;

// server-only
// TODO fix serialization?
#if SERVER
//[DataContract]
public class EventLogData
{
	//[DataMember]
	private List<EventLogData.LogPair> m_values;

	private static readonly char[] s_hexDigits = new char[]
	{
		'0',
		'1',
		'2',
		'3',
		'4',
		'5',
		'6',
		'7',
		'8',
		'9',
		'a',
		'b',
		'c',
		'd',
		'e',
		'f'
	};

	public EventLogData()
	{
		m_values = new List<LogPair>();
	}

	public void AddData(string key, object value)
	{
		if (value == null)
		{
			return;
		}
		string text = Encode(value.ToString());
		if (text.IsNullOrEmpty())
		{
			return;
		}
		if (!(from v in m_values
		where v.Key == key
		select v).Any<LogPair>())
		{
			m_values.Add(new LogPair
			{
				Key = key,
				Value = text
			});
		}
	}

	public void AddData(EventLogData data)
	{
		if (data == null || data.m_values == null)
		{
			return;
		}
		foreach (LogPair value in data.m_values)
		{
			if (!(from v in this.m_values
			where v.Key == value.Key
			select v).Any<LogPair>())
			{
				m_values.Add(value);
			}
		}
	}

	public override string ToString()
	{
		if (m_values == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (LogPair logPair in m_values)
		{
			stringBuilder.AppendFormat("{0}={1}\t", logPair.Key, logPair.Value);
		}
		return stringBuilder.ToString();
	}

	private string Encode(string input)
	{
		if (input.Length > 1024)
		{
			input = input.Substring(0, 1024);
		}
		StringBuilder stringBuilder = new StringBuilder((int)((double)input.Length * 1.1));
		foreach (char c in input)
		{
			if (char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '-' || c == ':' || c == '!' || c == '/' || c == '\\' || c == '\'' || c == '(' || c == ')')
			{
				stringBuilder.Append(c);
			}
			else if (c == ' ')
			{
				stringBuilder.Append('+');
			}
			else
			{
				stringBuilder.Append('%');
				byte b = (byte)c;
				stringBuilder.Append(s_hexDigits[b >> 4 & 15]);
				stringBuilder.Append(s_hexDigits[(int)(b & 15)]);
			}
		}
		return stringBuilder.ToString();
	}

    //[DataContract]
    public class LogPair
	{
		//[DataMember]
		public string Key;

		//[DataMember]
		public string Value;
	}
}
#endif
