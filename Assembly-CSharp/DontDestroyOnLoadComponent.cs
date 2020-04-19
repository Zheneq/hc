using System;
using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
	private void Awake()
	{
		if (base.gameObject.transform.parent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DontDestroyOnLoadComponent.Awake()).MethodHandle;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
