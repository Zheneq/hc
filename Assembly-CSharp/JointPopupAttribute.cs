using UnityEngine;

public class JointPopupAttribute : PropertyAttribute
{
	public string tooltip;

	public JointPopupAttribute(string tooltipText)
	{
		tooltip = tooltipText;
	}
}
