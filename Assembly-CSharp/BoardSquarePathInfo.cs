using System;
using System.Collections.Generic;
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
			int num = Mathf.Max(m_expectedBackupNum - 1, 0);
			if (num > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						float num2 = 1.5f * (float)num + 0.1f;
						return moveCost + heuristicCost + num2;
					}
					}
				}
			}
			return moveCost + heuristicCost;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return 1;
				}
			}
		}
		BoardSquarePathInfo boardSquarePathInfo = obj as BoardSquarePathInfo;
		if (boardSquarePathInfo != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return FindMoveCostToEnd().CompareTo(boardSquarePathInfo.FindMoveCostToEnd());
				}
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		bool flag = true;
		flag &= (other.square == square);
		flag &= (other.moveCost == moveCost);
		flag &= (other.heuristicCost == heuristicCost);
		flag &= (other.m_unskippable == m_unskippable);
		flag &= (other.m_moverClashesHere = m_moverClashesHere);
		flag &= (other.m_moverBumpedFromClash = m_moverBumpedFromClash);
		flag &= (other.m_reverse == m_reverse);
		flag &= (other.chargeEndType == chargeEndType);
		flag &= (other.chargeCycleType == chargeCycleType);
		flag &= (other.segmentMovementSpeed == segmentMovementSpeed);
		flag &= (other.segmentMovementDuration == segmentMovementDuration);
		if (flag)
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
			if (next != null)
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
				if (other.next != null)
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
					flag = next.IsSamePathAs(other.next);
					goto IL_0168;
				}
			}
			if (next == null && other.next != null)
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
				flag = false;
			}
			else if (next != null)
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
				if (other.next == null)
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
					flag = false;
				}
			}
		}
		goto IL_0168;
		IL_0168:
		return flag;
	}

	public BoardSquarePathInfo Clone(BoardSquarePathInfo previous)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = square;
		boardSquarePathInfo.moveCost = moveCost;
		boardSquarePathInfo.heuristicCost = heuristicCost;
		boardSquarePathInfo.m_unskippable = m_unskippable;
		boardSquarePathInfo.m_reverse = m_reverse;
		boardSquarePathInfo.chargeEndType = chargeEndType;
		boardSquarePathInfo.chargeCycleType = chargeCycleType;
		boardSquarePathInfo.segmentMovementSpeed = segmentMovementSpeed;
		boardSquarePathInfo.segmentMovementDuration = segmentMovementDuration;
		boardSquarePathInfo.connectionType = connectionType;
		boardSquarePathInfo.m_moverDiesHere = m_moverDiesHere;
		boardSquarePathInfo.m_moverHasGameplayHitHere = m_moverHasGameplayHitHere;
		boardSquarePathInfo.m_updateLastKnownPos = m_updateLastKnownPos;
		boardSquarePathInfo.m_visibleToEnemies = m_visibleToEnemies;
		boardSquarePathInfo.m_moverClashesHere = m_moverClashesHere;
		boardSquarePathInfo.m_moverBumpedFromClash = m_moverBumpedFromClash;
		if (previous != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			boardSquarePathInfo.prev = previous;
		}
		if (next != null)
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
			boardSquarePathInfo.next = next.Clone(boardSquarePathInfo);
		}
		return boardSquarePathInfo;
	}

	public void CalcAndSetMoveCostToEnd()
	{
		float num = moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo.moveCost = num, boardSquarePathInfo = boardSquarePathInfo.next)
		{
			bool flag = Board.Get()._0015(boardSquarePathInfo.square, boardSquarePathInfo.prev.square);
			bool flag2 = Board.Get()._0012(boardSquarePathInfo.square, boardSquarePathInfo.prev.square);
			bool flag3 = boardSquarePathInfo.square == boardSquarePathInfo.prev.square;
			if (flag)
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
				num += 1.5f;
				continue;
			}
			if (flag2)
			{
				num += 1f;
				continue;
			}
			if (flag3)
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
				if (boardSquarePathInfo.next != null)
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
					Log.Warning("Calculating move costs on a path, but it has the same square twice in a row.");
				}
				continue;
			}
			if (boardSquarePathInfo.connectionType != 0)
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
				if (boardSquarePathInfo.connectionType != ConnectionType.Vault)
				{
					num += boardSquarePathInfo.square.HorizontalDistanceOnBoardTo(boardSquarePathInfo.prev.square);
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			Log.Warning("Calculating move costs on a path, but it has two non-adjacent consecutive squares.");
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public float FindMoveCostToEnd()
	{
		float num = moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			num = boardSquarePathInfo.moveCost;
		}
		return num - moveCost;
	}

	public float FindMoveCostToOneBeforeEnd()
	{
		float num = moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (boardSquarePathInfo.next == null)
			{
				break;
			}
			num = boardSquarePathInfo.moveCost;
		}
		return num - moveCost;
	}

	internal float FindDistanceToEnd()
	{
		float num = 0f;
		for (BoardSquarePathInfo boardSquarePathInfo = next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			num += (boardSquarePathInfo.square.ToVector3() - square.ToVector3()).magnitude;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return num;
		}
	}

	public int GetNumSquaresToEnd(bool checkDuplicate = true)
	{
		BoardSquarePathInfo boardSquarePathInfo = this;
		int num = 1;
		for (; boardSquarePathInfo.next != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (checkDuplicate)
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
				if (!(boardSquarePathInfo.next.square != boardSquarePathInfo.square))
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			num++;
		}
		return num;
	}

	public BoardSquarePathInfo GetPathMidpoint()
	{
		float num = FindMoveCostToEnd();
		float num2 = num / 2f;
		BoardSquarePathInfo result = this;
		float num3 = num;
		BoardSquarePathInfo boardSquarePathInfo = next;
		while (true)
		{
			if (boardSquarePathInfo != null)
			{
				float num4 = boardSquarePathInfo.FindMoveCostToEnd();
				if (Math.Abs(num4 - num2) < Math.Abs(num3 - num2))
				{
					result = boardSquarePathInfo;
					num3 = num4;
					boardSquarePathInfo = boardSquarePathInfo.next;
					continue;
				}
				break;
			}
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
			break;
		}
		return result;
	}

	public BoardSquarePathInfo GetPathEndpoint()
	{
		BoardSquarePathInfo boardSquarePathInfo = this;
		while (boardSquarePathInfo.next != null)
		{
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
		return boardSquarePathInfo;
	}

	public BoardSquarePathInfo GetPathStartPoint()
	{
		BoardSquarePathInfo boardSquarePathInfo = this;
		while (boardSquarePathInfo.prev != null)
		{
			boardSquarePathInfo = boardSquarePathInfo.prev;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return boardSquarePathInfo;
		}
	}

	public bool IsPathEndpoint()
	{
		if (next == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return true;
				}
			}
		}
		return false;
	}

	public bool IsPathStartPoint()
	{
		if (prev == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return true;
				}
			}
		}
		return false;
	}

	public BoardSquarePathInfo BackUpOnceFromEnd()
	{
		BoardSquarePathInfo pathEndpoint = GetPathEndpoint();
		if (pathEndpoint != null)
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
			if (pathEndpoint.prev != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						BoardSquarePathInfo boardSquarePathInfo = pathEndpoint.prev;
						boardSquarePathInfo.next = null;
						return boardSquarePathInfo;
					}
					}
				}
			}
		}
		return pathEndpoint;
	}

	public List<GridPos> ToGridPosPath()
	{
		List<GridPos> list = new List<GridPos>();
		for (BoardSquarePathInfo boardSquarePathInfo = this; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			list.Add(boardSquarePathInfo.square.GetGridPos());
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public bool IsValidPathForMaxMovement(float maxMovement)
	{
		if (maxMovement <= 0f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return next == null;
				}
			}
		}
		float num = FindMoveCostToOneBeforeEnd();
		return maxMovement > num;
	}

	public static bool IsConnectionTypeConventional(ConnectionType connectionType)
	{
		int result;
		if (connectionType != ConnectionType.Flight && connectionType != ConnectionType.Teleport)
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
			if (connectionType != ConnectionType.Knockback)
			{
				result = ((connectionType != ConnectionType.Charge) ? 1 : 0);
				goto IL_0029;
			}
		}
		result = 0;
		goto IL_0029;
		IL_0029:
		return (byte)result != 0;
	}

	public string GetDebugPathStringToEnd(string prefix)
	{
		string text = prefix;
		int num = 0;
		BoardSquarePathInfo boardSquarePathInfo = this;
		while (boardSquarePathInfo != null)
		{
			if (num < 100)
			{
				string text2;
				if (boardSquarePathInfo.square != null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					text2 = boardSquarePathInfo.square.ToString();
				}
				else
				{
					text2 = null;
				}
				string text3 = text;
				text = text3 + "\n" + text2 + " | Connection Type = " + boardSquarePathInfo.connectionType;
				if (boardSquarePathInfo == boardSquarePathInfo.next)
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
					break;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
				num++;
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		return text;
	}

	public string GetDebugPathStringToBeginning(string prefix)
	{
		string text = prefix;
		int num = 0;
		BoardSquarePathInfo boardSquarePathInfo = this;
		while (boardSquarePathInfo != null)
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
			if (num < 100)
			{
				string text2;
				if (boardSquarePathInfo.square != null)
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
					text2 = boardSquarePathInfo.square.ToString();
				}
				else
				{
					text2 = null;
				}
				string text3 = text;
				text = text3 + "\n" + text2 + " | Connection Type = " + boardSquarePathInfo.connectionType;
				if (boardSquarePathInfo == boardSquarePathInfo.prev)
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
					break;
				}
				boardSquarePathInfo = boardSquarePathInfo.prev;
				num++;
				continue;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		return text;
	}

	public bool CheckPathConnectionForSelfReference()
	{
		return true;
	}

	public bool WillDieAtEnd()
	{
		BoardSquarePathInfo pathEndpoint = GetPathEndpoint();
		if (pathEndpoint != null)
		{
			if (pathEndpoint.m_moverDiesHere)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public void CheckIsValidTriggeringPath(ActorData mover)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		BoardSquarePathInfo pathStartPoint = GetPathStartPoint();
		while (pathStartPoint != null)
		{
			if (num2 >= 100)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				break;
			}
			if (square == null)
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
				num++;
			}
			if (flag)
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
				num3++;
			}
			if (pathStartPoint.m_moverDiesHere)
			{
				flag = true;
			}
			pathStartPoint = pathStartPoint.next;
			num2++;
		}
		if (num <= 0)
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
			if (num2 < 100)
			{
				if (num3 <= 0)
				{
					return;
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
		}
		string text = num + " null squares";
		if (num != 0)
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
			text = "INVALID SQUARES: " + text;
		}
		string text2 = num2 + " total path nodes";
		if (num2 >= 100)
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
			text2 = "INVALID LENGTH: " + text2;
		}
		string text3 = num3 + " steps after death";
		if (num3 > 0)
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
			text3 = "INVALID DEATH-MOVEMENT: " + text3;
		}
		string str = "Invalid BoardSquarePathInfo for gameplay!  Path has:\n\t" + text2 + "\n\t" + text + "\n\t" + text3;
		str = str + "\nMover: " + mover.GetDebugName();
		Debug.LogError(str);
	}

	public bool IsNodePartOfMyFuturePath(BoardSquarePathInfo other, bool includePresent = true)
	{
		bool result = false;
		BoardSquarePathInfo boardSquarePathInfo;
		if (includePresent)
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
			boardSquarePathInfo = this;
		}
		else
		{
			boardSquarePathInfo = next;
		}
		while (true)
		{
			if (boardSquarePathInfo != null)
			{
				if (other == boardSquarePathInfo)
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
					result = true;
					break;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		return result;
	}

	public void ResetClashingOfPath()
	{
		for (BoardSquarePathInfo boardSquarePathInfo = this; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (boardSquarePathInfo.m_moverClashesHere)
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
				boardSquarePathInfo.m_moverClashesHere = false;
			}
			if (boardSquarePathInfo.m_moverBumpedFromClash)
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
				boardSquarePathInfo.m_moverBumpedFromClash = false;
			}
		}
	}
}
