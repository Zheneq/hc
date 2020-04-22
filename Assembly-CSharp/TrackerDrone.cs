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
			while (true)
			{
				switch (5)
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
			Debug.LogError("No drone tracker component");
		}
		m_visionProvider = GetComponent<ActorAdditionalVisionProviders>();
		if (m_visionProvider == null)
		{
			while (true)
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
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (m_droneInfoComp == null)
		{
			while (true)
			{
				switch (2)
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
			m_droneInfoComp = GetComponent<TrackerDroneInfoComponent>();
		}
		if (m_droneInfoComp == null)
		{
			while (true)
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
		if (m_droneTracker == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (7)
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
			while (true)
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
		if ((bool)abilityMod_TrackerDrone)
		{
			while (true)
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
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnTracked");
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnUntracked");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
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
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.GetDamageOnTracked(true));
			m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.GetDamageOnUntracked(true));
			m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	public override List<int> _001D()
	{
		List<int> list = new List<int>();
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			while (true)
			{
				switch (1)
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
			list.Add(component.m_droneHitDamageAmount);
			list.Add(component.m_untrackedDroneHitDamageAmount);
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		int result;
		if (boardSquareSafe != null)
		{
			while (true)
			{
				switch (5)
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
			if (boardSquareSafe.IsBaselineHeight())
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
				if (caster.GetCurrentBoardSquare() != null)
				{
					float num = m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
					float num2 = m_droneInfoComp.GetTargeterMaxRangeFromCaster(false) * Board.Get().squareSize;
					Vector3 b = caster.GetTravelBoardSquareWorldPosition();
					if (m_droneTracker.DroneIsActive())
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
						BoardSquare boardSquare = Board.Get().GetBoardSquare(m_droneTracker.BoardX(), m_droneTracker.BoardY());
						if (boardSquare != null)
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
							if (boardSquareSafe == boardSquare)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
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
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (!(num2 <= 0f))
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
