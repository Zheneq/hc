using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardQuad
{
	public string m_name = "Region";

	public Transform m_corner1;

	public Transform m_corner2;

	public List<BoardSquare> GetSquares()
	{
		List<BoardSquare> result;
		if (!(m_corner1 == null))
		{
			if (!(m_corner2 == null))
			{
				BoardSquare a;
				if (m_corner1.GetComponent<BoardSquare>() == null)
				{
					a = Board.Get().GetSquareFromTransform(m_corner1);
				}
				else
				{
					a = m_corner1.GetComponent<BoardSquare>();
				}
				BoardSquare b = (!(m_corner2.GetComponent<BoardSquare>() == null)) ? m_corner2.GetComponent<BoardSquare>() : Board.Get().GetSquareFromTransform(m_corner2);
				result = Board.Get().GetSquaresInRect(a, b);
				goto IL_0100;
			}
		}
		result = new List<BoardSquare>();
		if (Application.isEditor)
		{
			Log.Error("BoardRegion " + m_name + " has a BoardQuad with null corners.");
		}
		goto IL_0100;
		IL_0100:
		return result;
	}
}
