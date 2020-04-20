using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Made You Look";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		this.SetCachedFields();
		bool flag = base.HasSelfEffectFromBaseMod();
		if (this.HitActorsInBetween())
		{
			AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, this.GetRadiusAroundEnds(), this.GetRadiusAroundEnds(), this.GetRadiusFromLine(), -1, true, this.PenetrateLos());
			abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(true, false, flag);
			abilityUtil_Targeter_ChargeAoE.AllowChargeThroughInvalidSquares = true;
			base.Targeter = abilityUtil_Targeter_ChargeAoE;
		}
		else
		{
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false);
			abilityUtil_Targeter_Charge.AllowChargeThroughInvalidSquares = true;
			if (flag)
			{
				abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
			}
			base.Targeter = abilityUtil_Targeter_Charge;
		}
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		bool result;
		if (this.m_canQueueMoveAfterEvade)
		{
			result = (this.m_afterImageSyncComp != null);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetCachedFields()
	{
		this.m_cachedEnemyOnHitEffect = ((!this.m_abilityMod) ? this.m_enemyOnHitEffect : this.m_abilityMod.m_enemyOnHitEffectMod.GetModifiedValue(this.m_enemyOnHitEffect));
	}

	public bool HitActorsInBetween()
	{
		return (!this.m_abilityMod) ? this.m_hitActorsInBetween : this.m_abilityMod.m_hitActorsInBetweenMod.GetModifiedValue(this.m_hitActorsInBetween);
	}

	public float GetRadiusFromLine()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_radiusFromLineMod.GetModifiedValue(this.m_radiusFromLine);
		}
		else
		{
			result = this.m_radiusFromLine;
		}
		return result;
	}

	public float GetRadiusAroundEnds()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_radiusAroundEndsMod.GetModifiedValue(this.m_radiusAroundEnds);
		}
		else
		{
			result = this.m_radiusAroundEnds;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public StandardEffectInfo GetEnemyOnHitEffect()
	{
		return (this.m_cachedEnemyOnHitEffect == null) ? this.m_enemyOnHitEffect : this.m_cachedEnemyOnHitEffect;
	}

	public SpoilsSpawnData GetSpoilsSpawnDataOnDisappear(SpoilsSpawnData defaultValue)
	{
		SpoilsSpawnData result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_spoilsSpawnDataOnDisappear.GetModifiedValue(defaultValue);
		}
		else
		{
			result = defaultValue;
		}
		return result;
	}

	public bool HasCooldownReductionForPassingThrough()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cooldownReductionForTravelHit.HasCooldownReduction();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!(this.m_afterImageSyncComp == null))
		{
			if (!(caster == null))
			{
				if (!(caster.GetAbilityData() == null))
				{
					List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
					return validAfterImages.Count > 0 && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(TricksterCatchMeIfYouCan));
				}
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		if (validAfterImages.Count == 1)
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.GetCurrentBoardSquare() == boardSquareSafe)
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		return false;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.m_afterImageSyncComp != null)
		{
			List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
			if (validAfterImages.Count == 1)
			{
				return new TargetData[0];
			}
		}
		return base.GetTargetData();
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (this.m_afterImageSyncComp != null)
		{
			List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
			if (validAfterImages.Count == 1)
			{
				ActorData casterActor = validAfterImages[0];
				return AbilityTarget.CreateSimpleAbilityTarget(casterActor);
			}
		}
		return base.CreateAbilityTargetForSimpleAction(caster);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterMadeYouLook abilityMod_TricksterMadeYouLook = modAsBase as AbilityMod_TricksterMadeYouLook;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_TricksterMadeYouLook)
		{
			val = abilityMod_TricksterMadeYouLook.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterMadeYouLook) ? this.m_enemyOnHitEffect : abilityMod_TricksterMadeYouLook.m_enemyOnHitEffectMod.GetModifiedValue(this.m_enemyOnHitEffect), "EnemyOnHitEffect", this.m_enemyOnHitEffect, true);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(false);
		BoardSquare boardSquare = Board.Get().GetBoardSquare(targetPos);
		bool flag = validAfterImages.Count > 1;
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
				{
					if (!actorData.IsDead())
					{
						if (actorData.GetCurrentBoardSquare() != null)
						{
							if (flag)
							{
								if (!(actorData.GetCurrentBoardSquare() == boardSquare))
								{
									continue;
								}
							}
							this.m_afterImageSyncComp.TurnToPosition(actorData, caster.GetTravelBoardSquareWorldPosition());
							Animator modelAnimator = actorData.GetModelAnimator();
							modelAnimator.SetFloat(TricksterMadeYouLook.animDistToGoal, 10f);
							modelAnimator.ResetTrigger(TricksterMadeYouLook.animStartDamageReaction);
							modelAnimator.SetInteger(TricksterMadeYouLook.animAttack, animationIndex);
							modelAnimator.SetBool(TricksterMadeYouLook.animCinematicCam, false);
							modelAnimator.SetTrigger(TricksterMadeYouLook.animStartAttack);
						}
					}
				}
			}
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null && !actorData.IsDead())
				{
					Animator modelAnimator = actorData.GetModelAnimator();
					modelAnimator.SetInteger(TricksterMadeYouLook.animAttack, 0);
					modelAnimator.SetBool(TricksterMadeYouLook.animCinematicCam, false);
				}
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterMadeYouLook))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_TricksterMadeYouLook);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
