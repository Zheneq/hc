﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ThiefOnTheRun : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiStep = true;

	public float m_minDistanceBetweenSteps = 2f;

	public float m_minDistanceBetweenAnySteps = -1f;

	public float m_maxDistanceBetweenSteps = 10f;

	[Header("-- Dash Hit Size")]
	public float m_dashRadius = 1f;

	public bool m_dashPenetrateLineOfSight;

	[Header("-- Hit Damage and Effect")]
	public int m_damageAmount;

	public int m_subsequentDamage;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Hid On Self")]
	public StandardEffectInfo m_effectOnSelfThroughSmokeField;

	public int m_cooldownReductionIfNoEnemy;

	public AbilityData.ActionType m_cooldownReductionOnAbility = AbilityData.ActionType.ABILITY_3;

	[Header("-- Spoil Powerup Spawn")]
	public SpoilsSpawnData m_spoilSpawnInfo;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private int m_numChargePiviots = 1;

	private AbilityMod_ThiefOnTheRun m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedEffectOnSelfThroughSmokeField;

	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "On the Run";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		int numChargePiviots;
		if (this.m_targeterMultiStep)
		{
			numChargePiviots = Mathf.Max(base.GetNumTargets(), 1);
		}
		else
		{
			numChargePiviots = 1;
		}
		this.m_numChargePiviots = numChargePiviots;
		float dashRadius = this.GetDashRadius();
		if (this.GetExpectedNumberOfTargeters() < 2)
		{
			base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, dashRadius, dashRadius, dashRadius, -1, false, this.DashPenetrateLineOfSight());
		}
		else
		{
			base.ClearTargeters();
			int expectedNumberOfTargeters = this.GetExpectedNumberOfTargeters();
			for (int i = 0; i < expectedNumberOfTargeters; i++)
			{
				AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, dashRadius, dashRadius, dashRadius, -1, false, this.DashPenetrateLineOfSight());
				if (i < expectedNumberOfTargeters - 1)
				{
					abilityUtil_Targeter_ChargeAoE.UseEndPosAsDamageOriginIfOverlap = true;
				}
				base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return this.m_numChargePiviots;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (this.GetMaxDistanceBetweenSteps() - 0.5f) * (float)this.m_numChargePiviots;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		this.m_cachedEffectOnSelfThroughSmokeField = ((!this.m_abilityMod) ? this.m_effectOnSelfThroughSmokeField : this.m_abilityMod.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(this.m_effectOnSelfThroughSmokeField));
		this.m_cachedSpoilSpawnInfo = ((!this.m_abilityMod) ? this.m_spoilSpawnInfo : this.m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(this.m_spoilSpawnInfo));
	}

	public float GetMinDistanceBetweenSteps()
	{
		return (!this.m_abilityMod) ? this.m_minDistanceBetweenSteps : this.m_abilityMod.m_minDistanceBetweenStepsMod.GetModifiedValue(this.m_minDistanceBetweenSteps);
	}

	public float GetMinDistanceBetweenAnySteps()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_minDistanceBetweenAnyStepsMod.GetModifiedValue(this.m_minDistanceBetweenAnySteps);
		}
		else
		{
			result = this.m_minDistanceBetweenAnySteps;
		}
		return result;
	}

	public float GetMaxDistanceBetweenSteps()
	{
		return (!this.m_abilityMod) ? this.m_maxDistanceBetweenSteps : this.m_abilityMod.m_maxDistanceBetweenStepsMod.GetModifiedValue(this.m_maxDistanceBetweenSteps);
	}

	public float GetDashRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashRadiusMod.GetModifiedValue(this.m_dashRadius);
		}
		else
		{
			result = this.m_dashRadius;
		}
		return result;
	}

	public bool DashPenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(this.m_dashPenetrateLineOfSight);
		}
		else
		{
			result = this.m_dashPenetrateLineOfSight;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetSubsequentDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_subsequentDamageMod.GetModifiedValue(this.m_subsequentDamage);
		}
		else
		{
			result = this.m_subsequentDamage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
		{
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfThroughSmokeField()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelfThroughSmokeField != null)
		{
			result = this.m_cachedEffectOnSelfThroughSmokeField;
		}
		else
		{
			result = this.m_effectOnSelfThroughSmokeField;
		}
		return result;
	}

	public int GetCooldownReductionIfNoEnemy()
	{
		return (!this.m_abilityMod) ? this.m_cooldownReductionIfNoEnemy : this.m_abilityMod.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(this.m_cooldownReductionIfNoEnemy);
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		SpoilsSpawnData result;
		if (this.m_cachedSpoilSpawnInfo != null)
		{
			result = this.m_cachedSpoilSpawnInfo;
		}
		else
		{
			result = this.m_spoilSpawnInfo;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		this.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		ActorData actorData = base.ActorData;
		if (actorData != null)
		{
			if (actorData.GetCurrentBoardSquare() != null)
			{
				for (int i = 0; i <= currentTargeterIndex; i++)
				{
					if (i >= base.Targeters.Count)
					{
						break;
					}
					BoardSquare y = (i <= 0) ? actorData.GetCurrentBoardSquare() : Board.Get().GetBoardSquareSafe(base.Targeters[i - 1].LastUpdatingGridPos);
					int subsequentAmount = this.GetSubsequentDamage();
					if (targetActor.GetCurrentBoardSquare() == y)
					{
						subsequentAmount = 0;
					}
					Ability.AddNameplateValueForOverlap(ref result, base.Targeters[i], targetActor, currentTargeterIndex, this.GetDamageAmount(), subsequentAmount, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
				}
			}
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefOnTheRun abilityMod_ThiefOnTheRun = modAsBase as AbilityMod_ThiefOnTheRun;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_ThiefOnTheRun) ? this.m_damageAmount : abilityMod_ThiefOnTheRun.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		string name = "SubsequentDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefOnTheRun)
		{
			val = abilityMod_ThiefOnTheRun.m_subsequentDamageMod.GetModifiedValue(this.m_subsequentDamage);
		}
		else
		{
			val = this.m_subsequentDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ThiefOnTheRun)
		{
			effectInfo = abilityMod_ThiefOnTheRun.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ThiefOnTheRun) ? this.m_effectOnSelfThroughSmokeField : abilityMod_ThiefOnTheRun.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(this.m_effectOnSelfThroughSmokeField), "EffectOnSelfThroughSmokeField", this.m_effectOnSelfThroughSmokeField, true);
		string name2 = "CooldownReductionIfNoEnemy";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ThiefOnTheRun)
		{
			val2 = abilityMod_ThiefOnTheRun.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(this.m_cooldownReductionIfNoEnemy);
		}
		else
		{
			val2 = this.m_cooldownReductionIfNoEnemy;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe == null || !boardSquareSafe.IsBaselineHeight())
		{
			return false;
		}
		bool result;
		if (this.GetExpectedNumberOfTargeters() < 2)
		{
			result = (KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null);
		}
		else
		{
			BoardSquare boardSquare;
			if (targetIndex == 0)
			{
				boardSquare = caster.GetCurrentBoardSquare();
			}
			else
			{
				boardSquare = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
			}
			bool flag = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, boardSquare, false) != null;
			float squareSize = Board.Get().squareSize;
			float num = Vector3.Distance(boardSquare.ToVector3(), boardSquareSafe.ToVector3());
			bool flag2;
			if (num >= this.GetMinDistanceBetweenSteps() * squareSize)
			{
				flag2 = (num <= this.GetMaxDistanceBetweenSteps() * squareSize);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag)
			{
				if (flag3 && this.GetMinDistanceBetweenAnySteps() > 0f)
				{
					for (int i = 0; i < targetIndex; i++)
					{
						BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[i].GridPos);
						flag3 &= (Vector3.Distance(boardSquareSafe2.ToVector3(), boardSquareSafe.ToVector3()) >= this.GetMinDistanceBetweenAnySteps() * squareSize);
					}
				}
			}
			bool flag4;
			if (flag)
			{
				flag4 = flag3;
			}
			else
			{
				flag4 = false;
			}
			result = flag4;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefOnTheRun))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ThiefOnTheRun);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
