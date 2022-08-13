// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;
//using CoOp;

public class ServerCombatManager : MonoBehaviour
{
	public enum DamageType
	{
		Ability,
		Effect,
		Thorns,
		Barrier
	}

	public enum HealingType
	{
		Ability,
		Effect,
		Card,
		Powerup,
		Lifesteal,
		Barrier
	}

	public enum TechPointChangeType
	{
		Ability,
		Effect,
		Barrier,
		AbilityInteraction,
		Card,
		Powerup,
		Regen
	}

#if SERVER
	// added in rogues
	private static ServerCombatManager s_instance;

	// added in rogues
	private Dictionary<ActorData, List<UnresolvedHealthChange>> m_unresolvedHealthChanges;

	// added in rogues
	private Dictionary<ActorData, List<UnresolvedHealthChange>> m_unresolvedTechPointChanges;
#endif

#if SERVER
	// added in rogues
	public static ServerCombatManager Get()
	{
		return s_instance;
	}

	// added in rogues
	private void Awake()
	{
		s_instance = this;
		m_unresolvedHealthChanges = new Dictionary<ActorData, List<UnresolvedHealthChange>>();
		m_unresolvedTechPointChanges = new Dictionary<ActorData, List<UnresolvedHealthChange>>();
	}

	// added in rogues
	private void OnDestroy()
	{
		s_instance = null;
	}

	// added in rogues
	public void ExecuteDamage(ActorHitResults actorHitResults, DamageType damageType)
	{
		ActorData caster = actorHitResults.m_hitParameters.Caster;
		ActorData target = actorHitResults.m_hitParameters.Target;
		int finalDamage = actorHitResults.FinalDamage;
		DamageSource damageSource = actorHitResults.m_hitParameters.DamageSource;
		bool inCover = damageType != DamageType.Thorns && actorHitResults.TargetInCoverWrtDamage;
		// rogues
		//HitChanceBracket.HitType hitType = actorHitResults.m_hitType;
		ApplyDamage(actorHitResults, damageType);
		LogDamage(caster, target, damageSource.Name, finalDamage, damageType, inCover); // , hitType in rogues
		GameEventManager.ActorHitHealthChangeArgs args = new GameEventManager.ActorHitHealthChangeArgs(
			GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage,
			finalDamage,
			target,
			caster,
			damageSource.IsCharacterSpecificAbility(caster));
		GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorDamaged_Server, args);
		GameplayMetricHelper.CollectDamageDealt(caster, target, finalDamage, damageSource.Ability);
		GameplayMetricHelper.CollectDamageReceived(target, finalDamage);
	}

	// added in rogues
	public static bool TargetInCover(
		DamageSource src,
		ActorData target,
		Vector3 damageOrigin,
		bool ignoreCoverMinDist,
		DamageType damageType,
		bool isFromMovement) // , out HitChanceBracketType strongestCover in rogues
	{
		ActorCover actorCover = target.GetActorCover();
		bool flag = isFromMovement || damageType == DamageType.Barrier || damageType == DamageType.Thorns || src.IgnoresCover;
		// rogues
		//strongestCover = HitChanceBracketType.Default;
		if (flag)
		{
			return false;
		}
		if (ignoreCoverMinDist)
		{
			return actorCover.IsInCoverWrtDirectionOnly(damageOrigin, target.GetCurrentBoardSquare());  // , out strongestCover in rogues
		}
		else
		{
			return actorCover.IsInCoverWrt(damageOrigin);  // , out strongestCover in rogues
		}
	}

	// added in rogues
	public int CalcDamage(
		DamageSource src,
		ActorData caster,
		ActorData target,
		int baseDamage,
		Vector3 damageOrigin,
		bool ignoreCoverMinDist,
		DamageType damageType,
		bool isFromMovement,
		out int lifeOnDamage,
		out bool targetInCoverWrtDamage,
		out bool damageBoosted,
		out bool damageReduced,
		out ServerGameplayUtils.DamageStatAdjustments damageStatAdjustments,
		ActorHitResults.DamageCalcScratch damageCalcScratch,
		bool log = false)
	{
		bool ignoresCover = isFromMovement
		                    || damageType == DamageType.Barrier
		                    || damageType == DamageType.Thorns
		                    || src.IgnoresCover;
		// rogues
		//HitChanceBracketType hitChanceBracketType;
		targetInCoverWrtDamage = TargetInCover(src, target, damageOrigin, ignoreCoverMinDist, damageType, isFromMovement);  // , out hitChanceBracketType in rogues
		bool casterInCoverWrtTarget = caster != null
			&& caster.GetActorCover().IsInCoverWrt(target.GetFreePos())  // , out hitChanceBracketType in rogues
			&& !ignoresCover;
		damageBoosted = false;
		damageReduced = false;
		if (DebugParameters.Get() != null)
		{
			if (DebugParameters.Get().GetParameterAsBool("NoDamage"))
			{
				baseDamage = 0;
			}
			else if (DebugParameters.Get().GetParameterAsBool("NoDamageToHumans") && GameplayUtils.IsHumanControlled(target))
			{
				baseDamage = 0;
			}
			else if (DebugParameters.Get().GetParameterAsBool("QuickKillDamage"))
			{
				baseDamage *= 100;
			}
			else if (DebugParameters.Get().GetParameterAsBool("QuickKillDamageByHumans") && GameplayUtils.IsHumanControlled(caster))
			{
				baseDamage *= 100;
			}
			else if (DebugParameters.Get().GetParameterAsBool("ExtraDamage"))
			{
				baseDamage *= 2;
			}
		}
		if (GameplayMutators.Get() != null)
		{
			baseDamage = Mathf.RoundToInt(baseDamage * GameplayMutators.GetDamageMultiplier());
		}
		bool isDamageUnpreventable = src.IsAbility() && src.Ability.IsDamageUnpreventable();
		string casterStr = caster == null ? "[null]" : caster.DisplayName;
		ActorStatus component = target.GetComponent<ActorStatus>();
		int finalDamage;
		if (component.HasStatus(StatusType.DamageImmune) && !isDamageUnpreventable)
		{
			finalDamage = 0;
			damageStatAdjustments = null;
			if (log)
			{
				MatchLogger.Get().Log($"{casterStr} hits {target.DisplayName} for 0 of {baseDamage} due to StatusType.DamageImmune");
			}
		}
		else if (component.HasStatus(StatusType.ImmuneToPlayerDamage) && caster != null && caster.IsHumanControlled() && !isDamageUnpreventable)
		{
			finalDamage = 0;
			damageStatAdjustments = null;
			if (log)
			{
				MatchLogger.Get().Log($"{casterStr} hits {target.DisplayName} for 0 of {baseDamage} due to StatusType.ImmuneToPlayerDamage");
			}
		}
		else if (damageType == DamageType.Effect && component.HasStatus(StatusType.EffectImmune) && !isDamageUnpreventable)
		{
			finalDamage = 0;
			damageStatAdjustments = null;
			if (log)
			{
				MatchLogger.Get().Log($"{casterStr} hits {target.DisplayName} for 0 out of {baseDamage} due to StatusType.EffectImmune");
			}
		}
		else
		{
			ActorStats actorStats = target.GetComponent<ActorStats>();
			int damageOutgoingModified = baseDamage;
			int modifiedDamageNormal = baseDamage;
			int modifiedDamageEmpowered = baseDamage;
			int modifiedDamageWeakened = baseDamage;
			if (!src.IgnoreDamageBuffsAndDebuffs() && caster != null)
			{
				damageOutgoingModified = caster.GetComponent<ActorStats>().CalculateOutgoingDamage(
					baseDamage,
					casterInCoverWrtTarget,
					false,
					out modifiedDamageNormal,
					out modifiedDamageEmpowered,
					out modifiedDamageWeakened);
			}
			damageCalcScratch.m_damageAfterOutgoingMod = damageOutgoingModified;
			int damageIncomingModified = actorStats.CalculateIncomingDamage(
				damageOutgoingModified,
				out var damageAfterIncomingBuffDebuff,
				out var damageIncomingNormal,
				out var damageIncomingVulnerable,
				out var damageIncomingArmored);
			int damageOutgoingNormal = actorStats.CalculateIncomingDamage(
				modifiedDamageNormal,
				out _,
				out _,
				out _,
				out _);
			int damageOutgoingEmpowered = actorStats.CalculateIncomingDamage(
				modifiedDamageEmpowered, 
				out _, 
				out _, 
				out _, 
				out _);
			int damageOutgoingWeakened = actorStats.CalculateIncomingDamage(
				modifiedDamageWeakened,
				out _, 
				out _, 
				out _, 
				out _);
			// custom
			int damageIncomingModifiedWithCover = damageIncomingModified;
			if (targetInCoverWrtDamage)
			{
				bool reducedCoverEffectiveness = src.Ability != null && AbilityUtils.AbilityReduceCoverEffectiveness(src.Ability, target);
				damageIncomingModifiedWithCover = AbilityUtils.ApplyCoverDamageReduction(actorStats, damageIncomingModified, reducedCoverEffectiveness);
			}
			// end custom
			damageCalcScratch.m_damageAfterIncomingBuffDebuff = damageAfterIncomingBuffDebuff;
			
			// custom
			damageCalcScratch.m_damageAfterIncomingBuffDebuffWithCover = damageIncomingModifiedWithCover;
			// rogues
			// damageCalcScratch.m_damageAfterIncomingBuffDebuffWithCover = damageAfterIncomingBuffDebuff;
			
			int damage_incomingModified = Mathf.Max(damageIncomingModified, 0);
			damageBoosted = damage_incomingModified > baseDamage;
			damageReduced = damage_incomingModified < baseDamage;
			
			// custom
			int damage_actual = Mathf.Max(damageIncomingModifiedWithCover, 0);
			// rogues
			// int damage_actual = damage_incomingModified;
			
			int damage_outgoingNormal = Mathf.Max(damageOutgoingNormal, 0);
			int damage_outgoingEmpowered = Mathf.Max(damageOutgoingEmpowered, 0);
			int damage_outgoingWeakened = Mathf.Max(damageOutgoingWeakened, 0);
			int damage_incomingNormal = Mathf.Max(damageIncomingNormal, 0);
			int damage_incomingVulnerable = Mathf.Max(damageIncomingVulnerable, 0);
			int damage_incomingArmored = Mathf.Max(damageIncomingArmored, 0);
			if (ServerActionBuffer.Get() != null && !ServerActionBuffer.Get().GatheringFakeResults)
			{
				damageStatAdjustments = new ServerGameplayUtils.DamageStatAdjustments(
					caster,
					target,
					damage_actual,
					damage_outgoingNormal,
					damage_outgoingEmpowered,
					damage_outgoingWeakened,
					damage_incomingNormal,
					damage_incomingVulnerable,
					damage_incomingArmored,
					damage_incomingModified);
			}
			else
			{
				damageStatAdjustments = null;
			}
			finalDamage = damage_actual;
			if (log)
			{
				MatchLogger.Get().Log($"{casterStr} hits {target.DisplayName} for {finalDamage} out of " +
				                      $"(base: {baseDamage}) " +
				                      $"(outgoingModified: {damageOutgoingModified}) " +
				                      $"(incomingModified: {damageIncomingModified}) " +
				                      $"(coverModified? {targetInCoverWrtDamage}: {damage_actual})");
			}
		}
		// rogues
		// AbilityData.ActionType abilityIndex = AbilityData.ActionType.INVALID_ACTION;
		// if (src.Ability != null)
		// {
		// 	abilityIndex = src.Ability.CachedActionType;
		// }
		lifeOnDamage = target.GetActorStats().CalculateLifeOnDamage(finalDamage); // , caster, (int)abilityIndex); in rogues
		return finalDamage;
	}

	// added in rogues
	private void ApplyDamage(ActorHitResults actorHitResults, DamageType damageType)
	{
		ActorData caster = actorHitResults.m_hitParameters.Caster;
		ActorData target = actorHitResults.m_hitParameters.Target;
		DamageSource damageSource = actorHitResults.m_hitParameters.DamageSource;
		int num = damageType == DamageType.Thorns
			? actorHitResults.ThornsDamage
			: actorHitResults.FinalDamage;
		if (num != 0)
		{
			target.UnresolvedDamage += num;
			UnresolvedHealthChange item = default(UnresolvedHealthChange);
			item.InitAsDamage(caster, damageSource, num);
			item.SetActorHitResults(actorHitResults);
			if (!m_unresolvedHealthChanges.ContainsKey(target))
			{
				m_unresolvedHealthChanges.Add(target, new List<UnresolvedHealthChange>());
			}
			m_unresolvedHealthChanges[target].Add(item);
			if (caster != null)
			{
				ActorBehavior actorBehavior = caster.GetActorBehavior();
				if (actorBehavior != null)
				{
					actorBehavior.CurrentTurn.RecordDamageToActor(target, num, damageSource);
				}
			}
		}
		if (BrushCoordinator.Get() != null && caster != null)
		{
			BrushCoordinator.Get().OnDamaged_HandleConcealment(target, caster, damageSource, num, damageType);
		}
		ServerEffectManager.Get().OnUnresolvedDamage(actorHitResults);
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null && num != 0)
		//{
		//	CaptureTheFlag.Get().OnUnresolvedDamage_Ctf(target, num);
		//}
	}

	// added in rogues
	private void LogDamage(
		ActorData caster,
		ActorData target,
		string sourceName,
		int appliedDamage,
		DamageType damageType,
		bool inCover) // , HitChanceBracket.HitType hitType in rogues
	{
		if (ServerActionBuffer.c_clientOnlySequences)
		{
			return;
		}
		string arg = "N";
		if (target.GetComponent<ActorStatus>().HasStatus(StatusType.DamageImmune))
		{
			arg = "I";
		}
		else if (inCover)
		{
			arg = "C";
		}
		string combatText = $"{appliedDamage}|{arg}";
		string text = caster == null ? "[null]" : caster.DisplayName;
		string text2 = $"{text}'s {sourceName} hits {target.DisplayName} for {appliedDamage}";
		if (inCover)
		{
			text2 += " (covered)";
		}
		// rogues
		//if (damageType == DamageType.Ability)
		//{
		//	text2 += string.Format("{0}", hitType);
		//}
		target.CallRpcCombatText(combatText, text2, CombatTextCategory.Damage, BuffIconToDisplay.None);
	}

	// added in rogues
	private void LogHealing(ActorData caster, ActorData target, string sourceName, int healAmount)
	{
		if (ServerActionBuffer.c_clientOnlySequences)
		{
			return;
		}
		string combatText = $"{healAmount}";
		string casterStr = caster == null ? "[null]" : caster.DisplayName;
		string logText = $"{casterStr}'s {sourceName} heals {target.DisplayName} for {healAmount}";
		target.CallRpcCombatText(combatText, logText, CombatTextCategory.Healing, BuffIconToDisplay.None);
	}

	// added in rogues
	public UnresolvedHealthChange? Heal(DamageSource src, ActorData caster, ActorData target, int baseHeal, HealingType type)
	{
		if (target.IsDead())
		{
			return null;
		}
		int num = CalcHealing(caster, target, baseHeal, src, type);
		UnresolvedHealthChange? result = ApplyHealing(caster, target, num, src, type);
		LogHealing(caster, target, src.Name, num);
		return result;
	}

	// added in rogues
	public void Heal(Ability src, ActorData caster, ActorData target, int baseHeal, HealingType type)
	{
		DamageSource healSrc = new DamageSource(src, caster.GetFreePos());
		Heal(healSrc, caster, target, baseHeal, type);
	}

	// added in rogues
	public void Heal(Passive src, ActorData caster, ActorData target, int baseHeal, HealingType type)
	{
		DamageSource healSrc = new DamageSource(src, caster.GetFreePos());
		Heal(healSrc, caster, target, baseHeal, type);
	}

	// added in rogues
	public void Heal(EffectSource src, ActorData caster, ActorData target, int baseHeal, HealingType type)
	{
		DamageSource healSrc = src.IsAbility()
			? new DamageSource(src.Ability, caster.GetFreePos())
			: new DamageSource(src.Passive, caster.GetFreePos());
		Heal(healSrc, caster, target, baseHeal, type);
	}

	// added in rogues
	public int CalcHealing(ActorData caster, ActorData target, int baseHeal, DamageSource src, HealingType type)
	{
		if (target.GetComponent<ActorStatus>().HasStatus(StatusType.HealImmune))
		{
			return 0;
		}
		if (caster != target
		    && caster != null
		    && caster.GetTeam() == target.GetTeam()
		    && target.GetComponent<ActorStatus>().HasStatus(StatusType.CantBeHelpedByTeam))
		{
			return 0;
		}
		if (GameplayMutators.Get() != null)
		{
			baseHeal = Mathf.RoundToInt(baseHeal * GameplayMutators.GetHealingMultiplier());
		}
		ActorStats component = target.GetComponent<ActorStats>();
		int heal;
		if (type == HealingType.Ability || type == HealingType.Effect)
		{
			if (caster == null)
			{
				heal = baseHeal;
			}
			else
			{
				ActorStats component2 = caster.GetComponent<ActorStats>();
				heal = component2.GetModifiedStatInt(StatType.OutgoingHealing, baseHeal);
				if (!src.IgnoreDamageBuffsAndDebuffs())
				{
					heal = component2.CalculateOutgoingHealing(baseHeal, out var num2, out var num3, out var num4);
				}
			}
		}
		else
		{
			heal = baseHeal;
		}
		// rogues
		//EquipmentStats equipmentStats = target.GetEquipmentStats();
		//if (equipmentStats != null)
		//{
		//	num = Mathf.RoundToInt((float)num * equipmentStats.GetTotalStatValueForSlot(GearStatType.IncomingHealingAdjustment, (float)target.m_baseIncomingHealingAdjustment, -1, target));
		//}
		return Mathf.Max(component.GetModifiedStatInt(StatType.IncomingHealing, heal), 0);
	}

	// added in rogues
	private void ApplyHealing(ActorHitResults actorHitResults, int finalHealing)
	{
		ActorData caster = actorHitResults.m_hitParameters.Caster;
		ActorData target = actorHitResults.m_hitParameters.Target;
		if (finalHealing < 0)
		{
			return;
		}
		target.UnresolvedHealing += finalHealing;
		UnresolvedHealthChange item = default(UnresolvedHealthChange);
		DamageSource damageSource = actorHitResults.m_hitParameters.DamageSource;
		item.InitAsHealing(caster, damageSource, finalHealing);
		item.SetActorHitResults(actorHitResults);
		if (!m_unresolvedHealthChanges.TryGetValue(target, out var list))
		{
			list = new List<UnresolvedHealthChange>();
			m_unresolvedHealthChanges[target] = list;
		}
		list.Add(item);
		if (caster == null)
		{
			return;
		}
		ActorBehavior actorBehavior = caster.GetActorBehavior();
		if (actorBehavior == null)
		{
			return;
		}
		actorBehavior.CurrentTurn.RecordHealingToActor(target, finalHealing, damageSource);
	}

	// added in rogues
	private UnresolvedHealthChange? ApplyHealing(ActorData caster, ActorData target, int finalHealing, DamageSource src, HealingType type)
	{
		if (finalHealing < 0)
		{
			return null;
		}
		target.UnresolvedHealing += finalHealing;
		UnresolvedHealthChange unresolvedHealthChange = default(UnresolvedHealthChange);
		unresolvedHealthChange.InitAsHealing(caster, src, finalHealing);
		if (!m_unresolvedHealthChanges.ContainsKey(target))
		{
			m_unresolvedHealthChanges.Add(target, new List<UnresolvedHealthChange>());
		}
		m_unresolvedHealthChanges[target].Add(unresolvedHealthChange);
		if (caster == null)
		{
			return unresolvedHealthChange;
		}
		ActorBehavior actorBehavior = caster.GetActorBehavior();
		if (actorBehavior != null)
		{
			actorBehavior.CurrentTurn.RecordHealingToActor(target, finalHealing, src);
		}
		return unresolvedHealthChange;
	}

	// added in rogues
	public void ExecuteHealing(ActorHitResults actorHitResults, HealingType type)
	{
		int heal = type == HealingType.Lifesteal
			? actorHitResults.LifestealHealingOnCaster
			: actorHitResults.FinalHealing;
		ActorData caster = actorHitResults.m_hitParameters.Caster;
		ActorData target = actorHitResults.m_hitParameters.Target;
		DamageSource damageSource = actorHitResults.m_hitParameters.DamageSource;
		ApplyHealing(actorHitResults, heal);
		LogHealing(caster, target, damageSource.Name, heal);
		GameEventManager.ActorHitHealthChangeArgs args = new GameEventManager.ActorHitHealthChangeArgs(
			GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing,
			heal,
			target,
			caster,
			damageSource.IsCharacterSpecificAbility(caster));
		GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorHealed_Server, args);
		GameplayMetricHelper.CollectHealingDealt(caster, target, heal, damageSource.Ability);
		GameplayMetricHelper.CollectHealingReceived(target, heal);
	}

	// added in rogues
	public bool HasUnresolvedHealthEntries()
	{
		return m_unresolvedHealthChanges.Count > 0;
	}

	// added in rogues
	public void ResolveHitPoints()
	{
		if (m_unresolvedHealthChanges.Count == 0)
		{
			return;
		}
		List<ActorData> actors = GameFlowData.Get().GetActors();
		Dictionary<ActorData, BoardSquare> dictionary = new Dictionary<ActorData, BoardSquare>();
		Dictionary<ActorData, BoardSquare> dictionary2 = new Dictionary<ActorData, BoardSquare>();
		foreach (ActorData actorData in new List<ActorData>(m_unresolvedHealthChanges.Keys))
		{
			bool flag = actorData.IsDead();
			BoardSquare boardSquare = actorData.GetTravelBoardSquare();
			if (boardSquare == null)
			{
				boardSquare = actorData.GetCurrentBoardSquare();
			}
			ActorBehavior actorBehavior = actorData.GetActorBehavior();
			actorBehavior.CurrentTurn.DamageTaken += actorData.UnresolvedDamage;
			actorBehavior.CurrentTurn.HealingTaken += actorData.UnresolvedHealing;
			int unresolvedDamage = actorData.UnresolvedDamage;
			int unresolvedHealing = actorData.UnresolvedHealing;
			int num = ServerEffectManager.Get().CountAbsorbPoints(actorData);
			actorData.UnresolvedDamage = ServerEffectManager.Get().AdjustDamage(actorData, actorData.UnresolvedDamage);
			actorData.SetAbsorbPoints(ServerEffectManager.Get().CountAbsorbPoints(actorData));
			int num2 = actorData.UnresolvedHealing;
			int unresolvedDamage2 = actorData.UnresolvedDamage;
			if (num2 > 0 && actorData.IsDead())
			{
				num2 = 0;
			}
			actorData.UnresolvedDamage = 0;
			actorData.UnresolvedHealing = 0;
			int num3 = actorData.HitPoints + num2 - unresolvedDamage2;
			if (num3 < 0)
			{
				int num4 = -num3;
				actorBehavior.OnOverkillDamageReceived(num4);
				ProcessOverkill(actorData, num4, actorBehavior.GetIncomingUnresolvedDamage());
			}
			int num5 = num3;
			int incomingUnresolvedHealFromAbilities = actorBehavior.GetIncomingUnresolvedHealFromAbilities();
			if (num2 > incomingUnresolvedHealFromAbilities)
			{
				num5 -= num2 - incomingUnresolvedHealFromAbilities;
			}
			int overhealAmountTotal = num5 - actorData.GetMaxHitPoints();
			ProcessOverheal(actorData, overhealAmountTotal, incomingUnresolvedHealFromAbilities);
			num3 = Mathf.Clamp(num3, 0, actorData.GetMaxHitPoints());
			actorData.SetHitPoints(num3);
			actorData.CallRpcOnHitPointsResolved(num3);
			MatchLogger.Get().Log($"{actorData.DisplayName} Resolved HP for Turn {GameFlowData.Get().CurrentTurn}, phase {ServerActionBuffer.Get().AbilityPhase} damage before effects {unresolvedDamage}, healing before effects {unresolvedHealing}, absorb before damage {num}, absorb after damage {actorData.AbsorbPoints}, damage after effects {actorData.UnresolvedDamage}, healing after effects {actorData.UnresolvedHealing}, final HitPoints {actorData.HitPoints}");
			if (flag && num3 > 0)
			{
				Log.Error($"ResolveHitPoints, actor {actorData.DebugNameString()} getting {num3} hp while dead");
			}
			foreach (UnresolvedHealthChange unresolvedHealthChange in m_unresolvedHealthChanges[actorData])
			{
				ActorData caster = unresolvedHealthChange.caster;
				ActorData actorData2 = actorData;
				DamageSource src = unresolvedHealthChange.src;
				if (unresolvedHealthChange.type == UnresolvedHealthChange.HealthChangeType.Damage)
				{
					int finalHealthAdjust = unresolvedHealthChange.finalHealthAdjust;
					if (finalHealthAdjust != 0)
					{
						if (actorData2 != null && actorData2.GetPassiveData() != null)
						{
							actorData2.GetPassiveData().OnDamaged(caster, src, finalHealthAdjust);
						}
						if (caster != null && caster.GetPassiveData() != null)
						{
							caster.GetPassiveData().OnDamagedOther(actorData2, src, finalHealthAdjust);
						}
					}
					if (finalHealthAdjust != 0)
					{
						ServerEffectManager.Get().OnDamaged(actorData2, caster, src, finalHealthAdjust, unresolvedHealthChange.actorHitResults);
					}
				}
				else
				{
					int finalHealthAdjust2 = unresolvedHealthChange.finalHealthAdjust;
					if (finalHealthAdjust2 != 0)
					{
						if (actorData2 != null && actorData2.GetPassiveData() != null)
						{
							actorData2.GetPassiveData().OnHealed(caster, src, finalHealthAdjust2);
						}
						if (caster != null && caster.GetPassiveData() != null)
						{
							caster.GetPassiveData().OnHealedOther(actorData2, src, finalHealthAdjust2);
						}
					}
					if (finalHealthAdjust2 != 0)
					{
						ServerEffectManager.Get().OnHealed(actorData2, caster, src, finalHealthAdjust2, unresolvedHealthChange.actorHitResults);
					}
				}
			}
			if (actorData.IsDead() && !flag)
			{
				// rogues
				//ServerActionBuffer.Get().GetPlayerActionFSM().MarkedToCheckActorDeath = true;
				if (actorData.GetComponent<PassiveData>())
				{
					actorData.GetComponent<PassiveData>().OnDied(m_unresolvedHealthChanges[actorData]);
				}
				if (SpoilsManager.Get() != null && SpoilsManager.Get().GetPowerUpInPos(boardSquare) != null)
				{
					dictionary2.Add(actorData, boardSquare);
				}
				else
				{
					dictionary.Add(actorData, boardSquare);
				}

				// rogues
				//if (actors.FindAll((ActorData x) => (x.GetTeam() == Team.TeamA && !x.IsDead()) || x.NextRespawnTurn > GameFlowData.Get().CurrentTurn).Count <= 0)
				//{
				//	UIDialogPopupManager.OpenRunSummaryDialog(false);
				//	if (CoOpSessionManager.Get().IsHosting())
				//	{
				//		CoOpSessionManager.Get().NotifyShowRunSummaryNotification(false);
				//	}
				//}
				//if (actorData.GetTeam() != Team.TeamA)
				//{
				//	ChallengeArchivist.Get().FireTagCheckChallengeEvent(ChallengeConditionTagCheckType.KILL_NPC, actorData.GetAllActorTags().ToList<string>());
				//}
			}
		}
		m_unresolvedHealthChanges.Clear();
		foreach (ActorData actorData3 in actors)
		{
			if (actorData3.GetActorBehavior() != null)
			{
				actorData3.GetActorBehavior().ClearUnresolvedHitpointsTracking();
			}
		}
		NotifyOnResolvedHitPoints();
		ProcessSpoilSpawns(dictionary);
		ProcessSpoilSpawns(dictionary2);
	}

	// added in rogues
	private void ProcessOverheal(ActorData actor, int overhealAmountTotal, int healFromAbilities)
	{
		if (overhealAmountTotal > 0 && healFromAbilities > 0)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData.GetActorBehavior() != null)
				{
					int unresolvedHealToTarget = actorData.GetActorBehavior().GetUnresolvedHealToTarget(actor);
					if (unresolvedHealToTarget > 0)
					{
						float num = unresolvedHealToTarget / (float)healFromAbilities;
						int amount = Mathf.RoundToInt(overhealAmountTotal * num);
						actorData.GetActorBehavior().OnOverhealDealt(amount);
					}
				}
			}
		}
	}

	// added in rogues
	private void ProcessOverkill(ActorData actor, int overkillAmountTotal, int totalUnresolvedDamage)
	{
		if (overkillAmountTotal > 0 && totalUnresolvedDamage > 0)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData.GetActorBehavior() != null)
				{
					int unresolvedDamageToTarget = actorData.GetActorBehavior().GetUnresolvedDamageToTarget(actor);
					if (unresolvedDamageToTarget > 0)
					{
						float num = unresolvedDamageToTarget / (float)totalUnresolvedDamage;
						int amount = Mathf.RoundToInt(overkillAmountTotal * num);
						actorData.GetActorBehavior().OnOverkillDamageDealt(amount);
					}
				}
			}
		}
	}

	// added in rogues
	private void ProcessSpoilSpawns(Dictionary<ActorData, BoardSquare> spoilSpawns)
	{
		foreach (ActorData actorData in spoilSpawns.Keys)
		{
			SpoilsData component = actorData.GetComponent<SpoilsData>();
			if (component != null)
			{
				component.SpawnSpoilOnDeath(spoilSpawns[actorData]);
			}
		}
	}

	// added in rogues
	private void NotifyOnResolvedHitPoints()
	{
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	CaptureTheFlag.Get().OnResolvedHitPoints_Ctf();
		//}
	}

	// added in rogues
	private void TrackUnresolvedTechPointChange(ActorData target, UnresolvedHealthChange unresolvedChange)
	{
		if (!m_unresolvedTechPointChanges.ContainsKey(target))
		{
			m_unresolvedTechPointChanges.Add(target, new List<UnresolvedHealthChange>());
		}
		m_unresolvedTechPointChanges[target].Add(unresolvedChange);
	}

	// added in rogues
	public bool HasUnresolvedTechPointsEntries()
	{
		return m_unresolvedTechPointChanges.Count > 0;
	}

	// added in rogues
	public void ResolveTechPoints()
	{
		if (m_unresolvedTechPointChanges.Count == 0)
		{
			return;
		}
		foreach (ActorData actorData in new List<ActorData>(m_unresolvedTechPointChanges.Keys))
		{
			int unresolvedTechPointGain = actorData.UnresolvedTechPointGain;
			int unresolvedTechPointLoss = actorData.UnresolvedTechPointLoss;
			actorData.UnresolvedTechPointGain = 0;
			actorData.UnresolvedTechPointLoss = 0;
			actorData.SetTechPoints(actorData.TechPoints + unresolvedTechPointGain - unresolvedTechPointLoss);

			// rogues
			//actorData.CallRpcOnTechPointsResolved(actorData.TechPoints);

			MatchLogger.Get().Log($"{actorData.DisplayName} Resolved TechPoint for Turn {GameFlowData.Get().CurrentTurn}, phase {ServerActionBuffer.Get().AbilityPhase}, TechPoint gain: {unresolvedTechPointGain}, TechPoint loss: {unresolvedTechPointLoss}, final TechPoints {actorData.TechPoints}");
		}
		m_unresolvedTechPointChanges.Clear();
	}

	// added in rogues
	public void TechPointGain(DamageSource src, ActorData caster, ActorData target, int baseGain, TechPointChangeType type)
	{
		if (!target.IsDead())
		{
			ApplyTechPointGain(caster, target, baseGain, src, type);
		}
	}

	// added in rogues
	public void TechPointGain(Ability src, ActorData caster, ActorData target, int baseGain, TechPointChangeType type)
	{
		DamageSource src2 = new DamageSource(src, caster.GetFreePos());
		TechPointGain(src2, caster, target, baseGain, type);
	}

	// added in rogues
	public void TechPointGain(Passive src, ActorData caster, ActorData target, int baseGain, TechPointChangeType type)
	{
		DamageSource src2 = new DamageSource(src, caster.GetFreePos());
		TechPointGain(src2, caster, target, baseGain, type);
	}

	// added in rogues
	public void TechPointGain(EffectSource src, ActorData caster, ActorData target, int baseGain, TechPointChangeType type)
	{
		DamageSource src2;
		if (src.IsAbility())
		{
			src2 = new DamageSource(src.Ability, caster.GetFreePos());
		}
		else
		{
			src2 = new DamageSource(src.Passive, caster.GetFreePos());
		}
		TechPointGain(src2, caster, target, baseGain, type);
	}

	// added in rogues
	private int ApplyTechPointGain(ActorData caster, ActorData target, int baseGain, DamageSource src, TechPointChangeType type)
	{
		int num = CalcTechPointGain(target, baseGain, AbilityData.ActionType.INVALID_ACTION, null);
		ExecuteTechPointGain(caster, target, num, src);
		return num;
	}

	// added in rogues
	public void ExecuteTechPointGain(ActorData caster, ActorData target, int finalGain, DamageSource src)
	{
		if (finalGain > 0)
		{
			target.UnresolvedTechPointGain += finalGain;
			UnresolvedHealthChange unresolvedChange = default(UnresolvedHealthChange);
			unresolvedChange.InitAsHealing(caster, src, finalGain);
			TrackUnresolvedTechPointChange(target, unresolvedChange);
			GameplayMetricHelper.CollectTechPointGainForAbility(caster, target, finalGain, src.Ability);
		}
	}

	// added in rogues
	public int CalcTechPointGain(ActorData actor, int baseGain, AbilityData.ActionType actionType, ServerGameplayUtils.EnergyStatAdjustments statAdjustments)
	{
		ActorStatus actorStatus = actor.GetActorStatus();
		// custom
		int num = baseGain;
		// rogues
		//EquipmentStats equipmentStats = actor.GetEquipmentStats();
		//int num = Mathf.Max(0, Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.TechPointGenerationAdjustment, (float)baseGain, (int)actionType, actor)));
		bool flag = actorStatus.HasStatus(StatusType.Energized);
		bool flag2 = actorStatus.HasStatus(StatusType.SlowEnergyGain);
		AbilityModPropertyInt energizedEnergyGainMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useEnergizedOverride)
		{
			energizedEnergyGainMod = GameWideData.Get().m_energizedEnergyGainMod;
		}
		else
		{
			energizedEnergyGainMod = GameplayMutators.Get().m_energizedEnergyGainMod;
		}
		AbilityModPropertyInt slowEnergyGainEnergyGainMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useSlowEnergyGainOverride)
		{
			slowEnergyGainEnergyGainMod = GameWideData.Get().m_slowEnergyGainEnergyGainMod;
		}
		else
		{
			slowEnergyGainEnergyGainMod = GameplayMutators.Get().m_slowEnergyGainEnergyGainMod;
		}
		int num2 = energizedEnergyGainMod.GetModifiedValue(num);
		int num3 = slowEnergyGainEnergyGainMod.GetModifiedValue(num);
		if (flag && !flag2)
		{
			num = num2;
		}
		else if (!flag && flag2)
		{
			num = num3;
		}
		if (GameplayMutators.Get() != null)
		{
			float energyGainMultiplier = GameplayMutators.GetEnergyGainMultiplier();
			num = Mathf.RoundToInt(num * energyGainMultiplier);
			baseGain = Mathf.RoundToInt(baseGain * energyGainMultiplier);
			num2 = Mathf.RoundToInt(num2 * energyGainMultiplier);
			num3 = Mathf.RoundToInt(num3 * energyGainMultiplier);
		}
		num = Mathf.Max(num, 0);
		if (statAdjustments != null)
		{
			statAdjustments.IncrementTotals(num, baseGain, num2, num3);
		}
		return num;
	}

	// added in rogues
	public void TechPointLoss(DamageSource src, ActorData caster, ActorData target, int baseLoss, TechPointChangeType type)
	{
		if (!target.IsDead())
		{
			ApplyTechPointLoss(caster, target, baseLoss, src, type);
		}
	}

	// added in rogues
	public void TechPointLoss(Ability src, ActorData caster, ActorData target, int baseLoss, TechPointChangeType type)
	{
		DamageSource src2 = new DamageSource(src, caster.GetFreePos());
		TechPointLoss(src2, caster, target, baseLoss, type);
	}

	// added in rogues
	public void TechPointLoss(Passive src, ActorData caster, ActorData target, int baseLoss, TechPointChangeType type)
	{
		DamageSource src2 = new DamageSource(src, caster.GetFreePos());
		TechPointLoss(src2, caster, target, baseLoss, type);
	}

	// added in rogues
	public void TechPointLoss(EffectSource src, ActorData caster, ActorData target, int baseLoss, TechPointChangeType type)
	{
		DamageSource src2;
		if (src.IsAbility())
		{
			src2 = new DamageSource(src.Ability, caster.GetFreePos());
		}
		else
		{
			src2 = new DamageSource(src.Passive, caster.GetFreePos());
		}
		TechPointLoss(src2, caster, target, baseLoss, type);
	}

	// added in rogues
	private int ApplyTechPointLoss(ActorData caster, ActorData target, int baseLoss, DamageSource src, TechPointChangeType type)
	{
		int num = CalcTechPointLoss(caster, target, baseLoss, src, type);
		ExecuteTechPointLoss(caster, target, num, src);
		return num;
	}

	// added in rogues
	public void ExecuteTechPointLoss(ActorData caster, ActorData target, int finalLoss, DamageSource src)
	{
		if (finalLoss > 0)
		{
			target.UnresolvedTechPointLoss += finalLoss;
			UnresolvedHealthChange unresolvedChange = default(UnresolvedHealthChange);
			unresolvedChange.InitAsDamage(caster, src, finalLoss);
			TrackUnresolvedTechPointChange(target, unresolvedChange);
			GameplayMetricHelper.CollectTechPointLossForAbility(caster, target, finalLoss, src.Ability);
		}
	}

	// added in rogues
	public int CalcTechPointLoss(ActorData caster, ActorData target, int baseLoss, DamageSource src, TechPointChangeType type)
	{
		return Mathf.Max(baseLoss, 0);
	}
#endif

	public void ExecuteObjectivePointGain(ActorData caster, ActorData target, int finalChange)
	{
		if (target == null)
		{
			throw new ApplicationException("Objective point change requires a target");
		}
		ObjectivePoints objectivePoints = ObjectivePoints.Get();
		if (objectivePoints != null)
		{
			Team team = target.GetTeam();
			objectivePoints.AdjustPoints(finalChange, team);
		}
	}
}
