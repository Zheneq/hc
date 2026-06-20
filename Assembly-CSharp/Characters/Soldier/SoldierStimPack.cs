// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoldierStimPack : Ability
{
	[Separator("On Hit")]
	public int m_selfHealAmount;
	public StandardEffectInfo m_selfHitEffect;
	[Separator("For other abilities when active")]
	public bool m_basicAttackIgnoreCover;
	public bool m_basicAttackReduceCoverEffectiveness;
	public float m_grenadeExtraRange;
	public StandardEffectInfo m_dashShootExtraEffect;
	[Separator("CDR - Health threshold to trigger cooldown reset, value:(0-1)")]
	public float m_cooldownResetHealthThreshold = -1f;
	[Header("-- CDR - if dash and shoot used on same turn")]
	public int m_cdrIfDashAndShootUsed;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierStimPack m_abilityMod;
	private AbilityData m_abilityData;
	private SoldierGrenade m_grenadeAbility;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedDashShootExtraEffect;

#if SERVER
	// added in rogues
	private AbilityData.ActionType m_myActionType;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Stim Pack";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_abilityData == null)
		{
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null)
		{
			if (m_grenadeAbility == null)
			{
				m_grenadeAbility = m_abilityData.GetAbilityOfType(typeof(SoldierGrenade)) as SoldierGrenade;
			}
#if SERVER
			// added in rogues
			m_myActionType = m_abilityData.GetActionTypeOfAbility(this);
#endif
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
		m_cachedDashShootExtraEffect = m_abilityMod != null
			? m_abilityMod.m_dashShootExtraEffectMod.GetModifiedValue(m_dashShootExtraEffect)
			: m_dashShootExtraEffect;
	}

	public int GetSelfHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public bool BasicAttackIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_basicAttackIgnoreCoverMod.GetModifiedValue(m_basicAttackIgnoreCover)
			: m_basicAttackIgnoreCover;
	}

	public bool BasicAttackReduceCoverEffectiveness()
	{
		return m_abilityMod != null
			? m_abilityMod.m_basicAttackReduceCoverEffectivenessMod.GetModifiedValue(m_basicAttackReduceCoverEffectiveness)
			: m_basicAttackReduceCoverEffectiveness;
	}

	public float GetGrenadeExtraRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_grenadeExtraRangeMod.GetModifiedValue(m_grenadeExtraRange)
			: m_grenadeExtraRange;
	}

	public StandardEffectInfo GetDashShootExtraEffect()
	{
		return m_cachedDashShootExtraEffect ?? m_dashShootExtraEffect;
	}

	public float GetCooldownResetHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold)
			: m_cooldownResetHealthThreshold;
	}

	public int GetCdrIfDashAndShootUsed()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfDashAndShootUsedMod.GetModifiedValue(m_cdrIfDashAndShootUsed)
			: m_cdrIfDashAndShootUsed;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetSelfHealAmount());
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Self);
		return number;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierStimPack abilityMod_SoldierStimPack = modAsBase as AbilityMod_SoldierStimPack;
		AddTokenInt(tokens, "SelfHealAmount", string.Empty, abilityMod_SoldierStimPack != null
			? abilityMod_SoldierStimPack.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierStimPack != null
			? abilityMod_SoldierStimPack.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierStimPack != null
			? abilityMod_SoldierStimPack.m_dashShootExtraEffectMod.GetModifiedValue(m_dashShootExtraEffect)
			: m_dashShootExtraEffect, "DashShootExtraEffect", m_dashShootExtraEffect);
		AddTokenInt(tokens, "CdrIfDashAndShootUsed", string.Empty, m_cdrIfDashAndShootUsed);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierStimPack))
		{
			m_abilityMod = abilityMod as AbilityMod_SoldierStimPack;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// added in rogues
	public override List<AbilityData.ActionType> GetOtherActionsToCancelOnAbilityUnqueue(ActorData caster)
	{
		if (!NetworkServer.active || m_abilityData == null || m_grenadeAbility == null)
		{
			return null;
		}
		AbilityData.ActionType actionTypeOfAbility = m_abilityData.GetActionTypeOfAbility(m_grenadeAbility);
		if (!m_abilityData.HasQueuedAbilityOfType(typeof(SoldierGrenade))) // , true in rogues
		{
			return null;
		}
		List<AbilityData.ActionType> list = new List<AbilityData.ActionType>();
		List<AbilityRequest> allStoredAbilityRequests = ServerActionBuffer.Get().GetAllStoredAbilityRequests();
		foreach (AbilityRequest abilityRequest in allStoredAbilityRequests)
		{
			if (abilityRequest != null
			    && abilityRequest.m_ability != null
			    && abilityRequest.m_ability.GetType() == typeof(SoldierGrenade)
			    && abilityRequest.m_targets.Count > 0)
			{
				if (!m_abilityData.IsAbilityTargetInRange(m_grenadeAbility, abilityRequest.m_targets[0], 0))
				{
					list.Add(actionTypeOfAbility);
				}
				break;
			}
		}
		return list;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				Board.Get().GetSquare(targets[0].GridPos),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = MakeActorHitRes(caster, caster.GetFreePos());
		actorHitResults.SetBaseHealing(GetSelfHealAmount());
		actorHitResults.AddStandardEffectInfo(GetSelfHitEffect());
		if (GetCdrIfDashAndShootUsed() > 0 && m_abilityData.HasQueuedAbilityOfType(typeof(SoldierDashAndOverwatch))) // , true in rogues
		{
			actorHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(m_myActionType, -1 * GetCdrIfDashAndShootUsed()));
		}
		abilityResults.StoreActorHit(actorHitResults);
	}

	// added in rogues
	public override void OnCalculatedExtraDamageFromEmpoweredGrantedByMyEffect(ActorData effectCaster, ActorData empoweredActor, int extraDamage)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SoldierStats.DamageAddedByStimMight, extraDamage);
	}
#endif
}
