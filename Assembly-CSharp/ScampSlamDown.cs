using System.Collections.Generic;
using UnityEngine;

public class ScampSlamDown : Ability
{
	[Separator("Targeting", true)]
	public LaserTargetingInfo m_suitedLaserInfo;

	[Header("-- Out of Suit --")]
	public LaserTargetingInfo m_scampLaserInfo;

	[Space(10f)]
	public int m_laserCount = 3;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 360f;

	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	[Separator("Enemy Hit", true)]
	public int m_suitedBaseDamage = 10;

	public int m_suitedSubseqDamage = 5;

	[Space(10f)]
	public int m_scampBaseDamage = 15;

	public int m_scampSubseqDamage = 5;

	public int m_maxDamage;

	public StandardEffectInfo m_enemyHitEffect;

	[Separator("Sequences", true)]
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
		float maxAngle = CalculateMaxTargeterAngle();
		AbilityUtil_Targeter abilityUtil_Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(this, m_laserCount, m_suitedLaserInfo, 10f, true, m_targeterMinAngle, maxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
		if (abilityUtil_Targeter is AbilityUtil_Targeter_ThiefFanLaser)
		{
			AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = abilityUtil_Targeter as AbilityUtil_Targeter_ThiefFanLaser;
			abilityUtil_Targeter_ThiefFanLaser.m_delegateLaserLength = GetLaserLengthForTargeter;
			abilityUtil_Targeter_ThiefFanLaser.m_delegateLaserWidth = GetLaserWidthForTargeter;
		}
		base.Targeter = abilityUtil_Targeter;
	}

	public float CalculateMaxTargeterAngle()
	{
		if (!(m_targeterMaxAngle >= 360f))
		{
			if (!(m_targeterMaxAngle <= 0f))
			{
				return m_targeterMaxAngle;
			}
		}
		float num = 360f / (float)m_laserCount;
		return 360f - num;
	}

	public float GetLaserLengthForTargeter(ActorData caster, float baseLength)
	{
		float range;
		if (IsInSuit())
		{
			range = m_suitedLaserInfo.range;
		}
		else
		{
			range = m_scampLaserInfo.range;
		}
		return range;
	}

	public float GetLaserWidthForTargeter(ActorData caster, float baseWidth)
	{
		return (!IsInSuit()) ? m_scampLaserInfo.width : m_suitedLaserInfo.width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public bool IsInSuit()
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.m_suitWasActiveOnTurnStart ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int CalcTotalDamage(int numHits)
	{
		int num = 0;
		int num2;
		if (IsInSuit())
		{
			num2 = m_suitedBaseDamage;
		}
		else
		{
			num2 = m_scampBaseDamage;
		}
		int num3 = num2;
		int num4 = (!IsInSuit()) ? m_scampSubseqDamage : m_suitedSubseqDamage;
		if (numHits > 0)
		{
			num += num3;
			int num5 = numHits - 1;
			if (num5 > 0)
			{
				if (num4 > 0)
				{
					num += num5 * num4;
				}
			}
		}
		if (m_maxDamage > 0)
		{
			num = Mathf.Min(m_maxDamage, num);
		}
		return num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_suitedBaseDamage);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					int tooltipSubjectCountOnActor = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
					int num = results.m_damage = CalcTotalDamage(tooltipSubjectCountOnActor);
					return true;
				}
				}
			}
		}
		return false;
	}
}
