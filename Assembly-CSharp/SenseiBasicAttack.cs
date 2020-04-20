using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiBasicAttack : Ability
{
	[Separator("Targeting Info", "cyan")]
	public float m_circleDistThreshold = 2f;

	[Header("  Targeting: For Circle")]
	public float m_circleRadius = 1.5f;

	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("On Hit Stuff", "cyan")]
	public int m_circleDamage = 0xF;

	public StandardEffectInfo m_circleEnemyHitEffect;

	[Space(10f)]
	public int m_laserDamage = 0x14;

	public StandardEffectInfo m_laserEnemyHitEffect;

	[Header("-- Extra Damage: alternate use")]
	public int m_extraDamageForAlternating;

	[Header("-- Extra Damage: far away target hits")]
	public int m_extraDamageForFarTarget;

	public float m_laserFarDistThresh;

	public float m_circleFarDistThresh;

	[Separator("Heal Per Target Hit", true)]
	public int m_healPerEnemyHit;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrOnAbility;

	public AbilityData.ActionType m_cdrAbilityTarget = AbilityData.ActionType.ABILITY_1;

	public int m_cdrMinTriggerHitCount = 3;

	[Separator("Shielding on turn start per enemy hit", true)]
	public int m_absorbPerEnemyHitOnTurnStart;

	public int m_absorbShieldDuration = 1;

	public int m_absorbAmountIfTriggeredHitCount;

	[Header("-- Animation Indices --")]
	public int m_onCastLaserAnimIndex = 1;

	public int m_onCastCircleAnimIndex = 6;

	[Header("-- Sequences --")]
	public GameObject m_circleSequencePrefab;

	public GameObject m_laserSequencePrefab;

	private AbilityMod_SenseiBasicAttack m_abilityMod;

	private Sensei_SyncComponent m_syncComp;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedCircleEnemyHitEffect;

	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.Start()).MethodHandle;
			}
			this.m_abilityName = "Sensei Circle Or Laser";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.Setup()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Sensei_SyncComponent>();
		}
		this.SetCachedFields();
		ConeTargetingInfo coneTargetingInfo = new ConeTargetingInfo();
		coneTargetingInfo.m_affectsAllies = this.GetLaserInfo().affectsAllies;
		coneTargetingInfo.m_affectsEnemies = this.GetLaserInfo().affectsEnemies;
		ConeTargetingInfo coneTargetingInfo2 = coneTargetingInfo;
		bool affectsCaster;
		if (this.GetHealPerEnemyHit() <= 0)
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
			affectsCaster = (this.GetAbsorbAmountIfTriggeredHitCount() > 0);
		}
		else
		{
			affectsCaster = true;
		}
		coneTargetingInfo2.m_affectsCaster = affectsCaster;
		coneTargetingInfo.m_penetrateLos = false;
		coneTargetingInfo.m_radiusInSquares = this.GetCircleRadius();
		coneTargetingInfo.m_widthAngleDeg = 360f;
		base.Targeter = new AbilityUtil_Targeter_ConeOrLaser(this, coneTargetingInfo, this.GetLaserInfo(), this.m_circleDistThreshold)
		{
			m_customShouldAddCasterDelegate = new AbilityUtil_Targeter_ConeOrLaser.ShouldAddCasterDelegate(this.ShouldAddCasterForTargeter)
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	private bool ShouldAddCasterForTargeter(ActorData caster, List<ActorData> actorsSoFar)
	{
		if (this.GetHealPerEnemyHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.ShouldAddCasterForTargeter(ActorData, List<ActorData>)).MethodHandle;
			}
			if (actorsSoFar.Count > 0)
			{
				return true;
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
		bool result;
		if (this.GetAbsorbAmountIfTriggeredHitCount() > 0)
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
			result = (actorsSoFar.Count >= this.GetCdrMinTriggerHitCount());
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedCircleEnemyHitEffect;
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
			cachedCircleEnemyHitEffect = this.m_abilityMod.m_circleEnemyHitEffectMod.GetModifiedValue(this.m_circleEnemyHitEffect);
		}
		else
		{
			cachedCircleEnemyHitEffect = this.m_circleEnemyHitEffect;
		}
		this.m_cachedCircleEnemyHitEffect = cachedCircleEnemyHitEffect;
		StandardEffectInfo cachedLaserEnemyHitEffect;
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
			cachedLaserEnemyHitEffect = this.m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(this.m_laserEnemyHitEffect);
		}
		else
		{
			cachedLaserEnemyHitEffect = this.m_laserEnemyHitEffect;
		}
		this.m_cachedLaserEnemyHitEffect = cachedLaserEnemyHitEffect;
	}

	public float GetCircleDistThreshold()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetCircleDistThreshold()).MethodHandle;
			}
			result = this.m_abilityMod.m_circleDistThresholdMod.GetModifiedValue(this.m_circleDistThreshold);
		}
		else
		{
			result = this.m_circleDistThreshold;
		}
		return result;
	}

	public float GetCircleRadius()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetCircleRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_circleRadiusMod.GetModifiedValue(this.m_circleRadius);
		}
		else
		{
			result = this.m_circleRadius;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public int GetCircleDamage()
	{
		int result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetCircleDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_circleDamageMod.GetModifiedValue(this.m_circleDamage);
		}
		else
		{
			result = this.m_circleDamage;
		}
		return result;
	}

	public StandardEffectInfo GetCircleEnemyHitEffect()
	{
		return (this.m_cachedCircleEnemyHitEffect == null) ? this.m_circleEnemyHitEffect : this.m_cachedCircleEnemyHitEffect;
	}

	public int GetLaserDamage()
	{
		return (!this.m_abilityMod) ? this.m_laserDamage : this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetLaserEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserEnemyHitEffect;
		}
		else
		{
			result = this.m_laserEnemyHitEffect;
		}
		return result;
	}

	public int GetExtraDamageForAlternating()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForAlternating : this.m_abilityMod.m_extraDamageForAlternatingMod.GetModifiedValue(this.m_extraDamageForAlternating);
	}

	public int GetExtraDamageForFarTarget()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForFarTarget : this.m_abilityMod.m_extraDamageForFarTargetMod.GetModifiedValue(this.m_extraDamageForFarTarget);
	}

	public float GetLaserFarDistThresh()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetLaserFarDistThresh()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserFarDistThreshMod.GetModifiedValue(this.m_laserFarDistThresh);
		}
		else
		{
			result = this.m_laserFarDistThresh;
		}
		return result;
	}

	public float GetCircleFarDistThresh()
	{
		return (!this.m_abilityMod) ? this.m_circleFarDistThresh : this.m_abilityMod.m_circleFarDistThreshMod.GetModifiedValue(this.m_circleFarDistThresh);
	}

	public int GetHealPerEnemyHit()
	{
		return (!this.m_abilityMod) ? this.m_healPerEnemyHit : this.m_abilityMod.m_healPerEnemyHitMod.GetModifiedValue(this.m_healPerEnemyHit);
	}

	public int GetCdrOnAbility()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetCdrOnAbility()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrOnAbilityMod.GetModifiedValue(this.m_cdrOnAbility);
		}
		else
		{
			result = this.m_cdrOnAbility;
		}
		return result;
	}

	public int GetCdrMinTriggerHitCount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetCdrMinTriggerHitCount()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrMinTriggerHitCountMod.GetModifiedValue(this.m_cdrMinTriggerHitCount);
		}
		else
		{
			result = this.m_cdrMinTriggerHitCount;
		}
		return result;
	}

	public int GetAbsorbPerEnemyHitOnTurnStart()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetAbsorbPerEnemyHitOnTurnStart()).MethodHandle;
			}
			result = this.m_abilityMod.m_absorbPerEnemyHitOnTurnStartMod.GetModifiedValue(this.m_absorbPerEnemyHitOnTurnStart);
		}
		else
		{
			result = this.m_absorbPerEnemyHitOnTurnStart;
		}
		return result;
	}

	public int GetAbsorbAmountIfTriggeredHitCount()
	{
		return (!(this.m_abilityMod != null)) ? this.m_absorbAmountIfTriggeredHitCount : this.m_abilityMod.m_absorbAmountIfTriggeredHitCountMod.GetModifiedValue(this.m_absorbAmountIfTriggeredHitCount);
	}

	public int GetExtraDamageForFarTarget(ActorData targetActor, ActorData caster, bool forCone)
	{
		float num;
		if (forCone)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetExtraDamageForFarTarget(ActorData, ActorData, bool)).MethodHandle;
			}
			num = this.GetCircleFarDistThresh();
		}
		else
		{
			num = this.GetLaserFarDistThresh();
		}
		float num2 = num;
		if (num2 > 0f)
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
			if (this.GetExtraDamageForFarTarget() > 0)
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
				Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - caster.GetTravelBoardSquareWorldPosition();
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude >= num2 * Board.Get().squareSize)
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
					return this.GetExtraDamageForFarTarget();
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "CircleDamage", string.Empty, this.m_circleDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_circleEnemyHitEffect, "CircleEnemyHitEffect", this.m_circleEnemyHitEffect, true);
		base.AddTokenInt(tokens, "LaserDamage", string.Empty, this.m_laserDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserEnemyHitEffect, "LaserEnemyHitEffect", this.m_laserEnemyHitEffect, true);
		base.AddTokenInt(tokens, "ExtraDamageForAlternating", string.Empty, this.m_extraDamageForAlternating, false);
		base.AddTokenInt(tokens, "ExtraDamageForFarTarget", string.Empty, this.m_extraDamageForFarTarget, false);
		base.AddTokenInt(tokens, "HealPerEnemyHit", string.Empty, this.m_healPerEnemyHit, false);
		base.AddTokenInt(tokens, "CdrOnAbility", string.Empty, this.m_cdrOnAbility, false);
		base.AddTokenInt(tokens, "CdrMinTriggerHitCount", string.Empty, this.m_cdrMinTriggerHitCount, false);
		base.AddTokenInt(tokens, "AbsorbPerEnemyHitOnTurnStart", string.Empty, this.m_absorbPerEnemyHitOnTurnStart, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetCircleDamage());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetLaserDamage());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealPerEnemyHit());
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			bool flag = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0;
			int num;
			if (flag)
			{
				num = this.GetCircleDamage();
				if (this.GetExtraDamageForAlternating() > 0)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_syncComp && (int)this.m_syncComp.m_lastPrimaryUsedMode == 2)
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
						num += this.GetExtraDamageForAlternating();
					}
				}
			}
			else
			{
				num = this.GetLaserDamage();
				if (this.GetExtraDamageForAlternating() > 0)
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
					if (this.m_syncComp)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if ((int)this.m_syncComp.m_lastPrimaryUsedMode == 1)
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
							num += this.GetExtraDamageForAlternating();
						}
					}
				}
			}
			int extraDamageForFarTarget = this.GetExtraDamageForFarTarget(targetActor, base.ActorData, flag);
			num += extraDamageForFarTarget;
			results.m_damage = num;
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
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
			int healing = 0;
			if (this.GetHealPerEnemyHit() > 0)
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
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				healing = visibleActorsCountByTooltipSubject * this.GetHealPerEnemyHit();
			}
			results.m_healing = healing;
			if (this.GetAbsorbAmountIfTriggeredHitCount() > 0)
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
				int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (visibleActorsCountByTooltipSubject2 >= this.GetCdrMinTriggerHitCount())
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
					results.m_absorb = this.GetAbsorbAmountIfTriggeredHitCount();
				}
			}
		}
		return true;
	}

	private bool ShouldUseCircle(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.GetTravelBoardSquareWorldPosition();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= this.m_circleDistThreshold;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.m_circleDistThreshold - 0.1f;
		max = this.m_circleDistThreshold + 0.1f;
		return true;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == this.m_onCastCircleAnimIndex || animIndex == this.m_onCastLaserAnimIndex;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (targets != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBasicAttack.GetActionAnimType(List<AbilityTarget>, ActorData)).MethodHandle;
			}
			if (caster != null)
			{
				bool flag = this.ShouldUseCircle(targets[0].FreePos, caster);
				ActorModelData.ActionAnimationType result;
				if (flag)
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
					result = (ActorModelData.ActionAnimationType)this.m_onCastCircleAnimIndex;
				}
				else
				{
					result = (ActorModelData.ActionAnimationType)this.m_onCastLaserAnimIndex;
				}
				return result;
			}
		}
		return base.GetActionAnimType();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SenseiBasicAttack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public enum LastUsedModeFlag
	{
		None,
		Cone,
		Laser
	}
}
