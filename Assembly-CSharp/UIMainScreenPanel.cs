using UnityEngine;

public class UIMainScreenPanel : MonoBehaviour
{
	public UIAbilityBar m_abilityBar;

	public UICardBar m_cardBar;

	public UIQueueListPanel m_queueListPanel;

	public UINameplatePanel m_nameplatePanel;

	public UIOffscreenIndicatorPanel m_offscreenIndicatorPanel;

	public UIControlPointNameplatePanel m_controlPointNameplatePanel;

	public UICombatTextPanel m_combatTextPanel;

	public UIAlertDisplay m_alertDisplay;

	public GameObject m_gameSpecificDisplay;

	public RectTransform m_gameSpecificRectDisplay;

	public UIAutoCameraButton m_autoCameraButton;

	public UINotificationPanel m_notificationPanel;

	public UIPlayerDisplay m_playerDisplayPanel;

	public UISideNotifications m_sideNotificationsPanel;

	public UICharacterProfile m_characterProfile;

	public UIMinimap m_minimap;

	public GameObject m_bigPingPanel;

	public BigPingPanelControlpad m_bigPingPanelControlpad;

	public UIAbilitySelectPanel m_abilitySelectPanel;

	public UISpectatorHUD m_spectatorHUD;

	public RectTransform[] m_outsideHierachyContainers;

	public UIMouseTargetingCursor m_targetingCursor;

	private static UIMainScreenPanel s_instance;

	internal static UIMainScreenPanel Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_targetingCursor, false);
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void NotifyStartGame()
	{
		m_characterProfile.Setup();
		m_sideNotificationsPanel.Setup();
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible);
		for (int i = 0; i < m_outsideHierachyContainers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_outsideHierachyContainers[i], visible);
		}
		while (true)
		{
			Log.Info("HEALTHBARCHECK: ENDPOINT " + visible);
			UIManager.SetGameObjectActive(m_nameplatePanel, visible);
			if (visible)
			{
				while (true)
				{
					m_nameplatePanel.RefreshNameplates();
					return;
				}
			}
			return;
		}
	}
}
