using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityUtils
{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.AbilityHasTag(Ability, AbilityTags)).MethodHandle;
				}
				result = AbilityUtils.HasTagInList(tag, ability.CurrentAbilityMod.m_abilityTagsInMod);
			}
			else
			{
				bool flag3;
				if (!AbilityUtils.HasTagInList(tag, ability.m_tags))
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
					if (flag2)
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
						flag3 = AbilityUtils.HasTagInList(tag, ability.CurrentAbilityMod.m_abilityTagsInMod);
					}
					else
					{
						flag3 = false;
					}
				}
				else
				{
					flag3 = true;
				}
				result = flag3;
			}
		}
		return result;
	}

	private static bool HasTagInList(AbilityTags tagToFind, List<AbilityTags> listToCheck)
	{
		if (listToCheck != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.HasTagInList(AbilityTags, List<AbilityTags>)).MethodHandle;
			}
			for (int i = 0; i < listToCheck.Count; i++)
			{
				if (listToCheck[i] == tagToFind)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.AbilityIgnoreCover(Ability, ActorData)).MethodHandle;
			}
			bool flag;
			if (!AbilityUtils.AbilityHasTag(ability, AbilityTags.IgnoreCover))
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
				flag = ability.ForceIgnoreCover(targetActor);
			}
			else
			{
				flag = true;
			}
			result = flag;
		}
		return result;
	}

	public static bool AbilityReduceCoverEffectiveness(Ability ability, ActorData targetActor)
	{
		bool result = false;
		if (ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.AbilityReduceCoverEffectiveness(Ability, ActorData)).MethodHandle;
			}
			bool flag;
			if (!AbilityUtils.AbilityHasTag(ability, AbilityTags.ReduceCoverByHalf))
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
				flag = ability.ForceReduceCoverEffectiveness(targetActor);
			}
			else
			{
				flag = true;
			}
			result = flag;
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
		AbilityPriority result = AbilityUtils.GetLowestAbilityPriority();
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
		float num2;
		if (caster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.GetCurrentRangeInSquares(Ability, ActorData, int)).MethodHandle;
			}
			ActorStats actorStats = caster.GetActorStats();
			ActorMovement actorMovement = caster.GetActorMovement();
			if (actorStats != null)
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
				float statBaseValueFloat = actorStats.GetStatBaseValueFloat(StatType.Movement_Horizontal);
				float modifiedStatFloat = actorStats.GetModifiedStatFloat(StatType.Movement_Horizontal);
				num = Mathf.Max(modifiedStatFloat - statBaseValueFloat, 0f);
				num2 = actorMovement.CalculateMaxHorizontalMovement(false, false);
			}
			else
			{
				num = (num2 = 0f);
			}
		}
		else
		{
			num = (num2 = 0f);
		}
		float result;
		if (AbilityUtils.AbilityHasTag(ability, AbilityTags.SetRangeToCurrentMovement))
		{
			result = num2;
		}
		else if (AbilityUtils.AbilityHasTag(ability, AbilityTags.AddBonusMovementToRange))
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
			result = rangeInSquares + num;
		}
		else
		{
			result = rangeInSquares;
		}
		return result;
	}

	public static float GetCurrentMinRangeInSquares(Ability ability, ActorData caster, int targetIndex)
	{
		return ability.GetMinRangeInSquares(targetIndex);
	}

	public static HashSet<BoardSquare> GetTargetableSquaresForAbility(Ability ability, AbilityData abilityData, ActorData caster, int targetIndex)
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		Board board = Board.Get();
		float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(ability, caster, targetIndex);
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (currentRangeInSquares >= 0f)
		{
			if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.GetTargetableSquaresForAbility(Ability, AbilityData, ActorData, int)).MethodHandle;
				}
				if (targetingParadigm != Ability.TargetingParadigm.Position)
				{
					return hashSet;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			int num = Mathf.Max(0, Mathf.FloorToInt((float)caster.GetGridPosWithIncrementedHeight().x - currentRangeInSquares));
			int num2 = Mathf.Min(board.GetMaxX(), Mathf.CeilToInt((float)caster.GetGridPosWithIncrementedHeight().x + currentRangeInSquares) + 1);
			int num3 = Mathf.Max(0, Mathf.FloorToInt((float)caster.GetGridPosWithIncrementedHeight().y - currentRangeInSquares));
			int num4 = Mathf.Min(board.GetMaxY(), Mathf.CeilToInt((float)caster.GetGridPosWithIncrementedHeight().y + currentRangeInSquares) + 1);
			float currentMinRangeInSquares = AbilityUtils.GetCurrentMinRangeInSquares(ability, caster, targetIndex);
			float currentRangeInSquares2 = AbilityUtils.GetCurrentRangeInSquares(ability, caster, targetIndex);
			AbilityTarget abilityTarget = AbilityTarget.CreateSimpleAbilityTarget(caster);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					BoardSquare boardSquare = board.GetBoardSquare(i, j);
					abilityTarget.SetValuesFromBoardSquare(boardSquare, caster.GetTravelBoardSquareWorldPosition());
					if (abilityData.ValidateAbilityOnTarget(ability, abilityTarget, targetIndex, currentMinRangeInSquares, currentRangeInSquares2))
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
						hashSet.Add(boardSquare);
					}
				}
			}
		}
		return hashSet;
	}

	public static int GetTechPointRewardForInteraction(Ability ability, AbilityInteractionType interaction, bool firstTime, bool hitOnAlly = false, bool hitOnEnemy = false)
	{
		int result = 0;
		HashSet<TechPointInteractionType> hashSet = new HashSet<TechPointInteractionType>();
		TechPointInteraction[] baseTechPointInteractions = ability.GetBaseTechPointInteractions();
		foreach (TechPointInteraction techPointInteraction in baseTechPointInteractions)
		{
			if (!hashSet.Contains(techPointInteraction.m_type))
			{
				hashSet.Add(techPointInteraction.m_type);
				int num = techPointInteraction.m_amount;
				if (ability.CurrentAbilityMod != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.GetTechPointRewardForInteraction(Ability, AbilityInteractionType, bool, bool, bool)).MethodHandle;
					}
					num = ability.CurrentAbilityMod.GetModdedTechPointForInteraction(techPointInteraction.m_type, num);
				}
				AbilityUtils.AddToRewardAmountForInteraction(ref result, techPointInteraction.m_type, interaction, num, firstTime, hitOnAlly, hitOnEnemy);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (ability.CurrentAbilityMod != null)
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
			foreach (TechPointInteractionMod techPointInteractionMod in ability.CurrentAbilityMod.m_techPointInteractionMods)
			{
				if (!hashSet.Contains(techPointInteractionMod.interactionType))
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
					hashSet.Add(techPointInteractionMod.interactionType);
					int moddedTechPointForInteraction = ability.CurrentAbilityMod.GetModdedTechPointForInteraction(techPointInteractionMod.interactionType, 0);
					if (moddedTechPointForInteraction > 0)
					{
						AbilityUtils.AddToRewardAmountForInteraction(ref result, techPointInteractionMod.interactionType, interaction, moddedTechPointForInteraction, firstTime, hitOnAlly, hitOnEnemy);
					}
				}
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
		return result;
	}

	private unsafe static void AddToRewardAmountForInteraction(ref int reward, TechPointInteractionType techInteractionType, AbilityInteractionType interaction, int addAmount, bool firstTime, bool hitOnAlly, bool hitOnEnemy)
	{
		switch (techInteractionType)
		{
		case TechPointInteractionType.RewardOnCast:
			if (interaction == AbilityInteractionType.Cast)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.AddToRewardAmountForInteraction(int*, TechPointInteractionType, AbilityInteractionType, int, bool, bool, bool)).MethodHandle;
				}
				reward += addAmount;
			}
			break;
		case TechPointInteractionType.RewardOnDamage_OncePerCast:
			if (interaction == AbilityInteractionType.Damage && firstTime)
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
				reward += addAmount;
			}
			break;
		case TechPointInteractionType.RewardOnDamage_PerTarget:
			if (interaction == AbilityInteractionType.Damage)
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
				reward += addAmount;
			}
			break;
		case TechPointInteractionType.RewardOnHit_OncePerCast:
			if (interaction == AbilityInteractionType.Hit)
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
				if (firstTime)
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
					reward += addAmount;
				}
			}
			break;
		case TechPointInteractionType.RewardOnHit_PerTarget:
			if (interaction == AbilityInteractionType.Hit)
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
				reward += addAmount;
			}
			break;
		case TechPointInteractionType.RewardOnHit_PerAllyTarget:
			if (interaction == AbilityInteractionType.Hit)
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
				if (hitOnAlly)
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
					reward += addAmount;
				}
			}
			break;
		case TechPointInteractionType.RewardOnHit_PerEnemyTarget:
			if (interaction == AbilityInteractionType.Hit)
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
				if (hitOnEnemy)
				{
					reward += addAmount;
				}
			}
			break;
		}
	}

	public static int CalculateDamageForTargeter(ActorData caster, ActorData target, Ability ability, int baseDamage, bool targetInCover)
	{
		bool flag = AbilityUtils.AbilityIgnoreCover(ability, target);
		bool flag2;
		if (targetInCover)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.CalculateDamageForTargeter(ActorData, ActorData, Ability, int, bool)).MethodHandle;
			}
			flag2 = !flag;
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		bool flag4 = ability.IsDamageUnpreventable();
		ActorStatus actorStatus = target.GetActorStatus();
		if (actorStatus.HasStatus(StatusType.DamageImmune, true))
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
			if (!flag4)
			{
				return 0;
			}
		}
		int result;
		if (actorStatus.HasStatus(StatusType.ImmuneToPlayerDamage, true) && caster.GetIsHumanControlled() && !flag4)
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
			result = 0;
		}
		else
		{
			ActorStats actorStats = caster.GetActorStats();
			ActorStats actorStats2 = target.GetActorStats();
			if (GameplayMutators.Get() != null)
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
				baseDamage = Mathf.RoundToInt((float)baseDamage * GameplayMutators.GetDamageMultiplier());
			}
			bool flag5 = AbilityUtils.AbilityHasTag(ability, AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs);
			int baseDamage2 = (!flag5) ? actorStats.CalculateOutgoingDamageForTargeter(baseDamage) : baseDamage;
			int a = actorStats2.CalculateIncomingDamageForTargeter(baseDamage2);
			int num = Mathf.Max(a, 0);
			int num2 = num;
			if (flag3)
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
				bool reducedCoverEffectiveness = AbilityUtils.AbilityReduceCoverEffectiveness(ability, target);
				num2 = AbilityUtils.ApplyCoverDamageReduction(actorStats2, num2, reducedCoverEffectiveness);
			}
			result = num2;
		}
		return result;
	}

	public static int CalculateHealingForTargeter(ActorData caster, ActorData target, Ability ability, int baseHeal)
	{
		int result;
		if (target.GetActorStatus().HasStatus(StatusType.HealImmune, true))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.CalculateHealingForTargeter(ActorData, ActorData, Ability, int)).MethodHandle;
			}
			result = 0;
		}
		else
		{
			if (caster != target)
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
				if (caster.GetTeam() == target.GetTeam())
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
					if (target.GetActorStatus().HasStatus(StatusType.CantBeHelpedByTeam, true))
					{
						return 0;
					}
				}
			}
			ActorStats actorStats = caster.GetActorStats();
			if (GameplayMutators.Get() != null)
			{
				baseHeal = Mathf.RoundToInt((float)baseHeal * GameplayMutators.GetHealingMultiplier());
			}
			bool flag = AbilityUtils.AbilityHasTag(ability, AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs);
			int num;
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
				num = baseHeal;
			}
			else
			{
				num = actorStats.CalculateOutgoingHealForTargeter(baseHeal);
			}
			int num2 = num;
			result = num2;
		}
		return result;
	}

	public static int CalculateAbsorbForTargeter(ActorData caster, ActorData target, Ability ability, int baseAbsorb)
	{
		int result;
		if (target.GetActorStatus().HasStatus(StatusType.BuffImmune, true))
		{
			result = 0;
		}
		else
		{
			if (caster != target)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.CalculateAbsorbForTargeter(ActorData, ActorData, Ability, int)).MethodHandle;
				}
				if (caster.GetTeam() == target.GetTeam())
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
					if (target.GetActorStatus().HasStatus(StatusType.CantBeHelpedByTeam, true))
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
						return 0;
					}
				}
			}
			ActorStats actorStats = caster.GetActorStats();
			if (GameplayMutators.Get() != null)
			{
				baseAbsorb = Mathf.RoundToInt((float)baseAbsorb * GameplayMutators.GetAbsorbMultiplier());
			}
			bool flag = AbilityUtils.AbilityHasTag(ability, AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs);
			int num;
			if (flag)
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
				num = baseAbsorb;
			}
			else
			{
				num = actorStats.CalculateOutgoingAbsorbForTargeter(baseAbsorb);
			}
			int num2 = num;
			result = num2;
		}
		return result;
	}

	public static int CalculateTechPointsForTargeter(ActorData target, Ability ability, int baseGain)
	{
		if (GameplayMutators.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.CalculateTechPointsForTargeter(ActorData, Ability, int)).MethodHandle;
			}
			baseGain = MathUtil.RoundToIntPadded((float)baseGain * GameplayMutators.GetEnergyGainMultiplier());
		}
		int a = baseGain;
		ActorStatus actorStatus = target.GetActorStatus();
		bool flag = actorStatus.IsEnergized(true);
		bool flag2 = actorStatus.HasStatus(StatusType.SlowEnergyGain, true);
		if (flag)
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
			if (!flag2)
			{
				AbilityModPropertyInt energizedEnergyGainMod;
				if (!(GameplayMutators.Get() == null))
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
					if (GameplayMutators.Get().m_useEnergizedOverride)
					{
						energizedEnergyGainMod = GameplayMutators.Get().m_energizedEnergyGainMod;
						goto IL_B1;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				energizedEnergyGainMod = GameWideData.Get().m_energizedEnergyGainMod;
				IL_B1:
				a = energizedEnergyGainMod.GetModifiedValue(baseGain);
				goto IL_135;
			}
		}
		if (!flag)
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
			if (flag2)
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
				AbilityModPropertyInt slowEnergyGainEnergyGainMod;
				if (!(GameplayMutators.Get() == null))
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
					if (GameplayMutators.Get().m_useSlowEnergyGainOverride)
					{
						slowEnergyGainEnergyGainMod = GameplayMutators.Get().m_slowEnergyGainEnergyGainMod;
						goto IL_12C;
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
				slowEnergyGainEnergyGainMod = GameWideData.Get().m_slowEnergyGainEnergyGainMod;
				IL_12C:
				a = slowEnergyGainEnergyGainMod.GetModifiedValue(baseGain);
			}
		}
		IL_135:
		return Mathf.Max(a, 0);
	}

	public static int ApplyCoverDamageReduction(ActorStats targetStats, int initialDamage, bool reducedCoverEffectiveness)
	{
		float num = targetStats.GetModifiedStatFloat(StatType.CoverIncomingDamageMultiplier);
		if (reducedCoverEffectiveness)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.ApplyCoverDamageReduction(ActorStats, int, bool)).MethodHandle;
			}
			num = Mathf.Min(num * 1.5f, 1f);
		}
		return Mathf.RoundToInt((float)initialDamage * num);
	}

	public static int GetEnemyCount(List<ActorData> actorsToConsider, ActorData observingActor)
	{
		int num = 0;
		if (actorsToConsider != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.GetEnemyCount(List<ActorData>, ActorData)).MethodHandle;
			}
			for (int i = 0; i < actorsToConsider.Count; i++)
			{
				ActorData actorData = actorsToConsider[i];
				if (actorData.GetTeam() != observingActor.GetTeam())
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
				if (actorData.GetTeam() == observingActor.GetTeam())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtils.GetAllyCount(List<ActorData>, ActorData, bool)).MethodHandle;
					}
					if (!includeSelf)
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
						if (!(observingActor != actorData))
						{
							goto IL_52;
						}
					}
					num++;
				}
				IL_52:;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}

	public class CustomTargetDamageableResult
	{
		public ActorData m_caster;

		public DamageSource m_src;

		public bool m_targetInCover;

		public bool m_damageable;

		public string m_failReason;

		public CustomTargetDamageableResult(ActorData caster, DamageSource src, bool targetInCover)
		{
			this.m_caster = caster;
			this.m_src = src;
			this.m_targetInCover = targetInCover;
			this.m_damageable = true;
			this.m_failReason = string.Empty;
		}
	}
}
