using System.Collections.Generic;
using UnityEngine;

public class ClericMeleeKnockback : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;

	public float m_minSeparationBetweenAoeAndCaster = 1f;

	public float m_maxSeparationBetweenAoeAndCaster = 2.5f;

	public float m_aoeRadius = 1.5f;

	public int m_maxTargets = 5;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;

	public float m_knockbackDistance = 1f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public StandardEffectInfo m_targetHitEffect;

	[Separator("Connecting Laser between caster and aoe center", true)]
	public float m_connectLaserWidth;

	public int m_connectLaserDamage = 20;

	public StandardEffectInfo m_connectLaserEnemyHitEffect;

	[Separator("-- Sequences", true)]
	public GameObject m_castSequencePrefab;

	[Header("-- Anim versions")]
	public float m_rangePercentForLongRangeAnim = 0.5f;

	private Cleric_SyncComponent m_syncComp;

	private AbilityMod_ClericMeleeKnockback m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private StandardEffectInfo m_cachedConnectLaserEnemyHitEffect;

	private StandardEffectInfo m_cachedSingleTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Sphere of Might";
		}
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_AoE_Smooth_FixedOffset abilityUtil_Targeter_AoE_Smooth_FixedOffset = new AbilityUtil_Targeter_AoE_Smooth_FixedOffset(this, GetMinSeparationBetweenAoeAndCaster(), GetMaxSeparationBetweenAoeAndCaster(), GetAoeRadius(), PenetrateLineOfSight(), GetKnockbackDistance(), GetKnockbackType(), GetConnectLaserWidth(), true, false, GetMaxTargets());
		abilityUtil_Targeter_AoE_Smooth_FixedOffset.m_customShouldIncludeActorDelegate = ShouldIncludeAoEActor;
		abilityUtil_Targeter_AoE_Smooth_FixedOffset.m_delegateIsSquareInLos = IsSquareInLosForCone;
		base.Targeter = abilityUtil_Targeter_AoE_Smooth_FixedOffset;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ClericMeleeKnockback))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_ClericMeleeKnockback);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetHitEffect;
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
			cachedTargetHitEffect = m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = m_targetHitEffect;
		}
		m_cachedTargetHitEffect = cachedTargetHitEffect;
		StandardEffectInfo cachedConnectLaserEnemyHitEffect;
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
			cachedConnectLaserEnemyHitEffect = m_abilityMod.m_connectLaserEnemyHitEffectMod.GetModifiedValue(m_connectLaserEnemyHitEffect);
		}
		else
		{
			cachedConnectLaserEnemyHitEffect = m_connectLaserEnemyHitEffect;
		}
		m_cachedConnectLaserEnemyHitEffect = cachedConnectLaserEnemyHitEffect;
		m_cachedSingleTargetHitEffect = ((!m_abilityMod) ? null : m_abilityMod.m_singleTargetHitEffectMod.GetModifiedValue(m_targetHitEffect));
	}

	public bool PenetrateLineOfSight()
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

	public float GetMinSeparationBetweenAoeAndCaster()
	{
		return (!m_abilityMod) ? m_minSeparationBetweenAoeAndCaster : m_abilityMod.m_minSeparationBetweenAoeAndCasterMod.GetModifiedValue(m_minSeparationBetweenAoeAndCaster);
	}

	public float GetMaxSeparationBetweenAoeAndCaster()
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
			result = m_abilityMod.m_maxSeparationBetweenAoeAndCasterMod.GetModifiedValue(m_maxSeparationBetweenAoeAndCaster);
		}
		else
		{
			result = m_maxSeparationBetweenAoeAndCaster;
		}
		return result;
	}

	public float GetAoeRadius()
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
			result = m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius);
		}
		else
		{
			result = m_aoeRadius;
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
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmount()
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
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public float GetKnockbackDistance()
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
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		else
		{
			result = m_knockbackDistance;
		}
		return result;
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
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
			result = m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
		}
		else
		{
			result = m_knockbackType;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedTargetHitEffect != null)
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
			result = m_cachedTargetHitEffect;
		}
		else
		{
			result = m_targetHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSingleTargetHitEffect()
	{
		object result;
		if (m_cachedSingleTargetHitEffect != null)
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
			result = m_cachedSingleTargetHitEffect;
		}
		else
		{
			result = null;
		}
		return (StandardEffectInfo)result;
	}

	public int GetExtraTechPointsPerHitWithAreaBuff()
	{
		return m_abilityMod ? m_abilityMod.m_extraTechPointsPerHitWithAreaBuff.GetModifiedValue(0) : 0;
	}

	public float GetConnectLaserWidth()
	{
		return (!m_abilityMod) ? m_connectLaserWidth : m_abilityMod.m_connectLaserWidthMod.GetModifiedValue(m_connectLaserWidth);
	}

	public int GetConnectLaserDamage()
	{
		return (!m_abilityMod) ? m_connectLaserDamage : m_abilityMod.m_connectLaserDamageMod.GetModifiedValue(m_connectLaserDamage);
	}

	public StandardEffectInfo GetConnectLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedConnectLaserEnemyHitEffect != null)
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
			result = m_cachedConnectLaserEnemyHitEffect;
		}
		else
		{
			result = m_connectLaserEnemyHitEffect;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
		AddTokenInt(tokens, "ConnectLaserDamage", string.Empty, m_connectLaserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_connectLaserEnemyHitEffect, "ConnectLaserEnemyHitEffect", m_connectLaserEnemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		if (GetConnectLaserWidth() > 0f)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_connectLaserDamage);
		}
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (targetActor.GetTeam() != base.ActorData.GetTeam())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
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
						results.m_damage = GetDamageAmount();
					}
					else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
					{
						results.m_damage = GetConnectLaserDamage();
					}
					return true;
				}
			}
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		AbilityData abilityData = caster.GetAbilityData();
		if (abilityData != null && abilityData.HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
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
			num += base.Targeters[currentTargeterIndex].GetNumActorsInRange() * GetExtraTechPointsPerHitWithAreaBuff();
		}
		return num;
	}

	public bool IsSquareInLosForCone(BoardSquare testSquare, Vector3 centerPos, ActorData targetingActor)
	{
		if (testSquare == null)
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
					return false;
				}
			}
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		centerPos.y = travelBoardSquareWorldPositionForLos.y;
		Vector3 vector = centerPos - travelBoardSquareWorldPositionForLos;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, vector.normalized, vector.magnitude, false, targetingActor);
		if (Vector3.Distance(laserEndPoint, centerPos) > 0.1f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!PenetrateLineOfSight())
		{
			Vector3 vector2 = testSquare.ToVector3();
			vector2.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			Vector3 vector3 = vector2 - centerPos;
			laserEndPoint = VectorUtils.GetLaserEndPoint(centerPos, vector3.normalized, vector3.magnitude, false, targetingActor);
			if (Vector3.Distance(laserEndPoint, vector2) > 0.1f)
			{
				return false;
			}
		}
		return true;
	}

	public bool ShouldIncludeAoEActor(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor)
	{
		if (potentialActor == null)
		{
			return false;
		}
		return IsSquareInLosForCone(potentialActor.GetCurrentBoardSquare(), centerPos, targetingActor);
	}
}
