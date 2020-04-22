using System.Collections.Generic;
using UnityEngine;

public class ScampHoloBlobs : Ability
{
	[Separator("Targeting", true)]
	public ConeTargetingInfo m_coneTargetInfo;

	[Separator("On Direct Hit", true)]
	public int m_directHitDamage;

	public StandardEffectInfo m_directHitEnemyEffect;

	[Separator("Damage to Shield Conversion", true)]
	public float m_damageToShieldMult = 0.25f;

	public int m_maxShields;

	public int m_shieldDuration = 1;

	[Separator("Effect Data for Holo Blob effect, mostly to contain persistent visual vfx", true)]
	public StandardActorEffectData m_holoBlobEffectData;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "ScampHoloBlobs";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		ConeTargetingInfo coneTargetInfo = m_coneTargetInfo;
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, coneTargetInfo.m_widthAngleDeg, coneTargetInfo.m_radiusInSquares, coneTargetInfo.m_backwardsOffset, coneTargetInfo.m_penetrateLos, true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public float GetDamageToShieldMult()
	{
		return m_damageToShieldMult;
	}

	public int GetMaxShields()
	{
		return m_maxShields;
	}

	public int GetShieldDuration()
	{
		return m_shieldDuration;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_directHitDamage);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		int result;
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (m_syncComp.m_suitWasActiveOnTurnStart ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
