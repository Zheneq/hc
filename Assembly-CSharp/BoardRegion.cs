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
								this.m_squaresInRegion.Add(item);
							}
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
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(worldCorner1.x, worldCorner1.z);
		BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(worldCorner2.x, worldCorner2.z);
		this.m_squaresInRegion = Board.Get().GetSquaresInRect(boardSquareSafe, boardSquareSafe2);
	}

	public List<BoardSquare> GetSquaresInRegion()
	{
		if (this.m_squaresInRegion == null)
		{
			Log.Error("Did not call CacheSquaresInRegion before calling GetSquaresInRegion.  This will cause slowdowns", new object[0]);
			this.CacheSquaresInRegion();
		}
		return this.m_squaresInRegion;
	}

	public BoardSquare GetClosestToCenter()
	{
		BoardSquare result = null;
		BoardSquare centerSquare = this.GetCenterSquare();
		List<BoardSquare> squaresInRegion = this.GetSquaresInRegion();
		float num = 100000f;
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.IsBaselineHeight())
				{
					float num2 = boardSquare.HorizontalDistanceInSquaresTo(centerSquare);
					if (num2 < num)
					{
						num = num2;
						result = boardSquare;
					}
				}
			}
		}
		return result;
	}

	public List<ActorData> GetOccupantActors()
	{
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> squaresInRegion = this.GetSquaresInRegion();
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				ActorData occupantActor = boardSquare.OccupantActor;
				if (occupantActor != null)
				{
					if (!list.Contains(occupantActor))
					{
						list.Add(occupantActor);
					}
				}
			}
		}
		return list;
	}

	public bool IsActorInRegion(ActorData actor)
	{
		if (actor == null)
		{
			return false;
		}
		List<BoardSquare> squaresInRegion = this.GetSquaresInRegion();
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.OccupantActor == actor)
				{
					return true;
				}
			}
		}
		return false;
	}

	public Vector3 GetCenter()
	{
		Vector3 vector = Vector3.zero;
		if (this.m_quads.Length > 0)
		{
			foreach (BoardQuad boardQuad in this.m_quads)
			{
				if (boardQuad == null)
				{
					Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.", new object[0]);
				}
				else
				{
					vector += (boardQuad.m_corner1.position + boardQuad.m_corner2.position) / 2f;
				}
			}
			vector /= (float)this.m_quads.Length;
		}
		return vector;
	}

	public BoardSquare GetCenterSquare()
	{
		Vector3 center = this.GetCenter();
		return Board.Get().GetBoardSquare(center);
	}

	public bool Contains(int x, int y)
	{
		List<BoardSquare> squaresInRegion = this.GetSquaresInRegion();
		foreach (BoardSquare boardSquare in squaresInRegion)
		{
			if (boardSquare.x == x)
			{
				if (boardSquare.y == y)
				{
					return true;
				}
			}
		}
		return false;
	}

	public float symbol_001D(BoardSquare symbol_001D)
	{
		if (symbol_001D == null)
		{
			return 0f;
		}
		List<BoardSquare> squaresInRegion = this.GetSquaresInRegion();
		if (squaresInRegion != null)
		{
			if (squaresInRegion.Count != 0)
			{
				BoardSquare boardSquare = null;
				for (int i = 0; i < squaresInRegion.Count; i++)
				{
					BoardSquare boardSquare2 = squaresInRegion[i];
					if (boardSquare2 == null)
					{
					}
					else if (symbol_001D == boardSquare2)
					{
						boardSquare = symbol_001D;
						IL_E6:
						if (boardSquare == null)
						{
							return 0f;
						}
						return boardSquare.HorizontalDistanceOnBoardTo(symbol_001D);
					}
					else if (boardSquare == null)
					{
						boardSquare = boardSquare2;
					}
					else
					{
						float num = boardSquare.HorizontalDistanceOnBoardTo(symbol_001D);
						float num2 = boardSquare2.HorizontalDistanceOnBoardTo(symbol_001D);
						if (num2 < num)
						{
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
		List<BoardSquare> squaresInRegion = this.GetSquaresInRegion();
		foreach (BoardSquare boardSquare in squaresInRegion)
		{
			if (boardSquare.WorldBounds != null)
			{
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

	public Sprite GetTurnInRegionIcon()
	{
		if (CaptureTheFlag.Get() != null)
		{
			return CaptureTheFlag.Get().m_turnInRegionIcon;
		}
		return null;
	}

	public bool ShouldShowIndicator()
	{
		return CaptureTheFlag.Get() != null;
	}
}
