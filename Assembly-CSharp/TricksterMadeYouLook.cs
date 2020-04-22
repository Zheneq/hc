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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Made You Look";
		}
		Setup();
	}

	private void Setup()
	{
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		SetCachedFields();
		bool flag = HasSelfEffectFromBaseMod();
		if (HitActorsInBetween())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, GetRadiusAroundEnds(), GetRadiusAroundEnds(), GetRadiusFromLine(), -1, true, PenetrateLos());
					abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(true, false, flag);
					abilityUtil_Targeter_ChargeAoE.AllowChargeThroughInvalidSquares = true;
					base.Targeter = abilityUtil_Targeter_ChargeAoE;
					return;
				}
				}
			}
		}
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
		abilityUtil_Targeter_Charge.AllowChargeThroughInvalidSquares = true;
		if (flag)
		{
			while (true)
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

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		int result;
		if (m_canQueueMoveAfterEvade)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((m_afterImageSyncComp != null) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyOnHitEffect = ((!m_abilityMod) ? m_enemyOnHitEffect : m_abilityMod.m_enemyOnHitEffectMod.GetModifiedValue(m_enemyOnHitEffect));
	}

	public bool HitActorsInBetween()
	{
		return (!m_abilityMod) ? m_hitActorsInBetween : m_abilityMod.m_hitActorsInBetweenMod.GetModifiedValue(m_hitActorsInBetween);
	}

	public float GetRadiusFromLine()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_radiusFromLineMod.GetModifiedValue(m_radiusFromLine);
		}
		else
		{
			result = m_radiusFromLine;
		}
		return result;
	}

	public float GetRadiusAroundEnds()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_radiusAroundEndsMod.GetModifiedValue(m_radiusAroundEnds);
		}
		else
		{
			result = m_radiusAroundEnds;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public StandardEffectInfo GetEnemyOnHitEffect()
	{
		return (m_cachedEnemyOnHitEffect == null) ? m_enemyOnHitEffect : m_cachedEnemyOnHitEffect;
	}

	public SpoilsSpawnData GetSpoilsSpawnDataOnDisappear(SpoilsSpawnData defaultValue)
	{
		SpoilsSpawnData result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_spoilsSpawnDataOnDisappear.GetModifiedValue(defaultValue);
		}
		else
		{
			result = defaultValue;
		}
		return result;
	}

	public bool HasCooldownReductionForPassingThrough()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (m_abilityMod.m_cooldownReductionForTravelHit.HasCooldownReduction() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!(m_afterImageSyncComp == null))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(caster == null))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(caster.GetAbilityData() == null))
				{
					List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
					return validAfterImages.Count > 0 && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(TricksterCatchMeIfYouCan));
				}
				while (true)
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
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		if (validAfterImages.Count == 1)
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (boardSquareSafe.IsBaselineHeight())
			{
				using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (current.GetCurrentBoardSquare() == boardSquareSafe)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
					while (true)
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
			while (true)
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
		if (m_afterImageSyncComp != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
			if (validAfterImages.Count == 1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return new TargetData[0];
					}
				}
			}
		}
		return base.GetTargetData();
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		if (m_afterImageSyncComp != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
			if (validAfterImages.Count == 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						ActorData casterActor = validAfterImages[0];
						return AbilityTarget.CreateSimpleAbilityTarget(casterActor);
					}
					}
				}
			}
		}
		return base.CreateAbilityTargetForSimpleAction(caster);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterMadeYouLook abilityMod_TricksterMadeYouLook = modAsBase as AbilityMod_TricksterMadeYouLook;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_TricksterMadeYouLook)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_TricksterMadeYouLook.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterMadeYouLook) ? m_enemyOnHitEffect : abilityMod_TricksterMadeYouLook.m_enemyOnHitEffectMod.GetModifiedValue(m_enemyOnHitEffect), "EnemyOnHitEffect", m_enemyOnHitEffect);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages(false);
		BoardSquare boardSquare = Board.Get().GetBoardSquare(targetPos);
		bool flag = validAfterImages.Count > 1;
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!current.IsDead())
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (current.GetCurrentBoardSquare() != null)
						{
							while (true)
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
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!(current.GetCurrentBoardSquare() == boardSquare))
								{
									continue;
								}
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							m_afterImageSyncComp.TurnToPosition(current, caster.GetTravelBoardSquareWorldPosition());
							Animator modelAnimator = current.GetModelAnimator();
							modelAnimator.SetFloat(animDistToGoal, 10f);
							modelAnimator.ResetTrigger(animStartDamageReaction);
							modelAnimator.SetInteger(animAttack, animationIndex);
							modelAnimator.SetBool(animCinematicCam, false);
							modelAnimator.SetTrigger(animStartAttack);
						}
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null && !current.IsDead())
				{
					Animator modelAnimator = current.GetModelAnimator();
					modelAnimator.SetInteger(animAttack, 0);
					modelAnimator.SetBool(animCinematicCam, false);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterMadeYouLook))
		{
			m_abilityMod = (abilityMod as AbilityMod_TricksterMadeYouLook);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
