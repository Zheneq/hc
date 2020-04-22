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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (instantTurnOffIfInactive)
			{
				while (true)
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
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(obj, false);
				}
			}
			if (obj.GetComponent<StaggerComponent>() == null)
			{
				while (true)
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
			return;
		}
	}
}
