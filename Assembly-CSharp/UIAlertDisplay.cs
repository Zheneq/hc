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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertDisplay.Start()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_lowTimePulse, false, null);
		}
		if (this.m_timebankTimePulse != null)
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertDisplay.DisplayAlert(string, Color, float, bool, int)).MethodHandle;
				}
				if (!(message == alertMessage2.label.text))
				{
					continue;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertDisplay.CancelAlert(string)).MethodHandle;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertDisplay.TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType)).MethodHandle;
				}
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (type != UIAlertDisplay.LowTimePulseType.Standard)
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
					UIManager.SetGameObjectActive(this.m_timebankTimePulse, false, null);
					UIManager.SetGameObjectActive(this.m_timebankTimePulse, true, null);
				}
			}
			if (type != UIAlertDisplay.LowTimePulseType.TurnEndWarning)
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
				if (type != UIAlertDisplay.LowTimePulseType.UsingTimeBank)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAlertDisplay.Update()).MethodHandle;
				}
				if (activeOwnedActorData.IsDead())
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
					if (activeOwnedActorData.IsModelAnimatorDisabled())
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
						if (GameFlowData.Get().gameState > GameState.StartingGame)
						{
							UIManager.SetGameObjectActive(this.m_DeathAnimController, true, null);
							string text;
							if (activeOwnedActorData.NextRespawnTurn != -1)
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
								int num = activeOwnedActorData.NextRespawnTurn - GameFlowData.Get().CurrentTurn - 1;
								if (num > 0)
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
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!(this.m_deathLabel.text != text))
								{
									goto IL_160;
								}
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
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
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							UIManager.SetGameObjectActive(this.m_label[alertMessage.alertLabelIndex], false, null);
						}
						if (this.m_optionalLabelBackground != null)
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
							if (alertMessage.showBackground)
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
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							color2 = this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color;
						}
						else
						{
							color2 = default(Color);
						}
						Color color3 = color2;
						if (num2 > 0)
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
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							UIManager.SetGameObjectActive(alertMessage.label, true, null);
							if (this.m_optionalLabelBackground != null)
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
								if (alertMessage.showBackground)
								{
									for (int i = 0; i < this.m_optionalLabelBackground.Length; i++)
									{
										if (this.m_optionalLabelBackground[i] != null)
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
											UIManager.SetGameObjectActive(this.m_optionalLabelBackground[i], i == alertMessage.alertLabelIndex, null);
										}
									}
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									color3.a = this.m_originalLabelBgAlpha;
									this.m_optionalLabelBackground[alertMessage.alertLabelIndex].color = color3;
								}
							}
						}
						num2++;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			using (List<UIAlertDisplay.AlertMessage>.Enumerator enumerator2 = this.m_messagesToRemove.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIAlertDisplay.AlertMessage item = enumerator2.Current;
					this.m_currentMessages.Remove(item);
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
