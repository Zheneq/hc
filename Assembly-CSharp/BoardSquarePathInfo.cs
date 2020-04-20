using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquarePathInfo : IComparable
{
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

	public BoardSquarePathInfo.ConnectionType connectionType;

	public BoardSquarePathInfo.ChargeCycleType chargeCycleType;

	public BoardSquarePathInfo.ChargeEndType chargeEndType = BoardSquarePathInfo.ChargeEndType.None;

	public float segmentMovementSpeed;

	public float segmentMovementDuration;

	public float F_cost
	{
		get
		{
			int num = Mathf.Max(this.m_expectedBackupNum - 1, 0);
			if (num > 0)
			{
				float num2 = 1.5f * (float)num + 0.1f;
				return this.moveCost + this.heuristicCost + num2;
			}
			return this.moveCost + this.heuristicCost;
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
			return this.FindMoveCostToEnd().CompareTo(boardSquarePathInfo.FindMoveCostToEnd());
		}
		throw new ArgumentException("Object is not a BoardSquarePathInfo");
	}

	public void ResetValuesToDefault()
	{
		this.square = null;
		this.moveCost = 0f;
		this.heuristicCost = 0f;
		this.prev = null;
		this.next = null;
		this.m_unskippable = false;
		this.m_reverse = false;
		this.m_visibleToEnemies = false;
		this.m_updateLastKnownPos = false;
		this.m_moverDiesHere = false;
		this.m_moverHasGameplayHitHere = false;
		this.m_moverClashesHere = false;
		this.m_moverBumpedFromClash = false;
		this.m_expectedBackupNum = 0;
	}

	public bool IsSamePathAs(BoardSquarePathInfo other)
	{
		if (other == null)
		{
			return false;
		}
		bool flag = true;
		flag &= (other.square == this.square);
		flag &= (other.moveCost == this.moveCost);
		flag &= (other.heuristicCost == this.heuristicCost);
		flag &= (other.m_unskippable == this.m_unskippable);
		flag &= (other.m_moverClashesHere = this.m_moverClashesHere);
		flag &= (other.m_moverBumpedFromClash = this.m_moverBumpedFromClash);
		flag &= (other.m_reverse == this.m_reverse);
		flag &= (other.chargeEndType == this.chargeEndType);
		flag &= (other.chargeCycleType == this.chargeCycleType);
		flag &= (other.segmentMovementSpeed == this.segmentMovementSpeed);
		flag &= (other.segmentMovementDuration == this.segmentMovementDuration);
		if (flag)
		{
			if (this.next != null)
			{
				if (other.next != null)
				{
					return this.next.IsSamePathAs(other.next);
				}
			}
			if (this.next == null && other.next != null)
			{
				flag = false;
			}
			else if (this.next != null)
			{
				if (other.next == null)
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	public BoardSquarePathInfo Clone(BoardSquarePathInfo previous)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = this.square;
		boardSquarePathInfo.moveCost = this.moveCost;
		boardSquarePathInfo.heuristicCost = this.heuristicCost;
		boardSquarePathInfo.m_unskippable = this.m_unskippable;
		boardSquarePathInfo.m_reverse = this.m_reverse;
		boardSquarePathInfo.chargeEndType = this.chargeEndType;
		boardSquarePathInfo.chargeCycleType = this.chargeCycleType;
		boardSquarePathInfo.segmentMovementSpeed = this.segmentMovementSpeed;
		boardSquarePathInfo.segmentMovementDuration = this.segmentMovementDuration;
		boardSquarePathInfo.connectionType = this.connectionType;
		boardSquarePathInfo.m_moverDiesHere = this.m_moverDiesHere;
		boardSquarePathInfo.m_moverHasGameplayHitHere = this.m_moverHasGameplayHitHere;
		boardSquarePathInfo.m_updateLastKnownPos = this.m_updateLastKnownPos;
		boardSquarePathInfo.m_visibleToEnemies = this.m_visibleToEnemies;
		boardSquarePathInfo.m_moverClashesHere = this.m_moverClashesHere;
		boardSquarePathInfo.m_moverBumpedFromClash = this.m_moverBumpedFromClash;
		if (previous != null)
		{
			boardSquarePathInfo.prev = previous;
		}
		if (this.next != null)
		{
			boardSquarePathInfo.next = this.next.Clone(boardSquarePathInfo);
		}
		return boardSquarePathInfo;
	}

	public void CalcAndSetMoveCostToEnd()
	{
		float num = this.moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = this.next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			bool flag = Board.Get().symbol_0015(boardSquarePathInfo.square, boardSquarePathInfo.prev.square);
			bool flag2 = Board.Get().symbol_0012(boardSquarePathInfo.square, boardSquarePathInfo.prev.square);
			bool flag3 = boardSquarePathInfo.square == boardSquarePathInfo.prev.square;
			if (flag)
			{
				num += 1.5f;
			}
			else if (flag2)
			{
				num += 1f;
			}
			else if (flag3)
			{
				if (boardSquarePathInfo.next != null)
				{
					Log.Warning("Calculating move costs on a path, but it has the same square twice in a row.", new object[0]);
				}
			}
			else
			{
				if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Run)
				{
					if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Vault)
					{
						num += boardSquarePathInfo.square.HorizontalDistanceOnBoardTo(boardSquarePathInfo.prev.square);
						goto IL_120;
					}
				}
				Log.Warning("Calculating move costs on a path, but it has two non-adjacent consecutive squares.", new object[0]);
			}
			IL_120:
			boardSquarePathInfo.moveCost = num;
		}
	}

	public float FindMoveCostToEnd()
	{
		float num = this.moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = this.next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			num = boardSquarePathInfo.moveCost;
		}
		return num - this.moveCost;
	}

	public float FindMoveCostToOneBeforeEnd()
	{
		float num = this.moveCost;
		for (BoardSquarePathInfo boardSquarePathInfo = this.next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (boardSquarePathInfo.next == null)
			{
				break;
			}
			num = boardSquarePathInfo.moveCost;
		}
		return num - this.moveCost;
	}

	internal float FindDistanceToEnd()
	{
		float num = 0f;
		for (BoardSquarePathInfo boardSquarePathInfo = this.next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			num += (boardSquarePathInfo.square.ToVector3() - this.square.ToVector3()).magnitude;
		}
		return num;
	}

	public int GetNumSquaresToEnd(bool checkDuplicate = true)
	{
		BoardSquarePathInfo boardSquarePathInfo = this;
		int num = 1;
		while (boardSquarePathInfo.next != null)
		{
			if (!checkDuplicate)
			{
				goto IL_40;
			}
			if (boardSquarePathInfo.next.square != boardSquarePathInfo.square)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					goto IL_40;
				}
			}
			IL_44:
			boardSquarePathInfo = boardSquarePathInfo.next;
			continue;
			IL_40:
			num++;
			goto IL_44;
		}
		return num;
	}

	public BoardSquarePathInfo GetPathMidpoint()
	{
		float num = this.FindMoveCostToEnd();
		float num2 = num / 2f;
		BoardSquarePathInfo result = this;
		float num3 = num;
		for (BoardSquarePathInfo boardSquarePathInfo = this.next; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			float num4 = boardSquarePathInfo.FindMoveCostToEnd();
			if (Math.Abs(num4 - num2) >= Math.Abs(num3 - num2))
			{
				return result;
			}
			result = boardSquarePathInfo;
			num3 = num4;
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
		return boardSquarePathInfo;
	}

	public bool IsPathEndpoint()
	{
		if (this.next == null)
		{
			return true;
		}
		return false;
	}

	public bool IsPathStartPoint()
	{
		if (this.prev == null)
		{
			return true;
		}
		return false;
	}

	public BoardSquarePathInfo BackUpOnceFromEnd()
	{
		BoardSquarePathInfo pathEndpoint = this.GetPathEndpoint();
		if (pathEndpoint != null)
		{
			if (pathEndpoint.prev != null)
			{
				BoardSquarePathInfo boardSquarePathInfo = pathEndpoint.prev;
				boardSquarePathInfo.next = null;
				return boardSquarePathInfo;
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
		return list;
	}

	public bool IsValidPathForMaxMovement(float maxMovement)
	{
		bool result;
		if (maxMovement <= 0f)
		{
			result = (this.next == null);
		}
		else
		{
			float num = this.FindMoveCostToOneBeforeEnd();
			result = (maxMovement > num);
		}
		return result;
	}

	public static bool IsConnectionTypeConventional(BoardSquarePathInfo.ConnectionType connectionType)
	{
		if (connectionType != BoardSquarePathInfo.ConnectionType.Flight && connectionType != BoardSquarePathInfo.ConnectionType.Teleport)
		{
			if (connectionType != BoardSquarePathInfo.ConnectionType.Knockback)
			{
				return connectionType != BoardSquarePathInfo.ConnectionType.Charge;
			}
		}
		return false;
	}

	public string GetDebugPathStringToEnd(string prefix)
	{
		string text = prefix;
		int num = 0;
		BoardSquarePathInfo boardSquarePathInfo = this;
		while (boardSquarePathInfo != null)
		{
			if (num >= 0x64)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					return text;
				}
			}
			else
			{
				string text2;
				if (boardSquarePathInfo.square != null)
				{
					text2 = boardSquarePathInfo.square.ToString();
				}
				else
				{
					text2 = null;
				}
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					"\n",
					text2,
					" | Connection Type = ",
					boardSquarePathInfo.connectionType.ToString()
				});
				if (boardSquarePathInfo == boardSquarePathInfo.next)
				{
					break;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
				num++;
			}
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
			if (num >= 0x64)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					return text;
				}
			}
			else
			{
				string text2;
				if (boardSquarePathInfo.square != null)
				{
					text2 = boardSquarePathInfo.square.ToString();
				}
				else
				{
					text2 = null;
				}
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					"\n",
					text2,
					" | Connection Type = ",
					boardSquarePathInfo.connectionType.ToString()
				});
				if (boardSquarePathInfo == boardSquarePathInfo.prev)
				{
					break;
				}
				boardSquarePathInfo = boardSquarePathInfo.prev;
				num++;
			}
		}
		return text;
	}

	public bool CheckPathConnectionForSelfReference()
	{
		return true;
	}

	public bool WillDieAtEnd()
	{
		BoardSquarePathInfo pathEndpoint = this.GetPathEndpoint();
		bool result;
		if (pathEndpoint != null)
		{
			if (pathEndpoint.m_moverDiesHere)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void CheckIsValidTriggeringPath(ActorData mover)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		BoardSquarePathInfo pathStartPoint = this.GetPathStartPoint();
		while (pathStartPoint != null)
		{
			if (num2 >= 0x64)
			{
				break;
			}
			if (this.square == null)
			{
				num++;
			}
			if (flag)
			{
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
			if (num2 < 0x64)
			{
				if (num3 <= 0)
				{
					return;
				}
			}
		}
		string text = num + " null squares";
		if (num != 0)
		{
			text = "INVALID SQUARES: " + text;
		}
		string text2 = num2 + " total path nodes";
		if (num2 >= 0x64)
		{
			text2 = "INVALID LENGTH: " + text2;
		}
		string text3 = num3 + " steps after death";
		if (num3 > 0)
		{
			text3 = "INVALID DEATH-MOVEMENT: " + text3;
		}
		string text4 = string.Concat(new string[]
		{
			"Invalid BoardSquarePathInfo for gameplay!  Path has:\n\t",
			text2,
			"\n\t",
			text,
			"\n\t",
			text3
		});
		text4 = text4 + "\nMover: " + mover.GetDebugName();
		Debug.LogError(text4);
	}

	public bool IsNodePartOfMyFuturePath(BoardSquarePathInfo other, bool includePresent = true)
	{
		bool result = false;
		BoardSquarePathInfo boardSquarePathInfo;
		if (includePresent)
		{
			boardSquarePathInfo = this;
		}
		else
		{
			boardSquarePathInfo = this.next;
		}
		while (boardSquarePathInfo != null)
		{
			if (other == boardSquarePathInfo)
			{
				result = true;
				return result;
			}
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return result;
		}
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
}
