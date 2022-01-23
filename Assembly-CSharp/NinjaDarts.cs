using System.Collections.Generic;
using UnityEngine;

public class NinjaDarts : Ability
{
	[Separator("Targeting Properties", true)]
	public LaserTargetingInfo m_laserInfo;

	[Space(10f)]
	public int m_laserCount = 3;

	public float m_angleInBetween = 10f;

	public bool m_changeAngleByCursorDistance = true;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 180f;

	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	[Separator("On Hit Stuff", true)]
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

	[Separator("Energy per dart hit", true)]
	public int m_energyPerDartHit;

	[Separator("Cooldown Reduction", true)]
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
		AbilityUtil_Targeter abilityUtil_Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(this, GetLaserCount(), GetLaserInfo(), GetAngleInBetween(), ChangeAngleByCursorDistance(), GetTargeterMinAngle(), GetTargeterMaxAngle(), GetTargeterMinInterpDistance(), GetTargeterMaxInterpDistance());
		if (ChangeAngleByCursorDistance())
		{
			if (abilityUtil_Targeter is AbilityUtil_Targeter_ThiefFanLaser)
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = abilityUtil_Targeter as AbilityUtil_Targeter_ThiefFanLaser;
				abilityUtil_Targeter_ThiefFanLaser.m_customDamageOriginDelegate = GetCustomDamageOriginForTargeter;
			}
		}
		base.Targeter = abilityUtil_Targeter;
	}

	private Vector3 GetCustomDamageOriginForTargeter(ActorData potentialActor, ActorData caster, Vector3 defaultPos)
	{
		if (IgnoreCoverOnTargets())
		{
			if (ActorIsMarked(potentialActor))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return potentialActor.GetFreePos();
					}
				}
			}
		}
		return defaultPos;
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
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEnemySingleHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemySingleHitEffect = m_abilityMod.m_enemySingleHitEffectMod.GetModifiedValue(m_enemySingleHitEffect);
		}
		else
		{
			cachedEnemySingleHitEffect = m_enemySingleHitEffect;
		}
		m_cachedEnemySingleHitEffect = cachedEnemySingleHitEffect;
		StandardEffectInfo cachedEnemyMultiHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyMultiHitEffect = m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect);
		}
		else
		{
			cachedEnemyMultiHitEffect = m_enemyMultiHitEffect;
		}
		m_cachedEnemyMultiHitEffect = cachedEnemyMultiHitEffect;
		StandardEffectInfo cachedEnemyExtraHitEffectForHitCount;
		if ((bool)m_abilityMod)
		{
			cachedEnemyExtraHitEffectForHitCount = m_abilityMod.m_enemyExtraHitEffectForHitCountMod.GetModifiedValue(m_enemyExtraHitEffectForHitCount);
		}
		else
		{
			cachedEnemyExtraHitEffectForHitCount = m_enemyExtraHitEffectForHitCount;
		}
		m_cachedEnemyExtraHitEffectForHitCount = cachedEnemyExtraHitEffectForHitCount;
		StandardEffectInfo cachedAllySingleHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllySingleHitEffect = m_abilityMod.m_allySingleHitEffectMod.GetModifiedValue(m_allySingleHitEffect);
		}
		else
		{
			cachedAllySingleHitEffect = m_allySingleHitEffect;
		}
		m_cachedAllySingleHitEffect = cachedAllySingleHitEffect;
		m_cachedAllyMultiHitEffect = ((!m_abilityMod) ? m_allyMultiHitEffect : m_abilityMod.m_allyMultiHitEffectMod.GetModifiedValue(m_allyMultiHitEffect));
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public int GetLaserCount()
	{
		int num;
		if ((bool)m_abilityMod)
		{
			num = m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount);
		}
		else
		{
			num = m_laserCount;
		}
		int b = num;
		return Mathf.Max(1, b);
	}

	public float GetAngleInBetween()
	{
		return (!m_abilityMod) ? m_angleInBetween : m_abilityMod.m_angleInBetweenMod.GetModifiedValue(m_angleInBetween);
	}

	public bool ChangeAngleByCursorDistance()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_changeAngleByCursorDistanceMod.GetModifiedValue(m_changeAngleByCursorDistance);
		}
		else
		{
			result = m_changeAngleByCursorDistance;
		}
		return result;
	}

	public float GetTargeterMinAngle()
	{
		float num;
		if ((bool)m_abilityMod)
		{
			num = m_abilityMod.m_targeterMinAngleMod.GetModifiedValue(m_targeterMinAngle);
		}
		else
		{
			num = m_targeterMinAngle;
		}
		float a = num;
		return Mathf.Max(a, 0f);
	}

	public float GetTargeterMaxAngle()
	{
		float b = (!m_abilityMod) ? m_targeterMaxAngle : m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle);
		return Mathf.Max(1f, b);
	}

	public float GetTargeterMinInterpDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_targeterMinInterpDistanceMod.GetModifiedValue(m_targeterMinInterpDistance);
		}
		else
		{
			result = m_targeterMinInterpDistance;
		}
		return result;
	}

	public float GetTargeterMaxInterpDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_targeterMaxInterpDistanceMod.GetModifiedValue(m_targeterMaxInterpDistance);
		}
		else
		{
			result = m_targeterMaxInterpDistance;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			result = m_damage;
		}
		return result;
	}

	public int GetExtraDamagePerSubseqHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamagePerSubseqHitMod.GetModifiedValue(m_extraDamagePerSubseqHit);
		}
		else
		{
			result = m_extraDamagePerSubseqHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemySingleHitEffect != null)
		{
			result = m_cachedEnemySingleHitEffect;
		}
		else
		{
			result = m_enemySingleHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyMultiHitEffect != null)
		{
			result = m_cachedEnemyMultiHitEffect;
		}
		else
		{
			result = m_enemyMultiHitEffect;
		}
		return result;
	}

	public int GetEnemyExtraEffectHitCount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_enemyExtraEffectHitCountMod.GetModifiedValue(m_enemyExtraEffectHitCount);
		}
		else
		{
			result = m_enemyExtraEffectHitCount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyExtraHitEffectForHitCount()
	{
		return (m_cachedEnemyExtraHitEffectForHitCount == null) ? m_enemyExtraHitEffectForHitCount : m_cachedEnemyExtraHitEffectForHitCount;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllySingleHitEffect != null)
		{
			result = m_cachedAllySingleHitEffect;
		}
		else
		{
			result = m_allySingleHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyMultiHitEffect != null)
		{
			result = m_cachedAllyMultiHitEffect;
		}
		else
		{
			result = m_allyMultiHitEffect;
		}
		return result;
	}

	public int GetEnergyPerDartHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyPerDartHitMod.GetModifiedValue(m_energyPerDartHit);
		}
		else
		{
			result = m_energyPerDartHit;
		}
		return result;
	}

	public int GetCdrOnMiss()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrOnMissMod.GetModifiedValue(m_cdrOnMiss);
		}
		else
		{
			result = m_cdrOnMiss;
		}
		return result;
	}

	public bool ApplyDeathmarkEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(m_applyDeathmarkEffect);
		}
		else
		{
			result = m_applyDeathmarkEffect;
		}
		return result;
	}

	public bool IgnoreCoverOnTargets()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_ignoreCoverOnTargetsMod.GetModifiedValue(m_ignoreCoverOnTargets);
		}
		else
		{
			result = m_ignoreCoverOnTargets;
		}
		return result;
	}

	public int CalcDamageFromNumHits(int numHits)
	{
		if (numHits > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					int num = GetDamage();
					if (GetExtraDamagePerSubseqHit() > 0 && numHits > 1)
					{
						num += GetExtraDamagePerSubseqHit() * (numHits - 1);
					}
					return num;
				}
				}
			}
		}
		return 0;
	}

	public bool ActorIsMarked(ActorData actor)
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.ActorHasDeathmark(actor) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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
		int tooltipSubjectCountOnActor = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		int num = results.m_damage = CalcDamageFromNumHits(tooltipSubjectCountOnActor);
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyPerDartHit() > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					int tooltipSubjectCountTotalWithDuplicates = base.Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
					return tooltipSubjectCountTotalWithDuplicates * GetEnergyPerDartHit();
				}
				}
			}
		}
		return 0;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (m_syncComp != null)
			{
				if (m_syncComp.m_deathmarkOnTriggerDamage > 0 && m_syncComp.ActorHasDeathmark(targetActor))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, m_syncComp.m_deathmarkOnTriggerDamage, false);
						}
					}
				}
			}
		}
		return null;
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, GetTargeterMaxAngle(), GetTargeterMinInterpDistance(), GetTargeterMaxInterpDistance());
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetTargeterMinInterpDistance() * Board.Get().squareSize;
		max = GetTargeterMaxInterpDistance() * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NinjaDarts))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NinjaDarts);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
