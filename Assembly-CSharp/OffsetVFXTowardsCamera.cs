using UnityEngine;

public class OffsetVFXTowardsCamera : MonoBehaviour
{
	public static Vector3 ProcessOffset(Vector3 position)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(position);
		Camera main = Camera.main;
		if ((bool)main)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((bool)boardSquare)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if ((bool)boardSquare.occupant)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							Vector3 direction = main.WorldToRay(position).direction;
							float num = 0f;
							Renderer[] componentsInChildren = boardSquare.occupant.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer in componentsInChildren)
							{
								num = Mathf.Max(num, Vector3.Dot(renderer.bounds.extents, MathUtil.Vector3Abs(renderer.transform.rotation * direction)) / 2f);
							}
							return position - direction * num;
						}
						}
					}
				}
			}
		}
		return position;
	}
}
