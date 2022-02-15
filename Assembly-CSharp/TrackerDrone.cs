using System.Collections.Generic;
using UnityEngine;

public class TrackerDrone : Ability
{
	protected TrackerDroneInfoComponent m_droneInfoComp;
	protected TrackerDroneTrackerComponent m_droneTracker;
	protected ActorAdditionalVisionProviders m_visionProvider;
	protected bool m_droneEffectHandled;

	private AbilityMod_TrackerDrone m_abilityMod;

	protected virtual bool UseAltMovement()
	{
		return false;
	}

	public AbilityMod_TrackerDrone GetDroneMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hawk Drone";
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		m_visionProvider = GetComponent<ActorAdditionalVisionProviders>();
		if (m_visionProvider == null)
		{
			Debug.LogError("No additional vision provider component");
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (m_droneInfoComp == null)
		{
			m_droneInfoComp = GetComponent<TrackerDroneInfoComponent>();
		}
		if (m_droneInfoComp == null)
		{
			Debug.LogError("No Drone Info component");
		}
		if (m_droneTracker == null)
		{
			m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		}
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		bool hitUntrackedTargets = m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect || m_droneInfoComp.GetDamageOnUntracked(true) > 0;
		Targeter = new AbilityUtil_Targeter_TrackerDrone(
			this, m_droneTracker, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterEndRadius,
			m_droneInfoComp.m_travelTargeterLineRadius, -1, false, m_droneInfoComp.m_targetingIgnoreLos,
			m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerDrone abilityMod_TrackerDrone = modAsBase as AbilityMod_TrackerDrone;
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			tokens.Add(new TooltipTokenInt("DamageOnTracked", "damage on Tracked targets", abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_trackedHitDamageMod.GetModifiedValue(component.m_droneHitDamageAmount)
				: component.m_droneHitDamageAmount));
			tokens.Add(new TooltipTokenInt("DamageOnUntracked", "damage on Untracked targets", abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_untrackedHitDamageMod.GetModifiedValue(component.m_untrackedDroneHitDamageAmount)
				: component.m_untrackedDroneHitDamageAmount));
			AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_trackedHitEffectOverride.GetModifiedValue(component.m_droneHitEffect)
				: component.m_droneHitEffect, "EffectOnTracked");
			AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_untrackedHitEffectOverride.GetModifiedValue(component.m_untrackedDroneHitEffect)
				: component.m_untrackedDroneHitEffect, "EffectOnUntracked");
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.m_droneHitDamageAmount);
			m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.m_untrackedDroneHitDamageAmount);
			m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.GetDamageOnTracked(true));
			m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.GetDamageOnUntracked(true));
			m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = new List<int>();
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_droneHitDamageAmount);
			list.Add(component.m_untrackedDroneHitDamageAmount);
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
			|| !targetSquare.IsValidForGameplay()
			|| caster.GetCurrentBoardSquare() == null)
		{
			return false;
		}
		float maxMoveDist = m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
		float maxDistFromCaster = m_droneInfoComp.GetTargeterMaxRangeFromCaster(false) * Board.Get().squareSize;
		Vector3 startPos = caster.GetFreePos();
		if (m_droneTracker.DroneIsActive())
		{
			BoardSquare dronePos = Board.Get().GetSquareFromIndex(m_droneTracker.BoardX(), m_droneTracker.BoardY());
			if (dronePos != null)
			{
				if (targetSquare == dronePos)
				{
					return false;
				}
				startPos = dronePos.ToVector3();
			}
		}
		Vector3 casterPos = caster.GetCurrentBoardSquare().ToVector3();
		return (maxMoveDist <= 0f || Vector3.Distance(targetSquare.ToVector3(), startPos) <= maxMoveDist)
			&& (maxDistFromCaster <= 0f || Vector3.Distance(targetSquare.ToVector3(), casterPos) <= maxDistFromCaster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerDrone))
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerDrone);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
