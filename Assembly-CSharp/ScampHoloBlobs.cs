using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ScampHoloBlobs";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		ConeTargetingInfo coneTargetInfo = this.m_coneTargetInfo;
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, coneTargetInfo.m_widthAngleDeg, coneTargetInfo.m_radiusInSquares, coneTargetInfo.m_backwardsOffset, coneTargetInfo.m_penetrateLos, true, true, false, false, -1, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public float GetDamageToShieldMult()
	{
		return this.m_damageToShieldMult;
	}

	public int GetMaxShields()
	{
		return this.m_maxShields;
	}

	public int GetShieldDuration()
	{
		return this.m_shieldDuration;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_directHitDamage);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}
}
