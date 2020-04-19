using System;
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
	public int m_suitedBaseDamage = 0xA;

	public int m_suitedSubseqDamage = 5;

	[Space(10f)]
	public int m_scampBaseDamage = 0xF;

	public int m_scampSubseqDamage = 5;

	public int m_maxDamage;

	public StandardEffectInfo m_enemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.Start()).MethodHandle;
			}
			this.m_abilityName = "ScampSlamDown";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		this.m_laserCount = Mathf.Max(1, this.m_laserCount);
		float maxAngle = this.CalculateMaxTargeterAngle();
		AbilityUtil_Targeter abilityUtil_Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(this, this.m_laserCount, this.m_suitedLaserInfo, 10f, true, this.m_targeterMinAngle, maxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance);
		if (abilityUtil_Targeter is AbilityUtil_Targeter_ThiefFanLaser)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.Setup()).MethodHandle;
			}
			AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = abilityUtil_Targeter as AbilityUtil_Targeter_ThiefFanLaser;
			abilityUtil_Targeter_ThiefFanLaser.m_delegateLaserLength = new AbilityUtil_Targeter_ThiefFanLaser.LaserLengthDelegate(this.GetLaserLengthForTargeter);
			abilityUtil_Targeter_ThiefFanLaser.m_delegateLaserWidth = new AbilityUtil_Targeter_ThiefFanLaser.LaserWidthDelegate(this.GetLaserWidthForTargeter);
		}
		base.Targeter = abilityUtil_Targeter;
	}

	public float CalculateMaxTargeterAngle()
	{
		if (this.m_targeterMaxAngle < 360f)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.CalculateMaxTargeterAngle()).MethodHandle;
			}
			if (this.m_targeterMaxAngle > 0f)
			{
				return this.m_targeterMaxAngle;
			}
		}
		float num = 360f / (float)this.m_laserCount;
		return 360f - num;
	}

	public float GetLaserLengthForTargeter(ActorData caster, float baseLength)
	{
		float range;
		if (this.IsInSuit())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.GetLaserLengthForTargeter(ActorData, float)).MethodHandle;
			}
			range = this.m_suitedLaserInfo.range;
		}
		else
		{
			range = this.m_scampLaserInfo.range;
		}
		return range;
	}

	public float GetLaserWidthForTargeter(ActorData caster, float baseWidth)
	{
		return (!this.IsInSuit()) ? this.m_scampLaserInfo.width : this.m_suitedLaserInfo.width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public bool IsInSuit()
	{
		bool result;
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.IsInSuit()).MethodHandle;
			}
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int CalcTotalDamage(int numHits)
	{
		int num = 0;
		int num2;
		if (this.IsInSuit())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.CalcTotalDamage(int)).MethodHandle;
			}
			num2 = this.m_suitedBaseDamage;
		}
		else
		{
			num2 = this.m_scampBaseDamage;
		}
		int num3 = num2;
		int num4 = (!this.IsInSuit()) ? this.m_scampSubseqDamage : this.m_suitedSubseqDamage;
		if (numHits > 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num += num3;
			int num5 = numHits - 1;
			if (num5 > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num4 > 0)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num += num5 * num4;
				}
			}
		}
		if (this.m_maxDamage > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num = Mathf.Min(this.m_maxDamage, num);
		}
		return num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_suitedBaseDamage);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampSlamDown.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			int tooltipSubjectCountOnActor = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
			int damage = this.CalcTotalDamage(tooltipSubjectCountOnActor);
			results.m_damage = damage;
			return true;
		}
		return false;
	}
}
