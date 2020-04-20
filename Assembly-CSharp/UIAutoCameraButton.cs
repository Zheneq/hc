using System;
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
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnAutoCameraButtonClick));
		UIEventTriggerUtils.AddListener(this.m_mouseHitBox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnToggleClick));
		UIEventTriggerUtils.AddListener(this.m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnToggleEnter));
		UIEventTriggerUtils.AddListener(this.m_clockwiseRotateHitbox, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.ToggleClockwiseDown));
		UIEventTriggerUtils.AddListener(this.m_clockwiseRotateHitbox, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.ToggleClockwiseUp));
		UIEventTriggerUtils.AddListener(this.m_clockwiseRotateHitbox, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.ToggleClockwiseUp));
		UIEventTriggerUtils.AddListener(this.m_counterClockwiseRotateHitbox, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.ToggleCounterClockwiseDown));
		UIEventTriggerUtils.AddListener(this.m_counterClockwiseRotateHitbox, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.ToggleCounterClockwiseUp));
		UIEventTriggerUtils.AddListener(this.m_counterClockwiseRotateHitbox, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.ToggleCounterClockwiseUp));
		Graphic[] componentsInChildren = base.gameObject.GetComponentsInChildren<Graphic>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != this.m_mouseHitBox)
			{
				if (componentsInChildren[i].gameObject != this.m_clockwiseRotateHitbox)
				{
					if (componentsInChildren[i].gameObject != this.m_counterClockwiseRotateHitbox)
					{
						componentsInChildren[i].raycastTarget = false;
					}
				}
			}
		}
	}

	private void OnEnable()
	{
		this.RefreshAutoCameraButton();
		if (GameFlowData.Get() != null)
		{
			this.resolutionPhase = !GameFlowData.Get().IsInResolveState();
		}
	}

	internal void RefreshAutoCameraButton()
	{
		AccountPreferences accountPreferences = AccountPreferences.Get();
		if (accountPreferences != null)
		{
			bool @bool = accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
			UIManager.SetGameObjectActive(this.m_autoCameraOnSprite, @bool, null);
			UIManager.SetGameObjectActive(this.m_autoCameraOffSprite, !@bool, null);
		}
	}

	internal void OnPlayerMovedCamera()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().IsInResolveState())
			{
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
					{
						if (this.m_visibleRemainingTime <= 0f)
						{
							if (!this.autoCenterCamera)
							{
								this.m_camAnimatorController.Play("CameraIn", 0, 0f);
							}
						}
						this.m_visibleRemainingTime = this.m_visibleDuration;
						if (this.autoCenterCamera)
						{
							this.m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
						}
					}
				}
			}
		}
	}

	public void OnToggleEnter(BaseEventData data)
	{
		this.m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
	}

	public void OnToggleClick(BaseEventData data)
	{
		AccountPreferences accountPreferences = AccountPreferences.Get();
		CameraManager cameraManager = CameraManager.Get();
		if (accountPreferences != null & cameraManager != null)
		{
			bool flag = CameraManager.Get().UseCameraToggleKey;
			if (!flag)
			{
				bool flag2;
				if (Input.GetMouseButtonUp(1))
				{
					if (GameFlowData.Get() != null)
					{
						if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
						{
							if (!GameFlowData.Get().GetPause())
							{
								flag2 = (GameFlowData.Get().GetTimeInState() >= 1.5f);
							}
							else
							{
								flag2 = true;
							}
							goto IL_D0;
						}
					}
				}
				flag2 = false;
				IL_D0:
				flag = flag2;
			}
			if (flag)
			{
				bool flag3 = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
				accountPreferences.SetBool(BoolPreference.AutoCameraCenter, flag3);
				this.RefreshAutoCameraButton();
				AbilitiesCamera abilitiesCamera = cameraManager.GetAbilitiesCamera();
				if (flag3)
				{
					if (abilitiesCamera != null)
					{
						abilitiesCamera.OnAutoCenterCameraPreferenceSet();
					}
				}
			}
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
		this.RefreshAutoCameraButton();
	}

	private void CamToggleAnimDone()
	{
		UIManager.SetGameObjectActive(this.m_autoCamLabelContainer, this.autoCenterCamera, null);
		UIManager.SetGameObjectActive(this.m_manualCamLabelContainer, !this.autoCenterCamera, null);
		if (!this.resolutionPhase)
		{
			this.m_visibleRemainingTime = -1f;
			this.m_camAnimatorController.Play("CameraOut");
		}
	}

	private void SetAutoCamBtnVisuals(bool autoCam)
	{
		if (this.autoCenterCamera != autoCam && Time.time - this.m_lastToggleTime >= 0.1f)
		{
			this.m_lastToggleTime = Time.time;
			bool flag = this.autoCenterCamera != autoCam;
			this.autoCenterCamera = autoCam;
			if (this.resolutionPhase)
			{
				UIManager.SetGameObjectActive(this.m_autoCamLabelContainer, true, null);
				UIManager.SetGameObjectActive(this.m_manualCamLabelContainer, true, null);
				if (this.autoCenterCamera)
				{
					this.m_camAnimatorController.Play("CameraToAuto", 0, 0f);
				}
				else
				{
					this.m_camAnimatorController.Play("CameraToManual", 0, 0f);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_autoCamLabelContainer, this.autoCenterCamera, null);
				UIManager.SetGameObjectActive(this.m_manualCamLabelContainer, !this.autoCenterCamera, null);
				if (flag)
				{
					this.m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
				}
			}
		}
	}

	private void CheckPhase()
	{
		if (this.resolutionPhase != GameFlowData.Get().IsInResolveState())
		{
			this.resolutionPhase = GameFlowData.Get().IsInResolveState();
			if (this.resolutionPhase)
			{
			}
			else
			{
				this.m_visibleRemainingTime = -1f;
				this.m_camAnimatorController.Play("CameraOut");
			}
		}
		else
		{
			this.SetAutoCamBtnVisuals(AccountPreferences.Get().GetBool(BoolPreference.AutoCameraCenter));
		}
	}

	private void Update()
	{
		if (!(GameManager.Get() == null) && GameManager.Get().GameConfig != null)
		{
			if (!(GameFlowData.Get() == null))
			{
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					UIManager.SetGameObjectActive(this.m_container, false, null);
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_container, true, null);
					this.CheckPhase();
				}
				if (this.m_visibleRemainingTime > 0f)
				{
					this.m_visibleRemainingTime -= Time.deltaTime;
					if (this.m_visibleRemainingTime <= 0f)
					{
						this.m_camAnimatorController.Play("CameraOut");
					}
				}
				GameFlowData gameFlowData = GameFlowData.Get();
				if (gameFlowData != null && gameFlowData.IsOwnerTargeting() != this.lastIsTargeting)
				{
					base.GetComponent<CanvasGroup>().blocksRaycasts = this.lastIsTargeting;
					this.lastIsTargeting = !this.lastIsTargeting;
				}
				return;
			}
		}
		UIManager.SetGameObjectActive(this.m_container, false, null);
		this.m_visibleRemainingTime = -1f;
	}
}
