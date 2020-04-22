using UnityEngine;

public class BoardRegionEditorDefinition : MonoBehaviour
{
	private BoardRegion m_region;

	[Tooltip("Horizontal size of this region in board squares.")]
	public int m_boardSquareSizeX = 3;

	[Tooltip("Vertical size of this region in board squares.")]
	public int m_boardSquareSizeY = 3;

	[Tooltip("Color to show in Editor.  Not used in the game.")]
	public Color m_gizmoColor = Color.magenta;

	private Vector3 m_lastGizmoPos = Vector3.zero;

	public BoardRegion GetRegion()
	{
		return m_region;
	}

	protected virtual void Start()
	{
		CreateRegion();
	}

	private void CreateRegion()
	{
		Board board = Board.Get();
		if (!(board != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_boardSquareSizeX > 0 && m_boardSquareSizeY > 0)
			{
				m_region = new BoardRegion();
				float num = (float)(m_boardSquareSizeX - 1) * board.squareSize * 0.5f;
				float num2 = (float)(m_boardSquareSizeY - 1) * board.squareSize * 0.5f;
				Vector3 position = base.transform.position;
				float x = position.x - num;
				Vector3 position2 = base.transform.position;
				Vector3 worldCorner = new Vector3(x, 0f, position2.z - num2);
				Vector3 position3 = base.transform.position;
				float x2 = position3.x + num;
				Vector3 position4 = base.transform.position;
				Vector3 worldCorner2 = new Vector3(x2, 0f, position4.z + num2);
				m_region.InitializeAsRect(worldCorner, worldCorner2);
			}
			return;
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
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		if (m_region != null)
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
			if (!((m_lastGizmoPos - base.transform.position).sqrMagnitude > 0f))
			{
				goto IL_0081;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		CreateRegion();
		m_lastGizmoPos = base.transform.position;
		goto IL_0081;
		IL_0081:
		m_region.GizmosDrawRegion(m_gizmoColor);
	}

	private void OnValidate()
	{
		m_region = null;
	}
}
