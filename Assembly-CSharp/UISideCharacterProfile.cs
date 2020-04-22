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
		return m_actorData;
	}

	private void Awake()
	{
		SetCurrentActorArrowActive(false);
		if (m_playerHandleText != null)
		{
			m_playerHandleText.gameObject.SetActiveIfNeeded(false);
		}
		if (m_hitbox != null)
		{
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerClick, DoClick);
		}
	}

	public void DoClick(BaseEventData data)
	{
		if (!(m_actorData != null) || !GameFlowData.Get().SetActiveOwnedActor_FCFS(m_actorData))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.HudLockIn);
			return;
		}
	}

	public void Update()
	{
		ActorTurnSM actorTurnSM;
		int num;
		if (m_actorData != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get() != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				actorTurnSM = m_actorData.GetActorTurnSM();
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (actorTurnSM != null)
				{
					while (true)
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
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_actorData.GetTeam() == activeOwnedActorData.GetTeam())
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (activeOwnedActorData == m_actorData)
							{
								while (true)
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
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									num = ((actorTurnSM.CurrentState != TurnStateEnum.WAITING) ? 1 : 0);
									goto IL_00ec;
								}
							}
							num = 0;
							goto IL_00ec;
						}
					}
				}
				if (activeOwnedActorData != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_actorData.GetTeam() != activeOwnedActorData.GetTeam())
					{
						SetCurrentActorArrowActive(false);
					}
				}
				goto IL_0125;
			}
		}
		goto IL_013a;
		IL_00ec:
		bool currentActorArrowActive = (byte)num != 0;
		SetCurrentActorArrowActive(currentActorArrowActive);
		goto IL_0125;
		IL_0125:
		UIManager.SetGameObjectActive(m_hasLockedInIndicator, actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED);
		goto IL_013a;
		IL_013a:
		if (!(m_currentCharacterArrow != null) || !m_currentCharacterArrow.gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			SetAlphaForImage(m_currentCharacterArrow, 0.75f + 0.25f * Mathf.Cos(6f * Time.time));
			return;
		}
	}

	public void SetCurrentActorArrowActive(bool active)
	{
		if (m_currentCharacterArrow != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_currentCharacterArrow.gameObject.activeSelf != active)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_currentCharacterArrow.gameObject.SetActive(active);
				if (m_childAnchor != null)
				{
					while (true)
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
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector2 offsetMin = new Vector2(m_activeActorSideOffset, 0f);
						Vector2 offsetMax = new Vector2(m_activeActorSideOffset, 0f);
						m_childAnchor.offsetMin = offsetMin;
						m_childAnchor.offsetMax = offsetMax;
					}
					else
					{
						m_childAnchor.offsetMin = Vector2.zero;
						m_childAnchor.offsetMax = Vector2.zero;
					}
				}
			}
		}
		if (!(m_sideHealthBar != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			m_sideHealthBar.SetHealthBarColorForActiveActor(active);
			return;
		}
	}

	public void SetAlpha(float newAlpha)
	{
		if (m_currentAlpha != Mathf.Clamp(newAlpha, 0f, 1f))
		{
			m_currentAlpha = newAlpha;
			m_currentAlpha = Mathf.Clamp(m_currentAlpha, 0f, 1f);
			SetAlphaForImage(m_characterIcon, m_currentAlpha);
		}
	}

	private void SetAlphaForImage(Image uiImage, float alpha)
	{
		if (!(uiImage != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Color color = uiImage.color;
			color.a = alpha;
			uiImage.color = color;
			return;
		}
	}

	public void Setup(ActorData forActor, string hotKeyText, bool showNameLabel)
	{
		if (m_actorData == forActor && forActor != null)
		{
			return;
		}
		m_actorData = forActor;
		int num;
		if (forActor != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (GameFlowData.Get().IsActorDataOwned(forActor) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (forActor != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_playerHandleText != null)
			{
				m_playerHandleText.text = forActor.DisplayName;
				TextMeshProUGUI playerHandleText = m_playerHandleText;
				Color color;
				if (flag)
				{
					while (true)
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
				m_playerHandleText.gameObject.SetActiveIfNeeded(showNameLabel);
			}
		}
		m_hotkeyText.text = hotKeyText;
		if (m_hitbox != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			m_hitbox.gameObject.SetActiveIfNeeded(flag);
		}
		if (m_characterIcon != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_actorData != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				m_characterIcon.sprite = m_actorData.GetAliveHUDIcon();
			}
		}
		SetAlpha(1f);
		if (!(m_sideHealthBar != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			m_sideHealthBar.SetActor(forActor);
			return;
		}
	}
}
