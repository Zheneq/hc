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
		if (m_nextEntryIndex < m_allocatedInstances.Count)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					BoardSquarePathInfo boardSquarePathInfo = m_allocatedInstances[m_nextEntryIndex];
					InitNodeValues(boardSquarePathInfo);
					m_nextEntryIndex++;
					return boardSquarePathInfo;
				}
				}
			}
		}
		BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
		m_allocatedInstances.Add(boardSquarePathInfo2);
		m_nextEntryIndex++;
		return boardSquarePathInfo2;
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
