using System;
using System.Collections.Generic;
using UnityEngine;

public class SquareIndicators
{
	private List<GameObject> m_indicators = new List<GameObject>();

	private int m_nextIndicatorIndex;

	private SquareIndicators.CreateIndicatorDelegate m_createIndicatorDelegate;

	private int m_initialAllocation;

	private int m_allocateIncrement;

	private float m_heightOffset;

	public SquareIndicators(SquareIndicators.CreateIndicatorDelegate createIndicatorDelegate, int initialAllocation, int allocateIncrement, float heightOffset)
	{
		this.m_createIndicatorDelegate = createIndicatorDelegate;
		this.m_initialAllocation = initialAllocation;
		this.m_allocateIncrement = allocateIncrement;
		this.m_heightOffset = heightOffset;
	}

	public void Initialize()
	{
		this.m_indicators.Clear();
		for (int i = 0; i < this.m_initialAllocation; i++)
		{
			GameObject gameObject = this.m_createIndicatorDelegate();
			if (gameObject != null)
			{
				this.m_indicators.Add(gameObject);
			}
		}
	}

	public void ResetNextIndicatorIndex()
	{
		this.m_nextIndicatorIndex = 0;
	}

	public int GetNextIndicatorIndex()
	{
		return this.m_nextIndicatorIndex;
	}

	public int ShowIndicatorForSquare(BoardSquare square)
	{
		if (!(square == null) && square.IsBaselineHeight())
		{
			if (HighlightUtils.Get() == null)
			{
			}
			else
			{
				if (this.m_indicators.Count > 0xC8)
				{
					return this.m_nextIndicatorIndex;
				}
				if (this.m_nextIndicatorIndex >= this.m_indicators.Count)
				{
					for (int i = 0; i < this.m_allocateIncrement; i++)
					{
						this.m_indicators.Add(this.m_createIndicatorDelegate());
					}
				}
				Vector3 position = square.ToVector3();
				position.y += this.m_heightOffset;
				this.m_indicators[this.m_nextIndicatorIndex].transform.position = position;
				this.m_indicators[this.m_nextIndicatorIndex].SetActive(true);
				this.m_nextIndicatorIndex++;
				return this.m_nextIndicatorIndex;
			}
		}
		return this.m_nextIndicatorIndex;
	}

	public void HideAllSquareIndicators(int fromIndex = 0)
	{
		for (int i = fromIndex; i < this.m_indicators.Count; i++)
		{
			if (this.m_indicators[i] != null)
			{
				if (!this.m_indicators[i].activeSelf)
				{
					IL_76:
					this.m_nextIndicatorIndex = fromIndex;
					return;
				}
				this.m_indicators[i].SetActive(false);
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			goto IL_76;
		}
	}

	public void ClearAllSquareIndicators()
	{
		this.HideAllSquareIndicators(0);
		this.m_indicators.Clear();
	}

	public delegate GameObject CreateIndicatorDelegate();
}
