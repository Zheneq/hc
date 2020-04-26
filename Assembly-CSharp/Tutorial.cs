using UnityEngine;
using UnityEngine.Networking;

public class Tutorial : NetworkBehaviour
{
	public RectTransform m_gameModePanelPrefab;

	public UIObjectivePointsPanel m_tutorialPanel;

	private bool m_startPending;

	private static Tutorial s_instance;

	public static Tutorial Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_startPending = false;
	}

	private void Start()
	{
		if (!(m_gameModePanelPrefab != null))
		{
			return;
		}
		while (true)
		{
			RectTransform rectTransform = Object.Instantiate(m_gameModePanelPrefab);
			m_tutorialPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
			if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
			{
				if (HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay != null)
				{
					m_tutorialPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
					m_tutorialPanel.Setup(() => StringUtil.TR("Tutorial!", "GameModes"));
					m_tutorialPanel.transform.localScale = Vector3.one;
					return;
				}
			}
			m_startPending = true;
			return;
		}
	}

	private void Update()
	{
		if (!m_startPending)
		{
			return;
		}
		while (true)
		{
			if (!(HUD_UI.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay != null)
				{
					m_startPending = false;
					RectTransform rectTransform = Object.Instantiate(m_gameModePanelPrefab);
					m_tutorialPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
					m_tutorialPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
					m_tutorialPanel.Setup(() => StringUtil.TR("Tutorial!", "GameModes"));
					m_tutorialPanel.transform.localScale = Vector3.one;
				}
				return;
			}
		}
	}

	private void OnDestroy()
	{
		if ((bool)m_tutorialPanel)
		{
			Object.Destroy(m_tutorialPanel.gameObject);
			m_tutorialPanel = null;
		}
		s_instance = null;
	}

	public void SetUpGameUI(UIGameModePanel UIPanel)
	{
		UIObjectivePointsPanel uIObjectivePointsPanel = UIPanel as UIObjectivePointsPanel;
		if (uIObjectivePointsPanel == null)
		{
			return;
		}
		string tutorialInMatchValue = string.Empty;
		int num = -1;
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
		{
			num = 0;
			tutorialInMatchValue = StringUtil.TR("Decision", "Global");
		}
		else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
		{
			UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
			if (uIPhaseFromAbilityPriority != 0)
			{
				if (uIPhaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
				{
					if (uIPhaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
					{
					}
					else
					{
						num = 3;
						tutorialInMatchValue = StringUtil.TR("Blast", "Global");
					}
				}
				else
				{
					num = 2;
					tutorialInMatchValue = StringUtil.TR("Dash", "Global");
				}
			}
			else
			{
				num = 1;
				tutorialInMatchValue = StringUtil.TR("Prep", "Global");
			}
		}
		else
		{
			if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.Movement)
			{
				if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.MovementChase)
				{
					goto IL_011a;
				}
			}
			num = 4;
			tutorialInMatchValue = StringUtil.TR("Movement", "Global");
		}
		goto IL_011a;
		IL_011a:
		for (int i = 0; i < uIObjectivePointsPanel.PhaseIndicators.Length; i++)
		{
			if (num == i)
			{
				uIObjectivePointsPanel.SetPhaseIndicatorActive(true, i);
			}
			else
			{
				uIObjectivePointsPanel.SetPhaseIndicatorActive(false, i);
			}
		}
		uIObjectivePointsPanel.SetTutorialInMatchValue(tutorialInMatchValue);
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
