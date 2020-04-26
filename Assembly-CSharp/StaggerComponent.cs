using UnityEngine;

public class StaggerComponent : MonoBehaviour
{
	public bool DoActivateOnStagger
	{
		get;
		set;
	}

	public static void SetStaggerComponent(GameObject obj, bool setActive, bool instantTurnOffIfInactive = true)
	{
		if (!(obj != null))
		{
			return;
		}
		while (true)
		{
			if (instantTurnOffIfInactive)
			{
				if (!setActive)
				{
					UIManager.SetGameObjectActive(obj, false);
				}
			}
			if (obj.GetComponent<StaggerComponent>() == null)
			{
				obj.AddComponent<StaggerComponent>();
			}
			obj.GetComponent<StaggerComponent>().DoActivateOnStagger = setActive;
			return;
		}
	}
}
