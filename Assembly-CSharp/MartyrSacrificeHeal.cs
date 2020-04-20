using System;
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
	public int m_baseHealingToAlly = 0x14;

	public int m_baseDamageToEnemy = 0x14;

	public int m_baseDamageToSelf = 0x14;

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
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.Start()).MethodHandle;
			}
			this.m_abilityName = "Martyr Sacrifice Heal";
		}
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		this.SetupTargeter();
		base.ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return this.m_syncComponent;
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		using (List<MartyrSacrificeThreshold>.Enumerator enumerator = this.m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrSacrificeThreshold item = enumerator.Current;
				list.Add(item);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.GetThresholdBasedCrystalBonusList()).MethodHandle;
			}
		}
		return list;
	}

	protected void SetupTargeter()
	{
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Always;
		if (this.m_freeTargetPosition)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.SetupTargeter()).MethodHandle;
			}
			affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Never;
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, this.m_targetShape, this.GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.m_affectsEnemies, this.m_affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, affectsBestTarget);
		abilityUtil_Targeter_Shape.m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentDamageForSelf = this.GetCurrentDamageForSelf(caster);
			return currentDamageForSelf != 0;
		};
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		base.Targeter = abilityUtil_Targeter_Shape;
	}

	public AbilityAreaShape GetShape()
	{
		return this.m_targetShape;
	}

	public int GetBaseDamageOnSelfAmount()
	{
		return this.m_baseDamageToSelf;
	}

	public int GetBaseDamageAmount()
	{
		return this.m_baseDamageToEnemy;
	}

	public int GetBaseHealAmount()
	{
		return this.m_baseHealingToAlly;
	}

	public int GetDamageOnSelfAmountPerCrystalSpent()
	{
		return this.m_damageToSelfPerCrystalSpent;
	}

	public int GetDamageAmountPerCrystalSpent()
	{
		return this.m_damageToEnemyPerCrystalSpent;
	}

	public int GetHealAmountPerCrystalSpent()
	{
		return this.m_healingToAllyPerCrystalSpent;
	}

	public int GetCurrentDamageForSelf(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = base.GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int num;
		if (martyrSacrificeThreshold != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.GetCurrentDamageForSelf(ActorData)).MethodHandle;
			}
			num = martyrSacrificeThreshold.m_additionalDamageToSelf;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseDamageOnSelfAmount() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetDamageOnSelfAmountPerCrystalSpent() + num2;
	}

	public int GetCurrentDamageForEnemy(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = base.GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int num;
		if (martyrSacrificeThreshold != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.GetCurrentDamageForEnemy(ActorData)).MethodHandle;
			}
			num = martyrSacrificeThreshold.m_additionalDamageToEnemy;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseDamageAmount() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetDamageAmountPerCrystalSpent() + num2;
	}

	public int GetCurrentHealingForAlly(ActorData caster)
	{
		MartyrSacrificeThreshold martyrSacrificeThreshold = base.GetCurrentPowerEntry(caster) as MartyrSacrificeThreshold;
		int num;
		if (martyrSacrificeThreshold != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.GetCurrentHealingForAlly(ActorData)).MethodHandle;
			}
			num = martyrSacrificeThreshold.m_additionalHealToAlly;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseHealAmount() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetHealAmountPerCrystalSpent() + num2;
	}

	public bool GetPenetratesLoS()
	{
		return this.m_penetratesLoS;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		tokens.Add(new TooltipTokenInt("BaseHealing", "Healing on allies with no crystal bonus", this.GetBaseHealAmount()));
		tokens.Add(new TooltipTokenInt("BaseDamage", "Damage on enemies with no crystal bonus", this.GetBaseDamageAmount()));
		tokens.Add(new TooltipTokenInt("BaseDamageOnSelf", "Damage on self with no crystal bonus", this.GetBaseDamageOnSelfAmount()));
		tokens.Add(new TooltipTokenInt("HealingOnAllyPerCrystal", "Healing on targeted ally added per crystal spent", this.GetHealAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("DamageOnEnemyPerCrystal", "Damage on targeted enemy added per crystal spent", this.GetDamageAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("DamageOnSelfPerCrystal", "Damage on self added per crystal spent", this.GetDamageOnSelfAmountPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateNameplateTargetingNumbers();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetBaseHealAmount());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetBaseDamageAmount());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Self, this.GetBaseDamageOnSelfAmount());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == base.ActorData)
		{
			int currentDamageForSelf = this.GetCurrentDamageForSelf(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, currentDamageForSelf, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Self);
		}
		else if (targetActor.GetTeam() == base.ActorData.GetTeam())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			int currentHealingForAlly = this.GetCurrentHealingForAlly(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, currentHealingForAlly, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally);
		}
		else
		{
			int currentDamageForEnemy = this.GetCurrentDamageForEnemy(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, currentDamageForEnemy, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy);
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		if (!this.m_freeTargetPosition)
		{
			result = base.HasTargetableActorsInDecision(caster, this.m_affectsEnemies, this.m_affectsAllies, false, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = true;
		if (!this.m_freeTargetPosition)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSacrificeHeal.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
			result = base.CanTargetActorInDecision(caster, currentBestActorTarget, this.m_affectsEnemies, this.m_affectsAllies, false, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
		}
		return result;
	}
}
