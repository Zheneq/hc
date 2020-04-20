using System;
using UnityEngine;

public class StaggerComponent : MonoBehaviour
{
	public bool DoActivateOnStagger { get; set; }

	public static void SetStaggerComponent(GameObject obj, bool setActive, bool instantTurnOffIfInactive = true)
	{
		if (obj != null)
		{
			if (instantTurnOffIfInactive)
			{
				if (!setActive)
				{
					UIManager.SetGameObjectActive(obj, false, null);
				}
			}
			if (obj.GetComponent<StaggerComponent>() == null)
			{
				obj.AddComponent<StaggerComponent>();
			}
			obj.GetComponent<StaggerComponent>().DoActivateOnStagger = setActive;
		}
	}
}
