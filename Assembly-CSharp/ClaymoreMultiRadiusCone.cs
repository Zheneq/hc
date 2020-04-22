using System.Collections.Generic;
using UnityEngine;

public class ClaymoreMultiRadiusCone : Ability
{
	[Header("-- Cone Targeting")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLengthInner = 1.5f;

	public float m_coneLengthMiddle = 2.5f;

	public float m_coneLengthOuter = 3.5f;

	public bool m_penetrateLineOfSight;

	[Header("-- Base Damage")]
	public int m_damageAmountInner = 5;

	public int m_damageAmountMiddle = 4;

	public int m_damageAmountOuter = 3;

	[Header("-- Bonus Damage, (threshold value range 0 to 1)")]
	public int m_bonusDamageIfEnemyLowHealth;

	public float m_enemyHealthThreshForBonus = -1f;

	public int m_bonusDamageIfCasterLowHealth;

	public float m_casterHealthThreshForBonus = -1f;

	[Header("-- Hit Effects")]
	public StandardEffectInfo m_effectInner;

	public StandardEffectInfo m_effectMiddle;

	public StandardEffectInfo m_effectOuter;

	[Header("-- Energy Gain on Self for Hits")]
	public int m_tpGainInner;

	public int m_tpGainMiddle;

	public int m_tpGainOuter;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreMultiRadiusCone m_abilityMod;

	private Claymore_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEffectInner;

	private StandardEffectInfo m_cachedEffectMiddle;

	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Mountain Cleaver";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedOuterRadius();
	}

	private float ModdedConeAngle()
	{
		float result;
		if (m_abilityMod == null)
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
			result = m_coneWidthAngle;
		}
		else
		{
			result = m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		return result;
	}

	private float ModdedInnerRadius()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_coneInnerRadiusMod.GetModifiedValue(m_coneLengthInner) : m_coneLengthInner;
	}

	private float ModdedMiddleRadius()
	{
		float result;
		if (m_abilityMod == null)
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
			result = m_coneLengthMiddle;
		}
		else
		{
			result = m_abilityMod.m_coneMiddleRadiusMod.GetModifiedValue(m_coneLengthMiddle);
		}
		return result;
	}

	private float ModdedOuterRadius()
	{
		float result;
		if (m_abilityMod == null)
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
			result = m_coneLengthOuter;
		}
		else
		{
			result = m_abilityMod.m_coneOuterRadiusMod.GetModifiedValue(m_coneLengthOuter);
		}
		return result;
	}

	private bool GetPenetrateLineOfSight()
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
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	private int ModdedInnerDamage()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damageAmountInner;
		}
		else
		{
			result = m_abilityMod.m_innerDamageMod.GetModifiedValue(m_damageAmountInner);
		}
		return result;
	}

	private int ModdedMiddleDamage()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damageAmountMiddle;
		}
		else
		{
			result = m_abilityMod.m_middleDamageMod.GetModifiedValue(m_damageAmountMiddle);
		}
		return result;
	}

	private int ModdedOuterDamage()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damageAmountOuter;
		}
		else
		{
			result = m_abilityMod.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter);
		}
		return result;
	}

	private int ModdedInnerTpGain()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_tpGainInner;
		}
		else
		{
			result = m_abilityMod.m_innerTpGain.GetModifiedValue(m_tpGainInner);
		}
		return result;
	}

	private int ModdedMiddleTpGain()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_tpGainMiddle;
		}
		else
		{
			result = m_abilityMod.m_middleTpGain.GetModifiedValue(m_tpGainMiddle);
		}
		return result;
	}

	private int ModdedOuterTpGain()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_tpGainOuter;
		}
		else
		{
			result = m_abilityMod.m_outerTpGain.GetModifiedValue(m_tpGainOuter);
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectInner;
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
			cachedEffectInner = m_abilityMod.m_effectInnerMod.GetModifiedValue(m_effectInner);
		}
		else
		{
			cachedEffectInner = m_effectInner;
		}
		m_cachedEffectInner = cachedEffectInner;
		StandardEffectInfo cachedEffectMiddle;
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
			cachedEffectMiddle = m_abilityMod.m_effectMiddleMod.GetModifiedValue(m_effectMiddle);
		}
		else
		{
			cachedEffectMiddle = m_effectMiddle;
		}
		m_cachedEffectMiddle = cachedEffectMiddle;
		StandardEffectInfo cachedEffectOuter;
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
			cachedEffectOuter = m_abilityMod.m_effectOuterMod.GetModifiedValue(m_effectOuter);
		}
		else
		{
			cachedEffectOuter = m_effectOuter;
		}
		m_cachedEffectOuter = cachedEffectOuter;
	}

	public StandardEffectInfo GetEffectInner()
	{
		return (m_cachedEffectInner == null) ? m_effectInner : m_cachedEffectInner;
	}

	public StandardEffectInfo GetEffectMiddle()
	{
		StandardEffectInfo result;
		if (m_cachedEffectMiddle != null)
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
			result = m_cachedEffectMiddle;
		}
		else
		{
			result = m_effectMiddle;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOuter != null)
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
			result = m_cachedEffectOuter;
		}
		else
		{
			result = m_effectOuter;
		}
		return result;
	}

	public int GetBonusDamageIfEnemyHealthBelow()
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
			result = m_abilityMod.m_bonusDamageIfEnemyLowHealthMod.GetModifiedValue(m_bonusDamageIfEnemyLowHealth);
		}
		else
		{
			result = m_bonusDamageIfEnemyLowHealth;
		}
		return result;
	}

	public float GetEnemyHealthThreshForBonus()
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
			result = m_abilityMod.m_enemyHealthThreshForBonusMod.GetModifiedValue(m_enemyHealthThreshForBonus);
		}
		else
		{
			result = m_enemyHealthThreshForBonus;
		}
		return result;
	}

	public int GetBonusDamageIfCasterHealthBelow()
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
			result = m_abilityMod.m_bonusDamageIfCasterLowHealthMod.GetModifiedValue(m_bonusDamageIfCasterLowHealth);
		}
		else
		{
			result = m_bonusDamageIfCasterLowHealth;
		}
		return result;
	}

	public float GetCasterHealthThreshForBonus()
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
			result = m_abilityMod.m_casterHealthThreshForBonusMod.GetModifiedValue(m_casterHealthThreshForBonus);
		}
		else
		{
			result = m_casterHealthThreshForBonus;
		}
		return result;
	}

	public bool ShouldApplyCasterBonusPerThresholdReached()
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
			result = (m_abilityMod.m_applyBonusPerThresholdReached ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		SetCachedFields();
		float angle = ModdedConeAngle();
		List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedInnerRadius()));
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedMiddleRadius()));
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedOuterRadius()));
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, list, m_coneBackwardOffset, GetPenetrateLineOfSight(), true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreMultiRadiusCone abilityMod_ClaymoreMultiRadiusCone = modAsBase as AbilityMod_ClaymoreMultiRadiusCone;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			val = abilityMod_ClaymoreMultiRadiusCone.m_innerDamageMod.GetModifiedValue(m_damageAmountInner);
		}
		else
		{
			val = m_damageAmountInner;
		}
		AddTokenInt(tokens, "DamageAmountInner", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			val2 = abilityMod_ClaymoreMultiRadiusCone.m_middleDamageMod.GetModifiedValue(m_damageAmountMiddle);
		}
		else
		{
			val2 = m_damageAmountMiddle;
		}
		AddTokenInt(tokens, "DamageAmountMiddle", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			val3 = abilityMod_ClaymoreMultiRadiusCone.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter);
		}
		else
		{
			val3 = m_damageAmountOuter;
		}
		AddTokenInt(tokens, "DamageAmountOuter", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			val4 = abilityMod_ClaymoreMultiRadiusCone.m_innerTpGain.GetModifiedValue(m_tpGainInner);
		}
		else
		{
			val4 = m_tpGainInner;
		}
		AddTokenInt(tokens, "TpGainInner", empty4, val4);
		AddTokenInt(tokens, "TpGainMiddle", string.Empty, (!abilityMod_ClaymoreMultiRadiusCone) ? m_tpGainMiddle : abilityMod_ClaymoreMultiRadiusCone.m_middleTpGain.GetModifiedValue(m_tpGainMiddle));
		AddTokenInt(tokens, "TpGainOuter", string.Empty, (!abilityMod_ClaymoreMultiRadiusCone) ? m_tpGainOuter : abilityMod_ClaymoreMultiRadiusCone.m_outerTpGain.GetModifiedValue(m_tpGainOuter));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			effectInfo = abilityMod_ClaymoreMultiRadiusCone.m_effectInnerMod.GetModifiedValue(m_effectInner);
		}
		else
		{
			effectInfo = m_effectInner;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectInner", m_effectInner);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			effectInfo2 = abilityMod_ClaymoreMultiRadiusCone.m_effectMiddleMod.GetModifiedValue(m_effectMiddle);
		}
		else
		{
			effectInfo2 = m_effectMiddle;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectMiddle", m_effectMiddle);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_ClaymoreMultiRadiusCone)
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
			effectInfo3 = abilityMod_ClaymoreMultiRadiusCone.m_effectOuterMod.GetModifiedValue(m_effectOuter);
		}
		else
		{
			effectInfo3 = m_effectOuter;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "EffectOuter", m_effectOuter);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, m_damageAmountInner));
		GetEffectInner().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Near);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Midranged, m_damageAmountMiddle));
		GetEffectMiddle().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Midranged);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, m_damageAmountOuter));
		GetEffectOuter().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Far);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			ActorData actorData = base.ActorData;
			int num = 0;
			if (GetBonusDamageIfCasterHealthBelow() > 0)
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
				if (GetCasterHealthThreshForBonus() > 0f)
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
					float num2 = (float)actorData.HitPoints / (float)actorData.GetMaxHitPoints();
					if (ShouldApplyCasterBonusPerThresholdReached())
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
						int num3 = Mathf.FloorToInt((1f - num2) / GetCasterHealthThreshForBonus());
						num += GetBonusDamageIfCasterHealthBelow() * num3;
					}
					else if (num2 < GetCasterHealthThreshForBonus())
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
						num += GetBonusDamageIfCasterHealthBelow();
					}
				}
			}
			if (GetBonusDamageIfEnemyHealthBelow() > 0 && GetEnemyHealthThreshForBonus() > 0f)
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
				if ((float)targetActor.HitPoints / (float)targetActor.GetMaxHitPoints() < GetEnemyHealthThreshForBonus())
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
					num += GetBonusDamageIfEnemyHealthBelow();
				}
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
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
				dictionary[AbilityTooltipSymbol.Damage] = ModdedInnerDamage() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Midranged))
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
				dictionary[AbilityTooltipSymbol.Damage] = ModdedMiddleDamage() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
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
				dictionary[AbilityTooltipSymbol.Damage] = ModdedOuterDamage() + num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (ModdedInnerTpGain() > 0)
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
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Near);
			num += visibleActorsInRangeByTooltipSubject.Count * ModdedInnerTpGain();
		}
		if (ModdedMiddleTpGain() > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject2 = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Midranged);
			num += visibleActorsInRangeByTooltipSubject2.Count * ModdedMiddleTpGain();
		}
		if (ModdedOuterTpGain() > 0)
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
			List<ActorData> visibleActorsInRangeByTooltipSubject3 = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Far);
			num += visibleActorsInRangeByTooltipSubject3.Count * ModdedOuterTpGain();
		}
		return num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
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
			if (subjectType != AbilityTooltipSubject.Midranged)
			{
				if (subjectType != AbilityTooltipSubject.Far)
				{
					return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
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
			}
		}
		float num = ModdedInnerRadius() * Board.Get().squareSize;
		float num2 = ModdedMiddleRadius() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - damageOrigin;
		vector.y = 0f;
		float num3 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num3 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		bool flag = num3 <= num;
		int num4;
		if (!flag)
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
			num4 = ((num3 <= num2) ? 1 : 0);
		}
		else
		{
			num4 = 0;
		}
		bool flag2 = (byte)num4 != 0;
		int num5;
		if (!flag)
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
			num5 = ((!flag2) ? 1 : 0);
		}
		else
		{
			num5 = 0;
		}
		bool result = (byte)num5 != 0;
		if (subjectType == AbilityTooltipSubject.Near)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return flag;
				}
			}
		}
		if (subjectType == AbilityTooltipSubject.Midranged)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return flag2;
				}
			}
		}
		return result;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return (!(m_syncComp != null)) ? null : m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreMultiRadiusCone))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_ClaymoreMultiRadiusCone);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
