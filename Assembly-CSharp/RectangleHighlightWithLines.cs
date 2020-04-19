using System;
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
		this.m_highlightParent = new GameObject("LineRectangleHighlight");
		this.m_highlightParent.transform.localPosition = Vector3.zero;
		float widthInWorld = 0.5f;
		Color cyan = Color.cyan;
		this.m_left = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		this.m_right = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		this.m_left.transform.parent = this.m_highlightParent.transform;
		this.m_left.transform.localPosition = new Vector3(-num, 0f, -num);
		this.m_right.transform.parent = this.m_highlightParent.transform;
		this.m_right.transform.localPosition = new Vector3(num, 0f, -num);
		this.m_top = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		this.m_bottom = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, widthInWorld, false, cyan);
		this.m_top.transform.parent = this.m_highlightParent.transform;
		this.m_top.transform.localPosition = new Vector3(num, 0f, num);
		this.m_top.transform.Rotate(Vector3.up, -90f);
		this.m_bottom.transform.parent = this.m_highlightParent.transform;
		this.m_bottom.transform.localPosition = new Vector3(num, 0f, -num);
		this.m_bottom.transform.Rotate(Vector3.up, -90f);
	}

	public void AdjustSize(float heightInSquares, float widthInSquares)
	{
		if (this.m_currentHeightInSquares == heightInSquares)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RectangleHighlightWithLines.AdjustSize(float, float)).MethodHandle;
			}
			if (this.m_currentWidthInSquares == widthInSquares)
			{
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_currentHeightInSquares = heightInSquares;
		this.m_currentWidthInSquares = widthInSquares;
		float num = 0.5f * heightInSquares * Board.SquareSizeStatic;
		float num2 = 0.5f * widthInSquares * Board.SquareSizeStatic;
		if (this.m_left != null)
		{
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(this.m_left, heightInSquares);
			this.m_left.transform.localPosition = new Vector3(-num2, 0f, -num);
		}
		if (this.m_right != null)
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
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(this.m_right, heightInSquares);
			this.m_right.transform.localPosition = new Vector3(num2, 0f, -num);
		}
		if (this.m_top != null)
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
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(this.m_top, widthInSquares);
			this.m_top.transform.localPosition = new Vector3(num2, 0f, num);
		}
		if (this.m_bottom != null)
		{
			HighlightUtils.Get().AdjustDynamicLineSegmentLength(this.m_bottom, widthInSquares);
			this.m_bottom.transform.localPosition = new Vector3(num2, 0f, -num);
		}
	}
}
