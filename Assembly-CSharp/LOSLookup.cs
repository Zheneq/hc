using System;
using UnityEngine;

public class LOSLookup : MonoBehaviour
{
	public LOSLookup.BoardSquareLOSLookup[] m_boardSquares;

	public int m_maxX;

	public int m_maxY;

	private void Start()
	{
	}

	public float GetLOSDistance(int xSource, int ySource, int xDest, int yDest)
	{
		float result = 0f;
		if (this.m_boardSquares[xSource + ySource * this.m_maxX].m_LOS != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LOSLookup.GetLOSDistance(int, int, int, int)).MethodHandle;
			}
			if (this.m_boardSquares[xSource + ySource * this.m_maxX].m_LOS.Length > 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				result = this.m_boardSquares[xSource + ySource * this.m_maxX].m_LOS[xDest + yDest * this.m_maxX];
			}
		}
		return result;
	}

	[Serializable]
	public class BoardSquareLOSLookup
	{
		public float[] m_LOS;
	}
}
