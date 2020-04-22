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
		m_squaresInRegion = new List<BoardSquare>();
		if (m_quads == null)
		{
			return;
		}
		BoardQuad[] quads = m_quads;
		foreach (BoardQuad boardQuad in quads)
		{
			if (boardQuad == null)
			{
				Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.");
			}
			else
			{
				List<BoardSquare> squares = boardQuad.GetSquares();
				using (List<BoardSquare>.Enumerator enumerator = squares.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare current = enumerator.Current;
						if (!m_squaresInRegion.Contains(current))
						{
							m_squaresInRegion.Add(current);
						}
					}
				}
			}
		}
	}

	public virtual void Initialize()
	{
		CacheSquaresInRegion();
	}

	public virtual void InitializeAsRect(Vector3 worldCorner1, Vector3 worldCorner2)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(worldCorner1.x, worldCorner1.z);
		BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(worldCorner2.x, worldCorner2.z);
		m_squaresInRegion = Board.Get().GetSquaresInRect(boardSquareSafe, boardSquareSafe2);
	}

	public List<BoardSquare> GetSquaresInRegion()
	{
		if (m_squaresInRegion == null)
		{
			Log.Error("Did not call CacheSquaresInRegion before calling GetSquaresInRegion.  This will cause slowdowns");
			CacheSquaresInRegion();
		}
		return m_squaresInRegion;
	}

	public BoardSquare GetClosestToCenter()
	{
		BoardSquare result = null;
		BoardSquare centerSquare = GetCenterSquare();
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		float num = 100000f;
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.IsBaselineHeight())
				{
					float num2 = current.HorizontalDistanceInSquaresTo(centerSquare);
					if (num2 < num)
					{
						num = num2;
						result = current;
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public List<ActorData> GetOccupantActors()
	{
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				ActorData occupantActor = current.OccupantActor;
				if (occupantActor != null)
				{
					if (!list.Contains(occupantActor))
					{
						list.Add(occupantActor);
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public bool IsActorInRegion(ActorData actor)
	{
		if (actor == null)
		{
			return false;
		}
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.OccupantActor == actor)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public Vector3 GetCenter()
	{
		Vector3 zero = Vector3.zero;
		if (m_quads.Length > 0)
		{
			BoardQuad[] quads = m_quads;
			foreach (BoardQuad boardQuad in quads)
			{
				if (boardQuad == null)
				{
					Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.");
				}
				else
				{
					zero += (boardQuad.m_corner1.position + boardQuad.m_corner2.position) / 2f;
				}
			}
			zero /= (float)m_quads.Length;
		}
		return zero;
	}

	public BoardSquare GetCenterSquare()
	{
		Vector3 center = GetCenter();
		return Board.Get().GetBoardSquare(center);
	}

	public bool Contains(int x, int y)
	{
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		foreach (BoardSquare item in squaresInRegion)
		{
			if (item.x == x)
			{
				if (item.y == y)
				{
					return true;
				}
			}
		}
		return false;
	}

	public float _001D(BoardSquare _001D)
	{
		if (_001D == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 0f;
				}
			}
		}
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		if (squaresInRegion != null)
		{
			if (squaresInRegion.Count != 0)
			{
				BoardSquare boardSquare = null;
				int num = 0;
				while (true)
				{
					if (num < squaresInRegion.Count)
					{
						BoardSquare boardSquare2 = squaresInRegion[num];
						if (boardSquare2 == null)
						{
						}
						else
						{
							if (_001D == boardSquare2)
							{
								boardSquare = _001D;
								break;
							}
							if (boardSquare == null)
							{
								boardSquare = boardSquare2;
							}
							else
							{
								float num2 = boardSquare.HorizontalDistanceOnBoardTo(_001D);
								float num3 = boardSquare2.HorizontalDistanceOnBoardTo(_001D);
								if (num3 < num2)
								{
									boardSquare = boardSquare2;
								}
							}
						}
						num++;
						continue;
					}
					break;
				}
				if (boardSquare == null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return 0f;
						}
					}
				}
				return boardSquare.HorizontalDistanceOnBoardTo(_001D);
			}
		}
		return 0f;
	}

	public void GizmosDrawRegion(Color color)
	{
		Gizmos.color = color;
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		foreach (BoardSquare item in squaresInRegion)
		{
			if (item.WorldBounds.HasValue)
			{
				Vector3 center = item.WorldBounds.Value.center;
				center.y += 0.1f;
				Gizmos.DrawWireCube(center, item.WorldBounds.Value.extents * 2f);
				Gizmos.DrawWireCube(center, item.WorldBounds.Value.extents * 1.9f);
			}
		}
	}

	public bool HasNonZeroArea()
	{
		return m_quads.Length > 0;
	}

	public Sprite GetTurnInRegionIcon()
	{
		if (CaptureTheFlag.Get() != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return CaptureTheFlag.Get().m_turnInRegionIcon;
				}
			}
		}
		return null;
	}

	public bool ShouldShowIndicator()
	{
		return CaptureTheFlag.Get() != null;
	}
}
