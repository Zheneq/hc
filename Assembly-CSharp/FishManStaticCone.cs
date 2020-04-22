using System.Collections.Generic;
using UnityEngine;

public class FishManStaticCone : Ability
{
	[Header("-- Cone Data")]
	public float m_coneWidthAngle = 60f;

	public float m_coneWidthAngleMin = 5f;

	public float m_coneLength = 5f;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets;

	[Header("-- (for stretch cone only)")]
	public bool m_useDiscreteAngleChange;

	public float m_stretchInterpMinDist = 2.5f;

	public float m_stretchInterpRange = 4f;

	[Header("-- On Hit Target")]
	public int m_damageToEnemies;

	public int m_damageToEnemiesMax;

	public StandardEffectInfo m_effectToEnemies;

	[Space(10f)]
	public int m_healingToAllies = 15;

	public int m_healingToAlliesMax = 25;

	public StandardEffectInfo m_effectToAllies;

	public int m_extraAllyHealForSingleHit;

	public StandardEffectInfo m_extraEffectOnClosestAlly;

	[Header("-- Self-Healing")]
	public int m_healToCasterOnCast;

	public int m_healToCasterPerEnemyHit;

	public int m_healToCasterPerAllyHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_FishManStaticCone m_abilityMod;

	private FishMan_SyncComponent m_syncComp;

	private FishManCone m_damageConeAbility;

	private AreaEffectUtils.StretchConeStyle m_stretchConeStyle;

	private StandardEffectInfo m_cachedEffectToEnemies;

	private StandardEffectInfo m_cachedEffectToAllies;

	private StandardEffectInfo m_cachedExtraEffectOnClosestAlly;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
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
			m_syncComp = GetComponent<FishMan_SyncComponent>();
		}
		if (m_damageConeAbility == null)
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
			AbilityData component = GetComponent<AbilityData>();
			if (component != null)
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
				m_damageConeAbility = (component.GetAbilityOfType(typeof(FishManCone)) as FishManCone);
			}
		}
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetConeLength(), GetConeLength(), GetConeWidthAngleMin(), GetConeWidthAngle(), m_stretchConeStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
		abilityUtil_Targeter_StretchCone.m_includeEnemies = AffectsEnemies();
		abilityUtil_Targeter_StretchCone.m_includeAllies = AffectsAllies();
		abilityUtil_Targeter_StretchCone.m_includeCaster = AffectsCaster();
		abilityUtil_Targeter_StretchCone.m_interpMinDistOverride = m_stretchInterpMinDist;
		abilityUtil_Targeter_StretchCone.m_interpRangeOverride = m_stretchInterpRange;
		abilityUtil_Targeter_StretchCone.m_discreteWidthAngleChange = m_useDiscreteAngleChange;
		abilityUtil_Targeter_StretchCone.m_numDiscreteWidthChanges = GetMaxHealingToAllies() - GetHealingToAllies();
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	private bool AffectsEnemies()
	{
		int result;
		if (GetDamageToEnemies() <= 0)
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
			result = (GetEffectToEnemies().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool AffectsAllies()
	{
		int result;
		if (GetHealingToAllies() <= 0)
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
			result = (GetEffectToAllies().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool AffectsCaster()
	{
		int result;
		if (GetHealToCasterOnCast() <= 0 && GetHealToCasterPerAllyHit() <= 0)
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
			result = ((GetHealToCasterPerEnemyHit() > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectToEnemies;
		if ((bool)m_abilityMod)
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
			cachedEffectToEnemies = m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies);
		}
		else
		{
			cachedEffectToEnemies = m_effectToEnemies;
		}
		m_cachedEffectToEnemies = cachedEffectToEnemies;
		StandardEffectInfo cachedEffectToAllies;
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
			cachedEffectToAllies = m_abilityMod.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies);
		}
		else
		{
			cachedEffectToAllies = m_effectToAllies;
		}
		m_cachedEffectToAllies = cachedEffectToAllies;
		StandardEffectInfo cachedExtraEffectOnClosestAlly;
		if ((bool)m_abilityMod)
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
			cachedExtraEffectOnClosestAlly = m_abilityMod.m_extraEffectOnClosestAllyMod.GetModifiedValue(m_extraEffectOnClosestAlly);
		}
		else
		{
			cachedExtraEffectOnClosestAlly = m_extraEffectOnClosestAlly;
		}
		m_cachedExtraEffectOnClosestAlly = cachedExtraEffectOnClosestAlly;
	}

	public float GetConeWidthAngle()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		else
		{
			result = m_coneWidthAngle;
		}
		return result;
	}

	public float GetConeWidthAngleMin()
	{
		return (!m_abilityMod) ? m_coneWidthAngleMin : m_abilityMod.m_coneWidthAngleMinMod.GetModifiedValue(m_coneWidthAngleMin);
	}

	public float GetConeLength()
	{
		return (!m_abilityMod) ? m_coneLength : m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
	}

	public float GetConeBackwardOffset()
	{
		return (!m_abilityMod) ? m_coneBackwardOffset : m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public int GetMaxTargets()
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
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public int GetDamageToEnemies()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies);
		}
		else
		{
			result = m_damageToEnemies;
		}
		return result;
	}

	public int GetMaxDamageToEnemies()
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
			result = m_abilityMod.m_damageToEnemiesMaxMod.GetModifiedValue(m_damageToEnemiesMax);
		}
		else
		{
			result = m_damageToEnemiesMax;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToEnemies != null)
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
			result = m_cachedEffectToEnemies;
		}
		else
		{
			result = m_effectToEnemies;
		}
		return result;
	}

	public int GetHealingToAllies()
	{
		int result;
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
			result = m_abilityMod.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies);
		}
		else
		{
			result = m_healingToAllies;
		}
		return result;
	}

	public int GetMaxHealingToAllies()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_healingToAlliesMaxMod.GetModifiedValue(m_healingToAlliesMax);
		}
		else
		{
			result = m_healingToAlliesMax;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToAllies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToAllies != null)
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
			result = m_cachedEffectToAllies;
		}
		else
		{
			result = m_effectToAllies;
		}
		return result;
	}

	public int GetExtraAllyHealForSingleHit()
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
			result = m_abilityMod.m_extraAllyHealForSingleHitMod.GetModifiedValue(m_extraAllyHealForSingleHit);
		}
		else
		{
			result = m_extraAllyHealForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnClosestAlly()
	{
		return (m_cachedExtraEffectOnClosestAlly == null) ? m_extraEffectOnClosestAlly : m_cachedExtraEffectOnClosestAlly;
	}

	public int GetHealToCasterOnCast()
	{
		return (!m_abilityMod) ? m_healToCasterOnCast : m_abilityMod.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast);
	}

	public int GetHealToCasterPerEnemyHit()
	{
		return (!m_abilityMod) ? m_healToCasterPerEnemyHit : m_abilityMod.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit);
	}

	public int GetHealToCasterPerAllyHit()
	{
		return (!m_abilityMod) ? m_healToCasterPerAllyHit : m_abilityMod.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit);
	}

	public int GetExtraAllyHealFromBasicAttack()
	{
		if (m_damageConeAbility != null)
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
			if (m_syncComp != null)
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
				if (m_syncComp.m_lastBasicAttackEnemyHitCount > 0)
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
					if (m_damageConeAbility.GetExtraHealPerEnemyHitForNextHealCone() > 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return m_syncComp.m_lastBasicAttackEnemyHitCount * m_damageConeAbility.GetExtraHealPerEnemyHitForNextHealCone();
							}
						}
					}
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManStaticCone abilityMod_FishManStaticCone = modAsBase as AbilityMod_FishManStaticCone;
		AddTokenInt(tokens, "DamageToEnemies", string.Empty, (!abilityMod_FishManStaticCone) ? m_damageToEnemies : abilityMod_FishManStaticCone.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies));
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManStaticCone) ? m_effectToEnemies : abilityMod_FishManStaticCone.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies), "EffectToEnemies", m_effectToEnemies);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManStaticCone)
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
			val = abilityMod_FishManStaticCone.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies);
		}
		else
		{
			val = m_healingToAllies;
		}
		AddTokenInt(tokens, "HealingToAllies", empty, val);
		AddTokenInt(tokens, "HealingToAlliesMax", string.Empty, (!abilityMod_FishManStaticCone) ? m_healingToAlliesMax : abilityMod_FishManStaticCone.m_healingToAlliesMaxMod.GetModifiedValue(m_healingToAlliesMax));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManStaticCone)
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
			effectInfo = abilityMod_FishManStaticCone.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies);
		}
		else
		{
			effectInfo = m_effectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectToAllies", m_effectToAllies);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManStaticCone)
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
			val2 = abilityMod_FishManStaticCone.m_extraAllyHealForSingleHitMod.GetModifiedValue(m_extraAllyHealForSingleHit);
		}
		else
		{
			val2 = m_extraAllyHealForSingleHit;
		}
		AddTokenInt(tokens, "ExtraAllyHealForSingleHit", empty2, val2);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManStaticCone) ? m_extraEffectOnClosestAlly : abilityMod_FishManStaticCone.m_extraEffectOnClosestAllyMod.GetModifiedValue(m_extraEffectOnClosestAlly), "ExtraEffectOnClosestAlly", m_extraEffectOnClosestAlly);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManStaticCone)
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
			val3 = abilityMod_FishManStaticCone.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			val3 = m_maxTargets;
		}
		AddTokenInt(tokens, "MaxTargets", empty3, val3);
		AddTokenInt(tokens, "HealToCasterOnCast", string.Empty, (!abilityMod_FishManStaticCone) ? m_healToCasterOnCast : abilityMod_FishManStaticCone.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast));
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_FishManStaticCone)
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
			val4 = abilityMod_FishManStaticCone.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit);
		}
		else
		{
			val4 = m_healToCasterPerEnemyHit;
		}
		AddTokenInt(tokens, "HealToCasterPerEnemyHit", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_FishManStaticCone)
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
			val5 = abilityMod_FishManStaticCone.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit);
		}
		else
		{
			val5 = m_healToCasterPerAllyHit;
		}
		AddTokenInt(tokens, "HealToCasterPerAllyHit", empty5, val5);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAllies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() + GetHealToCasterPerAllyHit());
		GetEffectToAllies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEffectToEnemies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				int visibleActorsCountByTooltipSubject = base.Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int visibleActorsCountByTooltipSubject2 = base.Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int num = GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() * visibleActorsCountByTooltipSubject + GetHealToCasterPerAllyHit() * visibleActorsCountByTooltipSubject2;
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(num);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				int num2 = dictionary[AbilityTooltipSymbol.Damage] = GetDamageForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
				int visibleActorsCountByTooltipSubject3 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int healingForSweepAngle = GetHealingForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				healingForSweepAngle += GetExtraAllyHealFromBasicAttack();
				if (visibleActorsCountByTooltipSubject3 == 1)
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
					healingForSweepAngle += GetExtraAllyHealForSingleHit();
				}
				dictionary[AbilityTooltipSymbol.Healing] = healingForSweepAngle;
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManStaticCone))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_FishManStaticCone);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private int GetDamageForSweepAngle(float sweepAngle)
	{
		float num = GetMaxDamageToEnemies() - GetDamageToEnemies();
		float num2 = GetConeWidthAngle() - GetConeWidthAngleMin();
		float num3;
		if (num2 > 0f)
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
			num3 = 1f - (sweepAngle - GetConeWidthAngleMin()) / num2;
		}
		else
		{
			num3 = 1f;
		}
		float value = num3;
		value = Mathf.Clamp(value, 0f, 1f);
		return GetDamageToEnemies() + Mathf.RoundToInt(num * value);
	}

	private int GetHealingForSweepAngle(float sweepAngle)
	{
		float num = GetMaxHealingToAllies() - GetHealingToAllies();
		float num2 = GetConeWidthAngle() - GetConeWidthAngleMin();
		float value = (!(num2 > 0f)) ? 1f : (1f - (sweepAngle - GetConeWidthAngleMin()) / num2);
		value = Mathf.Clamp(value, 0f, 1f);
		return GetHealingToAllies() + Mathf.RoundToInt(num * value);
	}
}
