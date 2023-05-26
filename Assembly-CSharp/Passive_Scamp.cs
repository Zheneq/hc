using UnityEngine;

public class Passive_Scamp : Passive
{
	[Separator("Suit Shield Effect Data")]
	public StandardActorEffectData m_shieldEffectData;
	[Separator("Whether to zero out energy when shield is depleted")]
	public bool m_clearEnergyOnSuitRemoval;
	[Separator("Energy Orbs")]
	public int m_orbDuration = 4;
	[Space(5f)]
	public int m_orbNumToSpawn = 5;
	public float m_orbMinSpawnDist = 1f;
	public float m_orbMaxSpawnDist = 10f;
	public int m_orbEnergyGainOnTrigger = 15;
	public StandardEffectInfo m_orbTriggerEffect;
	[Header("-- Whether to clear orbs on death")]
	public bool m_clearOrbsOnDeath = true;
	[Separator("Reset Energy on Respawn?")]
	public bool m_resetEnergyOnRespawn;
	[Separator("Approximate duration of orb spawn animation")]
	public float m_orbSpawnAnimDuration = 3f;
	[Separator("Sequences for Energy Orb")]
	public GameObject m_orbSpawnCasterSeqPrefab;
	[Header("-- Optional sequence to show projectile towards each spawned orb")]
	public GameObject m_orbSpawnProjectileSeqPrefab;
	[Space(10f)]
	public GameObject m_orbPersistentSeqPrefab;
	public GameObject m_orbTriggerSeqPrefab;

	public int GetMaxSuitShield()
	{
		return m_shieldEffectData.m_absorbAmount;
	}
}
