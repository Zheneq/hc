using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAlertDisplay : MonoBehaviour
{
	private class AlertMessage
	{
		public float startTime;

		public float duration;

		public TextMeshProUGUI label;

		public bool showBackground;

		public int alertLabelIndex;
	}

	public enum LowTimePulseType
	{
		Standard,
		TurnEndWarning,
		UsingTimeBank
	}

	public TextMeshProUGUI[] m_label;

	public Image[] m_optionalLabelBackground;

	public TextMeshProUGUI m_deathLabel;

	public Animator m_DeathAnimController;

	public GameObject m_lowTimePulse;

	public GameObject m_timebankTimePulse;

	[Space(10f)]
	[Header("Takedown")]
	public TextMeshProUGUI m_enemyTakeDownTitle;

	public TextMeshProUGUI m_enemyTakeDownSubTitle;

	public TextMeshProUGUI m_allyTakeDownTitle;

	public TextMeshProUGUI m_allyTakeDownSubTitle;

	public Animator m_takendownAlerts;

	public RectTransform m_enemyTakendownContainer;

	public RectTransform m_allyTakendownContainer;

	public RectTransform m_enemyAceContainer;

	public RectTransform m_allyAceContainer;

	public RectTransform[] m_enemyTakedownContainer;

	public RectTransform[] m_allyTakedownContainer;

	public Image[] m_enemyTakedowns;

	public Image[] m_allyTakedowns;

	private List<AlertMessage> m_currentMessages = new List<AlertMessage>();

	private List<AlertMessage> m_messagesToRemove = new List<AlertMessage>();

	private float m_originalLabelBgAlpha = 1f;

	public void Start()
	{
		if (m_lowTimePulse != null)
		{
			UIManager.SetGameObjectActive(m_lowTimePulse, false);
		}
		if (m_timebankTimePulse != null)
		{
			UIManager.SetGameObjectActive(m_timebankTimePulse, false);
		}
		for (int i = 0; i < m_label.Length; i++)
		{
			m_label[i].raycastTarget = false;
			UIManager.SetGameObjectActive(m_label[i], false);
			UIManager.SetGameObjectActive(m_optionalLabelBackground[i], false);
		}
		m_deathLabel.raycastTarget = false;
		if (m_optionalLabelBackground == null)
		{
			return;
		}
		while (true)
		{
			Color color = m_optionalLabelBackground[0].color;
			m_originalLabelBgAlpha = color.a;
			return;
		}
	}

	public void DisplayAlert(string message, Color color, float messageTimeSeconds, bool showBackground = false, int alertToUse = 0)
	{
		AlertMessage alertMessage = null;
		foreach (AlertMessage currentMessage in m_currentMessages)
		{
			if (!object.ReferenceEquals(message, currentMessage.label.text))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (!(message == currentMessage.label.text))
						{
							goto IL_0069;
						}
						goto IL_0065;
					}
				}
			}
			goto IL_0065;
			IL_0065:
			alertMessage = currentMessage;
			break;
			IL_0069:;
		}
		if (alertMessage == null)
		{
			alertMessage = new AlertMessage();
			alertMessage.alertLabelIndex = alertToUse;
			alertMessage.label = m_label[alertMessage.alertLabelIndex];
			m_currentMessages.Add(alertMessage);
		}
		alertMessage.showBackground = showBackground;
		if (m_optionalLabelBackground != null && showBackground)
		{
			for (int i = 0; i < m_optionalLabelBackground.Length; i++)
			{
				UIManager.SetGameObjectActive(m_optionalLabelBackground[i], i == alertMessage.alertLabelIndex);
			}
			Color color2 = m_optionalLabelBackground[alertMessage.alertLabelIndex].color;
			color2.a = m_originalLabelBgAlpha;
			m_optionalLabelBackground[alertMessage.alertLabelIndex].color = color2;
		}
		alertMessage.label.text = message;
		if (color != alertMessage.label.color)
		{
			alertMessage.label.color = color;
		}
		alertMessage.startTime = Time.time;
		AlertMessage alertMessage2 = alertMessage;
		float duration;
		if (messageTimeSeconds > 0f)
		{
			duration = messageTimeSeconds;
		}
		else
		{
			duration = 2f;
		}
		alertMessage2.duration = duration;
	}

	public void CancelAlert(string message)
	{
		using (List<AlertMessage>.Enumerator enumerator = m_currentMessages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AlertMessage current = enumerator.Current;
				if (current.label.text == message)
				{
					current.startTime = 0f;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	public void TriggerLowTimePulse(LowTimePulseType type)
	{
		TurnStateEnum currentState = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState;
		if (currentState == TurnStateEnum.CONFIRMED)
		{
			return;
		}
		if (GameFlowData.Get().activeOwnedActorData.IsDead())
		{
			if (currentState != TurnStateEnum.PICKING_RESPAWN)
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		if (m_lowTimePulse != null && type == LowTimePulseType.Standard)
		{
			UIManager.SetGameObjectActive(m_lowTimePulse, false);
			UIManager.SetGameObjectActive(m_lowTimePulse, true);
		}
		if (m_timebankTimePulse != null)
		{
			if (type != 0)
			{
				UIManager.SetGameObjectActive(m_timebankTimePulse, false);
				UIManager.SetGameObjectActive(m_timebankTimePulse, true);
			}
		}
		if (type != LowTimePulseType.TurnEndWarning)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (type != LowTimePulseType.UsingTimeBank)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								UISounds.GetUISounds().Play("ui/countdown/tick");
								return;
							}
						}
					}
					UISounds.GetUISounds().Play("ui/countdown/timebank");
					return;
				}
			}
		}
		UISounds.GetUISounds().Play("ui/countdown/one_second_left");
	}

	private void Update()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if ((bool)activeOwnedActorData)
		{
			if (activeOwnedActorData.IsDead())
			{
				if (activeOwnedActorData.IsModelAnimatorDisabled())
				{
					if (GameFlowData.Get().gameState > GameState.StartingGame)
					{
						UIManager.SetGameObjectActive(m_DeathAnimController, true);
						string text = null;
						if (activeOwnedActorData.NextRespawnTurn != -1)
						{
							int num = activeOwnedActorData.NextRespawnTurn - GameFlowData.Get().CurrentTurn - 1;
							if (num > 0)
							{
								text = string.Format(StringUtil.TR("RespawnInTurns", "Global"), num + 1);
							}
							else
							{
								text = StringUtil.TR("RespawnNextTurn", "Global");
							}
						}
						else
						{
							text = StringUtil.TR("YouAreDead", "Global");
						}
						if (object.ReferenceEquals(m_deathLabel.text, text))
						{
							if (!(m_deathLabel.text != text))
							{
								goto IL_0186;
							}
						}
						m_deathLabel.text = text;
						goto IL_0186;
					}
				}
			}
		}
		if (m_DeathAnimController.gameObject.activeInHierarchy)
		{
			m_DeathAnimController.Play("DeathTimePulseOUT");
		}
		goto IL_0186;
		IL_0186:
		int num2 = 0;
		using (List<AlertMessage>.Enumerator enumerator = m_currentMessages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AlertMessage current = enumerator.Current;
				if (Time.time - current.startTime > current.duration)
				{
					if (m_label[current.alertLabelIndex] != null)
					{
						UIManager.SetGameObjectActive(m_label[current.alertLabelIndex], false);
					}
					if (m_optionalLabelBackground != null)
					{
						if (current.showBackground)
						{
							if (m_optionalLabelBackground[current.alertLabelIndex] != null)
							{
								UIManager.SetGameObjectActive(m_optionalLabelBackground[current.alertLabelIndex], false);
							}
						}
					}
					m_messagesToRemove.Add(current);
				}
				else
				{
					float num3 = current.duration * 0.75f;
					float num4 = Time.time - current.startTime;
					float num5 = 1f - (num4 - num3) / (current.duration - num3);
					Color color = current.label.color;
					Color obj;
					if (m_optionalLabelBackground[current.alertLabelIndex] != null)
					{
						obj = m_optionalLabelBackground[current.alertLabelIndex].color;
					}
					else
					{
						obj = default(Color);
					}
					Color color2 = obj;
					if (num2 > 0)
					{
						color.a = 0f;
					}
					else
					{
						color.a = num5;
					}
					current.label.color = color;
					color2.a = num5;
					if (current.showBackground && m_optionalLabelBackground[current.alertLabelIndex] != null && num5 < m_originalLabelBgAlpha)
					{
						m_optionalLabelBackground[current.alertLabelIndex].color = color2;
					}
					if (!current.label.gameObject.activeSelf)
					{
						UIManager.SetGameObjectActive(current.label, true);
						if (m_optionalLabelBackground != null)
						{
							if (current.showBackground)
							{
								for (int i = 0; i < m_optionalLabelBackground.Length; i++)
								{
									if (m_optionalLabelBackground[i] != null)
									{
										UIManager.SetGameObjectActive(m_optionalLabelBackground[i], i == current.alertLabelIndex);
									}
								}
								color2.a = m_originalLabelBgAlpha;
								m_optionalLabelBackground[current.alertLabelIndex].color = color2;
							}
						}
					}
					num2++;
				}
			}
		}
		using (List<AlertMessage>.Enumerator enumerator2 = m_messagesToRemove.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				AlertMessage current2 = enumerator2.Current;
				m_currentMessages.Remove(current2);
			}
		}
		m_messagesToRemove.Clear();
	}

	private void OnEnable()
	{
		Update();
	}
}
