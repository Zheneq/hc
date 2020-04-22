using UnityEngine;

public class RectangleHighlightWithLines
{
	public GameObject m_highlightParent;

	private float m_currentHeightInSquares = 1f;

	private float m_currentWidthInSquares = 1f;

	private GameObject m_left;

	private GameObject m_right;

	private GameObject m_top;

	private GameObject m_bottom;

	public RectangleHighlightWithLines()
	{
		float num = 0.5f * Board.SquareSizeStatic;
		m_highlightParent = new GameObject("LineRectangleHighlight");
		m_highlightParent.transform.localPosition = Vector3.zero;
		float widthInWorld = 0.5f;
		Color cyan = Color.cyan;
		m_left = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		m_right = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		m_left.transform.parent = m_highlightParent.transform;
		m_left.transform.localPosition = new Vector3(0f - num, 0f, 0f - num);
		m_right.transform.parent = m_highlightParent.transform;
		m_right.transform.localPosition = new Vector3(num, 0f, 0f - num);
		m_top = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		m_bottom = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		m_top.transform.parent = m_highlightParent.transform;
		m_top.transform.localPosition = new Vector3(num, 0f, num);
		m_top.transform.Rotate(Vector3.up, -90f);
		m_bottom.transform.parent = m_highlightParent.transform;
		m_bottom.transform.localPosition = new Vector3(num, 0f, 0f - num);
		m_bottom.transform.Rotate(Vector3.up, -90f);
	}

	public void AdjustSize(float heightInSquares, float widthInSquares)
	{
		if (m_currentHeightInSquares == heightInSquares)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_currentWidthInSquares == widthInSquares)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_currentHeightInSquares = heightInSquares;
		m_currentWidthInSquares = widthInSquares;
		float num = 0.5f * heightInSquares * Board.SquareSizeStatic;
		float num2 = 0.5f * widthInSquares * Board.SquareSizeStatic;
		if (m_left != null)
		{
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(m_left, heightInSquares);
			m_left.transform.localPosition = new Vector3(0f - num2, 0f, 0f - num);
		}
		if (m_right != null)
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
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(m_right, heightInSquares);
			m_right.transform.localPosition = new Vector3(num2, 0f, 0f - num);
		}
		if (m_top != null)
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
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(m_top, widthInSquares);
			m_top.transform.localPosition = new Vector3(num2, 0f, num);
		}
		if (m_bottom != null)
		{
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(m_bottom, widthInSquares);
			m_bottom.transform.localPosition = new Vector3(num2, 0f, 0f - num);
		}
	}
}
