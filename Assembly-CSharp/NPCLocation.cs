using UnityEngine;

public class NPCLocation : MonoBehaviour
{
	private BoardSquare m_boardSquare;

	public BoardSquare boardSquare
	{
		get
		{
			if (m_boardSquare == null)
			{
				Board board = Board.Get();
				Vector3 position = base.transform.position;
				float x = position.x;
				Vector3 position2 = base.transform.position;
				m_boardSquare = board.GetSquareAtPosition(x, position2.z);
			}
			return m_boardSquare;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_NPC.png");
	}
}
