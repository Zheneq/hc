using UnityEngine;

public class DinoLayeredRings : GenericAbility_Container
{
	[Separator("Aoe ground effect at targeted position", true)]
	public bool m_addGroundAoeAtTargetPos = true;

	public int m_groundAoeDuration = 2;

	[Separator("On Hit Data for Ground Aoe", "yellow")]
	public OnHitAuthoredData m_groundAoeOnHitData;

	[Separator("Persistent Ground Aoe Sequences", true)]
	public GameObject m_persistentSeqPrefab;

	public GameObject m_onTriggerSeqPrefab;
}
