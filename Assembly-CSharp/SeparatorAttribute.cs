using UnityEngine;

public class SeparatorAttribute : PropertyAttribute
{
	public readonly string m_title;

	public readonly string m_colorStr;

	public readonly bool m_setColor;

	public SeparatorAttribute()
	{
		m_title = string.Empty;
	}

	public SeparatorAttribute(string title, bool useDefaultColor = true)
	{
		m_title = title;
		if (!useDefaultColor)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_setColor = true;
			m_colorStr = "cyan";
			return;
		}
	}

	public SeparatorAttribute(string title, string colorStr)
	{
		m_title = title;
		m_colorStr = colorStr;
		m_setColor = true;
	}
}
