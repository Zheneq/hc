// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// empty in rogues
// TODO TITUS server
public class ClaymoreMultiRadiusCone : Ability
{
	[Header("-- Cone Targeting")]
	public float m_coneWidthAngle = 180f;
	public float m_coneBackwardOffset;
	public float m_coneLengthInner = 1.5f;
	public float m_coneLengthMiddle = 2.5f;
	public float m_coneLengthOuter = 3.5f;
	public bool m_penetrateLineOfSight;
	[Header("-- Base Damage")]
	public int m_damageAmountInner = 5;
	public int m_damageAmountMiddle = 4;
	public int m_damageAmountOuter = 3;
	[Header("-- Bonus Damage, (threshold value range 0 to 1)")]
	public int m_bonusDamageIfEnemyLowHealth;
	public float m_enemyHealthThreshForBonus = -1f;
	public int m_bonusDamageIfCasterLowHealth;
	public float m_casterHealthThreshForBonus = -1f;
	[Header("-- Hit Effects")]
	public StandardEffectInfo m_effectInner;
	public StandardEffectInfo m_effectMiddle;
	public StandardEffectInfo m_effectOuter;
	[Header("-- Energy Gain on Self for Hits")]
	public int m_tpGainInner;
	public int m_tpGainMiddle;
	public int m_tpGainOuter;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreMultiRadiusCone m_abilityMod;
	private Claymore_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEffectInner;
	private StandardEffectInfo m_cachedEffectMiddle;
	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Mountain Cleaver";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedOuterRadius();
	}

	private float ModdedConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	private float ModdedInnerRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneInnerRadiusMod.GetModifiedValue(m_coneLengthInner)
			: m_coneLengthInner;
	}

	private float ModdedMiddleRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneMiddleRadiusMod.GetModifiedValue(m_coneLengthMiddle)
			: m_coneLengthMiddle;
	}

	private float ModdedOuterRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneOuterRadiusMod.GetModifiedValue(m_coneLengthOuter)
			: m_coneLengthOuter;
	}

	private bool GetPenetrateLineOfSight()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight) 
			: m_penetrateLineOfSight;
	}

	private int ModdedInnerDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerDamageMod.GetModifiedValue(m_damageAmountInner)
			: m_damageAmountInner;
	}

	private int ModdedMiddleDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_middleDamageMod.GetModifiedValue(m_damageAmountMiddle)
			: m_damageAmountMiddle;
	}

	private int ModdedOuterDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter)
			: m_damageAmountOuter;
	}

	private int ModdedInnerTpGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerTpGain.GetModifiedValue(m_tpGainInner)
			: m_tpGainInner;
	}

	private int ModdedMiddleTpGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_middleTpGain.GetModifiedValue(m_tpGainMiddle)
			: m_tpGainMiddle;
	}

	private int ModdedOuterTpGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_outerTpGain.GetModifiedValue(m_tpGainOuter)
			: m_tpGainOuter;
	}

	private void SetCachedFields()
	{
		m_cachedEffectInner = m_abilityMod != null
			? m_abilityMod.m_effectInnerMod.GetModifiedValue(m_effectInner)
			: m_effectInner;
		m_cachedEffectMiddle = m_abilityMod != null
			? m_abilityMod.m_effectMiddleMod.GetModifiedValue(m_effectMiddle)
			: m_effectMiddle;
		m_cachedEffectOuter = m_abilityMod != null
			? m_abilityMod.m_effectOuterMod.GetModifiedValue(m_effectOuter)
			: m_effectOuter;
	}

	public StandardEffectInfo GetEffectInner()
	{
		return m_cachedEffectInner ?? m_effectInner;
	}

	public StandardEffectInfo GetEffectMiddle()
	{
		return m_cachedEffectMiddle ?? m_effectMiddle;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		return m_cachedEffectOuter ?? m_effectOuter;
	}

	public int GetBonusDamageIfEnemyHealthBelow()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_bonusDamageIfEnemyLowHealthMod.GetModifiedValue(m_bonusDamageIfEnemyLowHealth) 
			: m_bonusDamageIfEnemyLowHealth;
	}

	public float GetEnemyHealthThreshForBonus()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_enemyHealthThreshForBonusMod.GetModifiedValue(m_enemyHealthThreshForBonus) 
			: m_enemyHealthThreshForBonus;
	}

	public int GetBonusDamageIfCasterHealthBelow()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_bonusDamageIfCasterLowHealthMod.GetModifiedValue(m_bonusDamageIfCasterLowHealth) 
			: m_bonusDamageIfCasterLowHealth;
	}

	public float GetCasterHealthThreshForBonus()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_casterHealthThreshForBonusMod.GetModifiedValue(m_casterHealthThreshForBonus) 
			: m_casterHealthThreshForBonus;
	}

	public bool ShouldApplyCasterBonusPerThresholdReached()
	{
		return m_abilityMod != null && m_abilityMod.m_applyBonusPerThresholdReached;
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		SetCachedFields();
		float angle = ModdedConeAngle();
		List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
		{
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedInnerRadius()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedMiddleRadius()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, ModdedOuterRadius())
		};
		Targeter = new AbilityUtil_Targeter_MultipleCones(this, list, m_coneBackwardOffset, GetPenetrateLineOfSight(), true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreMultiRadiusCone abilityMod_ClaymoreMultiRadiusCone = modAsBase as AbilityMod_ClaymoreMultiRadiusCone;
		AddTokenInt(tokens, "DamageAmountInner", string.Empty, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_innerDamageMod.GetModifiedValue(m_damageAmountInner)
			: m_damageAmountInner);
		AddTokenInt(tokens, "DamageAmountMiddle", string.Empty, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_middleDamageMod.GetModifiedValue(m_damageAmountMiddle)
			: m_damageAmountMiddle);
		AddTokenInt(tokens, "DamageAmountOuter", string.Empty, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_outerDamageMod.GetModifiedValue(m_damageAmountOuter)
			: m_damageAmountOuter);
		AddTokenInt(tokens, "TpGainInner", string.Empty, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_innerTpGain.GetModifiedValue(m_tpGainInner)
			: m_tpGainInner);
		AddTokenInt(tokens, "TpGainMiddle", string.Empty, abilityMod_ClaymoreMultiRadiusCone == null
			? m_tpGainMiddle
			: abilityMod_ClaymoreMultiRadiusCone.m_middleTpGain.GetModifiedValue(m_tpGainMiddle));
		AddTokenInt(tokens, "TpGainOuter", string.Empty, abilityMod_ClaymoreMultiRadiusCone == null
			? m_tpGainOuter
			: abilityMod_ClaymoreMultiRadiusCone.m_outerTpGain.GetModifiedValue(m_tpGainOuter));
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_effectInnerMod.GetModifiedValue(m_effectInner)
			: m_effectInner, "EffectInner", m_effectInner);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_effectMiddleMod.GetModifiedValue(m_effectMiddle)
			: m_effectMiddle, "EffectMiddle", m_effectMiddle);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreMultiRadiusCone != null
			? abilityMod_ClaymoreMultiRadiusCone.m_effectOuterMod.GetModifiedValue(m_effectOuter)
			: m_effectOuter, "EffectOuter", m_effectOuter);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, m_damageAmountInner));
		GetEffectInner().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Near);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Midranged, m_damageAmountMiddle));
		GetEffectMiddle().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Midranged);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, m_damageAmountOuter));
		GetEffectOuter().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Far);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return dictionary;
		}
		ActorData actorData = ActorData;
		int damage = 0;
		if (GetBonusDamageIfCasterHealthBelow() > 0 && GetCasterHealthThreshForBonus() > 0f)
		{
			float healthPercent = actorData.HitPoints / (float)actorData.GetMaxHitPoints();
			if (ShouldApplyCasterBonusPerThresholdReached())
			{
				int numBonus = Mathf.FloorToInt((1f - healthPercent) / GetCasterHealthThreshForBonus());
				damage += GetBonusDamageIfCasterHealthBelow() * numBonus;
			}
			else if (healthPercent < GetCasterHealthThreshForBonus())
			{
				damage += GetBonusDamageIfCasterHealthBelow();
			}
		}
		if (GetBonusDamageIfEnemyHealthBelow() > 0
		    && GetEnemyHealthThreshForBonus() > 0f
		    && targetActor.HitPoints / (float)targetActor.GetMaxHitPoints() < GetEnemyHealthThreshForBonus())
		{
			damage += GetBonusDamageIfEnemyHealthBelow();
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedInnerDamage() + damage;
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Midranged))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedMiddleDamage() + damage;
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedOuterDamage() + damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int energy = 0;
		if (ModdedInnerTpGain() > 0)
		{
			List<ActorData> targets = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Near);
			energy += targets.Count * ModdedInnerTpGain();
		}
		if (ModdedMiddleTpGain() > 0)
		{
			List<ActorData> targets = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Midranged);
			energy += targets.Count * ModdedMiddleTpGain();
		}
		if (ModdedOuterTpGain() > 0)
		{
			List<ActorData> targets = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Far);
			energy += targets.Count * ModdedOuterTpGain();
		}
		return energy;
	}

	public override bool DoesTargetActorMatchTooltipSubject(
		AbilityTooltipSubject subjectType,
		ActorData targetActor,
		Vector3 damageOrigin,
		ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near
		    && subjectType != AbilityTooltipSubject.Midranged
		    && subjectType != AbilityTooltipSubject.Far)
		{
			return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
		}
		float innerRadius = ModdedInnerRadius() * Board.Get().squareSize;
		float middleRadius = ModdedMiddleRadius() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetFreePos() - damageOrigin;
		vector.y = 0f;
		float dist = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			dist -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		bool inInnerRadius = dist <= innerRadius;
		bool inMiddleRadius = !inInnerRadius && dist <= middleRadius;
		bool inOuterRadius = !inInnerRadius && !inMiddleRadius;
		switch (subjectType)
		{
			case AbilityTooltipSubject.Near:
				return inInnerRadius;
			case AbilityTooltipSubject.Midranged:
				return inMiddleRadius;
			default:
				return inOuterRadius;
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ClaymoreMultiRadiusCone))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_ClaymoreMultiRadiusCone;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			Board.Get().GetSquare(targets[0].GridPos),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		Vector3 casterPos = caster.GetLoSCheckPos();
		Vector3 vec = currentTarget?.AimDirection ?? caster.transform.forward;
		float num = VectorUtils.HorizontalAngle_Deg(vec);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(
			casterPos,
			num,
			ModdedConeAngle(),
			ModdedOuterRadius(),
			m_coneBackwardOffset,
			GetPenetrateLineOfSight(),
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		int energy = 0;
		foreach (ActorData actor in actors)
		{
			float innerRadius = ModdedInnerRadius() * Board.Get().squareSize;
			float middleRadius = ModdedMiddleRadius() * Board.Get().squareSize;
			Vector3 vector = actor.GetFreePos() - casterPos;
			vector.y = 0f;
			float dist = vector.magnitude;
			if (GameWideData.Get().UseActorRadiusForCone())
			{
				dist -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
			}
			
			int damage;
			StandardEffectInfo effect;
			if (dist <= innerRadius)
			{
				damage = ModdedInnerDamage();
				effect = GetEffectInner();
				energy += targets.Count * ModdedInnerTpGain();
			}
			else if (dist <= middleRadius)
			{
				damage = ModdedMiddleDamage();
				effect = GetEffectMiddle();
				energy += targets.Count * ModdedMiddleTpGain();
			}
			else
			{
				damage = ModdedOuterDamage();
				effect = GetEffectOuter();
				energy += targets.Count * ModdedOuterTpGain();
			}
			ActorHitParameters hitParams = new ActorHitParameters(actor, casterPos);
			ActorHitResults hitResults = new ActorHitResults(damage, HitActionType.Damage, effect, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		if (energy > 0)
		{
			ActorHitParameters hitParams = new ActorHitParameters(caster, casterPos);
			ActorHitResults hitResults = new ActorHitResults(energy, HitActionType.TechPointsGain, (StandardEffectInfo) null, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ClaymoreStats.UltDamage, results.FinalDamage);
		}
	}
#endif
}
