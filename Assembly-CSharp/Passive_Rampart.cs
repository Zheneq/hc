using System;
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
		return (this.m_cachedShieldBarrierData == null) ? this.m_normalShieldBarrierData : this.m_cachedShieldBarrierData;
	}

	public void SetCachedShieldBarrierData(AbilityModPropertyBarrierDataV2 barrierMod)
	{
		StandardBarrierData cachedShieldBarrierData;
		if (barrierMod != null)
		{
			cachedShieldBarrierData = barrierMod.GetModifiedValue(this.m_normalShieldBarrierData);
		}
		else
		{
			cachedShieldBarrierData = null;
		}
		this.m_cachedShieldBarrierData = cachedShieldBarrierData;
	}
}
