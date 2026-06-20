// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrSacrificeHeal : MartyrLaserBase
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_targetShape;
	public bool m_affectsEnemies = true;
	public bool m_affectsAllies = true;
	public bool m_penetratesLoS;
	public bool m_freeTargetPosition;
	[Header("-- Damage & Healing & Crystal Bonuses")]
	public int m_baseHealingToAlly = 20;
	public int m_baseDamageToEnemy = 20;
	public int m_baseDamageToSelf = 20;
	public int m_healingToAllyPerCrystalSpent = 5;
	public int m_damageToEnemyPerCrystalSpent = 5;
	public int m_damageToSelfPerCrystalSpent = -5;
	public List<MartyrSacrificeThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Sequences")]
	public GameObject m_selfHitSequence;
	public GameObject m_allyHitSequence;
	public GameObject m_enemyHitSequence;
	public GameObject m_aoeHitSequence;

	private Martyr_SyncComponent m_syncComponent;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Sacrifice Heal";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrSacrificeThreshold bonus in m_thresholdBasedCrystalBonuses)
		{
			list.Add(bonus);
		}
		return list;
	}

	protected void SetupTargeter()
	{
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = m_freeTargetPosition
			? AbilityUtil_Targeter.AffectsActor.Never
			: AbilityUtil_Targeter.AffectsActor.Always;
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, m_targetShape, GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, m_affectsEnemies, m_affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, affectsBestTarget)
		{
			m_affectCasterDelegate = delegate (ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
			{
				int currentDamageForSelf = GetCurrentDamageForSelf(caster);
				return currentDamageForSelf != 0;
			}
		};
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		Targeter = abilityUtil_Targeter_Shape;
	}

	public AbilityAreaShape GetShape()
	{
		return m_targetShape;
	}

	public int GetBaseDamageOnSelfAmount()
	{
		return m_baseDamageToSelf;
	}

	public int GetBaseDamageAmount()
	{
		return m_baseDamageToEnemy;
	}

	public int GetBaseHealAmount()
	{
		return m_baseHealingToAlly;
	}

	public int GetDamageOnSelfAmountPerCrystalSpent()
	{
		return m_damageToSelfPerCrystalSpent;
	}

	public int GetDamageAmountPerCrystalSpent()
	{
		return m_damageToEnemyPerCrystalSpent;
	}

	public int GetHealAmountPerCrystalSpent()
	{
		return m_healingToAllyPerCrystalSpent;
	}

	public int GetCurrentDamageForSelf(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int additionalDamageToSelf = martyrSacrificeThreshold != null ? martyrSacrificeThreshold.m_additionalDamageToSelf : 0;
		return GetBaseDamageOnSelfAmount()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetDamageOnSelfAmountPerCrystalSpent()
			+ additionalDamageToSelf;
	}

	public int GetCurrentDamageForEnemy(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int additionalDamageToEnemy = martyrSacrificeThreshold != null ? martyrSacrificeThreshold.m_additionalDamageToEnemy : 0;
		return GetBaseDamageAmount()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetDamageAmountPerCrystalSpent()
			+ additionalDamageToEnemy;
	}

	public int GetCurrentHealingForAlly(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int additionalHealToAlly = martyrSacrificeThreshold != null ? martyrSacrificeThreshold.m_additionalHealToAlly : 0;
		return GetBaseHealAmount()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetHealAmountPerCrystalSpent()
			+ additionalHealToAlly;
	}

	public bool GetPenetratesLoS()
	{
		return m_penetratesLoS;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		tokens.Add(new TooltipTokenInt("BaseHealing", "Healing on allies with no crystal bonus", GetBaseHealAmount()));
		tokens.Add(new TooltipTokenInt("BaseDamage", "Damage on enemies with no crystal bonus", GetBaseDamageAmount()));
		tokens.Add(new TooltipTokenInt("BaseDamageOnSelf", "Damage on self with no crystal bonus", GetBaseDamageOnSelfAmount()));
		tokens.Add(new TooltipTokenInt("HealingOnAllyPerCrystal", "Healing on targeted ally added per crystal spent", GetHealAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("DamageOnEnemyPerCrystal", "Damage on targeted enemy added per crystal spent", GetDamageAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("DamageOnSelfPerCrystal", "Damage on self added per crystal spent", GetDamageOnSelfAmountPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> number = base.CalculateNameplateTargetingNumbers();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, GetBaseHealAmount());
		AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Primary, GetBaseDamageAmount());
		AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Self, GetBaseDamageOnSelfAmount());
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == ActorData)
		{
			int currentDamageForSelf = GetCurrentDamageForSelf(ActorData);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, currentDamageForSelf, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Self);
		}
		else if (targetActor.GetTeam() == ActorData.GetTeam())
		{
			int currentHealingForAlly = GetCurrentHealingForAlly(ActorData);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, currentHealingForAlly, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally);
		}
		else
		{
			int currentDamageForEnemy = GetCurrentDamageForEnemy(ActorData);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, currentDamageForEnemy, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_freeTargetPosition)
		{
			return true;
		}
		return HasTargetableActorsInDecision(caster, m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_freeTargetPosition)
		{
			return true;
		}
		return CanTargetActorInDecision(caster, target.GetCurrentBestActorTarget(), m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetShape(), targets[0]);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_aoeHitSequence, centerOfShape, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, null);
		list.Add(item);
		foreach (ActorData actorData in additionalData.m_abilityResults.HitActorsArray())
		{
			if (actorData == caster)
			{
				item = new ServerClientUtils.SequenceStartData(m_selfHitSequence, actorData.GetCurrentBoardSquare(), new ActorData[]
				{
					actorData
				}, caster, additionalData.m_sequenceSource, null);
			}
			else if (actorData.GetTeam() == caster.GetTeam())
			{
				item = new ServerClientUtils.SequenceStartData(m_allyHitSequence, actorData.GetCurrentBoardSquare(), new ActorData[]
				{
					actorData
				}, caster, additionalData.m_sequenceSource, null);
			}
			else
			{
				item = new ServerClientUtils.SequenceStartData(m_enemyHitSequence, actorData.GetCurrentBoardSquare(), new ActorData[]
				{
					actorData
				}, caster, additionalData.m_sequenceSource, null);
			}
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo);
		int currentHealingForAlly = GetCurrentHealingForAlly(caster);
		int currentDamageForEnemy = GetCurrentDamageForEnemy(caster);
		int currentDamageForSelf = GetCurrentDamageForSelf(caster);
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			if (hitActor == caster)
			{
				actorHitResults.AddBaseDamage(currentDamageForSelf);
			}
			else if (hitActor.GetTeam() == caster.GetTeam())
			{
				actorHitResults.AddBaseHealing(currentHealingForAlly);
			}
			else
			{
				actorHitResults.AddBaseDamage(currentDamageForEnemy);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	protected List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		endPoints = default(VectorUtils.LaserCoords);
		List<Team> list = new List<Team>();
		if (m_affectsAllies)
		{
			list.Add(caster.GetTeam());
		}
		if (m_affectsEnemies)
		{
			list.AddRange(caster.GetOtherTeams());
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(GetShape(), targets[0], GetPenetratesLoS(), caster, list, nonActorTargetInfo);
		if (!actorsInShape.Contains(caster))
		{
			actorsInShape.Add(caster);
		}
		return actorsInShape;
	}
#endif
}
