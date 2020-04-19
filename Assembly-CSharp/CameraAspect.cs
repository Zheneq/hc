using System;
using UnityEngine;

public class CameraAspect : MonoBehaviour
{
	[Tooltip("Width and Height are percentage of the screen that the camera box will take up. Both are percentage of current screen height.")]
	public float m_width = 0.17f;

	[Tooltip("Width and Height are percentage of the screen that the camera box will take up. Both are percentage of current screen height.")]
	public float m_height = 0.17f;

	private float Aspect
	{
		get
		{
			float result;
			if (this.m_height == 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraAspect.get_Aspect()).MethodHandle;
				}
				result = 1f;
			}
			else
			{
				result = this.m_width / this.m_height;
			}
			return result;
		}
	}

	private void Start()
	{
		this.SetAspect();
	}

	private void Update()
	{
		this.SetAspect();
	}

	private void SetAspect()
	{
		float num = (float)Screen.width * this.m_width;
		float width = num / (float)Screen.width;
		base.GetComponent<Camera>().rect = new Rect(base.GetComponent<Camera>().rect.x, base.GetComponent<Camera>().rect.y, width, this.m_height);
	}
}
