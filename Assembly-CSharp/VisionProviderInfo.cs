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
		if (m_actorIndex != ActorData.s_invalidActorIndex)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_actorIndex);
					if (actorData == null)
					{
						return null;
					}
					BoardSquare result = null;
					if (m_satelliteIndex == -1)
					{
						while (true)
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
							while (true)
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
						object obj;
						if (component == null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							obj = null;
						}
						else
						{
							obj = component.GetSatellite(m_satelliteIndex);
						}
						PersistentSatellite persistentSatellite = (PersistentSatellite)obj;
						if (persistentSatellite != null && persistentSatellite.IsVisible())
						{
							result = Board.Get().GetBoardSquare(persistentSatellite.transform.position);
						}
					}
					return result;
				}
				}
			}
		}
		return Board.Get().GetBoardSquare(m_boardX, m_boardY);
	}

	public bool IsEqual(VisionProviderInfo other)
	{
		return IsEqual(new GridPos(other.m_boardX, other.m_boardY, 0), other.m_radius, other.m_radiusAsStraightLineDist, other.m_brushRevealType, other.m_ignoreLos, other.m_flag, other.m_canFunctionInGlobalBlind);
	}

	public bool IsEqual(GridPos gridPos, float r, bool useSraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		int result;
		if (m_actorIndex == ActorData.s_invalidActorIndex)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_boardX == gridPos.x && m_boardY == gridPos.y)
			{
				result = (HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind) ? 1 : 0);
				goto IL_0053;
			}
		}
		result = 0;
		goto IL_0053;
		IL_0053:
		return (byte)result != 0;
	}

	public bool IsEqual(int actorIdx, float r, bool useSraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		int result;
		if (m_actorIndex == actorIdx)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_satelliteIndex == -1)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				result = (HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind) ? 1 : 0);
				goto IL_0044;
			}
		}
		result = 0;
		goto IL_0044;
		IL_0044:
		return (byte)result != 0;
	}

	public bool IsEqual(int actorIdx, int satelliteIdx, float r, bool useSraightLineDist, BrushRevealType brushRevealType, bool ignoreLos, BoardSquare.VisibilityFlags f, bool canFunctionInGlobalBlind)
	{
		int result;
		if (m_actorIndex == actorIdx)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_satelliteIndex == satelliteIdx)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				result = (HasSameProperties(r, useSraightLineDist, f, brushRevealType, ignoreLos, canFunctionInGlobalBlind) ? 1 : 0);
				goto IL_0045;
			}
		}
		result = 0;
		goto IL_0045;
		IL_0045:
		return (byte)result != 0;
	}

	private bool HasSameProperties(float r, bool useSraightLineDist, BoardSquare.VisibilityFlags f, BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind)
	{
		int result;
		if (m_flag == f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_brushRevealType == brushRevealType)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_ignoreLos == ignoreLos)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_canFunctionInGlobalBlind == canFunctionInGlobalBlind && m_radiusAsStraightLineDist == useSraightLineDist)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						result = ((Mathf.Abs(m_radius - r) < 0.05f) ? 1 : 0);
						goto IL_007d;
					}
				}
			}
		}
		result = 0;
		goto IL_007d;
		IL_007d:
		return (byte)result != 0;
	}
}
