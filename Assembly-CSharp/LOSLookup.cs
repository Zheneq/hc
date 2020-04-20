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
			if (this.m_boardSquares[xSource + ySource * this.m_maxX].m_LOS.Length > 0)
			{
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
