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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.Start()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.Setup()).MethodHandle;
			}
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.CanOverrideMoveStartSquare()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.GetRadiusFromLine()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.GetRadiusAroundEnds()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.PenetrateLos()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.GetSpoilsSpawnDataOnDisappear(SpoilsSpawnData)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.HasCooldownReductionForPassingThrough()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (!(caster == null))
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
				if (!(caster.\u000E() == null))
				{
					List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
					return validAfterImages.Count > 0 && !caster.\u000E().HasQueuedAbilityOfType(typeof(TricksterCatchMeIfYouCan));
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
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
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (!(boardSquare == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
			{
				using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.\u0012() == boardSquare)
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
							return true;
						}
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
				return false;
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
		return false;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.m_afterImageSyncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.GetTargetData()).MethodHandle;
			}
			List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
			if (validAfterImages.Count == 1)
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
				return new TargetData[0];
			}
		}
		return base.GetTargetData();
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (this.m_afterImageSyncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.CreateAbilityTargetForSimpleAction(ActorData)).MethodHandle;
			}
			List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
			if (validAfterImages.Count == 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
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
		BoardSquare y = Board.\u000E().\u000E(targetPos);
		bool flag = validAfterImages.Count > 1;
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.OnAbilityAnimationRequest(ActorData, int, bool, Vector3)).MethodHandle;
					}
					if (!actorData.\u000E())
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
						if (actorData.\u0012() != null)
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
							if (flag)
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
								if (!(actorData.\u0012() == y))
								{
									continue;
								}
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							this.m_afterImageSyncComp.TurnToPosition(actorData, caster.\u0016());
							Animator animator = actorData.\u000E();
							animator.SetFloat(TricksterMadeYouLook.animDistToGoal, 10f);
							animator.ResetTrigger(TricksterMadeYouLook.animStartDamageReaction);
							animator.SetInteger(TricksterMadeYouLook.animAttack, animationIndex);
							animator.SetBool(TricksterMadeYouLook.animCinematicCam, false);
							animator.SetTrigger(TricksterMadeYouLook.animStartAttack);
						}
					}
				}
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

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null && !actorData.\u000E())
				{
					Animator animator = actorData.\u000E();
					animator.SetInteger(TricksterMadeYouLook.animAttack, 0);
					animator.SetBool(TricksterMadeYouLook.animCinematicCam, false);
				}
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterMadeYouLook.OnAbilityAnimationRequestProcessed(ActorData)).MethodHandle;
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
