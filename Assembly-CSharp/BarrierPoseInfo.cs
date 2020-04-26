using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPoseInfo
{
	public Vector3 midpoint;

	public Vector3 facingDirection;

	public float widthInWorld;

	public BarrierPoseInfo(Vector3 midPos, Vector3 facing, float barrierWidthInWorld)
	{
		midpoint = midPos;
		facingDirection = facing;
		widthInWorld = barrierWidthInWorld;
	}

	public static List<BarrierPoseInfo> GetBarrierPosesForRegularPolygon(Vector3 centerPos, int numSides, float radiusInWorld, float angleOffsetDeg = 0f)
	{
		if (numSides > 2)
		{
			if (radiusInWorld > 0f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						List<BarrierPoseInfo> list = new List<BarrierPoseInfo>();
						float num = 360f / (float)numSides;
						float barrierWidthInWorld = 2f * radiusInWorld * Mathf.Tan((float)Math.PI / 180f * num / 2f);
						for (int i = 0; i < numSides; i++)
						{
							Vector3 vector = VectorUtils.AngleDegreesToVector(angleOffsetDeg + (float)i * num);
							Vector3 midPos = centerPos + radiusInWorld * vector;
							BarrierPoseInfo item = new BarrierPoseInfo(midPos, vector, barrierWidthInWorld);
							list.Add(item);
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return list;
							}
						}
					}
					}
				}
			}
		}
		return null;
	}

	public static bool GetBarrierPosesForSquaresMadeOfCornerAndMidsection(BoardSquare targetSquare, float cornerLength, float midsectionLength, float cornerLengthAdjustInSquares, out List<List<BarrierPoseInfo>> corners, out List<BarrierPoseInfo> midSections)
	{
		corners = new List<List<BarrierPoseInfo>>();
		midSections = new List<BarrierPoseInfo>();
		if (cornerLength > 0f)
		{
			if (midsectionLength >= 0f)
			{
				if (targetSquare != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
						{
							float squareSize = Board.Get().squareSize;
							float num = cornerLengthAdjustInSquares * squareSize;
							float num2 = cornerLength * squareSize + num;
							float num3 = midsectionLength * squareSize - num;
							Vector3 vector = targetSquare.ToVector3();
							vector.y = Board.Get().BaselineHeight;
							Vector3 a = vector;
							if (midsectionLength % 2f == 0f)
							{
								a += squareSize * new Vector3(0.5f, 0f, 0.5f);
							}
							float num4 = 0.5f * num3 + num2;
							float num5 = num4 - 0.5f * num2 + 0.01f;
							int[,] array = new int[4, 2]
							{
								{
									-1,
									1
								},
								{
									1,
									1
								},
								{
									-1,
									-1
								},
								{
									1,
									-1
								}
							};
							Vector3 vector2 = new Vector3(1f, 0f, 0f);
							Vector3 vector3 = new Vector3(0f, 0f, 1f);
							Vector3[] array2 = new Vector3[4]
							{
								-vector2,
								vector2,
								-vector3,
								vector3
							};
							for (int i = 0; i < 4; i++)
							{
								List<BarrierPoseInfo> list = new List<BarrierPoseInfo>();
								int num6 = array[i, 0];
								int num7 = array[i, 1];
								Vector3 midPos = a + new Vector3((float)num6 * num4, 0f, (float)num7 * num5);
								BarrierPoseInfo item = new BarrierPoseInfo(midPos, vector2 * num6, num2);
								Vector3 midPos2 = a + new Vector3((float)num6 * num5, 0f, (float)num7 * num4);
								BarrierPoseInfo item2 = new BarrierPoseInfo(midPos2, vector3 * num7, num2);
								list.Add(item);
								list.Add(item2);
								corners.Add(list);
								if (midsectionLength > 0f)
								{
									Vector3 midPos3 = a + num4 * array2[i];
									midSections.Add(new BarrierPoseInfo(midPos3, array2[i], num3));
								}
							}
							return true;
						}
						}
					}
				}
			}
		}
		return false;
	}
}
