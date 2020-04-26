using CameraManagerInternal;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAutoCameraButton : MonoBehaviour
{
	public Image m_autoCameraOnSprite;

	public Image m_autoCameraOffSprite;

	public Animator m_camAnimatorController;

	public TextMeshProUGUI[] m_hotkeyLabels;

	public RectTransform m_autoCamLabelContainer;

	public RectTransform m_manualCamLabelContainer;

	public RectTransform m_container;

	public Image m_mouseHitBox;

	public GameObject m_clockwiseRotateHitbox;

	public GameObject m_counterClockwiseRotateHitbox;

	public float m_visibleDuration = 1.5f;

	private bool autoCenterCamera;

	private bool resolutionPhase;

	private bool lastIsTargeting;

	private float m_lastToggleTime;

	private float m_visibleRemainingTime = -1f;

	private void Start()
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, OnAutoCameraButtonClick);
		UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerClick, OnToggleClick);
		UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, OnToggleEnter);
		UIEventTriggerUtils.AddListener(m_clockwiseRotateHitbox, EventTriggerType.PointerDown, ToggleClockwiseDown);
		UIEventTriggerUtils.AddListener(m_clockwiseRotateHitbox, EventTriggerType.PointerUp, ToggleClockwiseUp);
		UIEventTriggerUtils.AddListener(m_clockwiseRotateHitbox, EventTriggerType.PointerExit, ToggleClockwiseUp);
		UIEventTriggerUtils.AddListener(m_counterClockwiseRotateHitbox, EventTriggerType.PointerDown, ToggleCounterClockwiseDown);
		UIEventTriggerUtils.AddListener(m_counterClockwiseRotateHitbox, EventTriggerType.PointerUp, ToggleCounterClockwiseUp);
		UIEventTriggerUtils.AddListener(m_counterClockwiseRotateHitbox, EventTriggerType.PointerExit, ToggleCounterClockwiseUp);
		Graphic[] componentsInChildren = base.gameObject.GetComponentsInChildren<Graphic>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] != m_mouseHitBox))
			{
				continue;
			}
			if (!(componentsInChildren[i].gameObject != m_clockwiseRotateHitbox))
			{
				continue;
			}
			if (componentsInChildren[i].gameObject != m_counterClockwiseRotateHitbox)
			{
				componentsInChildren[i].raycastTarget = false;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void OnEnable()
	{
		RefreshAutoCameraButton();
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			resolutionPhase = !GameFlowData.Get().IsInResolveState();
			return;
		}
	}

	internal void RefreshAutoCameraButton()
	{
		AccountPreferences accountPreferences = AccountPreferences.Get();
		if (accountPreferences != null)
		{
			bool @bool = accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
			UIManager.SetGameObjectActive(m_autoCameraOnSprite, @bool);
			UIManager.SetGameObjectActive(m_autoCameraOffSprite, !@bool);
		}
	}

	internal void OnPlayerMovedCamera()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!GameFlowData.Get().IsInResolveState())
			{
				return;
			}
			while (true)
			{
				if (!(GameManager.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
					{
						return;
					}
					while (true)
					{
						if (m_visibleRemainingTime <= 0f)
						{
							if (!autoCenterCamera)
							{
								m_camAnimatorController.Play("CameraIn", 0, 0f);
							}
						}
						m_visibleRemainingTime = m_visibleDuration;
						if (autoCenterCamera)
						{
							while (true)
							{
								m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	public void OnToggleEnter(BaseEventData data)
	{
		m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
	}

	public void OnToggleClick(BaseEventData data)
	{
		AccountPreferences accountPreferences = AccountPreferences.Get();
		CameraManager cameraManager = CameraManager.Get();
		if (!((accountPreferences != null) & (cameraManager != null)))
		{
			return;
		}
		while (true)
		{
			bool flag = CameraManager.Get().UseCameraToggleKey;
			int num;
			if (!flag)
			{
				if (Input.GetMouseButtonUp(1))
				{
					if (GameFlowData.Get() != null)
					{
						if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
						{
							if (!GameFlowData.Get().GetPause())
							{
								num = ((GameFlowData.Get().GetTimeInState() >= 1.5f) ? 1 : 0);
							}
							else
							{
								num = 1;
							}
							goto IL_00d0;
						}
					}
				}
				num = 0;
				goto IL_00d0;
			}
			goto IL_00d1;
			IL_00d1:
			if (!flag)
			{
				return;
			}
			while (true)
			{
				bool flag2 = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
				accountPreferences.SetBool(BoolPreference.AutoCameraCenter, flag2);
				RefreshAutoCameraButton();
				AbilitiesCamera abilitiesCamera = cameraManager.GetAbilitiesCamera();
				if (!flag2)
				{
					return;
				}
				while (true)
				{
					if (abilitiesCamera != null)
					{
						while (true)
						{
							abilitiesCamera.OnAutoCenterCameraPreferenceSet();
							return;
						}
					}
					return;
				}
			}
			IL_00d0:
			flag = ((byte)num != 0);
			goto IL_00d1;
		}
	}

	public void ToggleClockwiseDown(BaseEventData data)
	{
		CameraControls.Get().CameraRotateClockwiseToggled = true;
	}

	public void ToggleClockwiseUp(BaseEventData data)
	{
		CameraControls.Get().CameraRotateClockwiseToggled = false;
	}

	public void ToggleCounterClockwiseDown(BaseEventData data)
	{
		CameraControls.Get().CameraRotateCounterClockwiseToggled = true;
	}

	public void ToggleCounterClockwiseUp(BaseEventData data)
	{
		CameraControls.Get().CameraRotateCounterClockwiseToggled = false;
	}

	private void OnAutoCameraButtonClick(BaseEventData data)
	{
		AccountPreferences accountPreferences = AccountPreferences.Get();
		bool value = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
		accountPreferences.SetBool(BoolPreference.AutoCameraCenter, value);
		RefreshAutoCameraButton();
	}

	private void CamToggleAnimDone()
	{
		UIManager.SetGameObjectActive(m_autoCamLabelContainer, autoCenterCamera);
		UIManager.SetGameObjectActive(m_manualCamLabelContainer, !autoCenterCamera);
		if (resolutionPhase)
		{
			return;
		}
		while (true)
		{
			m_visibleRemainingTime = -1f;
			m_camAnimatorController.Play("CameraOut");
			return;
		}
	}

	private void SetAutoCamBtnVisuals(bool autoCam)
	{
		if (autoCenterCamera == autoCam || !(Time.time - m_lastToggleTime >= 0.1f))
		{
			return;
		}
		while (true)
		{
			m_lastToggleTime = Time.time;
			bool flag = autoCenterCamera != autoCam;
			autoCenterCamera = autoCam;
			if (resolutionPhase)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_autoCamLabelContainer, true);
						UIManager.SetGameObjectActive(m_manualCamLabelContainer, true);
						if (autoCenterCamera)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									m_camAnimatorController.Play("CameraToAuto", 0, 0f);
									return;
								}
							}
						}
						m_camAnimatorController.Play("CameraToManual", 0, 0f);
						return;
					}
				}
			}
			UIManager.SetGameObjectActive(m_autoCamLabelContainer, autoCenterCamera);
			UIManager.SetGameObjectActive(m_manualCamLabelContainer, !autoCenterCamera);
			if (flag)
			{
				while (true)
				{
					m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
					return;
				}
			}
			return;
		}
	}

	private void CheckPhase()
	{
		if (resolutionPhase != GameFlowData.Get().IsInResolveState())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					resolutionPhase = GameFlowData.Get().IsInResolveState();
					if (resolutionPhase)
					{
						while (true)
						{
							switch (5)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					m_visibleRemainingTime = -1f;
					m_camAnimatorController.Play("CameraOut");
					return;
				}
			}
		}
		SetAutoCamBtnVisuals(AccountPreferences.Get().GetBool(BoolPreference.AutoCameraCenter));
	}

	private void Update()
	{
		if (!(GameManager.Get() == null) && GameManager.Get().GameConfig != null)
		{
			if (!(GameFlowData.Get() == null))
			{
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					UIManager.SetGameObjectActive(m_container, false);
				}
				else
				{
					UIManager.SetGameObjectActive(m_container, true);
					CheckPhase();
				}
				if (m_visibleRemainingTime > 0f)
				{
					m_visibleRemainingTime -= Time.deltaTime;
					if (m_visibleRemainingTime <= 0f)
					{
						m_camAnimatorController.Play("CameraOut");
					}
				}
				GameFlowData gameFlowData = GameFlowData.Get();
				if (!(gameFlowData != null) || gameFlowData.IsOwnerTargeting() == lastIsTargeting)
				{
					return;
				}
				while (true)
				{
					GetComponent<CanvasGroup>().blocksRaycasts = lastIsTargeting;
					lastIsTargeting = !lastIsTargeting;
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_container, false);
		m_visibleRemainingTime = -1f;
	}
}
