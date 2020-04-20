using System;
using System.Collections.Generic;
using UnityEngine;

public class RampartGrab : Ability
{
	[Header("-- On Hit Damage and Effect")]
	public int m_damageAmount = 0xA;

	public int m_damageAfterFirstHit;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Knockback Targeting")]
	public bool m_chooseEndPosition = true;

	public int m_maxTargets = 1;

	public float m_laserRange = 3f;

	public float m_laserWidth = 2f;

	public bool m_penetrateLos;

	[Header("-- Targeting Ranges")]
	public float m_destinationSelectRange = 1f;

	public int m_destinationAngleDegWithBack = 0x5A;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private float m_knockbackDistance = 100f;

	private AbilityMod_RampartGrab m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.Start()).MethodHandle;
			}
			this.m_abilityName = "Grab";
		}
		if (base.GetNumTargets() != 2)
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
			Debug.LogError("Need 2 entries in Target Data");
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.ClearTargeters();
		if (this.ChooseEndPosition())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.SetupTargeter()).MethodHandle;
			}
			base.ClearTargeters();
			AbilityUtil_Targeter_Laser item = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserRange(), this.PenetrateLos(), this.GetMaxTargets(), false, false);
			base.Targeters.Add(item);
			AbilityUtil_Targeter_RampartGrab abilityUtil_Targeter_RampartGrab = new AbilityUtil_Targeter_RampartGrab(this, AbilityAreaShape.SingleSquare, this.m_knockbackDistance, KnockbackType.PullToSource, this.GetLaserRange(), this.GetLaserWidth(), this.PenetrateLos(), this.GetMaxTargets());
			abilityUtil_Targeter_RampartGrab.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_RampartGrab);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, this.GetLaserWidth(), this.GetLaserRange(), this.PenetrateLos(), this.GetMaxTargets(), this.m_knockbackDistance, this.m_knockbackDistance, KnockbackType.PullToSourceActor, false);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (this.ChooseEndPosition())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetExpectedNumberOfTargeters()).MethodHandle;
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
		return this.GetLaserRange();
	}

	private void SetCachedFields()
	{
		this.m_cachedEnemyHitEffect = ((!this.m_abilityMod) ? this.m_enemyHitEffect : this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect));
	}

	public int GetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public int GetDamageAfterFirstHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetDamageAfterFirstHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(this.m_damageAfterFirstHit);
		}
		else
		{
			result = this.m_damageAfterFirstHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (this.m_cachedEnemyHitEffect == null) ? this.m_enemyHitEffect : this.m_cachedEnemyHitEffect;
	}

	public bool ChooseEndPosition()
	{
		return (!this.m_abilityMod) ? this.m_chooseEndPosition : this.m_abilityMod.m_chooseEndPositionMod.GetModifiedValue(this.m_chooseEndPosition);
	}

	public int GetMaxTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public float GetLaserRange()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetLaserRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLos : this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
	}

	public float GetDestinationSelectRange()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetDestinationSelectRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_destinationSelectRangeMod.GetModifiedValue(this.m_destinationSelectRange);
		}
		else
		{
			result = this.m_destinationSelectRange;
		}
		return result;
	}

	public int GetDestinationAngleDegWithBack()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetDestinationAngleDegWithBack()).MethodHandle;
			}
			result = this.m_abilityMod.m_destinationAngleDegWithBackMod.GetModifiedValue(this.m_destinationAngleDegWithBack);
		}
		else
		{
			result = this.m_destinationAngleDegWithBack;
		}
		return result;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = this.GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.CalcDamageForOrderIndex(int)).MethodHandle;
			}
			if (hitOrder > 0)
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
				return damageAfterFirstHit;
			}
		}
		return this.GetDamageAmount();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartGrab abilityMod_RampartGrab = modAsBase as AbilityMod_RampartGrab;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_RampartGrab) ? this.m_damageAmount : abilityMod_RampartGrab.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		base.AddTokenInt(tokens, "DamageAfterFirstHit", string.Empty, this.m_damageAfterFirstHit, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RampartGrab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_RampartGrab.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, (!abilityMod_RampartGrab) ? this.m_maxTargets : abilityMod_RampartGrab.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets), false);
		string name = "DestinationAngleDegWithBack";
		string empty = string.Empty;
		int val;
		if (abilityMod_RampartGrab)
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
			val = abilityMod_RampartGrab.m_destinationAngleDegWithBackMod.GetModifiedValue(this.m_destinationAngleDegWithBack);
		}
		else
		{
			val = this.m_destinationAngleDegWithBack;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (base.Targeter is AbilityUtil_Targeter_Laser)
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
				AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						results.m_damage = this.CalcDamageForOrderIndex(i);
						return true;
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex == 0)
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartGrab.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquareSafe.IsBaselineHeight())
			{
				bool result = false;
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					float num = VectorUtils.HorizontalPlaneDistInSquares(boardSquareSafe.ToVector3(), caster.GetTravelBoardSquareWorldPosition());
					if (num <= this.GetDestinationSelectRange())
					{
						Vector3 from = -1f * currentTargets[0].AimDirection;
						Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
						from.y = 0f;
						to.y = 0f;
						int num2 = Mathf.RoundToInt(Vector3.Angle(from, to));
						if (num2 <= this.GetDestinationAngleDegWithBack())
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
							result = true;
						}
					}
				}
				return result;
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
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartGrab))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RampartGrab);
		}
		this.SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
