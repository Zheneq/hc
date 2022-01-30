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
		foreach (BoardQuad boardQuad in m_quads)
		{
			if (boardQuad == null)
			{
				Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.");
			}
			else
			{
				foreach (BoardSquare square in boardQuad.GetSquares())
				{
					if (!m_squaresInRegion.Contains(square))
					{
						m_squaresInRegion.Add(square);
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
		m_squaresInRegion = Board.Get().GetSquaresBoundedBy(
			Board.Get().GetSquareFromPos(worldCorner1.x, worldCorner1.z),
			Board.Get().GetSquareFromPos(worldCorner2.x, worldCorner2.z));
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
		float minDist = 100000f;
		foreach (BoardSquare square in GetSquaresInRegion())
		{
			if (square.IsValidForGameplay())
			{
				float dist = square.HorizontalDistanceInSquaresTo(centerSquare);
				if (dist < minDist)
				{
					minDist = dist;
					result = square;
				}
			}
		}
		return result;
	}

	public List<ActorData> GetOccupantActors()
	{
		List<ActorData> result = new List<ActorData>();
		foreach (BoardSquare square in GetSquaresInRegion())
		{
			if (square.OccupantActor != null && !result.Contains(square.OccupantActor))
			{
				result.Add(square.OccupantActor);
			}
		}
		return result;
	}

	public bool IsActorInRegion(ActorData actor)
	{
		if (actor == null)
		{
			return false;
		}
		foreach (BoardSquare current in GetSquaresInRegion())
		{
			if (current.OccupantActor == actor)
			{
				return true;
			}
		}
		return false;
	}

	public Vector3 GetCenter()
	{
		Vector3 result = Vector3.zero;
		if (m_quads.Length > 0)
		{
			foreach (BoardQuad boardQuad in m_quads)
			{
				if (boardQuad == null)
				{
					Log.Error("Null BoardQuad in BoardRegion; fix region coordinator's data.");
				}
				else
				{
					result += (boardQuad.m_corner1.position + boardQuad.m_corner2.position) / 2f;
				}
			}
			result /= m_quads.Length;
		}
		return result;
	}

	public BoardSquare GetCenterSquare()
	{
		return Board.Get().GetSquareFromVec3(GetCenter());
	}

	public bool Contains(int x, int y)
	{
		foreach (BoardSquare square in GetSquaresInRegion())
		{
			if (square.x == x && square.y == y)
			{
				return true;
			}
		}
		return false;
	}

	public float GetShortestDistanceOnBoardTo(BoardSquare dest)
	{
		if (dest == null)
		{
			return 0f;
		}
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		if (squaresInRegion == null || squaresInRegion.Count == 0)
		{
			return 0f;
		}
		BoardSquare closestSquare = null;
		foreach (BoardSquare square in squaresInRegion)
		{
			if (square != null)
			{
				if (dest == square)
				{
					closestSquare = dest;
					break;
				}
				if (closestSquare == null)
				{
					closestSquare = square;
				}
				else
				{
					float minDist = closestSquare.HorizontalDistanceOnBoardTo(dest);
					float curDist = square.HorizontalDistanceOnBoardTo(dest);
					if (curDist < minDist)
					{
						closestSquare = square;
					}
				}
			}
		}
		if (closestSquare == null)
		{
			return 0f;
		}
		return closestSquare.HorizontalDistanceOnBoardTo(dest);
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
			return CaptureTheFlag.Get().m_turnInRegionIcon;
		}
		return null;
	}

	public bool ShouldShowIndicator()
	{
		return CaptureTheFlag.Get() != null;
	}
}
