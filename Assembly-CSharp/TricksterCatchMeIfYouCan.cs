using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TricksterCatchMeIfYouCan : Ability
{
	[Header("-- Hit actors in path")]
	public bool m_hitActorsInPath = true;
	public float m_pathRadius = 0.5f;
	public float m_pathStartRadius;
	public float m_pathEndRadius;
	public bool m_penetrateLos;
	public bool m_targeterAllowOccupiedSquares;
	public bool m_chargeThroughInvalidSquares;
	[Header("-- Enemy Hit")]
	public int m_damageAmount = 10;
	public int m_subsequentDamageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;
	public StandardEffectInfo m_enemyMultipleHitEffect;
	[Header("-- Ally Hit")]
	public int m_allyHealingAmount;
	public int m_subsequentHealingAmount;
	public int m_allyEnergyGain;
	public StandardEffectInfo m_allyHitEffect;
	[Space(10f)]
	public bool m_useAllyMultiHitEffect;
	public StandardEffectInfo m_allyMultipleHitEffect;
	[Space(10f)]
	public int m_selfHealingAmount;
	public StandardEffectInfo m_selfHitEffect;
	[Header("-- Sequences, use On Cast Sequence Prefab for hits")]
	public GameObject m_castSequencePrefab;
	[Header("  (assuming Simple Attached VFX Sequence, applied to caster and clones)")]
	public GameObject m_vfxOnCasterAndCloneSequencePrefab;
	public float m_hitImpactDelay = 0.35f;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private AbilityMod_TricksterCatchMeIfYouCan m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedEnemyMultipleHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedAllyMultipleHitEffect;
	private StandardEffectInfo m_cachedSelfHitEffect;

	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");
	private static readonly int animStartDamageReaction = Animator.StringToHash("StartDamageReaction");
	private static readonly int animAttack = Animator.StringToHash("Attack");
	private static readonly int animCinematicCam = Animator.StringToHash("CinematicCam");
	private static readonly int animStartAttack = Animator.StringToHash("StartAttack");

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Catch Me If You Can";
		}
		Setup();
	}

	private void Setup()
	{
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		if (m_afterImageSyncComp == null)
		{
			Debug.LogError("TricksterAfterImageNetworkBehavior not found");
		}
		SetCachedFields();
		int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
		if (expectedNumberOfTargeters < 2)
		{
			if (HitActorsInPath())
			{
				AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
					this,
					GetPathStartRadius(),
					GetPathEndRadius(), 
					GetPathRadius(),
					-1, 
					true,
					PenetrateLos());
				targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
				targeter.AllowChargeThroughInvalidSquares = m_chargeThroughInvalidSquares;
				Targeter = targeter;
			}
			else
			{
				Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true);
			}
		}
		else
		{
			ClearTargeters();
			for (int i = 0; i < expectedNumberOfTargeters; i++)
			{
				if (HitActorsInPath())
				{
					AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
						this,
						GetPathStartRadius(),
						GetPathEndRadius(),
						GetPathRadius(),
						-1,
						true,
						PenetrateLos());
					targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
					targeter.AllowChargeThroughInvalidSquares = m_chargeThroughInvalidSquares;
					if (i > 0)
					{
						targeter.SkipEvadeMovementLines = true;
					}
					Targeters.Add(targeter);
				}
				else
				{
					Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true));
				}
				Targeters[i].SetUseMultiTargetUpdate(false);
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Clamp(GetNumTargets(), 1, m_afterImageSyncComp.GetMaxAfterImageCount() + 1);
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedEnemyMultipleHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect)
			: m_enemyMultipleHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedAllyMultipleHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect)
			: m_allyMultipleHitEffect;
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
	}

	public bool HitActorsInPath()
	{
		return m_abilityMod != null
			? m_abilityMod.m_hitActorsInPathMod.GetModifiedValue(m_hitActorsInPath)
			: m_hitActorsInPath;
	}

	public float GetPathRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_pathRadiusMod.GetModifiedValue(m_pathRadius)
			: m_pathRadius;
	}

	public float GetPathStartRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_pathStartRadiusMod.GetModifiedValue(m_pathStartRadius)
			: m_pathStartRadius;
	}

	public float GetPathEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_pathEndRadiusMod.GetModifiedValue(m_pathEndRadius)
			: m_pathEndRadius;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetSubsequentDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount)
			: m_subsequentDamageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public bool UseEnemyMultiHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useEnemyMultiHitEffectMod.GetModifiedValue(m_useEnemyMultiHitEffect)
			: m_useEnemyMultiHitEffect;
	}

	public StandardEffectInfo GetEnemyMultipleHitEffect()
	{
		return m_cachedEnemyMultipleHitEffect ?? m_enemyMultipleHitEffect;
	}

	public int GetAllyHealingAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealingAmountMod.GetModifiedValue(m_allyHealingAmount)
			: m_allyHealingAmount;
	}

	public int GetSubsequentHealingAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subsequentHealingAmountMod.GetModifiedValue(m_subsequentHealingAmount)
			: m_subsequentHealingAmount;
	}

	public int GetAllyEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain)
			: m_allyEnergyGain;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public bool UseAllyMultiHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useAllyMultiHitEffectMod.GetModifiedValue(m_useAllyMultiHitEffect)
			: m_useAllyMultiHitEffect;
	}

	public StandardEffectInfo GetAllyMultipleHitEffect()
	{
		return m_cachedAllyMultipleHitEffect ?? m_allyMultipleHitEffect;
	}

	public int GetSelfHealingAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealingAmountMod.GetModifiedValue(m_selfHealingAmount)
			: m_selfHealingAmount;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public bool IncludeSelf()
	{
		return GetSelfHealingAmount() > 0 || GetSelfHitEffect().m_applyEffect;
	}

	public bool IncludeAllies()
	{
		return GetAllyHealingAmount() > 0
		       || GetAllyEnergyGain() > 0
		       || GetAllyHitEffect() != null && GetAllyHitEffect().m_applyEffect
		       || UseAllyMultiHitEffect() && GetAllyMultipleHitEffect() != null && GetAllyMultipleHitEffect().m_applyEffect;
	}

	public bool IncludeEnemies()
	{
		return GetDamageAmount() > 0
		       || GetEnemyHitEffect() != null && GetEnemyHitEffect().m_applyEffect
		       || UseEnemyMultiHitEffect() && GetEnemyMultipleHitEffect() != null && GetEnemyMultipleHitEffect().m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (IncludeEnemies())
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
			GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		if (IncludeAllies())
		{
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealingAmount());
			AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Ally, GetAllyEnergyGain());
			GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		if (IncludeSelf())
		{
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealingAmount());
			GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		bool isValid = targetSquare != null
		            && targetSquare.IsValidForGameplay()
		            && targetSquare != caster.GetCurrentBoardSquare();
		if (isValid && targetIndex > 0)
		{
			for (int i = 0; i < targetIndex; i++)
			{
				if (Board.Get().GetSquare(currentTargets[i].GridPos) == targetSquare)
				{
					isValid = false;
				}
			}
		}
		if (isValid
		    && targetSquare.OccupantActor != null
		    && !m_targeterAllowOccupiedSquares)
		{
			ActorData occupantActor = targetSquare.OccupantActor;
			if (NetworkClient.active && occupantActor.IsActorVisibleToClient())
			{
				bool isOccupiedByAfterImage = false;
				foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
				{
					if (afterImage == occupantActor)
					{
						isOccupiedByAfterImage = true;
						break;
					}
				}
				isValid = isOccupiedByAfterImage;
			}
		}
		if (isValid)
		{
			isValid = KnockbackUtils.CanBuildStraightLineChargePath(
				caster,
				targetSquare,
				caster.GetCurrentBoardSquare(),
				m_chargeThroughInvalidSquares,
				out _);
		}
		return isValid;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(TricksterMadeYouLook));
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeters[i],
				targetActor,
				currentTargeterIndex,
				GetDamageAmount(),
				GetSubsequentDamageAmount(),
				AbilityTooltipSymbol.Damage,
				AbilityTooltipSubject.Enemy);
		}
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeters[i],
				targetActor,
				currentTargeterIndex,
				GetAllyHealingAmount(),
				GetSubsequentHealingAmount(),
				AbilityTooltipSymbol.Healing,
				AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCatchMeIfYouCan abilityMod_TricksterCatchMeIfYouCan = modAsBase as AbilityMod_TricksterCatchMeIfYouCan;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "SubsequentDamageAmount", string.Empty, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount)
			: m_subsequentDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect)
			: m_enemyMultipleHitEffect, "EnemyMultipleHitEffect", m_enemyMultipleHitEffect);
		AddTokenInt(tokens, "AllyHealingAmount", string.Empty, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_allyHealingAmountMod.GetModifiedValue(m_allyHealingAmount)
			: m_allyHealingAmount);
		AddTokenInt(tokens, "SubsequentHealingAmount", string.Empty, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_subsequentHealingAmountMod.GetModifiedValue(m_subsequentHealingAmount)
			: m_subsequentHealingAmount);
		AddTokenInt(tokens, "AllyEnergyGain", string.Empty, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain)
			: m_allyEnergyGain);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect)
			: m_allyMultipleHitEffect, "AllyMultipleHitEffect", m_allyMultipleHitEffect);
		AddTokenInt(tokens, "SelfHealingAmount", string.Empty, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_selfHealingAmountMod.GetModifiedValue(m_selfHealingAmount)
			: m_selfHealingAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCatchMeIfYouCan != null
			? abilityMod_TricksterCatchMeIfYouCan.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages(false))
		{
			if (afterImage == null || afterImage.IsDead())
			{
				continue;
			}
			Animator modelAnimator = afterImage.GetModelAnimator();
			modelAnimator.SetFloat(animDistToGoal, 10f);
			modelAnimator.ResetTrigger(animStartDamageReaction);
			modelAnimator.SetInteger(animAttack, animationIndex);
			modelAnimator.SetBool(animCinematicCam, false);
			modelAnimator.SetTrigger(animStartAttack);
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages(false))
		{
			if (afterImage == null || afterImage.IsDead())
			{
				continue;
			}
			Animator modelAnimator = afterImage.GetModelAnimator();
			modelAnimator.SetInteger(animAttack, 0);
			modelAnimator.SetBool(animCinematicCam, false);
		}
	}

	public override void OnEvasionMoveStartEvent(ActorData caster)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages(false))
		{
			if (afterImage == null
			    || afterImage.IsDead()
			    || afterImage.GetActorModelData() == null)
			{
				continue;
			}
			afterImage.GetActorModelData().EnableRendererAndUpdateVisibility();
			afterImage.GetActorModelData().gameObject.transform.localScale = Vector3.one;
			TricksterAfterImageNetworkBehaviour.SetMaterialEnabledForAfterImage(caster, afterImage, true);
		}
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private int GetAllyHealAmountForHitCount(int hitCount)
	{
		if (hitCount > 1)
		{
			return GetAllyHealingAmount() + (hitCount - 1) * GetSubsequentHealingAmount();
		}
		return GetAllyHealingAmount();
	}

	private int GetDamageAmountForHitCount(int hitCount)
	{
		if (hitCount > 1)
		{
			return GetDamageAmount() + (hitCount - 1) * GetSubsequentDamageAmount();
		}
		return GetDamageAmount();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCatchMeIfYouCan))
		{
			m_abilityMod = abilityMod as AbilityMod_TricksterCatchMeIfYouCan;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
