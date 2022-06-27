// ROGUES
// SERVER
using System.Collections.Generic;

public class MartyrLaserBase : Ability
{
	public float m_additionalWidthPerCrystalSpent;
	public float m_additionalLengthPerCrystalSpent;

	protected virtual Martyr_SyncComponent GetSyncComponent()
	{
		return GetComponent<Martyr_SyncComponent>();
	}

	public virtual LaserTargetingInfo GetLaserInfo()
	{
		return null;
	}

	protected virtual List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		return null;
	}

	public float GetBonusWidthPerCrystalSpent()
	{
		return m_additionalWidthPerCrystalSpent;
	}

	public float GetBonusLengthPerCrystalSpent()
	{
		return m_additionalLengthPerCrystalSpent;
	}

	protected MartyrLaserThreshold GetCurrentPowerEntry(ActorData caster)
	{
		MartyrLaserThreshold result = null;
		if (GetSyncComponent() != null && GetSyncComponent().IsBonusActive(caster))
		{
			int spentDamageCrystals = GetSyncComponent().SpentDamageCrystals(caster);
			foreach (MartyrLaserThreshold bonus in GetThresholdBasedCrystalBonusList())
			{
				if (spentDamageCrystals >= bonus.m_crystalThreshold)
				{
					result = bonus;
				}
			}
		}
		return result;
	}

	public float GetCurrentLaserWidth()
	{
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(ActorData);
		float additionalWidth = currentPowerEntry != null ? currentPowerEntry.m_additionalWidth : 0f;
		return GetLaserInfo().width
			+ GetSyncComponent().SpentDamageCrystals(ActorData) * GetBonusWidthPerCrystalSpent()
			+ additionalWidth;
	}

	public float GetCurrentLaserRange()
	{
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(ActorData);
		float additionalLength = currentPowerEntry != null ? currentPowerEntry.m_additionalLength : 0f;
		return GetLaserInfo().range
			+ GetSyncComponent().SpentDamageCrystals(ActorData) * GetBonusLengthPerCrystalSpent()
			+ additionalLength;
	}

	public bool GetCurrentLaserPenetrateLoS()
	{
		return GetLaserInfo().penetrateLos;
	}

	public int GetCurrentLaserMaxTargets()
	{
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(ActorData);
		int additionalTargets = currentPowerEntry != null ? currentPowerEntry.m_additionalTargets : 0;
		return GetLaserInfo().maxTargets + additionalTargets;
	}

	public virtual float GetCurrentExplosionRadius()
	{
		return 0f;
	}

	public virtual float GetCurrentInnerExplosionRadius()
	{
		return 0f;
	}
}
