using System;
using System.Collections.Generic;
using UnityEngine;

public class GrydPlaceBomb : Ability
{
	[Header("-- Targeting")]
	public bool m_lockToCardinalDirs = true;

	[Header("-- Enemy direct hit")]
	public bool m_explodeThisTurnOnDirectHit;

	[Header("-- Bomb explosion")]
	public int m_bombDuration;

	public int m_damageAmount;

	public float m_explosionLaserRange;

	public float m_explosionLaserWidth;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentBombSequencePrefab;

	public GameObject m_explodeBombSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Place Bomb";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		GridPos start;
		if (targetIndex == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceBomb.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			start = caster.GetGridPosWithIncrementedHeight();
		}
		else
		{
			start = currentTargets[targetIndex - 1].GridPos;
		}
		if (this.m_lockToCardinalDirs && !this.CardinallyAligned(start, target.GridPos))
		{
			return false;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
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
			if (boardSquareSafe.IsBaselineHeight())
			{
				return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
			}
		}
		return false;
	}

	private bool CardinallyAligned(GridPos start, GridPos end)
	{
		bool result;
		if (!start.CoordsEqual(end))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceBomb.CardinallyAligned(GridPos, GridPos)).MethodHandle;
			}
			result = (start.x == end.x || start.y == end.y);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
