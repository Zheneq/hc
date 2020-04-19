using System;

[Serializable]
public class GridPosProp
{
	public int m_x;

	public int m_y;

	public int m_height;

	public static GridPosProp FromGridPos(GridPos gp)
	{
		return new GridPosProp
		{
			m_x = gp.x,
			m_y = gp.y,
			m_height = gp.height
		};
	}
}
