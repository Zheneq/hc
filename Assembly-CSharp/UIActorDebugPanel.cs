using System;
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
		return UIActorDebugPanel.s_instance;
	}

	private void Awake()
	{
		UIManager.SetGameObjectActive(this, false, null);
		UIActorDebugPanel.s_instance = this;
	}

	public void OnActorDestroyed(ActorData actor)
	{
		if (this.m_actorLabels.ContainsKey(actor))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorDebugPanel.OnActorDestroyed(ActorData)).MethodHandle;
			}
			UIActorDebugLabel uiactorDebugLabel = this.m_actorLabels[actor];
			uiactorDebugLabel.m_label.text = string.Empty;
			this.m_actorLabels.Remove(actor);
			UnityEngine.Object.Destroy(uiactorDebugLabel);
		}
		if (this.m_shouldDisplay.ContainsKey(actor))
		{
			this.m_shouldDisplay.Remove(actor);
		}
	}

	public void Reset()
	{
		using (Dictionary<ActorData, UIActorDebugLabel>.ValueCollection.Enumerator enumerator = this.m_actorLabels.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIActorDebugLabel uiactorDebugLabel = enumerator.Current;
				UnityEngine.Object.Destroy(uiactorDebugLabel.gameObject);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorDebugPanel.Reset()).MethodHandle;
			}
		}
		this.m_actorLabels.Clear();
		this.m_shouldDisplay.Clear();
	}

	public void SetActorValue(ActorData actorData, string key, string displayStr)
	{
		this.m_shouldDisplay[actorData] = true;
		UIActorDebugLabel uiactorDebugLabel;
		if (!this.m_actorLabels.ContainsKey(actorData))
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIActorDebugPanel.SetActorValue(ActorData, string, string)).MethodHandle;
			}
			uiactorDebugLabel = UnityEngine.Object.Instantiate<UIActorDebugLabel>(this.m_actorDebugLabelPrefab);
			uiactorDebugLabel.Setup(actorData);
			uiactorDebugLabel.transform.SetParent(base.transform);
			this.m_actorLabels[actorData] = uiactorDebugLabel;
		}
		else
		{
			uiactorDebugLabel = this.m_actorLabels[actorData];
		}
		uiactorDebugLabel.SetEntry(key, displayStr);
		UIManager.SetGameObjectActive(this, true, null);
	}
}
