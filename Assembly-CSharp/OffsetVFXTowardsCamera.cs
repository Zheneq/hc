using System;
using UnityEngine;

public class OffsetVFXTowardsCamera : MonoBehaviour
{
	public static Vector3 ProcessOffset(Vector3 position)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(position);
		Camera main = Camera.main;
		if (main)
		{
			if (boardSquare)
			{
				if (boardSquare.occupant)
				{
					Vector3 direction = main.WorldToRay(position).direction;
					float num = 0f;
					foreach (Renderer renderer in boardSquare.occupant.GetComponentsInChildren<Renderer>())
					{
						num = Mathf.Max(num, Vector3.Dot(renderer.bounds.extents, MathUtil.Vector3Abs(renderer.transform.rotation * direction)) / 2f);
					}
					return position - direction * num;
				}
			}
		}
		return position;
	}
}
