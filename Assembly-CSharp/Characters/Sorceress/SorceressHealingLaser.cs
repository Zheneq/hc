// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingLaser : Ability
{
	public bool m_includeAllies = true;
	public bool m_penetrateLineOfSight;
	public float m_width = 1f;
	public float m_distance = 15f;
	[Header("-- Damage")]
	public int m_damageAmount = 10;
	public int m_minDamageAmount;
	public int m_damageChangePerHit;
	[Header("-- Heal")]
	public int m_selfHealAmount = 1;
	public int m_allyHealAmount = 1;
	public int m_minHealAmount;
	public int m_healChangePerHit;

	private AbilityUtil_Targeter_Laser m_laserTargeter;
	private AbilityMod_SorceressHealingLaser m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_laserTargeter = new AbilityUtil_Targeter_Laser(
			this,
			ModdedLaserWidth(),
			ModdedLaserRange(),
			m_penetrateLineOfSight,
			-1,
			m_includeAllies,
			ModdedBaseHealOnSelf() > 0);
		Targeter = m_laserTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		if (m_includeAllies)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_allyHealAmount));
			if (ModdedAllyTechPointGain() > 0)
			{
				list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, ModdedAllyTechPointGain()));
			}
		}
		if (m_selfHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_selfHealAmount));
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		if (m_includeAllies)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_allyHealAmount));
			if (ModdedAllyTechPointGain() > 0)
			{
				list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, ModdedAllyTechPointGain()));
			}
		}
		if (ModdedBaseHealOnSelf() > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, ModdedBaseHealOnSelf()));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (m_laserTargeter == null)
		{
			return null;
		}
		ActorData component = GetComponent<ActorData>();
		if (component == null)
		{
			return null;
		}
		List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = m_laserTargeter.GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			if (hitActorContext[i].actor == targetActor)
			{
				Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				if (component == targetActor)
				{
					dictionary[AbilityTooltipSymbol.Healing] = ModdedBaseHealOnSelf();
				}
				else if (targetActor.GetTeam() != component.GetTeam())
				{
					dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountByHitOrder(i);
				}
				else
				{
					dictionary[AbilityTooltipSymbol.Healing] = GetHealAmountByHitOrder(i);
				}
				return dictionary;
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingLaser abilityMod_SorceressHealingLaser = modAsBase as AbilityMod_SorceressHealingLaser;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "MinDamageAmount", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_minDamageMod.GetModifiedValue(m_minDamageAmount)
			: m_minDamageAmount);
		AddTokenInt(tokens, "DamageChangePerHit", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit)
			: m_damageChangePerHit);
		AddTokenInt(tokens, "SelfHealAmount", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_selfHealMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount);
		AddTokenInt(tokens, "AllyHealAmount", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_allyHealMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount);
		AddTokenInt(tokens, "MinHealAmount", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_minHealMod.GetModifiedValue(m_minHealAmount)
			: m_minHealAmount);
		AddTokenInt(tokens, "HealChangePerHit", string.Empty, abilityMod_SorceressHealingLaser != null
			? abilityMod_SorceressHealingLaser.m_healChangePerHitMod.GetModifiedValue(m_healChangePerHit)
			: m_healChangePerHit);
	}

	public int GetHealAmountByHitOrder(int hitOrder)
	{
		return Mathf.Max(
			ModdedMinHealAmountOnAlly(), 
			ModdedBaseHealOnAlly() + ModdedHealChangePerHit() * hitOrder);
	}

	public int GetDamageAmountByHitOrder(int hitOrder)
	{
		return Mathf.Max(
			ModdedMinDamage(), 
			ModdedBaseDamage() + ModdedDamageChangePerHit() * hitOrder);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressHealingLaser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressHealingLaser;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedBaseHealOnSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount;
	}

	public int ModdedBaseHealOnAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount;
	}

	public int ModdedMinHealAmountOnAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minHealMod.GetModifiedValue(m_minHealAmount)
			: m_minHealAmount;
	}

	public int ModdedHealChangePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healChangePerHitMod.GetModifiedValue(m_healChangePerHit)
			: m_healChangePerHit;
	}

	public int ModdedBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int ModdedMinDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamageAmount)
			: m_minDamageAmount;
	}

	public int ModdedDamageChangePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit)
			: m_damageChangePerHit;
	}

	public float ModdedLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float ModdedLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance)
			: m_distance;
	}

	public int ModdedAllyTechPointGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyTechPointGain.GetModifiedValue(0)
			: 0;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetHitActorsExcludingSelfInOrder(targets, caster, out Vector3 endPos, null);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			null,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[]
			{
				new HealLaserSequence.ExtraParams
				{
					endPos = endPos
				}
			});
	}

	// added in rogues
	private List<ActorData> GetHitActorsExcludingSelfInOrder(
		List<AbilityTarget> targets,
		ActorData caster,
		out Vector3 laserEndPos,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			caster.GetLoSCheckPos(),
			targets[0].AimDirection,
			ModdedLaserRange(),
			ModdedLaserWidth(),
			caster,
			TargeterUtils.GetRelevantTeams(caster, m_includeAllies, true),
			m_penetrateLineOfSight,
			-1,
			false,
			true,
			out laserEndPos,
			nonActorTargetInfo);
		actorsInLaser.Remove(caster);
		return actorsInLaser;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActorsExcludingSelfInOrder = GetHitActorsExcludingSelfInOrder(targets, caster, out Vector3 vector, nonActorTargetInfo);
		bool flag = false;
		for (int i = 0; i < hitActorsExcludingSelfInOrder.Count; i++)
		{
			ActorData actorData = hitActorsExcludingSelfInOrder[i];
			ActorHitResults actorHitResults = MakeActorHitRes(actorData, caster.GetFreePos());
			if (actorData.GetTeam() == caster.GetTeam())
			{
				int healAmountByHitOrder = GetHealAmountByHitOrder(i);
				actorHitResults.SetBaseHealing(healAmountByHitOrder);
				if (ModdedAllyTechPointGain() > 0)
				{
					actorHitResults.AddTechPointGain(ModdedAllyTechPointGain());
				}
			}
			else
			{
				int damageAmountByHitOrder = GetDamageAmountByHitOrder(i);
				actorHitResults.SetBaseDamage(damageAmountByHitOrder);
			}
			if (!flag && hitActorsExcludingSelfInOrder.Count > 1)
			{
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_UpdateFreelancerStat(
					(int)FreelancerStats.DigitalSorceressStats.TimesPrimaryHitTwoOrMoreTargets,
					1,
					caster));
				flag = true;
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (ModdedBaseHealOnSelf() > 0)
		{
			ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
			abilityResults.StoreActorHit(new ActorHitResults(ModdedBaseHealOnSelf(), HitActionType.Healing, hitParams));
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
