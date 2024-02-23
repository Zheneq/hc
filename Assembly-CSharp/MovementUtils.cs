using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class MovementUtils
{
	internal static void SerializePath(BoardSquarePathInfo path, NetworkWriter writer)
	{
		bool isPathNotEmpty = path != null;
		float segmentMovementSpeed = 8f;
		float segmentMovementDuration = 0f;
		writer.Write(isPathNotEmpty);
		if (isPathNotEmpty)
		{
			segmentMovementSpeed = path.segmentMovementSpeed;
			segmentMovementDuration = path.segmentMovementDuration;
			writer.Write(path.segmentMovementSpeed);
			writer.Write(path.segmentMovementDuration);
			writer.Write(path.moveCost);
		}
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			byte x = 0;
			if (boardSquarePathInfo.square.x <= 255)
			{
				x = (byte)boardSquarePathInfo.square.x;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, x coordinate value too large for byte");
			}
			byte y = 0;
			if (boardSquarePathInfo.square.y <= 255)
			{
				y = (byte)boardSquarePathInfo.square.y;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, y coordinate value too large for byte");
			}
			sbyte connectionType = (sbyte)boardSquarePathInfo.connectionType;
			sbyte chargeCycleType = (sbyte)boardSquarePathInfo.chargeCycleType;
			sbyte chargeEndType = (sbyte)boardSquarePathInfo.chargeEndType;
			bool reverse = boardSquarePathInfo.m_reverse;
			bool unskippable = boardSquarePathInfo.m_unskippable;
			bool isEnd = boardSquarePathInfo.next == null;
			bool visibleToEnemies = boardSquarePathInfo.m_visibleToEnemies;
			bool updateLastKnownPos = boardSquarePathInfo.m_updateLastKnownPos;
			bool moverDiesHere = boardSquarePathInfo.m_moverDiesHere;
			bool overrideSpeed = !Mathf.Approximately(boardSquarePathInfo.segmentMovementSpeed, segmentMovementSpeed);
			bool overrideDuration = !Mathf.Approximately(boardSquarePathInfo.segmentMovementDuration, segmentMovementDuration);
			bool moverClashesHere = boardSquarePathInfo.m_moverClashesHere;
			bool moverBumpedFromClash = boardSquarePathInfo.m_moverBumpedFromClash;
			byte bitfield1 = ServerClientUtils.CreateBitfieldFromBools(reverse, unskippable, isEnd, visibleToEnemies, updateLastKnownPos, moverDiesHere, overrideSpeed, overrideDuration);
			byte bitfield2 = ServerClientUtils.CreateBitfieldFromBools(moverClashesHere, moverBumpedFromClash, false, false, false, false, false, false);
			writer.Write(x);
			writer.Write(y);
			writer.Write(connectionType);
			if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Run
				&& boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Vault
				&& boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Knockback)
			{
				writer.Write(chargeCycleType);
				writer.Write(chargeEndType);
			}
			writer.Write(bitfield1);
			writer.Write(bitfield2);
			if (overrideSpeed)
			{
				writer.Write(boardSquarePathInfo.segmentMovementSpeed);
			}
			if (overrideDuration)
			{
				writer.Write(boardSquarePathInfo.segmentMovementDuration);
			}
		}
	}

	internal static void SerializePath(BoardSquarePathInfo path, IBitStream stream)
	{
		if (stream.isReading)
		{
			Log.Error("Trying to serialize a path while reading");
			return;
		}
		bool isPathNotEmpty = path != null;
		float segmentMovementSpeed = 8f;
		float segmentMovementDuration = 0f;
		stream.Serialize(ref isPathNotEmpty);
		if (isPathNotEmpty)
		{
			segmentMovementSpeed = path.segmentMovementSpeed;
			segmentMovementDuration = path.segmentMovementDuration;
			stream.Serialize(ref path.segmentMovementSpeed);
			stream.Serialize(ref path.segmentMovementDuration);
			stream.Serialize(ref path.moveCost);
		}
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			byte x = 0;
			if (boardSquarePathInfo.square.x <= 255)
			{
				x = (byte)boardSquarePathInfo.square.x;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, x coordinate value too large for byte");
			}
			byte y = 0;
			if (boardSquarePathInfo.square.y <= 255)
			{
				y = (byte)boardSquarePathInfo.square.y;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, y coordinate value too large for byte");
			}
			sbyte connectionType = (sbyte)boardSquarePathInfo.connectionType;
			sbyte chargeCycleType = (sbyte)boardSquarePathInfo.chargeCycleType;
			sbyte chargeEndType = (sbyte)boardSquarePathInfo.chargeEndType;
			bool reverse = boardSquarePathInfo.m_reverse;
			bool unskippable = boardSquarePathInfo.m_unskippable;
			bool isEnd = boardSquarePathInfo.next == null;
			bool visibleToEnemies = boardSquarePathInfo.m_visibleToEnemies;
			bool updateLastKnownPos = boardSquarePathInfo.m_updateLastKnownPos;
			bool moverDiesHere = boardSquarePathInfo.m_moverDiesHere;
			bool overrideSpeed = !Mathf.Approximately(boardSquarePathInfo.segmentMovementSpeed, segmentMovementSpeed);
			bool overrideDuration = !Mathf.Approximately(boardSquarePathInfo.segmentMovementDuration, segmentMovementDuration);
			bool moverClashesHere = boardSquarePathInfo.m_moverClashesHere;
			bool moverBumpedFromClash = boardSquarePathInfo.m_moverBumpedFromClash;
			byte bitfield1 = ServerClientUtils.CreateBitfieldFromBools(reverse, unskippable, isEnd, visibleToEnemies, updateLastKnownPos, moverDiesHere, overrideSpeed, overrideDuration);
			byte bitfield2 = ServerClientUtils.CreateBitfieldFromBools(moverClashesHere, moverBumpedFromClash, false, false, false, false, false, false);
			stream.Serialize(ref x);
			stream.Serialize(ref y);
			stream.Serialize(ref connectionType);
			if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Run
				&& boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Vault
				&& boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Knockback)
			{
				stream.Serialize(ref chargeCycleType);
				stream.Serialize(ref chargeEndType);
			}
			stream.Serialize(ref bitfield1);
			stream.Serialize(ref bitfield2);
			if (overrideSpeed)
			{
				float value9 = boardSquarePathInfo.segmentMovementSpeed;
				stream.Serialize(ref value9);
			}
			if (overrideDuration)
			{
				float value10 = boardSquarePathInfo.segmentMovementDuration;
				stream.Serialize(ref value10);
			}
		}
	}

	internal static byte[] SerializePath(BoardSquarePathInfo path)
	{
		if (path == null)
		{
			return null;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		SerializePath(path, networkWriter);
		return networkWriter.ToArray();
	}

	internal static BoardSquarePathInfo DeSerializePath(IBitStream stream)
	{
		BoardSquarePathInfo start = new BoardSquarePathInfo();
		BoardSquarePathInfo next = start;
		BoardSquarePathInfo prev = null;
		byte x = 0;
		byte y = 0;
		float moveCost = 0f;
		sbyte connectionType = 0;
		sbyte chargeCycleType = 0;
		sbyte chargeEndType = 0;
		float _segmentMovementSpeed = 0f;
		float _segmentMovementDuration = 0f;
		float _moveCost = 0f;
		bool reverse = false;
		bool unskippable = false;
		bool isPathNotEmpty = false;
		stream.Serialize(ref isPathNotEmpty);
		if (isPathNotEmpty)
		{
			stream.Serialize(ref _segmentMovementSpeed);
			stream.Serialize(ref _segmentMovementDuration);
			stream.Serialize(ref _moveCost);
		}
		bool isEnd = !isPathNotEmpty;
		bool visibleToEnemies = false;
		bool updateLastKnownPos = false;
		bool moverDiesHere = false;
		bool moverClashesHere = false;
		bool moverBumpedFromClash = false;
		byte bitfield1 = 0;
		byte bitfield2 = 0;
		while (!isEnd)
		{
			stream.Serialize(ref x);
			stream.Serialize(ref y);
			stream.Serialize(ref connectionType);
			if (connectionType != 0 && connectionType != 3 && connectionType != 1)
			{
				stream.Serialize(ref chargeCycleType);
				stream.Serialize(ref chargeEndType);
			}
			stream.Serialize(ref bitfield1);
			stream.Serialize(ref bitfield2);
			bool overrideSpeed = false;
			bool overrideDuration = false;
			ServerClientUtils.GetBoolsFromBitfield(bitfield1, out reverse, out unskippable, out isEnd, out visibleToEnemies, out updateLastKnownPos, out moverDiesHere, out overrideSpeed, out overrideDuration);
			ServerClientUtils.GetBoolsFromBitfield(bitfield2, out moverClashesHere, out moverBumpedFromClash);
			float segmentMovementSpeed = _segmentMovementSpeed;
			float segmentMovementDuration = _segmentMovementDuration;
			if (overrideSpeed)
			{
				stream.Serialize(ref segmentMovementSpeed);
			}
			if (overrideDuration)
			{
				stream.Serialize(ref segmentMovementDuration);
			}
			BoardSquare square = Board.Get().GetSquareFromIndex(x, y);
			if (square == null)
			{
				Log.Error(new StringBuilder().Append("Failed to find square from index [").Append(x).Append(", ").Append(y).Append("] during serialization of path").ToString());
			}
			next.square = square;
			next.moveCost = moveCost;
			next.heuristicCost = 0f;
			next.connectionType = (BoardSquarePathInfo.ConnectionType)connectionType;
			next.chargeCycleType = (BoardSquarePathInfo.ChargeCycleType)chargeCycleType;
			next.chargeEndType = (BoardSquarePathInfo.ChargeEndType)chargeEndType;
			next.segmentMovementSpeed = segmentMovementSpeed;
			next.segmentMovementDuration = segmentMovementDuration;
			next.m_reverse = reverse;
			next.m_unskippable = unskippable;
			next.m_visibleToEnemies = visibleToEnemies;
			next.m_updateLastKnownPos = updateLastKnownPos;
			next.m_moverDiesHere = moverDiesHere;
			next.m_moverClashesHere = moverClashesHere;
			next.m_moverBumpedFromClash = moverBumpedFromClash;
			next.prev = prev;
			if (prev != null)
			{
				prev.next = next;
			}
			isEnd = (isEnd || !stream.isReading);
			if (!isEnd)
			{
				prev = next;
				next = new BoardSquarePathInfo();
			}
		}
		start.moveCost = _moveCost;
		start.CalcAndSetMoveCostToEnd();
		return start;
	}

	internal static BoardSquarePathInfo DeSerializePath(NetworkReader reader)
	{
		BoardSquarePathInfo start = new BoardSquarePathInfo();
		BoardSquarePathInfo next = start;
		BoardSquarePathInfo prev = null;
		float moveCost = 0f;
		sbyte chargeCycleType = 0;
		sbyte chargeEndType = 0;
		float _segmentMovementSpeed = 0f;
		float _segmentMovementDuration = 0f;
		float _moveCost = 0f;
		bool reverse = false;
		bool unskippable = false;
		bool flag = reader.ReadBoolean();
		if (flag)
		{
			_segmentMovementSpeed = reader.ReadSingle();
			_segmentMovementDuration = reader.ReadSingle();
			_moveCost = reader.ReadSingle();
		}
		bool isEnd = !flag;
		bool visibleToEnemies = false;
		bool updateLastKnownPos = false;
		bool moverDiesHere = false;
		bool moverClashesHere = false;
		bool moverBumpedFromClash = false;
		while (!isEnd)
		{
			byte x = reader.ReadByte();
			byte y = reader.ReadByte();
			sbyte connectionType = reader.ReadSByte();
			if (connectionType != 0 && connectionType != 3 && connectionType != 1)
			{
				chargeCycleType = reader.ReadSByte();
				chargeEndType = reader.ReadSByte();
			}
			byte bitfield1 = reader.ReadByte();
			byte bitfield2 = reader.ReadByte();
			bool overrideSpeed = false;
			bool overrideDuration = false;
			ServerClientUtils.GetBoolsFromBitfield(bitfield1, out reverse, out unskippable, out isEnd, out visibleToEnemies, out updateLastKnownPos, out moverDiesHere, out overrideSpeed, out overrideDuration);
			ServerClientUtils.GetBoolsFromBitfield(bitfield2, out moverClashesHere, out moverBumpedFromClash);
			float segmentMovementSpeed = _segmentMovementSpeed;
			float segmentMovementDuration = _segmentMovementDuration;
			if (overrideSpeed)
			{
				segmentMovementSpeed = reader.ReadSingle();
			}
			if (overrideDuration)
			{
				segmentMovementDuration = reader.ReadSingle();
			}
			BoardSquare square = Board.Get().GetSquareFromIndex(x, y);
			if (square == null)
			{
				Log.Error(new StringBuilder().Append("Failed to find square from index [").Append(x).Append(", ").Append(y).Append("] during serialization of path").ToString());
			}
			next.square = square;
			next.moveCost = moveCost;
			next.heuristicCost = 0f;
			next.connectionType = (BoardSquarePathInfo.ConnectionType)connectionType;
			next.chargeCycleType = (BoardSquarePathInfo.ChargeCycleType)chargeCycleType;
			next.chargeEndType = (BoardSquarePathInfo.ChargeEndType)chargeEndType;
			next.segmentMovementSpeed = segmentMovementSpeed;
			next.segmentMovementDuration = segmentMovementDuration;
			next.m_reverse = reverse;
			next.m_unskippable = unskippable;
			next.m_visibleToEnemies = visibleToEnemies;
			next.m_updateLastKnownPos = updateLastKnownPos;
			next.m_moverDiesHere = moverDiesHere;
			next.m_moverClashesHere = moverClashesHere;
			next.m_moverBumpedFromClash = moverBumpedFromClash;
			next.prev = prev;
			if (prev != null)
			{
				prev.next = next;
			}
			if (!isEnd)
			{
				prev = next;
				next = new BoardSquarePathInfo();
			}
		}
		start.moveCost = _moveCost;
		start.CalcAndSetMoveCostToEnd();
		return start;
	}

	internal static BoardSquarePathInfo DeSerializePath(byte[] data)
	{
		if (data == null)
		{
			return null;
		}
		return DeSerializePath(new NetworkReader(data));
	}

	internal static void SerializeLightweightPath(BoardSquarePathInfo path, NetworkWriter stream)
	{
		SerializeLightweightPath(path, new NetworkWriterAdapter(stream));
	}

	internal static void SerializeLightweightPath(BoardSquarePathInfo path, IBitStream stream)
	{
		if (stream == null)
		{
			Debug.LogError("Calling SerializeLightweightPath with a null stream");
			return;
		}
		if (stream.isReading)
		{
			Log.Error("Trying to serialize a (lightweight) path while reading");
			return;
		}
		uint position = stream.Position;
		if (path == null)
		{
			sbyte value = 0;
			stream.Serialize(ref value);
		}
		else
		{
			sbyte value2 = 0;
			for (BoardSquarePathInfo it = path; it != null; it = it.next)
			{
				value2 += 1;
			}
			sbyte value3 = 0;
			for (BoardSquarePathInfo it = path.prev; it != null; it = it.prev)
			{
				value3 += 1;
			}
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			int num = 0;
			BoardSquarePathInfo boardSquarePathInfo = path;
			for (int i = 0; i < value2; i++)
			{
				short x;
				short y;
				if (boardSquarePathInfo.square != null)
				{
					x = (short)boardSquarePathInfo.square.x;
					y = (short)boardSquarePathInfo.square.y;
				}
				else
				{
					x = -1;
					y = -1;
					num++;
				}
				stream.Serialize(ref x);
				stream.Serialize(ref y);
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			boardSquarePathInfo = path.prev;
			for (int j = 0; j < value3; j++)
			{
				short x;
				short y;
				if (boardSquarePathInfo.square != null)
				{
					x = (short)boardSquarePathInfo.square.x;
					y = (short)boardSquarePathInfo.square.y;
				}
				else
				{
					x = -1;
					y = -1;
					num++;
				}
				stream.Serialize(ref x);
				stream.Serialize(ref y);
				boardSquarePathInfo = boardSquarePathInfo.prev;
			}
			if (num > 0)
			{
				Debug.LogError(new StringBuilder().Append("Calling SerializeLightweightPath with a path that has ").Append(num).Append(" null square(s).").ToString());
			}
		}
		uint numBytes = stream.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning(new StringBuilder().Append("\t\t\t Serializing Lightweight Movement Path: \n\t\t\t numBytes: ").Append(numBytes).ToString());
		}
	}

	internal static BoardSquarePathInfo DeSerializeLightweightPath(IBitStream stream)
	{
		if (stream == null)
		{
			Debug.LogError("Calling DeSerializeLightweightPath with a null stream");
			return null;
		}
		BoardSquarePathInfo boardSquarePathInfo;
		if (stream.isWriting)
		{
			Log.Error("Trying to deserialize a (lightweight) path while writing.");
			return null;
		}
		sbyte value = 0;
		stream.Serialize(ref value);
		if (value <= 0)
		{
			boardSquarePathInfo = null;
		}
		else
		{
			sbyte value2 = 0;
			stream.Serialize(ref value2);
			boardSquarePathInfo = null;
			BoardSquarePathInfo boardSquarePathInfo3 = null;
			for (int i = 0; i < value; i++)
			{
				short value3 = -1;
				short value4 = -1;
				stream.Serialize(ref value3);
				stream.Serialize(ref value4);
				BoardSquare square;
				if (value3 == -1 && value4 == -1)
				{
					square = null;
				}
				else
				{
					square = Board.Get().GetSquareFromIndex(value3, value4);
				}
				BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
				boardSquarePathInfo2.square = square;
				boardSquarePathInfo2.prev = boardSquarePathInfo3;
				if (boardSquarePathInfo3 != null)
				{
					boardSquarePathInfo3.next = boardSquarePathInfo2;
				}
				boardSquarePathInfo3 = boardSquarePathInfo2;
				if (i == 0)
				{
					boardSquarePathInfo = boardSquarePathInfo2;
				}
			}
			BoardSquarePathInfo boardSquarePathInfo4 = boardSquarePathInfo;
			for (int j = 0; j < value2; j++)
			{
				short x = -1;
				short y = -1;
				stream.Serialize(ref x);
				stream.Serialize(ref y);
				BoardSquare square2;
				if (x != -1 || y != -1)
				{
					square2 = Board.Get().GetSquareFromIndex(x, y);
				}
				else
				{
					square2 = null;
				}
				BoardSquarePathInfo boardSquarePathInfo5 = new BoardSquarePathInfo
				{
					square = square2,
					next = boardSquarePathInfo4
				};
				boardSquarePathInfo4.prev = boardSquarePathInfo5;
				boardSquarePathInfo4 = boardSquarePathInfo5;
			}
		}
		return boardSquarePathInfo;
	}

	public static BoardSquarePathInfo CreateClonePathEndingAt(BoardSquarePathInfo desiredEnding)
	{
		BoardSquarePathInfo pathStartPoint = desiredEnding.GetPathStartPoint();
		BoardSquarePathInfo boardSquarePathInfo = pathStartPoint.Clone(null);
		BoardSquarePathInfo boardSquarePathInfo2 = null;
		BoardSquarePathInfo boardSquarePathInfo3 = pathStartPoint;
		BoardSquarePathInfo boardSquarePathInfo4 = boardSquarePathInfo;
		while (boardSquarePathInfo3 != null)
		{
			if (boardSquarePathInfo4 == null || boardSquarePathInfo4.square != boardSquarePathInfo3.square)
			{
				Debug.LogError("CreateClonePathEndingAt somehow has a bad clone...?  Tell Danny");
				break;
			}
			if (boardSquarePathInfo3 == desiredEnding)
			{
				boardSquarePathInfo2 = boardSquarePathInfo4;
				break;
			}
			boardSquarePathInfo3 = boardSquarePathInfo3.next;
			boardSquarePathInfo4 = boardSquarePathInfo4.next;
		}
		if (boardSquarePathInfo2 != null)
		{
			boardSquarePathInfo2.next = null;
		}
		return boardSquarePathInfo2;
	}

	public static bool ShouldVault(BoardSquare srcSquare, BoardSquare destSquare)
	{
		if (Board.Get().GetSquaresAreDiagonallyAdjacent(srcSquare, destSquare))
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(srcSquare.GetGridPos().x, destSquare.GetGridPos().y);
			BoardSquare boardSquare2 = Board.Get().GetSquareFromIndex(destSquare.GetGridPos().x, srcSquare.GetGridPos().y);
			if (srcSquare.GetThinCover(VectorUtils.GetCoverDirection(srcSquare, boardSquare)) == ThinCover.CoverType.Half
				|| srcSquare.GetThinCover(VectorUtils.GetCoverDirection(srcSquare, boardSquare2)) == ThinCover.CoverType.Half
				|| destSquare.GetThinCover(VectorUtils.GetCoverDirection(destSquare, boardSquare)) == ThinCover.CoverType.Half
				|| destSquare.GetThinCover(VectorUtils.GetCoverDirection(destSquare, boardSquare2)) == ThinCover.CoverType.Half)
			{
				return true;
			}
		}
		else
		{
			return srcSquare.GetThinCover(VectorUtils.GetCoverDirection(srcSquare, destSquare)) == ThinCover.CoverType.Half;
		}
		return false;
	}

	public static void CalculateVaultConnectionTypes(ref BoardSquarePathInfo path)
	{
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			BoardSquarePathInfo prev = boardSquarePathInfo.prev;
			if (prev == null)
			{
				boardSquarePathInfo.connectionType = BoardSquarePathInfo.ConnectionType.Run;
			}
			else
			{
				BoardSquare square = prev.square;
				BoardSquare square2 = boardSquarePathInfo.square;
				if (ShouldVault(square, square2))
				{
					boardSquarePathInfo.connectionType = BoardSquarePathInfo.ConnectionType.Vault;
				}
				else
				{
					boardSquarePathInfo.connectionType = BoardSquarePathInfo.ConnectionType.Run;
				}
			}
		}
	}

	public static bool MovingOverHole(BoardSquare src, BoardSquare dst)
	{
		foreach (BoardSquare current in Board.Get().GetSquaresBoundedBy(src, dst))
		{
			if (current.height < Board.Get().BaselineHeight)
			{
				return true;
			}
		}
		return false;
	}

	private static bool CanRunDirectly(BoardSquare src, BoardSquare dst, ActorData mover)
	{
		LayerMask mask = (1 << LayerMask.NameToLayer("LineOfSight")) | (1 << LayerMask.NameToLayer("DynamicLineOfSight"));
		Vector3 b = new Vector3(0f, 0.5f, 0f);
		Vector3 vector = src.ToVector3() + b;
		Vector3 vector2 = dst.ToVector3() + b - vector;
		Vector3 b2 = Vector3.Cross(vector2, new Vector3(0f, 1f, 0f)).normalized * 0.5f;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		RaycastHit hitInfo;
		return !MovingOverHole(src, dst)
		       && !Physics.Raycast(vector + b2, vector2, out hitInfo, magnitude, mask)
		       && !Physics.Raycast(vector, vector2, out hitInfo, magnitude, mask)
		       && !Physics.Raycast(vector - b2, vector2, out hitInfo, magnitude, mask)
		       && !BarrierManager.Get().IsMovementBlocked(mover, src, dst);
	}

	public static void CreateUnskippableAestheticPath(ref BoardSquarePathInfo path, ActorData.MovementType movementType)
	{
		BoardSquarePathInfo prev = null;
		BoardSquarePathInfo.ConnectionType connectionType;
		switch (movementType)
		{
			case ActorData.MovementType.Charge:
				connectionType = BoardSquarePathInfo.ConnectionType.Charge;
				break;
			case ActorData.MovementType.Flight:
				connectionType = BoardSquarePathInfo.ConnectionType.Flight;
				break;
			case ActorData.MovementType.WaypointFlight:
				connectionType = BoardSquarePathInfo.ConnectionType.Charge;
				break;
			default:
				connectionType = BoardSquarePathInfo.ConnectionType.Knockback;
				break;
		}
		path.connectionType = connectionType;
		for (BoardSquarePathInfo next = path; next != null; next = next.next)
		{
			bool isRunOrVault = next.connectionType == BoardSquarePathInfo.ConnectionType.Run
				|| next.connectionType == BoardSquarePathInfo.ConnectionType.Vault;
			if (next != path
				&& (next.m_unskippable
					|| next.m_moverClashesHere && isRunOrVault
					|| next.next == null))
			{
				if (prev != null)
				{
					prev.next = next;
					next.prev = prev;
				}
				else
				{
					path = next;
					path.prev = null;
				}
				prev = next;
				prev.connectionType = connectionType;
			}
		}
	}

	public static void CreateRunAndVaultAestheticPath(ref BoardSquarePathInfo path, ActorData mover)
	{
		CalculateVaultConnectionTypes(ref path);
		BoardSquarePathInfo a = null;
		BoardSquarePathInfo b = null;
		BoardSquarePathInfo c = null;
		if (path != null)
		{
			a = path;
		}
		if (a != null)
		{
			b = path.next;
		}
		if (b != null)
		{
			c = path.next.next;
		}
		while (a != null && b != null && c != null)
		{
			bool isRunOrVault = b.connectionType == BoardSquarePathInfo.ConnectionType.Run
				|| b.connectionType == BoardSquarePathInfo.ConnectionType.Vault;
			bool isSkippable = !b.m_unskippable && (!isRunOrVault || !b.m_moverClashesHere);
			bool isValidShortcut = CanRunDirectly(a.square, c.square, mover);
			bool isABRun = a.connectionType == BoardSquarePathInfo.ConnectionType.Run
				&& b.connectionType == BoardSquarePathInfo.ConnectionType.Run;
			bool isBCRun = b.connectionType == BoardSquarePathInfo.ConnectionType.Run
				&& c.connectionType == BoardSquarePathInfo.ConnectionType.Run;
			if (isSkippable && isValidShortcut && isABRun && isBCRun)
			{
				a.next = c;
				c.prev = a;
				b = a.next;
				c = c.next;
			}
			else
			{
				a = a.next;
				b = b.next;
				c = c.next;
			}
		}
	}

	public static int GetLinkType(BoardSquarePathInfo info)
	{
		MoveState.LinkType result = MoveState.LinkType.None;
		if (info != null)
		{
			switch (info.connectionType)
			{
				case BoardSquarePathInfo.ConnectionType.Run:
					result = MoveState.LinkType.Run;
					break;
				case BoardSquarePathInfo.ConnectionType.Charge:
					result = MoveState.LinkType.Charge;
					break;
				case BoardSquarePathInfo.ConnectionType.Knockback:
					result = MoveState.LinkType.KnockBack;
					break;
				case BoardSquarePathInfo.ConnectionType.Vault:
					result = MoveState.LinkType.Vault;
					break;
			}
		}
		return (int)result;
	}

	public static BoardSquarePathInfo Build2PointTeleportPath(ActorData mover, BoardSquare destination)
	{
		return Build2PointTeleportPath(mover.GetCurrentBoardSquare(), destination);
	}

	public static BoardSquarePathInfo Build2PointTeleportPath(BoardSquare start, BoardSquare destination)
	{
		BoardSquarePathInfo a = new BoardSquarePathInfo
		{
			square = start,
			connectionType = BoardSquarePathInfo.ConnectionType.Teleport
		};
		BoardSquarePathInfo b = new BoardSquarePathInfo
		{
			square = destination,
			connectionType = BoardSquarePathInfo.ConnectionType.Teleport
		};
		a.next = b;
		b.prev = a;
		a.moveCost = 0f;
		if (start == null)
		{
			b.moveCost = 0f;
		}
		else
		{
			b.moveCost = start.HorizontalDistanceOnBoardTo(destination);
		}
		return a;
	}

	private static BoardSquare GetClosestAdjacentSquareTo(BoardSquare currentSquare, BoardSquare originalSquare, BoardSquare destinationSquare)
	{
		Vector2 a = new Vector2(destinationSquare.x, destinationSquare.y);
		Vector2 a2 = new Vector2(a.x - currentSquare.x, a.y - currentSquare.y);
		float sqrMagnitude = a2.sqrMagnitude;
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i != 0 || j != 0)
				{
					BoardSquare boardSquare = Board.Get().GetSquareFromIndex(currentSquare.x + i, currentSquare.y + j);
					if (boardSquare != null)
					{
						Vector2 vector3 = new Vector2(a.x - boardSquare.x, a.y - boardSquare.y);
						if (vector3.sqrMagnitude < sqrMagnitude)
						{
							list.Add(boardSquare);
						}
					}
				}
			}
		}
		float num = 99999.9f;
		BoardSquare result = null;
		Vector2 vector = new Vector2(originalSquare.x, originalSquare.y);
		Vector2 vector2 = a - vector;
		vector2.Normalize();
		foreach (BoardSquare current in list)
		{
			Vector2 vector3 = new Vector2(current.x, current.y);
			float d = Vector2.Dot(vector3 - vector, vector2);
			float magnitude = (vector + vector2 * d - vector3).magnitude;
			if (magnitude < num)
			{
				num = magnitude;
				result = current;
			}
		}
		return result;
	}

	public static bool CanStopOnSquare(BoardSquare square)
	{
		return square != null && square.height >= 0;
	}

	public static bool ArePathSegmentsEquivalent_ForwardAndBackward(BoardSquarePathInfo pathA, BoardSquarePathInfo pathB)
	{
		if (pathA == null
			|| pathB == null
			|| pathA.square == null
			|| pathB.square == null
			|| pathA.square != pathB.square)
		{
			return false;
		}

		bool isEqForward = true;
		bool isEqBackward = true;
		BoardSquarePathInfo a1 = pathA;
		BoardSquarePathInfo b1 = pathB;
		int i = 0;
		int j = 0;
		for (; i < 100 && isEqForward; i++)
		{
			a1 = a1.next;
			b1 = b1.next;
			if (a1 == null && b1 == null)
			{
				break;
			}
			if (a1 == null && b1 != null)
			{
				isEqForward = false;
			}
			else if (a1 != null && b1 == null)
			{
				isEqForward = false;
			}
			else if (a1.square != b1.square)
			{
				isEqForward = false;
			}
		}
		if (isEqForward)
		{
			BoardSquarePathInfo a2 = pathA;
			BoardSquarePathInfo b2 = pathB;
			for (; j < 100 && isEqBackward; j++)
			{
				a2 = a2.prev;
				b2 = b2.prev;
				if (a2 == null && b2 == null)
				{
					break;
				}
				if (a2 == null && b2 != null)
				{
					isEqBackward = false;
				}
				else if (a2 != null && b2 == null)
				{
					isEqBackward = false;
				}
				else if (a2.square != b2.square)
				{
					isEqBackward = false;
				}
			}
		}
		if (i >= 100 || j >= 100)
		{
			Debug.LogError("Infinite/circular (or maybe just massive) loop detected in ArePathSegmentsEquivalent_ForwardAndBackward.");
		}
		return isEqForward && isEqBackward;
	}

	public static bool ArePathSegmentsEquivalent_FromBeginning(BoardSquarePathInfo pathA, BoardSquarePathInfo pathB)
	{
		if (pathA == null
			|| pathB == null
			|| pathA.square == null
			|| pathB.square == null
			|| pathA.square != pathB.square)
		{
			return false;
		}

		bool result = true;
		BoardSquarePathInfo a = pathA;
		BoardSquarePathInfo b = pathB;
		int i;
		for (i = 0; i < 100 && result; i++)
		{
			a = a.prev;
			b = b.prev;
			if (a == null && b == null)
			{
				break;
			}
			if (a == null && b != null)
			{
				result = false;
			}
			else if (a != null && b == null)
			{
				result = false;
			}
			else if (a.square != b.square)
			{
				result = false;
			}
		}
		if (i >= 100)
		{
			Debug.LogError("Infinite/circular (or maybe just massive) loop detected in ArePathSegmentsEquivalent_FromBeginning.");
		}
		return result;
	}

	public static float RoundToNearestHalf(float val)
	{
		return Mathf.Round(val * 2f) / 2f;
	}
}
