using System;
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
		this.Initialize(initialCapacity);
	}

	private void Initialize(int initialCapacity)
	{
		int num = Mathf.Max(1, initialCapacity);
		this.m_buffer = new BoardSquarePathInfo[num];
	}

	public void Clear()
	{
		for (int i = 0; i < this.m_numElements; i++)
		{
			this.m_buffer[i] = null;
		}
		this.m_numElements = 0;
		this.m_squareToIndex.Clear();
	}

	public void SetTieBreakerDirAndPos(Vector3 tieBreakerDir, Vector3 tieBreakerPos)
	{
		this.m_tieBreakerDir = tieBreakerDir;
		this.m_tieBreakerTestPos = tieBreakerPos;
	}

	public bool IsEmpty()
	{
		return this.m_numElements == 0;
	}

	private int CompareFunc(BoardSquarePathInfo p1, BoardSquarePathInfo p2)
	{
		if (Mathf.Approximately(p1.F_cost, p2.F_cost))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.CompareFunc(BoardSquarePathInfo, BoardSquarePathInfo)).MethodHandle;
			}
			Vector3 from = p1.square.ToVector3() - this.m_tieBreakerTestPos;
			Vector3 from2 = p2.square.ToVector3() - this.m_tieBreakerTestPos;
			return Vector3.Angle(from, this.m_tieBreakerDir).CompareTo(Vector3.Angle(from2, this.m_tieBreakerDir));
		}
		return p1.F_cost.CompareTo(p2.F_cost);
	}

	private int Parent(int n)
	{
		if (n == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.Parent(int)).MethodHandle;
			}
			return -1;
		}
		return (n + 1) / 2 - 1;
	}

	private int LeftChild(int n)
	{
		return 2 * n + 1;
	}

	private void EnsureSize(int targetSize)
	{
		if (targetSize > this.m_buffer.Length)
		{
			if (Application.isEditor)
			{
				Debug.LogWarning(base.GetType() + " ---- doubling heap buffer size, from " + this.m_buffer.Length);
			}
			BoardSquarePathInfo[] array = new BoardSquarePathInfo[this.m_buffer.Length * 2];
			for (int i = 0; i < this.m_buffer.Length; i++)
			{
				array[i] = this.m_buffer[i];
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.EnsureSize(int)).MethodHandle;
			}
			this.m_buffer = array;
		}
	}

	public void Insert(BoardSquarePathInfo elem)
	{
		this.EnsureSize(this.m_numElements + 1);
		this.m_buffer[this.m_numElements] = elem;
		if (this.m_squareToIndex.ContainsKey(elem.square))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.Insert(BoardSquarePathInfo)).MethodHandle;
			}
			Debug.LogError("square added to heap multiple times?");
		}
		this.m_squareToIndex[elem.square] = this.m_numElements;
		this.BubbleUp(this.m_numElements);
		this.m_numElements++;
	}

	private void BubbleUp(int index)
	{
		int num = this.Parent(index);
		if (num >= 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.BubbleUp(int)).MethodHandle;
			}
			if (this.CompareFunc(this.m_buffer[index], this.m_buffer[num]) < 0)
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
				BoardSquarePathInfo boardSquarePathInfo = this.m_buffer[num];
				this.m_buffer[num] = this.m_buffer[index];
				this.m_buffer[index] = boardSquarePathInfo;
				this.m_squareToIndex[this.m_buffer[num].square] = num;
				this.m_squareToIndex[this.m_buffer[index].square] = index;
				this.BubbleUp(num);
			}
		}
	}

	public BoardSquarePathInfo ExtractTop()
	{
		if (this.m_numElements == 0)
		{
			Debug.LogError("Cannot extract on empty heap");
			return null;
		}
		BoardSquarePathInfo boardSquarePathInfo = this.m_buffer[0];
		this.m_buffer[0] = this.m_buffer[this.m_numElements - 1];
		this.m_buffer[this.m_numElements - 1] = null;
		this.m_squareToIndex.Remove(boardSquarePathInfo.square);
		if (this.m_numElements > 1)
		{
			this.m_squareToIndex[this.m_buffer[0].square] = 0;
		}
		this.m_numElements--;
		this.BubbleDown(0);
		return boardSquarePathInfo;
	}

	private void BubbleDown(int index)
	{
		int num = index;
		int num2 = this.LeftChild(index);
		for (int i = 0; i < 2; i++)
		{
			int num3 = num2 + i;
			if (num3 < this.m_numElements)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.BubbleDown(int)).MethodHandle;
				}
				if (this.CompareFunc(this.m_buffer[num3], this.m_buffer[num]) < 0)
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
					num = num3;
				}
			}
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
		if (num != index)
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
			BoardSquarePathInfo boardSquarePathInfo = this.m_buffer[index];
			this.m_buffer[index] = this.m_buffer[num];
			this.m_buffer[num] = boardSquarePathInfo;
			this.m_squareToIndex[this.m_buffer[index].square] = index;
			this.m_squareToIndex[this.m_buffer[num].square] = num;
			this.BubbleDown(num);
		}
	}

	public bool HasSquare(BoardSquare square)
	{
		return this.m_squareToIndex.ContainsKey(square);
	}

	public BoardSquarePathInfo TryGetNodeInHeapBySquare(BoardSquare square)
	{
		int num;
		if (this.m_squareToIndex.TryGetValue(square, out num))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.TryGetNodeInHeapBySquare(BoardSquare)).MethodHandle;
			}
			return this.m_buffer[num];
		}
		return null;
	}

	public void UpdatePriority(BoardSquarePathInfo adjSquarePathInfo)
	{
		int num;
		if (this.m_squareToIndex.TryGetValue(adjSquarePathInfo.square, out num))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BuildNormalPathHeap.UpdatePriority(BoardSquarePathInfo)).MethodHandle;
			}
			BoardSquarePathInfo boardSquarePathInfo = this.m_buffer[num];
			float f_cost = boardSquarePathInfo.F_cost;
			float f_cost2 = adjSquarePathInfo.F_cost;
			boardSquarePathInfo.heuristicCost = adjSquarePathInfo.heuristicCost;
			boardSquarePathInfo.moveCost = adjSquarePathInfo.moveCost;
			boardSquarePathInfo.prev = adjSquarePathInfo.prev;
			boardSquarePathInfo.m_expectedBackupNum = adjSquarePathInfo.m_expectedBackupNum;
			if (f_cost2 < f_cost)
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
				this.BubbleUp(num);
			}
			else
			{
				this.BubbleDown(num);
			}
		}
		else
		{
			Debug.LogError("Cannot update priority, does not exist in heap");
		}
	}
}
