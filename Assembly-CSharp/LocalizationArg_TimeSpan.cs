using System;

[Serializable]
public class LocalizationArg_TimeSpan : LocalizationArg
{
	public TimeSpan m_span;

	public static LocalizationArg_TimeSpan Create(TimeSpan span)
	{
		LocalizationArg_TimeSpan localizationArg_TimeSpan = new LocalizationArg_TimeSpan();
		localizationArg_TimeSpan.m_span = span;
		return localizationArg_TimeSpan;
	}

	public override string TR()
	{
		string text = null;
		int num = 0;
		string text2 = null;
		int num2 = 0;
		if (m_span.Days > 0)
		{
			num = m_span.Days;
			num2 = m_span.Hours;
			object obj;
			if (m_span.Days == 1)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				obj = "Day";
			}
			else
			{
				obj = "Days";
			}
			text = (string)obj;
			if (m_span.Hours > 0)
			{
				object obj2;
				if (m_span.Hours == 1)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					obj2 = "Hour";
				}
				else
				{
					obj2 = "Hours";
				}
				text2 = (string)obj2;
			}
		}
		else if (m_span.Hours > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_span.Hours;
			num2 = m_span.Minutes;
			object obj3;
			if (m_span.Hours == 1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				obj3 = "Hour";
			}
			else
			{
				obj3 = "Hours";
			}
			text = (string)obj3;
			if (m_span.Minutes > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				object obj4;
				if (m_span.Minutes == 1)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					obj4 = "Minute";
				}
				else
				{
					obj4 = "Minutes";
				}
				text2 = (string)obj4;
			}
		}
		else if (m_span.Minutes > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_span.Minutes;
			num2 = m_span.Seconds;
			object obj5;
			if (m_span.Minutes == 1)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				obj5 = "Minute";
			}
			else
			{
				obj5 = "Minutes";
			}
			text = (string)obj5;
			if (m_span.Seconds > 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				object obj6;
				if (m_span.Seconds == 1)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					obj6 = "Second";
				}
				else
				{
					obj6 = "Seconds";
				}
				text2 = (string)obj6;
			}
		}
		else
		{
			if (m_span.Seconds <= 0)
			{
				return StringUtil.TR("Soon", "TimeSpanFragment");
			}
			num = m_span.Seconds;
			object obj7;
			if (m_span.Seconds == 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				obj7 = "Second";
			}
			else
			{
				obj7 = "Seconds";
			}
			text = (string)obj7;
		}
		if (text2 == null)
		{
			string term = $"Just{text}";
			return string.Format(StringUtil.TR(term, "TimeSpanFragment"), num);
		}
		string term2 = $"{text}And{text2}";
		return string.Format(StringUtil.TR(term2, "TimeSpanFragment"), num, num2);
	}
}
