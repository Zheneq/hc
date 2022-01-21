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
		base.Targeter = new AbilityUtil_Targeter_TrackerDrone(this, m_droneTracker, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterLineRadius, -1, false, m_droneInfoComp.m_targetingIgnoreLos, m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerDrone abilityMod_TrackerDrone = modAsBase as AbilityMod_TrackerDrone;
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (!(component != null))
		{
			return;
		}
		int val = (!abilityMod_TrackerDrone) ? component.m_droneHitDamageAmount : abilityMod_TrackerDrone.m_trackedHitDamageMod.GetModifiedValue(component.m_droneHitDamageAmount);
		int num;
		if ((bool)abilityMod_TrackerDrone)
		{
			num = abilityMod_TrackerDrone.m_untrackedHitDamageMod.GetModifiedValue(component.m_untrackedDroneHitDamageAmount);
		}
		else
		{
			num = component.m_untrackedDroneHitDamageAmount;
		}
		int val2 = num;
		StandardEffectInfo standardEffectInfo;
		if ((bool)abilityMod_TrackerDrone)
		{
			standardEffectInfo = abilityMod_TrackerDrone.m_trackedHitEffectOverride.GetModifiedValue(component.m_droneHitEffect);
		}
		else
		{
			standardEffectInfo = component.m_droneHitEffect;
		}
		StandardEffectInfo effectInfo = standardEffectInfo;
		StandardEffectInfo standardEffectInfo2;
		if ((bool)abilityMod_TrackerDrone)
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
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnTracked");
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnUntracked");
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
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		int result;
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsValidForGameplay())
			{
				if (caster.GetCurrentBoardSquare() != null)
				{
					float num = m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
					float num2 = m_droneInfoComp.GetTargeterMaxRangeFromCaster(false) * Board.Get().squareSize;
					Vector3 b = caster.GetTravelBoardSquareWorldPosition();
					if (m_droneTracker.DroneIsActive())
					{
						BoardSquare boardSquare = Board.Get().GetSquare(m_droneTracker.BoardX(), m_droneTracker.BoardY());
						if (boardSquare != null)
						{
							if (boardSquareSafe == boardSquare)
							{
								while (true)
								{
									return false;
								}
							}
							b = boardSquare.ToVector3();
						}
					}
					if (!(num <= 0f))
					{
						if (!(Vector3.Distance(boardSquareSafe.ToVector3(), b) <= num))
						{
							result = 0;
							goto IL_0172;
						}
					}
					if (!(num2 <= 0f))
					{
						result = ((Vector3.Distance(boardSquareSafe.ToVector3(), caster.GetCurrentBoardSquare().ToVector3()) <= num2) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					goto IL_0172;
				}
			}
		}
		return false;
		IL_0172:
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_TrackerDrone))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerDrone);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
