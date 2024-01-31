using UnityEngine;

public class AbilityCommon_LaserWithCone
{
	private const float Sqrt2 = 1.415f;
	
	public static Vector3 GetConeLosCheckPos(Vector3 startPos, Vector3 endPos)
	{
		AreaEffectUtils.GetEndPointForValidGameplaySquare(startPos, endPos, out Vector3 adjustedEndPoint);
		BoardSquare boardSquare = Board.Get().GetSquareFromVec3(adjustedEndPoint);
		if (boardSquare != null && boardSquare.IsNextToFullCover())
		{
			Vector3 vector = adjustedEndPoint - startPos;
			vector.y = 0f;
			float maxDistance = vector.magnitude + 0.1f; // TODO CLIENT BUG looks like it's supposed to be Board.SquareSizeStatic * Sqrt2 + 0.1f
			vector.Normalize();
			Vector3 origin = adjustedEndPoint - Board.SquareSizeStatic * Sqrt2 * vector;
			LayerMask mask = 1 << VectorUtils.s_raycastLayerDynamicLineOfSight;
			if (Physics.Raycast(origin, vector, out RaycastHit hitInfo, maxDistance, mask)
			    && (hitInfo.collider.gameObject.layer & VectorUtils.s_raycastLayerDynamicLineOfSight) != 0)
			{
				return hitInfo.point - 0.1f * vector;
			}
		}
		return adjustedEndPoint;
	}
}
