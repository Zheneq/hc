using System;
using UnityEngine;

[Serializable]
public class TargeterTemplateSwapData
{
	public string m_notes;

	public TargeterTemplateSwapData.TargeterTemplateType m_templateToReplace;

	public GameObject m_prefabToUse;

	public enum TargeterTemplateType
	{
		Unknown,
		DynamicCone,
		Laser
	}
}
