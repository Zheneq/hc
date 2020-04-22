using UnityEngine;

public class Passive_Claymore : Passive
{
	[Header("-- Stack Indicator Ability")]
	public AbilityData.ActionType m_stackIndicatorActionType = AbilityData.ActionType.ABILITY_4;

	[Header("-- Sequence")]
	public GameObject m_selfHealSequencePrefab;

	public GameObject m_ultScaleSequencePrefab;
}
