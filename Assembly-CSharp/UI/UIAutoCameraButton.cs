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
        UIEventTriggerUtils.AddListener(gameObject, EventTriggerType.PointerClick, OnAutoCameraButtonClick);
        UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerClick, OnToggleClick);
        UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, OnToggleEnter);
        UIEventTriggerUtils.AddListener(m_clockwiseRotateHitbox, EventTriggerType.PointerDown, ToggleClockwiseDown);
        UIEventTriggerUtils.AddListener(m_clockwiseRotateHitbox, EventTriggerType.PointerUp, ToggleClockwiseUp);
        UIEventTriggerUtils.AddListener(m_clockwiseRotateHitbox, EventTriggerType.PointerExit, ToggleClockwiseUp);
        UIEventTriggerUtils.AddListener(m_counterClockwiseRotateHitbox, EventTriggerType.PointerDown, ToggleCounterClockwiseDown);
        UIEventTriggerUtils.AddListener(m_counterClockwiseRotateHitbox, EventTriggerType.PointerUp, ToggleCounterClockwiseUp);
        UIEventTriggerUtils.AddListener(m_counterClockwiseRotateHitbox, EventTriggerType.PointerExit, ToggleCounterClockwiseUp);

        foreach (Graphic graphic in gameObject.GetComponentsInChildren<Graphic>(true))
        {
            if (graphic != m_mouseHitBox
                && graphic.gameObject != m_clockwiseRotateHitbox
                && graphic.gameObject != m_counterClockwiseRotateHitbox)
            {
                graphic.raycastTarget = false;
            }
        }
    }

    private void OnEnable()
    {
        RefreshAutoCameraButton();
        if (GameFlowData.Get() != null)
        {
            resolutionPhase = !GameFlowData.Get().IsInResolveState();
        }
    }

    internal void RefreshAutoCameraButton()
    {
        AccountPreferences accountPreferences = AccountPreferences.Get();
        if (accountPreferences != null)
        {
            bool autoCameraCenter = accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
            UIManager.SetGameObjectActive(m_autoCameraOnSprite, autoCameraCenter);
            UIManager.SetGameObjectActive(m_autoCameraOffSprite, !autoCameraCenter);
        }
    }

    internal void OnPlayerMovedCamera()
    {
        if (GameFlowData.Get() == null
            || !GameFlowData.Get().IsInResolveState()
            || GameManager.Get() == null
            || GameManager.Get().GameConfig.GameType == GameType.Tutorial)
        {
            return;
        }

        if (m_visibleRemainingTime <= 0f && !autoCenterCamera)
        {
            m_camAnimatorController.Play("CameraIn", 0, 0f);
        }

        m_visibleRemainingTime = m_visibleDuration;
        if (autoCenterCamera)
        {
            m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
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
        if (accountPreferences == null || cameraManager == null)
        {
            return;
        }
        
        if (CameraManager.Get().UseCameraToggleKey
            || (Input.GetMouseButtonUp(1)
                && GameFlowData.Get() != null
                && GameFlowData.Get().gameState == GameState.BothTeams_Resolve
                && (GameFlowData.Get().GetPause() || GameFlowData.Get().GetTimeInState() >= 1.5f)))
        {
            bool autoCameraCenter = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
            accountPreferences.SetBool(BoolPreference.AutoCameraCenter, autoCameraCenter);
            RefreshAutoCameraButton();
            AbilitiesCamera abilitiesCamera = cameraManager.GetAbilitiesCamera();
            if (autoCameraCenter && abilitiesCamera != null)
            {
                abilitiesCamera.OnAutoCenterCameraPreferenceSet();
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
        bool autoCameraCenter = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
        accountPreferences.SetBool(BoolPreference.AutoCameraCenter, autoCameraCenter);
        RefreshAutoCameraButton();
    }

    private void CamToggleAnimDone()
    {
        UIManager.SetGameObjectActive(m_autoCamLabelContainer, autoCenterCamera);
        UIManager.SetGameObjectActive(m_manualCamLabelContainer, !autoCenterCamera);
        if (!resolutionPhase)
        {
            m_visibleRemainingTime = -1f;
            m_camAnimatorController.Play("CameraOut");
        }
    }

    private void SetAutoCamBtnVisuals(bool autoCam)
    {
        if (autoCenterCamera == autoCam || !(Time.time - m_lastToggleTime >= 0.1f))
        {
            return;
        }

        m_lastToggleTime = Time.time;
        bool isChange = autoCenterCamera != autoCam;
        autoCenterCamera = autoCam;
        if (resolutionPhase)
        {
            UIManager.SetGameObjectActive(m_autoCamLabelContainer, true);
            UIManager.SetGameObjectActive(m_manualCamLabelContainer, true);
            m_camAnimatorController.Play(autoCenterCamera ? "CameraToAuto" : "CameraToManual", 0, 0f);
        }
        else
        {
            UIManager.SetGameObjectActive(m_autoCamLabelContainer, autoCenterCamera);
            UIManager.SetGameObjectActive(m_manualCamLabelContainer, !autoCenterCamera);
            if (isChange)
            {
                m_camAnimatorController.Play("CameraWhiteStrip", 1, 0f);
            }
        }
    }

    private void CheckPhase()
    {
        if (resolutionPhase != GameFlowData.Get().IsInResolveState())
        {
            resolutionPhase = GameFlowData.Get().IsInResolveState();
            if (!resolutionPhase)
            {
                m_visibleRemainingTime = -1f;
                m_camAnimatorController.Play("CameraOut");
            }
        }
        else
        {
            SetAutoCamBtnVisuals(AccountPreferences.Get().GetBool(BoolPreference.AutoCameraCenter));
        }
    }

    private void Update()
    {
        if (GameManager.Get() == null || GameManager.Get().GameConfig == null || GameFlowData.Get() == null)
        {
            UIManager.SetGameObjectActive(m_container, false);
            m_visibleRemainingTime = -1f;
            return;
        }

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
        if (gameFlowData != null && gameFlowData.IsOwnerTargeting() != lastIsTargeting)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = lastIsTargeting;
            lastIsTargeting = !lastIsTargeting;
        }
    }
}