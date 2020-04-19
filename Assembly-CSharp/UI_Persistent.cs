using System;
using UnityEngine;

public class UI_Persistent : MonoBehaviour
{
	public GameObject[] m_newFrontEndPersistantScreens;

	public GameObject[] m_enableOnVisible;

	private static UI_Persistent s_instance;

	public static UI_Persistent Get()
	{
		return UI_Persistent.s_instance;
	}

	public void NotifyFrontEndVisible(bool visible)
	{
		for (int i = 0; i < this.m_newFrontEndPersistantScreens.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_newFrontEndPersistantScreens[i], visible, null);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UI_Persistent.NotifyFrontEndVisible(bool)).MethodHandle;
		}
		if (visible)
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
			for (int j = 0; j < this.m_enableOnVisible.Length; j++)
			{
				UIManager.SetGameObjectActive(this.m_enableOnVisible[j], true, null);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void Awake()
	{
		UI_Persistent.s_instance = this;
		if (base.gameObject.transform.parent == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UI_Persistent.Awake()).MethodHandle;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		UI_Persistent.s_instance = null;
	}
}
