using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiWindBlade : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 0.6f;

	public float m_minRangeBeforeBend = 1f;

	public float m_maxRangeBeforeBend = 5.5f;

	public float m_maxTotalRange = 7.5f;

	public float m_maxBendAngle = 45f;

	public bool m_penetrateLoS;

	public int m_maxTargets = 1;

	[Header("-- Damage")]
	public int m_laserDamageAmount = 5;

	public int m_damageChangePerTarget;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public int m_shieldingPerEnemyHitNextTurn;

	public int m_shieldingDuration = 1;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SamuraiWindBlade m_abilityMod;

	private Samurai_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.Start()).MethodHandle;
			}
			this.m_abilityName = "Wind Blade";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		this.m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		base.ClearTargeters();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = new AbilityUtil_Targeter_BendingLaser(this, this.GetLaserWidth(), this.GetMinRangeBeforeBend(), this.GetMaxRangeBeforeBend(), this.GetMaxTotalRange(), this.GetMaxBendAngle(), this.PenetrateLoS(), this.GetMaxTargets(), false, false);
			abilityUtil_Targeter_BendingLaser.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_BendingLaser);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (!base.Targeters.IsNullOrEmpty<AbilityUtil_Targeter>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (abilityUtil_Targeter_BendingLaser.DidStopShort())
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
				return 1;
			}
		}
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetMaxTotalRange();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.SetCachedFields()).MethodHandle;
			}
			cachedLaserHitEffect = this.m_abilityMod.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = this.m_laserHitEffect;
		}
		this.m_cachedLaserHitEffect = cachedLaserHitEffect;
	}

	public float GetLaserWidth()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetMinRangeBeforeBend()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetMinRangeBeforeBend()).MethodHandle;
			}
			result = this.m_abilityMod.m_minRangeBeforeBendMod.GetModifiedValue(this.m_minRangeBeforeBend);
		}
		else
		{
			result = this.m_minRangeBeforeBend;
		}
		return result;
	}

	public float GetMaxRangeBeforeBend()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetMaxRangeBeforeBend()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxRangeBeforeBendMod.GetModifiedValue(this.m_maxRangeBeforeBend);
		}
		else
		{
			result = this.m_maxRangeBeforeBend;
		}
		return result;
	}

	public float GetMaxTotalRange()
	{
		return (!this.m_abilityMod) ? this.m_maxTotalRange : this.m_abilityMod.m_maxTotalRangeMod.GetModifiedValue(this.m_maxTotalRange);
	}

	public float GetMaxBendAngle()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetMaxBendAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxBendAngleMod.GetModifiedValue(this.m_maxBendAngle);
		}
		else
		{
			result = this.m_maxBendAngle;
		}
		return result;
	}

	public bool PenetrateLoS()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLoS : this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
	}

	public int GetMaxTargets()
	{
		return (!this.m_abilityMod) ? this.m_maxTargets : this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
	}

	public int GetLaserDamageAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetLaserDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			result = this.m_laserDamageAmount;
		}
		return result;
	}

	public int GetDamageChangePerTarget()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetDamageChangePerTarget()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageChangePerTargetMod.GetModifiedValue(this.m_damageChangePerTarget);
		}
		else
		{
			result = this.m_damageChangePerTarget;
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetLaserHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserHitEffect;
		}
		else
		{
			result = this.m_laserHitEffect;
		}
		return result;
	}

	public int GetShieldingPerEnemyHitNextTurn()
	{
		return (!this.m_abilityMod) ? this.m_shieldingPerEnemyHitNextTurn : this.m_abilityMod.m_shieldingPerEnemyHitNextTurnMod.GetModifiedValue(this.m_shieldingPerEnemyHitNextTurn);
	}

	public int GetShieldingDuration()
	{
		return (!this.m_abilityMod) ? this.m_shieldingDuration : this.m_abilityMod.m_shieldingDurationMod.GetModifiedValue(this.m_shieldingDuration);
	}

	public int CalcDamage(int hitOrder)
	{
		int num = this.GetLaserDamageAmount();
		if (this.GetDamageChangePerTarget() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.CalcDamage(int)).MethodHandle;
			}
			if (hitOrder > 0)
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
				num += this.GetDamageChangePerTarget() * hitOrder;
			}
		}
		return num;
	}

	public int GetHitOrderIndexFromTargeters(ActorData actor, int currentTargetIndex)
	{
		int num = 0;
		if (base.Targeters != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetHitOrderIndexFromTargeters(ActorData, int)).MethodHandle;
			}
			int num2 = 0;
			while (num2 < base.Targeters.Count && num2 <= currentTargetIndex)
			{
				AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[num2] as AbilityUtil_Targeter_BendingLaser;
				if (abilityUtil_Targeter_BendingLaser != null)
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
					using (List<ActorData>.Enumerator enumerator = abilityUtil_Targeter_BendingLaser.m_ordererdHitActors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData x = enumerator.Current;
							if (x == actor)
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
								return num;
							}
							num++;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				num2++;
			}
		}
		return -1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "LaserDamageAmount", string.Empty, this.m_laserDamageAmount, false);
		base.AddTokenInt(tokens, "DamageChangePerTarget", string.Empty, this.m_damageChangePerTarget, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", this.m_laserHitEffect, true);
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "ShieldingPerEnemyHitNextTurn", string.Empty, this.m_shieldingPerEnemyHitNextTurn, false);
		base.AddTokenInt(tokens, "ShieldingDuration", string.Empty, this.m_shieldingDuration, false);
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = this.GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f && maxBendAngle < 360f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetTargeterClampedAimDirection(Vector3, List<AbilityTarget>)).MethodHandle;
			}
			aimDir = Vector3.RotateTowards(aimDirection, aimDir, 0.0174532924f * maxBendAngle, 0f);
		}
		return aimDir;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float magnitude = (currentTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
		if (magnitude < this.GetMinRangeBeforeBend() * Board.Get().squareSize)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetClampedRangeInSquares(ActorData, AbilityTarget)).MethodHandle;
			}
			return this.GetMinRangeBeforeBend();
		}
		if (magnitude > this.GetMaxRangeBeforeBend() * Board.Get().squareSize)
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
			return this.GetMaxRangeBeforeBend();
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float clampedRangeInSquares = this.GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = travelBoardSquareWorldPositionForLos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return this.GetMaxTotalRange() - clampedRangeInSquares;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_laserDamageAmount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		}
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.m_laserDamageAmount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			int num = this.GetLaserDamageAmount();
			if (this.GetDamageChangePerTarget() > 0)
			{
				int hitOrderIndexFromTargeters = this.GetHitOrderIndexFromTargeters(targetActor, currentTargeterIndex);
				num = this.CalcDamage(hitOrderIndexFromTargeters);
			}
			if (this.m_syncComponent != null)
			{
				num += this.m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiWindBlade))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiWindBlade.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SamuraiWindBlade);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
