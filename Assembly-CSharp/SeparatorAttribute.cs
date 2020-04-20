using System;
using UnityEngine;

public class SeparatorAttribute : PropertyAttribute
{
	public readonly string m_title;

	public readonly string m_colorStr;

	public readonly bool m_setColor;

	public SeparatorAttribute()
	{
		this.m_title = string.Empty;
	}

	public SeparatorAttribute(string title, bool useDefaultColor = true)
	{
		this.m_title = title;
		if (useDefaultColor)
		{
			this.m_setColor = true;
			this.m_colorStr = "cyan";
		}
	}

	public SeparatorAttribute(string title, string colorStr)
	{
		this.m_title = title;
		this.m_colorStr = colorStr;
		this.m_setColor = true;
	}
}
