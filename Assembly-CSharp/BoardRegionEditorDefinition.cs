using System;
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
		return this.m_region;
	}

	protected virtual void Start()
	{
		this.CreateRegion();
	}

	private void CreateRegion()
	{
		Board board = Board.Get();
		if (board != null)
		{
			if (this.m_boardSquareSizeX > 0 && this.m_boardSquareSizeY > 0)
			{
				this.m_region = new BoardRegion();
				float num = (float)(this.m_boardSquareSizeX - 1) * board.squareSize * 0.5f;
				float num2 = (float)(this.m_boardSquareSizeY - 1) * board.squareSize * 0.5f;
				Vector3 worldCorner = new Vector3(base.transform.position.x - num, 0f, base.transform.position.z - num2);
				Vector3 worldCorner2 = new Vector3(base.transform.position.x + num, 0f, base.transform.position.z + num2);
				this.m_region.InitializeAsRect(worldCorner, worldCorner2);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		if (this.m_region != null)
		{
			if ((this.m_lastGizmoPos - base.transform.position).sqrMagnitude <= 0f)
			{
				goto IL_81;
			}
		}
		this.CreateRegion();
		this.m_lastGizmoPos = base.transform.position;
		IL_81:
		this.m_region.GizmosDrawRegion(this.m_gizmoColor);
	}

	private void OnValidate()
	{
		this.m_region = null;
	}
}
