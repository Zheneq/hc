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
			if (!(m_corner2 == null))
			{
				BoardSquare a;
				if (m_corner1.GetComponent<BoardSquare>() == null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					a = Board.Get().GetBoardSquare(m_corner1);
				}
				else
				{
					a = m_corner1.GetComponent<BoardSquare>();
				}
				BoardSquare b = (!(m_corner2.GetComponent<BoardSquare>() == null)) ? m_corner2.GetComponent<BoardSquare>() : Board.Get().GetBoardSquare(m_corner2);
				result = Board.Get().GetSquaresInRect(a, b);
				goto IL_0100;
			}
		}
		result = new List<BoardSquare>();
		if (Application.isEditor)
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
			Log.Error("BoardRegion " + m_name + " has a BoardQuad with null corners.");
		}
		goto IL_0100;
		IL_0100:
		return result;
	}
}
