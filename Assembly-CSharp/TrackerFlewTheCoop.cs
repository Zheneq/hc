﻿using System;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.SetupTargeter()).MethodHandle;
			}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_abilityMod.m_additionalEffectOnSelf != null)
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
				if (this.m_abilityMod.m_additionalEffectOnSelf.m_applyEffect)
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
					flag = true;
				}
			}
		}
		AbilityUtil_Targeter.AffectsActor affectsActor;
		if (flag)
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			if (this.m_abilityMod.m_additionalEffectOnSelf != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
			if (boardSquare != null)
			{
				float rangeInSquares = this.GetRangeInSquares(0);
				if (rangeInSquares != 0f)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				if (this.m_droneTracker != null)
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
					BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
					if (boardSquare != null)
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
						if (!this.m_includeDroneSquare)
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
							if (boardSquare == boardSquareSafe)
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
						}
						List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(this.GetLandingShape(), boardSquare.ToVector3(), boardSquare, true, caster);
						if (squaresInShape.Contains(boardSquareSafe))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.GetLandingShape()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.GetModdedExtraDroneDamage()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerFlewTheCoop.GetModdedExtraDroneUntrackedDamage()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraDroneUntrackedDamage.GetModifiedValue(0);
		}
		return result;
	}
}
