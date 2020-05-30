using System.Collections.Generic;
using UnityEngine;

public class ClericRangedHeal : Ability
{
	public enum ExtraHealApplyTiming
	{
		CombatEndOfInitialTurn,
		PrepPhaseOfNextTurn
	}

	[Separator("On Hit Heal/Effect", true)]
	public int m_healAmount = 30;

	public int m_selfHealIfTargetingAlly = 15;

	public StandardEffectInfo m_targetHitEffect;

	[Separator("Extra Heal Based on Enemy Hits", true)]
	public ExtraHealApplyTiming m_extraHealApplyTiming;

	public int m_extraHealOnEnemyHit;

	public int m_extraHealOnSubseqEnemyHit;

	[Separator("Extra Heal Based on Current Health", true)]
	public float m_healPerPercentHealthLost;

	[Separator("On Self", true)]
	public StandardEffectInfo m_effectOnSelf;

	[Separator("Effect in Radius", true)]
	public float m_enemyDebuffRadiusAroundTarget;

	public float m_enemyDebuffRadiusAroundCaster;

	public bool m_enemyDebuffRadiusIgnoreLoS;

	public StandardEffectInfo m_enemyDebuffInRadiusEffect;

	[Separator("Reactions", true)]
	public StandardEffectInfo m_reactionEffectForHealTarget;

	public StandardEffectInfo m_reactionEffectForCaster;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_reactionProjectileSequencePrefab;

	[Header("-- For Extra Heal Effect, if Extra Heal On Enemy Hit is used")]
	public GameObject m_extraHealPersistentSeqPrefab;

	public GameObject m_extraHealTriggerSeqPrefab;

	private AbilityMod_ClericRangedHeal m_abilityMod;

	private ClericAreaBuff m_buffAbility;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private StandardEffectInfo m_cachedEffectOnSelf;

	private StandardEffectInfo m_cachedReactionEffectForHealTarget;

	private StandardEffectInfo m_cachedReactionEffectForCaster;

	private StandardEffectInfo m_cachedEnemyDebuffInRadiusEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cleric Ranged Heal";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		m_buffAbility = (GetAbilityOfType(typeof(ClericAreaBuff)) as ClericAreaBuff);
		if (GetEnemyDebuffRadiusAroundCaster() > 0f)
		{
			base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, GetEnemyDebuffRadiusAroundCaster(), m_enemyDebuffRadiusIgnoreLoS, true, false, -1, false, false);
			base.Targeter.SetAffectedGroups(true, false, true);
			base.Targeter.SetShowArcToShape(false);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, GetEnemyDebuffRadiusAroundTarget(), m_enemyDebuffRadiusIgnoreLoS);
			base.Targeter.SetAffectedGroups(true, false, true);
			base.Targeter.SetShowArcToShape(true);
		}
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (HasTargetableActorsInDecision(caster, false, true, true, ValidateCheckPath.Ignore, m_targetData[0].m_checkLineOfSight, false))
		{
			return base.CustomCanCastValidation(caster);
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.OccupantActor != null)
		{
			if (boardSquareSafe.OccupantActor.GetTeam() == caster.GetTeam() && !boardSquareSafe.OccupantActor.IgnoreForAbilityHits)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ClericRangedHeal))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ClericRangedHeal);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedTargetHitEffect = m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = m_targetHitEffect;
		}
		m_cachedTargetHitEffect = cachedTargetHitEffect;
		StandardEffectInfo cachedEffectOnSelf;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnSelf = m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = m_effectOnSelf;
		}
		m_cachedEffectOnSelf = cachedEffectOnSelf;
		StandardEffectInfo cachedReactionEffectForHealTarget;
		if ((bool)m_abilityMod)
		{
			cachedReactionEffectForHealTarget = m_abilityMod.m_reactionEffectForHealTargetMod.GetModifiedValue(m_reactionEffectForHealTarget);
		}
		else
		{
			cachedReactionEffectForHealTarget = m_reactionEffectForHealTarget;
		}
		m_cachedReactionEffectForHealTarget = cachedReactionEffectForHealTarget;
		StandardEffectInfo cachedReactionEffectForCaster;
		if ((bool)m_abilityMod)
		{
			cachedReactionEffectForCaster = m_abilityMod.m_reactionEffectForCasterMod.GetModifiedValue(m_reactionEffectForCaster);
		}
		else
		{
			cachedReactionEffectForCaster = m_reactionEffectForCaster;
		}
		m_cachedReactionEffectForCaster = cachedReactionEffectForCaster;
		StandardEffectInfo cachedEnemyDebuffInRadiusEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyDebuffInRadiusEffect = m_abilityMod.m_enemyDebuffInRadiusEffectMod.GetModifiedValue(m_enemyDebuffInRadiusEffect);
		}
		else
		{
			cachedEnemyDebuffInRadiusEffect = m_enemyDebuffInRadiusEffect;
		}
		m_cachedEnemyDebuffInRadiusEffect = cachedEnemyDebuffInRadiusEffect;
	}

	public int GetHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healAmountMod.GetModifiedValue(m_healAmount);
		}
		else
		{
			result = m_healAmount;
		}
		return result;
	}

	public int GetSelfHealIfTargetingAlly()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealIfTargetingAllyMod.GetModifiedValue(m_selfHealIfTargetingAlly);
		}
		else
		{
			result = m_selfHealIfTargetingAlly;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedTargetHitEffect != null)
		{
			result = m_cachedTargetHitEffect;
		}
		else
		{
			result = m_targetHitEffect;
		}
		return result;
	}

	public int GetExtraHealOnEnemyHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraHealOnEnemyHitMod.GetModifiedValue(m_extraHealOnEnemyHit);
		}
		else
		{
			result = m_extraHealOnEnemyHit;
		}
		return result;
	}

	public int GetExtraHealOnSubseqEnemyHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraHealOnSubseqEnemyHitMod.GetModifiedValue(m_extraHealOnSubseqEnemyHit);
		}
		else
		{
			result = m_extraHealOnSubseqEnemyHit;
		}
		return result;
	}

	public int GetExtraHealPerTargetDistance()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraHealPerTargetDistanceMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetSelfHealAdjustIfTargetingSelf()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealAdjustIfTargetingSelfMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetHealPerPercentHealthLost()
	{
		return (!m_abilityMod) ? m_healPerPercentHealthLost : m_abilityMod.m_healPerPercentHealthLostMod.GetModifiedValue(m_healPerPercentHealthLost);
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return (m_cachedEffectOnSelf == null) ? m_effectOnSelf : m_cachedEffectOnSelf;
	}

	public StandardEffectInfo GetReactionEffectForHealTarget()
	{
		StandardEffectInfo result;
		if (m_cachedReactionEffectForHealTarget != null)
		{
			result = m_cachedReactionEffectForHealTarget;
		}
		else
		{
			result = m_reactionEffectForHealTarget;
		}
		return result;
	}

	public StandardEffectInfo GetReactionEffectForCaster()
	{
		StandardEffectInfo result;
		if (m_cachedReactionEffectForCaster != null)
		{
			result = m_cachedReactionEffectForCaster;
		}
		else
		{
			result = m_reactionEffectForCaster;
		}
		return result;
	}

	public float GetEnemyDebuffRadiusAroundTarget()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_enemyDebuffRadiusAroundTargetMod.GetModifiedValue(m_enemyDebuffRadiusAroundTarget);
		}
		else
		{
			result = m_enemyDebuffRadiusAroundTarget;
		}
		return result;
	}

	public float GetEnemyDebuffRadiusAroundCaster()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_enemyDebuffRadiusAroundCasterMod.GetModifiedValue(m_enemyDebuffRadiusAroundCaster);
		}
		else
		{
			result = m_enemyDebuffRadiusAroundCaster;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyDebuffInRadiusEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyDebuffInRadiusEffect != null)
		{
			result = m_cachedEnemyDebuffInRadiusEffect;
		}
		else
		{
			result = m_enemyDebuffInRadiusEffect;
		}
		return result;
	}

	public int GetTechPointGainPerIncomingHit()
	{
		return m_abilityMod ? m_abilityMod.m_techPointGainPerIncomingHitThisTurn.GetModifiedValue(0) : 0;
	}

	public int CalcExtraHealFromMissingHealth(ActorData healTarget)
	{
		int result = 0;
		if (GetHealPerPercentHealthLost() > 0f)
		{
			if (healTarget.HitPoints < healTarget.GetMaxHitPoints())
			{
				int num = Mathf.CeilToInt((1f - healTarget.GetHitPointShareOfMax()) * 100f);
				result = Mathf.RoundToInt(GetHealPerPercentHealthLost() * (float)num);
			}
		}
		return result;
	}

	public int CalcFinalHealOnActor(ActorData forActor, ActorData caster, ActorData actorOnTargetedSquare)
	{
		int num = 0;
		bool flag = caster == actorOnTargetedSquare;
		int num2 = GetHealAmount();
		int num3 = m_healAmount;
		if (forActor == caster)
		{
			if (!flag)
			{
				num2 = GetSelfHealIfTargetingAlly();
				num3 = m_selfHealIfTargetingAlly;
			}
		}
		if (num3 > num2)
		{
			num3 = num2;
		}
		int num4 = 0;
		if (GetExtraHealPerTargetDistance() != 0)
		{
			if (!flag)
			{
				float num5 = actorOnTargetedSquare.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
				if (num5 > 0f)
				{
					num5 -= 1f;
				}
				num4 += Mathf.RoundToInt((float)GetExtraHealPerTargetDistance() * num5);
			}
		}
		num = Mathf.Max(num3, num2 + num4);
		if (flag)
		{
			num = Mathf.Max(0, num + GetSelfHealAdjustIfTargetingSelf());
		}
		num += CalcExtraHealFromMissingHealth(forActor);
		if (m_buffAbility != null)
		{
			if (m_buffAbility.GetExtraHealForPurifyOnBuffedAllies() != 0)
			{
				if (m_buffAbility.IsActorInBuffShape(forActor))
				{
					num += m_buffAbility.GetExtraHealForPurifyOnBuffedAllies();
				}
			}
		}
		return num;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AddTokenInt(tokens, "SelfHealIfTargetingAlly", string.Empty, m_selfHealIfTargetingAlly);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
		AddTokenInt(tokens, "ExtraHealOnEnemyHit", string.Empty, m_extraHealOnEnemyHit);
		AddTokenInt(tokens, "ExtraHealOnSubseqEnemyHit", string.Empty, m_extraHealOnSubseqEnemyHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		AbilityMod.AddToken_EffectInfo(tokens, m_reactionEffectForHealTarget, "ReactionEffectForHealTarget", m_reactionEffectForHealTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_reactionEffectForCaster, "ReactionEffectForCaster", m_reactionEffectForCaster);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyDebuffInRadiusEffect, "EnemyDebuffInRadiusEffect", m_enemyDebuffInRadiusEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_selfHealIfTargetingAlly));
		return list;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		AbilityUtil_Targeter_AoE_AroundActor abilityUtil_Targeter_AoE_AroundActor = base.Targeter as AbilityUtil_Targeter_AoE_AroundActor;
		if (abilityUtil_Targeter_AoE_AroundActor != null)
		{
			if (actorData.GetTeam() == targetActor.GetTeam())
			{
				if (abilityUtil_Targeter_AoE_AroundActor.m_lastCenterActor != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							int num = results.m_healing = CalcFinalHealOnActor(targetActor, actorData, abilityUtil_Targeter_AoE_AroundActor.m_lastCenterActor);
							return true;
						}
						}
					}
				}
			}
		}
		return false;
	}
}
