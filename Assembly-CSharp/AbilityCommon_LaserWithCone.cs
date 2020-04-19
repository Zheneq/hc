using System;
using UnityEngine;

public class AbilityCommon_LaserWithCone
{
	public static Vector3 GetConeLosCheckPos(Vector3 startPos, Vector3 endPos)
	{
		Vector3 vector;
		AreaEffectUtils.GetEndPointForValidGameplaySquare(startPos, endPos, out vector);
		BoardSquare boardSquare = Board.\u000E().\u000E(vector);
		if (boardSquare != null && boardSquare.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCommon_LaserWithCone.GetConeLosCheckPos(Vector3, Vector3)).MethodHandle;
			}
			Vector3 vector2 = vector - startPos;
			vector2.y = 0f;
			float maxDistance = vector2.magnitude + 0.1f;
			vector2.Normalize();
			Vector3 origin = vector - Board.SquareSizeStatic * 1.415f * vector2;
			LayerMask mask = 1 << VectorUtils.s_raycastLayerDynamicLineOfSight;
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(origin, vector2, out raycastHit, maxDistance, mask);
			if (flag)
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
				if ((raycastHit.collider.gameObject.layer & VectorUtils.s_raycastLayerDynamicLineOfSight) != 0)
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
					vector = raycastHit.point - 0.1f * vector2;
				}
			}
		}
		return vector;
	}
}
