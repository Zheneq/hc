using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActorDebugLabel : MonoBehaviour
{
	public Text m_label;

	private ActorData m_actorData;

	private Dictionary<string, string> m_entries = new Dictionary<string, string>();

	private HashSet<string> m_shouldKeep = new HashSet<string>();

	private void Start()
	{
	}

	public void Setup(ActorData actorData)
	{
		this.m_actorData = actorData;
	}

	public void SetEntry(string key, string displayStr)
	{
		this.m_entries[key] = displayStr;
		this.m_shouldKeep.Add(key);
	}

	private void Update()
	{
		string text = string.Empty;
		using (Dictionary<string, string>.Enumerator enumerator = this.m_entries.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> keyValuePair = enumerator.Current;
				if (this.m_shouldKeep.Contains(keyValuePair.Key))
				{
					text = text + keyValuePair.Value + "\n\n";
				}
			}
		}
		this.m_label.text = text;
		Canvas componentInParent = base.GetComponentInParent<Canvas>();
		RectTransform rectTransform = componentInParent.transform as RectTransform;
		Vector3 nameplatePosition = this.m_actorData.GetNameplatePosition(-10f);
		Vector2 vector = Camera.main.WorldToViewportPoint(nameplatePosition);
		Vector2 anchoredPosition = new Vector2(vector.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f, vector.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f);
		(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
		this.m_shouldKeep.Clear();
	}
}
