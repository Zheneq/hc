// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SamuraiSelfBuff : Ability
{
	[Header("-- Buffs")]
	public bool m_selfBuffLastsUntilYouDealDamage;
	public StandardEffectInfo m_selfBuffEffect;
	[Header("-- Extra damage to other abilities if this ability is queued")]
	public int m_extraDamageIfQueued;
	[Header("-- Shielding")]
	public int m_baseShielding = 30;
	public int m_extraShieldingIfOnlyAbility;
	public StandardEffectInfo m_generalEffectOnSelf;
	[Header("-- AoE")]
	public float m_aoeRadius = 1.5f;
	public float m_knockbackDist = 2f;
	public int m_damageAmount;
	public bool m_penetrateLoS;
	[Header("-- Damage reactions")]
	public int m_damageIncreaseFirstHit = 10;
	public int m_damageIncreaseSubseqHits;
	public int m_techPointGainPerIncomingHit;
	public bool m_buffInResponseToIndirectDamage = true;
	[Space(10f)]
	public int m_cdrIfNotHit;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_hitReactionSequencePrefab;
	public GameObject m_buffStartSequencePrefab;

	private AbilityMod_SamuraiSelfBuff m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private AbilityData.ActionType m_myActionType;
	private AbilityData m_abilityData;
	private StandardEffectInfo m_cachedSelfBuffEffect;
	private StandardEffectInfo m_cachedGeneralEffectOnSelf;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Self Buff";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_abilityData = GetComponent<AbilityData>();
		m_myActionType = GetActionTypeOfAbility(this);
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_StretchCone targeter = new AbilityUtil_Targeter_StretchCone(
			this,
			GetAoeRadius(),
			GetAoeRadius(),
			360f,
			360f,
			AreaEffectUtils.StretchConeStyle.Linear,
			0f,
			PenetrateLoS());
		targeter.InitKnockbackData(GetKnockbackDist(), KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
		
		// reactor
		targeter.m_includeCaster = true;
		// rogues
		// targeter.SetAffectedGroups(false, false, true);
		
		Targeter = targeter;
	}

	private void SetCachedFields()
	{
		m_cachedSelfBuffEffect = m_abilityMod != null
			? m_abilityMod.m_selfBuffEffectMod.GetModifiedValue(m_selfBuffEffect)
			: m_selfBuffEffect;
		m_cachedGeneralEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_generalEffectOnSelfMod.GetModifiedValue(m_generalEffectOnSelf)
			: m_generalEffectOnSelf;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		return m_cachedSelfBuffEffect ?? m_selfBuffEffect;
	}

	public StandardEffectInfo GetGeneralEffectOnSelf()
	{
		return m_cachedGeneralEffectOnSelf ?? m_generalEffectOnSelf;
	}

	public int GetExtraDamageIfQueued()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfQueuedMod.GetModifiedValue(m_extraDamageIfQueued)
			: m_extraDamageIfQueued;
	}

	public int GetBaseShielding()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseShieldingMod.GetModifiedValue(m_baseShielding)
			: m_baseShielding;
	}

	public int GetExtraShieldingIfOnlyAbility()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraShieldingIfOnlyAbilityMod.GetModifiedValue(m_extraShieldingIfOnlyAbility)
			: m_extraShieldingIfOnlyAbility;
	}

	public bool SelfBuffLastsUntilYouDealDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfBuffLastsUntilYouDealDamageMod.GetModifiedValue(m_selfBuffLastsUntilYouDealDamage)
			: m_selfBuffLastsUntilYouDealDamage;
	}

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius)
			: m_aoeRadius;
	}

	public float GetKnockbackDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistMod.GetModifiedValue(m_knockbackDist)
			: m_knockbackDist;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public int GetDamageIncreaseFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageIncreaseFirstHitMod.GetModifiedValue(m_damageIncreaseFirstHit)
			: m_damageIncreaseFirstHit;
	}

	public int GetDamageIncreaseSubseqHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageIncreaseSubseqHitsMod.GetModifiedValue(m_damageIncreaseSubseqHits)
			: m_damageIncreaseSubseqHits;
	}

	public int GetTechPointGainPerIncomingHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerIncomingHitMod.GetModifiedValue(m_techPointGainPerIncomingHit)
			: m_techPointGainPerIncomingHit;
	}

	public bool BuffInResponseToIndirectDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_buffInResponseToIndirectDamageMod.GetModifiedValue(m_buffInResponseToIndirectDamage)
			: m_buffInResponseToIndirectDamage;
	}

	public int GetCdrIfNotHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfNotHitMod.GetModifiedValue(m_cdrIfNotHit)
			: m_cdrIfNotHit;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_selfBuffEffect, "SelfBuffEffect", m_selfBuffEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_generalEffectOnSelf, "GeneralEffectOnSelf", m_generalEffectOnSelf);
		AddTokenInt(tokens, "ExtraDamageIfQueued", string.Empty, m_extraDamageIfQueued);
		AddTokenInt(tokens, "BaseShielding", string.Empty, m_baseShielding);
		AddTokenInt(tokens, "ExtraShieldingIfOnlyAbility", string.Empty, m_extraShieldingIfOnlyAbility);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "DamageIncreaseFirstHit", string.Empty, m_damageIncreaseFirstHit);
		AddTokenInt(tokens, "DamageIncreaseSubseqHits", string.Empty, m_damageIncreaseSubseqHits);
		AddTokenInt(tokens, "TechPointGainPerIncomingHit", string.Empty, m_techPointGainPerIncomingHit);
		AddTokenInt(tokens, "CdrIfNotHit", string.Empty, m_cdrIfNotHit);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateAbilityTooltipNumbers();
		GetSelfBuffEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		GetGeneralEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (targetActor != ActorData)
		{
			return false;
		}
		int absorb = GetBaseShielding();
		if (GetExtraShieldingIfOnlyAbility() > 0 && !HasOtherQueuedAbilities())
		{
			absorb += GetExtraShieldingIfOnlyAbility();
		}
		results.m_absorb = absorb;
		return true;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComponent != null
			? m_syncComponent.m_lastSelfBuffTurn == -1
			: base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiSelfBuff))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiSelfBuff;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public bool HasOtherQueuedAbilities()
	{
		if (m_abilityData != null)
		{
			for (int i = 0; i <= 4; i++)
			{
				if (i != (int)m_myActionType && m_abilityData.HasQueuedAction((AbilityData.ActionType)i)) // , true in rogues
				{
					return true;
				}
			}
		}
		return false;
	}

#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComponent != null)
		{
			m_syncComponent.Networkm_lastSelfBuffTurn = GameFlowData.Get().CurrentTurn;
		}
	}

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
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		StandardEffectInfo selfBuffEffect = GetSelfBuffEffect();
		AbilityStatMod buffFromFirstIncomingHit = new AbilityStatMod
		{
			stat = StatType.OutgoingDamage,
			modType = ModType.BaseAdd,
			modValue = GetDamageIncreaseFirstHit()
		};
		AbilityStatMod buffPerIncomingHit = new AbilityStatMod
		{
			stat = StatType.OutgoingDamage,
			modType = ModType.BaseAdd,
			modValue = GetDamageIncreaseSubseqHits()
		};
		SamuraiSelfBuffEffect samuraiSelfBuffEffect = new SamuraiSelfBuffEffect(
			AsEffectSource(),
			caster.GetCurrentBoardSquare(),
			caster,
			caster,
			selfBuffEffect.m_effectData,
			buffFromFirstIncomingHit,
			buffPerIncomingHit,
			GetTechPointGainPerIncomingHit(),
			BuffInResponseToIndirectDamage(),
			GetCdrIfNotHit(),
			m_myActionType,
			m_hitReactionSequencePrefab,
			m_buffStartSequencePrefab);
		if (SelfBuffLastsUntilYouDealDamage())
		{
			samuraiSelfBuffEffect.SetDurationBeforeStart(0);
		}
		casterHitResults.AddEffect(samuraiSelfBuffEffect);
		if (GetGeneralEffectOnSelf().m_applyEffect)
		{
			StandardActorEffect standardActorEffect = GetGeneralEffectOnSelf().CreateEffect(AsEffectSource(), caster, caster);
			int num = GetBaseShielding();
			if (GetExtraShieldingIfOnlyAbility() > 0 && !HasOtherQueuedAbilities())
			{
				num += GetExtraShieldingIfOnlyAbility();
			}
			standardActorEffect.InitAbsorbtion(num);
			casterHitResults.AddEffect(standardActorEffect);
		}
		abilityResults.StoreActorHit(casterHitResults);
		if (GetAoeRadius() > 0f && (GetDamageAmount() > 0 || GetKnockbackDist() > 0f))
		{
			List<NonActorTargetInfo> nonActorTargets = new List<NonActorTargetInfo>();
			List<ActorData> actors = caster
				.GetOtherTeams()
				.SelectMany(otherTeam => AreaEffectUtils.GetActorsInRadius(
					caster.GetFreePos(),
					GetAoeRadius(),
					PenetrateLoS(),
					caster,
					otherTeam,
					nonActorTargets))
				.ToList();
			foreach (ActorData target in actors)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
				actorHitResults.AddBaseDamage(GetDamageAmount());
				KnockbackHitData knockbackData = new KnockbackHitData(
					target,
					caster,
					KnockbackType.AwayFromSource,
					Vector3.forward,
					caster.GetFreePos(),
					GetKnockbackDist());
				actorHitResults.AddKnockbackData(knockbackData);
				abilityResults.StoreActorHit(actorHitResults);
			}
			abilityResults.StoreNonActorTargetInfo(nonActorTargets);
		}
	}

	// added in rogues
	public override void OnEffectAbsorbedDamage(ActorData effectCaster, int damageAbsorbed)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SamuraiStats.EffectiveShielding_RensFury, damageAbsorbed);
	}
#endif
}
