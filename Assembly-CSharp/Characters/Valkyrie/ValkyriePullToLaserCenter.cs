// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePullToLaserCenter : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 5f;
	public float m_laserRangeInSquares = 6.5f;
	public int m_maxTargets = 5;
	public bool m_lengthIgnoreLos = true;
	[Header("-- Damage & effects")]
	public int m_damage = 40;
	public StandardEffectInfo m_effectToEnemies;
	public int m_extraDamageForCenterHits; // removed in rogues
	public float m_centerHitWidth = 0.1f; // removed in rogues
	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;
	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	private AbilityMod_ValkyriePullToLaserCenter m_abilityMod;
	private StandardEffectInfo m_cachedEffectToEnemies;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Valkyrie Pull Beam";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_KnockbackLaser(
			this,
			GetLaserWidth(),
			GetLaserRangeInSquares(),
			false,
			m_maxTargets,
			GetMaxKnockbackDist(),
			GetMaxKnockbackDist(),
			m_knockbackType,
			false)
		{
			LengthIgnoreWorldGeo = m_lengthIgnoreLos
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRangeInSquares();
	}

	private void SetCachedFields()
	{
		m_cachedEffectToEnemies = m_abilityMod != null
			? m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies)
			: m_effectToEnemies;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectToEnemies, "EffectToEnemies", m_effectToEnemies);
		AddTokenInt(tokens, "ExtraDamageForCenterHits", string.Empty, m_extraDamageForCenterHits); // removed in rogues
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int damage = GetDamage();
		int extraDamageIfKnockedInPlace = GetExtraDamageIfKnockedInPlace();
		if (extraDamageIfKnockedInPlace != 0 && !targetActor.GetActorStatus().IsMovementDebuffImmune())
		{
			foreach (AbilityUtil_Targeter.ActorTarget target in Targeter.GetActorsInRange())
			{
				if (target.m_actor != targetActor)
				{
					continue;
				}
				if (target.m_subjectTypes.Contains(AbilityTooltipSubject.HighHP))
				{
					damage += extraDamageIfKnockedInPlace;
				}
				break;
			}
		}
		
		// removed in rogues
		int extraDamageForCenterHits = GetExtraDamageForCenterHits();
		if (extraDamageForCenterHits > 0 && Targeter is AbilityUtil_Targeter_KnockbackLaser targeter)
		{
			if (AreaEffectUtils.IsSquareInBoxByActorRadius(
				    targetActor.GetCurrentBoardSquare(),
				    ActorData.GetLoSCheckPos(),
				    targeter.GetLastLaserEndPos(),
				    GetCenterHitWidth()))
			{
				damage += extraDamageForCenterHits;
			}
		}
		// end removed in rogues
		
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetLaserRangeInSquares()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeInSquaresMod.GetModifiedValue(m_laserRangeInSquares)
			: m_laserRangeInSquares;
	}

	// TODO VALKYRIE not used in code, not used in any mods, m_maxTargets used instead in desc and still does not actually limit anything
	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	// TODO VALKYRIE not used in code, not used in any mods, m_lengthIgnoreLos is actually used everywhere including client
	public bool LengthIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lengthIgnoreLosMod.GetModifiedValue(m_lengthIgnoreLos)
			: m_lengthIgnoreLos;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int GetExtraDamageIfKnockedInPlace()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfKnockedInPlaceMod.GetModifiedValue(0)
			: 0;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		return m_cachedEffectToEnemies ?? m_effectToEnemies;
	}

	// removed in rogues
	public int GetExtraDamageForCenterHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForCenterHitsMod.GetModifiedValue(m_extraDamageForCenterHits)
			: m_extraDamageForCenterHits;
	}

	// removed in rogues
	public float GetCenterHitWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_centerHitWidthMod.GetModifiedValue(m_centerHitWidth)
			: m_centerHitWidth;
	}

	public float GetMaxKnockbackDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist)
			: m_maxKnockbackDist;
	}

	// TODO VALKYRIE not used in code, not used in any mods, m_knockbackType is actually used everywhere including client
	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public bool ShouldSkipDamageReductionOnNextTurnStab()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_nextTurnStabSkipsDamageReduction.GetModifiedValue(false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyriePullToLaserCenter))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyriePullToLaserCenter;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

#if SERVER
	//Added in rouges
	private List<ActorData> FindHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out Vector3 endPos)
	{
		return AreaEffectUtils.GetActorsInLaser(
			caster.GetLoSCheckPos(),
			targets[0].AimDirection,
			GetLaserRangeInSquares(),
			GetLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			false,
			m_maxTargets,
			m_lengthIgnoreLos,
			true,
			out endPos,
			nonActorTargetInfo);
	}

	//Added in rouges
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> list = FindHitActors(targets, caster, null, out Vector3 targetPos);
		targetPos.y = Board.Get().BaselineHeight;
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targetPos,
			list.ToArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = FindHitActors(targets, caster, nonActorTargetInfo, out Vector3 laserEndPoint);
		Vector3 casterPos = caster.GetLoSCheckPos();
		foreach (ActorData actorData in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, casterPos);
			ActorHitResults actorHitResults = new ActorHitResults(GetDamage(), HitActionType.Damage, GetEffectToEnemies(), hitParams);
			if (GetMaxKnockbackDist() != 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(
					actorData,
					caster,
					m_knockbackType,
					targets[0].AimDirection,
					casterPos,
					GetMaxKnockbackDist());
				actorHitResults.AddKnockbackData(knockbackData);
				int extraDamageIfKnockedInPlace = GetExtraDamageIfKnockedInPlace();
				if (extraDamageIfKnockedInPlace != 0
				    && !actorData.GetActorStatus().IsMovementDebuffImmune())
				{
					BoardSquarePathInfo knockbackPath = KnockbackUtils.BuildKnockbackPath(
						actorData,
						m_knockbackType,
						targets[0].AimDirection,
						casterPos,
						GetMaxKnockbackDist());
					if (knockbackPath.FindMoveCostToEnd() < 0.5f)
					{
						actorHitResults.AddBaseDamage(extraDamageIfKnockedInPlace);
					}
				}
				
				// custom
				int extraDamageForCenterHits = GetExtraDamageForCenterHits();
				if (extraDamageForCenterHits > 0
				    && AreaEffectUtils.IsSquareInBoxByActorRadius(
					    actorData.GetCurrentBoardSquare(),
					    ActorData.GetLoSCheckPos(),
					    laserEndPoint,
					    GetCenterHitWidth()))
				{
					actorHitResults.AddBaseDamage(extraDamageForCenterHits);
				}
				// end custom
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	//Added in rouges
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam()
		    && results.HasKnockback
		    && !target.GetActorStatus().HasStatus(StatusType.Unstoppable))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ValkyrieStats.NumKnockbackTargetsWithUlt);
		}
	}
#endif
}
