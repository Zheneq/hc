using System;
using UnityEngine;

public class LOSLookup : MonoBehaviour
{
	[Serializable]
	public class BoardSquareLOSLookup
	{
		public float[] m_LOS;
	}

	public BoardSquareLOSLookup[] m_boardSquares;

	public int m_maxX;

	public int m_maxY;

	private void Start()
	{
	}

	public float GetLOSDistance(int xSource, int ySource, int xDest, int yDest)
	{
		float result = 0f;
		if (m_boardSquares[xSource + ySource * m_maxX].m_LOS != null)
		{
			if (m_boardSquares[xSource + ySource * m_maxX].m_LOS.Length > 0)
			{
				result = m_boardSquares[xSource + ySource * m_maxX].m_LOS[xDest + yDest * m_maxX];
			}
		}
		return result;
	}
}
