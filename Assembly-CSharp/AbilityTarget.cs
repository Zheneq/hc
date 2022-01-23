using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityTarget
{
	private GridPos m_gridPos;

	private Vector3 m_dir;

	private Vector3 m_freePos;

	private static AbilityTarget s_abilityTargetForTargeterUpdate = new AbilityTarget();

	public Vector3 FreePos
	{
		get
		{
			return m_freePos;
		}
		private set
		{
		}
	}

	public GridPos GridPos
	{
		get
		{
			return m_gridPos;
		}
		private set
		{
		}
	}

	public Vector3 AimDirection
	{
		get
		{
			return m_dir;
		}
		private set
		{
		}
	}

	private AbilityTarget()
	{
	}

	private AbilityTarget(GridPos targetPos, Vector3 freePos, Vector3 dir)
	{
		m_freePos = freePos;
		m_gridPos = targetPos;
		m_dir = dir;
	}

	public void SetPosAndDir(GridPos gridPos, Vector3 freePos, Vector3 dir)
	{
		m_gridPos = gridPos;
		m_freePos = freePos;
		m_dir = dir;
	}

	public void SetValuesFromBoardSquare(BoardSquare targetSquare, Vector3 currentWorldPos)
	{
		Vector3 dir = targetSquare.ToVector3() - currentWorldPos;
		dir.y = 0f;
		dir.Normalize();
		SetPosAndDir(targetSquare.GetGridPos(), targetSquare.ToVector3(), dir);
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		Vector3 value = m_dir;
		Vector3 value2 = m_freePos;
		m_gridPos.OnSerializeHelper(stream);
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		m_dir = value;
		m_freePos = value2;
	}

	public Vector3 GetWorldGridPos()
	{
		Vector3 result = new Vector3(GridPos.worldX, GridPos.height, GridPos.worldY);
		return result;
	}

	public static AbilityTarget CreateAbilityTargetFromInterface()
	{
		Vector3 playerClampedPos = Board.Get().PlayerClampedPos;
		Vector3 playerLookDir = Board.Get().PlayerLookDir;
		GridPos targetPos;
		if (Board.Get().PlayerClampedSquare != null)
		{
			targetPos = Board.Get().PlayerClampedSquare.GetGridPos();
		}
		else
		{
			targetPos = default(GridPos);
			targetPos.x = -1;
			targetPos.y = -1;
			targetPos.height = -1;
		}
		return new AbilityTarget(targetPos, playerClampedPos, playerLookDir);
	}

	public AbilityTarget GetCopy()
	{
		return new AbilityTarget(GridPos, FreePos, AimDirection);
	}

	public static AbilityTarget GetAbilityTargetForTargeterUpdate()
	{
		return s_abilityTargetForTargeterUpdate;
	}

	public static void UpdateAbilityTargetForForTargeterUpdate()
	{
		Vector3 playerClampedPos = Board.Get().PlayerClampedPos;
		Vector3 playerLookDir = Board.Get().PlayerLookDir;
		GridPos gridPos;
		if (Board.Get().PlayerClampedSquare != null)
		{
			gridPos = Board.Get().PlayerClampedSquare.GetGridPos();
		}
		else
		{
			gridPos = default(GridPos);
			gridPos.x = -1;
			gridPos.y = -1;
			gridPos.height = -1;
		}
		s_abilityTargetForTargeterUpdate.SetPosAndDir(gridPos, playerClampedPos, playerLookDir);
	}

	public static AbilityTarget CreateAbilityTargetFromActor(ActorData targetActor, ActorData casterActor)
	{
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - casterActor.GetTravelBoardSquareWorldPosition();
		if (vector != Vector3.zero)
		{
			vector.Normalize();
		}
		return new AbilityTarget(targetActor.GetGridPos(), targetActor.GetTravelBoardSquareWorldPosition(), vector);
	}

	public static AbilityTarget CreateSimpleAbilityTarget(ActorData casterActor)
	{
		return new AbilityTarget(casterActor.GetGridPos(), casterActor.GetTravelBoardSquareWorldPosition(), Vector3.zero);
	}

	public static AbilityTarget CreateAbilityTargetFromBoardSquare(BoardSquare targetSquare, Vector3 currentWorldPos)
	{
		Vector3 dir = targetSquare.ToVector3() - currentWorldPos;
		dir.y = 0f;
		dir.Normalize();
		return new AbilityTarget(targetSquare.GetGridPos(), targetSquare.ToVector3(), dir);
	}

	public static AbilityTarget CreateAbilityTargetFromTwoBoardSquares(BoardSquare targetSquare, BoardSquare targetSquare2, Vector3 currentWorldPos)
	{
		Vector3 vector = (targetSquare.ToVector3() + targetSquare2.ToVector3()) / 2f;
		Vector3 dir = vector - currentWorldPos;
		dir.y = 0f;
		dir.Normalize();
		GridPos targetPos = default(GridPos);
		targetPos.x = -1;
		targetPos.y = -1;
		targetPos.height = -1;
		return new AbilityTarget(targetPos, vector, dir);
	}

	public static AbilityTarget CreateAbilityTargetFromWorldPos(Vector3 targetWorldPos, Vector3 casterWorldPos)
	{
		Vector3 dir = targetWorldPos - casterWorldPos;
		dir.y = 0f;
		if (dir.magnitude > 0f)
		{
			dir.Normalize();
		}
		BoardSquare boardSquare = Board.Get()._0013(targetWorldPos.x, targetWorldPos.z);
		GridPos targetPos;
		if ((bool)boardSquare)
		{
			targetPos = boardSquare.GetGridPos();
		}
		else
		{
			targetPos = default(GridPos);
			targetPos.x = -1;
			targetPos.y = -1;
			targetPos.height = -1;
		}
		return new AbilityTarget(targetPos, targetWorldPos, dir);
	}

	public static List<AbilityTarget> CreateAbilityTargetsFromActorDataList(List<ActorData> playerList, ActorData casterActor)
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		foreach (ActorData player in playerList)
		{
			vector += player.GetTravelBoardSquareWorldPosition();
		}
		vector /= (float)playerList.Count;
		Vector3 vector2 = vector - casterActor.GetTravelBoardSquareWorldPosition();
		vector2.y = 0f;
		vector2.Normalize();
		bool flag = false;
		if (vector2 == Vector3.zero)
		{
			Vector3 rhs = playerList[0].GetTravelBoardSquareWorldPosition() - casterActor.GetTravelBoardSquareWorldPosition();
			rhs.Normalize();
			vector2 = Vector3.Cross(Vector3.up, rhs);
			flag = true;
		}
		BoardSquare boardSquareUnsafe = Board.Get().GetSquareClosestToPos(vector.x, vector.z);
		List<AbilityTarget> list = new List<AbilityTarget>();
		AbilityTarget item = new AbilityTarget(boardSquareUnsafe.GetGridPos(), vector, vector2);
		list.Add(item);
		if (flag)
		{
			AbilityTarget item2 = new AbilityTarget(boardSquareUnsafe.GetGridPos(), vector, -vector2);
			list.Add(item2);
		}
		return list;
	}

	internal static AbilityTarget CreateAbilityTarget(IBitStream stream)
	{
		AbilityTarget abilityTarget = new AbilityTarget();
		abilityTarget.OnSerializeHelper(stream);
		return abilityTarget;
	}

	public static List<AbilityTarget> AbilityTargetList(AbilityTarget onlyTarget)
	{
		List<AbilityTarget> list = new List<AbilityTarget>();
		list.Add(onlyTarget);
		return list;
	}

	internal static void SerializeAbilityTargetList(List<AbilityTarget> targetList, NetworkWriter stream)
	{
		SerializeAbilityTargetList(targetList, new NetworkWriterAdapter(stream));
	}

	internal static void SerializeAbilityTargetList(List<AbilityTarget> targetList, IBitStream stream)
	{
		checked
		{
			byte value = (byte)targetList.Count;
			stream.Serialize(ref value);
			for (int i = 0; i < targetList.Count; i = unchecked(i + 1))
			{
				AbilityTarget abilityTarget = targetList[i];
				short value2 = (short)abilityTarget.GridPos.x;
				short value3 = (short)abilityTarget.GridPos.y;
				Vector3 value4 = abilityTarget.m_dir;
				Vector3 value5 = abilityTarget.m_freePos;
				stream.Serialize(ref value2);
				stream.Serialize(ref value3);
				stream.Serialize(ref value4);
				stream.Serialize(ref value5);
			}
			while (true)
			{
				return;
			}
		}
	}

	internal static List<AbilityTarget> DeSerializeAbilityTargetList(NetworkReader stream)
	{
		return DeSerializeAbilityTargetList(new NetworkReaderAdapter(stream));
	}

	internal static List<AbilityTarget> DeSerializeAbilityTargetList(IBitStream stream)
	{
		List<AbilityTarget> list = new List<AbilityTarget>();
		byte value = 0;
		stream.Serialize(ref value);
		for (int i = 0; i < value; i++)
		{
			short value2 = -1;
			short value3 = -1;
			Vector3 value4 = Vector3.zero;
			Vector3 value5 = Vector3.zero;
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			stream.Serialize(ref value5);
			GridPos targetPos = default(GridPos);
			targetPos.x = value2;
			targetPos.y = value3;
			targetPos.height = (int)Board.Get().GetSquareHeight(value2, value3);
			AbilityTarget item = new AbilityTarget(targetPos, value5, value4);
			list.Add(item);
		}
		return list;
	}

	public ActorData GetCurrentBestActorTarget()
	{
		ActorData result = null;
		GridPos gridPos = GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(gridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.occupant != null)
			{
				result = boardSquareSafe.occupant.GetComponent<ActorData>();
			}
		}
		return result;
	}

	public string GetDebugString()
	{
		return "[GridPos= " + m_gridPos.ToStringWithCross() + " | FreePos= (" + m_freePos.x + ", " + m_freePos.z + ")]";
	}
}
