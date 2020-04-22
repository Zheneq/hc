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
		if (GetSyncComponent() != null)
		{
			while (true)
			{
				switch (2)
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
			if (GetSyncComponent().IsBonusActive(caster))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				List<MartyrLaserThreshold> thresholdBasedCrystalBonusList = GetThresholdBasedCrystalBonusList();
				int num = GetSyncComponent().SpentDamageCrystals(caster);
				for (int i = 0; i < thresholdBasedCrystalBonusList.Count; i++)
				{
					if (num >= thresholdBasedCrystalBonusList[i].m_crystalThreshold)
					{
						while (true)
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
				while (true)
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
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(base.ActorData);
		float num;
		if (currentPowerEntry != null)
		{
			while (true)
			{
				switch (1)
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
			num = currentPowerEntry.m_additionalWidth;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return GetLaserInfo().width + (float)GetSyncComponent().SpentDamageCrystals(base.ActorData) * GetBonusWidthPerCrystalSpent() + num2;
	}

	public float GetCurrentLaserRange()
	{
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(base.ActorData);
		float num;
		if (currentPowerEntry != null)
		{
			while (true)
			{
				switch (3)
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
			num = currentPowerEntry.m_additionalLength;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return GetLaserInfo().range + (float)GetSyncComponent().SpentDamageCrystals(base.ActorData) * GetBonusLengthPerCrystalSpent() + num2;
	}

	public bool GetCurrentLaserPenetrateLoS()
	{
		return GetLaserInfo().penetrateLos;
	}

	public int GetCurrentLaserMaxTargets()
	{
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(base.ActorData);
		int num;
		if (currentPowerEntry != null)
		{
			while (true)
			{
				switch (5)
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
			num = currentPowerEntry.m_additionalTargets;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetLaserInfo().maxTargets + num2;
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
