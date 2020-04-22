using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
	private void Awake()
	{
		if (!(base.gameObject.transform.parent == null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
	}
}
