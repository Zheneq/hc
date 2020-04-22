using System.Collections.Generic;
using UnityEngine;

public class BuildNormalPathHeap
{
	private BoardSquarePathInfo[] m_buffer;

	private int m_numElements;

	private Vector3 m_tieBreakerDir = Vector3.forward;

	private Vector3 m_tieBreakerTestPos = Vector3.one;

	private Dictionary<BoardSquare, int> m_squareToIndex = new Dictionary<BoardSquare, int>();

	public BuildNormalPathHeap(int initialCapacity)
	{
		Initialize(initialCapacity);
	}

	private void Initialize(int initialCapacity)
	{
		int num = Mathf.Max(1, initialCapacity);
		m_buffer = new BoardSquarePathInfo[num];
	}

	public void Clear()
	{
		for (int i = 0; i < m_numElements; i++)
		{
			m_buffer[i] = null;
		}
		m_numElements = 0;
		m_squareToIndex.Clear();
	}

	public void SetTieBreakerDirAndPos(Vector3 tieBreakerDir, Vector3 tieBreakerPos)
	{
		m_tieBreakerDir = tieBreakerDir;
		m_tieBreakerTestPos = tieBreakerPos;
	}

	public bool IsEmpty()
	{
		return m_numElements == 0;
	}

	private int CompareFunc(BoardSquarePathInfo p1, BoardSquarePathInfo p2)
	{
		if (Mathf.Approximately(p1.F_cost, p2.F_cost))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					Vector3 from = p1.square.ToVector3() - m_tieBreakerTestPos;
					Vector3 from2 = p2.square.ToVector3() - m_tieBreakerTestPos;
					return Vector3.Angle(from, m_tieBreakerDir).CompareTo(Vector3.Angle(from2, m_tieBreakerDir));
				}
				}
			}
		}
		return p1.F_cost.CompareTo(p2.F_cost);
	}

	private int Parent(int n)
	{
		if (n == 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
		}
		return (n + 1) / 2 - 1;
	}

	private int LeftChild(int n)
	{
		return 2 * n + 1;
	}

	private void EnsureSize(int targetSize)
	{
		if (targetSize <= m_buffer.Length)
		{
			return;
		}
		if (Application.isEditor)
		{
			Debug.LogWarning(string.Concat(GetType(), " ---- doubling heap buffer size, from ", m_buffer.Length));
		}
		BoardSquarePathInfo[] array = new BoardSquarePathInfo[m_buffer.Length * 2];
		for (int i = 0; i < m_buffer.Length; i++)
		{
			array[i] = m_buffer[i];
		}
		while (true)
		{
			m_buffer = array;
			return;
		}
	}

	public void Insert(BoardSquarePathInfo elem)
	{
		EnsureSize(m_numElements + 1);
		m_buffer[m_numElements] = elem;
		if (m_squareToIndex.ContainsKey(elem.square))
		{
			Debug.LogError("square added to heap multiple times?");
		}
		m_squareToIndex[elem.square] = m_numElements;
		BubbleUp(m_numElements);
		m_numElements++;
	}

	private void BubbleUp(int index)
	{
		int num = Parent(index);
		if (num < 0)
		{
			return;
		}
		while (true)
		{
			if (CompareFunc(m_buffer[index], m_buffer[num]) < 0)
			{
				while (true)
				{
					BoardSquarePathInfo boardSquarePathInfo = m_buffer[num];
					m_buffer[num] = m_buffer[index];
					m_buffer[index] = boardSquarePathInfo;
					m_squareToIndex[m_buffer[num].square] = num;
					m_squareToIndex[m_buffer[index].square] = index;
					BubbleUp(num);
					return;
				}
			}
			return;
		}
	}

	public BoardSquarePathInfo ExtractTop()
	{
		if (m_numElements == 0)
		{
			Debug.LogError("Cannot extract on empty heap");
			return null;
		}
		BoardSquarePathInfo boardSquarePathInfo = m_buffer[0];
		m_buffer[0] = m_buffer[m_numElements - 1];
		m_buffer[m_numElements - 1] = null;
		m_squareToIndex.Remove(boardSquarePathInfo.square);
		if (m_numElements > 1)
		{
			m_squareToIndex[m_buffer[0].square] = 0;
		}
		m_numElements--;
		BubbleDown(0);
		return boardSquarePathInfo;
	}

	private void BubbleDown(int index)
	{
		int num = index;
		int num2 = LeftChild(index);
		for (int i = 0; i < 2; i++)
		{
			int num3 = num2 + i;
			if (num3 >= m_numElements)
			{
				continue;
			}
			if (CompareFunc(m_buffer[num3], m_buffer[num]) < 0)
			{
				num = num3;
			}
		}
		while (true)
		{
			if (num != index)
			{
				while (true)
				{
					BoardSquarePathInfo boardSquarePathInfo = m_buffer[index];
					m_buffer[index] = m_buffer[num];
					m_buffer[num] = boardSquarePathInfo;
					m_squareToIndex[m_buffer[index].square] = index;
					m_squareToIndex[m_buffer[num].square] = num;
					BubbleDown(num);
					return;
				}
			}
			return;
		}
	}

	public bool HasSquare(BoardSquare square)
	{
		return m_squareToIndex.ContainsKey(square);
	}

	public BoardSquarePathInfo TryGetNodeInHeapBySquare(BoardSquare square)
	{
		if (m_squareToIndex.TryGetValue(square, out int value))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_buffer[value];
				}
			}
		}
		return null;
	}

	public void UpdatePriority(BoardSquarePathInfo adjSquarePathInfo)
	{
		if (m_squareToIndex.TryGetValue(adjSquarePathInfo.square, out int value))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					BoardSquarePathInfo boardSquarePathInfo = m_buffer[value];
					float f_cost = boardSquarePathInfo.F_cost;
					float f_cost2 = adjSquarePathInfo.F_cost;
					boardSquarePathInfo.heuristicCost = adjSquarePathInfo.heuristicCost;
					boardSquarePathInfo.moveCost = adjSquarePathInfo.moveCost;
					boardSquarePathInfo.prev = adjSquarePathInfo.prev;
					boardSquarePathInfo.m_expectedBackupNum = adjSquarePathInfo.m_expectedBackupNum;
					if (f_cost2 < f_cost)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								BubbleUp(value);
								return;
							}
						}
					}
					BubbleDown(value);
					return;
				}
				}
			}
		}
		Debug.LogError("Cannot update priority, does not exist in heap");
	}
}
