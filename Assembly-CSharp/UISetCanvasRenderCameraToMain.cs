using System;
using UnityEngine;

public class UISetCanvasRenderCameraToMain : MonoBehaviour
{
	private void Update()
	{
		if (Camera.main != null)
		{
			Canvas component = base.GetComponent<Canvas>();
			Camera worldCamera = Camera.main;
			PKFxRenderingPlugin componentInChildren = Camera.main.gameObject.GetComponentInChildren<PKFxRenderingPlugin>();
			if (componentInChildren != null)
			{
				Camera component2 = componentInChildren.gameObject.GetComponent<Camera>();
				if (component2 != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UISetCanvasRenderCameraToMain.Update()).MethodHandle;
					}
					worldCamera = component2;
				}
			}
			component.worldCamera = worldCamera;
			base.enabled = false;
		}
	}
}
