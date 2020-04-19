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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SeparatorAttribute..ctor(string, bool)).MethodHandle;
			}
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
