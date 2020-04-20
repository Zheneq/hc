using System;
using UnityEngine;

[Serializable]
public struct VisionProviderInfo
{
	public int m_actorIndex;

	public int m_satelliteIndex;

	public int m_boardX;

	public int m_boardY;

	public float m_radius;

	public bool m_radiusAsStraightLineDist;

	public BoardSquare.VisibilityFlags m_flag;

	public VisionProviderInfo.BrushRevealType m_brushRevealType;

	public bool m_ignoreLos;

	public bool m_canFunctionInGlobalBlind;

	public VisionProviderInfo(GridPos gridPos, float r, bool radiusAsStraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags f = BoardSquare.VisibilityFlags.Team)
	{
		this.m_actorIndex = ActorData.s_invalidActorIndex;
		this.m_satelliteIndex = -1;
		this.m_boardX = gridPos.x;
		this.m_boardY = gridPos.y;
		this.m_radius = r;
		this.m_radiusAsStraightLineDist = radiusAsStraightLineDist;
		this.m_flag = f;
		this.m_brushRevealType = brushRevealType;
		this.m_ignoreLos = ignoreLos;
		this.m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
	}

	public VisionProviderInfo(int actorIdx, float r, bool radiusAsStraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags f = BoardSquare.VisibilityFlags.Team)
	{
		this.m_actorIndex = actorIdx;
		this.m_satelliteIndex = -1;
		this.m_boardX = -1;
		this.m_boardY = -1;
		this.m_radius = r;
		this.m_radiusAsStraightLineDist = radiusAsStraightLineDist;
		this.m_flag = f;
		this.m_brushRevealType = brushRevealType;
		this.m_ignoreLos = ignoreLos;
		this.m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
	}

	public VisionProviderInfo(int actorIdx, int satelliteIdx, float r, bool radiusAsStraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags f = BoardSquare.VisibilityFlags.Team)
	{
		this.m_actorIndex = actorIdx;
		this.m_satelliteIndex = satelliteIdx;
		this.m_boardX = -1;
		this.m_boardY = -1;
		this.m_radius = r;
		this.m_radiusAsStraightLineDist = radiusAsStraightLineDist;
		this.m_flag = f;
		this.m_brushRevealType = brushRevealType;
		this.m_ignoreLos = ignoreLos;
		this.m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
	}

	public BoardSquare GetBoardSquare()
	{
		if (this.m_actorIndex == ActorData.s_invalidActorIndex)
		{
			return Board.Get().GetBoardSquare(this.m_boardX, this.m_boardY);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(VisionProviderInfo.GetBoardSquare()).MethodHandle;
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(this.m_actorIndex);
		if (actorData == null)
		{
			return null;
		}
		BoardSquare result = null;
		if (this.m_satelliteIndex == -1)
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
			if (!actorData.IsDead())
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
				result = actorData.GetCurrentBoardSquare();
			}
		}
		else
		{
			SatelliteController component = actorData.GetComponent<SatelliteController>();
			PersistentSatellite persistentSatellite;
			if (component == null)
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
				persistentSatellite = null;
			}
			else
			{
				persistentSatellite = component.GetSatellite(this.m_satelliteIndex);
			}
			PersistentSatellite persistentSatellite2 = persistentSatellite;
			if (persistentSatellite2 != null && persistentSatellite2.IsVisible())
			{
				result = Board.Get().GetBoardSquare(persistentSatellite2.transform.position);
			}
		}
		return result;
	}

	public bool IsEqual(VisionProviderInfo other)
	{
		return this.IsEqual(new GridPos(other.m_boardX, other.m_boardY, 0), other.m_radius, other.m_radiusAsStraightLineDist, other.m_brushRevealType, other.m_ignoreLos, other.m_flag, other.m_canFunctionInGlobalBlind);
	}

	public bool IsEqual(GridPos gridPos, float r, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		if (this.m_actorIndex == ActorData.s_invalidActorIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionProviderInfo.IsEqual(GridPos, float, bool, VisionProviderInfo.BrushRevealType, bool, BoardSquare.VisibilityFlags, bool)).MethodHandle;
			}
			if (this.m_boardX == gridPos.x && this.m_boardY == gridPos.y)
			{
				return this.HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind);
			}
		}
		return false;
	}

	public bool IsEqual(int actorIdx, float r, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		if (this.m_actorIndex == actorIdx)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionProviderInfo.IsEqual(int, float, bool, VisionProviderInfo.BrushRevealType, bool, BoardSquare.VisibilityFlags, bool)).MethodHandle;
			}
			if (this.m_satelliteIndex == -1)
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
				return this.HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind);
			}
		}
		return false;
	}

	public bool IsEqual(int actorIdx, int satelliteIdx, float r, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		if (this.m_actorIndex == actorIdx)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionProviderInfo.IsEqual(int, int, float, bool, VisionProviderInfo.BrushRevealType, bool, BoardSquare.VisibilityFlags, bool)).MethodHandle;
			}
			if (this.m_satelliteIndex == satelliteIdx)
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
				return this.HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind);
			}
		}
		return false;
	}

	private bool HasSameProperties(float r, bool useSraightLineDist, BoardSquare.VisibilityFlags f, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind)
	{
		if (this.m_flag == f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionProviderInfo.HasSameProperties(float, bool, BoardSquare.VisibilityFlags, VisionProviderInfo.BrushRevealType, bool, bool)).MethodHandle;
			}
			if (this.m_brushRevealType == brushRevealType)
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
				if (this.m_ignoreLos == ignoreLos)
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
					if (this.m_canFunctionInGlobalBlind == canFunctionInGlobalBlind && this.m_radiusAsStraightLineDist == useSraightLineDist)
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
						return Mathf.Abs(this.m_radius - r) < 0.05f;
					}
				}
			}
		}
		return false;
	}

	public enum BrushRevealType
	{
		Never,
		BaseOnCenterPosition,
		Always
	}
}
