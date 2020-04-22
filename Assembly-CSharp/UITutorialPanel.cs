using Fabric;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialPanel : MonoBehaviour
{
	public enum SubtitleLocation
	{
		BottomLeft,
		BottomRight
	}

	public class QueuedTutorialElement
	{
		public int Index;

		public string Subtitle;

		public SubtitleLocation DisplayLocation;

		public float TimeToDisplay;

		public CharacterType CharacterType;

		public float DisplayStartTime;

		public string AudioEvent;

		public Action Action;
	}

	[Serializable]
	public struct SubtitleDisplayObjects
	{
		public RectTransform Container;

		public TextMeshProUGUI ChatText;

		public TextMeshProUGUI CharacterText;

		public Image CharacterImage;

		public GameObject CharacterImageContainer;
	}

	public GameObject m_tutorialRightClickPanel;

	public TextMeshProUGUI m_tutorialRightClickText;

	public GameObject m_tutorialShiftRightClickPanel;

	public TextMeshProUGUI m_tutorialShiftRightClickText;

	public GameObject m_tutorialLeftClickPanel;

	public TextMeshProUGUI m_tutorialLeftClickText;

	public GameObject m_tutorialTextPanel;

	public TextMeshProUGUI m_tutorialText;

	public GameObject m_tutorialTextPanel2;

	public TextMeshProUGUI m_tutorialText2;

	public GameObject m_tutorialTextPanel3;

	public TextMeshProUGUI m_tutorialText3;

	public GameObject m_tutorialCameraRotationPanel;

	public TextMeshProUGUI m_tutorialCameraRotationText;

	public GameObject m_tutorialCameraMovementPanel;

	public TextMeshProUGUI m_tutorialCameraMovementText;

	public GameObject m_tutorialErrorPanel;

	public TextMeshProUGUI m_tutorialErrorText;

	public GameObject m_tutorialPassedStamp;

	public SubtitleDisplayObjects BottomLeftDisplay;

	public SubtitleDisplayObjects BottomRightDisplay;

	private static UITutorialPanel s_instance;

	private bool m_tutorialCleanUpNeeded;

	private List<QueuedTutorialElement> QueuedTutorialElements = new List<QueuedTutorialElement>();

	private int m_nextIndex;

	public static UITutorialPanel Get()
	{
		return s_instance;
	}

	public int QueueDialogue(string subtitleText, string audioEvent, float timeToDisplay, CharacterType characterType)
	{
		int num;
		if (characterType == CharacterType.Scoundrel)
		{
			num = 0;
		}
		else
		{
			num = 1;
		}
		SubtitleLocation displayLocation = (SubtitleLocation)num;
		if (subtitleText.IsNullOrEmpty())
		{
			if (audioEvent.IsNullOrEmpty())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return -1;
					}
				}
			}
		}
		if (audioEvent.IsNullOrEmpty())
		{
			if (timeToDisplay == 0f)
			{
				timeToDisplay = 3f;
			}
		}
		QueuedTutorialElement queuedTutorialElement = new QueuedTutorialElement();
		queuedTutorialElement.Index = m_nextIndex++;
		string subtitle;
		if (subtitleText.Contains("@"))
		{
			subtitle = StringUtil.TR(subtitleText);
		}
		else
		{
			subtitle = subtitleText;
		}
		queuedTutorialElement.Subtitle = subtitle;
		queuedTutorialElement.AudioEvent = audioEvent;
		queuedTutorialElement.TimeToDisplay = timeToDisplay;
		queuedTutorialElement.CharacterType = characterType;
		queuedTutorialElement.DisplayStartTime = -1f;
		queuedTutorialElement.DisplayLocation = displayLocation;
		QueuedTutorialElements.Add(queuedTutorialElement);
		return queuedTutorialElement.Index;
	}

	public void QueueAction(Action handler)
	{
		QueuedTutorialElement queuedTutorialElement = new QueuedTutorialElement();
		queuedTutorialElement.Index = m_nextIndex++;
		queuedTutorialElement.Action = handler;
		queuedTutorialElement.DisplayStartTime = -1f;
		QueuedTutorialElements.Add(queuedTutorialElement);
	}

	public bool HasQueuedElements()
	{
		return QueuedTutorialElements.Count > 0;
	}

	public void ClearAll()
	{
		QueuedTutorialElements.Clear();
		RebuildSubtitles();
		if (!(SinglePlayerManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			SinglePlayerManager.Get().OnTutorialQueueEmpty();
			return;
		}
	}

	public void HideTutorialPassedStamp()
	{
		UIManager.SetGameObjectActive(m_tutorialPassedStamp, false);
	}

	public void ShowTutorialPassedStamp()
	{
		UIManager.SetGameObjectActive(m_tutorialPassedStamp, true);
	}

	public void RemoveQueuedElement(int index)
	{
		if (QueuedTutorialElements.Count <= 0 || QueuedTutorialElements[0] == null)
		{
			return;
		}
		while (true)
		{
			if (QueuedTutorialElements[0].Index != index)
			{
				return;
			}
			while (true)
			{
				QueuedTutorialElements.RemoveAt(0);
				UIManager.SetGameObjectActive(BottomLeftDisplay.Container, false);
				UIManager.SetGameObjectActive(BottomRightDisplay.Container, false);
				RebuildSubtitles();
				if (QueuedTutorialElements.Count != 0)
				{
					return;
				}
				while (true)
				{
					if (SinglePlayerManager.Get() != null)
					{
						while (true)
						{
							SinglePlayerManager.Get().OnTutorialQueueEmpty();
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void RebuildSubtitles()
	{
		if (QueuedTutorialElements.Count == 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(BottomLeftDisplay.Container, false);
					UIManager.SetGameObjectActive(BottomRightDisplay.Container, false);
					return;
				}
			}
		}
		QueuedTutorialElement queuedElement = QueuedTutorialElements[0];
		if (queuedElement.DisplayStartTime >= 0f && queuedElement.TimeToDisplay > 0f && Time.realtimeSinceStartup - queuedElement.DisplayStartTime >= queuedElement.TimeToDisplay)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					RemoveQueuedElement(queuedElement.Index);
					return;
				}
			}
		}
		if (queuedElement.DisplayStartTime != -1f)
		{
			return;
		}
		while (true)
		{
			if (Time.timeScale > 1f)
			{
				queuedElement.AudioEvent = null;
				if (queuedElement.TimeToDisplay == 0f)
				{
					queuedElement.TimeToDisplay = 3f / Time.timeScale;
				}
				else
				{
					queuedElement.TimeToDisplay /= Time.timeScale;
				}
			}
			SubtitleDisplayObjects subtitleDisplayObjects;
			if (queuedElement.DisplayLocation == SubtitleLocation.BottomLeft)
			{
				subtitleDisplayObjects = BottomLeftDisplay;
			}
			else
			{
				subtitleDisplayObjects = BottomRightDisplay;
			}
			SubtitleDisplayObjects subtitleDisplayObjects2 = subtitleDisplayObjects;
			if (!queuedElement.Subtitle.IsNullOrEmpty())
			{
				UIManager.SetGameObjectActive(subtitleDisplayObjects2.Container, true);
				subtitleDisplayObjects2.CharacterText.text = GameWideData.Get().GetCharacterDisplayName(queuedElement.CharacterType);
				subtitleDisplayObjects2.CharacterImage.sprite = GameWideData.Get().GetCharacterResourceLink(queuedElement.CharacterType).GetCharacterSelectIcon();
				subtitleDisplayObjects2.ChatText.text = queuedElement.Subtitle;
				int num;
				if (HUD_UI.Get().MainHUDElementsVisible())
				{
					num = (UIGameOverScreen.Get().IsVisible ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				bool doActive = (byte)num != 0;
				UIManager.SetGameObjectActive(BottomLeftDisplay.CharacterImageContainer, doActive);
			}
			else
			{
				UIManager.SetGameObjectActive(subtitleDisplayObjects2.Container, false);
			}
			queuedElement.DisplayStartTime = Time.realtimeSinceStartup;
			if (!queuedElement.AudioEvent.IsNullOrEmpty())
			{
				OnEventNotify notifyCallback = delegate(EventNotificationType eventType, string eventName, object info, GameObject gameObject)
				{
					if (queuedElement.TimeToDisplay == 0f && eventType == EventNotificationType.OnFinished)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								queuedElement.TimeToDisplay = Time.realtimeSinceStartup - queuedElement.DisplayStartTime + 0.25f;
								return;
							}
						}
					}
				};
				if (!AudioManager.PostEventNotify(queuedElement.AudioEvent, notifyCallback))
				{
					RemoveQueuedElement(queuedElement.Index);
				}
			}
			if (queuedElement.Action != null)
			{
				while (true)
				{
					queuedElement.Action();
					RemoveQueuedElement(queuedElement.Index);
					return;
				}
			}
			return;
		}
	}

	private void Awake()
	{
		UIManager.SetGameObjectActive(BottomLeftDisplay.Container, false);
		UIManager.SetGameObjectActive(BottomRightDisplay.Container, false);
		s_instance = this;
	}

	public void Update()
	{
		RebuildSubtitles();
	}

	public void LateUpdate()
	{
		if (SinglePlayerManager.Get() != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					SinglePlayerManager.Get().UpdateRightAndLeftClickElements(m_tutorialRightClickPanel, m_tutorialRightClickText, m_tutorialLeftClickPanel, m_tutorialLeftClickText, m_tutorialShiftRightClickPanel, m_tutorialShiftRightClickText);
					SinglePlayerManager.Get().UpdateTutorialTextElements(m_tutorialTextPanel, m_tutorialText, m_tutorialTextPanel2, m_tutorialText2, m_tutorialTextPanel3, m_tutorialText3, m_tutorialCameraMovementPanel, m_tutorialCameraMovementText, m_tutorialCameraRotationPanel, m_tutorialCameraRotationText);
					SinglePlayerManager.Get().UpdateTutorialError(m_tutorialErrorPanel, m_tutorialErrorText);
					m_tutorialCleanUpNeeded = true;
					return;
				}
			}
		}
		if (!m_tutorialCleanUpNeeded)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_tutorialLeftClickPanel, false);
			UIManager.SetGameObjectActive(m_tutorialRightClickPanel, false);
			UIManager.SetGameObjectActive(m_tutorialTextPanel, false);
			UIManager.SetGameObjectActive(m_tutorialTextPanel2, false);
			UIManager.SetGameObjectActive(m_tutorialTextPanel3, false);
			UIManager.SetGameObjectActive(m_tutorialCameraMovementPanel, false);
			UIManager.SetGameObjectActive(m_tutorialCameraRotationPanel, false);
			m_tutorialCleanUpNeeded = false;
			return;
		}
	}
}
