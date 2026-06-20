using System.Collections.Generic;
using UnityEngine;

public class ScampSlamDown : Ability
{
	[Separator("Targeting")]
	public LaserTargetingInfo m_suitedLaserInfo;
	[Header("-- Out of Suit --")]
	public LaserTargetingInfo m_scampLaserInfo;
	[Space(10f)]
	public int m_laserCount = 3;
	public float m_targeterMinAngle;
	public float m_targeterMaxAngle = 360f;
	public float m_targeterMinInterpDistance = 0.5f;
	public float m_targeterMaxInterpDistance = 4f;
	[Separator("Enemy Hit")]
	public int m_suitedBaseDamage = 10;
	public int m_suitedSubseqDamage = 5;
	[Space(10f)]
	public int m_scampBaseDamage = 15;
	public int m_scampSubseqDamage = 5;
	public int m_maxDamage;
	public StandardEffectInfo m_enemyHitEffect;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampSlamDown";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		m_laserCount = Mathf.Max(1, m_laserCount);
		AbilityUtil_Targeter targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(
			this,
			m_laserCount,
			m_suitedLaserInfo,
			10f,
			true,
			m_targeterMinAngle,
			CalculateMaxTargeterAngle(),
			m_targeterMinInterpDistance,
			m_targeterMaxInterpDistance);
		if (targeter is AbilityUtil_Targeter_ThiefFanLaser targeterThiefFanLaser)
		{
			targeterThiefFanLaser.m_delegateLaserLength = GetLaserLengthForTargeter;
			targeterThiefFanLaser.m_delegateLaserWidth = GetLaserWidthForTargeter;
		}
		Targeter = targeter;
	}

	public float CalculateMaxTargeterAngle()
	{
		return m_targeterMaxAngle >= 360f || m_targeterMaxAngle <= 0f
			? 360f - 360f / m_laserCount
			: m_targeterMaxAngle;
	}

	public float GetLaserLengthForTargeter(ActorData caster, float baseLength)
	{
		return IsInSuit()
			? m_suitedLaserInfo.range
			: m_scampLaserInfo.range;
	}

	public float GetLaserWidthForTargeter(ActorData caster, float baseWidth)
	{
		return IsInSuit()
			? m_suitedLaserInfo.width
			: m_scampLaserInfo.width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public bool IsInSuit()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public int CalcTotalDamage(int numHits)
	{
		int damage = 0;
		int baseDamage = IsInSuit() ? m_suitedBaseDamage : m_scampBaseDamage;
		int subseqDamage = IsInSuit() ? m_suitedSubseqDamage : m_scampSubseqDamage;
		if (numHits > 0)
		{
			damage += baseDamage;
			int subseqHits = numHits - 1;
			if (subseqHits > 0 && subseqDamage > 0)
			{
				damage += subseqHits * subseqDamage;
			}
		}
		if (m_maxDamage > 0)
		{
			damage = Mathf.Min(m_maxDamage, damage);
		}
		return damage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_suitedBaseDamage);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) <= 0)
		{
			return false;
		}
		
		int primaryNum = Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		results.m_damage = CalcTotalDamage(primaryNum);
		return true;
	}
}
