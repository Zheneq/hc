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
		if (!(this.m_corner1 == null))
		{
			if (!(this.m_corner2 == null))
			{
				BoardSquare a;
				if (this.m_corner1.GetComponent<BoardSquare>() == null)
				{
					a = Board.Get().GetBoardSquare(this.m_corner1);
				}
				else
				{
					a = this.m_corner1.GetComponent<BoardSquare>();
				}
				BoardSquare b;
				if (this.m_corner2.GetComponent<BoardSquare>() == null)
				{
					b = Board.Get().GetBoardSquare(this.m_corner2);
				}
				else
				{
					b = this.m_corner2.GetComponent<BoardSquare>();
				}
				return Board.Get().GetSquaresInRect(a, b);
			}
		}
		List<BoardSquare> result = new List<BoardSquare>();
		if (Application.isEditor)
		{
			Log.Error("BoardRegion " + this.m_name + " has a BoardQuad with null corners.", new object[0]);
		}
		return result;
	}
}
