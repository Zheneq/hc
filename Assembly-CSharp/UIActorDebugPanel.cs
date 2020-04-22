using System.Collections.Generic;
using UnityEngine;

public class UIActorDebugPanel : MonoBehaviour
{
	public UIActorDebugLabel m_actorDebugLabelPrefab;

	private Dictionary<ActorData, UIActorDebugLabel> m_actorLabels = new Dictionary<ActorData, UIActorDebugLabel>();

	private Dictionary<ActorData, bool> m_shouldDisplay = new Dictionary<ActorData, bool>();

	private static UIActorDebugPanel s_instance;

	public static UIActorDebugPanel Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		UIManager.SetGameObjectActive(this, false);
		s_instance = this;
	}

	public void OnActorDestroyed(ActorData actor)
	{
		if (m_actorLabels.ContainsKey(actor))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIActorDebugLabel uIActorDebugLabel = m_actorLabels[actor];
			uIActorDebugLabel.m_label.text = string.Empty;
			m_actorLabels.Remove(actor);
			Object.Destroy(uIActorDebugLabel);
		}
		if (m_shouldDisplay.ContainsKey(actor))
		{
			m_shouldDisplay.Remove(actor);
		}
	}

	public void Reset()
	{
		using (Dictionary<ActorData, UIActorDebugLabel>.ValueCollection.Enumerator enumerator = m_actorLabels.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIActorDebugLabel current = enumerator.Current;
				Object.Destroy(current.gameObject);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_0015;
				}
			}
			end_IL_0015:;
		}
		m_actorLabels.Clear();
		m_shouldDisplay.Clear();
	}

	public void SetActorValue(ActorData actorData, string key, string displayStr)
	{
		m_shouldDisplay[actorData] = true;
		UIActorDebugLabel uIActorDebugLabel = null;
		if (!m_actorLabels.ContainsKey(actorData))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			uIActorDebugLabel = Object.Instantiate(m_actorDebugLabelPrefab);
			uIActorDebugLabel.Setup(actorData);
			uIActorDebugLabel.transform.SetParent(base.transform);
			m_actorLabels[actorData] = uIActorDebugLabel;
		}
		else
		{
			uIActorDebugLabel = m_actorLabels[actorData];
		}
		uIActorDebugLabel.SetEntry(key, displayStr);
		UIManager.SetGameObjectActive(this, true);
	}
}
