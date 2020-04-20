using System;
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
		BoardSquarePathInfo boardSquarePathInfo = path;
		while (boardSquarePathInfo != null)
		{
			byte value = 0;
			if (boardSquarePathInfo.square.x <= 0xFF)
			{
				value = (byte)boardSquarePathInfo.square.x;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("MovementUtils.SerializePath, x coordinate value too large for byte");
			}
			byte value2 = 0;
			if (boardSquarePathInfo.square.y <= 0xFF)
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
			if (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Run)
			{
				goto IL_1FC;
			}
			if (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Vault)
			{
				goto IL_1FC;
			}
			bool flag4 = boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Knockback;
			IL_1FD:
			if (!flag4)
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
			boardSquarePathInfo = boardSquarePathInfo.next;
			continue;
			IL_1FC:
			flag4 = true;
			goto IL_1FD;
		}
	}

	internal static void SerializePath(BoardSquarePathInfo path, IBitStream stream)
	{
		if (stream.isReading)
		{
			Log.Error("Trying to serialize a path while reading", new object[0]);
		}
		else
		{
			bool flag = path != null;
			float b = 8f;
			float b2 = 0f;
			stream.Serialize(ref flag);
			if (flag)
			{
				b = path.segmentMovementSpeed;
				b2 = path.segmentMovementDuration;
				stream.Serialize(ref path.segmentMovementSpeed);
				stream.Serialize(ref path.segmentMovementDuration);
				stream.Serialize(ref path.moveCost);
			}
			BoardSquarePathInfo boardSquarePathInfo = path;
			while (boardSquarePathInfo != null)
			{
				byte b3 = 0;
				if (boardSquarePathInfo.square.x <= 0xFF)
				{
					b3 = (byte)boardSquarePathInfo.square.x;
				}
				else if (Application.isEditor)
				{
					Debug.LogError("MovementUtils.SerializePath, x coordinate value too large for byte");
				}
				byte b4 = 0;
				if (boardSquarePathInfo.square.y <= 0xFF)
				{
					b4 = (byte)boardSquarePathInfo.square.y;
				}
				else if (Application.isEditor)
				{
					Debug.LogError("MovementUtils.SerializePath, y coordinate value too large for byte");
				}
				sbyte b5 = (sbyte)boardSquarePathInfo.connectionType;
				sbyte b6 = (sbyte)boardSquarePathInfo.chargeCycleType;
				sbyte b7 = (sbyte)boardSquarePathInfo.chargeEndType;
				bool reverse = boardSquarePathInfo.m_reverse;
				bool unskippable = boardSquarePathInfo.m_unskippable;
				bool b8 = boardSquarePathInfo.next == null;
				bool visibleToEnemies = boardSquarePathInfo.m_visibleToEnemies;
				bool updateLastKnownPos = boardSquarePathInfo.m_updateLastKnownPos;
				bool moverDiesHere = boardSquarePathInfo.m_moverDiesHere;
				bool flag2 = !Mathf.Approximately(boardSquarePathInfo.segmentMovementSpeed, b);
				bool flag3 = !Mathf.Approximately(boardSquarePathInfo.segmentMovementDuration, b2);
				bool moverClashesHere = boardSquarePathInfo.m_moverClashesHere;
				bool moverBumpedFromClash = boardSquarePathInfo.m_moverBumpedFromClash;
				byte b9 = ServerClientUtils.CreateBitfieldFromBools(reverse, unskippable, b8, visibleToEnemies, updateLastKnownPos, moverDiesHere, flag2, flag3);
				byte b10 = ServerClientUtils.CreateBitfieldFromBools(moverClashesHere, moverBumpedFromClash, false, false, false, false, false, false);
				stream.Serialize(ref b3);
				stream.Serialize(ref b4);
				stream.Serialize(ref b5);
				if (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Run)
				{
					goto IL_200;
				}
				if (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Vault)
				{
					goto IL_200;
				}
				bool flag4 = boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Knockback;
				IL_201:
				if (!flag4)
				{
					stream.Serialize(ref b6);
					stream.Serialize(ref b7);
				}
				stream.Serialize(ref b9);
				stream.Serialize(ref b10);
				if (flag2)
				{
					float segmentMovementSpeed = boardSquarePathInfo.segmentMovementSpeed;
					stream.Serialize(ref segmentMovementSpeed);
				}
				if (flag3)
				{
					float segmentMovementDuration = boardSquarePathInfo.segmentMovementDuration;
					stream.Serialize(ref segmentMovementDuration);
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
				continue;
				IL_200:
				flag4 = true;
				goto IL_201;
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
		MovementUtils.SerializePath(path, networkWriter);
		return networkWriter.ToArray();
	}

	internal static BoardSquarePathInfo DeSerializePath(IBitStream stream)
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
		bool reverse = false;
		bool unskippable = false;
		bool flag = false;
		stream.Serialize(ref flag);
		if (flag)
		{
			stream.Serialize(ref num);
			stream.Serialize(ref num2);
			stream.Serialize(ref moveCost2);
		}
		bool flag2 = !flag;
		bool visibleToEnemies = false;
		bool updateLastKnownPos = false;
		bool moverDiesHere = false;
		bool moverClashesHere = false;
		bool moverBumpedFromClash = false;
		byte bitField = 0;
		byte bitField2 = 0;
		while (!flag2)
		{
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref b3);
			if ((int)b3 == 0)
			{
				goto IL_CA;
			}
			if ((int)b3 == 3)
			{
				goto IL_CA;
			}
			bool flag3 = (int)b3 == 1;
			IL_CB:
			if (!flag3)
			{
				stream.Serialize(ref b4);
				stream.Serialize(ref b5);
			}
			stream.Serialize(ref bitField);
			stream.Serialize(ref bitField2);
			bool flag4 = false;
			bool flag5 = false;
			ServerClientUtils.GetBoolsFromBitfield(bitField, out reverse, out unskippable, out flag2, out visibleToEnemies, out updateLastKnownPos, out moverDiesHere, out flag4, out flag5);
			ServerClientUtils.GetBoolsFromBitfield(bitField2, out moverClashesHere, out moverBumpedFromClash);
			float segmentMovementSpeed = num;
			float segmentMovementDuration = num2;
			if (flag4)
			{
				stream.Serialize(ref segmentMovementSpeed);
			}
			if (flag5)
			{
				stream.Serialize(ref segmentMovementDuration);
			}
			BoardSquare boardSquare = Board.Get().GetBoardSquare((int)b, (int)b2);
			if (boardSquare == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Failed to find square from index [",
					b,
					", ",
					b2,
					"] during serialization of path"
				}), new object[0]);
			}
			boardSquarePathInfo2.square = boardSquare;
			boardSquarePathInfo2.moveCost = moveCost;
			boardSquarePathInfo2.heuristicCost = 0f;
			boardSquarePathInfo2.connectionType = (BoardSquarePathInfo.ConnectionType)b3;
			boardSquarePathInfo2.chargeCycleType = (BoardSquarePathInfo.ChargeCycleType)b4;
			boardSquarePathInfo2.chargeEndType = (BoardSquarePathInfo.ChargeEndType)b5;
			boardSquarePathInfo2.segmentMovementSpeed = segmentMovementSpeed;
			boardSquarePathInfo2.segmentMovementDuration = segmentMovementDuration;
			boardSquarePathInfo2.m_reverse = reverse;
			boardSquarePathInfo2.m_unskippable = unskippable;
			boardSquarePathInfo2.m_visibleToEnemies = visibleToEnemies;
			boardSquarePathInfo2.m_updateLastKnownPos = updateLastKnownPos;
			boardSquarePathInfo2.m_moverDiesHere = moverDiesHere;
			boardSquarePathInfo2.m_moverClashesHere = moverClashesHere;
			boardSquarePathInfo2.m_moverBumpedFromClash = moverBumpedFromClash;
			boardSquarePathInfo2.prev = boardSquarePathInfo3;
			if (boardSquarePathInfo3 != null)
			{
				boardSquarePathInfo3.next = boardSquarePathInfo2;
			}
			flag2 = (flag2 || !stream.isReading);
			if (!flag2)
			{
				boardSquarePathInfo3 = boardSquarePathInfo2;
				boardSquarePathInfo2 = new BoardSquarePathInfo();
				continue;
			}
			continue;
			IL_CA:
			flag3 = true;
			goto IL_CB;
		}
		boardSquarePathInfo.moveCost = moveCost2;
		boardSquarePathInfo.CalcAndSetMoveCostToEnd();
		return boardSquarePathInfo;
	}

	internal static BoardSquarePathInfo DeSerializePath(NetworkReader reader)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		BoardSquarePathInfo boardSquarePathInfo3 = null;
		float moveCost = 0f;
		sbyte b = 0;
		sbyte b2 = 0;
		float num = 0f;
		float num2 = 0f;
		float moveCost2 = 0f;
		bool reverse = false;
		bool unskippable = false;
		bool flag = reader.ReadBoolean();
		if (flag)
		{
			num = reader.ReadSingle();
			num2 = reader.ReadSingle();
			moveCost2 = reader.ReadSingle();
		}
		bool flag2 = !flag;
		bool visibleToEnemies = false;
		bool updateLastKnownPos = false;
		bool moverDiesHere = false;
		bool moverClashesHere = false;
		bool moverBumpedFromClash = false;
		while (!flag2)
		{
			byte b3 = reader.ReadByte();
			byte b4 = reader.ReadByte();
			sbyte b5 = reader.ReadSByte();
			if ((int)b5 == 0)
			{
				goto IL_E1;
			}
			if ((int)b5 == 3)
			{
				goto IL_E1;
			}
			bool flag3 = (int)b5 == 1;
			IL_E2:
			if (!flag3)
			{
				b = reader.ReadSByte();
				b2 = reader.ReadSByte();
			}
			byte bitField = reader.ReadByte();
			byte bitField2 = reader.ReadByte();
			bool flag4 = false;
			bool flag5 = false;
			ServerClientUtils.GetBoolsFromBitfield(bitField, out reverse, out unskippable, out flag2, out visibleToEnemies, out updateLastKnownPos, out moverDiesHere, out flag4, out flag5);
			ServerClientUtils.GetBoolsFromBitfield(bitField2, out moverClashesHere, out moverBumpedFromClash);
			float segmentMovementSpeed = num;
			float segmentMovementDuration = num2;
			if (flag4)
			{
				segmentMovementSpeed = reader.ReadSingle();
			}
			if (flag5)
			{
				segmentMovementDuration = reader.ReadSingle();
			}
			BoardSquare boardSquare = Board.Get().GetBoardSquare((int)b3, (int)b4);
			if (boardSquare == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Failed to find square from index [",
					b3,
					", ",
					b4,
					"] during serialization of path"
				}), new object[0]);
			}
			boardSquarePathInfo2.square = boardSquare;
			boardSquarePathInfo2.moveCost = moveCost;
			boardSquarePathInfo2.heuristicCost = 0f;
			boardSquarePathInfo2.connectionType = (BoardSquarePathInfo.ConnectionType)b5;
			boardSquarePathInfo2.chargeCycleType = (BoardSquarePathInfo.ChargeCycleType)b;
			boardSquarePathInfo2.chargeEndType = (BoardSquarePathInfo.ChargeEndType)b2;
			boardSquarePathInfo2.segmentMovementSpeed = segmentMovementSpeed;
			boardSquarePathInfo2.segmentMovementDuration = segmentMovementDuration;
			boardSquarePathInfo2.m_reverse = reverse;
			boardSquarePathInfo2.m_unskippable = unskippable;
			boardSquarePathInfo2.m_visibleToEnemies = visibleToEnemies;
			boardSquarePathInfo2.m_updateLastKnownPos = updateLastKnownPos;
			boardSquarePathInfo2.m_moverDiesHere = moverDiesHere;
			boardSquarePathInfo2.m_moverClashesHere = moverClashesHere;
			boardSquarePathInfo2.m_moverBumpedFromClash = moverBumpedFromClash;
			boardSquarePathInfo2.prev = boardSquarePathInfo3;
			if (boardSquarePathInfo3 != null)
			{
				boardSquarePathInfo3.next = boardSquarePathInfo2;
			}
			if (!flag2)
			{
				boardSquarePathInfo3 = boardSquarePathInfo2;
				boardSquarePathInfo2 = new BoardSquarePathInfo();
				continue;
			}
			continue;
			IL_E1:
			flag3 = true;
			goto IL_E2;
		}
		boardSquarePathInfo.moveCost = moveCost2;
		boardSquarePathInfo.CalcAndSetMoveCostToEnd();
		return boardSquarePathInfo;
	}

	internal static BoardSquarePathInfo DeSerializePath(byte[] data)
	{
		if (data == null)
		{
			return null;
		}
		NetworkReader reader = new NetworkReader(data);
		return MovementUtils.DeSerializePath(reader);
	}

	internal static void SerializeLightweightPath(BoardSquarePathInfo path, NetworkWriter stream)
	{
		MovementUtils.SerializeLightweightPath(path, new NetworkWriterAdapter(stream));
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
			Log.Error("Trying to serialize a (lightweight) path while reading", new object[0]);
		}
		else
		{
			uint position = stream.Position;
			if (path == null)
			{
				sbyte b = 0;
				stream.Serialize(ref b);
			}
			else
			{
				sbyte b2 = 0;
				BoardSquarePathInfo boardSquarePathInfo;
				for (boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
				{
					b2 = (sbyte)((int)b2 + 1);
				}
				sbyte b3 = 0;
				for (boardSquarePathInfo = path.prev; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.prev)
				{
					b3 = (sbyte)((int)b3 + 1);
				}
				stream.Serialize(ref b2);
				stream.Serialize(ref b3);
				int num = 0;
				boardSquarePathInfo = path;
				for (int i = 0; i < (int)b2; i++)
				{
					short num2;
					short num3;
					if (boardSquarePathInfo.square != null)
					{
						num2 = (short)boardSquarePathInfo.square.x;
						num3 = (short)boardSquarePathInfo.square.y;
					}
					else
					{
						num2 = -1;
						num3 = -1;
						num++;
					}
					stream.Serialize(ref num2);
					stream.Serialize(ref num3);
					boardSquarePathInfo = boardSquarePathInfo.next;
				}
				boardSquarePathInfo = path.prev;
				for (int j = 0; j < (int)b3; j++)
				{
					short num4;
					short num5;
					if (boardSquarePathInfo.square != null)
					{
						num4 = (short)boardSquarePathInfo.square.x;
						num5 = (short)boardSquarePathInfo.square.y;
					}
					else
					{
						num4 = -1;
						num5 = -1;
						num++;
					}
					stream.Serialize(ref num4);
					stream.Serialize(ref num5);
					boardSquarePathInfo = boardSquarePathInfo.prev;
				}
				if (num > 0)
				{
					Debug.LogError("Calling SerializeLightweightPath with a path that has " + num + " null square(s).");
				}
			}
			uint num6 = stream.Position - position;
			if (ClientAbilityResults.symbol_000E)
			{
				Debug.LogWarning("\t\t\t Serializing Lightweight Movement Path: \n\t\t\t numBytes: " + num6);
			}
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
			Log.Error("Trying to deserialize a (lightweight) path while writing.", new object[0]);
			boardSquarePathInfo = null;
		}
		else
		{
			sbyte b = 0;
			stream.Serialize(ref b);
			if ((int)b <= 0)
			{
				boardSquarePathInfo = null;
			}
			else
			{
				sbyte b2 = 0;
				stream.Serialize(ref b2);
				boardSquarePathInfo = null;
				BoardSquarePathInfo boardSquarePathInfo2 = null;
				for (int i = 0; i < (int)b; i++)
				{
					short num = -1;
					short num2 = -1;
					stream.Serialize(ref num);
					stream.Serialize(ref num2);
					BoardSquare square;
					if (num == -1 && num2 == -1)
					{
						square = null;
					}
					else
					{
						square = Board.Get().GetBoardSquare((int)num, (int)num2);
					}
					BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
					boardSquarePathInfo3.square = square;
					boardSquarePathInfo3.prev = boardSquarePathInfo2;
					if (boardSquarePathInfo2 != null)
					{
						boardSquarePathInfo2.next = boardSquarePathInfo3;
					}
					boardSquarePathInfo2 = boardSquarePathInfo3;
					if (i == 0)
					{
						boardSquarePathInfo = boardSquarePathInfo3;
					}
				}
				BoardSquarePathInfo boardSquarePathInfo4 = boardSquarePathInfo;
				int j = 0;
				while (j < (int)b2)
				{
					short num3 = -1;
					short num4 = -1;
					stream.Serialize(ref num3);
					stream.Serialize(ref num4);
					if (num3 != -1)
					{
						goto IL_135;
					}
					if (num4 != -1)
					{
						goto IL_135;
					}
					BoardSquare square2 = null;
					IL_147:
					BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
					boardSquarePathInfo3.square = square2;
					boardSquarePathInfo3.next = boardSquarePathInfo4;
					boardSquarePathInfo4.prev = boardSquarePathInfo3;
					boardSquarePathInfo4 = boardSquarePathInfo3;
					j++;
					continue;
					IL_135:
					square2 = Board.Get().GetBoardSquare((int)num3, (int)num4);
					goto IL_147;
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
		while (boardSquarePathInfo3 != null)
		{
			if (boardSquarePathInfo4 != null)
			{
				if (boardSquarePathInfo4.square != boardSquarePathInfo3.square)
				{
				}
				else
				{
					if (boardSquarePathInfo3 == desiredEnding)
					{
						boardSquarePathInfo2 = boardSquarePathInfo4;
						goto IL_95;
					}
					boardSquarePathInfo3 = boardSquarePathInfo3.next;
					boardSquarePathInfo4 = boardSquarePathInfo4.next;
					continue;
				}
			}
			Debug.LogError("CreateClonePathEndingAt somehow has a bad clone...?  Tell Danny");
			IL_95:
			if (boardSquarePathInfo2 != null)
			{
				boardSquarePathInfo2.next = null;
			}
			return boardSquarePathInfo2;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			goto IL_95;
		}
	}

	public static bool ShouldVault(BoardSquare srcSquare, BoardSquare destSquare)
	{
		bool result = false;
		if (Board.Get().AreDiagonallyAdjacent(srcSquare, destSquare))
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare(srcSquare.GetGridPos().x, destSquare.GetGridPos().y);
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(destSquare.GetGridPos().x, srcSquare.GetGridPos().y);
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

	public unsafe static void CalculateVaultConnectionTypes(ref BoardSquarePathInfo path)
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
				if (MovementUtils.ShouldVault(square, square2))
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
		bool result = false;
		List<BoardSquare> squaresInRect = Board.Get().GetSquaresInRect(src, dst);
		using (List<BoardSquare>.Enumerator enumerator = squaresInRect.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.height < Board.Get().BaselineHeight)
				{
					return true;
				}
			}
		}
		return result;
	}

	private static bool CanRunDirectly(BoardSquare src, BoardSquare dst, ActorData mover)
	{
		bool result = false;
		LayerMask mask = 1 << LayerMask.NameToLayer("LineOfSight") | 1 << LayerMask.NameToLayer("DynamicLineOfSight");
		Vector3 b = new Vector3(0f, 0.5f, 0f);
		Vector3 vector = src.ToVector3() + b;
		Vector3 a = dst.ToVector3() + b;
		Vector3 vector2 = a - vector;
		Vector3 b2 = Vector3.Cross(vector2, new Vector3(0f, 1f, 0f)).normalized * 0.5f;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		RaycastHit raycastHit;
		if (!MovementUtils.MovingOverHole(src, dst) && !Physics.Raycast(vector + b2, vector2, out raycastHit, magnitude, mask))
		{
			if (!Physics.Raycast(vector, vector2, out raycastHit, magnitude, mask))
			{
				if (!Physics.Raycast(vector - b2, vector2, out raycastHit, magnitude, mask))
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

	public unsafe static void CreateUnskippableAestheticPath(ref BoardSquarePathInfo path, ActorData.MovementType movementType)
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
		while (boardSquarePathInfo != null)
		{
			bool flag;
			if (boardSquarePathInfo.connectionType != BoardSquarePathInfo.ConnectionType.Run)
			{
				flag = (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Vault);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (boardSquarePathInfo != path)
			{
				if (!boardSquarePathInfo.m_unskippable)
				{
					if (boardSquarePathInfo.m_moverClashesHere)
					{
						if (flag2)
						{
							goto IL_A9;
						}
					}
					if (boardSquarePathInfo.next != null)
					{
						goto IL_DA;
					}
				}
				IL_A9:
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
			IL_DA:
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
	}

	public unsafe static void CreateRunAndVaultAestheticPath(ref BoardSquarePathInfo path, ActorData mover)
	{
		MovementUtils.CalculateVaultConnectionTypes(ref path);
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
		while (boardSquarePathInfo != null && boardSquarePathInfo2 != null)
		{
			if (boardSquarePathInfo3 != null)
			{
				bool flag;
				if (boardSquarePathInfo2.connectionType != BoardSquarePathInfo.ConnectionType.Run)
				{
					flag = (boardSquarePathInfo2.connectionType == BoardSquarePathInfo.ConnectionType.Vault);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				int num;
				if (!boardSquarePathInfo2.m_unskippable)
				{
					if (flag2)
					{
						num = (boardSquarePathInfo2.m_moverClashesHere ? 1 : 0);
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 1;
				}
				bool flag3 = num == 0;
				bool flag4 = MovementUtils.CanRunDirectly(boardSquarePathInfo.square, boardSquarePathInfo3.square, mover);
				bool flag5;
				if (boardSquarePathInfo.connectionType == BoardSquarePathInfo.ConnectionType.Run)
				{
					flag5 = (boardSquarePathInfo2.connectionType == BoardSquarePathInfo.ConnectionType.Run);
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				bool flag7;
				if (boardSquarePathInfo2.connectionType == BoardSquarePathInfo.ConnectionType.Run)
				{
					flag7 = (boardSquarePathInfo3.connectionType == BoardSquarePathInfo.ConnectionType.Run);
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				if (!flag3)
				{
					goto IL_12D;
				}
				if (!flag4)
				{
					goto IL_12D;
				}
				if (!flag6)
				{
					goto IL_12D;
				}
				bool flag9 = flag8;
				IL_12E:
				bool flag10 = flag9;
				if (flag10)
				{
					boardSquarePathInfo.next = boardSquarePathInfo3;
					boardSquarePathInfo3.prev = boardSquarePathInfo;
					boardSquarePathInfo2 = boardSquarePathInfo.next;
					boardSquarePathInfo3 = boardSquarePathInfo3.next;
					continue;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
				boardSquarePathInfo3 = boardSquarePathInfo3.next;
				continue;
				IL_12D:
				flag9 = false;
				goto IL_12E;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return;
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
			case BoardSquarePathInfo.ConnectionType.Knockback:
				result = MoveState.LinkType.KnockBack;
				break;
			case BoardSquarePathInfo.ConnectionType.Charge:
				result = MoveState.LinkType.Charge;
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
		return MovementUtils.Build2PointTeleportPath(currentBoardSquare, destination);
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
		Vector2 a = new Vector2((float)destinationSquare.x, (float)destinationSquare.y);
		Vector2 vector = new Vector2(a.x - (float)currentSquare.x, a.y - (float)currentSquare.y);
		float sqrMagnitude = vector.sqrMagnitude;
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = -1; i <= 1; i++)
		{
			int j = -1;
			while (j <= 1)
			{
				if (i != 0)
				{
					goto IL_8C;
				}
				if (j != 0)
				{
					goto IL_8C;
				}
				IL_10F:
				j++;
				continue;
				IL_8C:
				BoardSquare boardSquare = Board.Get().GetBoardSquare(currentSquare.x + i, currentSquare.y + j);
				if (!(boardSquare != null))
				{
					goto IL_10F;
				}
				Vector2 vector2 = new Vector2(a.x - (float)boardSquare.x, a.y - (float)boardSquare.y);
				if (vector2.sqrMagnitude < sqrMagnitude)
				{
					list.Add(boardSquare);
					goto IL_10F;
				}
				goto IL_10F;
			}
		}
		float num = 99999.9f;
		BoardSquare result = null;
		Vector2 vector3 = new Vector2((float)originalSquare.x, (float)originalSquare.y);
		Vector2 vector4 = a - vector3;
		vector4.Normalize();
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare2 = enumerator.Current;
				Vector2 vector5 = new Vector2((float)boardSquare2.x, (float)boardSquare2.y);
				Vector2 lhs = vector5 - vector3;
				float d = Vector2.Dot(lhs, vector4);
				Vector2 a2 = vector3 + vector4 * d;
				float magnitude = (a2 - vector5).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					result = boardSquare2;
				}
			}
		}
		return result;
	}

	public static bool CanStopOnSquare(BoardSquare square)
	{
		bool result;
		if (square != null)
		{
			result = (square.height >= 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public static bool ArePathSegmentsEquivalent_ForwardAndBackward(BoardSquarePathInfo pathA, BoardSquarePathInfo pathB)
	{
		if (pathA != null)
		{
			if (pathB != null)
			{
				if (!(pathA.square == null))
				{
					if (pathB.square == null)
					{
					}
					else
					{
						if (pathA.square != pathB.square)
						{
							return false;
						}
						bool flag = true;
						bool flag2 = true;
						BoardSquarePathInfo boardSquarePathInfo = pathA;
						BoardSquarePathInfo boardSquarePathInfo2 = pathB;
						BoardSquarePathInfo boardSquarePathInfo3 = pathA;
						BoardSquarePathInfo boardSquarePathInfo4 = pathB;
						int num = 0;
						int num2 = 0;
						while (flag)
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
									goto IL_12D;
								}
							}
							else
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
								if (boardSquarePathInfo != null)
								{
									goto IL_D2;
								}
								if (boardSquarePathInfo2 == null)
								{
									goto IL_D2;
								}
								flag = false;
								IL_107:
								num++;
								continue;
								IL_D2:
								if (boardSquarePathInfo != null && boardSquarePathInfo2 == null)
								{
									flag = false;
									goto IL_107;
								}
								if (boardSquarePathInfo.square != boardSquarePathInfo2.square)
								{
									flag = false;
									goto IL_107;
								}
								goto IL_107;
							}
						}
						IL_12D:
						if (flag)
						{
							while (flag2)
							{
								if (num2 >= 0x64)
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
								if (boardSquarePathInfo3 != null)
								{
									goto IL_18B;
								}
								if (boardSquarePathInfo4 == null)
								{
									goto IL_18B;
								}
								flag2 = false;
								IL_1CE:
								num2++;
								continue;
								IL_18B:
								if (boardSquarePathInfo3 != null)
								{
									if (boardSquarePathInfo4 == null)
									{
										flag2 = false;
										goto IL_1CE;
									}
								}
								if (boardSquarePathInfo3.square != boardSquarePathInfo4.square)
								{
									flag2 = false;
									goto IL_1CE;
								}
								goto IL_1CE;
							}
						}
						if (num < 0x64)
						{
							if (num2 < 0x64)
							{
								goto IL_20A;
							}
						}
						Debug.LogError("Infinite/circular (or maybe just massive) loop detected in ArePathSegmentsEquivalent_ForwardAndBackward.");
						IL_20A:
						bool result;
						if (flag)
						{
							result = flag2;
						}
						else
						{
							result = false;
						}
						return result;
					}
				}
			}
		}
		return false;
	}

	public static bool ArePathSegmentsEquivalent_FromBeginning(BoardSquarePathInfo pathA, BoardSquarePathInfo pathB)
	{
		if (pathA != null)
		{
			if (pathB != null)
			{
				if (!(pathA.square == null))
				{
					if (pathB.square == null)
					{
					}
					else
					{
						if (pathA.square != pathB.square)
						{
							return false;
						}
						bool flag = true;
						BoardSquarePathInfo boardSquarePathInfo = pathA;
						BoardSquarePathInfo boardSquarePathInfo2 = pathB;
						int num = 0;
						while (flag)
						{
							if (num >= 0x64)
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
							}
							else
							{
								if (boardSquarePathInfo != null)
								{
									if (boardSquarePathInfo2 == null)
									{
										flag = false;
										goto IL_D7;
									}
								}
								if (boardSquarePathInfo.square != boardSquarePathInfo2.square)
								{
									flag = false;
								}
							}
							IL_D7:
							num++;
						}
						if (num >= 0x64)
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
