using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardRegion
{
	public BoardQuad[] m_quads;

	private List<BoardSquare> m_squaresInRegion;

	private void CacheSquaresInRegion()
	{
		this.m_squaresInRegion = new List<BoardSquare>();
		if (this.m_quads != null)
		{
			foreach (BoardQuad boardQuad in this.m_quads)
			{
				if (boardQuad == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.CacheSquaresInRegion()).MethodHandle;
					}
					Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.", new object[0]);
				}
				else
				{
					List<BoardSquare> squares = boardQuad.GetSquares();
					using (List<BoardSquare>.Enumerator enumerator = squares.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare item = enumerator.Current;
							if (!this.m_squaresInRegion.Contains(item))
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								this.m_squaresInRegion.Add(item);
							}
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
	}

	public virtual void Initialize()
	{
		this.CacheSquaresInRegion();
	}

	public virtual void InitializeAsRect(Vector3 worldCorner1, Vector3 worldCorner2)
	{
		BoardSquare u001D = Board.\u000E().\u0012(worldCorner1.x, worldCorner1.z);
		BoardSquare u000E = Board.\u000E().\u0012(worldCorner2.x, worldCorner2.z);
		this.m_squaresInRegion = Board.\u000E().\u000E(u001D, u000E);
	}

	public List<BoardSquare> \u001D()
	{
		if (this.m_squaresInRegion == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D()).MethodHandle;
			}
			Log.Error("Did not call CacheSquaresInRegion before calling GetSquaresInRegion.  This will cause slowdowns", new object[0]);
			this.CacheSquaresInRegion();
		}
		return this.m_squaresInRegion;
	}

	public BoardSquare \u001D()
	{
		BoardSquare result = null;
		BoardSquare other = this.\u000E();
		List<BoardSquare> list = this.\u001D();
		float num = 100000f;
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.\u0016())
				{
					float num2 = boardSquare.HorizontalDistanceInSquaresTo(other);
					if (num2 < num)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D()).MethodHandle;
						}
						num = num2;
						result = boardSquare;
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	public List<ActorData> \u001D()
	{
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> list2 = this.\u001D();
		using (List<BoardSquare>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				ActorData occupantActor = boardSquare.OccupantActor;
				if (occupantActor != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D()).MethodHandle;
					}
					if (!list.Contains(occupantActor))
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
						list.Add(occupantActor);
					}
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return list;
	}

	public bool \u001D(ActorData \u001D)
	{
		if (\u001D == null)
		{
			return false;
		}
		List<BoardSquare> list = this.\u001D();
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.OccupantActor == \u001D)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D(ActorData)).MethodHandle;
					}
					return true;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public Vector3 \u001D()
	{
		Vector3 vector = Vector3.zero;
		if (this.m_quads.Length > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D()).MethodHandle;
			}
			foreach (BoardQuad boardQuad in this.m_quads)
			{
				if (boardQuad == null)
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
					Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.", new object[0]);
				}
				else
				{
					vector += (boardQuad.m_corner1.position + boardQuad.m_corner2.position) / 2f;
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			vector /= (float)this.m_quads.Length;
		}
		return vector;
	}

	public BoardSquare \u000E()
	{
		Vector3 u001D = this.\u001D();
		return Board.\u000E().\u000E(u001D);
	}

	public bool Contains(int x, int y)
	{
		List<BoardSquare> list = this.\u001D();
		foreach (BoardSquare boardSquare in list)
		{
			if (boardSquare.x == x)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.Contains(int, int)).MethodHandle;
				}
				if (boardSquare.y == y)
				{
					return true;
				}
			}
		}
		return false;
	}

	public float \u001D(BoardSquare \u001D)
	{
		if (\u001D == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D(BoardSquare)).MethodHandle;
			}
			return 0f;
		}
		List<BoardSquare> list = this.\u001D();
		if (list != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (list.Count != 0)
			{
				BoardSquare boardSquare = null;
				for (int i = 0; i < list.Count; i++)
				{
					BoardSquare boardSquare2 = list[i];
					if (boardSquare2 == null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					else if (\u001D == boardSquare2)
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
						boardSquare = \u001D;
						IL_E6:
						if (boardSquare == null)
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
							return 0f;
						}
						return boardSquare.HorizontalDistanceOnBoardTo(\u001D);
					}
					else if (boardSquare == null)
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
						boardSquare = boardSquare2;
					}
					else
					{
						float num = boardSquare.HorizontalDistanceOnBoardTo(\u001D);
						float num2 = boardSquare2.HorizontalDistanceOnBoardTo(\u001D);
						if (num2 < num)
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
							boardSquare = boardSquare2;
						}
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_E6;
				}
			}
		}
		return 0f;
	}

	public void GizmosDrawRegion(Color color)
	{
		Gizmos.color = color;
		List<BoardSquare> list = this.\u001D();
		foreach (BoardSquare boardSquare in list)
		{
			if (boardSquare.WorldBounds != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.GizmosDrawRegion(Color)).MethodHandle;
				}
				Vector3 center = boardSquare.WorldBounds.Value.center;
				center.y += 0.1f;
				Gizmos.DrawWireCube(center, boardSquare.WorldBounds.Value.extents * 2f);
				Gizmos.DrawWireCube(center, boardSquare.WorldBounds.Value.extents * 1.9f);
			}
		}
	}

	public bool HasNonZeroArea()
	{
		return this.m_quads.Length > 0;
	}

	public Sprite \u001D()
	{
		if (CaptureTheFlag.Get() != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardRegion.\u001D()).MethodHandle;
			}
			return CaptureTheFlag.Get().m_turnInRegionIcon;
		}
		return null;
	}

	public bool ShouldShowIndicator()
	{
		return CaptureTheFlag.Get() != null;
	}
}
