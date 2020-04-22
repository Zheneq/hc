using System.Collections.Generic;
using UnityEngine;

public class ThiefSpoilLaserUlt : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget;

	public float m_targeterMaxAngle = 120f;

	public float m_targeterMinInterpDistance = 1.5f;

	public float m_targeterMaxInterpDistance = 6f;

	[Header("-- Damage")]
	public int m_laserDamageAmount = 3;

	public int m_laserSubsequentDamageAmount = 3;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;

	public float m_laserWidth = 0.5f;

	public int m_laserMaxTargets = 1;

	public int m_laserCount = 2;

	public bool m_laserPenetrateLos;

	[Header("-- Spoil Spawn Data On Enemy Hit")]
	public SpoilsSpawnData m_spoilSpawnData;

	[Header("-- PowerUp/Spoils Interaction")]
	public bool m_hitPowerups;

	public bool m_stopOnPowerupHit = true;

	public bool m_includeSpoilsPowerups = true;

	public bool m_ignorePickupTeamRestriction;

	public int m_maxPowerupsHit;

	[Header("-- Buffs Copy --")]
	public bool m_copyBuffsOnEnemyHit;

	public int m_copyBuffDuration = 2;

	public List<StatusType> m_buffsToCopy;

	[Header("-- Sequences")]
	public GameObject m_laserSequencePrefab;

	public GameObject m_powerupReturnPrefab;

	public GameObject m_onBuffCopyAudioSequencePrefab;

	private AbilityMod_ThiefSpoilLaserUlt m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private SpoilsSpawnData m_cachedSpoilSpawnData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Ult 2";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_targeterMultiTarget)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ClearTargeters();
					for (int i = 0; i < GetLaserCount(); i++)
					{
						base.Targeters.Add(new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance, GetLaserRange(), GetLaserWidth(), GetLaserMaxTargets(), GetLaserCount(), LaserPenetrateLos(), HitPowerups(), StopOnPowerupHit(), IncludeSpoilsPowerups(), IgnorePickupTeamRestriction(), GetMaxPowerupsHit()));
						base.Targeters[i].SetUseMultiTargetUpdate(true);
					}
					while (true)
					{
						switch (3)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance, GetLaserRange(), GetLaserWidth(), GetLaserMaxTargets(), GetLaserCount(), LaserPenetrateLos(), HitPowerups(), StopOnPowerupHit(), IncludeSpoilsPowerups(), IgnorePickupTeamRestriction(), GetMaxPowerupsHit());
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (m_targeterMultiTarget)
		{
			while (true)
			{
				switch (6)
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
			result = GetLaserCount();
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
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
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		SpoilsSpawnData cachedSpoilSpawnData;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedSpoilSpawnData = m_abilityMod.m_spoilSpawnDataMod.GetModifiedValue(m_spoilSpawnData);
		}
		else
		{
			cachedSpoilSpawnData = m_spoilSpawnData;
		}
		m_cachedSpoilSpawnData = cachedSpoilSpawnData;
	}

	public float GetTargeterMaxAngle()
	{
		return (!m_abilityMod) ? m_targeterMaxAngle : m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle);
	}

	public int GetLaserDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			result = m_laserDamageAmount;
		}
		return result;
	}

	public int GetLaserSubsequentDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount);
		}
		else
		{
			result = m_laserSubsequentDamageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
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
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
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
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!m_abilityMod) ? m_laserWidth : m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
	}

	public int GetLaserMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			result = m_laserMaxTargets;
		}
		return result;
	}

	public int GetLaserCount()
	{
		return (!m_abilityMod) ? m_laserCount : m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount);
	}

	public bool LaserPenetrateLos()
	{
		return (!m_abilityMod) ? m_laserPenetrateLos : m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos);
	}

	public SpoilsSpawnData GetSpoilSpawnData()
	{
		return (m_cachedSpoilSpawnData == null) ? m_spoilSpawnData : m_cachedSpoilSpawnData;
	}

	public bool HitPowerups()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_hitPowerupsMod.GetModifiedValue(m_hitPowerups);
		}
		else
		{
			result = m_hitPowerups;
		}
		return result;
	}

	public bool StopOnPowerupHit()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_stopOnPowerupHitMod.GetModifiedValue(m_stopOnPowerupHit);
		}
		else
		{
			result = m_stopOnPowerupHit;
		}
		return result;
	}

	public bool IncludeSpoilsPowerups()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_includeSpoilsPowerupsMod.GetModifiedValue(m_includeSpoilsPowerups);
		}
		else
		{
			result = m_includeSpoilsPowerups;
		}
		return result;
	}

	public bool IgnorePickupTeamRestriction()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_ignorePickupTeamRestrictionMod.GetModifiedValue(m_ignorePickupTeamRestriction);
		}
		else
		{
			result = m_ignorePickupTeamRestriction;
		}
		return result;
	}

	public int GetMaxPowerupsHit()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_maxPowerupsHitMod.GetModifiedValue(m_maxPowerupsHit);
		}
		else
		{
			result = m_maxPowerupsHit;
		}
		return result;
	}

	public bool CopyBuffsOnEnemyHit()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_copyBuffsOnEnemyHitMod.GetModifiedValue(m_copyBuffsOnEnemyHit);
		}
		else
		{
			result = m_copyBuffsOnEnemyHit;
		}
		return result;
	}

	public int GetCopyBuffDuration()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_copyBuffDurationMod.GetModifiedValue(m_copyBuffDuration);
		}
		else
		{
			result = m_copyBuffDuration;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (GetExpectedNumberOfTargeters() < 2)
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
			AccumulateDamageFromTargeter(targetActor, base.Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				AccumulateDamageFromTargeter(targetActor, base.Targeters[i], dictionary);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return dictionary;
	}

	private void AccumulateDamageFromTargeter(ActorData targetActor, AbilityUtil_Targeter targeter, Dictionary<AbilityTooltipSymbol, int> symbolToDamage)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			foreach (AbilityTooltipSubject item in tooltipSubjectTypes)
			{
				if (item == AbilityTooltipSubject.Primary)
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
					if (!symbolToDamage.ContainsKey(AbilityTooltipSymbol.Damage))
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
						symbolToDamage[AbilityTooltipSymbol.Damage] = GetLaserDamageAmount();
					}
					else
					{
						symbolToDamage[AbilityTooltipSymbol.Damage] += GetLaserSubsequentDamageAmount();
					}
				}
			}
			return;
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefSpoilLaserUlt abilityMod_ThiefSpoilLaserUlt = modAsBase as AbilityMod_ThiefSpoilLaserUlt;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ThiefSpoilLaserUlt)
		{
			while (true)
			{
				switch (6)
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
			val = abilityMod_ThiefSpoilLaserUlt.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ThiefSpoilLaserUlt)
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
			val2 = abilityMod_ThiefSpoilLaserUlt.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount);
		}
		else
		{
			val2 = m_laserSubsequentDamageAmount;
		}
		AddTokenInt(tokens, "LaserSubsequentDamageAmount", empty2, val2);
		AddTokenInt(tokens, "LaserDamageTotalCombined", string.Empty, m_laserDamageAmount + m_laserSubsequentDamageAmount);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ThiefSpoilLaserUlt)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_ThiefSpoilLaserUlt.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ThiefSpoilLaserUlt)
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
			val3 = abilityMod_ThiefSpoilLaserUlt.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			val3 = m_laserMaxTargets;
		}
		AddTokenInt(tokens, "LaserMaxTargets", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ThiefSpoilLaserUlt)
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
			val4 = abilityMod_ThiefSpoilLaserUlt.m_laserCountMod.GetModifiedValue(m_laserCount);
		}
		else
		{
			val4 = m_laserCount;
		}
		AddTokenInt(tokens, "LaserCount", empty4, val4);
		AddTokenInt(tokens, "CopyBuffDuration", string.Empty, (!abilityMod_ThiefSpoilLaserUlt) ? m_copyBuffDuration : abilityMod_ThiefSpoilLaserUlt.m_copyBuffDurationMod.GetModifiedValue(m_copyBuffDuration));
		AddTokenInt(tokens, "MaxPowerupsHit", string.Empty, (!abilityMod_ThiefSpoilLaserUlt) ? m_maxPowerupsHit : abilityMod_ThiefSpoilLaserUlt.m_maxPowerupsHitMod.GetModifiedValue(m_maxPowerupsHit));
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = m_targeterMinInterpDistance * Board.Get().squareSize;
		max = m_targeterMaxInterpDistance * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ThiefSpoilLaserUlt))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_ThiefSpoilLaserUlt);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
