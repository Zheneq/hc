using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAlertDisplay : MonoBehaviour
{
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

	private List<UIAlertDisplay.AlertMessage> m_currentMessages = new List<UIAlertDisplay.AlertMessage>();

	private List<UIAlertDisplay.AlertMessage> m_messagesToRemove = new List<UIAlertDisplay.AlertMessage>();

	private float m_originalLabelBgAlpha = 1f;

	public void Start()
	{
		if (this.m_lowTimePulse != null)
		{
			UIManager.SetGameObjectActive(this.m_lowTimePulse, false, null);
		}
		if (this.m_timebankTimePulse != null)
		{
			UIManager.SetGameObjectActive(this.m_timebankTimePulse, false, null);
		}
		for (int i = 0; i < this.m_label.Length; i++)
		{
			this.m_label[i].raycastTarget = false;
			UIManager.SetGameObjectActive(this.m_label[i], false, null);
			UIManager.SetGameObjectActive(this.m_optionalLabelBackground[i], false, null);
		}
		this.m_deathLabel.raycastTarget = false;
		if (this.m_optionalLabelBackground != null)
		{
			this.m_originalLabelBgAlpha = this.m_optionalLabelBackground[0].color.a;
		}
	}

	public void DisplayAlert(string message, Color color, float messageTimeSeconds, bool showBackground = false, int alertToUse = 0)
	{
		UIAlertDisplay.AlertMessage alertMessage = null;
		foreach (UIAlertDisplay.AlertMessage alertMessage2 in this.m_currentMessages)
		{
			if (!object.ReferenceEquals(message, alertMessage2.label.text))
			{
				if (!(message == alertMessage2.label.text))
				{
					continue;
				}
			}
			alertMessage = alertMessage2;
			break;
		}
		if (alertMessage == null)
		{
			alertMessage = new UIAlertDisplay.AlertMessage();
			alertMessage.alertLabelIndex = alertToUse;
			alertMessage.label = this.m_label[alertMessage.alertLabelIndex];
			this.m_currentMessages.Add(alertMessage);
		}
		alertMessage.showBackground = showBackground;
		if (this.m_optionalLabelBackground != null && showBackground)
		{
			for (int i = 0; i < this.m_optionalLabelBackground.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_optionalLabelBackground[i], i == alertMessage.alertLabelIndex, null);
			}
			Color color2 = this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color;
			color2.a = this.m_originalLabelBgAlpha;
			this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color = color2;
		}
		alertMessage.label.text = message;
		if (color != alertMessage.label.color)
		{
			alertMessage.label.color = color;
		}
		alertMessage.startTime = Time.time;
		UIAlertDisplay.AlertMessage alertMessage3 = alertMessage;
		float duration;
		if (messageTimeSeconds > 0f)
		{
			duration = messageTimeSeconds;
		}
		else
		{
			duration = 2f;
		}
		alertMessage3.duration = duration;
	}

	public void CancelAlert(string message)
	{
		using (List<UIAlertDisplay.AlertMessage>.Enumerator enumerator = this.m_currentMessages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIAlertDisplay.AlertMessage alertMessage = enumerator.Current;
				if (alertMessage.label.text == message)
				{
					alertMessage.startTime = 0f;
				}
			}
		}
	}

	public void TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType type)
	{
		TurnStateEnum currentState = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState;
		if (currentState != TurnStateEnum.CONFIRMED)
		{
			if (GameFlowData.Get().activeOwnedActorData.IsDead())
			{
				if (currentState != TurnStateEnum.PICKING_RESPAWN)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						return;
					}
				}
			}
			if (this.m_lowTimePulse != null && type == UIAlertDisplay.LowTimePulseType.Standard)
			{
				UIManager.SetGameObjectActive(this.m_lowTimePulse, false, null);
				UIManager.SetGameObjectActive(this.m_lowTimePulse, true, null);
			}
			if (this.m_timebankTimePulse != null)
			{
				if (type != UIAlertDisplay.LowTimePulseType.Standard)
				{
					UIManager.SetGameObjectActive(this.m_timebankTimePulse, false, null);
					UIManager.SetGameObjectActive(this.m_timebankTimePulse, true, null);
				}
			}
			if (type != UIAlertDisplay.LowTimePulseType.TurnEndWarning)
			{
				if (type != UIAlertDisplay.LowTimePulseType.UsingTimeBank)
				{
					UISounds.GetUISounds().Play("ui/countdown/tick");
				}
				else
				{
					UISounds.GetUISounds().Play("ui/countdown/timebank");
				}
			}
			else
			{
				UISounds.GetUISounds().Play("ui/countdown/one_second_left");
			}
			return;
		}
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData)
			{
				if (activeOwnedActorData.IsDead())
				{
					if (activeOwnedActorData.IsModelAnimatorDisabled())
					{
						if (GameFlowData.Get().gameState > GameState.StartingGame)
						{
							UIManager.SetGameObjectActive(this.m_DeathAnimController, true, null);
							string text;
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
							if (object.ReferenceEquals(this.m_deathLabel.text, text))
							{
								if (!(this.m_deathLabel.text != text))
								{
									goto IL_160;
								}
							}
							this.m_deathLabel.text = text;
							IL_160:
							goto IL_186;
						}
					}
				}
			}
			if (this.m_DeathAnimController.gameObject.activeInHierarchy)
			{
				this.m_DeathAnimController.Play("DeathTimePulseOUT");
			}
			IL_186:
			int num2 = 0;
			using (List<UIAlertDisplay.AlertMessage>.Enumerator enumerator = this.m_currentMessages.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIAlertDisplay.AlertMessage alertMessage = enumerator.Current;
					if (Time.time - alertMessage.startTime > alertMessage.duration)
					{
						if (this.m_label[alertMessage.alertLabelIndex] != null)
						{
							UIManager.SetGameObjectActive(this.m_label[alertMessage.alertLabelIndex], false, null);
						}
						if (this.m_optionalLabelBackground != null)
						{
							if (alertMessage.showBackground)
							{
								if (this.m_optionalLabelBackground[alertMessage.alertLabelIndex] != null)
								{
									UIManager.SetGameObjectActive(this.m_optionalLabelBackground[alertMessage.alertLabelIndex], false, null);
								}
							}
						}
						this.m_messagesToRemove.Add(alertMessage);
					}
					else
					{
						float num3 = alertMessage.duration * 0.75f;
						float num4 = Time.time - alertMessage.startTime;
						float num5 = 1f - (num4 - num3) / (alertMessage.duration - num3);
						Color color = alertMessage.label.color;
						Color color2;
						if (this.m_optionalLabelBackground[alertMessage.alertLabelIndex] != null)
						{
							color2 = this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color;
						}
						else
						{
							color2 = default(Color);
						}
						Color color3 = color2;
						if (num2 > 0)
						{
							color.a = 0f;
						}
						else
						{
							color.a = num5;
						}
						alertMessage.label.color = color;
						color3.a = num5;
						if (alertMessage.showBackground && this.m_optionalLabelBackground[alertMessage.alertLabelIndex] != null && num5 < this.m_originalLabelBgAlpha)
						{
							this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color = color3;
						}
						if (!alertMessage.label.gameObject.activeSelf)
						{
							UIManager.SetGameObjectActive(alertMessage.label, true, null);
							if (this.m_optionalLabelBackground != null)
							{
								if (alertMessage.showBackground)
								{
									for (int i = 0; i < this.m_optionalLabelBackground.Length; i++)
									{
										if (this.m_optionalLabelBackground[i] != null)
										{
											UIManager.SetGameObjectActive(this.m_optionalLabelBackground[i], i == alertMessage.alertLabelIndex, null);
										}
									}
									color3.a = this.m_originalLabelBgAlpha;
									this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color = color3;
								}
							}
						}
						num2++;
					}
				}
			}
			using (List<UIAlertDisplay.AlertMessage>.Enumerator enumerator2 = this.m_messagesToRemove.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIAlertDisplay.AlertMessage item = enumerator2.Current;
					this.m_currentMessages.Remove(item);
				}
			}
			this.m_messagesToRemove.Clear();
		}
	}

	private void OnEnable()
	{
		this.Update();
	}

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
}
