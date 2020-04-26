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
		UIManager.SetGameObjectActive(SampleName, false);
		SampleName.transform.SetParent(base.gameObject.transform);
		Clear();
	}

	public void UpdateCharacterList(List<LobbyPlayerInfo> spectatorPlayerInfos)
	{
		Clear();
		using (List<LobbyPlayerInfo>.Enumerator enumerator = spectatorPlayerInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo current = enumerator.Current;
				AddPlayer(current.GetHandle());
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	private void Clear()
	{
		TextMeshProUGUI[] componentsInChildren = m_layout.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] array = componentsInChildren;
		foreach (TextMeshProUGUI textMeshProUGUI in array)
		{
			textMeshProUGUI.gameObject.transform.SetParent(null);
			Object.Destroy(textMeshProUGUI.gameObject);
		}
		while (true)
		{
			return;
		}
	}

	private void AddPlayer(string playerHandle)
	{
		GameObject gameObject = Object.Instantiate(SampleName);
		UIManager.SetGameObjectActive(gameObject, true);
		gameObject.transform.SetParent(m_layout.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
		component.text = playerHandle;
	}
}
