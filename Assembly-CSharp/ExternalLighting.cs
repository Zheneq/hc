using Corale.Colore.Core;
using Corale.Colore.Razer.ChromaLink.Effects;
using Corale.Colore.Razer.Headset.Effects;
using Corale.Colore.Razer.Keyboard;
using Corale.Colore.Razer.Keyboard.Effects;
using Corale.Colore.Razer.Keypad.Effects;
using Corale.Colore.Razer.Mouse.Effects;
using Corale.Colore.Razer.Mousepad.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class ExternalLighting : MonoBehaviour, IGameEventListener
{
	private bool[] m_chromaEnabled;

	private bool m_inDecision = true;

	private Dictionary<AbilityData.ActionType, bool> m_tutorialGlowOn;

	private Corale.Colore.Core.Color m_decisionPhaseColor;

	private Corale.Colore.Core.Color m_prepPhaseColor;

	private Corale.Colore.Core.Color m_evasionPhaseColor;

	private Corale.Colore.Core.Color m_combatPhaseColor;

	private Corale.Colore.Core.Color m_movementPhaseColor;

	private Corale.Colore.Core.Color m_blackColor;

	private bool m_hueEnabled;

	private string m_hueHubAddress = "http://127.0.0.1";

	private string m_hueURL = "/api/newdeveloper/lights/1/state";

	private bool m_markedToRestartDecisionPhaseEffects;

	private void Start()
	{
		m_tutorialGlowOn = new Dictionary<AbilityData.ActionType, bool>();
		m_chromaEnabled = new bool[6];
		for (int i = 0; i < 6; i++)
		{
			m_chromaEnabled[i] = true;
		}
		m_hueEnabled = HydrogenConfig.Get().HueEnabled;
		m_hueHubAddress = HydrogenConfig.Get().HueAddress;
		m_hueURL = HydrogenConfig.Get().HuePutString;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedEvasion);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedCombat);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UITutorialHighlightChanged);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		m_decisionPhaseColor = new Corale.Colore.Core.Color(1f, 1f, 1f);
		m_prepPhaseColor = new Corale.Colore.Core.Color(0f, 1f, 0f);
		m_evasionPhaseColor = new Corale.Colore.Core.Color(1f, 1f, 0f);
		m_combatPhaseColor = new Corale.Colore.Core.Color(1f, 0f, 0f);
		m_movementPhaseColor = new Corale.Colore.Core.Color(0f, 0f, 1f);
		m_blackColor = new Corale.Colore.Core.Color(0f, 0f, 0f);
		StartOutOfGameEffects();
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedEvasion);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedCombat);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UITutorialHighlightChanged);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
	}

	private void OnApplicationQuit()
	{
		if (!Chroma.SdkAvailable)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Chroma.Instance.Uninitialize();
			return;
		}
	}

	private void Update()
	{
		if (!m_markedToRestartDecisionPhaseEffects)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_inDecision)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					StartDecisionPhaseEffects();
					m_markedToRestartDecisionPhaseEffects = false;
					return;
				}
			}
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedPrep)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_inDecision = false;
					StartPrepPhaseEffects();
					return;
				}
			}
		}
		if (eventType == GameEventManager.EventType.UIPhaseStartedEvasion)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_inDecision = false;
					StartEvasionPhaseEffects();
					return;
				}
			}
		}
		if (eventType == GameEventManager.EventType.UIPhaseStartedCombat)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_inDecision = false;
					StartCombatPhaseEffects();
					return;
				}
			}
		}
		if (eventType == GameEventManager.EventType.UIPhaseStartedMovement)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_inDecision = false;
					StartMovementPhaseEffects();
					return;
				}
			}
		}
		if (eventType != GameEventManager.EventType.UIPhaseStartedDecision)
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
			if (eventType != GameEventManager.EventType.TurnTick)
			{
				if (eventType == GameEventManager.EventType.UITutorialHighlightChanged)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
						{
							if (m_tutorialGlowOn == null)
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
								m_tutorialGlowOn = new Dictionary<AbilityData.ActionType, bool>();
							}
							GameEventManager.ActivationInfo activationInfo = (GameEventManager.ActivationInfo)args;
							if (activationInfo.active)
							{
								m_tutorialGlowOn[activationInfo.actionType] = true;
							}
							else
							{
								m_tutorialGlowOn[activationInfo.actionType] = false;
							}
							if (m_inDecision)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										m_markedToRestartDecisionPhaseEffects = true;
										return;
									}
								}
							}
							return;
						}
						}
					}
				}
				if (eventType != GameEventManager.EventType.GameTeardown)
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
					StartOutOfGameEffects();
					return;
				}
			}
		}
		m_inDecision = true;
		m_markedToRestartDecisionPhaseEffects = true;
	}

	private void StartPrepPhaseEffects()
	{
		Corale.Colore.Core.Color prepPhaseColor = m_prepPhaseColor;
		if (m_chromaEnabled[5])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[5] = false;
			}
		}
		if (m_chromaEnabled[0])
		{
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[0] = false;
			}
		}
		if (m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[1] = false;
			}
		}
		if (m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[2] = false;
			}
		}
		if (m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[3] = false;
			}
		}
		if (m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (!m_hueEnabled)
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
			SetHueLightstripColor(new UnityEngine.Color((int)m_prepPhaseColor.R, (int)m_prepPhaseColor.G, (int)m_prepPhaseColor.B));
			return;
		}
	}

	private void StartEvasionPhaseEffects()
	{
		Corale.Colore.Core.Color evasionPhaseColor = m_evasionPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[5] = false;
			}
		}
		if (m_chromaEnabled[0])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[0] = false;
			}
		}
		if (m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[1] = false;
			}
		}
		if (m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[2] = false;
			}
		}
		if (m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[3] = false;
			}
		}
		if (m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color((int)m_evasionPhaseColor.R, (int)m_evasionPhaseColor.G, (int)m_evasionPhaseColor.B));
		}
	}

	private void StartCombatPhaseEffects()
	{
		Corale.Colore.Core.Color combatPhaseColor = m_combatPhaseColor;
		if (m_chromaEnabled[5])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[5] = false;
			}
		}
		if (m_chromaEnabled[0])
		{
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(combatPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[0] = false;
			}
		}
		if (m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[1] = false;
			}
		}
		if (m_chromaEnabled[2])
		{
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[2] = false;
			}
		}
		if (m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[3] = false;
			}
		}
		if (m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color((int)m_combatPhaseColor.R, (int)m_combatPhaseColor.G, (int)m_combatPhaseColor.B));
		}
	}

	private void StartMovementPhaseEffects()
	{
		Corale.Colore.Core.Color movementPhaseColor = m_movementPhaseColor;
		if (m_chromaEnabled[5])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[5] = false;
			}
		}
		if (m_chromaEnabled[0])
		{
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[0] = false;
			}
		}
		if (m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[1] = false;
			}
		}
		if (m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[2] = false;
			}
		}
		if (m_chromaEnabled[3])
		{
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[3] = false;
			}
		}
		if (m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (!m_hueEnabled)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			SetHueLightstripColor(new UnityEngine.Color((int)m_movementPhaseColor.R, (int)m_movementPhaseColor.G, (int)m_movementPhaseColor.B));
			return;
		}
	}

	private Corale.Colore.Core.Color GetColorForActionType(AbilityData.ActionType actionType, bool checkCooldown)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			bool queuedMovementAllowsAbility = activeOwnedActorData.QueuedMovementAllowsAbility;
			activeOwnedActorData.QueuedMovementAllowsAbility = true;
			AbilityData component = activeOwnedActorData.GetComponent<AbilityData>();
			AbilityData.AbilityEntry abilityEntry = component.abilityEntries[(int)actionType];
			if (abilityEntry != null && abilityEntry.ability != null)
			{
				switch (abilityEntry.ability.RunPriority)
				{
				case AbilityPriority.Prep_Defense:
				case AbilityPriority.Prep_Offense:
					if (!component.ValidateActionIsRequestableDisregardingQueuedActions(actionType))
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (checkCooldown)
						{
							return m_blackColor;
						}
					}
					return m_prepPhaseColor;
				case AbilityPriority.Evasion:
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							if (!component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										return m_blackColor;
									}
								}
							}
							return m_evasionPhaseColor;
						}
					}
				default:
					if (!component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return m_blackColor;
							}
						}
					}
					return m_combatPhaseColor;
				}
			}
			activeOwnedActorData.QueuedMovementAllowsAbility = queuedMovementAllowsAbility;
		}
		return m_blackColor;
	}

	private void StartDecisionPhaseEffects()
	{
		Corale.Colore.Core.Color decisionPhaseColor = m_decisionPhaseColor;
		if (m_chromaEnabled[5])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[5] = false;
			}
		}
		if (m_chromaEnabled[0])
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
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[0] = false;
			}
		}
		if (m_chromaEnabled[1])
		{
			try
			{
				bool flag = false;
				Corale.Colore.Razer.Keyboard.Effects.Custom custom = new Corale.Colore.Razer.Keyboard.Effects.Custom(m_blackColor);
				using (Dictionary<AbilityData.ActionType, bool>.Enumerator enumerator = m_tutorialGlowOn.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<AbilityData.ActionType, bool> current = enumerator.Current;
						if (current.Value)
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
							flag = true;
							Key razerKey;
							if (current.Key == AbilityData.ActionType.ABILITY_0)
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
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability1, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.ABILITY_1)
							{
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability2, out razerKey))
								{
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.ABILITY_2)
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
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability3, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.ABILITY_3)
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
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability4, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.ABILITY_4)
							{
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability5, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.CARD_0)
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
								if (InputManager.Get().GetRazorKey(KeyPreference.Card1, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.CARD_1)
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
								if (InputManager.Get().GetRazorKey(KeyPreference.Card2, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
							else if (current.Key == AbilityData.ActionType.CARD_2)
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
								if (InputManager.Get().GetRazorKey(KeyPreference.Card3, out razerKey))
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
									custom[razerKey] = GetColorForActionType(current.Key, false);
								}
							}
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (!flag)
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
					custom = new Corale.Colore.Razer.Keyboard.Effects.Custom(m_decisionPhaseColor);
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanUp, out Key razerKey2))
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
						custom[razerKey2] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanLeft, out razerKey2))
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
						custom[razerKey2] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanDown, out razerKey2))
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
						custom[razerKey2] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanRight, out razerKey2))
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
						custom[razerKey2] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability1, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.ABILITY_0, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability2, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.ABILITY_1, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability3, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.ABILITY_2, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability4, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.ABILITY_3, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability5, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.ABILITY_4, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card1, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.CARD_0, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card2, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.CARD_1, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card3, out razerKey2))
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
						custom[razerKey2] = GetColorForActionType(AbilityData.ActionType.CARD_2, true);
					}
				}
				Keyboard.Instance.SetCustom(custom);
			}
			catch (Exception)
			{
				m_chromaEnabled[1] = false;
			}
		}
		if (m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[2] = false;
			}
		}
		if (m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[3] = false;
			}
		}
		if (m_chromaEnabled[4])
		{
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (!m_hueEnabled)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			SetHueLightstripColor(new UnityEngine.Color((int)m_decisionPhaseColor.R, (int)m_decisionPhaseColor.G, (int)m_decisionPhaseColor.B));
			return;
		}
	}

	private void StartOutOfGameEffects()
	{
		Corale.Colore.Core.Color decisionPhaseColor = m_decisionPhaseColor;
		if (m_chromaEnabled[5])
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[5] = false;
			}
		}
		if (m_chromaEnabled[0])
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
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[0] = false;
			}
		}
		if (m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[1] = false;
			}
		}
		if (m_chromaEnabled[2])
		{
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[2] = false;
			}
		}
		if (m_chromaEnabled[3])
		{
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[3] = false;
			}
		}
		if (m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (!m_hueEnabled)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			SetHueLightstripColor(new UnityEngine.Color((int)m_decisionPhaseColor.R, (int)m_decisionPhaseColor.G, (int)m_decisionPhaseColor.B));
			return;
		}
	}

	private static Vector3 HSVFromRGB(UnityEngine.Color rgb)
	{
		float num = Mathf.Max(rgb.r, Mathf.Max(rgb.g, rgb.b));
		float num2 = Mathf.Min(rgb.r, Mathf.Min(rgb.g, rgb.b));
		float z = num;
		float y;
		float num3;
		if (num == num2)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num3 = 0f;
			y = 0f;
		}
		else
		{
			float num4 = num - num2;
			if (num == rgb.r)
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
				num3 = (rgb.g - rgb.b) / num4;
			}
			else if (num == rgb.g)
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
				num3 = (rgb.b - rgb.r) / num4 + 2f;
			}
			else
			{
				num3 = (rgb.r - rgb.g) / num4 + 4f;
			}
			num3 *= 60f;
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			y = num4 / num;
		}
		return new Vector3(num3, y, z);
	}

	private void SetHueLightstripColor(UnityEngine.Color newColor)
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_hueHubAddress + m_hueURL);
		request.Method = "PUT";
		Vector3 vector = HSVFromRGB(newColor);
		int value = (int)(vector.x / 360f * 65535f);
		int value2 = (int)(vector.y * 255f);
		int value3 = (int)vector.z;
		if (newColor.r == 255f && newColor.g == 255f)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (newColor.b == 255f)
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
				value = 39742;
				value2 = 111;
				value3 = 254;
				goto IL_0121;
			}
		}
		if (vector.x == 60f)
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
			if (vector.y == 1f)
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
				if (vector.z == 255f)
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
					value = 17500;
					value2 = 100;
					value3 = 254;
				}
			}
		}
		goto IL_0121;
		IL_0121:
		string s = $"{{\"on\":true, \"transitiontime\":0, \"sat\":{Convert.ToString(value2)}, \"bri\":{Convert.ToString(value3)},\"hue\":{Convert.ToString(value)}}}";
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		request.ContentLength = bytes.Length;
		request.BeginGetRequestStream(delegate(IAsyncResult ar)
		{
			Stream stream = request.EndGetRequestStream(ar);
			stream.Write(bytes, 0, bytes.Length);
			stream.Close();
			request.BeginGetResponse(delegate(IAsyncResult ar2)
			{
				HttpWebResponse httpWebResponse;
				try
				{
					httpWebResponse = (HttpWebResponse)request.EndGetResponse(ar2);
				}
				catch (WebException ex)
				{
					httpWebResponse = (HttpWebResponse)ex.Response;
				}
				httpWebResponse.Close();
			}, null);
		}, null);
	}
}
