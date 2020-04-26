using System.Collections.Generic;
using UnityEngine;

public class SquareIndicators
{
	public delegate GameObject CreateIndicatorDelegate();

	private List<GameObject> m_indicators = new List<GameObject>();

	private int m_nextIndicatorIndex;

	private CreateIndicatorDelegate m_createIndicatorDelegate;

	private int m_initialAllocation;

	private int m_allocateIncrement;

	private float m_heightOffset;

	public SquareIndicators(CreateIndicatorDelegate createIndicatorDelegate, int initialAllocation, int allocateIncrement, float heightOffset)
	{
		m_createIndicatorDelegate = createIndicatorDelegate;
		m_initialAllocation = initialAllocation;
		m_allocateIncrement = allocateIncrement;
		m_heightOffset = heightOffset;
	}

	public void Initialize()
	{
		m_indicators.Clear();
		for (int i = 0; i < m_initialAllocation; i++)
		{
			GameObject gameObject = m_createIndicatorDelegate();
			if (gameObject != null)
			{
				m_indicators.Add(gameObject);
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void ResetNextIndicatorIndex()
	{
		m_nextIndicatorIndex = 0;
	}

	public int GetNextIndicatorIndex()
	{
		return m_nextIndicatorIndex;
	}

	public int ShowIndicatorForSquare(BoardSquare square)
	{
		if (!(square == null) && square.IsBaselineHeight())
		{
			if (!(HighlightUtils.Get() == null))
			{
				if (m_indicators.Count > 200)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return m_nextIndicatorIndex;
						}
					}
				}
				if (m_nextIndicatorIndex >= m_indicators.Count)
				{
					for (int i = 0; i < m_allocateIncrement; i++)
					{
						m_indicators.Add(m_createIndicatorDelegate());
					}
				}
				Vector3 position = square.ToVector3();
				position.y += m_heightOffset;
				m_indicators[m_nextIndicatorIndex].transform.position = position;
				m_indicators[m_nextIndicatorIndex].SetActive(true);
				m_nextIndicatorIndex++;
				return m_nextIndicatorIndex;
			}
		}
		return m_nextIndicatorIndex;
	}

	public void HideAllSquareIndicators(int fromIndex = 0)
	{
		int num = fromIndex;
		while (true)
		{
			if (num < m_indicators.Count)
			{
				if (m_indicators[num] != null)
				{
					if (!m_indicators[num].activeSelf)
					{
						break;
					}
					m_indicators[num].SetActive(false);
				}
				num++;
				continue;
			}
			break;
		}
		m_nextIndicatorIndex = fromIndex;
	}

	public void ClearAllSquareIndicators()
	{
		HideAllSquareIndicators();
		m_indicators.Clear();
	}
}
