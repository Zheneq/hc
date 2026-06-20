// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBasicAttack : Ability
{
	[Space(10f)]
	public float m_coneWidthAngle = 270f;
	public float m_coneLength = 1.5f;
	public float m_coneBackwardOffset;
	public int m_damageAmount = 20;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets = 2;
	public int m_healAmountPerTargetHit;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_BattleMonkBasicAttack m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Siphon Slash";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_DirectionCone(
			this,
			ModdedConeAngle(),
			ModdedConeLength(),
			m_coneBackwardOffset,
			m_penetrateLineOfSight,
			true,
			true,
			false,
			ModdedHealPerTargetHit() > 0);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedConeLength();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkBasicAttack abilityMod_BattleMonkBasicAttack = modAsBase as AbilityMod_BattleMonkBasicAttack;
		tokens.Add(new TooltipTokenInt("Damage", "damage to enemies", abilityMod_BattleMonkBasicAttack != null
			? abilityMod_BattleMonkBasicAttack.m_coneDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount)
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, ModdedConeDamage(1));
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, ModdedHealPerTargetHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			int healing = ModdedHealPerTargetHit() * visibleActorsCountByTooltipSubject;
			dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(healing);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedConeDamage(visibleActorsCountByTooltipSubject);
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BattleMonkBasicAttack))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BattleMonkBasicAttack;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float ModdedConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float ModdedConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public int ModdedConeDamage(int numTargets)
	{
		int damage = m_damageAmount;
		if (m_abilityMod != null)
		{
			damage = m_abilityMod.m_coneDamageMod.GetModifiedValue(damage);
			damage += m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0) * (numTargets - 1);
		}
		return damage;
	}

	public int ModdedHealPerTargetHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healPerTargetHitMod.GetModifiedValue(m_healAmountPerTargetHit)
			: m_healAmountPerTargetHit;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetCurrentBoardSquare(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		int baseDamage = ModdedConeDamage(list.Count);
		foreach (ActorData target in list)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, loSCheckPos));
			actorHitResults.SetBaseDamage(baseDamage);
			if (m_abilityMod != null)
			{
				actorHitResults.AddStandardEffectInfo(m_abilityMod.m_enemyHitEffect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (ModdedHealPerTargetHit() > 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			actorHitResults.SetBaseHealing(ModdedHealPerTargetHit() * list.Count);
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		List<ActorData> list = null;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		list = AreaEffectUtils.GetActorsInCone(
			loSCheckPos,
			coneCenterAngleDegrees,
			ModdedConeAngle(),
			ModdedConeLength(),
			m_coneBackwardOffset,
			m_penetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		if (m_maxTargets > 0)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref list, loSCheckPos);
			TargeterUtils.LimitActorsToMaxNumber(ref list, m_maxTargets);
		}
		return list ?? new List<ActorData>();
	}
#endif
}
