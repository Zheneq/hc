using UnityEngine;

public struct GridPos
{
	internal static GridPos s_invalid = new GridPos(-1, -1, 0);

	private int m_x;

	private int m_y;

	private int m_height;

	public int x
	{
		get
		{
			return m_x;
		}
		set
		{
			m_x = value;
		}
	}

	public int y
	{
		get
		{
			return m_y;
		}
		set
		{
			m_y = value;
		}
	}

	public int height
	{
		get
		{
			return m_height;
		}
		set
		{
			m_height = value;
		}
	}

	public float worldX => (float)m_x * Board.SquareSizeStatic;

	public float worldY => (float)m_y * Board.SquareSizeStatic;

	public GridPos(int x, int y, int height)
	{
		m_x = x;
		m_y = y;
		m_height = height;
	}

	public override string ToString()
	{
		return $"({x}, {y})";
	}

	public string ToStringWithCross()
	{
		return $"({x} x {y})";
	}

	public static GridPos FromVector3(Vector3 vec)
	{
		GridPos result = default(GridPos);
		result.x = Mathf.RoundToInt(vec.x / Board.Get().squareSize);
		result.y = Mathf.RoundToInt(vec.z / Board.Get().squareSize);
		return result;
	}

	public static GridPos FromGridPosProp(GridPosProp gpp)
	{
		GridPos result = default(GridPos);
		result.m_x = gpp.m_x;
		result.m_y = gpp.m_y;
		result.m_height = gpp.m_height;
		return result;
	}

	public bool CoordsEqual(GridPos other)
	{
		int result;
		if (x == other.x)
		{
			result = ((y == other.y) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		int value = m_x;
		int value2 = m_y;
		int value3 = m_height;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		m_x = value;
		m_y = value2;
		m_height = value3;
	}
}
