using System;

[Serializable]
public class LocalizationArg_TimeSpan : LocalizationArg
{
	public TimeSpan m_span;

	public static LocalizationArg_TimeSpan Create(TimeSpan span)
	{
		return new LocalizationArg_TimeSpan
		{
			m_span = span
		};
	}

	public override string TR()
	{
		string text = null;
		int num = 0;
		int num2;
		string arg;
		if (this.m_span.Days > 0)
		{
			num2 = this.m_span.Days;
			num = this.m_span.Hours;
			string text2;
			if (this.m_span.Days == 1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationArg_TimeSpan.TR()).MethodHandle;
				}
				text2 = "Day";
			}
			else
			{
				text2 = "Days";
			}
			arg = text2;
			if (this.m_span.Hours > 0)
			{
				string text3;
				if (this.m_span.Hours == 1)
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
					text3 = "Hour";
				}
				else
				{
					text3 = "Hours";
				}
				text = text3;
			}
		}
		else if (this.m_span.Hours > 0)
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
			num2 = this.m_span.Hours;
			num = this.m_span.Minutes;
			string text4;
			if (this.m_span.Hours == 1)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				text4 = "Hour";
			}
			else
			{
				text4 = "Hours";
			}
			arg = text4;
			if (this.m_span.Minutes > 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				string text5;
				if (this.m_span.Minutes == 1)
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
					text5 = "Minute";
				}
				else
				{
					text5 = "Minutes";
				}
				text = text5;
			}
		}
		else if (this.m_span.Minutes > 0)
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
			num2 = this.m_span.Minutes;
			num = this.m_span.Seconds;
			string text6;
			if (this.m_span.Minutes == 1)
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
				text6 = "Minute";
			}
			else
			{
				text6 = "Minutes";
			}
			arg = text6;
			if (this.m_span.Seconds > 0)
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
				string text7;
				if (this.m_span.Seconds == 1)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					text7 = "Second";
				}
				else
				{
					text7 = "Seconds";
				}
				text = text7;
			}
		}
		else
		{
			if (this.m_span.Seconds <= 0)
			{
				return StringUtil.TR("Soon", "TimeSpanFragment");
			}
			num2 = this.m_span.Seconds;
			string text8;
			if (this.m_span.Seconds == 1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				text8 = "Second";
			}
			else
			{
				text8 = "Seconds";
			}
			arg = text8;
		}
		if (text == null)
		{
			string term = string.Format("Just{0}", arg);
			return string.Format(StringUtil.TR(term, "TimeSpanFragment"), num2);
		}
		string term2 = string.Format("{0}And{1}", arg, text);
		return string.Format(StringUtil.TR(term2, "TimeSpanFragment"), num2, num);
	}
}
