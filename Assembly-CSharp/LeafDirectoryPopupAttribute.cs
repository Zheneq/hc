using System;
using UnityEngine;

public class LeafDirectoryPopupAttribute : PropertyAttribute
{
	public string tooltip;

	public string relativePath;

	public LeafDirectoryPopupAttribute(string tooltipText, string relativePathText)
	{
		this.tooltip = tooltipText;
		this.relativePath = relativePathText;
	}
}
