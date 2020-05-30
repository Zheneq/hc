using UnityEngine;

public class AbilityCommon_LaserWithCone
{
	public static Vector3 GetConeLosCheckPos(Vector3 startPos, Vector3 endPos)
	{
		AreaEffectUtils.GetEndPointForValidGameplaySquare(startPos, endPos, out Vector3 adjustedEndPoint);
		BoardSquare boardSquare = Board.Get().GetSquare(adjustedEndPoint);
		if (boardSquare != null && boardSquare.HasFullCoverFromAnyDirection_zq())
		{
			Vector3 vector = adjustedEndPoint - startPos;
			vector.y = 0f;
			float maxDistance = vector.magnitude + 0.1f;
			vector.Normalize();
			Vector3 origin = adjustedEndPoint - Board.SquareSizeStatic * 1.415f * vector;
			LayerMask mask = 1 << VectorUtils.s_raycastLayerDynamicLineOfSight;
			if (Physics.Raycast(origin, vector, out RaycastHit hitInfo, maxDistance, mask))
			{
				if ((hitInfo.collider.gameObject.layer & VectorUtils.s_raycastLayerDynamicLineOfSight) != 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return hitInfo.point - 0.1f * vector;
						}
					}
				}
			}
		}
		return adjustedEndPoint;
	}
}
