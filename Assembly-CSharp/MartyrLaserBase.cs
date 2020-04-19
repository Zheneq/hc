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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrLaserBase.GetCurrentPowerEntry(ActorData)).MethodHandle;
			}
			if (this.GetSyncComponent().IsBonusActive(caster))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				List<MartyrLaserThreshold> thresholdBasedCrystalBonusList = this.GetThresholdBasedCrystalBonusList();
				int num = this.GetSyncComponent().SpentDamageCrystals(caster);
				for (int i = 0; i < thresholdBasedCrystalBonusList.Count; i++)
				{
					if (num >= thresholdBasedCrystalBonusList[i].m_crystalThreshold)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						result = thresholdBasedCrystalBonusList[i];
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrLaserBase.GetCurrentLaserWidth()).MethodHandle;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrLaserBase.GetCurrentLaserRange()).MethodHandle;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrLaserBase.GetCurrentLaserMaxTargets()).MethodHandle;
			}
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
