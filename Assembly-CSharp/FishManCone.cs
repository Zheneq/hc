using System;
using System.Collections.Generic;
using UnityEngine;

public class FishManCone : Ability
{
	public enum ConeTargetingMode
	{
		Static,
		MultiClick,
		Stretch
	}

	[Header("-- Cone Data")]
	public ConeTargetingMode m_coneMode = ConeTargetingMode.MultiClick;

	public float m_coneWidthAngle = 60f;

	public float m_coneWidthAngleMin = 5f;

	public float m_coneLength = 5f;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets;

	public float m_multiClickConeEdgeWidth = 0.2f;

	[Header("-- (for stretch cone only)")]
	public bool m_useDiscreteAngleChange;

	public float m_stretchInterpMinDist = 2.5f;

	public float m_stretchInterpRange = 4f;

	[Header("-- On Hit Target")]
	public int m_damageToEnemies;

	public int m_damageToEnemiesMax;

	public StandardEffectInfo m_effectToEnemies;

	[Header("-- Ally Healing")]
	public int m_healingToAllies = 15;

	public int m_healingToAlliesMax = 30;

	public StandardEffectInfo m_effectToAllies;

	[Header("-- Self-Healing")]
	public int m_healToCasterOnCast;

	public int m_healToCasterPerEnemyHit;

	public int m_healToCasterPerAllyHit;

	[Header("-- Bonus Healing on Heal Cone ability")]
	public int m_extraHealPerEnemyHitForNextHealCone;

	[Header("-- Extra Energy")]
	public int m_extraEnergyForSingleEnemyHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_FishManCone m_abilityMod;

	private FishMan_SyncComponent m_syncComp;

	private AreaEffectUtils.StretchConeStyle m_stretchConeStyle;

	private StandardEffectInfo m_cachedEffectToEnemies;

	private StandardEffectInfo m_cachedEffectToAllies;

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
			m_syncComp = GetComponent<FishMan_SyncComponent>();
		}
		if (m_coneMode == ConeTargetingMode.MultiClick)
		{
			ClearTargeters();
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = new AbilityUtil_Targeter_SweepMultiClickCone(this, GetConeWidthAngleMin(), GetConeWidthAngle(), GetConeLength(), GetConeBackwardOffset(), m_multiClickConeEdgeWidth, PenetrateLineOfSight(), GetMaxTargets());
				abilityUtil_Targeter_SweepMultiClickCone.SetAffectedGroups(AffectsEnemies(), AffectsAllies(), AffectsCaster());
				base.Targeters.Add(abilityUtil_Targeter_SweepMultiClickCone);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (m_coneMode == ConeTargetingMode.Stretch)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetConeLength(), GetConeLength(), GetConeWidthAngleMin(), GetConeWidthAngle(), m_stretchConeStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
			abilityUtil_Targeter_StretchCone.m_includeEnemies = AffectsEnemies();
			abilityUtil_Targeter_StretchCone.m_includeAllies = AffectsAllies();
			abilityUtil_Targeter_StretchCone.m_includeCaster = AffectsCaster();
			abilityUtil_Targeter_StretchCone.m_interpMinDistOverride = m_stretchInterpMinDist;
			abilityUtil_Targeter_StretchCone.m_interpRangeOverride = m_stretchInterpRange;
			abilityUtil_Targeter_StretchCone.m_discreteWidthAngleChange = m_useDiscreteAngleChange;
			abilityUtil_Targeter_StretchCone.m_numDiscreteWidthChanges = GetMaxDamageToEnemies() - GetDamageToEnemies();
			base.Targeter = abilityUtil_Targeter_StretchCone;
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, GetConeWidthAngle(), GetConeLength(), GetConeBackwardOffset(), PenetrateLineOfSight(), true, AffectsEnemies(), AffectsAllies(), AffectsCaster(), GetMaxTargets());
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (m_coneMode == ConeTargetingMode.MultiClick)
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
			result = 2;
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
		return GetConeLength();
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (m_coneMode == ConeTargetingMode.Stretch)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					min = m_stretchInterpMinDist * Board.Get().squareSize;
					max = (m_stretchInterpMinDist + m_stretchInterpRange) * Board.Get().squareSize;
					return true;
				}
			}
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	private bool AffectsEnemies()
	{
		int result;
		if (GetDamageToEnemies() <= 0)
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
		return GetHealingToAllies() > 0 || GetEffectToAllies().m_applyEffect;
	}

	private bool AffectsCaster()
	{
		return GetHealToCasterOnCast() > 0 || GetHealToCasterPerAllyHit() > 0 || GetHealToCasterPerEnemyHit() > 0;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectToEnemies;
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
			cachedEffectToEnemies = m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies);
		}
		else
		{
			cachedEffectToEnemies = m_effectToEnemies;
		}
		m_cachedEffectToEnemies = cachedEffectToEnemies;
		m_cachedEffectToAllies = ((!m_abilityMod) ? m_effectToAllies : m_abilityMod.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies));
	}

	public float GetConeWidthAngle()
	{
		float result;
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
		float result;
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
			result = m_abilityMod.m_coneWidthAngleMinMod.GetModifiedValue(m_coneWidthAngleMin);
		}
		else
		{
			result = m_coneWidthAngleMin;
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
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
			result = m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
		}
		else
		{
			result = m_coneLength;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
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
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
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
		return (!m_abilityMod) ? m_damageToEnemies : m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies);
	}

	public int GetMaxDamageToEnemies()
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
		return (m_cachedEffectToEnemies == null) ? m_effectToEnemies : m_cachedEffectToEnemies;
	}

	public int GetHealingToAllies()
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
			result = m_cachedEffectToAllies;
		}
		else
		{
			result = m_effectToAllies;
		}
		return result;
	}

	public int GetHealToCasterOnCast()
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
			result = m_abilityMod.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast);
		}
		else
		{
			result = m_healToCasterOnCast;
		}
		return result;
	}

	public int GetHealToCasterPerEnemyHit()
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
			result = m_abilityMod.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit);
		}
		else
		{
			result = m_healToCasterPerEnemyHit;
		}
		return result;
	}

	public int GetHealToCasterPerAllyHit()
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
			result = m_abilityMod.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit);
		}
		else
		{
			result = m_healToCasterPerAllyHit;
		}
		return result;
	}

	public int GetExtraHealPerEnemyHitForNextHealCone()
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
			result = m_abilityMod.m_extraHealPerEnemyHitForNextHealConeMod.GetModifiedValue(m_extraHealPerEnemyHitForNextHealCone);
		}
		else
		{
			result = m_extraHealPerEnemyHitForNextHealCone;
		}
		return result;
	}

	public int GetExtraEnergyForSingleEnemyHit()
	{
		return (!m_abilityMod) ? m_extraEnergyForSingleEnemyHit : m_abilityMod.m_extraEnergyForSingleEnemyHitMod.GetModifiedValue(m_extraEnergyForSingleEnemyHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManCone))
		{
			m_abilityMod = (abilityMod as AbilityMod_FishManCone);
			Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManCone abilityMod_FishManCone = modAsBase as AbilityMod_FishManCone;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManCone)
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
			val = abilityMod_FishManCone.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies);
		}
		else
		{
			val = m_damageToEnemies;
		}
		AddTokenInt(tokens, "DamageToEnemies", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManCone)
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
			val2 = abilityMod_FishManCone.m_damageToEnemiesMaxMod.GetModifiedValue(m_damageToEnemiesMax);
		}
		else
		{
			val2 = m_damageToEnemiesMax;
		}
		AddTokenInt(tokens, "DamageToEnemiesMax", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManCone)
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
			effectInfo = abilityMod_FishManCone.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies);
		}
		else
		{
			effectInfo = m_effectToEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectToEnemies", m_effectToEnemies);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManCone)
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
			val3 = abilityMod_FishManCone.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies);
		}
		else
		{
			val3 = m_healingToAllies;
		}
		AddTokenInt(tokens, "HealingToAllies", empty3, val3);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_FishManCone)
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
			effectInfo2 = abilityMod_FishManCone.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies);
		}
		else
		{
			effectInfo2 = m_effectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectToAllies", m_effectToAllies);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_FishManCone)
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
			val4 = abilityMod_FishManCone.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			val4 = m_maxTargets;
		}
		AddTokenInt(tokens, "MaxTargets", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_FishManCone)
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
			val5 = abilityMod_FishManCone.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast);
		}
		else
		{
			val5 = m_healToCasterOnCast;
		}
		AddTokenInt(tokens, "HealToCasterOnCast", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_FishManCone)
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
			val6 = abilityMod_FishManCone.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit);
		}
		else
		{
			val6 = m_healToCasterPerEnemyHit;
		}
		AddTokenInt(tokens, "HealToCasterPerEnemyHit", empty6, val6);
		string empty7 = string.Empty;
		int val7;
		if ((bool)abilityMod_FishManCone)
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
			val7 = abilityMod_FishManCone.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit);
		}
		else
		{
			val7 = m_healToCasterPerAllyHit;
		}
		AddTokenInt(tokens, "HealToCasterPerAllyHit", empty7, val7);
		string empty8 = string.Empty;
		int val8;
		if ((bool)abilityMod_FishManCone)
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
			val8 = abilityMod_FishManCone.m_extraHealPerEnemyHitForNextHealConeMod.GetModifiedValue(m_extraHealPerEnemyHitForNextHealCone);
		}
		else
		{
			val8 = m_extraHealPerEnemyHitForNextHealCone;
		}
		AddTokenInt(tokens, "ExtraHealPerEnemyHitForNextHealCone", empty8, val8);
		string empty9 = string.Empty;
		int val9;
		if ((bool)abilityMod_FishManCone)
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
			val9 = abilityMod_FishManCone.m_extraEnergyForSingleEnemyHitMod.GetModifiedValue(m_extraEnergyForSingleEnemyHit);
		}
		else
		{
			val9 = m_extraEnergyForSingleEnemyHit;
		}
		AddTokenInt(tokens, "ExtraEnergyForSingleEnemyHit", empty9, val9);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageToEnemies()));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, GetHealingToAllies()));
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAllies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() + GetHealToCasterPerAllyHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (currentTargeterIndex <= 0)
		{
			if (m_coneMode == ConeTargetingMode.MultiClick)
			{
				goto IL_0204;
			}
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
		}
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
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
				int visibleActorsCountByTooltipSubject = base.Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int visibleActorsCountByTooltipSubject2 = base.Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int num = GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() * visibleActorsCountByTooltipSubject + GetHealToCasterPerAllyHit() * visibleActorsCountByTooltipSubject2;
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(num);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				int value = GetDamageToEnemies();
				if (m_coneMode == ConeTargetingMode.MultiClick)
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
					AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
					value = GetDamageForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone.sweepAngle);
				}
				else if (m_coneMode == ConeTargetingMode.Stretch)
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
					if (abilityUtil_Targeter_StretchCone != null)
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
						value = GetDamageForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = value;
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
				int value2 = GetHealingToAllies();
				if (m_coneMode == ConeTargetingMode.MultiClick)
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
					AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone2 = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
					value2 = GetHealingForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone2.sweepAngle);
				}
				else if (m_coneMode == ConeTargetingMode.Stretch && abilityUtil_Targeter_StretchCone != null)
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
					value2 = GetHealingForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				}
				dictionary[AbilityTooltipSymbol.Healing] = value2;
			}
		}
		goto IL_0204;
		IL_0204:
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetExtraEnergyForSingleEnemyHit() > 0 && (currentTargeterIndex > 0 || m_coneMode != ConeTargetingMode.MultiClick))
		{
			AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[currentTargeterIndex];
			if (abilityUtil_Targeter != null)
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
				int visibleActorsCountByTooltipSubject = abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (visibleActorsCountByTooltipSubject == 1)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return GetExtraEnergyForSingleEnemyHit();
						}
					}
				}
			}
		}
		return 0;
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float coneWidthAngle = GetConeWidthAngle();
		float coneWidthAngleMin = GetConeWidthAngleMin();
		if (coneWidthAngle > 0f)
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
			if (sweepAngle > coneWidthAngle)
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
				endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - coneWidthAngle), 0f);
				sweepAngle = coneWidthAngle;
				goto IL_00a3;
			}
		}
		if (coneWidthAngleMin > 0f && sweepAngle < coneWidthAngleMin)
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
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - coneWidthAngleMin), 0f);
			sweepAngle = coneWidthAngleMin;
		}
		goto IL_00a3;
		IL_00a3:
		coneCenterDegrees = num;
		Vector3 vector = Vector3.Cross(startAimDirection, endAimDirection);
		if (vector.y > 0f)
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
			coneCenterDegrees -= sweepAngle * 0.5f;
		}
		else
		{
			coneCenterDegrees += sweepAngle * 0.5f;
		}
		return endAimDirection;
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		if (m_coneMode == ConeTargetingMode.MultiClick)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					float sweepAngle = GetConeWidthAngleMin();
					float num = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
					float coneCenterDegrees = num;
					if (targets.Count > 1)
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
						GetTargeterClampedAimDirection(targets[0].AimDirection, targets[targets.Count - 1].AimDirection, out sweepAngle, out coneCenterDegrees);
					}
					return caster.GetTravelBoardSquareWorldPosition() + VectorUtils.AngleDegreesToVector(coneCenterDegrees);
				}
				}
			}
		}
		return base.GetRotateToTargetPos(targets, caster);
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
		float num3;
		if (num2 > 0f)
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
			num3 = 1f - (sweepAngle - GetConeWidthAngleMin()) / num2;
		}
		else
		{
			num3 = 1f;
		}
		float value = num3;
		value = Mathf.Clamp(value, 0f, 1f);
		return GetHealingToAllies() + Mathf.RoundToInt(num * value);
	}
}
