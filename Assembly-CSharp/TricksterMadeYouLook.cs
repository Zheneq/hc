// ROGUES
// SERVER
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

#if SERVER
	private Passive_TricksterAfterImage m_tricksterPassive; // added in rogues
#endif
	
	// removed in rogues
	private static readonly int animDistToGoal = Animator.StringToHash("DistToGoal");
	private static readonly int animStartDamageReaction = Animator.StringToHash("StartDamageReaction");
	private static readonly int animAttack = Animator.StringToHash("Attack");
	private static readonly int animCinematicCam = Animator.StringToHash("CinematicCam");
	private static readonly int animStartAttack = Animator.StringToHash("StartAttack");
	// end removed in rogues

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
#if SERVER
		// added in rogues
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_tricksterPassive = component.GetPassiveOfType(typeof(Passive_TricksterAfterImage)) as Passive_TricksterAfterImage;
		}
#endif
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
		       && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(TricksterCatchMeIfYouCan)); // , true in rogues
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
			// reactor
			modelAnimator.SetFloat(animDistToGoal, 10f);
			modelAnimator.ResetTrigger(animStartDamageReaction);
			modelAnimator.SetInteger(animAttack, animationIndex);
			modelAnimator.SetBool(animCinematicCam, false);
			modelAnimator.SetTrigger(animStartAttack);
			// rogues
			// modelAnimator.SetFloat("DistToGoal", 10f);
			// modelAnimator.ResetTrigger("StartDamageReaction");
			// modelAnimator.SetInteger("Attack", animationIndex);
			// modelAnimator.SetBool("CinematicCam", false);
			// modelAnimator.SetTrigger("StartAttack");
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
			// reactor
			modelAnimator.SetInteger(animAttack, 0);
			modelAnimator.SetBool(animCinematicCam, false);
			// rogues
			// modelAnimator.SetInteger("Attack", 0);
			// modelAnimator.SetBool("CinematicCam", false);
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
	
#if SERVER
	// added in rogues
	public override BoardSquare GetModifiedMoveStartSquare(ActorData caster, List<AbilityTarget> targets)
	{
		ActorData afterImageForSwap = GetAfterImageForSwap(targets, caster);
		if (afterImageForSwap != null && afterImageForSwap.GetCurrentBoardSquare() != null)
		{
			return afterImageForSwap.GetCurrentBoardSquare();
		}
		return base.GetModifiedMoveStartSquare(caster, targets);
	}

	// added in rogues
	internal override bool IsEvadeDestinationReserved()
	{
		return true;
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (IsSimpleAction())
		{
			ActorData afterImageForSwap = GetAfterImageForSwap(targets, caster);
			if (afterImageForSwap != null)
			{
				ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[2];
				array[0] = new ServerEvadeUtils.ChargeSegment
				{
					m_pos = caster.GetCurrentBoardSquare(),
					m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
					m_end = BoardSquarePathInfo.ChargeEndType.Impact
				};
				array[1] = new ServerEvadeUtils.ChargeSegment
				{
					m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
					m_pos = afterImageForSwap.GetSquareAtPhaseStart()
				};
				float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
				array[0].m_segmentMovementSpeed = segmentMovementSpeed;
				array[1].m_segmentMovementSpeed = segmentMovementSpeed;
				return array;
			}
			else
			{
				Log.Error("Trickster swap ability did not find clone to swap with, when in single target mode");
			}
		}
		return base.GetChargePath(targets, caster, additionalData);
	}

	// added in rogues
	internal override List<ServerEvadeUtils.NonPlayerEvadeData> GetNonPlayerEvades(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerEvadeUtils.NonPlayerEvadeData> list = new List<ServerEvadeUtils.NonPlayerEvadeData>();
		ActorData afterImageForSwap = GetAfterImageForSwap(targets, caster);
		if (afterImageForSwap != null)
		{
			BoardSquare src = afterImageForSwap.GetSquareAtPhaseStart();
			BoardSquare dst = caster.GetSquareAtPhaseStart();
			float dist = src.HorizontalDistanceOnBoardTo(dst);
			float moveSpeed = m_movementDuration > 0f
				? dist / m_movementDuration
				: m_movementSpeed;
			Vector3 facingDirection = dst.ToVector3() - src.ToVector3();
			facingDirection.y = 0f;
			facingDirection.Normalize();
			list.Add(new ServerEvadeUtils.NonPlayerEvadeData(
				afterImageForSwap,
				src,
				dst,
				ActorData.MovementType.Flight,
				moveSpeed,
				facingDirection,
				true));
		}
		return list;
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_tricksterPassive != null && m_stayForNextTurn)
		{
			ActorData afterImageForSwap = GetAfterImageForSwap(targets, caster);
			if (afterImageForSwap != null)
			{
				m_tricksterPassive.ReduceSpawnedDurationCounter(afterImageForSwap.ActorIndex);
			}
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetSquareAtPhaseStart(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		if (!HitActorsInBetween() && !HasCooldownReductionForPassingThrough())
		{
			return;
		}
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> hitActors = GetHitActors(targets, caster, nonActorTargetInfo);
		int numEnemies = 0;
		foreach (ActorData actorData in hitActors)
		{
			if (actorData.GetTeam() != caster.GetTeam())
			{
				numEnemies++;
			}
			if (HitActorsInBetween())
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, loSCheckPos));
				actorHitResults.SetBaseDamage(GetDamageAmount());
				actorHitResults.AddStandardEffectInfo(GetEnemyOnHitEffect());
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		if (HasCooldownReductionForPassingThrough())
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			m_abilityMod.m_cooldownReductionForTravelHit.AppendCooldownMiscEvents(casterHitResults, false, 0, numEnemies);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		ActorData afterImageForSwap = GetAfterImageForSwap(targets, caster);
		if (afterImageForSwap == null)
		{
			return new List<ActorData>();
		}
		List<ActorData> result = AreaEffectUtils.GetActorsInRadiusOfLine(
			caster.GetSquareAtPhaseStart().ToVector3(),
			afterImageForSwap.GetSquareAtPhaseStart().ToVector3(),
			GetRadiusAroundEnds(),
			GetRadiusAroundEnds(),
			GetRadiusFromLine(),
			PenetrateLos(),
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref result);
		return result;
	}

	// added in rogues
	private ActorData GetAfterImageForSwap(List<AbilityTarget> targets, ActorData caster)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		if (validAfterImages.Count == 1)
		{
			return validAfterImages[0];
		}
		if (targets.Count > 0)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
			foreach (ActorData afterImage in validAfterImages)
			{
				if (targetSquare == afterImage.GetSquareAtPhaseStart())
				{
					return afterImage;
				}
			}
		}
		return null;
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.TricksterStats.DamageDodgedWithSwap, damageDodged);
	}
#endif
}
