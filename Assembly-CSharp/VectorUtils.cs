using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class VectorUtils
{
	private static float s_positionOffset = 0.3f;

	public static float s_laserOffset = 0.3f;

	private static float s_laserInitialLengthOffset = 0.71f;

	public static int s_raycastLayerLineOfSight = LayerMask.NameToLayer("LineOfSight");

	public static int s_raycastLayerDynamicLineOfSight = LayerMask.NameToLayer("DynamicLineOfSight");

	public static ActorCover.CoverDirections GetCoverDirection(BoardSquare srcSquare, BoardSquare destSquare)
	{
		int x = srcSquare.x;
		int y = srcSquare.y;
		int x2 = destSquare.x;
		int y2 = destSquare.y;
		ActorCover.CoverDirections result;
		if (Mathf.Abs(x - x2) > Mathf.Abs(y - y2))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetCoverDirection(BoardSquare, BoardSquare)).MethodHandle;
			}
			if (x > x2)
			{
				result = ActorCover.CoverDirections.X_NEG;
			}
			else
			{
				result = ActorCover.CoverDirections.X_POS;
			}
		}
		else if (y > y2)
		{
			result = ActorCover.CoverDirections.Y_NEG;
		}
		else
		{
			result = ActorCover.CoverDirections.Y_POS;
		}
		return result;
	}

	private static BoardSquare GetAdjSquare(BoardSquare square, ActorCover.CoverDirections direction)
	{
		BoardSquare result = null;
		switch (direction)
		{
		case ActorCover.CoverDirections.X_POS:
			result = Board.Get().GetBoardSquare(square.x + 1, square.y);
			break;
		case ActorCover.CoverDirections.X_NEG:
			result = Board.Get().GetBoardSquare(square.x - 1, square.y);
			break;
		case ActorCover.CoverDirections.Y_POS:
			result = Board.Get().GetBoardSquare(square.x, square.y + 1);
			break;
		case ActorCover.CoverDirections.Y_NEG:
			result = Board.Get().GetBoardSquare(square.x, square.y - 1);
			break;
		}
		return result;
	}

	public static bool HasCoverInDirection(BoardSquare square, ActorCover.CoverDirections coverDirection)
	{
		bool result = false;
		BoardSquare adjSquare = VectorUtils.GetAdjSquare(square, coverDirection);
		if (adjSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.HasCoverInDirection(BoardSquare, ActorCover.CoverDirections)).MethodHandle;
			}
			if ((float)(adjSquare.height - square.height) > 1f)
			{
				result = true;
			}
		}
		return result;
	}

	private static bool IsSuitableAdditionalCoverSquare(BoardSquare src, ActorCover.CoverDirections adjDirection, ActorCover.CoverDirections coverDirection)
	{
		bool result = false;
		if (!VectorUtils.HasCoverInDirection(src, adjDirection))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.IsSuitableAdditionalCoverSquare(BoardSquare, ActorCover.CoverDirections, ActorCover.CoverDirections)).MethodHandle;
			}
			BoardSquare adjSquare = VectorUtils.GetAdjSquare(src, adjDirection);
			if (adjSquare != null && !VectorUtils.HasCoverInDirection(adjSquare, coverDirection))
			{
				result = true;
			}
		}
		return result;
	}

	private static List<BoardSquare> GetAdditionalCoverSquares(BoardSquare src, BoardSquare dst)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		ActorCover.CoverDirections coverDirection = VectorUtils.GetCoverDirection(src, dst);
		list.Add(src);
		if (VectorUtils.HasCoverInDirection(src, coverDirection))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetAdditionalCoverSquares(BoardSquare, BoardSquare)).MethodHandle;
			}
			if (coverDirection != ActorCover.CoverDirections.X_NEG)
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
				if (coverDirection != ActorCover.CoverDirections.X_POS)
				{
					if (coverDirection != ActorCover.CoverDirections.Y_NEG)
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
						if (coverDirection != ActorCover.CoverDirections.Y_POS)
						{
							return list;
						}
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (VectorUtils.IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.X_NEG, coverDirection))
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
						list.Add(VectorUtils.GetAdjSquare(src, ActorCover.CoverDirections.X_NEG));
					}
					if (VectorUtils.IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.X_POS, coverDirection))
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
						list.Add(VectorUtils.GetAdjSquare(src, ActorCover.CoverDirections.X_POS));
						return list;
					}
					return list;
				}
			}
			if (VectorUtils.IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.Y_NEG, coverDirection))
			{
				list.Add(VectorUtils.GetAdjSquare(src, ActorCover.CoverDirections.Y_NEG));
			}
			if (VectorUtils.IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.Y_POS, coverDirection))
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
				list.Add(VectorUtils.GetAdjSquare(src, ActorCover.CoverDirections.Y_POS));
			}
		}
		return list;
	}

	public static bool HasLineOfSightFromIndex(int startX, int startY, int endX, int endY, Board board, float heightOffset, string layerName)
	{
		float squareSize = board.squareSize;
		float y;
		if (board.GetSquareHeight(startX, startY) < 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.HasLineOfSightFromIndex(int, int, int, int, Board, float, string)).MethodHandle;
			}
			y = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y = heightOffset + board.GetSquareHeight(startX, startY);
		}
		float y2;
		if (board.GetSquareHeight(endX, endY) < 0f)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			y2 = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y2 = heightOffset + board.GetSquareHeight(endX, endY);
		}
		Vector3 vector = new Vector3((float)startX * squareSize, y, (float)startY * squareSize);
		Vector3 vector2 = new Vector3((float)endX * squareSize, y2, (float)endY * squareSize);
		Vector3[] array = new Vector3[3];
		Vector3[] array2 = new Vector3[3];
		bool flag = false;
		if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 b = new Vector3(0f, 0f, squareSize * VectorUtils.s_positionOffset);
			array[0] = vector - b;
			array[1] = vector;
			array[2] = vector + b;
			array2[0] = vector2 - b;
			array2[1] = vector2;
			array2[2] = vector2 + b;
		}
		else
		{
			Vector3 b2 = new Vector3(squareSize * VectorUtils.s_positionOffset, 0f, 0f);
			array[0] = vector - b2;
			array[1] = vector;
			array[2] = vector + b2;
			array2[0] = vector2 - b2;
			array2[1] = vector2;
			array2[2] = vector2 + b2;
		}
		int i = 0;
		IL_26F:
		while (i < array.Length)
		{
			int j = 0;
			while (j < array2.Length)
			{
				flag = VectorUtils.HasLineOfSight(array[i], array2[j], layerName);
				if (flag)
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
					IL_259:
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
						return flag;
					}
					i++;
					goto IL_26F;
				}
				else
				{
					j++;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				goto IL_259;
			}
		}
		return flag;
	}

	private static bool HasLineOfSight(Vector3 startPos, Vector3 endPos, string layerName)
	{
		Vector3 direction = endPos - startPos;
		float magnitude = direction.magnitude;
		direction.Normalize();
		LayerMask mask = 1 << LayerMask.NameToLayer(layerName) | 1 << LayerMask.NameToLayer("DynamicLineOfSight");
		RaycastHit raycastHit;
		return !Physics.Raycast(startPos, direction, out raycastHit, magnitude, mask);
	}

	public static float GetLineOfSightPercentDistance(int startX, int startY, int endX, int endY, Board board, float heightOffset, string layerName)
	{
		float squareSize = board.squareSize;
		float y;
		if (board.GetSquareHeight(startX, startY) < 0f)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetLineOfSightPercentDistance(int, int, int, int, Board, float, string)).MethodHandle;
			}
			y = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y = heightOffset + board.GetSquareHeight(startX, startY);
		}
		float y2;
		if (board.GetSquareHeight(endX, endY) < 0f)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			y2 = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y2 = heightOffset + board.GetSquareHeight(endX, endY);
		}
		Vector3 vector = new Vector3((float)startX * squareSize, y, (float)startY * squareSize);
		Vector3 vector2 = new Vector3((float)endX * squareSize, y2, (float)endY * squareSize);
		Vector3[] array = new Vector3[3];
		Vector3[] array2 = new Vector3[3];
		if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
		{
			Vector3 b = new Vector3(0f, 0f, squareSize * VectorUtils.s_positionOffset);
			array[0] = vector - b;
			array[1] = vector;
			array[2] = vector + b;
			array2[0] = vector2 - b;
			array2[1] = vector2;
			array2[2] = vector2 + b;
		}
		else
		{
			Vector3 b2 = new Vector3(squareSize * VectorUtils.s_positionOffset, 0f, 0f);
			array[0] = vector - b2;
			array[1] = vector;
			array[2] = vector + b2;
			array2[0] = vector2 - b2;
			array2[1] = vector2;
			array2[2] = vector2 + b2;
		}
		float num = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				float lineOfSightPercentDistance = VectorUtils.GetLineOfSightPercentDistance(array[i], array2[j], layerName);
				if (lineOfSightPercentDistance > num)
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
					num = lineOfSightPercentDistance;
					if (num == 1f)
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
						break;
					}
				}
			}
			if (num == 1f)
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
				return num;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return num;
		}
	}

	private static float GetLineOfSightPercentDistance(Vector3 startPos, Vector3 endPos, string layerName)
	{
		Vector3 direction = endPos - startPos;
		float magnitude = direction.magnitude;
		direction.Normalize();
		LayerMask mask = 1 << LayerMask.NameToLayer(layerName) | 1 << LayerMask.NameToLayer("DynamicLineOfSight");
		RaycastHit raycastHit;
		bool flag = !Physics.Raycast(startPos, direction, out raycastHit, magnitude, mask);
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetLineOfSightPercentDistance(Vector3, Vector3, string)).MethodHandle;
			}
			return 1f;
		}
		return raycastHit.distance / magnitude;
	}

	public static VectorUtils.LaserCoords GetLaserCoordinates(Vector3 startPos, Vector3 dir, float maxDistanceInWorld, float widthInWorld, bool penetrateLoS, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		return new VectorUtils.LaserCoords
		{
			start = startPos,
			end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, penetrateLoS, caster, nonActorTargetInfo, true)
		};
	}

	public static Vector3 GetLaserEndPoint(Vector3 startPos, Vector3 dir, float maxDistanceInWorld, bool penetrateLoS, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo = null, bool checkBarriers = true)
	{
		dir.y = 0f;
		Vector3 vector;
		if (penetrateLoS)
		{
			vector = startPos + dir * maxDistanceInWorld;
		}
		else
		{
			Vector3[] array = new Vector3[3];
			array[0] = startPos;
			float d = VectorUtils.s_laserOffset * Board.Get().squareSize;
			float num = VectorUtils.s_laserInitialLengthOffset * Board.Get().squareSize;
			Vector3 vector2 = Vector3.Cross(Vector3.up, dir);
			vector2.Normalize();
			vector2 *= d;
			array[1] = startPos + vector2;
			array[2] = startPos - vector2;
			float num2 = 0f;
			bool flag;
			if (BarrierManager.Get() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetLaserEndPoint(Vector3, Vector3, float, bool, ActorData, List<NonActorTargetInfo>, bool)).MethodHandle;
				}
				flag = BarrierManager.Get().HasAbilityBlockingBarriers();
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			bool flag3 = true;
			bool flag4 = false;
			Vector3 a = array[0] + num * dir;
			if (maxDistanceInWorld > num)
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
				for (int i = 0; i < array.Length; i++)
				{
					Vector3 b = array[i];
					Vector3 dir2 = a - b;
					float magnitude = dir2.magnitude;
					dir2.Normalize();
					RaycastHit raycastHit;
					bool flag5 = VectorUtils.RaycastInDirection(array[i], dir2, magnitude, out raycastHit);
					if (flag5)
					{
						flag3 = false;
						if ((raycastHit.collider.gameObject.layer & VectorUtils.s_raycastLayerDynamicLineOfSight) != 0)
						{
							flag4 = true;
						}
						break;
					}
				}
			}
			List<NonActorTargetInfo> list = null;
			if (maxDistanceInWorld > num)
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
				if (flag3)
				{
					Vector3 vector3 = array[0] + num * dir;
					Vector3 lineEndPoint = VectorUtils.GetLineEndPoint(vector3, dir, maxDistanceInWorld - num);
					float num3 = (vector3 - lineEndPoint).magnitude + num;
					float num4 = num3 * num3;
					if (num4 > num2)
					{
						num2 = num4;
						goto IL_3EA;
					}
					goto IL_3EA;
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				List<NonActorTargetInfo> list2;
				if (nonActorTargetInfo != null)
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
					list2 = new List<NonActorTargetInfo>();
				}
				else
				{
					list2 = null;
				}
				List<NonActorTargetInfo> list3 = list2;
				Vector3 vector4 = array[j];
				Vector3 vector5 = VectorUtils.GetLineEndPoint(vector4, dir, maxDistanceInWorld);
				if (checkBarriers && flag2)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag6;
					Vector3 vector6;
					vector5 = BarrierManager.Get().GetAbilityLineEndpoint(caster, vector4, vector5, out flag6, out vector6, list3);
				}
				float sqrMagnitude = (vector4 - vector5).sqrMagnitude;
				if (sqrMagnitude > num2)
				{
					Vector3 b2 = array[j] - array[0];
					Vector3 vector7 = (vector4 + vector5) / 2f - b2;
					if (flag4)
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
						vector7 = startPos + (num + 0.3f) * dir;
					}
					float maxDistance = Mathf.Max(0f, (vector7 - startPos).magnitude - num);
					Vector3 lineEndPoint2 = VectorUtils.GetLineEndPoint(vector7, -dir, maxDistance);
					float maxDistance2 = maxDistanceInWorld - (lineEndPoint2 - startPos).magnitude;
					Vector3 lineEndPoint3 = VectorUtils.GetLineEndPoint(lineEndPoint2, dir, maxDistance2);
					float sqrMagnitude2 = (array[0] - lineEndPoint3).sqrMagnitude;
					float num5 = Mathf.Min(sqrMagnitude, sqrMagnitude2);
					if (num5 > num2)
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
						num2 = num5;
						list = list3;
					}
					else if (Mathf.Approximately(num5, num2))
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
						if (list != null && list.Count == 0)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							list = list3;
						}
					}
				}
			}
			IL_3EA:
			float num6 = Mathf.Sqrt(num2);
			if (num6 < maxDistanceInWorld - 0.1f)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num6 = Mathf.Max(0f, num6 - 0.05f);
			}
			vector = startPos + dir * num6;
			if (BarrierManager.Get() != null && checkBarriers)
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
				bool flag7;
				Vector3 vector8;
				vector = BarrierManager.Get().GetAbilityLineEndpoint(caster, startPos, vector, out flag7, out vector8, nonActorTargetInfo);
				if (!flag7)
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
					if (nonActorTargetInfo != null && list != null)
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
						nonActorTargetInfo.AddRange(list);
					}
				}
			}
		}
		return vector;
	}

	public static Vector3 GetLineEndPoint(Vector3 startPos, Vector3 dir, float maxDistance)
	{
		dir.Normalize();
		LayerMask mask = 1 << VectorUtils.s_raycastLayerLineOfSight | 1 << VectorUtils.s_raycastLayerDynamicLineOfSight;
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(startPos, dir, out raycastHit, maxDistance, mask);
		Vector3 result;
		if (flag)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetLineEndPoint(Vector3, Vector3, float)).MethodHandle;
			}
			result = raycastHit.point;
		}
		else
		{
			result = startPos + dir * maxDistance;
		}
		return result;
	}

	public static bool RaycastInDirection(Vector3 startPos, Vector3 dir, float maxDistance, out RaycastHit hit)
	{
		dir.Normalize();
		LayerMask mask = 1 << VectorUtils.s_raycastLayerLineOfSight | 1 << VectorUtils.s_raycastLayerDynamicLineOfSight;
		return Physics.Raycast(startPos, dir, out hit, maxDistance, mask);
	}

	public static bool RaycastInDirection(Vector3 startPos, Vector3 endPos, out RaycastHit hit)
	{
		Vector3 direction = endPos - startPos;
		direction.y = 0f;
		direction.Normalize();
		float magnitude = direction.magnitude;
		LayerMask mask = 1 << VectorUtils.s_raycastLayerLineOfSight | 1 << VectorUtils.s_raycastLayerDynamicLineOfSight;
		return Physics.Raycast(startPos, direction, out hit, magnitude, mask);
	}

	public static Vector3 GetAdjustedStartPosWithOffset(Vector3 startPos, Vector3 endPos, float offsetInSquares)
	{
		Vector3 vector = endPos - startPos;
		vector.y = 0f;
		Vector3 result = startPos;
		if (offsetInSquares != 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetAdjustedStartPosWithOffset(Vector3, Vector3, float)).MethodHandle;
			}
			float d = Mathf.Min(vector.magnitude, offsetInSquares * Board.Get().squareSize);
			result = startPos + d * vector.normalized;
		}
		return result;
	}

	public static bool SquareOnSameSideAsBounceBend(BoardSquare testSquare, Vector3 bounceFromPos, Vector3 collisionNormal)
	{
		Vector3 rhs = testSquare.ToVector3() - bounceFromPos;
		rhs.y = 0f;
		return Vector3.Dot(collisionNormal, rhs) >= 0f;
	}

	public unsafe static List<Vector3> CalculateBouncingLaserEndpoints(Vector3 laserStartPos, Vector3 forwardDirection, float maxDistancePerBounceInSquares, float totalMaxDistanceInSquares, int maxBounces, ActorData caster, float widthInSquares, int maxTargets, bool includeInvisibles, List<Team> validTeams, bool bounceOnActors, out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors, out List<ActorData> orderedHitActors, List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments, bool calculateLaserPastMaxTargets = false, bool skipHitsOnCaster = true)
	{
		Vector3 vector = laserStartPos;
		List<Vector3> list = new List<Vector3>();
		bounceHitActors = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		orderedHitActors = new List<ActorData>();
		float num = Board.Get().squareSize * AreaEffectUtils.GetActorTargetingRadius();
		float num2 = maxDistancePerBounceInSquares * Board.Get().squareSize;
		float num3 = totalMaxDistanceInSquares * Board.Get().squareSize;
		Vector3 vector2 = forwardDirection;
		int num4 = 0;
		int num5 = 0;
		float num6 = 0f;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		ActorData excludeActor = null;
		float maxDistanceInWorld = Board.Get().squareSize * 1.8f;
		List<NonActorTargetInfo> list2 = new List<NonActorTargetInfo>();
		Vector3 end = VectorUtils.GetLaserCoordinates(laserStartPos, forwardDirection, maxDistanceInWorld, 0f, false, caster, list2).end;
		Vector3 a = end - laserStartPos;
		Vector3 b = a * 0.5f;
		float magnitude = b.magnitude;
		laserStartPos += b;
		num6 += magnitude;
		num2 -= magnitude;
		int num7 = 0;
		while (!flag3)
		{
			bool flag4 = nonActorTargetInfoInSegments != null;
			if (nonActorTargetInfoInSegments != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.CalculateBouncingLaserEndpoints(Vector3, Vector3, float, float, int, ActorData, float, int, bool, List<Team>, bool, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>*, List<ActorData>*, List<List<NonActorTargetInfo>>, bool, bool)).MethodHandle;
				}
				nonActorTargetInfoInSegments.Add(new List<NonActorTargetInfo>());
			}
			vector2.Normalize();
			float num8 = Mathf.Min(num2, num3 - num6);
			num2 = maxDistancePerBounceInSquares * Board.Get().squareSize;
			Vector3 zero = Vector3.zero;
			Vector3 vector3 = Vector3.zero;
			bool flag5 = false;
			Vector3 prevStartPos = Vector3.zero;
			if (num7 == 1)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				prevStartPos = vector;
			}
			else if (num7 > 1)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				prevStartPos = list[num7 - 2];
			}
			Vector3 startPosForBounce = laserStartPos;
			Vector3 startPosForGameplay;
			if (num7 == 0)
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
				startPosForGameplay = vector;
			}
			else
			{
				startPosForGameplay = laserStartPos;
			}
			Vector3 dir = vector2;
			float maxDistance = num8;
			List<NonActorTargetInfo> nonActorTargetInfo;
			if (flag4)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				nonActorTargetInfo = nonActorTargetInfoInSegments[num7];
			}
			else
			{
				nonActorTargetInfo = null;
			}
			bool flag6;
			ActorData actorData;
			List<ActorData> list3 = VectorUtils.CalculateLaserBounce(startPosForBounce, startPosForGameplay, dir, maxDistance, caster, out vector3, out flag5, out zero, widthInSquares, validTeams, nonActorTargetInfo, includeInvisibles, num7, prevStartPos, bounceOnActors, excludeActor, out flag6, out actorData, skipHitsOnCaster);
			excludeActor = actorData;
			using (List<ActorData>.Enumerator enumerator = list3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData2 = enumerator.Current;
					if (!bounceHitActors.ContainsKey(actorData2))
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
						if (maxTargets > 0)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (orderedHitActors.Count >= maxTargets)
							{
								continue;
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo((num7 != 0) ? laserStartPos : vector, num7);
						bounceHitActors.Add(actorData2, value);
						orderedHitActors.Add(actorData2);
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			bool flag7;
			if (maxTargets > 0 && orderedHitActors.Count >= maxTargets)
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
				flag7 = !calculateLaserPastMaxTargets;
			}
			else
			{
				flag7 = false;
			}
			bool flag8 = flag7;
			if (flag8)
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
				Vector3 normalized = (vector3 - laserStartPos).normalized;
				Vector3 rhs = orderedHitActors[orderedHitActors.Count - 1].GetTravelBoardSquareWorldPosition() - laserStartPos;
				vector3 = laserStartPos + (Vector3.Dot(normalized, rhs) + num) * normalized;
				if (flag4)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					nonActorTargetInfoInSegments[num7].Clear();
					flag4 = false;
				}
			}
			if (flag4)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (list.Count == 0 && list2.Count > 0)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					using (List<NonActorTargetInfo>.Enumerator enumerator2 = list2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							NonActorTargetInfo item = enumerator2.Current;
							nonActorTargetInfoInSegments[num7].Add(item);
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
			list.Add(vector3);
			float magnitude2 = (laserStartPos - vector3).magnitude;
			num6 += magnitude2;
			if (num6 >= num3 - 0.01f)
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
				flag2 = true;
			}
			if (flag6)
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
				num4++;
				num5++;
				laserStartPos = vector3;
				vector2 -= 2f * Vector3.Dot(vector2, zero) * zero;
			}
			else if (flag5)
			{
				num4++;
				laserStartPos = vector3;
				vector2 -= 2f * Vector3.Dot(vector2, zero) * zero;
			}
			else
			{
				flag = true;
			}
			if (flag || flag2)
			{
				goto IL_48E;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num4 > maxBounces)
			{
				goto IL_48E;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag9 = flag8;
			IL_48F:
			flag3 = flag9;
			num7++;
			continue;
			IL_48E:
			flag9 = true;
			goto IL_48F;
		}
		return list;
	}

	public unsafe static List<ActorData> CalculateLaserBounce(Vector3 startPosForBounce, Vector3 startPosForGameplay, Vector3 dir, float maxDistance, ActorData caster, out Vector3 endPoint, out bool collisionWithGeo, out Vector3 collisionNormal, float widthInSquares, List<Team> validTeams, List<NonActorTargetInfo> nonActorTargetInfo, bool includeInvisibles, int segmentIndex, Vector3 prevStartPos, bool bounceOnActors, ActorData excludeActor, out bool hitActorFirst, out ActorData bounceHitActor, bool skipHitsOnCaster = true)
	{
		LayerMask mask = 1 << LayerMask.NameToLayer("LineOfSight") | 1 << LayerMask.NameToLayer("DynamicLineOfSight");
		RaycastHit raycastHit;
		collisionWithGeo = Physics.Raycast(startPosForBounce, dir, out raycastHit, maxDistance, mask);
		if (collisionWithGeo)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.CalculateLaserBounce(Vector3, Vector3, Vector3, float, ActorData, Vector3*, bool*, Vector3*, float, List<Team>, List<NonActorTargetInfo>, bool, int, Vector3, bool, ActorData, bool*, ActorData*, bool)).MethodHandle;
			}
			Vector3 a = raycastHit.point - startPosForBounce;
			a.y = 0f;
			float magnitude = a.magnitude;
			a.Normalize();
			endPoint = startPosForBounce + a * Mathf.Max(0f, magnitude - 0.1f);
			collisionNormal = raycastHit.normal;
		}
		else
		{
			endPoint = startPosForBounce + dir * maxDistance;
			collisionNormal = Vector3.zero;
		}
		if (BarrierManager.Get() != null)
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
			bool flag;
			Vector3 vector;
			Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(caster, startPosForBounce, endPoint, out flag, out vector, nonActorTargetInfo);
			if (flag)
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
				endPoint = abilityLineEndpoint;
				collisionWithGeo = true;
				collisionNormal = vector;
			}
		}
		List<ActorData> list;
		if (GameWideData.Get().UseActorRadiusForLaser())
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
			list = AreaEffectUtils.GetActorsInBoxByActorRadius(startPosForGameplay, endPoint, widthInSquares, false, caster, validTeams, null, null);
		}
		else
		{
			list = AreaEffectUtils.GetActorsInBox(startPosForGameplay, endPoint, widthInSquares, true, caster, validTeams);
		}
		List<ActorData> list2 = list;
		if (skipHitsOnCaster)
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
			list2.Remove(caster);
		}
		if (!includeInvisibles)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (NetworkServer.active)
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
				TargeterUtils.RemoveActorsInvisibleToActor(ref list2, caster);
			}
			else
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref list2);
			}
		}
		if (excludeActor != null)
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
			list2.Remove(excludeActor);
		}
		Vector3 a2 = endPoint - startPosForBounce;
		a2.y = 0f;
		a2.Normalize();
		if (segmentIndex > 0)
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
			if (list2.Count > 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 b = prevStartPos - startPosForBounce;
				b.y = 0f;
				b.Normalize();
				Vector3 collisionNormal2 = 0.5f * (a2 + b);
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					BoardSquare currentBoardSquare = list2[i].GetCurrentBoardSquare();
					if (!VectorUtils.SquareOnSameSideAsBounceBend(currentBoardSquare, startPosForBounce, collisionNormal2))
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
						list2.RemoveAt(i);
					}
				}
			}
		}
		TargeterUtils.SortActorsByDistanceToPos(ref list2, startPosForBounce);
		hitActorFirst = false;
		bounceHitActor = null;
		if (bounceOnActors)
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
			int num = 0;
			float num2 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
			int j = 0;
			while (j < list2.Count)
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
				if (hitActorFirst)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_4F3;
					}
				}
				else
				{
					ActorData actorData = list2[j];
					Vector3 vector2 = actorData.GetTravelBoardSquareWorldPosition() - startPosForBounce;
					vector2.y = 0f;
					float magnitude2 = vector2.magnitude;
					Vector3 vector3 = endPoint - startPosForBounce;
					vector3.y = 0f;
					float magnitude3 = vector3.magnitude;
					if (magnitude2 < magnitude3)
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
						Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
						float num3 = 0.5f * widthInSquares * Board.Get().squareSize;
						if (GameWideData.Get().UseActorRadiusForLaser())
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							num3 += num2;
						}
						num3 = Mathf.Min(0.4f * Board.Get().squareSize, num3);
						Vector3 vector4;
						Vector3 vector5;
						int lineCircleIntersections = VectorUtils.GetLineCircleIntersections(startPosForGameplay, endPoint, travelBoardSquareWorldPosition, num3, out vector4, out vector5);
						if (lineCircleIntersections > 1)
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
							float num4 = VectorUtils.HorizontalPlaneDistInWorld(vector4, startPosForGameplay);
							float num5 = VectorUtils.HorizontalPlaneDistInWorld(vector5, startPosForGameplay);
							Vector3 vector6;
							if (num4 <= num5)
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
								vector6 = vector4;
							}
							else
							{
								vector6 = vector5;
							}
							endPoint = vector6;
							endPoint.y = startPosForBounce.y;
							collisionNormal = endPoint - travelBoardSquareWorldPosition;
							collisionNormal.y = 0f;
							collisionNormal.Normalize();
							float num6 = 0.5f * AreaEffectUtils.GetMaxAngleForActorBounce();
							collisionNormal = Vector3.RotateTowards(-a2, collisionNormal, 0.0174532924f * num6, 0f);
							hitActorFirst = true;
							collisionWithGeo = false;
							bounceHitActor = actorData;
							num = j;
						}
					}
					j++;
				}
			}
			IL_4F3:
			if (hitActorFirst)
			{
				TargeterUtils.LimitActorsToMaxNumber(ref list2, num + 1);
			}
		}
		return list2;
	}

	public unsafe static List<Vector3> CalculateBouncingActorEndpoints(Vector3 laserStartPos, Vector3 forwardDirection, float maxDistancePerBounceInSquares, float totalMaxDistanceInSquares, int maxBounces, ActorData caster, bool bounceOnActors, float bounceTestWidthInSquares, List<Team> bounceActorTeams, int maxTargets, out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors, out List<ActorData> orderedHitActors, bool includeInvisibles, List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments)
	{
		Vector3 vector = laserStartPos;
		List<Vector3> list = new List<Vector3>();
		bounceHitActors = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		orderedHitActors = new List<ActorData>();
		float num = maxDistancePerBounceInSquares * Board.Get().squareSize;
		float num2 = totalMaxDistanceInSquares * Board.Get().squareSize;
		Vector3 vector2 = forwardDirection;
		int num3 = 0;
		float num4 = 0f;
		int num5 = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		float maxDistanceInWorld = Board.Get().squareSize * 1.8f;
		laserStartPos.y = caster.GetTravelBoardSquareWorldPositionForLos().y;
		List<NonActorTargetInfo> list2 = new List<NonActorTargetInfo>();
		Vector3 end = VectorUtils.GetLaserCoordinates(laserStartPos, forwardDirection, maxDistanceInWorld, 0f, false, caster, list2).end;
		Vector3 a = end - laserStartPos;
		Vector3 b = a * 0.5f;
		float magnitude = b.magnitude;
		laserStartPos += b;
		num4 += magnitude;
		num -= magnitude;
		int num6 = 0;
		ActorData actorData = null;
		while (!flag3)
		{
			bool flag4 = nonActorTargetInfoInSegments != null;
			if (nonActorTargetInfoInSegments != null)
			{
				nonActorTargetInfoInSegments.Add(new List<NonActorTargetInfo>());
			}
			vector2.Normalize();
			float num7 = Mathf.Min(num, num2 - num4);
			num = maxDistancePerBounceInSquares * Board.Get().squareSize;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			bool flag5 = false;
			bool flag6 = false;
			ActorData actorData2 = null;
			Vector3 prevStartPos = Vector3.zero;
			if (num6 == 1)
			{
				prevStartPos = vector;
			}
			else if (num6 > 1)
			{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.CalculateBouncingActorEndpoints(Vector3, Vector3, float, float, int, ActorData, bool, float, List<Team>, int, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>*, List<ActorData>*, bool, List<List<NonActorTargetInfo>>)).MethodHandle;
				}
				prevStartPos = list[num6 - 2];
			}
			Vector3 startPosForBounce = laserStartPos;
			Vector3 startPosForGameplay;
			if (num6 == 0)
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
				startPosForGameplay = vector;
			}
			else
			{
				startPosForGameplay = laserStartPos;
			}
			Vector3 dir = vector2;
			float maxDistance = num7;
			ActorData excludeActor = actorData;
			List<NonActorTargetInfo> nonActorTargetInfo;
			if (flag4)
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
				nonActorTargetInfo = nonActorTargetInfoInSegments[num6];
			}
			else
			{
				nonActorTargetInfo = null;
			}
			List<ActorData> list3 = VectorUtils.CalculateActorBounce(startPosForBounce, startPosForGameplay, dir, maxDistance, caster, bounceOnActors, bounceTestWidthInSquares, bounceActorTeams, excludeActor, includeInvisibles, out zero2, out flag5, out zero, out flag6, out actorData2, nonActorTargetInfo, num6, prevStartPos);
			actorData = actorData2;
			using (List<ActorData>.Enumerator enumerator = list3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData3 = enumerator.Current;
					if (!bounceHitActors.ContainsKey(actorData3))
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
						if (maxTargets > 0)
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
							if (orderedHitActors.Count >= maxTargets)
							{
								continue;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo(laserStartPos, num6);
						bounceHitActors.Add(actorData3, value);
						orderedHitActors.Add(actorData3);
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			bool flag7;
			if (maxTargets > 0)
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
				flag7 = (orderedHitActors.Count >= maxTargets);
			}
			else
			{
				flag7 = false;
			}
			bool flag8 = flag7;
			if (flag8 && flag4)
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
				nonActorTargetInfoInSegments[num6].Clear();
				flag4 = false;
			}
			if (flag4)
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
				if (list.Count == 0)
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
					if (list2.Count > 0)
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
						using (List<NonActorTargetInfo>.Enumerator enumerator2 = list2.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								NonActorTargetInfo item = enumerator2.Current;
								nonActorTargetInfoInSegments[num6].Add(item);
							}
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
			}
			list.Add(zero2);
			float magnitude2 = (laserStartPos - zero2).magnitude;
			num4 += magnitude2;
			if (num4 >= num2 - 0.01f)
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
				flag2 = true;
			}
			if (flag6)
			{
				num3++;
				num5++;
				laserStartPos = zero2;
				vector2 -= 2f * Vector3.Dot(vector2, zero) * zero;
			}
			else if (flag5)
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
				num3++;
				laserStartPos = zero2;
				vector2 -= 2f * Vector3.Dot(vector2, zero) * zero;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				goto IL_42E;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag2)
			{
				goto IL_42E;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num3 > maxBounces)
			{
				goto IL_42E;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag9 = flag8;
			IL_42F:
			flag3 = flag9;
			num6++;
			continue;
			IL_42E:
			flag9 = true;
			goto IL_42F;
		}
		return list;
	}

	public unsafe static List<ActorData> CalculateActorBounce(Vector3 startPosForBounce, Vector3 startPosForGameplay, Vector3 dir, float maxDistance, ActorData caster, bool bounceOnActors, float bounceTestWidthInSquares, List<Team> hitTeams, ActorData excludeActor, bool includeInvisibles, out Vector3 endPoint, out bool collisionWithGeo, out Vector3 collisionNormal, out bool hitActorFirst, out ActorData bounceHitActor, List<NonActorTargetInfo> nonActorTargetInfo, int segmentIndex, Vector3 prevStartPos)
	{
		LayerMask mask = 1 << LayerMask.NameToLayer("LineOfSight") | 1 << LayerMask.NameToLayer("DynamicLineOfSight");
		RaycastHit raycastHit;
		collisionWithGeo = Physics.Raycast(startPosForBounce, dir, out raycastHit, maxDistance, mask);
		if (collisionWithGeo)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.CalculateActorBounce(Vector3, Vector3, Vector3, float, ActorData, bool, float, List<Team>, ActorData, bool, Vector3*, bool*, Vector3*, bool*, ActorData*, List<NonActorTargetInfo>, int, Vector3)).MethodHandle;
			}
			Vector3 a = raycastHit.point - startPosForBounce;
			a.y = 0f;
			float magnitude = a.magnitude;
			a.Normalize();
			endPoint = startPosForBounce + a * Mathf.Max(0f, magnitude - 0.05f);
			collisionNormal = raycastHit.normal;
		}
		else
		{
			endPoint = startPosForBounce + dir * maxDistance;
			collisionNormal = Vector3.zero;
		}
		if (BarrierManager.Get() != null)
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
			bool flag;
			Vector3 vector;
			Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(caster, startPosForBounce, endPoint, out flag, out vector, nonActorTargetInfo);
			if (flag)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				endPoint = abilityLineEndpoint;
				collisionWithGeo = true;
				collisionNormal = vector;
			}
		}
		hitActorFirst = false;
		bounceHitActor = null;
		float actorTargetingRadius = AreaEffectUtils.GetActorTargetingRadius();
		List<ActorData> list;
		if (GameWideData.Get().UseActorRadiusForLaser())
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
			list = AreaEffectUtils.GetActorsInBoxByActorRadius(startPosForGameplay, endPoint, bounceTestWidthInSquares, false, caster, hitTeams, null, null);
		}
		else
		{
			list = AreaEffectUtils.GetActorsInBox(startPosForGameplay, endPoint, bounceTestWidthInSquares, true, caster, hitTeams);
		}
		List<ActorData> list2 = list;
		list2.Remove(caster);
		if (excludeActor != null)
		{
			list2.Remove(excludeActor);
		}
		if (!includeInvisibles)
		{
			if (NetworkServer.active)
			{
				TargeterUtils.RemoveActorsInvisibleToActor(ref list2, caster);
			}
			else
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref list2);
			}
		}
		Vector3 a2 = endPoint - startPosForBounce;
		a2.y = 0f;
		a2.Normalize();
		if (segmentIndex > 0)
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
			if (list2.Count > 0)
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
				Vector3 b = prevStartPos - startPosForBounce;
				b.y = 0f;
				b.Normalize();
				Vector3 collisionNormal2 = 0.5f * (a2 + b);
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					BoardSquare currentBoardSquare = list2[i].GetCurrentBoardSquare();
					if (!VectorUtils.SquareOnSameSideAsBounceBend(currentBoardSquare, startPosForBounce, collisionNormal2))
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
						list2.RemoveAt(i);
					}
				}
				for (;;)
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
		TargeterUtils.SortActorsByDistanceToPos(ref list2, startPosForBounce);
		if (bounceOnActors)
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
			int num = 0;
			int num2 = 0;
			while (num2 < list2.Count && !hitActorFirst)
			{
				ActorData actorData = list2[num2];
				Vector3 vector2 = actorData.GetTravelBoardSquareWorldPosition() - startPosForBounce;
				vector2.y = 0f;
				float magnitude2 = vector2.magnitude;
				Vector3 vector3 = endPoint - startPosForBounce;
				vector3.y = 0f;
				float magnitude3 = vector3.magnitude;
				if (magnitude2 < magnitude3)
				{
					Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
					float num3 = 0.5f * bounceTestWidthInSquares * Board.Get().squareSize;
					if (GameWideData.Get().UseActorRadiusForLaser())
					{
						num3 += actorTargetingRadius * Board.Get().squareSize;
					}
					num3 = Mathf.Min(0.4f * Board.Get().squareSize, num3);
					Vector3 vector4;
					Vector3 vector5;
					int lineCircleIntersections = VectorUtils.GetLineCircleIntersections(startPosForBounce, endPoint, travelBoardSquareWorldPosition, num3, out vector4, out vector5);
					if (lineCircleIntersections > 1)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						float num4 = VectorUtils.HorizontalPlaneDistInWorld(vector4, startPosForBounce);
						float num5 = VectorUtils.HorizontalPlaneDistInWorld(vector5, startPosForBounce);
						endPoint = ((num4 > num5) ? vector5 : vector4);
						endPoint.y = startPosForBounce.y;
						collisionNormal = endPoint - travelBoardSquareWorldPosition;
						collisionNormal.y = 0f;
						collisionNormal.Normalize();
						float num6 = 0.5f * AreaEffectUtils.GetMaxAngleForActorBounce();
						collisionNormal = Vector3.RotateTowards(-a2, collisionNormal, 0.0174532924f * num6, 0f);
						hitActorFirst = true;
						collisionWithGeo = false;
						bounceHitActor = actorData;
						num = num2;
					}
				}
				num2++;
			}
			if (hitActorFirst)
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
				TargeterUtils.LimitActorsToMaxNumber(ref list2, num + 1);
			}
		}
		return list2;
	}

	public unsafe static Vector3 GetLineLineIntersection(Vector3 pointOnFirst, Vector3 directionOfFirst, Vector3 pointOnSecond, Vector3 directionOfSecond, out bool intersecting)
	{
		Vector3 vector = pointOnFirst;
		Vector3 a = pointOnSecond;
		Vector3 vector2 = directionOfFirst;
		Vector3 rhs = directionOfSecond;
		vector.y = 0f;
		a.y = 0f;
		vector2.y = 0f;
		vector2.Normalize();
		rhs.y = 0f;
		rhs.Normalize();
		Vector3 vector3 = Vector3.Cross(vector2, rhs);
		Vector3 lhs = a - vector;
		if (vector3.magnitude == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetLineLineIntersection(Vector3, Vector3, Vector3, Vector3, bool*)).MethodHandle;
			}
			intersecting = false;
			return Vector3.zero;
		}
		intersecting = true;
		float d = Vector3.Cross(lhs, rhs).magnitude / vector3.magnitude;
		return vector + d * vector2;
	}

	public unsafe static int GetLineCircleIntersections(Vector3 testPoint1, Vector3 testPoint2, Vector3 circleCenter, float radius, out Vector3 intersectP1, out Vector3 intersectP2)
	{
		intersectP1 = Vector3.zero;
		intersectP2 = Vector3.zero;
		testPoint1.y = 0f;
		testPoint2.y = 0f;
		circleCenter.y = 0f;
		Vector3 vector = testPoint1 - circleCenter;
		Vector3 vector2 = testPoint2 - circleCenter;
		float num = vector2.x - vector.x;
		float num2 = vector2.z - vector.z;
		float num3 = num * num + num2 * num2;
		float num4 = vector.x * vector2.z - vector2.x * vector.z;
		float num5 = radius * radius * num3 - num4 * num4;
		if (num3 == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetLineCircleIntersections(Vector3, Vector3, Vector3, float, Vector3*, Vector3*)).MethodHandle;
			}
			return 0;
		}
		if (Mathf.Abs(num5) < 0.001f)
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
			intersectP1.x = num4 * num2 / num3;
			intersectP1.z = -num4 * num / num3;
			intersectP1 += circleCenter;
			return 1;
		}
		if (num5 < 0f)
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
			return 0;
		}
		float num6;
		if (num2 < 0f)
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
			num6 = -1f;
		}
		else
		{
			num6 = 1f;
		}
		float num7 = num6;
		float num8 = Mathf.Sqrt(num5);
		intersectP1.x = (num4 * num2 + num7 * num * num8) / num3;
		intersectP1.z = (-num4 * num + Mathf.Abs(num2) * num8) / num3;
		intersectP2.x = (num4 * num2 - num7 * num * num8) / num3;
		intersectP2.z = (-num4 * num - Mathf.Abs(num2) * num8) / num3;
		intersectP1 += circleCenter;
		intersectP2 += circleCenter;
		return 2;
	}

	public static bool IsSegmentIntersectingCircle(Vector3 startPos, Vector3 endPos, Vector3 circleCenter, float radius)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		circleCenter.y = 0f;
		Vector3 vector = endPos - startPos;
		Vector3 a;
		Vector3 a2;
		int lineCircleIntersections = VectorUtils.GetLineCircleIntersections(startPos, endPos, circleCenter, radius, out a, out a2);
		if (lineCircleIntersections >= 1)
		{
			float num = Vector3.Dot(vector, vector);
			Vector3 rhs = a - startPos;
			float num2 = Vector3.Dot(vector, rhs);
			if (num2 >= 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.IsSegmentIntersectingCircle(Vector3, Vector3, Vector3, float)).MethodHandle;
				}
				if (num2 <= num)
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
					return true;
				}
			}
			if (lineCircleIntersections >= 2)
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
				rhs = a2 - startPos;
				num2 = Vector3.Dot(vector, rhs);
				if (num2 >= 0f)
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
					if (num2 <= num)
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
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool OnSameSideOfLine(Vector3 testPoint1, Vector3 testPoint2, Vector3 linePtA, Vector3 linePtB)
	{
		Vector3 lhs = linePtB - linePtA;
		lhs.y = 0f;
		Vector3 rhs = testPoint1 - linePtA;
		rhs.y = 0f;
		Vector3 rhs2 = testPoint2 - linePtA;
		rhs2.y = 0f;
		Vector3 lhs2 = Vector3.Cross(lhs, rhs);
		Vector3 rhs3 = Vector3.Cross(lhs, rhs2);
		float num = Vector3.Dot(lhs2, rhs3);
		return num >= 0f;
	}

	public static bool IsPointInTriangle(Vector3 triA, Vector3 triB, Vector3 triC, Vector3 testPt)
	{
		bool flag = VectorUtils.OnSameSideOfLine(testPt, triA, triB, triC);
		bool result = VectorUtils.OnSameSideOfLine(testPt, triB, triA, triC);
		bool flag2 = VectorUtils.OnSameSideOfLine(testPt, triC, triA, triB);
		if (flag2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.IsPointInTriangle(Vector3, Vector3, Vector3, Vector3)).MethodHandle;
			}
			if (flag)
			{
				return result;
			}
		}
		return false;
	}

	public static bool IsPointInLaser(Vector3 testPoint, Vector3 laserStartPos, Vector3 laserEndPos, float laserWidthInWorld)
	{
		testPoint.y = 0f;
		laserStartPos.y = 0f;
		laserEndPos.y = 0f;
		float sqrMagnitude = (laserEndPos - laserStartPos).sqrMagnitude;
		Vector3 normalized = (laserEndPos - laserStartPos).normalized;
		Vector3 lhs = testPoint - laserStartPos;
		Vector3 vector = laserStartPos + Vector3.Dot(lhs, normalized) * normalized;
		float sqrMagnitude2 = (vector - laserStartPos).sqrMagnitude;
		float sqrMagnitude3 = (laserEndPos - vector).sqrMagnitude;
		bool flag;
		if (sqrMagnitude2 < sqrMagnitude)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.IsPointInLaser(Vector3, Vector3, Vector3, float)).MethodHandle;
			}
			flag = (sqrMagnitude3 < sqrMagnitude);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		float sqrMagnitude4 = (vector - testPoint).sqrMagnitude;
		float num = laserWidthInWorld / 2f * (laserWidthInWorld / 2f);
		bool flag3 = sqrMagnitude4 < num;
		bool result;
		if (flag2)
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
			result = flag3;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public static Vector3 GetProjectionPoint(Vector3 normalizedDir, Vector3 startPos, Vector3 pointToProject)
	{
		Vector3 lhs = pointToProject - startPos;
		return startPos + Vector3.Dot(lhs, normalizedDir) * normalizedDir;
	}

	public static float HorizontalAngle_Rad(Vector3 vec)
	{
		Vector2 vector = new Vector2(vec.x, vec.z);
		vector.Normalize();
		return Mathf.Atan2(vector.y, vector.x);
	}

	public static float HorizontalAngle_Deg(Vector3 vec)
	{
		float num = VectorUtils.HorizontalAngle_Rad(vec);
		float num2 = num * 57.29578f;
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		return num2;
	}

	public static float ClampAngle_Deg(float angle, float min, float max)
	{
		float num;
		if (min > max)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.ClampAngle_Deg(float, float, float)).MethodHandle;
			}
			Debug.LogError(string.Format("Clamping an angle {0} to a min of {1} and a max of {2}, but min is greater than max.", angle, min, max));
			num = angle;
		}
		else if (min == max)
		{
			num = min;
		}
		else
		{
			if (angle >= min)
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
				if (angle <= max)
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
					num = angle;
					goto IL_148;
				}
			}
			if (angle + 360f >= min)
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
				if (angle + 360f <= max)
				{
					num = angle;
					goto IL_148;
				}
			}
			if (angle - 360f >= min)
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
				if (angle - 360f <= max)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num = angle;
					goto IL_148;
				}
			}
			float num2 = Mathf.Clamp(angle, min, max);
			float num3 = Mathf.Clamp(angle, min + 360f, max + 360f);
			float num4 = Mathf.Clamp(angle, min - 360f, max - 360f);
			float num5 = Mathf.Abs(angle - num2);
			float num6 = Mathf.Abs(angle - num3);
			float num7 = Mathf.Abs(angle - num4);
			if (num5 <= num6 && num5 <= num7)
			{
				num = num2;
			}
			else if (num6 <= num5 && num6 <= num7)
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
				num = num3;
			}
			else
			{
				num = num4;
			}
		}
		IL_148:
		while (num > 360f)
		{
			num -= 360f;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		while (num < 0f)
		{
			num += 360f;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		return num;
	}

	public static Vector3 AngleRadToVector(float angle)
	{
		Vector3 result = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
		return result;
	}

	public static Vector3 AngleDegreesToVector(float angle)
	{
		float angle2 = angle * 0.0174532924f;
		return VectorUtils.AngleRadToVector(angle2);
	}

	public static Vector3 GetDirectionToClosestSide(BoardSquare square, Vector3 testPos)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (square != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetDirectionToClosestSide(BoardSquare, Vector3)).MethodHandle;
			}
			Vector3 b = square.ToVector3();
			Vector3 vec = testPos - b;
			vec.y = 0f;
			if (vec.magnitude < 0.1f)
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
				result = new Vector3(1f, 0f, 0f);
			}
			else
			{
				int angleWithHorizontal = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vec));
				result = VectorUtils.HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
			}
		}
		return result;
	}

	public static Vector3 HorizontalAngleToClosestCardinalDirection(int angleWithHorizontal)
	{
		Vector3 result;
		if (angleWithHorizontal > 0x2D && angleWithHorizontal <= 0x87)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.HorizontalAngleToClosestCardinalDirection(int)).MethodHandle;
			}
			result = new Vector3(0f, 0f, 1f);
		}
		else
		{
			if (angleWithHorizontal > 0x87)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (angleWithHorizontal <= 0xE1)
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
					result = new Vector3(-1f, 0f, 0f);
					return result;
				}
			}
			if (angleWithHorizontal > 0xE1)
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
				if (angleWithHorizontal <= 0x13B)
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
					result = new Vector3(0f, 0f, -1f);
					return result;
				}
			}
			result = new Vector3(1f, 0f, 0f);
		}
		return result;
	}

	public unsafe static Vector3 GetDirectionAndOffsetToClosestSide(BoardSquare square, Vector3 testPos, bool allowDiagonalAim, out Vector3 offset)
	{
		Vector3 vector = new Vector3(1f, 0f, 0f);
		offset = 0.5f * Board.Get().squareSize * vector;
		if (square != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VectorUtils.GetDirectionAndOffsetToClosestSide(BoardSquare, Vector3, bool, Vector3*)).MethodHandle;
			}
			Vector3 b = square.ToVector3();
			Vector3 vec = testPos - b;
			vec.y = 0f;
			if (vec.magnitude < 0.1f)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return vector;
			}
			if (!allowDiagonalAim)
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
				vector = VectorUtils.GetDirectionToClosestSide(square, testPos);
				offset = 0.5f * Board.Get().squareSize * vector;
			}
			else
			{
				int num = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vec));
				bool flag = false;
				float angle;
				if (num < 0x151)
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
					if (num <= 0x17)
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
					}
					else
					{
						if (num < 0x43)
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
							angle = 45f;
							flag = true;
							goto IL_1C2;
						}
						if (num <= 0x71)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							angle = 90f;
							goto IL_1C2;
						}
						if (num < 0x9D)
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
							angle = 135f;
							flag = true;
							goto IL_1C2;
						}
						if (num <= 0xCB)
						{
							angle = 180f;
							goto IL_1C2;
						}
						if (num < 0xF7)
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
							angle = 225f;
							flag = true;
							goto IL_1C2;
						}
						if (num <= 0x125)
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
							angle = 270f;
							goto IL_1C2;
						}
						angle = 315f;
						flag = true;
						goto IL_1C2;
					}
				}
				angle = 0f;
				IL_1C2:
				vector = VectorUtils.AngleDegreesToVector(angle);
				if (flag)
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
					float num2 = 0.5f * Mathf.Sqrt(2f) - 0.01f;
					offset = num2 * Board.Get().squareSize * vector;
				}
				else
				{
					offset = 0.5f * Board.Get().squareSize * vector;
				}
			}
		}
		return vector;
	}

	public static float HorizontalPlaneDistInWorld(Vector3 a, Vector3 b)
	{
		Vector3 vector = b - a;
		vector.y = 0f;
		return vector.magnitude;
	}

	public static float HorizontalPlaneDistInSquares(Vector3 a, Vector3 b)
	{
		return VectorUtils.HorizontalPlaneDistInWorld(a, b) / Board.Get().squareSize;
	}

	public struct LaserCoords
	{
		public Vector3 start;

		public Vector3 end;

		public float Length()
		{
			return Vector3.Magnitude(this.start - this.end);
		}

		public Vector3 Direction()
		{
			return (this.end - this.start).normalized;
		}
	}
}
