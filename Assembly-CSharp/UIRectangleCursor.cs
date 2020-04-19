using System;
using UnityEngine;

public class UIRectangleCursor : MonoBehaviour
{
	public GameObject m_start;

	public GameObject m_centerLine;

	public GameObject m_lengthLine1;

	public GameObject m_lengthLine2;

	public GameObject m_corner1;

	public GameObject m_corner2;

	public GameObject m_endWidthLine;

	public GameObject m_interior;

	public float m_distCasterToStart = 0.75f;

	public float m_distStartToCenterLine;

	public float m_distCasterToInterior = 0.75f;

	public float m_widthPerCorner = 0.1f;

	public float m_lengthPerCorner = 1.5f;

	public float m_heightOffset;

	private float m_worldWidth;

	private float m_worldLength;

	public void OnDimensionsChanged(float newWorldWidth, float newWorldLength)
	{
		this.m_worldWidth = newWorldWidth;
		this.m_worldLength = newWorldLength;
		if (this.m_worldWidth > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRectangleCursor.OnDimensionsChanged(float, float)).MethodHandle;
			}
			if (this.m_worldLength > 0f)
			{
				if (this.m_start != null)
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
					this.m_start.transform.localPosition = new Vector3(0f, this.m_heightOffset, this.m_distCasterToStart);
				}
				if (this.m_centerLine != null)
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
					float num = this.m_distCasterToStart + this.m_distStartToCenterLine;
					this.m_centerLine.transform.localPosition = new Vector3(0f, this.m_heightOffset, num);
					float z = this.m_worldLength - num;
					this.m_centerLine.transform.localScale = new Vector3(1f, 1f, z);
				}
				float num2 = this.m_worldWidth / 2f;
				if (this.m_lengthLine1 != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_lengthLine2 != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						float z2 = this.m_worldLength - this.m_lengthPerCorner - this.m_distCasterToStart;
						float z3 = this.m_worldLength - this.m_lengthPerCorner;
						this.m_lengthLine1.transform.localScale = new Vector3(1f, 1f, z2);
						this.m_lengthLine2.transform.localScale = new Vector3(1f, 1f, z2);
						this.m_lengthLine1.transform.localPosition = new Vector3(-num2, this.m_heightOffset, z3);
						this.m_lengthLine2.transform.localPosition = new Vector3(num2, this.m_heightOffset, z3);
					}
				}
				if (this.m_corner1 != null && this.m_corner2 != null)
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
					this.m_corner1.transform.localPosition = new Vector3(-num2, this.m_heightOffset, this.m_worldLength);
					this.m_corner2.transform.localPosition = new Vector3(num2, this.m_heightOffset, this.m_worldLength);
				}
				if (this.m_endWidthLine != null)
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
					float x = this.m_worldWidth - this.m_widthPerCorner * 2f;
					this.m_endWidthLine.transform.localScale = new Vector3(x, 1f, 1f);
					this.m_endWidthLine.transform.localPosition = new Vector3(0f, this.m_heightOffset, this.m_worldLength);
				}
				if (this.m_interior != null)
				{
					float worldWidth = this.m_worldWidth;
					float z4 = this.m_worldLength - this.m_lengthPerCorner - this.m_distCasterToInterior;
					this.m_interior.transform.localScale = new Vector3(worldWidth, 1f, z4);
					this.m_interior.transform.localPosition = new Vector3(0f, this.m_heightOffset, this.m_distCasterToInterior);
				}
				base.gameObject.SetActive(true);
				return;
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
		}
		base.gameObject.SetActive(false);
	}

	public void SetRectangleEndVisible(bool visible)
	{
		this.m_corner1.SetActive(visible);
		this.m_corner2.SetActive(visible);
		this.m_endWidthLine.SetActive(visible);
	}

	public void SetRectangleStartVisible(bool visible)
	{
		this.m_start.SetActive(visible);
	}

	private void Start()
	{
	}
}
