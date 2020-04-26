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
		using (List<MartyrSacrificeThreshold>.Enumerator enumerator = m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrSacrificeThreshold current = enumerator.Current;
				list.Add(current);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
	}

	protected void SetupTargeter()
	{
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Always;
		if (m_freeTargetPosition)
		{
			affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Never;
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, m_targetShape, GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, m_affectsEnemies, m_affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, affectsBestTarget);
		abilityUtil_Targeter_Shape.m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentDamageForSelf = GetCurrentDamageForSelf(caster);
			return currentDamageForSelf != 0;
		};
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		base.Targeter = abilityUtil_Targeter_Shape;
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
		int num;
		if (martyrSacrificeThreshold != null)
		{
			num = martyrSacrificeThreshold.m_additionalDamageToSelf;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseDamageOnSelfAmount() + m_syncComponent.SpentDamageCrystals(caster) * GetDamageOnSelfAmountPerCrystalSpent() + num2;
	}

	public int GetCurrentDamageForEnemy(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int num;
		if (martyrSacrificeThreshold != null)
		{
			num = martyrSacrificeThreshold.m_additionalDamageToEnemy;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseDamageAmount() + m_syncComponent.SpentDamageCrystals(caster) * GetDamageAmountPerCrystalSpent() + num2;
	}

	public int GetCurrentHealingForAlly(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int num;
		if (martyrSacrificeThreshold != null)
		{
			num = martyrSacrificeThreshold.m_additionalHealToAlly;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseHealAmount() + m_syncComponent.SpentDamageCrystals(caster) * GetHealAmountPerCrystalSpent() + num2;
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
		if (targetActor == base.ActorData)
		{
			int currentDamageForSelf = GetCurrentDamageForSelf(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentDamageForSelf, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Self);
		}
		else if (targetActor.GetTeam() == base.ActorData.GetTeam())
		{
			int currentHealingForAlly = GetCurrentHealingForAlly(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentHealingForAlly, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally);
		}
		else
		{
			int currentDamageForEnemy = GetCurrentDamageForEnemy(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentDamageForEnemy, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		if (!m_freeTargetPosition)
		{
			result = HasTargetableActorsInDecision(caster, m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = true;
		if (!m_freeTargetPosition)
		{
			ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
			result = CanTargetActorInDecision(caster, currentBestActorTarget, m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
		}
		return result;
	}
}
