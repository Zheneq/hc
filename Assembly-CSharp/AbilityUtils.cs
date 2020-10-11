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
		bool result = false;
		if (ability != null)
		{
			bool flag = ability.CurrentAbilityMod != null && ability.CurrentAbilityMod.m_tagsModType == AbilityMod.TagOverrideType.Override;
			bool flag2 = ability.CurrentAbilityMod != null && ability.CurrentAbilityMod.m_tagsModType == AbilityMod.TagOverrideType.Append;
			if (flag)
			{
				result = HasTagInList(tag, ability.CurrentAbilityMod.m_abilityTagsInMod);
			}
			else
			{
				int num;
				if (!HasTagInList(tag, ability.m_tags))
				{
					if (flag2)
					{
						num = (HasTagInList(tag, ability.CurrentAbilityMod.m_abilityTagsInMod) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 1;
				}
				result = ((byte)num != 0);
			}
		}
		return result;
	}

	private static bool HasTagInList(AbilityTags tagToFind, List<AbilityTags> listToCheck)
	{
		if (listToCheck != null)
		{
			for (int i = 0; i < listToCheck.Count; i++)
			{
				if (listToCheck[i] != tagToFind)
				{
					continue;
				}
				while (true)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool AbilityIgnoreCover(Ability ability, ActorData targetActor)
	{
		bool result = false;
		if (ability != null)
		{
			int num;
			if (!AbilityHasTag(ability, AbilityTags.IgnoreCover))
			{
				num = (ability.ForceIgnoreCover(targetActor) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			result = ((byte)num != 0);
		}
		return result;
	}

	public static bool AbilityReduceCoverEffectiveness(Ability ability, ActorData targetActor)
	{
		bool result = false;
		if (ability != null)
		{
			int num;
			if (!AbilityHasTag(ability, AbilityTags.ReduceCoverByHalf))
			{
				num = (ability.ForceReduceCoverEffectiveness(targetActor) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			result = ((byte)num != 0);
		}
		return result;
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
		AbilityPriority result = GetLowestAbilityPriority();
		switch (previousPriority)
		{
		case AbilityPriority.Prep_Defense:
			result = AbilityPriority.Prep_Offense;
			break;
		case AbilityPriority.Prep_Offense:
			result = AbilityPriority.Evasion;
			break;
		case AbilityPriority.Evasion:
			result = AbilityPriority.Combat_Damage;
			break;
		case AbilityPriority.Combat_Damage:
			result = AbilityPriority.DEPRICATED_Combat_Charge;
			break;
		case AbilityPriority.DEPRICATED_Combat_Charge:
			result = AbilityPriority.Combat_Knockback;
			break;
		case AbilityPriority.Combat_Knockback:
			result = AbilityPriority.Combat_Final;
			break;
		case AbilityPriority.Combat_Final:
			result = AbilityPriority.Combat_Final;
			break;
		}
		return result;
	}

	public static float GetCurrentRangeInSquares(Ability ability, ActorData caster, int targetIndex)
	{
		float rangeInSquares = ability.GetRangeInSquares(targetIndex);
		float num;
		float result;
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
			else
			{
				result = (num = 0f);
			}
		}
		else
		{
			result = (num = 0f);
		}
		if (AbilityHasTag(ability, AbilityTags.SetRangeToCurrentMovement))
		{
			return result;
		}
		if (AbilityHasTag(ability, AbilityTags.AddBonusMovementToRange))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return rangeInSquares + num;
				}
			}
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
		if (currentRangeInSquares >= 0f)
		{
			if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
			{
				if (targetingParadigm != Ability.TargetingParadigm.Position)
				{
					goto IL_0186;
				}
			}
			int num = Mathf.Max(0, Mathf.FloorToInt((float)caster.GetGridPosWithIncrementedHeight().x - currentRangeInSquares));
			int num2 = Mathf.Min(board.GetMaxX(), Mathf.CeilToInt((float)caster.GetGridPosWithIncrementedHeight().x + currentRangeInSquares) + 1);
			int num3 = Mathf.Max(0, Mathf.FloorToInt((float)caster.GetGridPosWithIncrementedHeight().y - currentRangeInSquares));
			int num4 = Mathf.Min(board.GetMaxY(), Mathf.CeilToInt((float)caster.GetGridPosWithIncrementedHeight().y + currentRangeInSquares) + 1);
			float currentMinRangeInSquares = GetCurrentMinRangeInSquares(ability, caster, targetIndex);
			float currentRangeInSquares2 = GetCurrentRangeInSquares(ability, caster, targetIndex);
			AbilityTarget abilityTarget = AbilityTarget.CreateSimpleAbilityTarget(caster);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					BoardSquare boardSquare = board.GetSquare(i, j);
					abilityTarget.SetValuesFromBoardSquare(boardSquare, caster.GetTravelBoardSquareWorldPosition());
					if (abilityData.ValidateAbilityOnTarget(ability, abilityTarget, targetIndex, currentMinRangeInSquares, currentRangeInSquares2))
					{
						hashSet.Add(boardSquare);
					}
				}
			}
		}
		goto IL_0186;
		IL_0186:
		return hashSet;
	}

	public static int GetTechPointRewardForInteraction(Ability ability, AbilityInteractionType interaction, bool firstTime, bool hitOnAlly = false, bool hitOnEnemy = false)
	{
		int reward = 0;
		HashSet<TechPointInteractionType> hashSet = new HashSet<TechPointInteractionType>();
		TechPointInteraction[] baseTechPointInteractions = ability.GetBaseTechPointInteractions();
		foreach (TechPointInteraction techPointInteraction in baseTechPointInteractions)
		{
			if (hashSet.Contains(techPointInteraction.m_type))
			{
				continue;
			}
			hashSet.Add(techPointInteraction.m_type);
			int num = techPointInteraction.m_amount;
			if (ability.CurrentAbilityMod != null)
			{
				num = ability.CurrentAbilityMod.GetModdedTechPointForInteraction(techPointInteraction.m_type, num);
			}
			AddToRewardAmountForInteraction(ref reward, techPointInteraction.m_type, interaction, num, firstTime, hitOnAlly, hitOnEnemy);
		}
		if (ability.CurrentAbilityMod != null)
		{
			TechPointInteractionMod[] techPointInteractionMods = ability.CurrentAbilityMod.m_techPointInteractionMods;
			foreach (TechPointInteractionMod techPointInteractionMod in techPointInteractionMods)
			{
				if (!hashSet.Contains(techPointInteractionMod.interactionType))
				{
					hashSet.Add(techPointInteractionMod.interactionType);
					int moddedTechPointForInteraction = ability.CurrentAbilityMod.GetModdedTechPointForInteraction(techPointInteractionMod.interactionType, 0);
					if (moddedTechPointForInteraction > 0)
					{
						AddToRewardAmountForInteraction(ref reward, techPointInteractionMod.interactionType, interaction, moddedTechPointForInteraction, firstTime, hitOnAlly, hitOnEnemy);
					}
				}
			}
		}
		return reward;
	}

	private static void AddToRewardAmountForInteraction(ref int reward, TechPointInteractionType techInteractionType, AbilityInteractionType interaction, int addAmount, bool firstTime, bool hitOnAlly, bool hitOnEnemy)
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
		ActorStatus tartgetStatus = target.GetActorStatus();
		if (tartgetStatus.HasStatus(StatusType.DamageImmune) && !isUnpreventable)
		{
			return 0;
		}
		if (tartgetStatus.HasStatus(StatusType.ImmuneToPlayerDamage) && caster.GetIsHumanControlled() && !isUnpreventable)
		{
			return 0;
		}
		ActorStats casterStats = caster.GetActorStats();
		ActorStats targetStats = target.GetActorStats();
		if (GameplayMutators.Get() != null)
		{
			baseDamage = Mathf.RoundToInt((float)baseDamage * GameplayMutators.GetDamageMultiplier());
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
		int num = baseHeal;
		if (target.GetActorStatus().HasStatus(StatusType.HealImmune))
		{
			return 0;
		}
		if (caster != target && caster.GetTeam() == target.GetTeam() && target.GetActorStatus().HasStatus(StatusType.CantBeHelpedByTeam))
		{
			return 0;
		}
		if (GameplayMutators.Get() != null)
		{
			baseHeal = Mathf.RoundToInt((float)baseHeal * GameplayMutators.GetHealingMultiplier());
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
		if (caster != target && caster.GetTeam() == target.GetTeam() && target.GetActorStatus().HasStatus(StatusType.CantBeHelpedByTeam))
		{
			return 0;
		}
		if (GameplayMutators.Get() != null)
		{
			baseAbsorb = Mathf.RoundToInt((float)baseAbsorb * GameplayMutators.GetAbsorbMultiplier());
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
			baseGain = MathUtil.RoundToIntPadded((float)baseGain * GameplayMutators.GetEnergyGainMultiplier());
		}
		int a = baseGain;
		ActorStatus actorStatus = target.GetActorStatus();
		bool flag = actorStatus.IsEnergized();
		bool flag2 = actorStatus.HasStatus(StatusType.SlowEnergyGain);
		AbilityModPropertyInt energizedEnergyGainMod;
		if (flag)
		{
			if (!flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useEnergizedOverride)
					{
						energizedEnergyGainMod = GameplayMutators.Get().m_energizedEnergyGainMod;
						goto IL_00b1;
					}
				}
				energizedEnergyGainMod = GameWideData.Get().m_energizedEnergyGainMod;
				goto IL_00b1;
			}
		}
		AbilityModPropertyInt slowEnergyGainEnergyGainMod;
		if (!flag)
		{
			if (flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useSlowEnergyGainOverride)
					{
						slowEnergyGainEnergyGainMod = GameplayMutators.Get().m_slowEnergyGainEnergyGainMod;
						goto IL_012c;
					}
				}
				slowEnergyGainEnergyGainMod = GameWideData.Get().m_slowEnergyGainEnergyGainMod;
				goto IL_012c;
			}
		}
		goto IL_0135;
		IL_0135:
		return Mathf.Max(a, 0);
		IL_00b1:
		a = energizedEnergyGainMod.GetModifiedValue(baseGain);
		goto IL_0135;
		IL_012c:
		a = slowEnergyGainEnergyGainMod.GetModifiedValue(baseGain);
		goto IL_0135;
	}

	public static int ApplyCoverDamageReduction(ActorStats targetStats, int initialDamage, bool reducedCoverEffectiveness)
	{
		float num = targetStats.GetModifiedStatFloat(StatType.CoverIncomingDamageMultiplier);
		if (reducedCoverEffectiveness)
		{
			num = Mathf.Min(num * 1.5f, 1f);
		}
		return Mathf.RoundToInt((float)initialDamage * num);
	}

	public static int GetEnemyCount(List<ActorData> actorsToConsider, ActorData observingActor)
	{
		int num = 0;
		if (actorsToConsider != null)
		{
			for (int i = 0; i < actorsToConsider.Count; i++)
			{
				ActorData actorData = actorsToConsider[i];
				if (actorData.GetTeam() != observingActor.GetTeam())
				{
					num++;
				}
			}
		}
		return num;
	}

	public static int GetAllyCount(List<ActorData> actorsToConsider, ActorData observingActor, bool includeSelf)
	{
		int num = 0;
		if (actorsToConsider != null)
		{
			for (int i = 0; i < actorsToConsider.Count; i++)
			{
				ActorData actorData = actorsToConsider[i];
				if (actorData.GetTeam() != observingActor.GetTeam())
				{
					continue;
				}
				if (!includeSelf)
				{
					if (!(observingActor != actorData))
					{
						continue;
					}
				}
				num++;
			}
		}
		return num;
	}
}
