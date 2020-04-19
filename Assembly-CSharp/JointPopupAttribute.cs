using System;
using UnityEngine;

public class JointPopupAttribute : PropertyAttribute
{
	public string tooltip;

	public JointPopupAttribute(string tooltipText)
	{
		this.tooltip = tooltipText;
	}
}
