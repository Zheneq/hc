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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Tutorial.Start()).MethodHandle;
			}
			RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.m_gameModePanelPrefab);
			this.m_tutorialPanel = rectTransform.GetComponent<UIObjectivePointsPanel>();
			if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Tutorial.Update()).MethodHandle;
			}
			if (HUD_UI.Get() != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Tutorial.OnDestroy()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Tutorial.SetUpGameUI(UIGameModePanel)).MethodHandle;
			}
			num = 0;
			tutorialInMatchValue = StringUtil.TR("Decision", "Global");
		}
		else if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
			if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Prep)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ServerClientUtils.GetCurrentActionPhase() != ActionBufferPhase.MovementChase)
				{
					goto IL_11A;
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
			}
			num = 4;
			tutorialInMatchValue = StringUtil.TR("Movement", "Global");
		}
		IL_11A:
		for (int i = 0; i < uiobjectivePointsPanel.PhaseIndicators.Length; i++)
		{
			if (num == i)
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
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
