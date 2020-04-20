using System;
using System.Collections.Generic;
using UnityEngine;

public class GrydCycleDash : Ability
{
	[Header("-- Targeting")]
	public bool m_lockToCardinalDirs = true;

	public float m_totalRange = 10f;

	public float m_legRange = 5f;

	public int m_numLegs = 2;

	[Header("-- Ground Trail")]
	public StandardGroundEffectInfo m_groundTrail;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Cycle Dash";
		}
		if (this.m_targetData != null)
		{
			if (this.m_targetData.Length >= this.GetExpectedNumberOfTargeters())
			{
				goto IL_74;
			}
		}
		Debug.LogError("GrydCycleDash has wrong number of Target Data entries - to match Num Legs it should be " + this.GetExpectedNumberOfTargeters());
		IL_74:
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BombingRun abilityUtil_Targeter_BombingRun = new AbilityUtil_Targeter_BombingRun(this, AbilityAreaShape.SingleSquare, Mathf.RoundToInt(this.m_totalRange));
			abilityUtil_Targeter_BombingRun.SetShowArcToShape(false);
			abilityUtil_Targeter_BombingRun.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_BombingRun);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return this.m_numLegs;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_groundTrail.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.m_groundTrail.m_groundEffectData.AddTooltipTokens(tokens, "GroundTrail", false, null);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		GridPos gridPos;
		if (targetIndex == 0)
		{
			gridPos = caster.GetGridPosWithIncrementedHeight();
		}
		else
		{
			gridPos = currentTargets[targetIndex - 1].GridPos;
		}
		if (this.m_lockToCardinalDirs)
		{
			if (!this.CardinallyAligned(gridPos, target.GridPos))
			{
				return false;
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (!boardSquareSafe.IsBaselineHeight())
			{
			}
			else
			{
				BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, Board.Get().GetBoardSquareSafe(gridPos), false);
				if (boardSquarePathInfo == null)
				{
					return false;
				}
				boardSquarePathInfo.CalcAndSetMoveCostToEnd();
				float num = boardSquarePathInfo.FindMoveCostToEnd();
				return num <= this.m_legRange * Board.Get().squareSize;
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private bool CardinallyAligned(GridPos start, GridPos end)
	{
		bool result;
		if (!start.CoordsEqual(end))
		{
			if (start.x != end.x)
			{
				result = (start.y == end.y);
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}
}
