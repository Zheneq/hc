using System;
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
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Hawk Drone";
		}
		this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		if (this.m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		this.m_visionProvider = base.GetComponent<ActorAdditionalVisionProviders>();
		if (this.m_visionProvider == null)
		{
			Debug.LogError("No additional vision provider component");
		}
		this.Setup();
		base.ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (this.m_droneInfoComp == null)
		{
			this.m_droneInfoComp = base.GetComponent<TrackerDroneInfoComponent>();
		}
		if (this.m_droneInfoComp == null)
		{
			Debug.LogError("No Drone Info component");
		}
		if (this.m_droneTracker == null)
		{
			this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		}
		if (this.m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		bool hitUntrackedTargets = this.m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect || this.m_droneInfoComp.GetDamageOnUntracked(true) > 0;
		base.Targeter = new AbilityUtil_Targeter_TrackerDrone(this, this.m_droneTracker, this.m_droneInfoComp.m_travelTargeterEndRadius, this.m_droneInfoComp.m_travelTargeterEndRadius, this.m_droneInfoComp.m_travelTargeterLineRadius, -1, false, this.m_droneInfoComp.m_targetingIgnoreLos, this.m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerDrone abilityMod_TrackerDrone = modAsBase as AbilityMod_TrackerDrone;
		TrackerDroneInfoComponent component = base.GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			int val = (!abilityMod_TrackerDrone) ? component.m_droneHitDamageAmount : abilityMod_TrackerDrone.m_trackedHitDamageMod.GetModifiedValue(component.m_droneHitDamageAmount);
			int num;
			if (abilityMod_TrackerDrone)
			{
				num = abilityMod_TrackerDrone.m_untrackedHitDamageMod.GetModifiedValue(component.m_untrackedDroneHitDamageAmount);
			}
			else
			{
				num = component.m_untrackedDroneHitDamageAmount;
			}
			int val2 = num;
			StandardEffectInfo standardEffectInfo;
			if (abilityMod_TrackerDrone)
			{
				standardEffectInfo = abilityMod_TrackerDrone.m_trackedHitEffectOverride.GetModifiedValue(component.m_droneHitEffect);
			}
			else
			{
				standardEffectInfo = component.m_droneHitEffect;
			}
			StandardEffectInfo effectInfo = standardEffectInfo;
			StandardEffectInfo standardEffectInfo2;
			if (abilityMod_TrackerDrone)
			{
				standardEffectInfo2 = abilityMod_TrackerDrone.m_untrackedHitEffectOverride.GetModifiedValue(component.m_untrackedDroneHitEffect);
			}
			else
			{
				standardEffectInfo2 = component.m_untrackedDroneHitEffect;
			}
			StandardEffectInfo effectInfo2 = standardEffectInfo2;
			tokens.Add(new TooltipTokenInt("DamageOnTracked", "damage on Tracked targets", val));
			tokens.Add(new TooltipTokenInt("DamageOnUntracked", "damage on Untracked targets", val2));
			AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnTracked", null, true);
			AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnUntracked", null, true);
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_droneInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_droneInfoComp.m_droneHitDamageAmount);
			this.m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_droneInfoComp.m_untrackedDroneHitDamageAmount);
			this.m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_droneInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_droneInfoComp.GetDamageOnTracked(true));
			this.m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_droneInfoComp.GetDamageOnUntracked(true));
			this.m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = new List<int>();
		TrackerDroneInfoComponent component = base.GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_droneHitDamageAmount);
			list.Add(component.m_untrackedDroneHitDamageAmount);
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (caster.GetCurrentBoardSquare() != null)
				{
					float num = this.m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
					float num2 = this.m_droneInfoComp.GetTargeterMaxRangeFromCaster(false) * Board.Get().squareSize;
					Vector3 b = caster.GetTravelBoardSquareWorldPosition();
					if (this.m_droneTracker.DroneIsActive())
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
						if (boardSquare != null)
						{
							if (boardSquareSafe == boardSquare)
							{
								return false;
							}
							b = boardSquare.ToVector3();
						}
					}
					if (num > 0f)
					{
						if (Vector3.Distance(boardSquareSafe.ToVector3(), b) > num)
						{
							return false;
						}
					}
					bool result;
					if (num2 > 0f)
					{
						result = (Vector3.Distance(boardSquareSafe.ToVector3(), caster.GetCurrentBoardSquare().ToVector3()) <= num2);
					}
					else
					{
						result = true;
					}
					return result;
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerDrone))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_TrackerDrone);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
