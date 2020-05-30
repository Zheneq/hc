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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (HitActorsInPath())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, GetPathStartRadius(), GetPathEndRadius(), GetPathRadius(), -1, true, PenetrateLos());
								abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
								abilityUtil_Targeter_ChargeAoE.AllowChargeThroughInvalidSquares = m_chargeThroughInvalidSquares;
								base.Targeter = abilityUtil_Targeter_ChargeAoE;
								return;
							}
							}
						}
					}
					base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true);
					return;
				}
			}
		}
		ClearTargeters();
		for (int i = 0; i < expectedNumberOfTargeters; i++)
		{
			if (HitActorsInPath())
			{
				AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE2 = new AbilityUtil_Targeter_ChargeAoE(this, GetPathStartRadius(), GetPathEndRadius(), GetPathRadius(), -1, true, PenetrateLos());
				abilityUtil_Targeter_ChargeAoE2.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
				abilityUtil_Targeter_ChargeAoE2.AllowChargeThroughInvalidSquares = m_chargeThroughInvalidSquares;
				if (i > 0)
				{
					abilityUtil_Targeter_ChargeAoE2.SkipEvadeMovementLines = true;
				}
				base.Targeters.Add(abilityUtil_Targeter_ChargeAoE2);
			}
			else
			{
				base.Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true));
			}
			base.Targeters[i].SetUseMultiTargetUpdate(false);
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Clamp(GetNumTargets(), 1, m_afterImageSyncComp.GetMaxAfterImageCount() + 1);
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = ((!m_abilityMod) ? m_enemyHitEffect : m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect));
		m_cachedEnemyMultipleHitEffect = ((!m_abilityMod) ? m_enemyMultipleHitEffect : m_abilityMod.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect));
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
		m_cachedAllyMultipleHitEffect = ((!m_abilityMod) ? m_allyMultipleHitEffect : m_abilityMod.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect));
		StandardEffectInfo cachedSelfHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedSelfHitEffect = m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = m_selfHitEffect;
		}
		m_cachedSelfHitEffect = cachedSelfHitEffect;
	}

	public bool HitActorsInPath()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_hitActorsInPathMod.GetModifiedValue(m_hitActorsInPath);
		}
		else
		{
			result = m_hitActorsInPath;
		}
		return result;
	}

	public float GetPathRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_pathRadiusMod.GetModifiedValue(m_pathRadius);
		}
		else
		{
			result = m_pathRadius;
		}
		return result;
	}

	public float GetPathStartRadius()
	{
		return (!m_abilityMod) ? m_pathStartRadius : m_abilityMod.m_pathStartRadiusMod.GetModifiedValue(m_pathStartRadius);
	}

	public float GetPathEndRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_pathEndRadiusMod.GetModifiedValue(m_pathEndRadius);
		}
		else
		{
			result = m_pathEndRadius;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
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
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public int GetSubsequentDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount);
		}
		else
		{
			result = m_subsequentDamageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public bool UseEnemyMultiHitEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useEnemyMultiHitEffectMod.GetModifiedValue(m_useEnemyMultiHitEffect);
		}
		else
		{
			result = m_useEnemyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultipleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyMultipleHitEffect != null)
		{
			result = m_cachedEnemyMultipleHitEffect;
		}
		else
		{
			result = m_enemyMultipleHitEffect;
		}
		return result;
	}

	public int GetAllyHealingAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyHealingAmountMod.GetModifiedValue(m_allyHealingAmount);
		}
		else
		{
			result = m_allyHealingAmount;
		}
		return result;
	}

	public int GetSubsequentHealingAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_subsequentHealingAmountMod.GetModifiedValue(m_subsequentHealingAmount);
		}
		else
		{
			result = m_subsequentHealingAmount;
		}
		return result;
	}

	public int GetAllyEnergyGain()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain);
		}
		else
		{
			result = m_allyEnergyGain;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
		{
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public bool UseAllyMultiHitEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useAllyMultiHitEffectMod.GetModifiedValue(m_useAllyMultiHitEffect);
		}
		else
		{
			result = m_useAllyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultipleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyMultipleHitEffect != null)
		{
			result = m_cachedAllyMultipleHitEffect;
		}
		else
		{
			result = m_allyMultipleHitEffect;
		}
		return result;
	}

	public int GetSelfHealingAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealingAmountMod.GetModifiedValue(m_selfHealingAmount);
		}
		else
		{
			result = m_selfHealingAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
		{
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
		}
		return result;
	}

	public bool IncludeSelf()
	{
		int result;
		if (GetSelfHealingAmount() <= 0)
		{
			result = (GetSelfHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IncludeAllies()
	{
		int result;
		if (GetAllyHealingAmount() <= 0)
		{
			if (GetAllyEnergyGain() <= 0)
			{
				if (GetAllyHitEffect() != null)
				{
					if (GetAllyHitEffect().m_applyEffect)
					{
						goto IL_007d;
					}
				}
				if (UseAllyMultiHitEffect())
				{
					if (GetAllyMultipleHitEffect() != null)
					{
						result = (GetAllyMultipleHitEffect().m_applyEffect ? 1 : 0);
						goto IL_007e;
					}
				}
				result = 0;
				goto IL_007e;
			}
		}
		goto IL_007d;
		IL_007e:
		return (byte)result != 0;
		IL_007d:
		result = 1;
		goto IL_007e;
	}

	public bool IncludeEnemies()
	{
		if (GetDamageAmount() > 0)
		{
			goto IL_0069;
		}
		if (GetEnemyHitEffect() != null)
		{
			if (GetEnemyHitEffect().m_applyEffect)
			{
				goto IL_0069;
			}
		}
		int result;
		if (UseEnemyMultiHitEffect() && GetEnemyMultipleHitEffect() != null)
		{
			result = (GetEnemyMultipleHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_006a;
		IL_0069:
		result = 1;
		goto IL_006a;
		IL_006a:
		return (byte)result != 0;
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
		bool flag = true;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
				{
					goto IL_0067;
				}
			}
		}
		flag = false;
		goto IL_0067;
		IL_0067:
		if (flag)
		{
			if (targetIndex > 0)
			{
				for (int i = 0; i < targetIndex; i++)
				{
					if (Board.Get().GetSquare(currentTargets[i].GridPos) == boardSquareSafe)
					{
						flag = false;
					}
				}
			}
		}
		if (flag)
		{
			if (boardSquareSafe.OccupantActor != null)
			{
				if (!m_targeterAllowOccupiedSquares)
				{
					ActorData occupantActor = boardSquareSafe.OccupantActor;
					int num;
					if (NetworkClient.active)
					{
						num = (occupantActor.IsVisibleToClient() ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					if (num != 0)
					{
						List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
						bool flag2 = false;
						using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
								ActorData current = enumerator.Current;
								if (current == occupantActor)
								{
									flag2 = true;
									break;
								}
							}
						}
						flag = flag2;
					}
				}
			}
		}
		if (flag)
		{
			flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), m_chargeThroughInvalidSquares, out int _);
		}
		return flag;
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
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeters[i], targetActor, currentTargeterIndex, GetDamageAmount(), GetSubsequentDamageAmount(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy);
		}
		for (int j = 0; j <= currentTargeterIndex; j++)
		{
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeters[j], targetActor, currentTargeterIndex, GetAllyHealingAmount(), GetSubsequentHealingAmount(), AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCatchMeIfYouCan abilityMod_TricksterCatchMeIfYouCan = modAsBase as AbilityMod_TricksterCatchMeIfYouCan;
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_TricksterCatchMeIfYouCan) ? m_damageAmount : abilityMod_TricksterCatchMeIfYouCan.m_damageAmountMod.GetModifiedValue(m_damageAmount));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			val = abilityMod_TricksterCatchMeIfYouCan.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount);
		}
		else
		{
			val = m_subsequentDamageAmount;
		}
		AddTokenInt(tokens, "SubsequentDamageAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			effectInfo = abilityMod_TricksterCatchMeIfYouCan.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			effectInfo2 = abilityMod_TricksterCatchMeIfYouCan.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect);
		}
		else
		{
			effectInfo2 = m_enemyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyMultipleHitEffect", m_enemyMultipleHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			val2 = abilityMod_TricksterCatchMeIfYouCan.m_allyHealingAmountMod.GetModifiedValue(m_allyHealingAmount);
		}
		else
		{
			val2 = m_allyHealingAmount;
		}
		AddTokenInt(tokens, "AllyHealingAmount", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			val3 = abilityMod_TricksterCatchMeIfYouCan.m_subsequentHealingAmountMod.GetModifiedValue(m_subsequentHealingAmount);
		}
		else
		{
			val3 = m_subsequentHealingAmount;
		}
		AddTokenInt(tokens, "SubsequentHealingAmount", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			val4 = abilityMod_TricksterCatchMeIfYouCan.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain);
		}
		else
		{
			val4 = m_allyEnergyGain;
		}
		AddTokenInt(tokens, "AllyEnergyGain", empty4, val4);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			effectInfo3 = abilityMod_TricksterCatchMeIfYouCan.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo3 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllyHitEffect", m_allyHitEffect);
		StandardEffectInfo effectInfo4;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			effectInfo4 = abilityMod_TricksterCatchMeIfYouCan.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect);
		}
		else
		{
			effectInfo4 = m_allyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "AllyMultipleHitEffect", m_allyMultipleHitEffect);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_TricksterCatchMeIfYouCan)
		{
			val5 = abilityMod_TricksterCatchMeIfYouCan.m_selfHealingAmountMod.GetModifiedValue(m_selfHealingAmount);
		}
		else
		{
			val5 = m_selfHealingAmount;
		}
		AddTokenInt(tokens, "SelfHealingAmount", empty5, val5);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterCatchMeIfYouCan) ? m_selfHitEffect : abilityMod_TricksterCatchMeIfYouCan.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect), "SelfHitEffect", m_selfHitEffect);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null)
				{
					if (!current.IsDead())
					{
						Animator modelAnimator = current.GetModelAnimator();
						modelAnimator.SetFloat(animDistToGoal, 10f);
						modelAnimator.ResetTrigger(animStartDamageReaction);
						modelAnimator.SetInteger(animAttack, animationIndex);
						modelAnimator.SetBool(animCinematicCam, false);
						modelAnimator.SetTrigger(animStartAttack);
					}
				}
			}
			while (true)
			{
				switch (2)
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
				if (current != null)
				{
					if (!current.IsDead())
					{
						Animator modelAnimator = current.GetModelAnimator();
						modelAnimator.SetInteger(animAttack, 0);
						modelAnimator.SetBool(animCinematicCam, false);
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public override void OnEvasionMoveStartEvent(ActorData caster)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages(false);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null && !current.IsDead())
				{
					if (current.GetActorModelData() != null)
					{
						current.GetActorModelData().EnableRendererAndUpdateVisibility();
						current.GetActorModelData().gameObject.transform.localScale = Vector3.one;
						TricksterAfterImageNetworkBehaviour.SetMaterialEnabledForAfterImage(caster, current, true);
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return GetAllyHealingAmount() + (hitCount - 1) * GetSubsequentHealingAmount();
				}
			}
		}
		return GetAllyHealingAmount();
	}

	private int GetDamageAmountForHitCount(int hitCount)
	{
		if (hitCount > 1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return GetDamageAmount() + (hitCount - 1) * GetSubsequentDamageAmount();
				}
			}
		}
		return GetDamageAmount();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_TricksterCatchMeIfYouCan))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_TricksterCatchMeIfYouCan);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
