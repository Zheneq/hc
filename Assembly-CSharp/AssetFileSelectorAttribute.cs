using UnityEngine;

public class AssetFileSelectorAttribute : PropertyAttribute
{
	public string m_assetPathRoot;

	public string m_prefix;

	public string m_extension;

	public AssetFileSelectorAttribute(string assetPathRoot, string prefix, string extension)
	{
		m_assetPathRoot = assetPathRoot;
		m_prefix = prefix;
		m_extension = extension;
	}
}
