using UnityEngine;

public class LeafDirectoryPopupAttribute : PropertyAttribute
{
	public string tooltip;

	public string relativePath;

	public LeafDirectoryPopupAttribute(string tooltipText, string relativePathText)
	{
		tooltip = tooltipText;
		relativePath = relativePathText;
	}
}
