using System.Collections.Generic;

public class BuildNormalPathNodePool
{
	private List<BoardSquarePathInfo> m_allocatedInstances;

	private int m_nextEntryIndex;

	public BuildNormalPathNodePool()
	{
		m_allocatedInstances = new List<BoardSquarePathInfo>(450);
	}

	public BoardSquarePathInfo GetAllocatedNode()
	{
		BoardSquarePathInfo boardSquarePathInfo;
		if (m_nextEntryIndex < m_allocatedInstances.Count)
		{
			boardSquarePathInfo = m_allocatedInstances[m_nextEntryIndex];
			InitNodeValues(boardSquarePathInfo);
		}
		else
		{
			boardSquarePathInfo = new BoardSquarePathInfo();
			m_allocatedInstances.Add(boardSquarePathInfo);
		}
		m_nextEntryIndex++;
		return boardSquarePathInfo;
	}

	private void InitNodeValues(BoardSquarePathInfo node)
	{
		node.ResetValuesToDefault();
	}

	public void ResetAvailableNodeIndex()
	{
		m_nextEntryIndex = 0;
	}

	public int GetNextAvailableIndex()
	{
		return m_nextEntryIndex;
	}
}
