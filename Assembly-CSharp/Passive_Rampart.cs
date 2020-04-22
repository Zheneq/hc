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
		return (m_cachedShieldBarrierData == null) ? m_normalShieldBarrierData : m_cachedShieldBarrierData;
	}

	public void SetCachedShieldBarrierData(AbilityModPropertyBarrierDataV2 barrierMod)
	{
		object cachedShieldBarrierData;
		if (barrierMod != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedShieldBarrierData = barrierMod.GetModifiedValue(m_normalShieldBarrierData);
		}
		else
		{
			cachedShieldBarrierData = null;
		}
		m_cachedShieldBarrierData = (StandardBarrierData)cachedShieldBarrierData;
	}
}
