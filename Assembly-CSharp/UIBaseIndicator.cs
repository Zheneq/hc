using UnityEngine;
using UnityEngine.UI;

public abstract class UIBaseIndicator : MonoBehaviour
{
	protected ActorData m_attachedToActor;

	protected ControlPoint m_attachedToControlPoint;

	protected CTF_Flag m_attachedToFlag;

	protected BoardRegion m_attachedToBoardRegion;

	protected UIWorldPing m_attachedToPing;

	protected Team m_regionTeam;

	private bool m_visible;

	protected Canvas m_canvas;

	protected Rect m_screenRect;

	protected RectTransform m_canvasRect;

	public RectTransform m_childContainer;

	public Image m_characterIcon;

	public Image m_friendlyFrame;

	public Image m_friendlyBackground;

	public Image m_enemyFrame;

	public Image m_enemyBackground;

	public Image m_objectiveFrame;

	public Image m_objectiveBackground;

	public Image m_grayFrame;

	public Image m_grayCharacterIcon;

	public Image m_grayOptionalArrow;

	public Image m_optionalArrow;

	public Image m_briefcaseIcon;

	public Image m_dropzoneIcon;

	public Image m_pingAssistIcon;

	public Image m_pingDefaultIcon;

	public Image m_pingEnemyIcon;

	public Image m_pingMoveIcon;

	public Image m_pingDefendIcon;

	public RectTransform m_pingGroupContainer;

	public Image m_pingCharacterIcon;

	private Image m_selectedFrame;

	private Image m_selectedCharacterIcon;

	private Image m_selectedOptionalArrow;

	public float angleOffset = 45f;

	public float borderLeft;

	public float borderRight;

	public float borderTop;

	public float borderBottom;

	private UIOffscreenIndicatorPanel m_parentPanel;

	private bool m_initialized;

	private bool m_forceUpdateFrame = true;

	private bool m_forceUpdateGrayout = true;

	private Team m_curTeam;

	private bool m_isGrayedOut;

	protected abstract bool CalculateVisibility();

	protected abstract bool ShouldHideOptionalArrowWhenOffscreen();

	protected abstract Vector2 CalculateScreenPos();

	protected abstract bool IsVisibleWhenOnScreen();

	protected abstract bool ShouldRotate();

	protected abstract bool ShouldGrayOutIndicator();

	protected virtual void SetupCharacterIcons(ActorData actorData)
	{
		m_characterIcon.sprite = actorData.GetAliveHUDIcon();
	}

	public ActorData GetAttachedActor()
	{
		return m_attachedToActor;
	}

	public ControlPoint GetAttachedControlPoint()
	{
		return m_attachedToControlPoint;
	}

	public CTF_Flag GetAttachedFlag()
	{
		return m_attachedToFlag;
	}

	public BoardRegion GetAttachedRegion()
	{
		return m_attachedToBoardRegion;
	}

	public UIWorldPing GetAttachedPing()
	{
		return m_attachedToPing;
	}

	public void Start()
	{
		if (m_grayCharacterIcon != null)
		{
			UIManager.SetGameObjectActive(m_grayCharacterIcon, false);
		}
		if (m_grayFrame != null)
		{
			UIManager.SetGameObjectActive(m_grayFrame, false);
		}
		if (!(m_grayOptionalArrow != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_grayOptionalArrow, false);
			return;
		}
	}

	public void Setup(ActorData actorData, UIOffscreenIndicatorPanel panel)
	{
		m_initialized = false;
		m_attachedToActor = actorData;
		m_attachedToPing = null;
		SetupCharacterIcons(actorData);
		if (actorData == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("Offscreen Indicator set up with no actor provided");
					return;
				}
			}
		}
		Setup_Base(panel);
	}

	public void Setup(ControlPoint controlPoint, UIOffscreenIndicatorPanel panel)
	{
		m_initialized = false;
		m_attachedToActor = null;
		m_attachedToControlPoint = controlPoint;
		m_characterIcon.sprite = controlPoint.m_icon;
		m_attachedToPing = null;
		Setup_Base(panel);
	}

	public void Setup(CTF_Flag flag, UIOffscreenIndicatorPanel panel)
	{
		m_initialized = false;
		m_attachedToActor = null;
		m_attachedToControlPoint = null;
		m_attachedToFlag = flag;
		m_attachedToPing = null;
		Setup_Base(panel);
		UIManager.SetGameObjectActive(m_briefcaseIcon, true);
		UIManager.SetGameObjectActive(m_characterIcon, false);
	}

	public void Setup(BoardRegion boardRegion, UIOffscreenIndicatorPanel panel, Team teamRegion, bool isFlagTurnIn)
	{
		m_initialized = false;
		m_attachedToActor = null;
		m_attachedToControlPoint = null;
		m_attachedToFlag = null;
		m_attachedToBoardRegion = boardRegion;
		m_regionTeam = teamRegion;
		m_attachedToPing = null;
		Setup_Base(panel);
		if (isFlagTurnIn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_dropzoneIcon, true);
					UIManager.SetGameObjectActive(m_characterIcon, false);
					return;
				}
			}
		}
		m_characterIcon.sprite = boardRegion.GetIcon();
		m_characterIcon.transform.localScale = Vector3.one;
	}

	public void Setup(UIWorldPing ping, ActorController.PingType pingtype, ActorData pingerActorData, UIOffscreenIndicatorPanel panel)
	{
		m_initialized = false;
		m_attachedToActor = null;
		m_attachedToControlPoint = null;
		m_attachedToFlag = null;
		m_attachedToPing = ping;
		Setup_Base(panel);
		UIManager.SetGameObjectActive(m_characterIcon, false);
		UIManager.SetGameObjectActive(m_pingAssistIcon, pingtype == ActorController.PingType.Assist);
		UIManager.SetGameObjectActive(m_pingDefendIcon, pingtype == ActorController.PingType.Defend);
		UIManager.SetGameObjectActive(m_pingDefaultIcon, pingtype == ActorController.PingType.Default);
		UIManager.SetGameObjectActive(m_pingEnemyIcon, pingtype == ActorController.PingType.Enemy);
		UIManager.SetGameObjectActive(m_pingMoveIcon, pingtype == ActorController.PingType.Move);
		UIManager.SetGameObjectActive(m_pingGroupContainer, pingerActorData != null);
		if (!(pingerActorData != null))
		{
			return;
		}
		while (true)
		{
			m_pingCharacterIcon.sprite = pingerActorData.GetAliveHUDIcon();
			return;
		}
	}

	private void CheckForFlagHolder()
	{
		if (!(CaptureTheFlag.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (m_attachedToActor != null && m_briefcaseIcon != null)
			{
				while (true)
				{
					bool flag = CaptureTheFlag.GetMainFlagCarrier_Client() == m_attachedToActor;
					UIManager.SetGameObjectActive(m_briefcaseIcon, flag);
					UIManager.SetGameObjectActive(m_characterIcon, !flag);
					return;
				}
			}
			return;
		}
	}

	private void Setup_Base(UIOffscreenIndicatorPanel panel)
	{
		m_parentPanel = panel;
		if (m_parentPanel == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Log.Error("Offscreen Indicator set up with no parent panel.");
					return;
				}
			}
		}
		if (m_canvas == null)
		{
			m_canvas = HUD_UI.Get().m_mainCanvas;
			if (m_canvas == null)
			{
				Log.Error("No UI canvas found in offscreen indicator parent");
				return;
			}
		}
		m_canvasRect = (m_canvas.transform as RectTransform);
		m_visible = CalculateVisibility();
		UIManager.SetGameObjectActive(m_childContainer, m_visible);
		if (m_briefcaseIcon != null)
		{
			UIManager.SetGameObjectActive(m_briefcaseIcon, false);
		}
		if (m_dropzoneIcon != null)
		{
			UIManager.SetGameObjectActive(m_dropzoneIcon, false);
		}
		if (m_pingGroupContainer != null)
		{
			UIManager.SetGameObjectActive(m_pingAssistIcon, false);
			UIManager.SetGameObjectActive(m_pingDefaultIcon, false);
			UIManager.SetGameObjectActive(m_pingEnemyIcon, false);
			UIManager.SetGameObjectActive(m_pingMoveIcon, false);
			UIManager.SetGameObjectActive(m_pingGroupContainer, false);
		}
		UIManager.SetGameObjectActive(m_characterIcon, true);
		m_initialized = true;
	}

	private void UpdateFrame()
	{
		if (!(m_briefcaseIcon == null))
		{
			if (m_briefcaseIcon.gameObject.activeSelf)
			{
				goto IL_00bb;
			}
		}
		if (!(m_dropzoneIcon == null))
		{
			if (m_dropzoneIcon.gameObject.activeSelf)
			{
				goto IL_00bb;
			}
		}
		if (!(m_pingGroupContainer == null))
		{
			if (m_pingGroupContainer.gameObject.activeSelf)
			{
				goto IL_00bb;
			}
		}
		UIManager.SetGameObjectActive(m_characterIcon, true);
		goto IL_00bb;
		IL_0394:
		m_forceUpdateFrame = false;
		return;
		IL_00bb:
		m_selectedCharacterIcon = m_characterIcon;
		m_selectedOptionalArrow = m_optionalArrow;
		if (m_attachedToActor == null)
		{
			m_selectedFrame = m_objectiveFrame;
			if (m_attachedToBoardRegion != null)
			{
				if (m_regionTeam != Team.Invalid)
				{
					UIManager.SetGameObjectActive(m_friendlyFrame, m_regionTeam == Team.TeamA);
					UIManager.SetGameObjectActive(m_friendlyBackground, m_regionTeam == Team.TeamA);
					UIManager.SetGameObjectActive(m_enemyFrame, m_regionTeam == Team.TeamB);
					UIManager.SetGameObjectActive(m_enemyBackground, m_regionTeam == Team.TeamB);
					UIManager.SetGameObjectActive(m_objectiveFrame, false);
					UIManager.SetGameObjectActive(m_objectiveBackground, false);
					if (m_regionTeam == Team.TeamA)
					{
						m_selectedFrame = m_friendlyFrame;
					}
					else if (m_regionTeam == Team.TeamB)
					{
						m_selectedFrame = m_enemyFrame;
					}
					goto IL_0394;
				}
			}
			if (m_attachedToPing != null)
			{
				UIManager.SetGameObjectActive(m_friendlyFrame, true);
				UIManager.SetGameObjectActive(m_friendlyBackground, true);
				UIManager.SetGameObjectActive(m_enemyFrame, false);
				UIManager.SetGameObjectActive(m_enemyBackground, false);
				UIManager.SetGameObjectActive(m_objectiveFrame, false);
				UIManager.SetGameObjectActive(m_objectiveBackground, false);
			}
			else
			{
				UIManager.SetGameObjectActive(m_friendlyFrame, false);
				UIManager.SetGameObjectActive(m_friendlyBackground, false);
				UIManager.SetGameObjectActive(m_enemyFrame, false);
				UIManager.SetGameObjectActive(m_enemyBackground, false);
				UIManager.SetGameObjectActive(m_objectiveFrame, true);
				UIManager.SetGameObjectActive(m_objectiveBackground, true);
			}
		}
		else
		{
			GameFlowData gameFlowData = GameFlowData.Get();
			Team team = Team.TeamA;
			bool flag = true;
			if (gameFlowData != null && gameFlowData.LocalPlayerData != null)
			{
				if (gameFlowData.activeOwnedActorData != null)
				{
					team = gameFlowData.activeOwnedActorData.GetTeam();
				}
				flag = (team == m_attachedToActor.GetTeam());
			}
			if (team == m_curTeam)
			{
				if (!m_forceUpdateFrame)
				{
					goto IL_0394;
				}
			}
			m_curTeam = team;
			UIManager.SetGameObjectActive(m_friendlyFrame, flag);
			UIManager.SetGameObjectActive(m_friendlyBackground, flag);
			UIManager.SetGameObjectActive(m_enemyFrame, !flag);
			UIManager.SetGameObjectActive(m_enemyBackground, !flag);
			UIManager.SetGameObjectActive(m_objectiveFrame, false);
			UIManager.SetGameObjectActive(m_objectiveBackground, false);
			if (flag)
			{
				m_selectedFrame = m_friendlyFrame;
			}
			else
			{
				m_selectedFrame = m_enemyFrame;
			}
		}
		goto IL_0394;
	}

	private void UpdateGrayedOut()
	{
		if (!ShouldGrayOutIndicator())
		{
			goto IL_00d5;
		}
		if (m_isGrayedOut)
		{
			if (!m_forceUpdateGrayout)
			{
				goto IL_00d5;
			}
		}
		m_isGrayedOut = true;
		UIManager.SetGameObjectActive(m_selectedFrame, false);
		UIManager.SetGameObjectActive(m_selectedCharacterIcon, false);
		UIManager.SetGameObjectActive(m_selectedOptionalArrow, false);
		m_selectedFrame = m_grayFrame;
		m_selectedCharacterIcon = m_grayCharacterIcon;
		m_selectedOptionalArrow = m_grayOptionalArrow;
		UIManager.SetGameObjectActive(m_selectedFrame, true);
		UIManager.SetGameObjectActive(m_selectedCharacterIcon, true);
		if (m_selectedOptionalArrow != null)
		{
			UIManager.SetGameObjectActive(m_selectedOptionalArrow, true);
		}
		goto IL_014e;
		IL_014e:
		m_forceUpdateGrayout = false;
		return;
		IL_00d5:
		if (!ShouldGrayOutIndicator())
		{
			if (!m_isGrayedOut)
			{
				if (!m_forceUpdateGrayout)
				{
					goto IL_014e;
				}
			}
			m_isGrayedOut = false;
			UIManager.SetGameObjectActive(m_selectedFrame, false);
			UIManager.SetGameObjectActive(m_selectedCharacterIcon, false);
			if (m_selectedOptionalArrow != null)
			{
				UIManager.SetGameObjectActive(m_selectedOptionalArrow, false);
			}
			m_forceUpdateFrame = true;
			UpdateFrame();
		}
		goto IL_014e;
	}

	private void LateUpdate()
	{
		if (Camera.main == null)
		{
			return;
		}
		while (true)
		{
			if (!m_initialized)
			{
				return;
			}
			if (m_forceUpdateFrame)
			{
				UpdateFrame();
			}
			if (!CalculateVisibility())
			{
				SetVisible(false);
				return;
			}
			CheckForFlagHolder();
			UpdateGrayedOut();
			Vector2 vector = CalculateScreenPos();
			float x = m_parentPanel.borderLeft;
			float y = m_parentPanel.borderBottom;
			Vector2 sizeDelta = m_canvasRect.sizeDelta;
			float width = sizeDelta.x - (m_parentPanel.borderRight + m_parentPanel.borderLeft);
			Vector2 sizeDelta2 = m_canvasRect.sizeDelta;
			m_screenRect = new Rect(x, y, width, sizeDelta2.y - (m_parentPanel.borderBottom + m_parentPanel.borderTop));
			if (!IsVisibleWhenOnScreen())
			{
				if (m_screenRect.Contains(vector))
				{
					SetVisible(false);
					return;
				}
			}
			if (IsVisibleWhenOnScreen())
			{
				if (m_screenRect.Contains(vector))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							SetVisible(true);
							(base.gameObject.transform as RectTransform).anchoredPosition = vector;
							if (ShouldRotate())
							{
								(m_selectedFrame.gameObject.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, -135f);
							}
							if (m_selectedOptionalArrow != null)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										if (!m_selectedOptionalArrow.gameObject.activeSelf)
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													UIManager.SetGameObjectActive(m_selectedOptionalArrow, true);
													return;
												}
											}
										}
										return;
									}
								}
							}
							return;
						}
					}
				}
			}
			SetVisible(true);
			if (ShouldHideOptionalArrowWhenOffscreen())
			{
				if (m_selectedOptionalArrow != null)
				{
					if (m_selectedOptionalArrow.gameObject.activeSelf)
					{
						UIManager.SetGameObjectActive(m_selectedOptionalArrow, false);
					}
				}
			}
			Vector2 sizeDelta3 = m_canvasRect.sizeDelta;
			float x2 = sizeDelta3.x * 0.5f;
			Vector2 sizeDelta4 = m_canvasRect.sizeDelta;
			Vector2 vector2 = new Vector2(x2, sizeDelta4.y * 0.5f);
			Vector2 vector3 = vector - vector2;
			if (ShouldRotate())
			{
				float num = Mathf.Atan2(vector3.y, vector3.x);
				num *= 57.29578f;
				num -= angleOffset;
				(m_selectedFrame.gameObject.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, num);
			}
			RectTransform rectTransform = base.gameObject.transform as RectTransform;
			Vector2 vector4 = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
			Rect rectA = new Rect(vector2 - vector4 / 2f, vector4);
			float hitTime;
			UIUtils.SweepRectRect(rectA, m_screenRect, vector3, out hitTime);
			rectTransform.anchoredPosition = vector2 + vector3 * hitTime;
			return;
		}
	}

	private void SetVisible(bool visible)
	{
		if (m_visible != visible)
		{
			UIManager.SetGameObjectActive(m_childContainer, visible);
			m_visible = visible;
		}
	}

	protected Vector2 ScreenPosFromWorldPos(Vector3 worldPos)
	{
		Vector3 vector = Camera.main.WorldToViewportPoint(worldPos);
		if (vector.z < 0f)
		{
			vector.y = 0f - vector.y;
			vector.x = 0f - vector.x;
		}
		float x = vector.x;
		Vector2 sizeDelta = m_canvasRect.sizeDelta;
		float x2 = x * sizeDelta.x;
		float y = vector.y;
		Vector2 sizeDelta2 = m_canvasRect.sizeDelta;
		Vector2 result = new Vector2(x2, y * sizeDelta2.y + 50f);
		return result;
	}

	public void MarkGrayoutForUpdate()
	{
		m_forceUpdateGrayout = true;
	}

	public void MarkFrameForUpdate()
	{
		m_forceUpdateFrame = true;
	}
}
