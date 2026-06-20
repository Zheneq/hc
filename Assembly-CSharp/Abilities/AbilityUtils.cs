using System.Collections.Generic;
using UnityEngine;

public static class AbilityUtils
{
	public class CustomTargetDamageableResult
	{
		public ActorData m_caster;
		public DamageSource m_src;
		public bool m_targetInCover;
		public bool m_damageable;
		public string m_failReason;

		public CustomTargetDamageableResult(ActorData caster, DamageSource src, bool targetInCover)
		{
			m_caster = caster;
			m_src = src;
			m_targetInCover = targetInCover;
			m_damageable = true;
			m_failReason = string.Empty;
		}
	}

	public static Color s_targeterHighlightGreen = new Color(0f, 1f, 0f, 0.8f);

	public static bool AbilityHasTag(Ability ability, AbilityTags tag)
	{
		if (ability == null)
		{
			return false;
		}
		bool isOverride = ability.CurrentAbilityMod != null
		                  && ability.CurrentAbilityMod.m_tagsModType == AbilityMod.TagOverrideType.Override;
		bool isAppend = ability.CurrentAbilityMod != null
		                && ability.CurrentAbilityMod.m_tagsModType == AbilityMod.TagOverrideType.Append;
		bool result;
		if (isOverride)
		{
			result = HasTagInList(tag, ability.CurrentAbilityMod.m_abilityTagsInMod);
		}
		else
		{
			result = HasTagInList(tag, ability.m_tags)
			         || isAppend && HasTagInList(tag, ability.CurrentAbilityMod.m_abilityTagsInMod);
		}
		return result;
	}

	private static bool HasTagInList(AbilityTags tagToFind, List<AbilityTags> listToCheck)
	{
		if (listToCheck == null)
		{
			return false;
		}
		foreach (AbilityTags t in listToCheck)
		{
			if (t == tagToFind)
			{
				return true;
			}
		}
		return false;
	}

	public static bool AbilityIgnoreCover(Ability ability, ActorData targetActor)
	{
		if (ability == null)
		{
			return false;
		}
		return AbilityHasTag(ability, AbilityTags.IgnoreCover) || ability.ForceIgnoreCover(targetActor);
	}

	public static bool AbilityReduceCoverEffectiveness(Ability ability, ActorData targetActor)
	{
		if (ability == null)
		{
			return false;
		}
		return AbilityHasTag(ability, AbilityTags.ReduceCoverByHalf)
		       || ability.ForceReduceCoverEffectiveness(targetActor);
	}

	public static AbilityPriority GetHighestAbilityPriority()
	{
		return AbilityPriority.Prep_Defense;
	}

	public static AbilityPriority GetLowestAbilityPriority()
	{
		return AbilityPriority.Combat_Final;
	}

	public static AbilityPriority GetNextAbilityPriority(AbilityPriority previousPriority)
	{
		switch (previousPriority)
		{
		case AbilityPriority.Prep_Defense:
			return AbilityPriority.Prep_Offense;
		case AbilityPriority.Prep_Offense:
			return AbilityPriority.Evasion;
		case AbilityPriority.Evasion:
			return AbilityPriority.Combat_Damage;
		case AbilityPriority.Combat_Damage:
			return AbilityPriority.DEPRICATED_Combat_Charge;
		case AbilityPriority.DEPRICATED_Combat_Charge:
			return AbilityPriority.Combat_Knockback;
		case AbilityPriority.Combat_Knockback:
			return AbilityPriority.Combat_Final;
		case AbilityPriority.Combat_Final:
			return AbilityPriority.Combat_Final;
		}
		return GetLowestAbilityPriority();
	}

	public static float GetCurrentRangeInSquares(Ability ability, ActorData caster, int targetIndex)
	{
		float rangeInSquares = ability.GetRangeInSquares(targetIndex);
		float num = 0f;
		float result = 0f;
		if (caster != null)
		{
			ActorStats actorStats = caster.GetActorStats();
			ActorMovement actorMovement = caster.GetActorMovement();
			if (actorStats != null)
			{
				float statBaseValueFloat = actorStats.GetStatBaseValueFloat(StatType.Movement_Horizontal);
				float modifiedStatFloat = actorStats.GetModifiedStatFloat(StatType.Movement_Horizontal);
				num = Mathf.Max(modifiedStatFloat - statBaseValueFloat, 0f);
				result = actorMovement.CalculateMaxHorizontalMovement();
			}
		}
		if (AbilityHasTag(ability, AbilityTags.SetRangeToCurrentMovement))
		{
			return result;
		}
		if (AbilityHasTag(ability, AbilityTags.AddBonusMovementToRange))
		{
			return rangeInSquares + num;
		}
		return rangeInSquares;
	}

	public static float GetCurrentMinRangeInSquares(Ability ability, ActorData caster, int targetIndex)
	{
		return ability.GetMinRangeInSquares(targetIndex);
	}

	public static HashSet<BoardSquare> GetTargetableSquaresForAbility(Ability ability, AbilityData abilityData, ActorData caster, int targetIndex)
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		Board board = Board.Get();
		float currentRangeInSquares = GetCurrentRangeInSquares(ability, caster, targetIndex);
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (currentRangeInSquares < 0f)
		{
			return hashSet;
		}
		if (targetingParadigm != Ability.TargetingParadigm.BoardSquare
		    && targetingParadigm != Ability.TargetingParadigm.Position)
		{
			return hashSet;
		}
		int x1 = Mathf.Max(0, Mathf.FloorToInt(caster.GetGridPos().x - currentRangeInSquares));
		int x2 = Mathf.Min(board.GetMaxX(), Mathf.CeilToInt(caster.GetGridPos().x + currentRangeInSquares) + 1);
		int y1 = Mathf.Max(0, Mathf.FloorToInt(caster.GetGridPos().y - currentRangeInSquares));
		int y2 = Mathf.Min(board.GetMaxY(), Mathf.CeilToInt(caster.GetGridPos().y + currentRangeInSquares) + 1);
		float currentMinRangeInSquares = GetCurrentMinRangeInSquares(ability, caster, targetIndex);
		float currentMaxRangeInSquares = GetCurrentRangeInSquares(ability, caster, targetIndex);
		AbilityTarget abilityTarget = AbilityTarget.CreateSimpleAbilityTarget(caster);
		for (int i = x1; i < x2; i++)
		{
			for (int j = y1; j < y2; j++)
			{
				BoardSquare boardSquare = board.GetSquareFromIndex(i, j);
				abilityTarget.SetValuesFromBoardSquare(boardSquare, caster.GetFreePos());
				if (abilityData.ValidateAbilityOnTarget(
					    ability,
					    abilityTarget,
					    targetIndex,
					    currentMinRangeInSquares,
					    currentMaxRangeInSquares))
				{
					hashSet.Add(boardSquare);
				}
			}
		}
		return hashSet;
	}

	public static int GetTechPointRewardForInteraction(
		Ability ability,
		AbilityInteractionType interaction,
		bool firstTime,
		bool hitOnAlly = false,
		bool hitOnEnemy = false)
	{
		int reward = 0;
		HashSet<TechPointInteractionType> hashSet = new HashSet<TechPointInteractionType>();
		TechPointInteraction[] baseTechPointInteractions = ability.GetBaseTechPointInteractions();
		foreach (TechPointInteraction techPointInteraction in baseTechPointInteractions)
		{
			if (!hashSet.Contains(techPointInteraction.m_type))
			{
				hashSet.Add(techPointInteraction.m_type);
				int techPoints = techPointInteraction.m_amount;
				if (ability.CurrentAbilityMod != null)
				{
					techPoints = ability.CurrentAbilityMod.GetModdedTechPointForInteraction(techPointInteraction.m_type, techPoints);
				}
				AddToRewardAmountForInteraction(ref reward, techPointInteraction.m_type, interaction, techPoints, firstTime, hitOnAlly, hitOnEnemy);
			}
		}
		if (ability.CurrentAbilityMod != null)
		{
			foreach (TechPointInteractionMod techPointInteractionMod in ability.CurrentAbilityMod.m_techPointInteractionMods)
			{
				if (!hashSet.Contains(techPointInteractionMod.interactionType))
				{
					hashSet.Add(techPointInteractionMod.interactionType);
					int techPoints = ability.CurrentAbilityMod.GetModdedTechPointForInteraction(techPointInteractionMod.interactionType, 0);
					if (techPoints > 0)
					{
						AddToRewardAmountForInteraction(
							ref reward,
							techPointInteractionMod.interactionType,
							interaction,
							techPoints,
							firstTime,
							hitOnAlly,
							hitOnEnemy);
					}
				}
			}
		}
		return reward;
	}

	private static void AddToRewardAmountForInteraction(
		ref int reward,
		TechPointInteractionType techInteractionType,
		AbilityInteractionType interaction,
		int addAmount,
		bool firstTime,
		bool hitOnAlly,
		bool hitOnEnemy)
	{
		switch (techInteractionType)
		{
			case TechPointInteractionType.RewardOnCast:
				if (interaction == AbilityInteractionType.Cast)
				{
					reward += addAmount;
				}
				break;

			case TechPointInteractionType.RewardOnDamage_OncePerCast:
				if (interaction == AbilityInteractionType.Damage && firstTime)
				{
					reward += addAmount;
				}
				break;

			case TechPointInteractionType.RewardOnDamage_PerTarget:
				if (interaction == AbilityInteractionType.Damage)
				{
					reward += addAmount;
				}
				break;

			case TechPointInteractionType.RewardOnHit_OncePerCast:
				if (interaction == AbilityInteractionType.Hit && firstTime)
				{
					reward += addAmount;
				}
				break;

			case TechPointInteractionType.RewardOnHit_PerTarget:
				if (interaction == AbilityInteractionType.Hit)
				{
					reward += addAmount;
				}
				break;

			case TechPointInteractionType.RewardOnHit_PerAllyTarget:
				if (interaction == AbilityInteractionType.Hit && hitOnAlly)
				{

					reward += addAmount;
				}
				break;

			case TechPointInteractionType.RewardOnHit_PerEnemyTarget:
				if (interaction == AbilityInteractionType.Hit && hitOnEnemy)
				{
					reward += addAmount;
				}
				break;
		}
	}

	public static int CalculateDamageForTargeter(ActorData caster, ActorData target, Ability ability, int baseDamage, bool targetInCover)
	{
		bool isInCover = targetInCover && !AbilityIgnoreCover(ability, target);
		bool isUnpreventable = ability.IsDamageUnpreventable();
		ActorStatus targetStatus = target.GetActorStatus();
		if (targetStatus.HasStatus(StatusType.DamageImmune) && !isUnpreventable)
		{
			return 0;
		}
		if (targetStatus.HasStatus(StatusType.ImmuneToPlayerDamage) && caster.IsHumanControlled() && !isUnpreventable)
		{
			return 0;
		}
		ActorStats casterStats = caster.GetActorStats();
		ActorStats targetStats = target.GetActorStats();
		if (GameplayMutators.Get() != null)
		{
			baseDamage = Mathf.RoundToInt(baseDamage * GameplayMutators.GetDamageMultiplier());
		}
		if (!AbilityHasTag(ability, AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs))
		{
			baseDamage = casterStats.CalculateOutgoingDamageForTargeter(baseDamage);
		}
		int damage = Mathf.Max(targetStats.CalculateIncomingDamageForTargeter(baseDamage), 0);
		if (isInCover)
		{
			bool reducedCoverEffectiveness = AbilityReduceCoverEffectiveness(ability, target);
			damage = ApplyCoverDamageReduction(targetStats, damage, reducedCoverEffectiveness);
		}
		return damage;
	}

	public static int CalculateHealingForTargeter(ActorData caster, ActorData target, Ability ability, int baseHeal)
	{
		if (target.GetActorStatus().HasStatus(StatusType.HealImmune))
		{
			return 0;
		}
		if (caster != target
		    && caster.GetTeam() == target.GetTeam()
		    && target.GetActorStatus().HasStatus(StatusType.CantBeHelpedByTeam))
		{
			return 0;
		}
		if (GameplayMutators.Get() != null)
		{
			baseHeal = Mathf.RoundToInt(baseHeal * GameplayMutators.GetHealingMultiplier());
		}
		if (AbilityHasTag(ability, AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs))
		{
			return baseHeal;
		}
		else
		{
			return caster.GetActorStats().CalculateOutgoingHealForTargeter(baseHeal);
		}
	}

	public static int CalculateAbsorbForTargeter(ActorData caster, ActorData target, Ability ability, int baseAbsorb)
	{
		if (target.GetActorStatus().HasStatus(StatusType.BuffImmune))
		{
			return 0;
		}
		if (caster != target
		    && caster.GetTeam() == target.GetTeam()
		    && target.GetActorStatus().HasStatus(StatusType.CantBeHelpedByTeam))
		{
			return 0;
		}
		if (GameplayMutators.Get() != null)
		{
			baseAbsorb = Mathf.RoundToInt(baseAbsorb * GameplayMutators.GetAbsorbMultiplier());
		}
		if (AbilityHasTag(ability, AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs))
		{
			return baseAbsorb;
		}
		else
		{
			return caster.GetActorStats().CalculateOutgoingAbsorbForTargeter(baseAbsorb);
		}
	}

	public static int CalculateTechPointsForTargeter(ActorData target, Ability ability, int baseGain)
	{
		if (GameplayMutators.Get() != null)
		{
			baseGain = MathUtil.RoundToIntPadded(baseGain * GameplayMutators.GetEnergyGainMultiplier());
		}
		int finalGain = baseGain;
		ActorStatus actorStatus = target.GetActorStatus();
		bool isEnergized = actorStatus.IsEnergized();
		bool isSlowEnergyGain = actorStatus.HasStatus(StatusType.SlowEnergyGain);
		if (isEnergized && !isSlowEnergyGain)
		{
			AbilityModPropertyInt energizedEnergyGainMod =
				GameplayMutators.Get() != null && GameplayMutators.Get().m_useEnergizedOverride
				? GameplayMutators.Get().m_energizedEnergyGainMod
				: GameWideData.Get().m_energizedEnergyGainMod;
			finalGain = energizedEnergyGainMod.GetModifiedValue(baseGain);
		}
		else if (!isEnergized && isSlowEnergyGain)
		{
			AbilityModPropertyInt slowEnergyGainEnergyGainMod =
				GameplayMutators.Get() != null && GameplayMutators.Get().m_useSlowEnergyGainOverride
					? GameplayMutators.Get().m_slowEnergyGainEnergyGainMod
					: GameWideData.Get().m_slowEnergyGainEnergyGainMod;
			finalGain = slowEnergyGainEnergyGainMod.GetModifiedValue(baseGain);
		}
		return Mathf.Max(finalGain, 0);
	}

	public static int ApplyCoverDamageReduction(ActorStats targetStats, int initialDamage, bool reducedCoverEffectiveness)
	{
		float num = targetStats.GetModifiedStatFloat(StatType.CoverIncomingDamageMultiplier);
		if (reducedCoverEffectiveness)
		{
			num = Mathf.Min(num * 1.5f, 1f);
		}
		return Mathf.RoundToInt(initialDamage * num);
	}

	public static int GetEnemyCount(List<ActorData> actorsToConsider, ActorData observingActor)
	{
		if (actorsToConsider == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ActorData actorData in actorsToConsider)
		{
			if (actorData.GetTeam() != observingActor.GetTeam())
			{
				num++;
			}
		}
		return num;
	}

	public static int GetAllyCount(List<ActorData> actorsToConsider, ActorData observingActor, bool includeSelf)
	{
		if (actorsToConsider == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ActorData actorData in actorsToConsider)
		{
			if (actorData.GetTeam() == observingActor.GetTeam() && (includeSelf || observingActor != actorData))
			{
				num++;
			}
		}
		return num;
	}
}
