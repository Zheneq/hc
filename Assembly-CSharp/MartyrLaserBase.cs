using System;
using System.Collections.Generic;

public class MartyrLaserBase : Ability
{
	public float m_additionalWidthPerCrystalSpent;

	public float m_additionalLengthPerCrystalSpent;

	protected virtual Martyr_SyncComponent GetSyncComponent()
	{
		return base.GetComponent<Martyr_SyncComponent>();
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
		return this.m_additionalWidthPerCrystalSpent;
	}

	public float GetBonusLengthPerCrystalSpent()
	{
		return this.m_additionalLengthPerCrystalSpent;
	}

	protected MartyrLaserThreshold GetCurrentPowerEntry(ActorData caster)
	{
		MartyrLaserThreshold result = null;
		if (this.GetSyncComponent() != null)
		{
			if (this.GetSyncComponent().IsBonusActive(caster))
			{
				List<MartyrLaserThreshold> thresholdBasedCrystalBonusList = this.GetThresholdBasedCrystalBonusList();
				int num = this.GetSyncComponent().SpentDamageCrystals(caster);
				for (int i = 0; i < thresholdBasedCrystalBonusList.Count; i++)
				{
					if (num >= thresholdBasedCrystalBonusList[i].m_crystalThreshold)
					{
						result = thresholdBasedCrystalBonusList[i];
					}
				}
			}
		}
		return result;
	}

	public float GetCurrentLaserWidth()
	{
		MartyrLaserThreshold currentPowerEntry = this.GetCurrentPowerEntry(base.ActorData);
		float num;
		if (currentPowerEntry != null)
		{
			num = currentPowerEntry.m_additionalWidth;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return this.GetLaserInfo().width + (float)this.GetSyncComponent().SpentDamageCrystals(base.ActorData) * this.GetBonusWidthPerCrystalSpent() + num2;
	}

	public float GetCurrentLaserRange()
	{
		MartyrLaserThreshold currentPowerEntry = this.GetCurrentPowerEntry(base.ActorData);
		float num;
		if (currentPowerEntry != null)
		{
			num = currentPowerEntry.m_additionalLength;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return this.GetLaserInfo().range + (float)this.GetSyncComponent().SpentDamageCrystals(base.ActorData) * this.GetBonusLengthPerCrystalSpent() + num2;
	}

	public bool GetCurrentLaserPenetrateLoS()
	{
		return this.GetLaserInfo().penetrateLos;
	}

	public int GetCurrentLaserMaxTargets()
	{
		MartyrLaserThreshold currentPowerEntry = this.GetCurrentPowerEntry(base.ActorData);
		int num;
		if (currentPowerEntry != null)
		{
			num = currentPowerEntry.m_additionalTargets;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetLaserInfo().maxTargets + num2;
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
