using System.Collections.Generic;
using UnityEngine;

public class ScampHoloBlobs : Ability
{
	[Separator("Targeting")]
	public ConeTargetingInfo m_coneTargetInfo;
	[Separator("On Direct Hit")]
	public int m_directHitDamage;
	public StandardEffectInfo m_directHitEnemyEffect;
	[Separator("Damage to Shield Conversion")]
	public float m_damageToShieldMult = 0.25f;
	public int m_maxShields;
	public int m_shieldDuration = 1;
	[Separator("Effect Data for Holo Blob effect, mostly to contain persistent visual vfx")]
	public StandardActorEffectData m_holoBlobEffectData;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampHoloBlobs";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		Targeter = new AbilityUtil_Targeter_DirectionCone(
			this,
			m_coneTargetInfo.m_widthAngleDeg,
			m_coneTargetInfo.m_radiusInSquares,
			m_coneTargetInfo.m_backwardsOffset,
			m_coneTargetInfo.m_penetrateLos,
			true);
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
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}
}
