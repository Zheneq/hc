﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowRain : Ability
{
	[Separator("Targeting Info", true)]
	public float m_startRadius = 3f;

	public float m_endRadius = 3f;

	public float m_lineRadius = 3f;

	public float m_minRangeBetween = 1f;

	public float m_maxRangeBetween = 4f;

	[Header("-- Whether require LoS to end square of line")]
	public bool m_linePenetrateLoS;

	[Header("-- Whether check LoS for gameplay hits")]
	public bool m_aoePenetrateLoS;

	public int m_maxTargets = 5;

	[Separator("Enemy Hit", true)]
	public int m_damage = 0x28;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_hitAreaSequencePrefab;

	private AbilityMod_ArcherArrowRain m_abilityMod;

	private ArcherHealingDebuffArrow m_healArrowAbility;

	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;

	private AbilityData m_abilityData;

	private ActorTargeting m_actorTargeting;

	private Archer_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAdditionalEnemyHitEffect;

	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Arrow Rain";
		}
		this.m_abilityData = base.GetComponent<AbilityData>();
		if (this.m_abilityData != null)
		{
			this.m_healArrowAbility = (base.GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow);
			if (this.m_healArrowAbility != null)
			{
				this.m_healArrowActionType = this.m_abilityData.GetActionTypeOfAbility(this.m_healArrowAbility);
			}
		}
		this.m_actorTargeting = base.GetComponent<ActorTargeting>();
		this.m_syncComp = base.GetComponent<Archer_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_CapsuleAoE abilityUtil_Targeter_CapsuleAoE = new AbilityUtil_Targeter_CapsuleAoE(this, this.GetStartRadius(), this.GetEndRadius(), this.GetLineRadius(), this.GetMaxTargets(), false, this.AoePenetrateLoS());
			abilityUtil_Targeter_CapsuleAoE.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_CapsuleAoE.ShowArcToShape = false;
			base.Targeters.Add(abilityUtil_Targeter_CapsuleAoE);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return this.GetTargetData().Length;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex > 0)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe2 != null)
				{
					float num = Vector3.Distance(boardSquareSafe.ToVector3(), boardSquareSafe2.ToVector3());
					if (num <= this.GetMaxRangeBetween() * Board.Get().squareSize)
					{
						if (num >= this.GetMinRangeBetween() * Board.Get().squareSize)
						{
							if (!this.LinePenetrateLoS())
							{
								if (!boardSquareSafe.symbol_0013(boardSquareSafe2.x, boardSquareSafe2.y))
								{
									return false;
								}
							}
							return true;
						}
					}
				}
			}
			return false;
		}
		return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherArrowRain))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ArcherArrowRain);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
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
		StandardEffectInfo cachedAdditionalEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedAdditionalEnemyHitEffect = this.m_abilityMod.m_additionalEnemyHitEffect.GetModifiedValue(null);
		}
		else
		{
			cachedAdditionalEnemyHitEffect = null;
		}
		this.m_cachedAdditionalEnemyHitEffect = cachedAdditionalEnemyHitEffect;
		this.m_cachedSingleEnemyHitEffect = ((!this.m_abilityMod) ? null : this.m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(null));
	}

	public float GetStartRadius()
	{
		return (!this.m_abilityMod) ? this.m_startRadius : this.m_abilityMod.m_startRadiusMod.GetModifiedValue(this.m_startRadius);
	}

	public float GetEndRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_endRadiusMod.GetModifiedValue(this.m_endRadius);
		}
		else
		{
			result = this.m_endRadius;
		}
		return result;
	}

	public float GetLineRadius()
	{
		return (!this.m_abilityMod) ? this.m_lineRadius : this.m_abilityMod.m_lineRadiusMod.GetModifiedValue(this.m_lineRadius);
	}

	public float GetMinRangeBetween()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_minRangeBetweenMod.GetModifiedValue(this.m_minRangeBetween);
		}
		else
		{
			result = this.m_minRangeBetween;
		}
		return result;
	}

	public float GetMaxRangeBetween()
	{
		return (!this.m_abilityMod) ? this.m_maxRangeBetween : this.m_abilityMod.m_maxRangeBetweenMod.GetModifiedValue(this.m_maxRangeBetween);
	}

	public bool LinePenetrateLoS()
	{
		return (!this.m_abilityMod) ? this.m_linePenetrateLoS : this.m_abilityMod.m_linePenetrateLoSMod.GetModifiedValue(this.m_linePenetrateLoS);
	}

	public bool AoePenetrateLoS()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoePenetrateLoSMod.GetModifiedValue(this.m_aoePenetrateLoS);
		}
		else
		{
			result = this.m_aoePenetrateLoS;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public int GetDamageBelowHealthThreshold()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageBelowHealthThresholdMod.GetModifiedValue(this.GetDamage());
		}
		else
		{
			result = this.GetDamage();
		}
		return result;
	}

	public float GetHealthThresholdForBonusDamage()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healthThresholdForDamageMod.GetModifiedValue(0f);
		}
		else
		{
			result = 0f;
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

	public StandardEffectInfo GetAdditionalEnemyHitEffect()
	{
		return this.m_cachedAdditionalEnemyHitEffect;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return this.m_cachedSingleEnemyHitEffect;
	}

	public int GetTechPointRefundNoHits()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_techPointRefundNoHits.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = this.GetDamage();
		if (targetActor.GetHitPointShareOfMax() <= this.GetHealthThresholdForBonusDamage())
		{
			num = this.GetDamageBelowHealthThreshold();
		}
		if (this.IsReactionHealTarget(targetActor))
		{
			num += this.m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
				if (this.IsReactionHealTarget(actorTarget.m_actor))
				{
					return this.m_healArrowAbility.GetTechPointsPerHeal();
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (this.m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex)
		{
			if (!this.m_syncComp.ActorHasUsedHealReaction(base.ActorData))
			{
				return true;
			}
		}
		if (this.m_healArrowActionType != AbilityData.ActionType.INVALID_ACTION)
		{
			if (this.m_actorTargeting != null)
			{
				List<AbilityTarget> abilityTargetsInRequest = this.m_actorTargeting.GetAbilityTargetsInRequest(this.m_healArrowActionType);
				if (abilityTargetsInRequest != null)
				{
					if (abilityTargetsInRequest.Count > 0)
					{
						BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityTargetsInRequest[0].GridPos);
						ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, true, false, base.ActorData);
						if (targetableActorOnSquare == targetActor)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}
