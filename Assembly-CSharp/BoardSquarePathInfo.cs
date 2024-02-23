using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BoardSquarePathInfo : IComparable
{
	public enum ConnectionType
	{
		Run,
		Knockback,
		Charge,
		Vault,
		Flight,
		Teleport,
		NumTypes
	}

	public enum ChargeCycleType
	{
		Movement,
		Recovery,
		None
	}

	public enum ChargeEndType
	{
		Pivot,
		Impact,
		Miss,
		Recovery,
		None
	}

	public BoardSquare square;
	public float moveCost;
	public float heuristicCost;
	public BoardSquarePathInfo prev;
	public BoardSquarePathInfo next;
	public bool m_unskippable;
	public bool m_reverse;
	public bool m_visibleToEnemies;
	public bool m_updateLastKnownPos;
	public bool m_moverDiesHere;
	public bool m_moverHasGameplayHitHere;
	public bool m_moverClashesHere;
	public bool m_moverBumpedFromClash;
	public int m_expectedBackupNum;
	public ConnectionType connectionType;
	public ChargeCycleType chargeCycleType;
	public ChargeEndType chargeEndType = ChargeEndType.None;
	public float segmentMovementSpeed;
	public float segmentMovementDuration;

	public float F_cost
	{
		get
		{
			int extra = Mathf.Max(m_expectedBackupNum - 1, 0);
			if (extra > 0)
			{
				float extraCost = 1.5f * extra + 0.1f;
				return moveCost + heuristicCost + extraCost;
			}
			return moveCost + heuristicCost;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}

		BoardSquarePathInfo boardSquarePathInfo = obj as BoardSquarePathInfo;
		if (boardSquarePathInfo != null)
		{
			return FindMoveCostToEnd().CompareTo(boardSquarePathInfo.FindMoveCostToEnd());
		}
		throw new ArgumentException("Object is not a BoardSquarePathInfo");
	}

	public void ResetValuesToDefault()
	{
		square = null;
		moveCost = 0f;
		heuristicCost = 0f;
		prev = null;
		next = null;
		m_unskippable = false;
		m_reverse = false;
		m_visibleToEnemies = false;
		m_updateLastKnownPos = false;
		m_moverDiesHere = false;
		m_moverHasGameplayHitHere = false;
		m_moverClashesHere = false;
		m_moverBumpedFromClash = false;
		m_expectedBackupNum = 0;
	}

	public bool IsSamePathAs(BoardSquarePathInfo other)
	{
		if (other == null)
		{
			return false;
		}
		bool flag = true;
		flag &= other.square == square;
		flag &= other.moveCost == moveCost;
		flag &= other.heuristicCost == heuristicCost;
		flag &= other.m_unskippable == m_unskippable;
		flag &= other.m_moverClashesHere == m_moverClashesHere; // bugfix: was = instead of ==
		flag &= other.m_moverBumpedFromClash == m_moverBumpedFromClash; // bugfix: was = instead of ==
		flag &= other.m_reverse == m_reverse;
		flag &= other.chargeEndType == chargeEndType;
		flag &= other.chargeCycleType == chargeCycleType;
		flag &= other.segmentMovementSpeed == segmentMovementSpeed;
		flag &= other.segmentMovementDuration == segmentMovementDuration;
		if (!flag)
		{
			return false;
		}
		if (next != null && other.next != null)
		{
			return next.IsSamePathAs(other.next);
		}
		return next == null && other.next == null;
	}

	public BoardSquarePathInfo Clone(BoardSquarePathInfo previous)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo
		{
			square = square,
			moveCost = moveCost,
			heuristicCost = heuristicCost,
			m_unskippable = m_unskippable,
			m_reverse = m_reverse,
			chargeEndType = chargeEndType,
			chargeCycleType = chargeCycleType,
			segmentMovementSpeed = segmentMovementSpeed,
			segmentMovementDuration = segmentMovementDuration,
			connectionType = connectionType,
			m_moverDiesHere = m_moverDiesHere,
			m_moverHasGameplayHitHere = m_moverHasGameplayHitHere,
			m_updateLastKnownPos = m_updateLastKnownPos,
			m_visibleToEnemies = m_visibleToEnemies,
			m_moverClashesHere = m_moverClashesHere,
			m_moverBumpedFromClash = m_moverBumpedFromClash
		};
		if (previous != null)
		{
			boardSquarePathInfo.prev = previous;
		}
		if (next != null)
		{
			boardSquarePathInfo.next = next.Clone(boardSquarePathInfo);
		}
		return boardSquarePathInfo;
	}

	public void CalcAndSetMoveCostToEnd()
	{
		float num = moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (Board.Get().GetSquaresAreDiagonallyAdjacent(boardSquarePathInfo.square, boardSquarePathInfo.prev.square))
			{
				num += 1.5f;
			}
			else if (Board.Get().GetSquaresAreCardinallyAdjacent(boardSquarePathInfo.square, boardSquarePathInfo.prev.square))
			{
				num += 1f;
			}
			else if (boardSquarePathInfo.square == boardSquarePathInfo.prev.square)
			{
				if (boardSquarePathInfo.next != null)
				{
					Log.Warning("Calculating move costs on a path, but it has the same square twice in a row.");
				}
			}
			else if (boardSquarePathInfo.connectionType == ConnectionType.Run ||
			         boardSquarePathInfo.connectionType == ConnectionType.Vault)
			{
				Log.Warning(
					"Calculating move costs on a path, but it has two non-adjacent consecutive squares.");
			}
			else
			{
				num += boardSquarePathInfo.square.HorizontalDistanceOnBoardTo(boardSquarePathInfo.prev.square);
			}

			boardSquarePathInfo.moveCost = num;
		}
	}

	public float FindMoveCostToEnd()
	{
		float endCost = moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			endCost = boardSquarePathInfo.moveCost;
		}
		return endCost - moveCost;
	}

	public float FindMoveCostToOneBeforeEnd()
	{
		float oneBeforeEndCost = moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (boardSquarePathInfo.next == null)
			{
				break;
			}
			oneBeforeEndCost = boardSquarePathInfo.moveCost;
		}
		return oneBeforeEndCost - moveCost;
	}

	internal float FindDistanceToEnd()
	{
		float dist = 0f;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			dist += (boardSquarePathInfo.square.ToVector3() - square.ToVector3()).magnitude;
		}
		return dist;
	}

	public int GetNumSquaresToEnd(bool checkDuplicate = true)
	{
		int num = 1;
		for (BoardSquarePathInfo step = this; step.next != null; step = step.next)
		{
			if (!checkDuplicate || step.next.square != step.square)
			{
				num++;
			}
		}
		return num;
	}

	public BoardSquarePathInfo GetPathMidpoint()
	{
		float costToEnd = FindMoveCostToEnd();
		float halfCostToEnd = costToEnd / 2f;
		BoardSquarePathInfo result = this;
		float bestCostToEnd = costToEnd;
		for (BoardSquarePathInfo step = next; step != null; step = step.next)
		{
			float remainingCostToEnd = step.FindMoveCostToEnd();
			if (Math.Abs(remainingCostToEnd - halfCostToEnd) >= Math.Abs(bestCostToEnd - halfCostToEnd))
			{
				break;
			}
			result = step;
			bestCostToEnd = remainingCostToEnd;
		}
		return result;
	}

	public BoardSquarePathInfo GetPathEndpoint()
	{
		BoardSquarePathInfo step = this;
		while (step.next != null)
		{
			step = step.next;
		}
		return step;
	}

	public BoardSquarePathInfo GetPathStartPoint()
	{
		BoardSquarePathInfo step = this;
		while (step.prev != null)
		{
			step = step.prev;
		}
		return step;
	}

	public bool IsPathEndpoint()
	{
		return next == null;
	}

	public bool IsPathStartPoint()
	{
		return prev == null;
	}

	public BoardSquarePathInfo BackUpOnceFromEnd()
	{
		BoardSquarePathInfo pathEndpoint = GetPathEndpoint();
		if (pathEndpoint != null && pathEndpoint.prev != null)
		{
			BoardSquarePathInfo newLastStep = pathEndpoint.prev;
			newLastStep.next = null;
			return newLastStep;
		}
		else
		{
			return pathEndpoint;
		}
	}

	public List<GridPos> ToGridPosPath()
	{
		List<GridPos> list = new List<GridPos>();
		for (BoardSquarePathInfo step = this; step != null; step = step.next)
		{
			list.Add(step.square.GetGridPos());
		}
		return list;
	}

	public bool IsValidPathForMaxMovement(float maxMovement)
	{
		if (maxMovement <= 0f)
		{
			return next == null;
		}
		return maxMovement > FindMoveCostToOneBeforeEnd();
	}

	public static bool IsConnectionTypeConventional(ConnectionType connectionType)
	{
		return connectionType != ConnectionType.Flight
		       && connectionType != ConnectionType.Teleport
		       && connectionType != ConnectionType.Knockback
		       && connectionType != ConnectionType.Charge;
	}

	public string GetDebugPathStringToEnd(string prefix)
	{
		string text = prefix;
		int num = 0;
		BoardSquarePathInfo step = this;
		while (step != null && num < 100)
		{
			text += new StringBuilder().Append("\n").Append(step.square != null ? step.square.ToString() : null).Append(" | Connection Type = ").Append(step.connectionType).ToString();
			if (step == step.next)
			{
				break;
			}
			step = step.next;
			num++;
		}
		return text;
	}

	public string GetDebugPathStringToBeginning(string prefix)
	{
		string text = prefix;
		int num = 0;
		BoardSquarePathInfo step = this;
		while (step != null && num < 100)
		{
			text += new StringBuilder().Append("\n").Append(step.square != null ? step.square.ToString() : null).Append(" | Connection Type = ").Append(step.connectionType).ToString();
			if (step == step.prev)
			{
				break;
			}
			step = step.prev;
			num++;
		}
		return text;
	}

	public bool CheckPathConnectionForSelfReference()
	{
		return true;
	}

	public bool WillDieAtEnd()
	{
		BoardSquarePathInfo boardSquarePathInfo = GetPathEndpoint();
		return boardSquarePathInfo != null && boardSquarePathInfo.m_moverDiesHere;
	}

	public void CheckIsValidTriggeringPath(ActorData mover)
	{
		int numNull = 0;
		int numTotal = 0;
		int numAfterDeath = 0;
		bool isDead = false;
		
		for (BoardSquarePathInfo step = GetPathStartPoint(); step != null; step = step.next)
		{
			if (numTotal >= 100)
			{
				break;
			}
			if (square == null)
			{
				numNull++;
			}
			if (isDead)
			{
				numAfterDeath++;
			}
			if (step.m_moverDiesHere)
			{
				isDead = true;
			}
			numTotal++;
		}
		if (numNull <= 0 && numTotal < 100 && numAfterDeath <= 0)
		{
			return;
		}
		string textNull = new StringBuilder().Append(numNull).Append(" null squares").ToString();
		if (numNull != 0)
		{
			textNull = new StringBuilder().Append("INVALID SQUARES: ").Append(textNull).ToString();
		}
		string textTotal = new StringBuilder().Append(numTotal).Append(" total path nodes").ToString();
		if (numTotal >= 100)
		{
			textTotal = new StringBuilder().Append("INVALID LENGTH: ").Append(textTotal).ToString();
		}
		string textAfterDeath = new StringBuilder().Append(numAfterDeath).Append(" steps after death").ToString();
		if (numAfterDeath > 0)
		{
			textAfterDeath = new StringBuilder().Append("INVALID DEATH-MOVEMENT: ").Append(textAfterDeath).ToString();
		}
		Debug.LogError(new StringBuilder().Append("Invalid BoardSquarePathInfo for gameplay!  Path has:\n\t").Append(textTotal).Append("\n\t").Append(textNull).Append("\n\t").Append(textAfterDeath).Append("\nMover: ").Append(mover.DebugNameString()).ToString());
	}

	public bool IsNodePartOfMyFuturePath(BoardSquarePathInfo other, bool includePresent = true)
	{
		BoardSquarePathInfo step = includePresent ? this : next;
		while (step != null)
		{
			if (other == step)
			{
				return true;
			}
			step = step.next;
		}
		return false;
	}

	public void ResetClashingOfPath()
	{
		for (BoardSquarePathInfo boardSquarePathInfo = this; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (boardSquarePathInfo.m_moverClashesHere)
			{
				boardSquarePathInfo.m_moverClashesHere = false;
			}
			if (boardSquarePathInfo.m_moverBumpedFromClash)
			{
				boardSquarePathInfo.m_moverBumpedFromClash = false;
			}
		}
	}
}
