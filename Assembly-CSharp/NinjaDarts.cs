using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NinjaDarts : Ability
{
	[Separator("Targeting Properties")]
	public LaserTargetingInfo m_laserInfo;
	[Space(10f)]
	public int m_laserCount = 3;
	public float m_angleInBetween = 10f;
	public bool m_changeAngleByCursorDistance = true;
	public float m_targeterMinAngle;
	public float m_targeterMaxAngle = 180f;
	public float m_targeterMinInterpDistance = 0.5f;
	public float m_targeterMaxInterpDistance = 4f;
	[Separator("On Hit Stuff")]
	public int m_damage = 10;
	public int m_extraDamagePerSubseqHit;
	[Space(10f)]
	public StandardEffectInfo m_enemySingleHitEffect;
	public StandardEffectInfo m_enemyMultiHitEffect;
	[Header("-- For effect when hitting over certain number of lasers --")]
	public int m_enemyExtraEffectHitCount;
	public StandardEffectInfo m_enemyExtraHitEffectForHitCount;
	[Header("-- For Ally Hit --")]
	public StandardEffectInfo m_allySingleHitEffect;
	public StandardEffectInfo m_allyMultiHitEffect;
	[Separator("Energy per dart hit")]
	public int m_energyPerDartHit;
	[Separator("Cooldown Reduction")]
	public int m_cdrOnMiss;
	[Separator("[Deathmark] Effect", "magenta")]
	public bool m_applyDeathmarkEffect = true;
	public bool m_ignoreCoverOnTargets;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_NinjaDarts m_abilityMod;
	private Ninja_SyncComponent m_syncComp;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedEnemySingleHitEffect;
	private StandardEffectInfo m_cachedEnemyMultiHitEffect;
	private StandardEffectInfo m_cachedEnemyExtraHitEffectForHitCount;
	private StandardEffectInfo m_cachedAllySingleHitEffect;
	private StandardEffectInfo m_cachedAllyMultiHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "NinjaDarts";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
		AbilityUtil_Targeter abilityUtil_Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(
			this,
			GetLaserCount(),
			GetLaserInfo(),
			GetAngleInBetween(),
			ChangeAngleByCursorDistance(),
			GetTargeterMinAngle(),
			GetTargeterMaxAngle(),
			GetTargeterMinInterpDistance(),
			GetTargeterMaxInterpDistance());
		if (ChangeAngleByCursorDistance())
		{
			AbilityUtil_Targeter_ThiefFanLaser targeter = abilityUtil_Targeter as AbilityUtil_Targeter_ThiefFanLaser;
			if (targeter != null)
			{
				targeter.m_customDamageOriginDelegate = GetCustomDamageOriginForTargeter;
			}
		}
		Targeter = abilityUtil_Targeter;
	}

	private Vector3 GetCustomDamageOriginForTargeter(ActorData potentialActor, ActorData caster, Vector3 defaultPos)
	{
		return IgnoreCoverOnTargets() && ActorIsMarked(potentialActor)
			? potentialActor.GetFreePos()
			: defaultPos;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LaserCount", string.Empty, m_laserCount);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AddTokenInt(tokens, "ExtraDamagePerSubseqHit", string.Empty, m_extraDamagePerSubseqHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemySingleHitEffect, "EnemySingleHitEffect", m_enemySingleHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyMultiHitEffect, "EnemyMultiHitEffect", m_enemyMultiHitEffect);
		AddTokenInt(tokens, "EnemyExtraEffectHitCount", string.Empty, m_enemyExtraEffectHitCount);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyExtraHitEffectForHitCount, "EnemyExtraHitEffectForHitCount", m_enemyExtraHitEffectForHitCount);
		AbilityMod.AddToken_EffectInfo(tokens, m_allySingleHitEffect, "AllySingleHitEffect", m_allySingleHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyMultiHitEffect, "AllyMultiHitEffect", m_allyMultiHitEffect);
		AddTokenInt(tokens, "EnergyPerDartHit", string.Empty, m_energyPerDartHit);
		AddTokenInt(tokens, "CdrOnMiss", string.Empty, m_cdrOnMiss);
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedEnemySingleHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemySingleHitEffectMod.GetModifiedValue(m_enemySingleHitEffect)
			: m_enemySingleHitEffect;
		m_cachedEnemyMultiHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect)
			: m_enemyMultiHitEffect;
		m_cachedEnemyExtraHitEffectForHitCount = m_abilityMod != null
			? m_abilityMod.m_enemyExtraHitEffectForHitCountMod.GetModifiedValue(m_enemyExtraHitEffectForHitCount)
			: m_enemyExtraHitEffectForHitCount;
		m_cachedAllySingleHitEffect = m_abilityMod != null
			? m_abilityMod.m_allySingleHitEffectMod.GetModifiedValue(m_allySingleHitEffect)
			: m_allySingleHitEffect;
		m_cachedAllyMultiHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyMultiHitEffectMod.GetModifiedValue(m_allyMultiHitEffect)
			: m_allyMultiHitEffect;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public int GetLaserCount()
	{
		int laserCount = m_abilityMod != null
			? m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount)
			: m_laserCount;
		return Mathf.Max(1, laserCount);
	}

	public float GetAngleInBetween()
	{
		return m_abilityMod != null
			? m_abilityMod.m_angleInBetweenMod.GetModifiedValue(m_angleInBetween)
			: m_angleInBetween;
	}

	public bool ChangeAngleByCursorDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_changeAngleByCursorDistanceMod.GetModifiedValue(m_changeAngleByCursorDistance)
			: m_changeAngleByCursorDistance;
	}

	public float GetTargeterMinAngle()
	{
		float minAngle = m_abilityMod != null
			? m_abilityMod.m_targeterMinAngleMod.GetModifiedValue(m_targeterMinAngle)
			: m_targeterMinAngle;
		return Mathf.Max(minAngle, 0f);
	}

	public float GetTargeterMaxAngle()
	{
		float maxAngle = m_abilityMod != null
			? m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle)
			: m_targeterMaxAngle;
		return Mathf.Max(1f, maxAngle);
	}

	public float GetTargeterMinInterpDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterMinInterpDistanceMod.GetModifiedValue(m_targeterMinInterpDistance)
			: m_targeterMinInterpDistance;
	}

	public float GetTargeterMaxInterpDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterMaxInterpDistanceMod.GetModifiedValue(m_targeterMaxInterpDistance)
			: m_targeterMaxInterpDistance;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int GetExtraDamagePerSubseqHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerSubseqHitMod.GetModifiedValue(m_extraDamagePerSubseqHit)
			: m_extraDamagePerSubseqHit;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		return m_cachedEnemySingleHitEffect ?? m_enemySingleHitEffect;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		return m_cachedEnemyMultiHitEffect ?? m_enemyMultiHitEffect;
	}

	public int GetEnemyExtraEffectHitCount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyExtraEffectHitCountMod.GetModifiedValue(m_enemyExtraEffectHitCount)
			: m_enemyExtraEffectHitCount;
	}

	public StandardEffectInfo GetEnemyExtraHitEffectForHitCount()
	{
		return m_cachedEnemyExtraHitEffectForHitCount ?? m_enemyExtraHitEffectForHitCount;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		return m_cachedAllySingleHitEffect ?? m_allySingleHitEffect;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		return m_cachedAllyMultiHitEffect ?? m_allyMultiHitEffect;
	}

	public int GetEnergyPerDartHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyPerDartHitMod.GetModifiedValue(m_energyPerDartHit)
			: m_energyPerDartHit;
	}

	public int GetCdrOnMiss()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnMissMod.GetModifiedValue(m_cdrOnMiss)
			: m_cdrOnMiss;
	}

	public bool ApplyDeathmarkEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(m_applyDeathmarkEffect)
			: m_applyDeathmarkEffect;
	}

	public bool IgnoreCoverOnTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignoreCoverOnTargetsMod.GetModifiedValue(m_ignoreCoverOnTargets)
			: m_ignoreCoverOnTargets;
	}

	public int CalcDamageFromNumHits(int numHits)
	{
		if (numHits <= 0)
		{
			return 0;
		}
		int damage = GetDamage();
		if (GetExtraDamagePerSubseqHit() > 0 && numHits > 1)
		{
			damage += GetExtraDamagePerSubseqHit() * (numHits - 1);
		}
		return damage;
	}

	public bool ActorIsMarked(ActorData actor)
	{
		return m_syncComp != null && m_syncComp.ActorHasDeathmark(actor);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamage());
		GetEnemySingleHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		GetEnemyMultiHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		int tooltipSubjectCountOnActor = Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		results.m_damage = CalcDamageFromNumHits(tooltipSubjectCountOnActor);
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyPerDartHit() <= 0)
		{
			return 0;
		}
		int hits = Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
		return hits * GetEnergyPerDartHit();
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage
		       && m_syncComp != null
		       && m_syncComp.m_deathmarkOnTriggerDamage > 0
		       && m_syncComp.ActorHasDeathmark(targetActor)
			? new StringBuilder().Append("\n+ ")
				.Append(AbilityUtils.CalculateDamageForTargeter(
					ActorData, targetActor, this, m_syncComp.m_deathmarkOnTriggerDamage, false))
				.ToString()
			: null;
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(
			fanAngleDegrees, 
			GetTargeterMaxAngle(),
			GetTargeterMinInterpDistance(),
			GetTargeterMaxInterpDistance());
	}

	public override bool HasRestrictedFreePosDistance(
		ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetTargeterMinInterpDistance() * Board.Get().squareSize;
		max = GetTargeterMaxInterpDistance() * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaDarts))
		{
			m_abilityMod = abilityMod as AbilityMod_NinjaDarts;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
