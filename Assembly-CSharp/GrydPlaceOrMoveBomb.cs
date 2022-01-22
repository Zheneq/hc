using System.Collections.Generic;
using UnityEngine;

public class GrydPlaceOrMoveBomb : Ability
{
	[Header("-- Targeting")]
	public bool m_lockToCardinalDirsForPlace = true;

	public bool m_lockToCardinalDirsForMove = true;

	public int m_placeRange = 4;

	public int m_moveRange = 4;

	public bool m_moveIsFreeAction = true;

	[Header("-- Enemy direct hit")]
	public bool m_explodeThisTurnOnDirectHit;

	public bool m_explodeImmediatelyOnMove;

	[Header("-- Bomb explosion")]
	public int m_bombDuration;

	public int m_damageAmount;

	public float m_explosionLaserRange;

	public float m_explosionLaserWidth;

	public int m_cooldownAfterExplode = 2;

	[Header("-- Anims")]
	public ActorModelData.ActionAnimationType m_moveBombAnimIndex = ActorModelData.ActionAnimationType.Ability2;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentBombSequencePrefab;

	public GameObject m_explodeBombSequencePrefab;

	public GameObject m_moveBombSequencePrefab;

	private Gryd_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Place/Move Bomb";
		}
		m_syncComp = GetComponent<Gryd_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_GrydBomb(this, m_moveRange);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (HasPlacedBomb())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		GridPos gridPosWithIncrementedHeight = caster.GetGridPosWithIncrementedHeight();
		if (m_lockToCardinalDirsForPlace && !CardinallyAligned(gridPosWithIncrementedHeight, target.GridPos))
		{
			return false;
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsValidForGameplay())
			{
				if (Mathf.Abs(gridPosWithIncrementedHeight.x - target.GridPos.x) > m_placeRange || Mathf.Abs(gridPosWithIncrementedHeight.y - target.GridPos.y) > m_placeRange)
				{
					return false;
				}
				return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		return numbers;
	}

	public override bool IsFreeAction()
	{
		if (HasPlacedBomb())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_moveIsFreeAction;
				}
			}
		}
		return base.IsFreeAction();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (HasPlacedBomb())
		{
			return m_moveBombAnimIndex;
		}
		return base.GetActionAnimType();
	}

	private bool CardinallyAligned(GridPos start, GridPos end)
	{
		int result;
		if (!start.CoordsEqual(end))
		{
			result = ((start.x == end.x || start.y == end.y) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private GridPos GetPushEndPos(Vector3 targetPos, out bool hitActor)
	{
		GridPos placedBomb = GetPlacedBomb();
		BoardSquare boardSquareSafe = Board.Get().GetSquare(placedBomb);
		Vector3 vector = boardSquareSafe.ToVector3();
		Vector3 vector2 = targetPos - vector;
		if (m_lockToCardinalDirsForMove)
		{
			vector2 = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
		}
		hitActor = false;
		bool collision;
		Vector3 collisionNormal;
		Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(base.ActorData, vector, vector + vector2 * m_moveRange * Board.Get().squareSize, out collision, out collisionNormal);
		BoardSquare boardSquare = null;
		if (collision)
		{
			boardSquare = Board.Get().GetSquare(abilityLineEndpoint);
		}
		GridPos result = placedBomb;
		if (vector2.x > 0.1f)
		{
			int num = placedBomb.x + 1;
			while (true)
			{
				if (num <= placedBomb.x + m_moveRange)
				{
					BoardSquare boardSquare2 = Board.Get().GetSquare(num, placedBomb.y);
					if (boardSquare2 == null)
					{
						break;
					}
					if (!boardSquare2.IsValidForGameplay())
					{
						break;
					}
					if (!boardSquareSafe.GetLOS(num, placedBomb.y))
					{
						break;
					}
					if (boardSquare != null)
					{
						if (boardSquare.x < num)
						{
							break;
						}
					}
					result = boardSquare2.GetGridPos();
					if (boardSquare2.OccupantActor != null)
					{
						if (boardSquare2.OccupantActor.GetTeam() != base.ActorData.GetTeam())
						{
							hitActor = true;
							break;
						}
					}
					num++;
					continue;
				}
				break;
			}
		}
		else if (vector2.x < -0.1f)
		{
			int num2 = placedBomb.x - 1;
			while (true)
			{
				if (num2 >= placedBomb.x - m_moveRange)
				{
					BoardSquare boardSquare3 = Board.Get().GetSquare(num2, placedBomb.y);
					if (boardSquare3 == null)
					{
						break;
					}
					if (!boardSquare3.IsValidForGameplay())
					{
						break;
					}
					if (!boardSquareSafe.GetLOS(num2, placedBomb.y))
					{
						break;
					}
					if (boardSquare != null)
					{
						if (boardSquare.x > num2)
						{
							break;
						}
					}
					result = boardSquare3.GetGridPos();
					if (boardSquare3.OccupantActor != null)
					{
						if (boardSquare3.OccupantActor.GetTeam() != base.ActorData.GetTeam())
						{
							hitActor = true;
							break;
						}
					}
					num2--;
					continue;
				}
				break;
			}
		}
		else if (vector2.z > 0.1f)
		{
			int num3 = placedBomb.y + 1;
			while (true)
			{
				if (num3 <= placedBomb.y + m_moveRange)
				{
					BoardSquare boardSquare4 = Board.Get().GetSquare(placedBomb.x, num3);
					if (boardSquare4 == null)
					{
						break;
					}
					if (!boardSquare4.IsValidForGameplay())
					{
						break;
					}
					if (!boardSquareSafe.GetLOS(placedBomb.x, num3))
					{
						break;
					}
					if (boardSquare != null)
					{
						if (boardSquare.y < num3)
						{
							break;
						}
					}
					result = boardSquare4.GetGridPos();
					if (boardSquare4.OccupantActor != null)
					{
						if (boardSquare4.OccupantActor.GetTeam() != base.ActorData.GetTeam())
						{
							hitActor = true;
							break;
						}
					}
					num3++;
					continue;
				}
				break;
			}
		}
		else if (vector2.z < -0.1f)
		{
			for (int num4 = placedBomb.y - 1; num4 >= placedBomb.y - m_moveRange; num4--)
			{
				BoardSquare boardSquare5 = Board.Get().GetSquare(placedBomb.x, num4);
				if (boardSquare5 == null || !boardSquare5.IsValidForGameplay() || !boardSquareSafe.GetLOS(placedBomb.x, num4))
				{
					break;
				}
				if (boardSquare != null && boardSquare.y > num4)
				{
					break;
				}
				result = boardSquare5.GetGridPos();
				if (boardSquare5.OccupantActor != null && boardSquare5.OccupantActor.GetTeam() != base.ActorData.GetTeam())
				{
					hitActor = true;
					break;
				}
			}
		}
		return result;
	}

	public bool HasPlacedBomb()
	{
		return GetPlacedBomb().x > 0 && GetPlacedBomb().y > 0;
	}

	public GridPos GetPlacedBomb()
	{
		GridPos result;
		if (m_syncComp != null)
		{
			result = m_syncComp.m_bombLocation;
		}
		else
		{
			result = GridPos.s_invalid;
		}
		return result;
	}
}
