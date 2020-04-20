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
		if (visible)
		{
			for (int j = 0; j < this.m_enableOnVisible.Length; j++)
			{
				UIManager.SetGameObjectActive(this.m_enableOnVisible[j], true, null);
			}
		}
	}

	private void Awake()
	{
		UI_Persistent.s_instance = this;
		if (base.gameObject.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		UI_Persistent.s_instance = null;
	}
}
