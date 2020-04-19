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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.Start()).MethodHandle;
			}
			Debug.LogError("No drone tracker component");
		}
		this.m_visionProvider = base.GetComponent<ActorAdditionalVisionProviders>();
		if (this.m_visionProvider == null)
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
			Debug.LogError("No additional vision provider component");
		}
		this.Setup();
		base.ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (this.m_droneInfoComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.Setup()).MethodHandle;
			}
			this.m_droneInfoComp = base.GetComponent<TrackerDroneInfoComponent>();
		}
		if (this.m_droneInfoComp == null)
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
			Debug.LogError("No Drone Info component");
		}
		if (this.m_droneTracker == null)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
				}
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_droneInfoComp.GetDamageOnTracked(true));
			this.m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_droneInfoComp.GetDamageOnUntracked(true));
			this.m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	public override List<int> \u001D()
	{
		List<int> list = new List<int>();
		TrackerDroneInfoComponent component = base.GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.\u001D()).MethodHandle;
			}
			list.Add(component.m_droneHitDamageAmount);
			list.Add(component.m_untrackedDroneHitDamageAmount);
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
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
				if (caster.\u0012() != null)
				{
					float num = this.m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.\u000E().squareSize;
					float num2 = this.m_droneInfoComp.GetTargeterMaxRangeFromCaster(false) * Board.\u000E().squareSize;
					Vector3 b = caster.\u0016();
					if (this.m_droneTracker.DroneIsActive())
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
						BoardSquare boardSquare2 = Board.\u000E().\u0016(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
						if (boardSquare2 != null)
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
							if (boardSquare == boardSquare2)
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
								return false;
							}
							b = boardSquare2.ToVector3();
						}
					}
					if (num > 0f)
					{
						if (Vector3.Distance(boardSquare.ToVector3(), b) > num)
						{
							return false;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					bool result;
					if (num2 > 0f)
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
						result = (Vector3.Distance(boardSquare.ToVector3(), caster.\u0012().ToVector3()) <= num2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerDrone.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
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
