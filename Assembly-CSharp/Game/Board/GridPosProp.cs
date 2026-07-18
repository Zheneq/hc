using System;

[Serializable]
public class GridPosProp
{
	public int m_x;

	public int m_y;

	public int m_height;

	public static GridPosProp FromGridPos(GridPos gp)
	{
		GridPosProp gridPosProp = new GridPosProp();
		gridPosProp.m_x = gp.x;
		gridPosProp.m_y = gp.y;
		gridPosProp.m_height = gp.height;
		return gridPosProp;
	}
}
