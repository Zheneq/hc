using UnityEngine;

public class Passive_Rampart : Passive
{
	[Header("-- Normal Shield Barrier Info")]
	public int m_normalShieldDuration = 2;
	public StandardBarrierData m_normalShieldBarrierData;
	[Header("-- Anim")]
	public int m_unshieldedIdleType;
	public int m_shieldedIdleType = 1;
	[Header("-- Sequences")]
	public GameObject m_removeShieldWaitSequencePrefab;

	private StandardBarrierData m_cachedShieldBarrierData;

	public StandardBarrierData GetShieldBarrierData()
	{
		return m_cachedShieldBarrierData ?? m_normalShieldBarrierData;
	}

	public void SetCachedShieldBarrierData(AbilityModPropertyBarrierDataV2 barrierMod)
	{
		m_cachedShieldBarrierData = (barrierMod?.GetModifiedValue(m_normalShieldBarrierData));
	}
}
