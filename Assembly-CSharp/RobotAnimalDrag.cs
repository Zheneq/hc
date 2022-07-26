using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalDrag : Ability
{
	public float m_width = 1f;
	public float m_distance = 3f;
	public int m_maxTargets = 1;
	public bool m_penetrateLineOfSight;
	public int m_damage;
	public StandardEffectInfo m_casterEffect;
	public StandardEffectInfo m_targetEffect;

	private AbilityMod_RobotAnimalDrag m_abilityMod;
	private StandardEffectInfo m_cachedCasterEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Death Snuggle";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Laser(
			this,
			GetLaserWidth(),
			GetLaserDistance(),
			m_penetrateLineOfSight,
			m_maxTargets,
			false,
			GetCasterEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserDistance();
	}

	private void SetCachedFields()
	{
		m_cachedCasterEffect = m_abilityMod != null
			? m_abilityMod.m_casterEffectMod.GetModifiedValue(m_casterEffect)
			: m_casterEffect;
	}

	public StandardEffectInfo GetCasterEffect()
	{
		return m_cachedCasterEffect ?? m_casterEffect;
	}

	private float GetLaserDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_distanceMod.GetModifiedValue(m_distance)
			: m_distance;
	}

	private float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public bool HasEffectOnNextTurnStart()
	{
		if (m_abilityMod == null)
		{
			return false;
		}
		if (m_abilityMod.m_enemyEffectOnNextTurnStart.m_applyEffect)
		{
			return true;
		}
		return m_abilityMod.m_powerUpsToSpawn != null && m_abilityMod.m_powerUpsToSpawn.Count > 0;
	}

	public StandardEffectInfo EffectInfoOnNextTurnStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyEffectOnNextTurnStart
			: new StandardEffectInfo();
	}

	public List<PowerUp> GetModdedPowerUpsToSpawn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_powerUpsToSpawn
			: null;
	}

	public AbilityAreaShape GetModdedPowerUpsToSpawnShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_powerUpsSpawnShape 
			: AbilityAreaShape.SingleSquare;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamage());
		GetCasterEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		m_targetEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalDrag abilityMod_RobotAnimalDrag = modAsBase as AbilityMod_RobotAnimalDrag;
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RobotAnimalDrag != null
			? abilityMod_RobotAnimalDrag.m_casterEffectMod.GetModifiedValue(m_casterEffect)
			: m_casterEffect, "CasterEffect", m_casterEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetEffect, "TargetEffect", null, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RobotAnimalDrag))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_RobotAnimalDrag;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
