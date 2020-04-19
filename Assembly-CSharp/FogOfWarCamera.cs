using System;
using UnityEngine;

public class FogOfWarCamera : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
	}

	public static FogOfWarCamera GetFirstFogOfWarCamera()
	{
		FogOfWarCamera fogOfWarCamera = null;
		foreach (Camera camera in Camera.allCameras)
		{
			fogOfWarCamera = camera.GetComponent<FogOfWarCamera>();
			if (fogOfWarCamera != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarCamera.GetFirstFogOfWarCamera()).MethodHandle;
				}
				return fogOfWarCamera;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return fogOfWarCamera;
		}
	}
}
