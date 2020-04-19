using System;
using UnityEngine;

public class AssetFileSelectorAttribute : PropertyAttribute
{
	public string m_assetPathRoot;

	public string m_prefix;

	public string m_extension;

	public AssetFileSelectorAttribute(string assetPathRoot, string prefix, string extension)
	{
		this.m_assetPathRoot = assetPathRoot;
		this.m_prefix = prefix;
		this.m_extension = extension;
	}
}
