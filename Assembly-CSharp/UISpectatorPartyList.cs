using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISpectatorPartyList : MonoBehaviour
{
	public LayoutGroup m_layout;

	public GameObject SampleName;

	private void Awake()
	{
		UIManager.SetGameObjectActive(this.SampleName, false, null);
		this.SampleName.transform.SetParent(base.gameObject.transform);
		this.Clear();
	}

	public void UpdateCharacterList(List<LobbyPlayerInfo> spectatorPlayerInfos)
	{
		this.Clear();
		using (List<LobbyPlayerInfo>.Enumerator enumerator = spectatorPlayerInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
				this.AddPlayer(lobbyPlayerInfo.GetHandle());
			}
		}
	}

	private void Clear()
	{
		TextMeshProUGUI[] componentsInChildren = this.m_layout.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
		{
			textMeshProUGUI.gameObject.transform.SetParent(null);
			UnityEngine.Object.Destroy(textMeshProUGUI.gameObject);
		}
	}

	private void AddPlayer(string playerHandle)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SampleName);
		UIManager.SetGameObjectActive(gameObject, true, null);
		gameObject.transform.SetParent(this.m_layout.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
		component.text = playerHandle;
	}
}
