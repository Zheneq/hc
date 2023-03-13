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
	public int m_extraDamageForCenterHits;
	public float m_centerHitWidth = 0.1f;
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
		AbilityUtil_Targeter_KnockbackLaser abilityUtil_Targeter_KnockbackLaser = new AbilityUtil_Targeter_KnockbackLaser(this, GetLaserWidth(), GetLaserRangeInSquares(), false, m_maxTargets, GetMaxKnockbackDist(), GetMaxKnockbackDist(), m_knockbackType, false);
		abilityUtil_Targeter_KnockbackLaser.LengthIgnoreWorldGeo = m_lengthIgnoreLos;
		base.Targeter = abilityUtil_Targeter_KnockbackLaser;
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
		m_cachedEffectToEnemies = (m_abilityMod ? m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies) : m_effectToEnemies);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectToEnemies, "EffectToEnemies", m_effectToEnemies);
		AddTokenInt(tokens, "ExtraDamageForCenterHits", string.Empty, m_extraDamageForCenterHits);
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
		int num = GetDamage();
		int extraDamageIfKnockedInPlace = GetExtraDamageIfKnockedInPlace();
		if (extraDamageIfKnockedInPlace != 0 && !targetActor.GetActorStatus().IsMovementDebuffImmune(true))
		{
			foreach (AbilityUtil_Targeter.ActorTarget actorTarget in base.Targeter.GetActorsInRange())
			{
				if (actorTarget.m_actor == targetActor)
				{
					if (actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.HighHP))
					{
						num += extraDamageIfKnockedInPlace;
						break;
					}
					break;
				}
			}
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public float GetLaserWidth()
	{
		if (!m_abilityMod)
		{
			return m_laserWidth;
		}
		return m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
	}

	public float GetLaserRangeInSquares()
	{
		return (!m_abilityMod) ? m_laserRangeInSquares : m_abilityMod.m_laserRangeInSquaresMod.GetModifiedValue(m_laserRangeInSquares);
	}

	public int GetMaxTargets()
	{
		if (!m_abilityMod)
		{
			return m_maxTargets;
		}
		return m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
	}

	public bool LengthIgnoreLos()
	{
		if (!m_abilityMod)
		{
			return m_lengthIgnoreLos;
		}
		return m_abilityMod.m_lengthIgnoreLosMod.GetModifiedValue(m_lengthIgnoreLos);
	}

	public int GetDamage()
	{
		if (!m_abilityMod)
		{
			return m_damage;
		}
		return m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
	}

	public int GetExtraDamageIfKnockedInPlace()
	{
		return m_abilityMod ? m_abilityMod.m_extraDamageIfKnockedInPlaceMod.GetModifiedValue(0) : 0;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToEnemies != null)
		{
			result = m_cachedEffectToEnemies;
		}
		else
		{
			result = m_effectToEnemies;
		}
		return result;
	}

	public int GetExtraDamageForCenterHits()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageForCenterHitsMod.GetModifiedValue(m_extraDamageForCenterHits);
		}
		else
		{
			result = m_extraDamageForCenterHits;
		}
		return result;
	}

	public float GetCenterHitWidth()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_centerHitWidthMod.GetModifiedValue(m_centerHitWidth);
		}
		else
		{
			result = m_centerHitWidth;
		}
		return result;
	}

	public float GetMaxKnockbackDist()
	{
		if (!m_abilityMod)
		{
			return m_maxKnockbackDist;
		}
		return m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist);
	}

	public KnockbackType GetKnockbackType()
	{
		return (!m_abilityMod) ? m_knockbackType : m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
	}

	//Added in rouges
	public bool ShouldSkipDamageReductionOnNextTurnStab()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = (m_abilityMod.m_nextTurnStabSkipsDamageReduction.GetModifiedValue(false) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyriePullToLaserCenter))
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyriePullToLaserCenter);
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
	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo, out Vector3 endPos)
	{
		return AreaEffectUtils.GetActorsInLaser(caster.GetLoSCheckPos(), targets[0].AimDirection, GetLaserRangeInSquares(), GetLaserWidth(), caster, caster.GetOtherTeams(), false, m_maxTargets, m_lengthIgnoreLos, true, out endPos, nonActorTargetInfo, null, false, true);
	}

	//Added in rouges
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
        List<ActorData> list = FindHitActors(targets, caster, null, out Vector3 targetPos);
        targetPos.y = (float)Board.Get().BaselineHeight;
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, targetPos, list.ToArray(), caster, additionalData.m_sequenceSource, null);
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo, out Vector3 vector);
        Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData actorData in list)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, loSCheckPos);
			ActorHitResults actorHitResults = new ActorHitResults(GetDamage(), HitActionType.Damage, GetEffectToEnemies(), hitParams);
			if (GetMaxKnockbackDist() != 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(actorData, caster, m_knockbackType, targets[0].AimDirection, loSCheckPos, GetMaxKnockbackDist());
				actorHitResults.AddKnockbackData(knockbackData);
				int extraDamageIfKnockedInPlace = GetExtraDamageIfKnockedInPlace();
				if (extraDamageIfKnockedInPlace != 0 && !actorData.GetActorStatus().IsMovementDebuffImmune(true) && KnockbackUtils.BuildKnockbackPath(actorData, 
					m_knockbackType, 
					targets[0].AimDirection, 
					loSCheckPos, 
					GetMaxKnockbackDist()).FindMoveCostToEnd() < 0.5f)
				{
					actorHitResults.AddBaseDamage(extraDamageIfKnockedInPlace);
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	//Added in rouges
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam() && results.HasKnockback && !target.GetActorStatus().HasStatus(StatusType.Unstoppable, true))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ValkyrieStats.NumKnockbackTargetsWithUlt);
		}
	}
#endif
}
