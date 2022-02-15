using System.Collections.Generic;
using UnityEngine;

public class TrackerHuntingCrossbow : Ability
{
	public int m_laserDamageAmount = 5;
	public LaserTargetingInfo m_laserInfo;
	[Header("-- Effect Data for <Tracked> effect")]
	public StandardActorEffectData m_huntedEffectData;

	private TrackerDroneTrackerComponent m_droneTracker;
	private AbilityMod_TrackerHuntingCrossbow m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hunting Crossbow";
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserLength(), m_laserInfo.penetrateLos, GetLaserMaxTargets());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerHuntingCrossbow abilityMod_TrackerHuntingCrossbow = modAsBase as AbilityMod_TrackerHuntingCrossbow;
		tokens.Add(new TooltipTokenInt("Damage", "damage amount", abilityMod_TrackerHuntingCrossbow != null
			? abilityMod_TrackerHuntingCrossbow.m_damageOnUntrackedMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount));
		GetHuntedEffect().AddTooltipTokens(tokens, "TrackedEffect");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		GetHuntedEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null
			|| !tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary)
			|| m_droneTracker == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> values = new Dictionary<AbilityTooltipSymbol, int>();
		int damage = m_droneTracker.IsTrackingActor(targetActor.ActorIndex)
			? GetDamageOnTracked()
			: GetDamageOnUntracked();
		if (m_abilityMod != null)
		{
			ActorData actorData = ActorData;
			bool isBrushBonusActive = m_abilityMod.m_requireFunctioningBrush
				? actorData.IsInBrush()
				: actorData.GetCurrentBoardSquare().IsInBrush();
			if (isBrushBonusActive)
			{
				damage += GetExtraDamageWhileInBrush();
			}
			if (GetDamageChangeAfterFirstHit() != 0)
			{
				foreach (AbilityUtil_Targeter_Laser.HitActorContext hitActorContext in (Targeter as AbilityUtil_Targeter_Laser).GetHitActorContext())
				{
					if (hitActorContext.actor == targetActor && hitActorContext.hitOrderIndex != 0)
					{
						damage += GetDamageChangeAfterFirstHit();
					}
				}
			}
		}
		values[AbilityTooltipSymbol.Damage] = Mathf.Max(0, damage);
		return values;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerHuntingCrossbow))
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerHuntingCrossbow);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetDamageOnUntracked()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageOnUntrackedMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	private int GetDamageOnTracked()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageOnTrackedMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	private int GetDamageChangeAfterFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangeOnSubsequentTargets
			: 0;
	}

	private int GetExtraDamageWhileInBrush()
	{
		return m_abilityMod != null
			? Mathf.Max(0, m_abilityMod.m_extraDamageWhenInBrush)
			: 0;
	}

	private int GetLaserMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserInfo.maxTargets)
			: m_laserInfo.maxTargets;
	}

	private float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserInfo.width)
			: m_laserInfo.width;
	}

	private float GetLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserInfo.range)
			: m_laserInfo.range;
	}

	public StandardActorEffectData GetHuntedEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_huntedEffectDataOverride.GetModifiedValue(m_huntedEffectData)
			: m_huntedEffectData;
	}
}
