using System;
using UnityEngine;

[Serializable]
public struct VisionProviderInfo
{
	public enum BrushRevealType
	{
		Never,
		BaseOnCenterPosition,
		Always
	}

	public int m_actorIndex;
	public int m_satelliteIndex;
	public int m_boardX;
	public int m_boardY;
	public float m_radius;
	public bool m_radiusAsStraightLineDist;
	public BoardSquare.VisibilityFlags m_flag;
	public BrushRevealType m_brushRevealType;
	public bool m_ignoreLos;
	public bool m_canFunctionInGlobalBlind;

	public VisionProviderInfo(GridPos gridPos, float r, bool radiusAsStraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags f = BoardSquare.VisibilityFlags.Team)
	{
		m_actorIndex = ActorData.s_invalidActorIndex;
		m_satelliteIndex = -1;
		m_boardX = gridPos.x;
		m_boardY = gridPos.y;
		m_radius = r;
		m_radiusAsStraightLineDist = radiusAsStraightLineDist;
		m_flag = f;
		m_brushRevealType = brushRevealType;
		m_ignoreLos = ignoreLos;
		m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
	}

	public VisionProviderInfo(int actorIdx, float r, bool radiusAsStraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags f = BoardSquare.VisibilityFlags.Team)
	{
		m_actorIndex = actorIdx;
		m_satelliteIndex = -1;
		m_boardX = -1;
		m_boardY = -1;
		m_radius = r;
		m_radiusAsStraightLineDist = radiusAsStraightLineDist;
		m_flag = f;
		m_brushRevealType = brushRevealType;
		m_ignoreLos = ignoreLos;
		m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
	}

	public VisionProviderInfo(int actorIdx, int satelliteIdx, float r, bool radiusAsStraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags f = BoardSquare.VisibilityFlags.Team)
	{
		m_actorIndex = actorIdx;
		m_satelliteIndex = satelliteIdx;
		m_boardX = -1;
		m_boardY = -1;
		m_radius = r;
		m_radiusAsStraightLineDist = radiusAsStraightLineDist;
		m_flag = f;
		m_brushRevealType = brushRevealType;
		m_ignoreLos = ignoreLos;
		m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
	}

	public BoardSquare GetBoardSquare()
	{
		if (m_actorIndex == ActorData.s_invalidActorIndex)
		{
			return Board.Get().GetSquareFromIndex(m_boardX, m_boardY);
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_actorIndex);
		if (actorData == null)
		{
			return null;
		}
		BoardSquare result = null;
		if (m_satelliteIndex == -1)
		{
			if (!actorData.IsDead())
			{
				result = actorData.GetCurrentBoardSquare();
			}
		}
		else
		{
			PersistentSatellite persistentSatellite = actorData.GetComponent<SatelliteController>()?.GetSatellite(m_satelliteIndex);
			if (persistentSatellite != null && persistentSatellite.IsVisible())
			{
				result = Board.Get().GetSquareFromVec3(persistentSatellite.transform.position);
			}
		}
		return result;
	}

	public bool IsEqual(VisionProviderInfo other)
	{
		return IsEqual(new GridPos(other.m_boardX, other.m_boardY, 0), other.m_radius, other.m_radiusAsStraightLineDist, other.m_brushRevealType, other.m_ignoreLos, other.m_flag, other.m_canFunctionInGlobalBlind);
	}

	public bool IsEqual(GridPos gridPos, float r, bool useSraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		return m_actorIndex == ActorData.s_invalidActorIndex
			&& m_boardX == gridPos.x
			&& m_boardY == gridPos.y
			&& HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind);
	}

	public bool IsEqual(int actorIdx, float r, bool useSraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		return m_actorIndex == actorIdx
			&& m_satelliteIndex == -1
			&& HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind);
	}

	public bool IsEqual(int actorIdx, int satelliteIdx, float r, bool useSraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		return m_actorIndex == actorIdx
			&& m_satelliteIndex == satelliteIdx
			&& HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind);
	}

	private bool HasSameProperties(float r, bool useSraightLineDist, BoardSquare.VisibilityFlags f, BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind)
	{
		return m_flag == f
			&& m_brushRevealType == brushRevealType
			&& m_ignoreLos == ignoreLos
			&& m_canFunctionInGlobalBlind == canFunctionInGlobalBlind
			&& m_radiusAsStraightLineDist == useSraightLineDist
			&& Mathf.Abs(m_radius - r) < 0.05f;
	}
}
