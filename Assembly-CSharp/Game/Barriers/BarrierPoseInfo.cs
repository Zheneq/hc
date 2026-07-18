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

	public static List<BarrierPoseInfo> GetBarrierPosesForRegularPolygon(
		Vector3 centerPos,
		int numSides,
		float radiusInWorld,
		float angleOffsetDeg = 0f)
	{
		if (numSides <= 2 || radiusInWorld <= 0f)
		{
			return null;
		}
		float angleStep = 360f / numSides;
		float barrierWidthInWorld = 2f * radiusInWorld * Mathf.Tan((float)Math.PI / 180f * angleStep / 2f);
		List<BarrierPoseInfo> list = new List<BarrierPoseInfo>();
		for (int i = 0; i < numSides; i++)
		{
			Vector3 facing = VectorUtils.AngleDegreesToVector(angleOffsetDeg + i * angleStep);
			Vector3 midPos = centerPos + radiusInWorld * facing;
			list.Add(new BarrierPoseInfo(midPos, facing, barrierWidthInWorld));
		}
		return list;
	}

	public static bool GetBarrierPosesForSquaresMadeOfCornerAndMidsection(
		BoardSquare targetSquare,
		float cornerLength,
		float midsectionLength,
		float cornerLengthAdjustInSquares,
		out List<List<BarrierPoseInfo>> corners,
		out List<BarrierPoseInfo> midSections)
	{
		corners = new List<List<BarrierPoseInfo>>();
		midSections = new List<BarrierPoseInfo>();
		if (!(cornerLength > 0f)
		    || !(midsectionLength >= 0f)
		    || targetSquare == null)
		{
			return false;
		}
		float squareSize = Board.Get().squareSize;
		float cornerLengthAdjust = cornerLengthAdjustInSquares * squareSize;
		float cornerLengthAdjusted = cornerLength * squareSize + cornerLengthAdjust;
		float midsectionLengthAdjusted = midsectionLength * squareSize - cornerLengthAdjust;
		Vector3 targetPos = targetSquare.ToVector3();
		targetPos.y = Board.Get().BaselineHeight;
		Vector3 targetPosAdjusted = targetPos;
		if (midsectionLength % 2f == 0f)
		{
			targetPosAdjusted += squareSize * new Vector3(0.5f, 0f, 0.5f);
		}
		float shiftA = 0.5f * midsectionLengthAdjusted + cornerLengthAdjusted;
#if VANILLA
		float shiftB = shiftA - 0.5f * cornerLengthAdjusted + 0.1f; // reactor
#else
		float shiftB = shiftA - 0.5f * cornerLengthAdjusted + 0.01f; // custom
#endif
		int[,] array = {
			{ -1,  1 },
			{  1,  1 },
			{ -1, -1 },
			{  1, -1 }
		};
		Vector3 x = new Vector3(1f, 0f, 0f);
		Vector3 y = new Vector3(0f, 0f, 1f);
		Vector3[] sideOffsetUnit = { -x, x, -y, y };
		for (int i = 0; i < 4; i++)
		{
			int xDir = array[i, 0];
			int yDir = array[i, 1];
			corners.Add(new List<BarrierPoseInfo>
			{
				new BarrierPoseInfo(
					targetPosAdjusted + new Vector3(xDir * shiftA, 0f, yDir * shiftB),
					x * xDir,
					cornerLengthAdjusted),
				new BarrierPoseInfo(
					targetPosAdjusted + new Vector3(xDir * shiftB, 0f, yDir * shiftA),
					y * yDir,
					cornerLengthAdjusted)
			});
			if (midsectionLength > 0f)
			{
				midSections.Add(new BarrierPoseInfo(
					targetPosAdjusted + shiftA * sideOffsetUnit[i],
					sideOffsetUnit[i],
					midsectionLengthAdjusted));
			}
		}
		return true;
	}
}
