using System.Collections.Generic;
using UnityEngine;

public class TrackerFlewTheCoop : Ability
{
	public AbilityAreaShape m_hookshotShape = AbilityAreaShape.Three_x_Three_NoCorners;

	public bool m_includeDroneSquare = true;

	private TrackerDroneTrackerComponent m_droneTracker;

	private AbilityMod_TrackerFlewTheCoop m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flew The Coop";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		bool flag = false;
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect)
		{
			flag = true;
		}
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_additionalEffectOnSelf != null)
			{
				if (m_abilityMod.m_additionalEffectOnSelf.m_applyEffect)
				{
					flag = true;
				}
			}
		}
		int num;
		if (flag)
		{
			num = 2;
		}
		else
		{
			num = 0;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, affectsCaster);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_additionalEffectOnSelf != null)
			{
				m_abilityMod.m_additionalEffectOnSelf.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
			}
		}
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_droneTracker == null)
		{
			return false;
		}
		bool flag = m_droneTracker.DroneIsActive();
		bool flag2 = false;
		if (!caster.IsDead())
		{
			BoardSquare boardSquare = Board.Get().GetSquare(m_droneTracker.BoardX(), m_droneTracker.BoardY());
			if (boardSquare != null)
			{
				float rangeInSquares = GetRangeInSquares(0);
				if (rangeInSquares != 0f)
				{
					if (!(VectorUtils.HorizontalPlaneDistInSquares(caster.GetTravelBoardSquareWorldPosition(), boardSquare.ToVector3()) <= rangeInSquares))
					{
						goto IL_00ac;
					}
				}
				flag2 = true;
			}
		}
		goto IL_00ac;
		IL_00ac:
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsValidForGameplay())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				if (m_droneTracker != null)
				{
					BoardSquare boardSquare = Board.Get().GetSquare(m_droneTracker.BoardX(), m_droneTracker.BoardY());
					if (boardSquare != null)
					{
						if (!m_includeDroneSquare)
						{
							if (boardSquare == boardSquareSafe)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										return false;
									}
								}
							}
						}
						List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(GetLandingShape(), boardSquare.ToVector3(), boardSquare, true, caster);
						if (squaresInShape.Contains(boardSquareSafe))
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_TrackerFlewTheCoop))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerFlewTheCoop);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private AbilityAreaShape GetLandingShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_hookshotShape;
		}
		else
		{
			result = m_abilityMod.m_landingShapeMod.GetModifiedValue(m_hookshotShape);
		}
		return result;
	}

	public int GetModdedExtraDroneDamageDuration()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_extraDroneDamageDuration.GetModifiedValue(0) : 0;
	}

	public int GetModdedExtraDroneDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_extraDroneDamage.GetModifiedValue(0);
		}
		return result;
	}

	public int GetModdedExtraDroneUntrackedDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_extraDroneUntrackedDamage.GetModifiedValue(0);
		}
		return result;
	}
}
