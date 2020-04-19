using System;

[Serializable]
public class TargetData
{
	public string m_description;

	public float m_range;

	public float m_minRange;

	public bool m_checkLineOfSight;

	public Ability.TargetingParadigm m_targetingParadigm;
}
