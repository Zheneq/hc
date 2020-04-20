using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Flew The Coop";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		if (this.m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		bool flag = false;
		StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
		if (moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect)
		{
			flag = true;
		}
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_additionalEffectOnSelf != null)
			{
				if (this.m_abilityMod.m_additionalEffectOnSelf.m_applyEffect)
				{
					flag = true;
				}
			}
		}
		AbilityUtil_Targeter.AffectsActor affectsActor;
		if (flag)
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Never;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_additionalEffectOnSelf != null)
			{
				this.m_abilityMod.m_additionalEffectOnSelf.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
			}
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_droneTracker == null)
		{
			return false;
		}
		bool flag = this.m_droneTracker.DroneIsActive();
		bool flag2 = false;
		if (!caster.IsDead())
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
			if (boardSquare != null)
			{
				float rangeInSquares = this.GetRangeInSquares(0);
				if (rangeInSquares != 0f)
				{
					if (VectorUtils.HorizontalPlaneDistInSquares(caster.GetTravelBoardSquareWorldPosition(), boardSquare.ToVector3()) > rangeInSquares)
					{
						goto IL_AC;
					}
				}
				flag2 = true;
			}
		}
		IL_AC:
		bool result;
		if (flag)
		{
			result = flag2;
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				if (this.m_droneTracker != null)
				{
					BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
					if (boardSquare != null)
					{
						if (!this.m_includeDroneSquare)
						{
							if (boardSquare == boardSquareSafe)
							{
								return false;
							}
						}
						List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(this.GetLandingShape(), boardSquare.ToVector3(), boardSquare, true, caster);
						if (squaresInShape.Contains(boardSquareSafe))
						{
							return true;
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
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerFlewTheCoop))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_TrackerFlewTheCoop);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private AbilityAreaShape GetLandingShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod == null)
		{
			result = this.m_hookshotShape;
		}
		else
		{
			result = this.m_abilityMod.m_landingShapeMod.GetModifiedValue(this.m_hookshotShape);
		}
		return result;
	}

	public int GetModdedExtraDroneDamageDuration()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_extraDroneDamageDuration.GetModifiedValue(0) : 0;
	}

	public int GetModdedExtraDroneDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraDroneDamage.GetModifiedValue(0);
		}
		return result;
	}

	public int GetModdedExtraDroneUntrackedDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraDroneUntrackedDamage.GetModifiedValue(0);
		}
		return result;
	}
}
