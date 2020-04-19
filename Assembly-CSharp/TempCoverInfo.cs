using System;

public struct TempCoverInfo
{
	public ActorCover.CoverDirections m_coverDir;

	public bool m_ignoreMinDist;

	public TempCoverInfo(ActorCover.CoverDirections coverDir, bool ignoreMinDist)
	{
		this.m_coverDir = coverDir;
		this.m_ignoreMinDist = ignoreMinDist;
	}
}
