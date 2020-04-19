using System;
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
	public int m_damageAmount = 0x14;

	public float m_knockbackDistance = 1f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public StandardEffectInfo m_targetHitEffect;

	[Separator("Connecting Laser between caster and aoe center", true)]
	public float m_connectLaserWidth;

	public int m_connectLaserDamage = 0x14;

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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.Start()).MethodHandle;
			}
			this.m_abilityName = "Sphere of Might";
		}
		this.m_syncComp = base.GetComponent<Cleric_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth_FixedOffset(this, this.GetMinSeparationBetweenAoeAndCaster(), this.GetMaxSeparationBetweenAoeAndCaster(), this.GetAoeRadius(), this.PenetrateLineOfSight(), this.GetKnockbackDistance(), this.GetKnockbackType(), this.GetConnectLaserWidth(), true, false, this.GetMaxTargets())
		{
			m_customShouldIncludeActorDelegate = new AbilityUtil_Targeter_AoE_Smooth.ShouldIncludeActorDelegate(this.ShouldIncludeAoEActor),
			m_delegateIsSquareInLos = new AbilityUtil_Targeter_AoE_Smooth_FixedOffset.IsSquareInLosDelegate(this.IsSquareInLosForCone)
		};
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericMeleeKnockback))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ClericMeleeKnockback);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.SetCachedFields()).MethodHandle;
			}
			cachedTargetHitEffect = this.m_abilityMod.m_targetHitEffectMod.GetModifiedValue(this.m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = this.m_targetHitEffect;
		}
		this.m_cachedTargetHitEffect = cachedTargetHitEffect;
		StandardEffectInfo cachedConnectLaserEnemyHitEffect;
		if (this.m_abilityMod)
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
			cachedConnectLaserEnemyHitEffect = this.m_abilityMod.m_connectLaserEnemyHitEffectMod.GetModifiedValue(this.m_connectLaserEnemyHitEffect);
		}
		else
		{
			cachedConnectLaserEnemyHitEffect = this.m_connectLaserEnemyHitEffect;
		}
		this.m_cachedConnectLaserEnemyHitEffect = cachedConnectLaserEnemyHitEffect;
		this.m_cachedSingleTargetHitEffect = ((!this.m_abilityMod) ? null : this.m_abilityMod.m_singleTargetHitEffectMod.GetModifiedValue(this.m_targetHitEffect));
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.PenetrateLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public float GetMinSeparationBetweenAoeAndCaster()
	{
		return (!this.m_abilityMod) ? this.m_minSeparationBetweenAoeAndCaster : this.m_abilityMod.m_minSeparationBetweenAoeAndCasterMod.GetModifiedValue(this.m_minSeparationBetweenAoeAndCaster);
	}

	public float GetMaxSeparationBetweenAoeAndCaster()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetMaxSeparationBetweenAoeAndCaster()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxSeparationBetweenAoeAndCasterMod.GetModifiedValue(this.m_maxSeparationBetweenAoeAndCaster);
		}
		else
		{
			result = this.m_maxSeparationBetweenAoeAndCaster;
		}
		return result;
	}

	public float GetAoeRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetAoeRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_aoeRadiusMod.GetModifiedValue(this.m_aoeRadius);
		}
		else
		{
			result = this.m_aoeRadius;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public float GetKnockbackDistance()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetKnockbackDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		else
		{
			result = this.m_knockbackDistance;
		}
		return result;
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetKnockbackType()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackTypeMod.GetModifiedValue(this.m_knockbackType);
		}
		else
		{
			result = this.m_knockbackType;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedTargetHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetTargetHitEffect()).MethodHandle;
			}
			result = this.m_cachedTargetHitEffect;
		}
		else
		{
			result = this.m_targetHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSingleTargetHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSingleTargetHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetSingleTargetHitEffect()).MethodHandle;
			}
			result = this.m_cachedSingleTargetHitEffect;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public int GetExtraTechPointsPerHitWithAreaBuff()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_extraTechPointsPerHitWithAreaBuff.GetModifiedValue(0);
	}

	public float GetConnectLaserWidth()
	{
		return (!this.m_abilityMod) ? this.m_connectLaserWidth : this.m_abilityMod.m_connectLaserWidthMod.GetModifiedValue(this.m_connectLaserWidth);
	}

	public int GetConnectLaserDamage()
	{
		return (!this.m_abilityMod) ? this.m_connectLaserDamage : this.m_abilityMod.m_connectLaserDamageMod.GetModifiedValue(this.m_connectLaserDamage);
	}

	public StandardEffectInfo GetConnectLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedConnectLaserEnemyHitEffect != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetConnectLaserEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedConnectLaserEnemyHitEffect;
		}
		else
		{
			result = this.m_connectLaserEnemyHitEffect;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetHitEffect, "TargetHitEffect", this.m_targetHitEffect, true);
		base.AddTokenInt(tokens, "ConnectLaserDamage", string.Empty, this.m_connectLaserDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_connectLaserEnemyHitEffect, "ConnectLaserEnemyHitEffect", this.m_connectLaserEnemyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		if (this.GetConnectLaserWidth() > 0f)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_connectLaserDamage);
		}
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (targetActor.\u000E() != base.ActorData.\u000E())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
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
				results.m_damage = this.GetDamageAmount();
			}
			else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
			{
				results.m_damage = this.GetConnectLaserDamage();
			}
			return true;
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		AbilityData abilityData = caster.\u000E();
		if (abilityData != null && abilityData.HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			num += base.Targeters[currentTargeterIndex].GetNumActorsInRange() * this.GetExtraTechPointsPerHitWithAreaBuff();
		}
		return num;
	}

	public bool IsSquareInLosForCone(BoardSquare testSquare, Vector3 centerPos, ActorData targetingActor)
	{
		if (testSquare == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericMeleeKnockback.IsSquareInLosForCone(BoardSquare, Vector3, ActorData)).MethodHandle;
			}
			return false;
		}
		Vector3 vector = targetingActor.\u0015();
		centerPos.y = vector.y;
		Vector3 vector2 = centerPos - vector;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, vector2.normalized, vector2.magnitude, false, targetingActor, null, true);
		if (Vector3.Distance(laserEndPoint, centerPos) > 0.1f)
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
			return false;
		}
		if (!this.PenetrateLineOfSight())
		{
			Vector3 vector3 = testSquare.ToVector3();
			vector3.y = (float)Board.\u000E().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			Vector3 vector4 = vector3 - centerPos;
			laserEndPoint = VectorUtils.GetLaserEndPoint(centerPos, vector4.normalized, vector4.magnitude, false, targetingActor, null, true);
			if (Vector3.Distance(laserEndPoint, vector3) > 0.1f)
			{
				return false;
			}
		}
		return true;
	}

	public bool ShouldIncludeAoEActor(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor)
	{
		return !(potentialActor == null) && this.IsSquareInLosForCone(potentialActor.\u0012(), centerPos, targetingActor);
	}
}
