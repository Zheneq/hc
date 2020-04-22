using UnityEngine;

public class UI_Persistent : MonoBehaviour
{
	public GameObject[] m_newFrontEndPersistantScreens;

	public GameObject[] m_enableOnVisible;

	private static UI_Persistent s_instance;

	public static UI_Persistent Get()
	{
		return s_instance;
	}

	public void NotifyFrontEndVisible(bool visible)
	{
		for (int i = 0; i < m_newFrontEndPersistantScreens.Length; i++)
		{
			UIManager.SetGameObjectActive(m_newFrontEndPersistantScreens[i], visible);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!visible)
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
				for (int j = 0; j < m_enableOnVisible.Length; j++)
				{
					UIManager.SetGameObjectActive(m_enableOnVisible[j], true);
				}
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private void Awake()
	{
		s_instance = this;
		if (!(base.gameObject.transform.parent == null))
		{
			return;
		}
		while (true)
		{
			switch (6)
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

	private void OnDestroy()
	{
		s_instance = null;
	}
}
