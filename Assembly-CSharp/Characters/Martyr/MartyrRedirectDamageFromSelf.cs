// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrRedirectDamageFromSelf : MartyrLaserBase
{
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnCaster = 0.5f;
	public float m_damageRedirectToTarget = 0.5f;
	public int m_techPointGainPerRedirect = 3;
	public StandardEffectInfo m_selfHitEffect;
	public bool m_affectsEnemies = true;
	public bool m_affectsAllies;
	public bool m_penetratesLoS;
	public StandardEffectInfo m_effectOnTarget;
	[Header("-- Self protection")]
	public int m_baseAbsorb;
	public int m_absorbPerCrystalSpent = 5;
	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Sequences")]
	public GameObject m_castSequence;
	public GameObject m_projectileSequence;
	public GameObject m_redirectProjectileSequence;

	private Martyr_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedEffectOnTarget;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Redirect Damage From Self";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, m_affectsEnemies, m_affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Always)
		{
			m_affectCasterDelegate = delegate (ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
			{
				return GetCurrentAbsorb(caster) > 0;
			}
		};
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_selfHitEffect;
		m_cachedEffectOnTarget = m_effectOnTarget;
	}

	public float GetDamageReductionOnCaster()
	{
		return m_damageReductionOnCaster;
	}

	public float GetDamageRedirectToTarget()
	{
		return m_damageRedirectToTarget;
	}

	public int GetTechPointGainPerRedirect()
	{
		return m_techPointGainPerRedirect;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public StandardEffectInfo GetEffectOnTarget()
	{
		return m_cachedEffectOnTarget ?? m_effectOnTarget;
	}

	public int GetAbsorbAmountPerCrystalSpent()
	{
		return m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbAmount()
	{
		return m_baseAbsorb;
	}

	public bool GetPenetratesLoS()
	{
		return m_penetratesLoS;
	}

	public float GetMaxRange()
	{
		return GetRangeInSquares(0);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfHitEffect, "SelfEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTarget, "TargetEffect", m_effectOnTarget);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", GetBonusLengthPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == ActorData)
		{
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, ActorData, GetCurrentAbsorb(ActorData), AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(caster, target.GetCurrentBestActorTarget(), m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrProtectAllyThreshold bonus in m_thresholdBasedCrystalBonuses)
		{
			list.Add(bonus);
		}
		return list;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int additionalAbsorb = martyrProtectAllyThreshold != null ? martyrProtectAllyThreshold.m_additionalAbsorb : 0;
		return GetBaseAbsorbAmount()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbAmountPerCrystalSpent()
			+ additionalAbsorb;
	}

#if SERVER

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo);
		list.Add(new ServerClientUtils.SequenceStartData(m_projectileSequence, laserCoords.end, hitActors.ToArray(), caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[0]));
		list.Add(new ServerClientUtils.SequenceStartData(m_castSequence, laserCoords.end, new ActorData[]
		{
			caster
		}, caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[0]));
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo);
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		StandardActorEffectData selfHitEffect = GetSelfHitEffect().m_effectData.GetShallowCopy();
		selfHitEffect.m_absorbAmount = GetCurrentAbsorb(caster);
		foreach (ActorData hitActor in hitActors)
		{
			MartyrDamageRedirectEffect effect = new MartyrDamageRedirectEffect(AsEffectSource(), hitActor.GetCurrentBoardSquare(), caster, caster, false, new List<ActorData>
			{
				hitActor
			}, selfHitEffect, GetDamageReductionOnCaster(), GetDamageRedirectToTarget(), GetTechPointGainPerRedirect(), GetMaxRange(), null, m_redirectProjectileSequence);
			actorHitResults.AddEffect(effect);
			ActorHitParameters hitParams = new ActorHitParameters(hitActor, caster.GetFreePos());
			ActorHitResults hitResults;
			if (GetEffectOnTarget().m_applyEffect)
			{
				hitResults = new ActorHitResults(GetEffectOnTarget(), hitParams);
			}
			else
			{
				hitResults = new ActorHitResults(hitParams);
			}
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreActorHit(actorHitResults);
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	protected List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		endPoints = default(VectorUtils.LaserCoords);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (square != null
			&& square.OccupantActor != null
			&& !square.OccupantActor.IgnoreForAbilityHits
			&& (m_affectsAllies || square.OccupantActor.GetTeam() != caster.GetTeam()))
		{
			return new List<ActorData>
			{
				square.OccupantActor
			};
		}
		return new List<ActorData>();
	}
#endif
}
