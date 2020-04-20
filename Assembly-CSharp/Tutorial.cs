using System;
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
		return Tutorial.s_instance;
	}

	private void Awake()
	{
		Tutorial.s_instance = this;
		this.m_startPending = false;
	}

	private void Start()
	{
		if (this.m_gameModePanelPrefab != null)
		{
			RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.m_gameModePanelPrefab);
			this.m_tutorialPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
			if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
			{
				if (HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay != null)
				{
					this.m_tutorialPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
					this.m_tutorialPanel.Setup(() => StringUtil.TR("Tutorial!", "GameModes"));
					this.m_tutorialPanel.transform.localScale = Vector3.one;
					return;
				}
			}
			this.m_startPending = true;
		}
	}

	private void Update()
	{
		if (this.m_startPending)
		{
			if (HUD_UI.Get() != null)
			{
				if (HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay != null)
				{
					this.m_startPending = false;
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.m_gameModePanelPrefab);
					this.m_tutorialPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
					this.m_tutorialPanel.transform.SetParent(HUD_UI.Get().m_mainScreenPanel.m_gameSpecificRectDisplay.transform);
					this.m_tutorialPanel.Setup(() => StringUtil.TR("Tutorial!", "GameModes"));
					this.m_tutorialPanel.transform.localScale = Vector3.one;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_tutorialPanel)
		{
			UnityEngine.Object.Destroy(this.m_tutorialPanel.gameObject);
			this.m_tutorialPanel = null;
		}
		Tutorial.s_instance = null;
	}

	public void SetUpGameUI(UIGameModePanel UIPanel)
	{
		UIObjectivePointsPanel uiobjectivePointsPanel = UIPanel as UIObjectivePointsPanel;
		if (uiobjectivePointsPanel == null)
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
			UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
			if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Prep)
			{
				if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
				{
					if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
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
					goto IL_11A;
				}
			}
			num = 4;
			tutorialInMatchValue = StringUtil.TR("Movement", "Global");
		}
		IL_11A:
		for (int i = 0; i < uiobjectivePointsPanel.PhaseIndicators.Length; i++)
		{
			if (num == i)
			{
				uiobjectivePointsPanel.SetPhaseIndicatorActive(true, i);
			}
			else
			{
				uiobjectivePointsPanel.SetPhaseIndicatorActive(false, i);
			}
		}
		uiobjectivePointsPanel.SetTutorialInMatchValue(tutorialInMatchValue);
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
