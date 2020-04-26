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
		m_actorData = actorData;
	}

	public void SetEntry(string key, string displayStr)
	{
		m_entries[key] = displayStr;
		m_shouldKeep.Add(key);
	}

	private void Update()
	{
		string text = string.Empty;
		using (Dictionary<string, string>.Enumerator enumerator = m_entries.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				if (m_shouldKeep.Contains(current.Key))
				{
					text = text + current.Value + "\n\n";
				}
			}
		}
		m_label.text = text;
		Canvas componentInParent = GetComponentInParent<Canvas>();
		RectTransform rectTransform = componentInParent.transform as RectTransform;
		Vector3 nameplatePosition = m_actorData.GetNameplatePosition(-10f);
		Vector2 vector = Camera.main.WorldToViewportPoint(nameplatePosition);
		float x = vector.x;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float num = x * sizeDelta.x;
		Vector2 sizeDelta2 = rectTransform.sizeDelta;
		float x2 = num - sizeDelta2.x * 0.5f;
		float y = vector.y;
		Vector2 sizeDelta3 = rectTransform.sizeDelta;
		float num2 = y * sizeDelta3.y;
		Vector2 sizeDelta4 = rectTransform.sizeDelta;
		Vector2 anchoredPosition = new Vector2(x2, num2 - sizeDelta4.y * 0.5f);
		(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
		m_shouldKeep.Clear();
	}
}
