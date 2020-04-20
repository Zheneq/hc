using System;
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
		this.m_characterIcon.sprite = actorData.GetAliveHUDIcon();
	}

	public ActorData GetAttachedActor()
	{
		return this.m_attachedToActor;
	}

	public ControlPoint GetAttachedControlPoint()
	{
		return this.m_attachedToControlPoint;
	}

	public CTF_Flag GetAttachedFlag()
	{
		return this.m_attachedToFlag;
	}

	public BoardRegion GetAttachedRegion()
	{
		return this.m_attachedToBoardRegion;
	}

	public UIWorldPing GetAttachedPing()
	{
		return this.m_attachedToPing;
	}

	public void Start()
	{
		if (this.m_grayCharacterIcon != null)
		{
			UIManager.SetGameObjectActive(this.m_grayCharacterIcon, false, null);
		}
		if (this.m_grayFrame != null)
		{
			UIManager.SetGameObjectActive(this.m_grayFrame, false, null);
		}
		if (this.m_grayOptionalArrow != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.Start()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_grayOptionalArrow, false, null);
		}
	}

	public void Setup(ActorData actorData, UIOffscreenIndicatorPanel panel)
	{
		this.m_initialized = false;
		this.m_attachedToActor = actorData;
		this.m_attachedToPing = null;
		this.SetupCharacterIcons(actorData);
		if (actorData == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.Setup(ActorData, UIOffscreenIndicatorPanel)).MethodHandle;
			}
			Log.Error("Offscreen Indicator set up with no actor provided", new object[0]);
			return;
		}
		this.Setup_Base(panel);
	}

	public void Setup(ControlPoint controlPoint, UIOffscreenIndicatorPanel panel)
	{
		this.m_initialized = false;
		this.m_attachedToActor = null;
		this.m_attachedToControlPoint = controlPoint;
		this.m_characterIcon.sprite = controlPoint.m_icon;
		this.m_attachedToPing = null;
		this.Setup_Base(panel);
	}

	public void Setup(CTF_Flag flag, UIOffscreenIndicatorPanel panel)
	{
		this.m_initialized = false;
		this.m_attachedToActor = null;
		this.m_attachedToControlPoint = null;
		this.m_attachedToFlag = flag;
		this.m_attachedToPing = null;
		this.Setup_Base(panel);
		UIManager.SetGameObjectActive(this.m_briefcaseIcon, true, null);
		UIManager.SetGameObjectActive(this.m_characterIcon, false, null);
	}

	public void Setup(BoardRegion boardRegion, UIOffscreenIndicatorPanel panel, Team teamRegion, bool isFlagTurnIn)
	{
		this.m_initialized = false;
		this.m_attachedToActor = null;
		this.m_attachedToControlPoint = null;
		this.m_attachedToFlag = null;
		this.m_attachedToBoardRegion = boardRegion;
		this.m_regionTeam = teamRegion;
		this.m_attachedToPing = null;
		this.Setup_Base(panel);
		if (isFlagTurnIn)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.Setup(BoardRegion, UIOffscreenIndicatorPanel, Team, bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_dropzoneIcon, true, null);
			UIManager.SetGameObjectActive(this.m_characterIcon, false, null);
		}
		else
		{
			this.m_characterIcon.sprite = boardRegion.GetTurnInRegionIcon();
			this.m_characterIcon.transform.localScale = Vector3.one;
		}
	}

	public void Setup(UIWorldPing ping, ActorController.PingType pingtype, ActorData pingerActorData, UIOffscreenIndicatorPanel panel)
	{
		this.m_initialized = false;
		this.m_attachedToActor = null;
		this.m_attachedToControlPoint = null;
		this.m_attachedToFlag = null;
		this.m_attachedToPing = ping;
		this.Setup_Base(panel);
		UIManager.SetGameObjectActive(this.m_characterIcon, false, null);
		UIManager.SetGameObjectActive(this.m_pingAssistIcon, pingtype == ActorController.PingType.Assist, null);
		UIManager.SetGameObjectActive(this.m_pingDefendIcon, pingtype == ActorController.PingType.Defend, null);
		UIManager.SetGameObjectActive(this.m_pingDefaultIcon, pingtype == ActorController.PingType.Default, null);
		UIManager.SetGameObjectActive(this.m_pingEnemyIcon, pingtype == ActorController.PingType.Enemy, null);
		UIManager.SetGameObjectActive(this.m_pingMoveIcon, pingtype == ActorController.PingType.Move, null);
		UIManager.SetGameObjectActive(this.m_pingGroupContainer, pingerActorData != null, null);
		if (pingerActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.Setup(UIWorldPing, ActorController.PingType, ActorData, UIOffscreenIndicatorPanel)).MethodHandle;
			}
			this.m_pingCharacterIcon.sprite = pingerActorData.GetAliveHUDIcon();
		}
	}

	private void CheckForFlagHolder()
	{
		if (CaptureTheFlag.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.CheckForFlagHolder()).MethodHandle;
			}
			if (this.m_attachedToActor != null && this.m_briefcaseIcon != null)
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
				bool flag = CaptureTheFlag.GetMainFlagCarrier_Client() == this.m_attachedToActor;
				UIManager.SetGameObjectActive(this.m_briefcaseIcon, flag, null);
				UIManager.SetGameObjectActive(this.m_characterIcon, !flag, null);
			}
		}
	}

	private void Setup_Base(UIOffscreenIndicatorPanel panel)
	{
		this.m_parentPanel = panel;
		if (this.m_parentPanel == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.Setup_Base(UIOffscreenIndicatorPanel)).MethodHandle;
			}
			Log.Error("Offscreen Indicator set up with no parent panel.", new object[0]);
			return;
		}
		if (this.m_canvas == null)
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
			this.m_canvas = HUD_UI.Get().m_mainCanvas;
			if (this.m_canvas == null)
			{
				Log.Error("No UI canvas found in offscreen indicator parent", new object[0]);
				return;
			}
		}
		this.m_canvasRect = (this.m_canvas.transform as RectTransform);
		this.m_visible = this.CalculateVisibility();
		UIManager.SetGameObjectActive(this.m_childContainer, this.m_visible, null);
		if (this.m_briefcaseIcon != null)
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
			UIManager.SetGameObjectActive(this.m_briefcaseIcon, false, null);
		}
		if (this.m_dropzoneIcon != null)
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
			UIManager.SetGameObjectActive(this.m_dropzoneIcon, false, null);
		}
		if (this.m_pingGroupContainer != null)
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
			UIManager.SetGameObjectActive(this.m_pingAssistIcon, false, null);
			UIManager.SetGameObjectActive(this.m_pingDefaultIcon, false, null);
			UIManager.SetGameObjectActive(this.m_pingEnemyIcon, false, null);
			UIManager.SetGameObjectActive(this.m_pingMoveIcon, false, null);
			UIManager.SetGameObjectActive(this.m_pingGroupContainer, false, null);
		}
		UIManager.SetGameObjectActive(this.m_characterIcon, true, null);
		this.m_initialized = true;
	}

	private void UpdateFrame()
	{
		if (!(this.m_briefcaseIcon == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.UpdateFrame()).MethodHandle;
			}
			if (this.m_briefcaseIcon.gameObject.activeSelf)
			{
				goto IL_BB;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!(this.m_dropzoneIcon == null))
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
			if (this.m_dropzoneIcon.gameObject.activeSelf)
			{
				goto IL_BB;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!(this.m_pingGroupContainer == null))
		{
			if (this.m_pingGroupContainer.gameObject.activeSelf)
			{
				goto IL_BB;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		UIManager.SetGameObjectActive(this.m_characterIcon, true, null);
		IL_BB:
		this.m_selectedCharacterIcon = this.m_characterIcon;
		this.m_selectedOptionalArrow = this.m_optionalArrow;
		if (this.m_attachedToActor == null)
		{
			this.m_selectedFrame = this.m_objectiveFrame;
			if (this.m_attachedToBoardRegion != null)
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
				if (this.m_regionTeam != Team.Invalid)
				{
					UIManager.SetGameObjectActive(this.m_friendlyFrame, this.m_regionTeam == Team.TeamA, null);
					UIManager.SetGameObjectActive(this.m_friendlyBackground, this.m_regionTeam == Team.TeamA, null);
					UIManager.SetGameObjectActive(this.m_enemyFrame, this.m_regionTeam == Team.TeamB, null);
					UIManager.SetGameObjectActive(this.m_enemyBackground, this.m_regionTeam == Team.TeamB, null);
					UIManager.SetGameObjectActive(this.m_objectiveFrame, false, null);
					UIManager.SetGameObjectActive(this.m_objectiveBackground, false, null);
					if (this.m_regionTeam == Team.TeamA)
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
						this.m_selectedFrame = this.m_friendlyFrame;
					}
					else if (this.m_regionTeam == Team.TeamB)
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
						this.m_selectedFrame = this.m_enemyFrame;
					}
					goto IL_279;
				}
			}
			if (this.m_attachedToPing != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_friendlyFrame, true, null);
				UIManager.SetGameObjectActive(this.m_friendlyBackground, true, null);
				UIManager.SetGameObjectActive(this.m_enemyFrame, false, null);
				UIManager.SetGameObjectActive(this.m_enemyBackground, false, null);
				UIManager.SetGameObjectActive(this.m_objectiveFrame, false, null);
				UIManager.SetGameObjectActive(this.m_objectiveBackground, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_friendlyFrame, false, null);
				UIManager.SetGameObjectActive(this.m_friendlyBackground, false, null);
				UIManager.SetGameObjectActive(this.m_enemyFrame, false, null);
				UIManager.SetGameObjectActive(this.m_enemyBackground, false, null);
				UIManager.SetGameObjectActive(this.m_objectiveFrame, true, null);
				UIManager.SetGameObjectActive(this.m_objectiveBackground, true, null);
			}
			IL_279:;
		}
		else
		{
			GameFlowData gameFlowData = GameFlowData.Get();
			Team team = Team.TeamA;
			bool flag = true;
			if (gameFlowData != null && gameFlowData.LocalPlayerData != null)
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
				if (gameFlowData.activeOwnedActorData != null)
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
					team = gameFlowData.activeOwnedActorData.GetTeam();
				}
				flag = (team == this.m_attachedToActor.GetTeam());
			}
			if (team == this.m_curTeam)
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
				if (!this.m_forceUpdateFrame)
				{
					goto IL_394;
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
			this.m_curTeam = team;
			UIManager.SetGameObjectActive(this.m_friendlyFrame, flag, null);
			UIManager.SetGameObjectActive(this.m_friendlyBackground, flag, null);
			UIManager.SetGameObjectActive(this.m_enemyFrame, !flag, null);
			UIManager.SetGameObjectActive(this.m_enemyBackground, !flag, null);
			UIManager.SetGameObjectActive(this.m_objectiveFrame, false, null);
			UIManager.SetGameObjectActive(this.m_objectiveBackground, false, null);
			if (flag)
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
				this.m_selectedFrame = this.m_friendlyFrame;
			}
			else
			{
				this.m_selectedFrame = this.m_enemyFrame;
			}
		}
		IL_394:
		this.m_forceUpdateFrame = false;
	}

	private void UpdateGrayedOut()
	{
		if (this.ShouldGrayOutIndicator())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.UpdateGrayedOut()).MethodHandle;
			}
			if (this.m_isGrayedOut)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_forceUpdateGrayout)
				{
					goto IL_D5;
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_isGrayedOut = true;
			UIManager.SetGameObjectActive(this.m_selectedFrame, false, null);
			UIManager.SetGameObjectActive(this.m_selectedCharacterIcon, false, null);
			UIManager.SetGameObjectActive(this.m_selectedOptionalArrow, false, null);
			this.m_selectedFrame = this.m_grayFrame;
			this.m_selectedCharacterIcon = this.m_grayCharacterIcon;
			this.m_selectedOptionalArrow = this.m_grayOptionalArrow;
			UIManager.SetGameObjectActive(this.m_selectedFrame, true, null);
			UIManager.SetGameObjectActive(this.m_selectedCharacterIcon, true, null);
			if (this.m_selectedOptionalArrow != null)
			{
				UIManager.SetGameObjectActive(this.m_selectedOptionalArrow, true, null);
			}
			goto IL_14E;
		}
		IL_D5:
		if (!this.ShouldGrayOutIndicator())
		{
			if (!this.m_isGrayedOut)
			{
				if (!this.m_forceUpdateGrayout)
				{
					goto IL_14E;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_isGrayedOut = false;
			UIManager.SetGameObjectActive(this.m_selectedFrame, false, null);
			UIManager.SetGameObjectActive(this.m_selectedCharacterIcon, false, null);
			if (this.m_selectedOptionalArrow != null)
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
				UIManager.SetGameObjectActive(this.m_selectedOptionalArrow, false, null);
			}
			this.m_forceUpdateFrame = true;
			this.UpdateFrame();
		}
		IL_14E:
		this.m_forceUpdateGrayout = false;
	}

	private void LateUpdate()
	{
		if (!(Camera.main == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.LateUpdate()).MethodHandle;
			}
			if (this.m_initialized)
			{
				if (this.m_forceUpdateFrame)
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
					this.UpdateFrame();
				}
				if (!this.CalculateVisibility())
				{
					this.SetVisible(false);
					return;
				}
				this.CheckForFlagHolder();
				this.UpdateGrayedOut();
				Vector2 vector = this.CalculateScreenPos();
				this.m_screenRect = new Rect(this.m_parentPanel.borderLeft, this.m_parentPanel.borderBottom, this.m_canvasRect.sizeDelta.x - (this.m_parentPanel.borderRight + this.m_parentPanel.borderLeft), this.m_canvasRect.sizeDelta.y - (this.m_parentPanel.borderBottom + this.m_parentPanel.borderTop));
				if (!this.IsVisibleWhenOnScreen())
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
					if (this.m_screenRect.Contains(vector))
					{
						this.SetVisible(false);
						return;
					}
				}
				if (this.IsVisibleWhenOnScreen())
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
					if (this.m_screenRect.Contains(vector))
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
						this.SetVisible(true);
						(base.gameObject.transform as RectTransform).anchoredPosition = vector;
						if (this.ShouldRotate())
						{
							(this.m_selectedFrame.gameObject.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, -135f);
						}
						if (this.m_selectedOptionalArrow != null)
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
							if (!this.m_selectedOptionalArrow.gameObject.activeSelf)
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
								UIManager.SetGameObjectActive(this.m_selectedOptionalArrow, true, null);
							}
						}
						return;
					}
				}
				this.SetVisible(true);
				if (this.ShouldHideOptionalArrowWhenOffscreen())
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
					if (this.m_selectedOptionalArrow != null)
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
						if (this.m_selectedOptionalArrow.gameObject.activeSelf)
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
							UIManager.SetGameObjectActive(this.m_selectedOptionalArrow, false, null);
						}
					}
				}
				Vector2 vector2 = new Vector2(this.m_canvasRect.sizeDelta.x * 0.5f, this.m_canvasRect.sizeDelta.y * 0.5f);
				Vector2 vector3 = vector - vector2;
				if (this.ShouldRotate())
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
					float num = Mathf.Atan2(vector3.y, vector3.x);
					num *= 57.29578f;
					num -= this.angleOffset;
					(this.m_selectedFrame.gameObject.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, num);
				}
				RectTransform rectTransform = base.gameObject.transform as RectTransform;
				Vector2 vector4 = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
				Rect rectA = new Rect(vector2 - vector4 / 2f, vector4);
				float d;
				UIUtils.SweepRectRect(rectA, this.m_screenRect, vector3, out d);
				rectTransform.anchoredPosition = vector2 + vector3 * d;
				return;
			}
		}
	}

	private void SetVisible(bool visible)
	{
		if (this.m_visible != visible)
		{
			UIManager.SetGameObjectActive(this.m_childContainer, visible, null);
			this.m_visible = visible;
		}
	}

	protected Vector2 ScreenPosFromWorldPos(Vector3 worldPos)
	{
		Vector3 vector = Camera.main.WorldToViewportPoint(worldPos);
		if (vector.z < 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseIndicator.ScreenPosFromWorldPos(Vector3)).MethodHandle;
			}
			vector.y = -vector.y;
			vector.x = -vector.x;
		}
		Vector2 result = new Vector2(vector.x * this.m_canvasRect.sizeDelta.x, vector.y * this.m_canvasRect.sizeDelta.y + 50f);
		return result;
	}

	public void MarkGrayoutForUpdate()
	{
		this.m_forceUpdateGrayout = true;
	}

	public void MarkFrameForUpdate()
	{
		this.m_forceUpdateFrame = true;
	}
}
