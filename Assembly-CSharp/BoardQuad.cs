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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardQuad.GetSquares()).MethodHandle;
			}
			if (!(this.m_corner2 == null))
			{
				BoardSquare u001D;
				if (this.m_corner1.GetComponent<BoardSquare>() == null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					u001D = Board.\u000E().\u000E(this.m_corner1);
				}
				else
				{
					u001D = this.m_corner1.GetComponent<BoardSquare>();
				}
				BoardSquare u000E;
				if (this.m_corner2.GetComponent<BoardSquare>() == null)
				{
					u000E = Board.\u000E().\u000E(this.m_corner2);
				}
				else
				{
					u000E = this.m_corner2.GetComponent<BoardSquare>();
				}
				return Board.\u000E().\u000E(u001D, u000E);
			}
		}
		List<BoardSquare> result = new List<BoardSquare>();
		if (Application.isEditor)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("BoardRegion " + this.m_name + " has a BoardQuad with null corners.", new object[0]);
		}
		return result;
	}
}
