using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityTarget
{
	private GridPos m_gridPos;

	private Vector3 m_dir;

	private Vector3 m_freePos;

	private static AbilityTarget s_abilityTargetForTargeterUpdate = new AbilityTarget();

	private AbilityTarget()
	{
	}

	private AbilityTarget(GridPos targetPos, Vector3 freePos, Vector3 dir)
	{
		this.m_freePos = freePos;
		this.m_gridPos = targetPos;
		this.m_dir = dir;
	}

	public Vector3 FreePos
	{
		get
		{
			return this.m_freePos;
		}
		private set
		{
		}
	}

	public GridPos GridPos
	{
		get
		{
			return this.m_gridPos;
		}
		private set
		{
		}
	}

	public Vector3 AimDirection
	{
		get
		{
			return this.m_dir;
		}
		private set
		{
		}
	}

	public void SetPosAndDir(GridPos gridPos, Vector3 freePos, Vector3 dir)
	{
		this.m_gridPos = gridPos;
		this.m_freePos = freePos;
		this.m_dir = dir;
	}

	public void SetValuesFromBoardSquare(BoardSquare targetSquare, Vector3 currentWorldPos)
	{
		Vector3 dir = targetSquare.ToVector3() - currentWorldPos;
		dir.y = 0f;
		dir.Normalize();
		this.SetPosAndDir(targetSquare.GetGridPos(), targetSquare.ToVector3(), dir);
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		Vector3 dir = this.m_dir;
		Vector3 freePos = this.m_freePos;
		this.m_gridPos.OnSerializeHelper(stream);
		stream.Serialize(ref dir);
		stream.Serialize(ref freePos);
		this.m_dir = dir;
		this.m_freePos = freePos;
	}

	public Vector3 GetWorldGridPos()
	{
		Vector3 result = new Vector3(this.GridPos.worldX, (float)this.GridPos.height, this.GridPos.worldY);
		return result;
	}

	public static AbilityTarget CreateAbilityTargetFromInterface()
	{
		Vector3 playerClampedPos = Board.Get().PlayerClampedPos;
		Vector3 playerLookDir = Board.Get().PlayerLookDir;
		GridPos targetPos;
		if (Board.Get().PlayerClampedSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.CreateAbilityTargetFromInterface()).MethodHandle;
			}
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
		return new AbilityTarget(this.GridPos, this.FreePos, this.AimDirection);
	}

	public static AbilityTarget GetAbilityTargetForTargeterUpdate()
	{
		return AbilityTarget.s_abilityTargetForTargeterUpdate;
	}

	public static void UpdateAbilityTargetForForTargeterUpdate()
	{
		Vector3 playerClampedPos = Board.Get().PlayerClampedPos;
		Vector3 playerLookDir = Board.Get().PlayerLookDir;
		GridPos gridPos;
		if (Board.Get().PlayerClampedSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.UpdateAbilityTargetForForTargeterUpdate()).MethodHandle;
			}
			gridPos = Board.Get().PlayerClampedSquare.GetGridPos();
		}
		else
		{
			gridPos = default(GridPos);
			gridPos.x = -1;
			gridPos.y = -1;
			gridPos.height = -1;
		}
		AbilityTarget.s_abilityTargetForTargeterUpdate.SetPosAndDir(gridPos, playerClampedPos, playerLookDir);
	}

	public static AbilityTarget CreateAbilityTargetFromActor(ActorData targetActor, ActorData casterActor)
	{
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - casterActor.GetTravelBoardSquareWorldPosition();
		if (vector != Vector3.zero)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.CreateAbilityTargetFromActor(ActorData, ActorData)).MethodHandle;
			}
			vector.Normalize();
		}
		return new AbilityTarget(targetActor.GetGridPosWithIncrementedHeight(), targetActor.GetTravelBoardSquareWorldPosition(), vector);
	}

	public static AbilityTarget CreateSimpleAbilityTarget(ActorData casterActor)
	{
		return new AbilityTarget(casterActor.GetGridPosWithIncrementedHeight(), casterActor.GetTravelBoardSquareWorldPosition(), Vector3.zero);
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
		return new AbilityTarget(new GridPos
		{
			x = -1,
			y = -1,
			height = -1
		}, vector, dir);
	}

	public static AbilityTarget CreateAbilityTargetFromWorldPos(Vector3 targetWorldPos, Vector3 casterWorldPos)
	{
		Vector3 dir = targetWorldPos - casterWorldPos;
		dir.y = 0f;
		if (dir.magnitude > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.CreateAbilityTargetFromWorldPos(Vector3, Vector3)).MethodHandle;
			}
			dir.Normalize();
		}
		BoardSquare boardSquare = Board.Get().\u0013(targetWorldPos.x, targetWorldPos.z);
		GridPos targetPos;
		if (boardSquare)
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
		foreach (ActorData actorData in playerList)
		{
			vector += actorData.GetTravelBoardSquareWorldPosition();
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
		BoardSquare boardSquareUnsafe = Board.Get().GetBoardSquareUnsafe(vector.x, vector.z);
		List<AbilityTarget> list = new List<AbilityTarget>();
		AbilityTarget item = new AbilityTarget(boardSquareUnsafe.GetGridPos(), vector, vector2);
		list.Add(item);
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.CreateAbilityTargetsFromActorDataList(List<ActorData>, ActorData)).MethodHandle;
			}
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
		return new List<AbilityTarget>
		{
			onlyTarget
		};
	}

	internal static void SerializeAbilityTargetList(List<AbilityTarget> targetList, NetworkWriter stream)
	{
		AbilityTarget.SerializeAbilityTargetList(targetList, new NetworkWriterAdapter(stream));
	}

	internal static void SerializeAbilityTargetList(List<AbilityTarget> targetList, IBitStream stream)
	{
		byte b = checked((byte)targetList.Count);
		stream.Serialize(ref b);
		for (int i = 0; i < targetList.Count; i++)
		{
			AbilityTarget abilityTarget = targetList[i];
			checked
			{
				short num = (short)abilityTarget.GridPos.x;
				short num2 = (short)abilityTarget.GridPos.y;
				Vector3 dir = abilityTarget.m_dir;
				Vector3 freePos = abilityTarget.m_freePos;
				stream.Serialize(ref num);
				stream.Serialize(ref num2);
				stream.Serialize(ref dir);
				stream.Serialize(ref freePos);
			}
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.SerializeAbilityTargetList(List<AbilityTarget>, IBitStream)).MethodHandle;
		}
	}

	internal static List<AbilityTarget> DeSerializeAbilityTargetList(NetworkReader stream)
	{
		return AbilityTarget.DeSerializeAbilityTargetList(new NetworkReaderAdapter(stream));
	}

	internal static List<AbilityTarget> DeSerializeAbilityTargetList(IBitStream stream)
	{
		List<AbilityTarget> list = new List<AbilityTarget>();
		byte b = 0;
		stream.Serialize(ref b);
		for (int i = 0; i < (int)b; i++)
		{
			short x = -1;
			short y = -1;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			stream.Serialize(ref x);
			stream.Serialize(ref y);
			stream.Serialize(ref zero);
			stream.Serialize(ref zero2);
			AbilityTarget item = new AbilityTarget(new GridPos
			{
				x = (int)x,
				y = (int)y,
				height = (int)Board.Get().GetSquareHeight((int)x, (int)y)
			}, zero2, zero);
			list.Add(item);
		}
		return list;
	}

	public ActorData GetCurrentBestActorTarget()
	{
		ActorData result = null;
		GridPos gridPos = this.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		if (boardSquareSafe != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTarget.GetCurrentBestActorTarget()).MethodHandle;
			}
			if (boardSquareSafe.occupant != null)
			{
				result = boardSquareSafe.occupant.GetComponent<ActorData>();
			}
		}
		return result;
	}

	public string GetDebugString()
	{
		return string.Concat(new object[]
		{
			"[GridPos= ",
			this.m_gridPos.ToStringWithCross(),
			" | FreePos= (",
			this.m_freePos.x,
			", ",
			this.m_freePos.z,
			")]"
		});
	}
}
