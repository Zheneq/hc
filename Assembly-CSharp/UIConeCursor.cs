using System;
using UnityEngine;

public class UIConeCursor : MonoBehaviour
{
	public GameObject m_innerArc;

	public GameObject m_outerArc;

	public float m_arcDegrees;

	public float m_heightOffset;

	private float m_worldRadius;

	public void OnRadiusChanged(float newWorldRadius)
	{
		this.m_worldRadius = newWorldRadius;
		if (this.m_worldRadius <= 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIConeCursor.OnRadiusChanged(float)).MethodHandle;
			}
			Log.Error("ConeCursor with invalid radius (" + this.m_worldRadius + ").  Disabling...", new object[0]);
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
		else
		{
			if (this.m_outerArc != null)
			{
				this.m_outerArc.transform.localScale = new Vector3(this.m_worldRadius, 1f, this.m_worldRadius);
			}
			UIManager.SetGameObjectActive(base.gameObject, true, null);
		}
	}

	private void Start()
	{
		if (this.m_innerArc != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIConeCursor.Start()).MethodHandle;
			}
			this.m_innerArc.transform.localPosition = new Vector3(0f, this.m_heightOffset, 0f);
		}
		if (this.m_outerArc != null)
		{
			this.m_outerArc.transform.localPosition = new Vector3(0f, this.m_heightOffset, 0f);
		}
	}
}
