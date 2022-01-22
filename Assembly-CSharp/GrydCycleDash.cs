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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cycle Dash";
		}
		if (m_targetData != null)
		{
			if (m_targetData.Length >= GetExpectedNumberOfTargeters())
			{
				goto IL_0074;
			}
		}
		Debug.LogError("GrydCycleDash has wrong number of Target Data entries - to match Num Legs it should be " + GetExpectedNumberOfTargeters());
		goto IL_0074;
		IL_0074:
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BombingRun abilityUtil_Targeter_BombingRun = new AbilityUtil_Targeter_BombingRun(this, AbilityAreaShape.SingleSquare, Mathf.RoundToInt(m_totalRange));
			abilityUtil_Targeter_BombingRun.SetShowArcToShape(false);
			abilityUtil_Targeter_BombingRun.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_BombingRun);
		}
		while (true)
		{
			return;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_numLegs;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_groundTrail.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_groundTrail.m_groundEffectData.AddTooltipTokens(tokens, "GroundTrail");
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		GridPos gridPos = (targetIndex != 0) ? currentTargets[targetIndex - 1].GridPos : caster.GetGridPos();
		if (m_lockToCardinalDirs)
		{
			if (!CardinallyAligned(gridPos, target.GridPos))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsValidForGameplay())
			{
				BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, Board.Get().GetSquare(gridPos), false);
				if (boardSquarePathInfo == null)
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
				boardSquarePathInfo.CalcAndSetMoveCostToEnd();
				float num = boardSquarePathInfo.FindMoveCostToEnd();
				return num <= m_legRange * Board.Get().squareSize;
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
		int result;
		if (!start.CoordsEqual(end))
		{
			if (start.x != end.x)
			{
				result = ((start.y == end.y) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
