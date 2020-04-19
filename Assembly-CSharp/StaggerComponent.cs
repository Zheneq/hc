using System;
using UnityEngine;

public class StaggerComponent : MonoBehaviour
{
	public bool DoActivateOnStagger { get; set; }

	public static void SetStaggerComponent(GameObject obj, bool setActive, bool instantTurnOffIfInactive = true)
	{
		if (obj != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StaggerComponent.SetStaggerComponent(GameObject, bool, bool)).MethodHandle;
			}
			if (instantTurnOffIfInactive)
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
				if (!setActive)
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
					UIManager.SetGameObjectActive(obj, false, null);
				}
			}
			if (obj.GetComponent<StaggerComponent>() == null)
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
				obj.AddComponent<StaggerComponent>();
			}
			obj.GetComponent<StaggerComponent>().DoActivateOnStagger = setActive;
		}
	}
}
