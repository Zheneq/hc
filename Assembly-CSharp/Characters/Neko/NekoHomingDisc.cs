using System.Collections.Generic;
using UnityEngine;

public class NekoHomingDisc : Ability
{
	[Separator("Targeting")]
	public float m_laserLength = 6.5f;
	public float m_laserWidth = 1f;
	public int m_maxTargets = 1;
	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;
	[Separator("On Cast Hit")]
	public StandardEffectInfo m_onCastEnemyHitEffect;
	[Separator("On Enemy Hit")]
	public int m_targetDamage = 25; // TODO NEKO unused (always equal to return trip damage)
	public int m_returnTripDamage = 10;
	public bool m_returnTripIgnoreCover = true;
	public float m_extraReturnDamagePerDist;
	public StandardEffectInfo m_returnTripEnemyEffect;
	[Separator("Cooldown Reduction")]
	public int m_cdrIfHitNoOneOnCast; // TODO NEKO unused (always 0)
	public int m_cdrIfHitNoOneOnReturn;
	[Header("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_returnTripSequencePrefab;
	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoHomingDisc m_abilityMod;
	private Neko_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedOnCastEnemyHitEffect;
	private StandardEffectInfo m_cachedReturnTripEnemyEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Homing Disc";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComp = GetComponent<Neko_SyncComponent>();
		Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserLength(), false, GetMaxTargets());
	}

	private void SetCachedFields()
	{
		m_cachedOnCastEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_onCastEnemyHitEffectMod.GetModifiedValue(m_onCastEnemyHitEffect)
			: m_onCastEnemyHitEffect;
		m_cachedReturnTripEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_returnTripEnemyEffectMod.GetModifiedValue(m_returnTripEnemyEffect)
			: m_returnTripEnemyEffect;
	}

	public float GetLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength)
			: m_laserLength;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public float GetDiscReturnEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius)
			: m_discReturnEndRadius;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		return m_cachedOnCastEnemyHitEffect ?? m_onCastEnemyHitEffect;
	}

	public int GetTargetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetDamageMod.GetModifiedValue(m_targetDamage)
			: m_targetDamage;
	}

	public int GetReturnTripDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripDamageMod.GetModifiedValue(m_returnTripDamage)
			: m_returnTripDamage;
	}

	public bool ReturnTripIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover)
			: m_returnTripIgnoreCover;
	}

	public float GetExtraReturnDamagePerDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraReturnDamagePerDistMod.GetModifiedValue(m_extraReturnDamagePerDist)
			: m_extraReturnDamagePerDist;
	}

	public StandardEffectInfo GetReturnTripEnemyEffect()
	{
		return m_cachedReturnTripEnemyEffect ?? m_returnTripEnemyEffect;
	}

	public int GetCdrIfHitNoOneOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfHitNoOneOnCastMod.GetModifiedValue(m_cdrIfHitNoOneOnCast)
			: m_cdrIfHitNoOneOnCast;
	}

	public int GetCdrIfHitNoOneOnReturn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfHitNoOneOnReturnMod.GetModifiedValue(m_cdrIfHitNoOneOnReturn)
			: m_cdrIfHitNoOneOnReturn;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_onCastEnemyHitEffect, "OnCastEnemyHitEffect", m_onCastEnemyHitEffect);
		AddTokenInt(tokens, "TargetDamage", string.Empty, m_targetDamage);
		AddTokenInt(tokens, "ReturnTripDamage", string.Empty, m_returnTripDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_returnTripEnemyEffect, "ReturnTripEnemyEffect", m_returnTripEnemyEffect);
		AddTokenInt(tokens, "CdrIfHitNoOneOnCast", string.Empty, m_cdrIfHitNoOneOnCast);
		AddTokenInt(tokens, "CdrIfHitNoOneOnReturn", string.Empty, m_cdrIfHitNoOneOnReturn);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetTargetDamage()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, GetReturnTripDamage())
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoHomingDisc))
		{
			m_abilityMod = abilityMod as AbilityMod_NekoHomingDisc;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
		
#if SERVER
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare targetSquare;
		if (!additionalData.m_abilityResults.HitActorsArray().IsNullOrEmpty())
		{
			targetSquare = additionalData.m_abilityResults.HitActorsArray()[0].GetCurrentBoardSquare();
		}
		else
		{
			GetHitActors(caster, targets[0], null, out Vector3 laserEndPoint);
			targetSquare = Board.Get().GetSquareClosestToPos(laserEndPoint.x, laserEndPoint.z);
		}
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targetSquare,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		Vector3 losCheckPos = caster.GetLoSCheckPos();
		List<ActorData> hitActors = GetHitActors(caster, currentTarget, nonActorTargetInfo, out _);

		if (hitActors.Count != 0)
		{
			ActorData target = hitActors[0];
			
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, losCheckPos));
			actorHitResults.AddStandardEffectInfo(GetOnCastEnemyHitEffect());
			abilityResults.StoreActorHit(actorHitResults);

			BoardSquare discEndSquare = target.GetCurrentBoardSquare();
			NekoHomingDiscEffect effect = new NekoHomingDiscEffect(
				AsEffectSource(),
				discEndSquare,
				target,
				caster,
				GetDiscReturnEndRadius(),
				GetReturnTripDamage(),
				GetExtraReturnDamagePerDist(),
				GetReturnTripEnemyEffect(),
				GetCdrIfHitNoOneOnReturn(),
				ReturnTripIgnoreCover(),
				m_returnTripSequencePrefab,
				m_persistentDiscSequencePrefab);
			PositionHitParameters positionHitParams = new PositionHitParameters(discEndSquare.ToVector3());
			PositionHitResults positionHitResults = new PositionHitResults(effect, positionHitParams);
			abilityResults.StorePositionHit(positionHitResults);
		}
		
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	private List<ActorData> GetHitActors(
		ActorData caster,
		AbilityTarget currentTarget,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out Vector3 laserEndPoint)
	{
		return AreaEffectUtils.GetActorsInLaser(
			caster.GetLoSCheckPos(),
			currentTarget.AimDirection,
			GetLaserLength(),
			GetLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			false,
			GetMaxTargets(),
			false,
			true,
			out laserEndPoint,
			nonActorTargetInfo);
	}

	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.NekoStats.SeekerDiscDamage,
				results.FinalDamage);

			if (results.m_hitParameters.Effect is NekoAbstractDiscEffect effect)
			{
				effect.ProcessEnlargeDiscExtraDamageFreelancerStat(caster, results);
			}
		}
	}
#endif
}
