using System;
using System.Collections.Generic;
using Fabric;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialPanel : MonoBehaviour
{
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

	public UITutorialPanel.SubtitleDisplayObjects BottomLeftDisplay;

	public UITutorialPanel.SubtitleDisplayObjects BottomRightDisplay;

	private static UITutorialPanel s_instance;

	private bool m_tutorialCleanUpNeeded;

	private List<UITutorialPanel.QueuedTutorialElement> QueuedTutorialElements = new List<UITutorialPanel.QueuedTutorialElement>();

	private int m_nextIndex;

	public static UITutorialPanel Get()
	{
		return UITutorialPanel.s_instance;
	}

	public int QueueDialogue(string subtitleText, string audioEvent, float timeToDisplay, CharacterType characterType)
	{
		UITutorialPanel.SubtitleLocation subtitleLocation;
		if (characterType == CharacterType.Scoundrel)
		{
			subtitleLocation = UITutorialPanel.SubtitleLocation.BottomLeft;
		}
		else
		{
			subtitleLocation = UITutorialPanel.SubtitleLocation.BottomRight;
		}
		UITutorialPanel.SubtitleLocation displayLocation = subtitleLocation;
		if (subtitleText.IsNullOrEmpty())
		{
			if (audioEvent.IsNullOrEmpty())
			{
				return -1;
			}
		}
		if (audioEvent.IsNullOrEmpty())
		{
			if (timeToDisplay == 0f)
			{
				timeToDisplay = 3f;
			}
		}
		UITutorialPanel.QueuedTutorialElement queuedTutorialElement = new UITutorialPanel.QueuedTutorialElement();
		queuedTutorialElement.Index = this.m_nextIndex++;
		UITutorialPanel.QueuedTutorialElement queuedTutorialElement2 = queuedTutorialElement;
		string subtitle;
		if (subtitleText.Contains("@"))
		{
			subtitle = StringUtil.TR(subtitleText);
		}
		else
		{
			subtitle = subtitleText;
		}
		queuedTutorialElement2.Subtitle = subtitle;
		queuedTutorialElement.AudioEvent = audioEvent;
		queuedTutorialElement.TimeToDisplay = timeToDisplay;
		queuedTutorialElement.CharacterType = characterType;
		queuedTutorialElement.DisplayStartTime = -1f;
		queuedTutorialElement.DisplayLocation = displayLocation;
		this.QueuedTutorialElements.Add(queuedTutorialElement);
		return queuedTutorialElement.Index;
	}

	public void QueueAction(Action handler)
	{
		UITutorialPanel.QueuedTutorialElement queuedTutorialElement = new UITutorialPanel.QueuedTutorialElement();
		queuedTutorialElement.Index = this.m_nextIndex++;
		queuedTutorialElement.Action = handler;
		queuedTutorialElement.DisplayStartTime = -1f;
		this.QueuedTutorialElements.Add(queuedTutorialElement);
	}

	public bool HasQueuedElements()
	{
		return this.QueuedTutorialElements.Count > 0;
	}

	public void ClearAll()
	{
		this.QueuedTutorialElements.Clear();
		this.RebuildSubtitles();
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().OnTutorialQueueEmpty();
		}
	}

	public void HideTutorialPassedStamp()
	{
		UIManager.SetGameObjectActive(this.m_tutorialPassedStamp, false, null);
	}

	public void ShowTutorialPassedStamp()
	{
		UIManager.SetGameObjectActive(this.m_tutorialPassedStamp, true, null);
	}

	public void RemoveQueuedElement(int index)
	{
		if (this.QueuedTutorialElements.Count > 0 && this.QueuedTutorialElements[0] != null)
		{
			if (this.QueuedTutorialElements[0].Index == index)
			{
				this.QueuedTutorialElements.RemoveAt(0);
				UIManager.SetGameObjectActive(this.BottomLeftDisplay.Container, false, null);
				UIManager.SetGameObjectActive(this.BottomRightDisplay.Container, false, null);
				this.RebuildSubtitles();
				if (this.QueuedTutorialElements.Count == 0)
				{
					if (SinglePlayerManager.Get() != null)
					{
						SinglePlayerManager.Get().OnTutorialQueueEmpty();
					}
				}
			}
		}
	}

	private void RebuildSubtitles()
	{
		if (this.QueuedTutorialElements.Count == 0)
		{
			UIManager.SetGameObjectActive(this.BottomLeftDisplay.Container, false, null);
			UIManager.SetGameObjectActive(this.BottomRightDisplay.Container, false, null);
			return;
		}
		UITutorialPanel.QueuedTutorialElement queuedElement = this.QueuedTutorialElements[0];
		if (queuedElement.DisplayStartTime >= 0f && queuedElement.TimeToDisplay > 0f && Time.realtimeSinceStartup - queuedElement.DisplayStartTime >= queuedElement.TimeToDisplay)
		{
			this.RemoveQueuedElement(queuedElement.Index);
			return;
		}
		if (queuedElement.DisplayStartTime == -1f)
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
			UITutorialPanel.SubtitleDisplayObjects subtitleDisplayObjects;
			if (queuedElement.DisplayLocation == UITutorialPanel.SubtitleLocation.BottomLeft)
			{
				subtitleDisplayObjects = this.BottomLeftDisplay;
			}
			else
			{
				subtitleDisplayObjects = this.BottomRightDisplay;
			}
			UITutorialPanel.SubtitleDisplayObjects subtitleDisplayObjects2 = subtitleDisplayObjects;
			if (!queuedElement.Subtitle.IsNullOrEmpty())
			{
				UIManager.SetGameObjectActive(subtitleDisplayObjects2.Container, true, null);
				subtitleDisplayObjects2.CharacterText.text = GameWideData.Get().GetCharacterDisplayName(queuedElement.CharacterType);
				subtitleDisplayObjects2.CharacterImage.sprite = GameWideData.Get().GetCharacterResourceLink(queuedElement.CharacterType).GetCharacterSelectIcon();
				subtitleDisplayObjects2.ChatText.text = queuedElement.Subtitle;
				bool flag;
				if (HUD_UI.Get().MainHUDElementsVisible())
				{
					flag = UIGameOverScreen.Get().IsVisible;
				}
				else
				{
					flag = true;
				}
				bool doActive = flag;
				UIManager.SetGameObjectActive(this.BottomLeftDisplay.CharacterImageContainer, doActive, null);
			}
			else
			{
				UIManager.SetGameObjectActive(subtitleDisplayObjects2.Container, false, null);
			}
			queuedElement.DisplayStartTime = Time.realtimeSinceStartup;
			if (!queuedElement.AudioEvent.IsNullOrEmpty())
			{
				OnEventNotify notifyCallback = delegate(EventNotificationType eventType, string eventName, object info, GameObject gameObject)
				{
					if (queuedElement.TimeToDisplay == 0f && eventType == EventNotificationType.OnFinished)
					{
						queuedElement.TimeToDisplay = Time.realtimeSinceStartup - queuedElement.DisplayStartTime + 0.25f;
					}
				};
				if (!AudioManager.PostEventNotify(queuedElement.AudioEvent, notifyCallback, null))
				{
					this.RemoveQueuedElement(queuedElement.Index);
				}
			}
			if (queuedElement.Action != null)
			{
				queuedElement.Action();
				this.RemoveQueuedElement(queuedElement.Index);
			}
		}
	}

	private void Awake()
	{
		UIManager.SetGameObjectActive(this.BottomLeftDisplay.Container, false, null);
		UIManager.SetGameObjectActive(this.BottomRightDisplay.Container, false, null);
		UITutorialPanel.s_instance = this;
	}

	public void Update()
	{
		this.RebuildSubtitles();
	}

	public void LateUpdate()
	{
		if (SinglePlayerManager.Get() != null)
		{
			SinglePlayerManager.Get().UpdateRightAndLeftClickElements(this.m_tutorialRightClickPanel, this.m_tutorialRightClickText, this.m_tutorialLeftClickPanel, this.m_tutorialLeftClickText, this.m_tutorialShiftRightClickPanel, this.m_tutorialShiftRightClickText);
			SinglePlayerManager.Get().UpdateTutorialTextElements(this.m_tutorialTextPanel, this.m_tutorialText, this.m_tutorialTextPanel2, this.m_tutorialText2, this.m_tutorialTextPanel3, this.m_tutorialText3, this.m_tutorialCameraMovementPanel, this.m_tutorialCameraMovementText, this.m_tutorialCameraRotationPanel, this.m_tutorialCameraRotationText);
			SinglePlayerManager.Get().UpdateTutorialError(this.m_tutorialErrorPanel, this.m_tutorialErrorText);
			this.m_tutorialCleanUpNeeded = true;
		}
		else if (this.m_tutorialCleanUpNeeded)
		{
			UIManager.SetGameObjectActive(this.m_tutorialLeftClickPanel, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialRightClickPanel, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialTextPanel, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialTextPanel2, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialTextPanel3, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialCameraMovementPanel, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialCameraRotationPanel, false, null);
			this.m_tutorialCleanUpNeeded = false;
		}
	}

	public enum SubtitleLocation
	{
		BottomLeft,
		BottomRight
	}

	public class QueuedTutorialElement
	{
		public int Index;

		public string Subtitle;

		public UITutorialPanel.SubtitleLocation DisplayLocation;

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
}
