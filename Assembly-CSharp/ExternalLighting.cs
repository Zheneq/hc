using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Corale.Colore.Core;
using Corale.Colore.Razer.ChromaLink.Effects;
using Corale.Colore.Razer.Keyboard;
using UnityEngine;
using Color = Corale.Colore.Core.Color;
using Static = Corale.Colore.Razer.Headset.Effects.Static;

public class ExternalLighting : MonoBehaviour, IGameEventListener
{
	private bool[] m_chromaEnabled;
	private bool m_inDecision = true;
	private Dictionary<AbilityData.ActionType, bool> m_tutorialGlowOn;
	private Color m_decisionPhaseColor;
	private Color m_prepPhaseColor;
	private Color m_evasionPhaseColor;
	private Color m_combatPhaseColor;
	private Color m_movementPhaseColor;
	private Color m_blackColor;
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
		m_decisionPhaseColor = new Color(1f, 1f, 1f);
		m_prepPhaseColor = new Color(0f, 1f, 0f);
		m_evasionPhaseColor = new Color(1f, 1f, 0f);
		m_combatPhaseColor = new Color(1f, 0f, 0f);
		m_movementPhaseColor = new Color(0f, 0f, 1f);
		m_blackColor = new Color(0f, 0f, 0f);
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
		if (Chroma.SdkAvailable)
		{
			Chroma.Instance.Uninitialize();
		}
	}

	private void Update()
	{
		if (m_markedToRestartDecisionPhaseEffects)
		{
			if (m_inDecision)
			{
				StartDecisionPhaseEffects();
				m_markedToRestartDecisionPhaseEffects = false;
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		switch (eventType)
		{
			case GameEventManager.EventType.UIPhaseStartedPrep:
				m_inDecision = false;
				StartPrepPhaseEffects();
				break;
			case GameEventManager.EventType.UIPhaseStartedEvasion:
				m_inDecision = false;
				StartEvasionPhaseEffects();
				break;
			case GameEventManager.EventType.UIPhaseStartedCombat:
				m_inDecision = false;
				StartCombatPhaseEffects();
				break;
			case GameEventManager.EventType.UIPhaseStartedMovement:
				m_inDecision = false;
				StartMovementPhaseEffects();
				break;
			case GameEventManager.EventType.UIPhaseStartedDecision:
			case GameEventManager.EventType.TurnTick:
				m_inDecision = true;
				m_markedToRestartDecisionPhaseEffects = true;
				break;
			case GameEventManager.EventType.UITutorialHighlightChanged:
			{
				if (m_tutorialGlowOn == null)
				{
					m_tutorialGlowOn = new Dictionary<AbilityData.ActionType, bool>();
				}

				GameEventManager.ActivationInfo activationInfo = (GameEventManager.ActivationInfo)args;
				m_tutorialGlowOn[activationInfo.actionType] = activationInfo.active;

				if (m_inDecision)
				{
					m_markedToRestartDecisionPhaseEffects = true;
				}

				break;
			}
			case GameEventManager.EventType.GameTeardown:
				StartOutOfGameEffects();
				break;
		}
	}

	private void StartPrepPhaseEffects()
	{
		Color prepPhaseColor = m_prepPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Custom(prepPhaseColor));
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
				Headset.Instance.SetStatic(new Static(prepPhaseColor));
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
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(prepPhaseColor));
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
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(prepPhaseColor));
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
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(prepPhaseColor));
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
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color(m_prepPhaseColor.R, m_prepPhaseColor.G, m_prepPhaseColor.B));
		}
	}

	private void StartEvasionPhaseEffects()
	{
		Color evasionPhaseColor = m_evasionPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Custom(evasionPhaseColor));
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
				Headset.Instance.SetStatic(new Static(evasionPhaseColor));
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
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(evasionPhaseColor));
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
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(evasionPhaseColor));
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
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(evasionPhaseColor));
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
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color(m_evasionPhaseColor.R, m_evasionPhaseColor.G, m_evasionPhaseColor.B));
		}
	}

	private void StartCombatPhaseEffects()
	{
		Color combatPhaseColor = m_combatPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Custom(combatPhaseColor));
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
				Headset.Instance.SetStatic(new Static(combatPhaseColor));
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
			SetHueLightstripColor(new UnityEngine.Color(m_combatPhaseColor.R, m_combatPhaseColor.G, m_combatPhaseColor.B));
		}
	}

	private void StartMovementPhaseEffects()
	{
		Color movementPhaseColor = m_movementPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Custom(movementPhaseColor));
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
				Headset.Instance.SetStatic(new Static(movementPhaseColor));
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
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(movementPhaseColor));
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color(m_movementPhaseColor.R, m_movementPhaseColor.G, m_movementPhaseColor.B));
		}
	}

	private Color GetColorForActionType(AbilityData.ActionType actionType, bool checkCooldown)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData == null)
		{
			return m_blackColor;
		}
		bool queuedMovementAllowsAbility = activeOwnedActorData.QueuedMovementAllowsAbility;
		activeOwnedActorData.QueuedMovementAllowsAbility = true;
		AbilityData component = activeOwnedActorData.GetComponent<AbilityData>();
		AbilityData.AbilityEntry abilityEntry = component.abilityEntries[(int)actionType];
		if (abilityEntry != null && abilityEntry.ability != null)
		{
			AbilityPriority runPriority = abilityEntry.ability.RunPriority;
			switch (runPriority)
			{
				case AbilityPriority.Prep_Defense:
				case AbilityPriority.Prep_Offense:
					return !component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown
						? m_blackColor
						: m_prepPhaseColor;
				case AbilityPriority.Evasion:
					return !component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown
						? m_blackColor
						: m_evasionPhaseColor;
				default:
					return !component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown
						? m_blackColor
						: m_combatPhaseColor;
			}
		}

		activeOwnedActorData.QueuedMovementAllowsAbility = queuedMovementAllowsAbility;
		return m_blackColor;
	}

	private void StartDecisionPhaseEffects()
	{
		Color decisionPhaseColor = m_decisionPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Custom(decisionPhaseColor));
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
				Headset.Instance.SetStatic(new Static(decisionPhaseColor));
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
				Corale.Colore.Razer.Keyboard.Effects.Custom effect = new Corale.Colore.Razer.Keyboard.Effects.Custom(m_blackColor);
				
				foreach (KeyValuePair<AbilityData.ActionType, bool> keyValuePair in m_tutorialGlowOn)
				{
					if (!keyValuePair.Value)
					{
						continue;
					}
					flag = true;
					switch (keyValuePair.Key)
					{
						case AbilityData.ActionType.ABILITY_0:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Ability1, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.ABILITY_1:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Ability2, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.ABILITY_2:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Ability3, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.ABILITY_3:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Ability4, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.ABILITY_4:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Ability5, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.CARD_0:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Card1, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.CARD_1:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Card2, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
						case AbilityData.ActionType.CARD_2:
						{
							if (InputManager.Get().GetRazorKey(KeyPreference.Card3, out Key key))
							{
								effect[key] = GetColorForActionType(keyValuePair.Key, false);
							}

							break;
						}
					}
				}
				
				if (!flag)
				{
					effect = new Corale.Colore.Razer.Keyboard.Effects.Custom(m_decisionPhaseColor);
					Key key;
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanUp, out key))
					{
						effect[key] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanLeft, out key))
					{
						effect[key] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanDown, out key))
					{
						effect[key] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanRight, out key))
					{
						effect[key] = m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability1, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.ABILITY_0, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability2, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.ABILITY_1, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability3, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.ABILITY_2, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability4, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.ABILITY_3, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability5, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.ABILITY_4, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card1, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.CARD_0, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card2, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.CARD_1, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card3, out key))
					{
						effect[key] = GetColorForActionType(AbilityData.ActionType.CARD_2, true);
					}
				}
				Keyboard.Instance.SetCustom(effect);
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color(m_decisionPhaseColor.R, m_decisionPhaseColor.G, m_decisionPhaseColor.B));
		}
	}

	private void StartOutOfGameEffects()
	{
		Color decisionPhaseColor = m_decisionPhaseColor;
		if (m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Custom(decisionPhaseColor));
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
				Headset.Instance.SetStatic(new Static(decisionPhaseColor));
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				m_chromaEnabled[4] = false;
			}
		}
		if (m_hueEnabled)
		{
			SetHueLightstripColor(new UnityEngine.Color(m_decisionPhaseColor.R, m_decisionPhaseColor.G, m_decisionPhaseColor.B));
		}
	}

	private static Vector3 HSVFromRGB(UnityEngine.Color rgb)
	{
		float num = Mathf.Max(rgb.r, Mathf.Max(rgb.g, rgb.b));
		float num2 = Mathf.Min(rgb.r, Mathf.Min(rgb.g, rgb.b));
		float z = num;
		float num3;
		float y;
		if (num == num2)
		{
			num3 = 0f;
			y = 0f;
		}
		else
		{
			float num4 = num - num2;
			if (num == rgb.r)
			{
				num3 = (rgb.g - rgb.b) / num4;
			}
			else if (num == rgb.g)
			{
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
		if (newColor.r == 255f && newColor.g == 255f && newColor.b == 255f)
		{
			value = 0x9B3E;
			value2 = 0x6F;
			value3 = 0xFE;
		}
		else if (vector.x == 60f && vector.y == 1f && vector.z == 255f)
		{
			value = 0x445C;
			value2 = 0x64;
			value3 = 0xFE;
		}
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
