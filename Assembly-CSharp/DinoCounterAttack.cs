using UnityEngine;

public class DinoCounterAttack : GenericAbility_Container
{
	[Separator("Counter reaction", true)]
	public int m_counterDamageAmount = 20;

	public StandardEffectInfo m_counterEffect;

	public StandardEffectInfo m_effectOnSelf;

	public int m_techPointGainPerCounter;

	public GameObject m_counterReactionSequencePrefab;
}
