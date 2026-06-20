using System;
using UnityEngine;

[Serializable]
public class TargeterTemplateSwapData
{
	public enum TargeterTemplateType
	{
		Unknown,
		DynamicCone,
		Laser
	}

	public string m_notes;

	public TargeterTemplateType m_templateToReplace;

	public GameObject m_prefabToUse;
}
