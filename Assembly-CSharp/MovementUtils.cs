using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class MovementUtils
{
	internal static void SerializePath(BoardSquarePathInfo path, NetworkWriter writer)
	{
		bool flag = path != null;
		float b = 8f;
		float b2 = 0f;
		writer.Write(flag);
		if (flag)
		{
			b = path.segmentMovementSpeed;
			b2 = path.segmentMovementDuration;
			writer.Write(path.segmentMovementSpeed);
			writer.Write(path.segmentMovementDuration);
			writer.Write(path.moveCost);
		}
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			byte value = 0;
			if (boardSquarePathInfo.square.x <= 255)
			{
				value = (byte)boardSquarePathInfo.square.x;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, x coordinate value too large for byte");
			}
			byte value2 = 0;
			if (boardSquarePathInfo.square.y <= 255)
			{
				value2 = (byte)boardSquarePathInfo.square.y;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, y coordinate value too large for byte");
			}
			sbyte value3 = (sbyte)boardSquarePathInfo.connectionType;
			sbyte value4 = (sbyte)boardSquarePathInfo.chargeCycleType;
			sbyte value5 = (sbyte)boardSquarePathInfo.chargeEndType;
			bool reverse = boardSquarePathInfo.m_reverse;
			bool unskippable = boardSquarePathInfo.m_unskippable;
			bool b3 = boardSquarePathInfo.next == null;
			bool visibleToEnemies = boardSquarePathInfo.m_visibleToEnemies;
			bool updateLastKnownPos = boardSquarePathInfo.m_updateLastKnownPos;
			bool moverDiesHere = boardSquarePathInfo.m_moverDiesHere;
			bool flag2 = !Mathf.Approximately(boardSquarePathInfo.segmentMovementSpeed, b);
			bool flag3 = !Mathf.Approximately(boardSquarePathInfo.segmentMovementDuration, b2);
			bool moverClashesHere = boardSquarePathInfo.m_moverClashesHere;
			bool moverBumpedFromClash = boardSquarePathInfo.m_moverBumpedFromClash;
			byte value6 = ServerClientUtils.CreateBitfieldFromBools(reverse, unskippable, b3, visibleToEnemies, updateLastKnownPos, moverDiesHere, flag2, flag3);
			byte value7 = ServerClientUtils.CreateBitfieldFromBools(moverClashesHere, moverBumpedFromClash, false, false, false, false, false, false);
			writer.Write(value);
			writer.Write(value2);
			writer.Write(value3);
			int num;
			if (boardSquarePathInfo.connectionType != 0)
			{
				if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Vault)
				{
					num = ((boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Knockback) ? 1 : 0);
					goto IL_01fd;
				}
			}
			num = 1;
			goto IL_01fd;
			IL_01fd:
			if (num == 0)
			{
				writer.Write(value4);
				writer.Write(value5);
			}
			writer.Write(value6);
			writer.Write(value7);
			if (flag2)
			{
				float segmentMovementSpeed = boardSquarePathInfo.segmentMovementSpeed;
				writer.Write(segmentMovementSpeed);
			}
			if (flag3)
			{
				float segmentMovementDuration = boardSquarePathInfo.segmentMovementDuration;
				writer.Write(segmentMovementDuration);
			}
		}
	}

	internal static void SerializePath(BoardSquarePathInfo path, IBitStream stream)
	{
		if (stream.isReading)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("Trying to serialize a path while reading");
					return;
				}
			}
		}
		bool value = path != null;
		float b = 8f;
		float b2 = 0f;
		stream.Serialize(ref value);
		if (value)
		{
			b = path.segmentMovementSpeed;
			b2 = path.segmentMovementDuration;
			stream.Serialize(ref path.segmentMovementSpeed);
			stream.Serialize(ref path.segmentMovementDuration);
			stream.Serialize(ref path.moveCost);
		}
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			byte value2 = 0;
			if (boardSquarePathInfo.square.x <= 255)
			{
				value2 = (byte)boardSquarePathInfo.square.x;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, x coordinate value too large for byte");
			}
			byte value3 = 0;
			if (boardSquarePathInfo.square.y <= 255)
			{
				value3 = (byte)boardSquarePathInfo.square.y;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, y coordinate value too large for byte");
			}
			sbyte value4 = (sbyte)boardSquarePathInfo.connectionType;
			sbyte value5 = (sbyte)boardSquarePathInfo.chargeCycleType;
			sbyte value6 = (sbyte)boardSquarePathInfo.chargeEndType;
			bool reverse = boardSquarePathInfo.m_reverse;
			bool unskippable = boardSquarePathInfo.m_unskippable;
			bool b3 = boardSquarePathInfo.next == null;
			bool visibleToEnemies = boardSquarePathInfo.m_visibleToEnemies;
			bool updateLastKnownPos = boardSquarePathInfo.m_updateLastKnownPos;
			bool moverDiesHere = boardSquarePathInfo.m_moverDiesHere;
			bool flag = !Mathf.Approximately(boardSquarePathInfo.segmentMovementSpeed, b);
			bool flag2 = !Mathf.Approximately(boardSquarePathInfo.segmentMovementDuration, b2);
			bool moverClashesHere = boardSquarePathInfo.m_moverClashesHere;
			bool moverBumpedFromClash = boardSquarePathInfo.m_moverBumpedFromClash;
			byte value7 = ServerClientUtils.CreateBitfieldFromBools(reverse, unskippable, b3, visibleToEnemies, updateLastKnownPos, moverDiesHere, flag, flag2);
			byte value8 = ServerClientUtils.CreateBitfieldFromBools(moverClashesHere, moverBumpedFromClash, false, false, false, false, false, false);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			int num;
			if (boardSquarePathInfo.connectionType != 0)
			{
				if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Vault)
				{
					num = ((boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Knockback) ? 1 : 0);
					goto IL_0201;
				}
			}
			num = 1;
			goto IL_0201;
			IL_0201:
			if (num == 0)
			{
				stream.Serialize(ref value5);
				stream.Serialize(ref value6);
			}
			stream.Serialize(ref value7);
			stream.Serialize(ref value8);
			if (flag)
			{
				float value9 = boardSquarePathInfo.segmentMovementSpeed;
				stream.Serialize(ref value9);
			}
			if (flag2)
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		SerializePath(path, networkWriter);
		return networkWriter.ToArray();
	}

	internal static BoardSquarePathInfo DeSerializePath(IBitStream stream)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		BoardSquarePathInfo boardSquarePathInfo3 = null;
		byte value = 0;
		byte value2 = 0;
		float moveCost = 0f;
		sbyte value3 = 0;
		sbyte value4 = 0;
		sbyte value5 = 0;
		float value6 = 0f;
		float value7 = 0f;
		float value8 = 0f;
		bool @out = false;
		bool out2 = false;
		bool value9 = false;
		stream.Serialize(ref value9);
		if (value9)
		{
			stream.Serialize(ref value6);
			stream.Serialize(ref value7);
			stream.Serialize(ref value8);
		}
		bool out3 = !value9;
		bool out4 = false;
		bool out5 = false;
		bool out6 = false;
		bool out7 = false;
		bool out8 = false;
		byte value10 = 0;
		byte value11 = 0;
		while (!out3)
		{
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			int num;
			if (value3 != 0)
			{
				if (value3 != 3)
				{
					num = ((value3 == 1) ? 1 : 0);
					goto IL_00cb;
				}
			}
			num = 1;
			goto IL_00cb;
			IL_00cb:
			if (num == 0)
			{
				stream.Serialize(ref value4);
				stream.Serialize(ref value5);
			}
			stream.Serialize(ref value10);
			stream.Serialize(ref value11);
			bool out9 = false;
			bool out10 = false;
			ServerClientUtils.GetBoolsFromBitfield(value10, out @out, out out2, out out3, out out4, out out5, out out6, out out9, out out10);
			ServerClientUtils.GetBoolsFromBitfield(value11, out out7, out out8);
			float value12 = value6;
			float value13 = value7;
			if (out9)
			{
				stream.Serialize(ref value12);
			}
			if (out10)
			{
				stream.Serialize(ref value13);
			}
			BoardSquare boardSquare = Board.Get().GetSquare(value, value2);
			if (boardSquare == null)
			{
				Log.Error("Failed to find square from index [" + value + ", " + value2 + "] during serialization of path");
			}
			boardSquarePathInfo2.square = boardSquare;
			boardSquarePathInfo2.moveCost = moveCost;
			boardSquarePathInfo2.heuristicCost = 0f;
			boardSquarePathInfo2.connectionType = (BoardSquarePathInfo.ConnectionType)value3;
			boardSquarePathInfo2.chargeCycleType = (BoardSquarePathInfo.ChargeCycleType)value4;
			boardSquarePathInfo2.chargeEndType = (BoardSquarePathInfo.ChargeEndType)value5;
			boardSquarePathInfo2.segmentMovementSpeed = value12;
			boardSquarePathInfo2.segmentMovementDuration = value13;
			boardSquarePathInfo2.m_reverse = @out;
			boardSquarePathInfo2.m_unskippable = out2;
			boardSquarePathInfo2.m_visibleToEnemies = out4;
			boardSquarePathInfo2.m_updateLastKnownPos = out5;
			boardSquarePathInfo2.m_moverDiesHere = out6;
			boardSquarePathInfo2.m_moverClashesHere = out7;
			boardSquarePathInfo2.m_moverBumpedFromClash = out8;
			boardSquarePathInfo2.prev = boardSquarePathInfo3;
			if (boardSquarePathInfo3 != null)
			{
				boardSquarePathInfo3.next = boardSquarePathInfo2;
			}
			out3 = (out3 || !stream.isReading);
			if (!out3)
			{
				boardSquarePathInfo3 = boardSquarePathInfo2;
				boardSquarePathInfo2 = new BoardSquarePathInfo();
			}
		}
		while (true)
		{
			boardSquarePathInfo.moveCost = value8;
			boardSquarePathInfo.CalcAndSetMoveCostToEnd();
			return boardSquarePathInfo;
		}
	}

	internal static BoardSquarePathInfo DeSerializePath(NetworkReader reader)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		BoardSquarePathInfo boardSquarePathInfo3 = null;
		byte b = 0;
		byte b2 = 0;
		float moveCost = 0f;
		sbyte b3 = 0;
		sbyte b4 = 0;
		sbyte b5 = 0;
		float num = 0f;
		float num2 = 0f;
		float moveCost2 = 0f;
		bool @out = false;
		bool out2 = false;
		bool flag = false;
		flag = reader.ReadBoolean();
		if (flag)
		{
			num = reader.ReadSingle();
			num2 = reader.ReadSingle();
			moveCost2 = reader.ReadSingle();
		}
		bool out3 = !flag;
		bool out4 = false;
		bool out5 = false;
		bool out6 = false;
		bool out7 = false;
		bool out8 = false;
		byte b6 = 0;
		byte b7 = 0;
		while (!out3)
		{
			b = reader.ReadByte();
			b2 = reader.ReadByte();
			b3 = reader.ReadSByte();
			int num3;
			if (b3 != 0)
			{
				if (b3 != 3)
				{
					num3 = ((b3 == 1) ? 1 : 0);
					goto IL_00e2;
				}
			}
			num3 = 1;
			goto IL_00e2;
			IL_00e2:
			if (num3 == 0)
			{
				b4 = reader.ReadSByte();
				b5 = reader.ReadSByte();
			}
			b6 = reader.ReadByte();
			b7 = reader.ReadByte();
			bool out9 = false;
			bool out10 = false;
			ServerClientUtils.GetBoolsFromBitfield(b6, out @out, out out2, out out3, out out4, out out5, out out6, out out9, out out10);
			ServerClientUtils.GetBoolsFromBitfield(b7, out out7, out out8);
			float segmentMovementSpeed = num;
			float segmentMovementDuration = num2;
			if (out9)
			{
				segmentMovementSpeed = reader.ReadSingle();
			}
			if (out10)
			{
				segmentMovementDuration = reader.ReadSingle();
			}
			BoardSquare boardSquare = Board.Get().GetSquare(b, b2);
			if (boardSquare == null)
			{
				Log.Error("Failed to find square from index [" + b + ", " + b2 + "] during serialization of path");
			}
			boardSquarePathInfo2.square = boardSquare;
			boardSquarePathInfo2.moveCost = moveCost;
			boardSquarePathInfo2.heuristicCost = 0f;
			boardSquarePathInfo2.connectionType = (BoardSquarePathInfo.ConnectionType)b3;
			boardSquarePathInfo2.chargeCycleType = (BoardSquarePathInfo.ChargeCycleType)b4;
			boardSquarePathInfo2.chargeEndType = (BoardSquarePathInfo.ChargeEndType)b5;
			boardSquarePathInfo2.segmentMovementSpeed = segmentMovementSpeed;
			boardSquarePathInfo2.segmentMovementDuration = segmentMovementDuration;
			boardSquarePathInfo2.m_reverse = @out;
			boardSquarePathInfo2.m_unskippable = out2;
			boardSquarePathInfo2.m_visibleToEnemies = out4;
			boardSquarePathInfo2.m_updateLastKnownPos = out5;
			boardSquarePathInfo2.m_moverDiesHere = out6;
			boardSquarePathInfo2.m_moverClashesHere = out7;
			boardSquarePathInfo2.m_moverBumpedFromClash = out8;
			boardSquarePathInfo2.prev = boardSquarePathInfo3;
			if (boardSquarePathInfo3 != null)
			{
				boardSquarePathInfo3.next = boardSquarePathInfo2;
			}
			if (!out3)
			{
				boardSquarePathInfo3 = boardSquarePathInfo2;
				boardSquarePathInfo2 = new BoardSquarePathInfo();
			}
		}
		while (true)
		{
			boardSquarePathInfo.moveCost = moveCost2;
			boardSquarePathInfo.CalcAndSetMoveCostToEnd();
			return boardSquarePathInfo;
		}
	}

	internal static BoardSquarePathInfo DeSerializePath(byte[] data)
	{
		if (data == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		NetworkReader reader = new NetworkReader(data);
		return DeSerializePath(reader);
	}

	internal static void SerializeLightweightPath(BoardSquarePathInfo path, NetworkWriter stream)
	{
		SerializeLightweightPath(path, new NetworkWriterAdapter(stream));
	}

	internal static void SerializeLightweightPath(BoardSquarePathInfo path, IBitStream stream)
	{
		if (stream == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("Calling SerializeLightweightPath with a null stream");
					return;
				}
			}
		}
		if (stream.isReading)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error("Trying to serialize a (lightweight) path while reading");
					return;
				}
			}
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
			BoardSquarePathInfo boardSquarePathInfo;
			for (boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
			{
				value2 = (sbyte)(value2 + 1);
			}
			sbyte value3 = 0;
			for (boardSquarePathInfo = path.prev; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.prev)
			{
				value3 = (sbyte)(value3 + 1);
			}
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			int num = 0;
			boardSquarePathInfo = path;
			for (int i = 0; i < value2; i++)
			{
				short value4;
				short value5;
				if (boardSquarePathInfo.square != null)
				{
					value4 = (short)boardSquarePathInfo.square.x;
					value5 = (short)boardSquarePathInfo.square.y;
				}
				else
				{
					value4 = -1;
					value5 = -1;
					num++;
				}
				stream.Serialize(ref value4);
				stream.Serialize(ref value5);
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			boardSquarePathInfo = path.prev;
			for (int j = 0; j < value3; j++)
			{
				short value6;
				short value7;
				if (boardSquarePathInfo.square != null)
				{
					value6 = (short)boardSquarePathInfo.square.x;
					value7 = (short)boardSquarePathInfo.square.y;
				}
				else
				{
					value6 = -1;
					value7 = -1;
					num++;
				}
				stream.Serialize(ref value6);
				stream.Serialize(ref value7);
				boardSquarePathInfo = boardSquarePathInfo.prev;
			}
			if (num > 0)
			{
				Debug.LogError("Calling SerializeLightweightPath with a path that has " + num + " null square(s).");
			}
		}
		uint num2 = stream.Position - position;
		if (ClientAbilityResults._000E)
		{
			Debug.LogWarning("\t\t\t Serializing Lightweight Movement Path: \n\t\t\t numBytes: " + num2);
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
			boardSquarePathInfo = null;
		}
		else
		{
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
				BoardSquarePathInfo boardSquarePathInfo2 = null;
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
						square = Board.Get().GetSquare(value3, value4);
					}
					boardSquarePathInfo2 = new BoardSquarePathInfo();
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
				BoardSquare square2;
				for (int j = 0; j < value2; boardSquarePathInfo2 = new BoardSquarePathInfo(), boardSquarePathInfo2.square = square2, boardSquarePathInfo2.next = boardSquarePathInfo4, boardSquarePathInfo4.prev = boardSquarePathInfo2, boardSquarePathInfo4 = boardSquarePathInfo2, j++)
				{
					short value5 = -1;
					short value6 = -1;
					stream.Serialize(ref value5);
					stream.Serialize(ref value6);
					if (value5 == -1)
					{
						if (value6 == -1)
						{
							square2 = null;
							continue;
						}
					}
					square2 = Board.Get().GetSquare(value5, value6);
				}
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
		while (true)
		{
			if (boardSquarePathInfo3 != null)
			{
				if (boardSquarePathInfo4 != null)
				{
					if (!(boardSquarePathInfo4.square != boardSquarePathInfo3.square))
					{
						if (boardSquarePathInfo3 == desiredEnding)
						{
							boardSquarePathInfo2 = boardSquarePathInfo4;
							break;
						}
						boardSquarePathInfo3 = boardSquarePathInfo3.next;
						boardSquarePathInfo4 = boardSquarePathInfo4.next;
						continue;
					}
				}
				Debug.LogError("CreateClonePathEndingAt somehow has a bad clone...?  Tell Danny");
			}
			else
			{
			}
			break;
		}
		if (boardSquarePathInfo2 != null)
		{
			boardSquarePathInfo2.next = null;
		}
		return boardSquarePathInfo2;
	}

	public static bool ShouldVault(BoardSquare srcSquare, BoardSquare destSquare)
	{
		bool result = false;
		if (Board.Get()._0015(srcSquare, destSquare))
		{
			BoardSquare boardSquare = Board.Get().GetSquare(srcSquare.GetGridPos().x, destSquare.GetGridPos().y);
			BoardSquare boardSquare2 = Board.Get().GetSquare(destSquare.GetGridPos().x, srcSquare.GetGridPos().y);
			if (srcSquare.GetCoverInDirection(VectorUtils.GetCoverDirection(srcSquare, boardSquare)) == ThinCover.CoverType.Half)
			{
				result = true;
			}
			else if (srcSquare.GetCoverInDirection(VectorUtils.GetCoverDirection(srcSquare, boardSquare2)) == ThinCover.CoverType.Half)
			{
				result = true;
			}
			else if (destSquare.GetCoverInDirection(VectorUtils.GetCoverDirection(destSquare, boardSquare)) == ThinCover.CoverType.Half)
			{
				result = true;
			}
			else if (destSquare.GetCoverInDirection(VectorUtils.GetCoverDirection(destSquare, boardSquare2)) == ThinCover.CoverType.Half)
			{
				result = true;
			}
		}
		else
		{
			result = (srcSquare.GetCoverInDirection(VectorUtils.GetCoverDirection(srcSquare, destSquare)) == ThinCover.CoverType.Half);
		}
		return result;
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
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static bool MovingOverHole(BoardSquare src, BoardSquare dst)
	{
		bool result = false;
		List<BoardSquare> squaresInRect = Board.Get().GetSquaresInRect(src, dst);
		using (List<BoardSquare>.Enumerator enumerator = squaresInRect.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.height < Board.Get().BaselineHeight)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	private static bool CanRunDirectly(BoardSquare src, BoardSquare dst, ActorData mover)
	{
		bool result = false;
		LayerMask mask = (1 << LayerMask.NameToLayer("LineOfSight")) | (1 << LayerMask.NameToLayer("DynamicLineOfSight"));
		Vector3 b = new Vector3(0f, 0.5f, 0f);
		Vector3 vector = src.ToVector3() + b;
		Vector3 a = dst.ToVector3() + b;
		Vector3 vector2 = a - vector;
		Vector3 b2 = Vector3.Cross(vector2, new Vector3(0f, 1f, 0f)).normalized * 0.5f;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		if (!MovingOverHole(src, dst) && !Physics.Raycast(vector + b2, vector2, out RaycastHit hitInfo, magnitude, mask))
		{
			if (!Physics.Raycast(vector, vector2, out hitInfo, magnitude, mask))
			{
				if (!Physics.Raycast(vector - b2, vector2, out hitInfo, magnitude, mask))
				{
					if (!BarrierManager.Get().IsMovementBlocked(mover, src, dst))
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static void CreateUnskippableAestheticPath(ref BoardSquarePathInfo path, ActorData.MovementType movementType)
	{
		BoardSquarePathInfo boardSquarePathInfo = path;
		BoardSquarePathInfo boardSquarePathInfo2 = null;
		BoardSquarePathInfo.ConnectionType connectionType;
		if (movementType == ActorData.MovementType.Charge)
		{
			connectionType = BoardSquarePathInfo.ConnectionType.Charge;
		}
		else if (movementType == ActorData.MovementType.Flight)
		{
			connectionType = BoardSquarePathInfo.ConnectionType.Flight;
		}
		else if (movementType == ActorData.MovementType.WaypointFlight)
		{
			connectionType = BoardSquarePathInfo.ConnectionType.Charge;
		}
		else
		{
			connectionType = BoardSquarePathInfo.ConnectionType.Knockback;
		}
		path.connectionType = connectionType;
		for (; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			int num;
			if (boardSquarePathInfo.connectionType != 0)
			{
				num = ((boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Vault) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag = (byte)num != 0;
			if (boardSquarePathInfo == path)
			{
				continue;
			}
			if (!boardSquarePathInfo.m_unskippable)
			{
				if (boardSquarePathInfo.m_moverClashesHere)
				{
					if (flag)
					{
						goto IL_00a9;
					}
				}
				if (boardSquarePathInfo.next != null)
				{
					continue;
				}
			}
			goto IL_00a9;
			IL_00a9:
			if (boardSquarePathInfo2 != null)
			{
				boardSquarePathInfo2.next = boardSquarePathInfo;
				boardSquarePathInfo.prev = boardSquarePathInfo2;
			}
			else
			{
				path = boardSquarePathInfo;
				path.prev = null;
			}
			boardSquarePathInfo2 = boardSquarePathInfo;
			boardSquarePathInfo2.connectionType = connectionType;
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

	public static void CreateRunAndVaultAestheticPath(ref BoardSquarePathInfo path, ActorData mover)
	{
		CalculateVaultConnectionTypes(ref path);
		BoardSquarePathInfo boardSquarePathInfo = null;
		BoardSquarePathInfo boardSquarePathInfo2 = null;
		BoardSquarePathInfo boardSquarePathInfo3 = null;
		if (path != null)
		{
			boardSquarePathInfo = path;
		}
		if (boardSquarePathInfo != null)
		{
			boardSquarePathInfo2 = path.next;
		}
		if (boardSquarePathInfo2 != null)
		{
			boardSquarePathInfo3 = path.next.next;
		}
		while (true)
		{
			if (boardSquarePathInfo == null || boardSquarePathInfo2 == null)
			{
				return;
			}
			if (boardSquarePathInfo3 == null)
			{
				break;
			}
			int num;
			if (boardSquarePathInfo2.connectionType != 0)
			{
				num = ((boardSquarePathInfo2.connectionType == BoardSquarePathInfo.ConnectionType.Vault) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag = (byte)num != 0;
			int num2;
			if (!boardSquarePathInfo2.m_unskippable)
			{
				if (flag)
				{
					num2 = (boardSquarePathInfo2.m_moverClashesHere ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
			}
			else
			{
				num2 = 1;
			}
			bool flag2 = num2 == 0;
			bool flag3 = CanRunDirectly(boardSquarePathInfo.square, boardSquarePathInfo3.square, mover);
			int num3;
			if (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Run)
			{
				num3 = ((boardSquarePathInfo2.connectionType == BoardSquarePathInfo.ConnectionType.Run) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			bool flag4 = (byte)num3 != 0;
			int num4;
			if (boardSquarePathInfo2.connectionType == BoardSquarePathInfo.ConnectionType.Run)
			{
				num4 = ((boardSquarePathInfo3.connectionType == BoardSquarePathInfo.ConnectionType.Run) ? 1 : 0);
			}
			else
			{
				num4 = 0;
			}
			bool flag5 = (byte)num4 != 0;
			int num5;
			if (flag2)
			{
				if (flag3)
				{
					if (flag4)
					{
						num5 = (flag5 ? 1 : 0);
						goto IL_012e;
					}
				}
			}
			num5 = 0;
			goto IL_012e;
			IL_012e:
			if (num5 != 0)
			{
				boardSquarePathInfo.next = boardSquarePathInfo3;
				boardSquarePathInfo3.prev = boardSquarePathInfo;
				boardSquarePathInfo2 = boardSquarePathInfo.next;
				boardSquarePathInfo3 = boardSquarePathInfo3.next;
			}
			else
			{
				boardSquarePathInfo = boardSquarePathInfo.next;
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
				boardSquarePathInfo3 = boardSquarePathInfo3.next;
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
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
		BoardSquare currentBoardSquare = mover.GetCurrentBoardSquare();
		return Build2PointTeleportPath(currentBoardSquare, destination);
	}

	public static BoardSquarePathInfo Build2PointTeleportPath(BoardSquare start, BoardSquare destination)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = start;
		boardSquarePathInfo.connectionType = BoardSquarePathInfo.ConnectionType.Teleport;
		BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
		boardSquarePathInfo2.square = destination;
		boardSquarePathInfo2.connectionType = BoardSquarePathInfo.ConnectionType.Teleport;
		boardSquarePathInfo.next = boardSquarePathInfo2;
		boardSquarePathInfo2.prev = boardSquarePathInfo;
		boardSquarePathInfo.moveCost = 0f;
		if (start == null)
		{
			boardSquarePathInfo2.moveCost = 0f;
		}
		else
		{
			boardSquarePathInfo2.moveCost = start.HorizontalDistanceOnBoardTo(destination);
		}
		return boardSquarePathInfo;
	}

	private static BoardSquare GetClosestAdjacentSquareTo(BoardSquare currentSquare, BoardSquare originalSquare, BoardSquare destinationSquare)
	{
		Vector2 a = new Vector2(destinationSquare.x, destinationSquare.y);
		float sqrMagnitude = new Vector2(a.x - (float)currentSquare.x, a.y - (float)currentSquare.y).sqrMagnitude;
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0)
				{
					if (j == 0)
					{
						continue;
					}
				}
				BoardSquare boardSquare = Board.Get().GetSquare(currentSquare.x + i, currentSquare.y + j);
				if (!(boardSquare != null))
				{
					continue;
				}
				if (new Vector2(a.x - (float)boardSquare.x, a.y - (float)boardSquare.y).sqrMagnitude < sqrMagnitude)
				{
					list.Add(boardSquare);
				}
			}
		}
		while (true)
		{
			float num = 99999.9f;
			BoardSquare result = null;
			Vector2 vector = new Vector2(originalSquare.x, originalSquare.y);
			Vector2 vector2 = a - vector;
			vector2.Normalize();
			using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare current = enumerator.Current;
					Vector2 vector3 = new Vector2(current.x, current.y);
					Vector2 lhs = vector3 - vector;
					float d = Vector2.Dot(lhs, vector2);
					Vector2 a2 = vector + vector2 * d;
					float magnitude = (a2 - vector3).magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						result = current;
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return result;
					}
				}
			}
		}
	}

	public static bool CanStopOnSquare(BoardSquare square)
	{
		int result;
		if (square != null)
		{
			result = ((square.height >= 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static bool ArePathSegmentsEquivalent_ForwardAndBackward(BoardSquarePathInfo pathA, BoardSquarePathInfo pathB)
	{
		bool flag;
		bool flag2;
		if (pathA != null)
		{
			if (pathB != null)
			{
				if (!(pathA.square == null))
				{
					if (!(pathB.square == null))
					{
						if (pathA.square != pathB.square)
						{
							while (true)
							{
								return false;
							}
						}
						flag = true;
						flag2 = true;
						BoardSquarePathInfo boardSquarePathInfo = pathA;
						BoardSquarePathInfo boardSquarePathInfo2 = pathB;
						BoardSquarePathInfo boardSquarePathInfo3 = pathA;
						BoardSquarePathInfo boardSquarePathInfo4 = pathB;
						int i = 0;
						int j = 0;
						for (; flag; i++)
						{
							if (i < 100)
							{
								boardSquarePathInfo = boardSquarePathInfo.next;
								boardSquarePathInfo2 = boardSquarePathInfo2.next;
								if (boardSquarePathInfo == null)
								{
									if (boardSquarePathInfo2 == null)
									{
										break;
									}
								}
								if (boardSquarePathInfo == null)
								{
									if (boardSquarePathInfo2 != null)
									{
										flag = false;
										continue;
									}
								}
								if (boardSquarePathInfo != null && boardSquarePathInfo2 == null)
								{
									flag = false;
								}
								else if (boardSquarePathInfo.square != boardSquarePathInfo2.square)
								{
									flag = false;
								}
								continue;
							}
							break;
						}
						if (flag)
						{
							for (; flag2; j++)
							{
								if (j >= 100)
								{
									break;
								}
								boardSquarePathInfo3 = boardSquarePathInfo3.prev;
								boardSquarePathInfo4 = boardSquarePathInfo4.prev;
								if (boardSquarePathInfo3 == null)
								{
									if (boardSquarePathInfo4 == null)
									{
										break;
									}
								}
								if (boardSquarePathInfo3 == null)
								{
									if (boardSquarePathInfo4 != null)
									{
										flag2 = false;
										continue;
									}
								}
								if (boardSquarePathInfo3 != null)
								{
									if (boardSquarePathInfo4 == null)
									{
										flag2 = false;
										continue;
									}
								}
								if (boardSquarePathInfo3.square != boardSquarePathInfo4.square)
								{
									flag2 = false;
								}
							}
						}
						if (i < 100)
						{
							if (j < 100)
							{
								goto IL_020a;
							}
						}
						Debug.LogError("Infinite/circular (or maybe just massive) loop detected in ArePathSegmentsEquivalent_ForwardAndBackward.");
						goto IL_020a;
					}
				}
			}
		}
		return false;
		IL_020a:
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static bool ArePathSegmentsEquivalent_FromBeginning(BoardSquarePathInfo pathA, BoardSquarePathInfo pathB)
	{
		if (pathA != null)
		{
			if (pathB != null)
			{
				if (!(pathA.square == null))
				{
					if (!(pathB.square == null))
					{
						if (pathA.square != pathB.square)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
						}
						bool flag = true;
						BoardSquarePathInfo boardSquarePathInfo = pathA;
						BoardSquarePathInfo boardSquarePathInfo2 = pathB;
						int i;
						for (i = 0; flag; i++)
						{
							if (i >= 100)
							{
								break;
							}
							boardSquarePathInfo = boardSquarePathInfo.prev;
							boardSquarePathInfo2 = boardSquarePathInfo2.prev;
							if (boardSquarePathInfo == null && boardSquarePathInfo2 == null)
							{
								break;
							}
							if (boardSquarePathInfo == null && boardSquarePathInfo2 != null)
							{
								flag = false;
								continue;
							}
							if (boardSquarePathInfo != null)
							{
								if (boardSquarePathInfo2 == null)
								{
									flag = false;
									continue;
								}
							}
							if (boardSquarePathInfo.square != boardSquarePathInfo2.square)
							{
								flag = false;
							}
						}
						if (i >= 100)
						{
							Debug.LogError("Infinite/circular (or maybe just massive) loop detected in ArePathSegmentsEquivalent_FromBeginning.");
						}
						return flag;
					}
				}
			}
		}
		return false;
	}

	public static float RoundToNearestHalf(float val)
	{
		float f = val * 2f;
		float num = Mathf.Round(f);
		return num / 2f;
	}
}
