// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class FishManBubbleLaser : Ability
{
	[Header("-- Targeting")]
	[Space(10f)]
	public LaserTargetingInfo m_laserInfo;
	[Header("-- Initial Hit")]
	public StandardEffectInfo m_effectOnAllies;
	public StandardEffectInfo m_effectOnEnemies;
	public int m_initialHitHealingToAllies;
	public int m_initialHitDamageToEnemies;
	[Header("-- Explosion Data")]
	public int m_numTurnsBeforeFirstExplosion = 1;
	public int m_numExplosionsBeforeEnding = 1;
	public AbilityAreaShape m_explosionShape;
	public bool m_explosionIgnoresLineOfSight;
	public bool m_explosionCanAffectEffectHolder;
	[Header("-- Explosion Results")]
	public int m_explosionHealingToAllies;
	public int m_explosionDamageToEnemies;
	public StandardEffectInfo m_explosionEffectToAllies;
	public StandardEffectInfo m_explosionEffectToEnemies;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_bubbleSequencePrefab;
	public GameObject m_explosionSequencePrefab;

	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedEffectOnAllies;
	private StandardEffectInfo m_cachedEffectOnEnemies;
	private StandardEffectInfo m_cachedExplosionEffectToAllies;
	private StandardEffectInfo m_cachedExplosionEffectToEnemies;
	private AbilityMod_FishManBubbleLaser m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Laser(this, m_cachedLaserInfo);
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedEffectOnAllies = m_abilityMod != null
			? m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies;
		m_cachedEffectOnEnemies = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies;
		m_cachedExplosionEffectToAllies = m_abilityMod != null
			? m_abilityMod.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies)
			: m_explosionEffectToAllies;
		m_cachedExplosionEffectToEnemies = m_abilityMod != null
			? m_abilityMod.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies)
			: m_explosionEffectToEnemies;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		return m_cachedEffectOnAllies ?? m_effectOnAllies;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return m_cachedEffectOnEnemies ?? m_effectOnEnemies;
	}

	public int GetInitialHitHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies)
			: m_initialHitHealingToAllies;
	}

	public int GetInitialHitDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies)
			: m_initialHitDamageToEnemies;
	}

	public int GetNumTurnsBeforeFirstExplosion()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion)
			: m_numTurnsBeforeFirstExplosion;
	}

	public int GetNumExplosionsBeforeEnding()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding)
			: m_numExplosionsBeforeEnding;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape)
			: m_explosionShape;
	}

	public bool ExplosionIgnoresLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionIgnoresLineOfSightMod.GetModifiedValue(m_explosionIgnoresLineOfSight)
			: m_explosionIgnoresLineOfSight;
	}

	public bool ExplosionCanAffectEffectHolder()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionCanAffectEffectHolderMod.GetModifiedValue(m_explosionCanAffectEffectHolder)
			: m_explosionCanAffectEffectHolder;
	}

	public int GetExplosionHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies)
			: m_explosionHealingToAllies;
	}

	public int GetExplosionDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies)
			: m_explosionDamageToEnemies;
	}

	public StandardEffectInfo GetExplosionEffectToAllies()
	{
		return m_cachedExplosionEffectToAllies ?? m_explosionEffectToAllies;
	}

	public StandardEffectInfo GetExplosionEffectToEnemies()
	{
		return m_cachedExplosionEffectToEnemies ?? m_explosionEffectToEnemies;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManBubbleLaser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_FishManBubbleLaser;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManBubbleLaser abilityMod_FishManBubbleLaser = modAsBase as AbilityMod_FishManBubbleLaser;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies, "EffectOnAllies", m_effectOnAllies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies, "EffectOnEnemies", m_effectOnEnemies);
		AddTokenInt(tokens, "InitialHitHealingToAllies", string.Empty, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies)
			: m_initialHitHealingToAllies);
		AddTokenInt(tokens, "InitialHitDamageToEnemies", string.Empty, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies)
			: m_initialHitDamageToEnemies);
		AddTokenInt(tokens, "NumTurnsBeforeFirstExplosion", string.Empty, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion)
			: m_numTurnsBeforeFirstExplosion);
		AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding)
			: m_numExplosionsBeforeEnding);
		AddTokenInt(tokens, "ExplosionHealingToAllies", string.Empty, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies)
			: m_explosionHealingToAllies);
		AddTokenInt(tokens, "ExplosionDamageToEnemies", string.Empty, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies)
			: m_explosionDamageToEnemies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies)
			: m_explosionEffectToAllies, "ExplosionEffectToAllies", m_explosionEffectToAllies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubbleLaser != null
			? abilityMod_FishManBubbleLaser.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies)
			: m_explosionEffectToEnemies, "ExplosionEffectToEnemies", m_explosionEffectToEnemies);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (GetInitialHitDamageToEnemies() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Enemy, GetInitialHitDamageToEnemies());
		}
		if (GetInitialHitHealingToAllies() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, GetInitialHitHealingToAllies());
		}
		GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Enemy);
		GetEffectOnAllies().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Ally);
		return number;
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
			Board.Get().GetSquare(targets[0].GridPos),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out _, nonActorTargetInfo);
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData.GetTeam() == caster.GetTeam())
			{
				actorHitResults.AddBaseHealing(GetInitialHitHealingToAllies());
				actorHitResults.AddEffect(new FishManBubbleEffect(
					AsEffectSource(),
					actorData.GetCurrentBoardSquare(),
					actorData,
					caster,
					GetEffectOnAllies().m_effectData,
					GetNumTurnsBeforeFirstExplosion(),
					GetNumExplosionsBeforeEnding(),
					m_bubbleSequencePrefab,
					0f,
					GetExplosionShape(),
					ExplosionIgnoresLineOfSight(),
					ExplosionCanAffectEffectHolder(),
					m_explosionSequencePrefab,
					GetExplosionHealingToAllies(),
					GetExplosionDamageToEnemies(),
					GetExplosionEffectToAllies(),
					GetExplosionEffectToEnemies()));
			}
			else
			{
				actorHitResults.AddBaseDamage(GetInitialHitDamageToEnemies());
				actorHitResults.AddEffect(new FishManBubbleEffect(
					AsEffectSource(),
					actorData.GetCurrentBoardSquare(),
					actorData,
					caster,
					GetEffectOnEnemies().m_effectData,
					GetNumTurnsBeforeFirstExplosion(),
					GetNumExplosionsBeforeEnding(),
					m_bubbleSequencePrefab,
					0f,
					GetExplosionShape(),
					ExplosionIgnoresLineOfSight(),
					ExplosionCanAffectEffectHolder(),
					m_explosionSequencePrefab,
					GetExplosionHealingToAllies(),
					GetExplosionDamageToEnemies(),
					GetExplosionEffectToAllies(),
					GetExplosionEffectToEnemies()));
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
	}

	// added in rogues
	private List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out VectorUtils.LaserCoords endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			GetLaserInfo().range,
			GetLaserInfo().width,
			caster,
			TargeterUtils.GetRelevantTeams(caster, m_cachedLaserInfo.affectsAllies, m_cachedLaserInfo.affectsEnemies),
			GetLaserInfo().penetrateLos,
			GetLaserInfo().maxTargets,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		endPoints = laserCoords;
		return actorsInLaser;
	}
#endif
}
