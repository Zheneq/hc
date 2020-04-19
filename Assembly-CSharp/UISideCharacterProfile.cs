using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISideCharacterProfile : MonoBehaviour
{
	public Image m_characterIcon;

	public Image m_currentCharacterArrow;

	public RectTransform m_childAnchor;

	public RectTransform m_hasLockedInIndicator;

	public TextMeshProUGUI m_playerHandleText;

	public TextMeshProUGUI m_hotkeyText;

	public UISideHealthBar m_sideHealthBar;

	public Button m_hitbox;

	public float m_activeActorSideOffset = 30f;

	private ActorData m_actorData;

	private float m_currentAlpha;

	public ActorData GetActor()
	{
		return this.m_actorData;
	}

	private void Awake()
	{
		this.SetCurrentActorArrowActive(false);
		if (this.m_playerHandleText != null)
		{
			this.m_playerHandleText.gameObject.SetActiveIfNeeded(false);
		}
		if (this.m_hitbox != null)
		{
			UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.DoClick));
		}
	}

	public void DoClick(BaseEventData data)
	{
		if (this.m_actorData != null && GameFlowData.Get().SetActiveOwnedActor_FCFS(this.m_actorData))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideCharacterProfile.DoClick(BaseEventData)).MethodHandle;
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.HudLockIn);
		}
	}

	public void Update()
	{
		if (this.m_actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideCharacterProfile.Update()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
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
				ActorTurnSM actorTurnSM = this.m_actorData.\u000E();
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (actorTurnSM != null)
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
					if (activeOwnedActorData != null)
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
						if (this.m_actorData.\u000E() == activeOwnedActorData.\u000E())
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
							bool flag;
							if (activeOwnedActorData == this.m_actorData)
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
								if (actorTurnSM.CurrentState != TurnStateEnum.RESOLVING)
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
									flag = (actorTurnSM.CurrentState != TurnStateEnum.WAITING);
									goto IL_EC;
								}
							}
							flag = false;
							IL_EC:
							bool currentActorArrowActive = flag;
							this.SetCurrentActorArrowActive(currentActorArrowActive);
							goto IL_125;
						}
					}
				}
				if (activeOwnedActorData != null)
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
					if (this.m_actorData.\u000E() != activeOwnedActorData.\u000E())
					{
						this.SetCurrentActorArrowActive(false);
					}
				}
				IL_125:
				UIManager.SetGameObjectActive(this.m_hasLockedInIndicator, actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED, null);
			}
		}
		if (this.m_currentCharacterArrow != null && this.m_currentCharacterArrow.gameObject.activeSelf)
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
			this.SetAlphaForImage(this.m_currentCharacterArrow, 0.75f + 0.25f * Mathf.Cos(6f * Time.time));
		}
	}

	public void SetCurrentActorArrowActive(bool active)
	{
		if (this.m_currentCharacterArrow != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideCharacterProfile.SetCurrentActorArrowActive(bool)).MethodHandle;
			}
			if (this.m_currentCharacterArrow.gameObject.activeSelf != active)
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
				this.m_currentCharacterArrow.gameObject.SetActive(active);
				if (this.m_childAnchor != null)
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
					if (active)
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
						Vector2 offsetMin = new Vector2(this.m_activeActorSideOffset, 0f);
						Vector2 offsetMax = new Vector2(this.m_activeActorSideOffset, 0f);
						this.m_childAnchor.offsetMin = offsetMin;
						this.m_childAnchor.offsetMax = offsetMax;
					}
					else
					{
						this.m_childAnchor.offsetMin = Vector2.zero;
						this.m_childAnchor.offsetMax = Vector2.zero;
					}
				}
			}
		}
		if (this.m_sideHealthBar != null)
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
			this.m_sideHealthBar.SetHealthBarColorForActiveActor(active);
		}
	}

	public void SetAlpha(float newAlpha)
	{
		if (this.m_currentAlpha == Mathf.Clamp(newAlpha, 0f, 1f))
		{
			return;
		}
		this.m_currentAlpha = newAlpha;
		this.m_currentAlpha = Mathf.Clamp(this.m_currentAlpha, 0f, 1f);
		this.SetAlphaForImage(this.m_characterIcon, this.m_currentAlpha);
	}

	private void SetAlphaForImage(Image uiImage, float alpha)
	{
		if (uiImage != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideCharacterProfile.SetAlphaForImage(Image, float)).MethodHandle;
			}
			Color color = uiImage.color;
			color.a = alpha;
			uiImage.color = color;
		}
	}

	public void Setup(ActorData forActor, string hotKeyText, bool showNameLabel)
	{
		if (this.m_actorData == forActor && forActor != null)
		{
			return;
		}
		this.m_actorData = forActor;
		bool flag;
		if (forActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideCharacterProfile.Setup(ActorData, string, bool)).MethodHandle;
			}
			flag = GameFlowData.Get().IsActorDataOwned(forActor);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (forActor != null)
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
			if (this.m_playerHandleText != null)
			{
				this.m_playerHandleText.text = forActor.DisplayName;
				Graphic playerHandleText = this.m_playerHandleText;
				Color color;
				if (flag2)
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
					color = Color.white;
				}
				else
				{
					color = Color.cyan;
				}
				playerHandleText.color = color;
				this.m_playerHandleText.gameObject.SetActiveIfNeeded(showNameLabel);
			}
		}
		this.m_hotkeyText.text = hotKeyText;
		if (this.m_hitbox != null)
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
			this.m_hitbox.gameObject.SetActiveIfNeeded(flag2);
		}
		if (this.m_characterIcon != null)
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
			if (this.m_actorData != null)
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
				this.m_characterIcon.sprite = this.m_actorData.\u000E();
			}
		}
		this.SetAlpha(1f);
		if (this.m_sideHealthBar != null)
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
			this.m_sideHealthBar.SetActor(forActor);
		}
	}
}
