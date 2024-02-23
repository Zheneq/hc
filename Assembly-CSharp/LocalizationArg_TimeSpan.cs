using System;
using System.Text;

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
		string primaryUnit = null;
		int primaryValue = 0;
		string secondaryUnit = null;
		int secondaryValue = 0;
		if (m_span.Days > 0)
		{
			primaryValue = m_span.Days;
			secondaryValue = m_span.Hours;
			primaryUnit = m_span.Days == 1 ? "Day" : "Days";
			if (m_span.Hours > 0)
			{
				secondaryUnit = m_span.Hours == 1 ? "Hour" : "Hours";
			}
		}
		else if (m_span.Hours > 0)
		{
			primaryValue = m_span.Hours;
			secondaryValue = m_span.Minutes;
			primaryUnit = m_span.Hours == 1 ? "Hour" : "Hours";
			if (m_span.Minutes > 0)
			{
				secondaryUnit = m_span.Minutes == 1 ? "Minute" : "Minutes";
			}
		}
		else if (m_span.Minutes > 0)
		{
			primaryValue = m_span.Minutes;
			secondaryValue = m_span.Seconds;
			primaryUnit = m_span.Minutes == 1 ? "Minute" : "Minutes";
			if (m_span.Seconds > 0)
			{
				secondaryUnit = m_span.Seconds == 1 ? "Second" : "Seconds";
			}
		}
		else
		{
			if (m_span.Seconds <= 0)
			{
				return StringUtil.TR("Soon", "TimeSpanFragment");
			}
			primaryValue = m_span.Seconds;
			primaryUnit = m_span.Seconds == 1 ? "Second" : "Seconds";
		}
		
		return secondaryUnit == null
			? string.Format(StringUtil.TR(new StringBuilder().Append("Just").Append(primaryUnit).ToString(), "TimeSpanFragment"), primaryValue)
			: string.Format(StringUtil.TR(new StringBuilder().Append(primaryUnit).Append("And").Append(secondaryUnit).ToString(), "TimeSpanFragment"), primaryValue, secondaryValue);
	}
}
