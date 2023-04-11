using System.Collections.Generic;
using UnityEngine;

public class TricksterMadeYouLook : Ability
{
	[Header("-- Whether can queue movement evade")]
	public bool m_canQueueMoveAfterEvade;
	[Header("-- Target Actors In-Between")]
	public bool m_hitActorsInBetween;
	public float m_radiusFromLine = 1f;
	public float m_radiusAroundEnds = 1f;
	public bool m_penetrateLos;
	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 5;
	public StandardEffectInfo m_enemyOnHitEffect;
	[Header("-- Whether to stay up till next turn")]
	public bool m_stayForNextTurn = true;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private AbilityMod_TricksterMadeYouLook m_abilityMod;
	private StandardEffectInfo m_cachedEnemyOnHitEffect;

	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");
	private static readonly int animStartDamageReaction = Animator.StringToHash("StartDamageReaction");
	private static readonly int animAttack = Animator.StringToHash("Attack");
	private static readonly int animCinematicCam = Animator.StringToHash("CinematicCam");
	private static readonly int animStartAttack = Animator.StringToHash("StartAttack");

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Made You Look";
		}
		Setup();
	}

	private void Setup()
	{
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		SetCachedFields();
		bool hasSelfEffectFromBaseMod = HasSelfEffectFromBaseMod();
		if (HitActorsInBetween())
		{
			AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
				this,
				GetRadiusAroundEnds(),
				GetRadiusAroundEnds(),
				GetRadiusFromLine(),
				-1,
				true,
				PenetrateLos());
			targeter.SetAffectedGroups(true, false, hasSelfEffectFromBaseMod);
			targeter.AllowChargeThroughInvalidSquares = true;
			Targeter = targeter;
		}
		else
		{
			AbilityUtil_Targeter_Charge targeter = new AbilityUtil_Targeter_Charge(
				this,
				AbilityAreaShape.SingleSquare,
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				false)
			{
				AllowChargeThroughInvalidSquares = true
			};
			if (hasSelfEffectFromBaseMod)
			{
				targeter.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
			}
			Targeter = targeter;
		}
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return m_canQueueMoveAfterEvade && m_afterImageSyncComp != null;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyOnHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyOnHitEffectMod.GetModifiedValue(m_enemyOnHitEffect)
			: m_enemyOnHitEffect;
	}

	public bool HitActorsInBetween()
	{
		return m_abilityMod != null
			? m_abilityMod.m_hitActorsInBetweenMod.GetModifiedValue(m_hitActorsInBetween)
			: m_hitActorsInBetween;
	}

	public float GetRadiusFromLine()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusFromLineMod.GetModifiedValue(m_radiusFromLine)
			: m_radiusFromLine;
	}

	public float GetRadiusAroundEnds()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusAroundEndsMod.GetModifiedValue(m_radiusAroundEnds)
			: m_radiusAroundEnds;
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

	public StandardEffectInfo GetEnemyOnHitEffect()
	{
		return m_cachedEnemyOnHitEffect ?? m_enemyOnHitEffect;
	}

	public SpoilsSpawnData GetSpoilsSpawnDataOnDisappear(SpoilsSpawnData defaultValue)
	{
		return m_abilityMod != null
			? m_abilityMod.m_spoilsSpawnDataOnDisappear.GetModifiedValue(defaultValue)
			: defaultValue;
	}

	public bool HasCooldownReductionForPassingThrough()
	{
		return m_abilityMod != null && m_abilityMod.m_cooldownReductionForTravelHit.HasCooldownReduction();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_afterImageSyncComp != null
		       && caster != null
		       && caster.GetAbilityData() != null
		       && m_afterImageSyncComp.GetValidAfterImages().Count > 0
		       && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(TricksterCatchMeIfYouCan));
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		if (validAfterImages.Count == 1)
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null || !targetSquare.IsValidForGameplay())
		{
			return false;
		}
		foreach (ActorData afterImage in validAfterImages)
		{
			if (afterImage.GetCurrentBoardSquare() == targetSquare)
			{
				return true;
			}
		}
		return false;
	}

	public override TargetData[] GetTargetData()
	{
		if (m_afterImageSyncComp != null && m_afterImageSyncComp.GetValidAfterImages().Count == 1)
		{
			return new TargetData[0];
		}
		return base.GetTargetData();
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (m_afterImageSyncComp != null)
		{
			List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
			if (validAfterImages.Count == 1)
			{
				return AbilityTarget.CreateSimpleAbilityTarget(validAfterImages[0]);
			}
		}
		return base.CreateAbilityTargetForSimpleAction(caster);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterMadeYouLook abilityMod_TricksterMadeYouLook = modAsBase as AbilityMod_TricksterMadeYouLook;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_TricksterMadeYouLook != null
			? abilityMod_TricksterMadeYouLook.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterMadeYouLook != null
			? abilityMod_TricksterMadeYouLook.m_enemyOnHitEffectMod.GetModifiedValue(m_enemyOnHitEffect)
			: m_enemyOnHitEffect, "EnemyOnHitEffect", m_enemyOnHitEffect);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages(false);
		BoardSquare targetSquare = Board.Get().GetSquareFromVec3(targetPos);
		bool hasValidAfterimages = validAfterImages.Count > 1;
		foreach (ActorData afterImage in validAfterImages)
		{
			if (afterImage == null
			    || afterImage.IsDead()
			    || afterImage.GetCurrentBoardSquare() == null
			    || (hasValidAfterimages && afterImage.GetCurrentBoardSquare() != targetSquare))
			{
				continue;
			}
			m_afterImageSyncComp.TurnToPosition(afterImage, caster.GetFreePos());
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

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterMadeYouLook))
		{
			m_abilityMod = abilityMod as AbilityMod_TricksterMadeYouLook;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
