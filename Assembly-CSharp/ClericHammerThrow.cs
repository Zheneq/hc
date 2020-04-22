using System;
using System.Collections.Generic;
using UnityEngine;

public class ClericHammerThrow : Ability
{
	[Serializable]
	public class RadiusToHitData : RadiusToDataBase
	{
		public int m_damage;

		public StandardEffectInfo m_hitEffectInfo;

		public RadiusToHitData(float radiusInSquares, int damage, StandardEffectInfo hitEffect)
		{
			m_radius = radiusInSquares;
			m_damage = damage;
			m_hitEffectInfo = hitEffect;
		}
	}

	[Separator("Targeting", true)]
	public float m_maxDistToRingCenter = 5.5f;

	public float m_outerRadius = 2.5f;

	public float m_innerRadius = 1f;

	public bool m_ignoreLos;

	public bool m_clampRingToCursorPos = true;

	[Separator("On Hit", true)]
	public int m_outerHitDamage = 15;

	public StandardEffectInfo m_outerEnemyHitEffect;

	public int m_innerHitDamage = 20;

	public StandardEffectInfo m_innerEnemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private List<RadiusToHitData> m_cachedRadiusToHitData = new List<RadiusToHitData>();

	private AbilityMod_ClericHammerThrow m_abilityMod;

	private Cleric_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedOuterEnemyHitEffect;

	private StandardEffectInfo m_cachedInnerEnemyHitEffect;

	private StandardEffectInfo m_cachedOuterEnemyHitEffectWithNoInnerHits;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "ClericHammerThrow";
		}
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_cachedRadiusToHitData.Clear();
		m_cachedRadiusToHitData.Add(new RadiusToHitData(GetInnerRadius(), GetInnerHitDamage(), GetInnerEnemyHitEffect()));
		m_cachedRadiusToHitData.Add(new RadiusToHitData(GetOuterRadius(), GetOuterHitDamage(), GetOuterEnemyHitEffect()));
		m_cachedRadiusToHitData.Sort();
		AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = (AbilityUtil_Targeter_MartyrLaser)(base.Targeter = new AbilityUtil_Targeter_MartyrLaser(this, 0f, GetMaxDistToRingCenter(), false, -1, true, false, false, true, false, GetOuterRadius(), GetInnerRadius(), false, true, false));
		base.Targeter.SetShowArcToShape(true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "OuterHitDamage", string.Empty, m_outerHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_outerEnemyHitEffect, "OuterEnemyHitEffect", m_outerEnemyHitEffect);
		AddTokenInt(tokens, "InnerHitDamage", string.Empty, m_innerHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_innerEnemyHitEffect, "InnerEnemyHitEffect", m_innerEnemyHitEffect);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOuterEnemyHitEffect;
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
			cachedOuterEnemyHitEffect = m_abilityMod.m_outerEnemyHitEffectMod.GetModifiedValue(m_outerEnemyHitEffect);
		}
		else
		{
			cachedOuterEnemyHitEffect = m_outerEnemyHitEffect;
		}
		m_cachedOuterEnemyHitEffect = cachedOuterEnemyHitEffect;
		StandardEffectInfo cachedInnerEnemyHitEffect;
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
			cachedInnerEnemyHitEffect = m_abilityMod.m_innerEnemyHitEffectMod.GetModifiedValue(m_innerEnemyHitEffect);
		}
		else
		{
			cachedInnerEnemyHitEffect = m_innerEnemyHitEffect;
		}
		m_cachedInnerEnemyHitEffect = cachedInnerEnemyHitEffect;
		object cachedOuterEnemyHitEffectWithNoInnerHits;
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
			cachedOuterEnemyHitEffectWithNoInnerHits = m_abilityMod.m_outerEnemyHitEffectWithNoInnerHits.GetModifiedValue(null);
		}
		else
		{
			cachedOuterEnemyHitEffectWithNoInnerHits = null;
		}
		m_cachedOuterEnemyHitEffectWithNoInnerHits = (StandardEffectInfo)cachedOuterEnemyHitEffectWithNoInnerHits;
	}

	public float GetMaxDistToRingCenter()
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
			result = m_abilityMod.m_maxDistToRingCenterMod.GetModifiedValue(m_maxDistToRingCenter);
		}
		else
		{
			result = m_maxDistToRingCenter;
		}
		return result;
	}

	public float GetOuterRadius()
	{
		float result;
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
			result = m_abilityMod.m_outerRadiusMod.GetModifiedValue(m_outerRadius);
		}
		else
		{
			result = m_outerRadius;
		}
		return result;
	}

	public float GetInnerRadius()
	{
		return (!m_abilityMod) ? m_innerRadius : m_abilityMod.m_innerRadiusMod.GetModifiedValue(m_innerRadius);
	}

	public bool IgnoreLos()
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
			result = m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos);
		}
		else
		{
			result = m_ignoreLos;
		}
		return result;
	}

	public bool ClampRingToCursorPos()
	{
		bool result;
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
			result = m_abilityMod.m_clampRingToCursorPosMod.GetModifiedValue(m_clampRingToCursorPos);
		}
		else
		{
			result = m_clampRingToCursorPos;
		}
		return result;
	}

	public int GetOuterHitDamage()
	{
		return (!m_abilityMod) ? m_outerHitDamage : m_abilityMod.m_outerHitDamageMod.GetModifiedValue(m_outerHitDamage);
	}

	public StandardEffectInfo GetOuterEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedOuterEnemyHitEffect != null)
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
			result = m_cachedOuterEnemyHitEffect;
		}
		else
		{
			result = m_outerEnemyHitEffect;
		}
		return result;
	}

	public int GetInnerHitDamage()
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
			result = m_abilityMod.m_innerHitDamageMod.GetModifiedValue(m_innerHitDamage);
		}
		else
		{
			result = m_innerHitDamage;
		}
		return result;
	}

	public StandardEffectInfo GetInnerEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedInnerEnemyHitEffect != null)
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
			result = m_cachedInnerEnemyHitEffect;
		}
		else
		{
			result = m_innerEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetOuterEnemyHitEffectWithNoInnerHits()
	{
		object result;
		if (m_cachedOuterEnemyHitEffectWithNoInnerHits != null)
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
			result = m_cachedOuterEnemyHitEffectWithNoInnerHits;
		}
		else
		{
			result = null;
		}
		return (StandardEffectInfo)result;
	}

	public int GetExtraInnerDamagePerOuterHit()
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
			result = m_abilityMod.m_extraInnerDamagePerOuterHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraTPGainInAreaBuff()
	{
		return m_abilityMod ? m_abilityMod.m_extraTechPointGainInAreaBuff.GetModifiedValue(0) : 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_outerHitDamage);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0 && m_cachedRadiusToHitData.Count > 0)
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
			AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = base.Targeter as AbilityUtil_Targeter_MartyrLaser;
			RadiusToHitData bestMatchingData = AbilityCommon_LayeredRings.GetBestMatchingData(m_cachedRadiusToHitData, targetActor.GetCurrentBoardSquare(), abilityUtil_Targeter_MartyrLaser.m_lastLaserEndPos, base.ActorData, true);
			if (bestMatchingData != null)
			{
				int num = 0;
				if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Tertiary) == 0)
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
					num = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Tertiary) * GetExtraInnerDamagePerOuterHit();
				}
				results.m_damage = bestMatchingData.m_damage + num;
				return true;
			}
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetExtraTPGainInAreaBuff() * base.Targeter.GetNumActorsInRange();
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericHammerThrow))
		{
			m_abilityMod = (abilityMod as AbilityMod_ClericHammerThrow);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
