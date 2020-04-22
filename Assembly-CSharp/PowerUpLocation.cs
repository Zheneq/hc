using UnityEngine;

public class PowerUpLocation : MonoBehaviour
{
	private BoardSquare m_boardSquare;

	public BoardSquare boardSquare => m_boardSquare;

	public void Initialize()
	{
		Board board = Board.Get();
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		m_boardSquare = board.GetBoardSquareSafe(x, position2.z);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
	}
}
