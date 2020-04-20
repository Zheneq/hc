using System;
using UnityEngine;

public struct GridPos
{
	internal static GridPos s_invalid = new GridPos(-1, -1, 0);

	private int m_x;

	private int m_y;

	private int m_height;

	public GridPos(int x, int y, int height)
	{
		this.m_x = x;
		this.m_y = y;
		this.m_height = height;
	}

	public int x
	{
		get
		{
			return this.m_x;
		}
		set
		{
			this.m_x = value;
		}
	}

	public int y
	{
		get
		{
			return this.m_y;
		}
		set
		{
			this.m_y = value;
		}
	}

	public int height
	{
		get
		{
			return this.m_height;
		}
		set
		{
			this.m_height = value;
		}
	}

	public float worldX
	{
		get
		{
			return (float)this.m_x * Board.SquareSizeStatic;
		}
	}

	public float worldY
	{
		get
		{
			return (float)this.m_y * Board.SquareSizeStatic;
		}
	}

	public override string ToString()
	{
		return string.Format("({0}, {1})", this.x, this.y);
	}

	public string ToStringWithCross()
	{
		return string.Format("({0} x {1})", this.x, this.y);
	}

	public static GridPos FromVector3(Vector3 vec)
	{
		return new GridPos
		{
			x = Mathf.RoundToInt(vec.x / Board.Get().squareSize),
			y = Mathf.RoundToInt(vec.z / Board.Get().squareSize)
		};
	}

	public static GridPos FromGridPosProp(GridPosProp gpp)
	{
		return new GridPos
		{
			m_x = gpp.m_x,
			m_y = gpp.m_y,
			m_height = gpp.m_height
		};
	}

	public bool CoordsEqual(GridPos other)
	{
		bool result;
		if (this.x == other.x)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GridPos.CoordsEqual(GridPos)).MethodHandle;
			}
			result = (this.y == other.y);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		int x = this.m_x;
		int y = this.m_y;
		int height = this.m_height;
		stream.Serialize(ref x);
		stream.Serialize(ref y);
		stream.Serialize(ref height);
		this.m_x = x;
		this.m_y = y;
		this.m_height = height;
	}
}
