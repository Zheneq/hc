using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Corale.Colore.Core;
using Corale.Colore.Razer.ChromaLink.Effects;
using Corale.Colore.Razer.Headset.Effects;
using Corale.Colore.Razer.Keyboard;
using Corale.Colore.Razer.Keyboard.Effects;
using Corale.Colore.Razer.Keypad.Effects;
using Corale.Colore.Razer.Mouse.Effects;
using Corale.Colore.Razer.Mousepad.Effects;
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
		this.m_tutorialGlowOn = new Dictionary<AbilityData.ActionType, bool>();
		this.m_chromaEnabled = new bool[6];
		for (int i = 0; i < 6; i++)
		{
			this.m_chromaEnabled[i] = true;
		}
		this.m_hueEnabled = HydrogenConfig.Get().HueEnabled;
		this.m_hueHubAddress = HydrogenConfig.Get().HueAddress;
		this.m_hueURL = HydrogenConfig.Get().HuePutString;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedEvasion);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedCombat);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UITutorialHighlightChanged);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		this.m_decisionPhaseColor = new Corale.Colore.Core.Color(1f, 1f, 1f);
		this.m_prepPhaseColor = new Corale.Colore.Core.Color(0f, 1f, 0f);
		this.m_evasionPhaseColor = new Corale.Colore.Core.Color(1f, 1f, 0f);
		this.m_combatPhaseColor = new Corale.Colore.Core.Color(1f, 0f, 0f);
		this.m_movementPhaseColor = new Corale.Colore.Core.Color(0f, 0f, 1f);
		this.m_blackColor = new Corale.Colore.Core.Color(0f, 0f, 0f);
		this.StartOutOfGameEffects();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.OnApplicationQuit()).MethodHandle;
			}
			Chroma.Instance.Uninitialize();
		}
	}

	private void Update()
	{
		if (this.m_markedToRestartDecisionPhaseEffects)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.Update()).MethodHandle;
			}
			if (this.m_inDecision)
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
				this.StartDecisionPhaseEffects();
				this.m_markedToRestartDecisionPhaseEffects = false;
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedPrep)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			this.m_inDecision = false;
			this.StartPrepPhaseEffects();
		}
		else if (eventType == GameEventManager.EventType.UIPhaseStartedEvasion)
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
			this.m_inDecision = false;
			this.StartEvasionPhaseEffects();
		}
		else if (eventType == GameEventManager.EventType.UIPhaseStartedCombat)
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
			this.m_inDecision = false;
			this.StartCombatPhaseEffects();
		}
		else if (eventType == GameEventManager.EventType.UIPhaseStartedMovement)
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
			this.m_inDecision = false;
			this.StartMovementPhaseEffects();
		}
		else
		{
			if (eventType != GameEventManager.EventType.UIPhaseStartedDecision)
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
				if (eventType != GameEventManager.EventType.TurnTick)
				{
					if (eventType == GameEventManager.EventType.UITutorialHighlightChanged)
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
						if (this.m_tutorialGlowOn == null)
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
							this.m_tutorialGlowOn = new Dictionary<AbilityData.ActionType, bool>();
						}
						GameEventManager.ActivationInfo activationInfo = (GameEventManager.ActivationInfo)args;
						if (activationInfo.active)
						{
							this.m_tutorialGlowOn[activationInfo.actionType] = true;
						}
						else
						{
							this.m_tutorialGlowOn[activationInfo.actionType] = false;
						}
						if (this.m_inDecision)
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
							this.m_markedToRestartDecisionPhaseEffects = true;
						}
						return;
					}
					if (eventType == GameEventManager.EventType.GameTeardown)
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
						this.StartOutOfGameEffects();
						return;
					}
					return;
				}
			}
			this.m_inDecision = true;
			this.m_markedToRestartDecisionPhaseEffects = true;
		}
	}

	private void StartPrepPhaseEffects()
	{
		Corale.Colore.Core.Color prepPhaseColor = this.m_prepPhaseColor;
		if (this.m_chromaEnabled[5])
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.StartPrepPhaseEffects()).MethodHandle;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[5] = false;
			}
		}
		if (this.m_chromaEnabled[0])
		{
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(prepPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[0] = false;
			}
		}
		if (this.m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[1] = false;
			}
		}
		if (this.m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[2] = false;
			}
		}
		if (this.m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[3] = false;
			}
		}
		if (this.m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(prepPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[4] = false;
			}
		}
		if (this.m_hueEnabled)
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
			this.SetHueLightstripColor(new UnityEngine.Color((float)this.m_prepPhaseColor.R, (float)this.m_prepPhaseColor.G, (float)this.m_prepPhaseColor.B));
		}
	}

	private void StartEvasionPhaseEffects()
	{
		Corale.Colore.Core.Color evasionPhaseColor = this.m_evasionPhaseColor;
		if (this.m_chromaEnabled[5])
		{
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[5] = false;
			}
		}
		if (this.m_chromaEnabled[0])
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.StartEvasionPhaseEffects()).MethodHandle;
			}
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(evasionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[0] = false;
			}
		}
		if (this.m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[1] = false;
			}
		}
		if (this.m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[2] = false;
			}
		}
		if (this.m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[3] = false;
			}
		}
		if (this.m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(evasionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[4] = false;
			}
		}
		if (this.m_hueEnabled)
		{
			this.SetHueLightstripColor(new UnityEngine.Color((float)this.m_evasionPhaseColor.R, (float)this.m_evasionPhaseColor.G, (float)this.m_evasionPhaseColor.B));
		}
	}

	private void StartCombatPhaseEffects()
	{
		Corale.Colore.Core.Color combatPhaseColor = this.m_combatPhaseColor;
		if (this.m_chromaEnabled[5])
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.StartCombatPhaseEffects()).MethodHandle;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[5] = false;
			}
		}
		if (this.m_chromaEnabled[0])
		{
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(combatPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[0] = false;
			}
		}
		if (this.m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[1] = false;
			}
		}
		if (this.m_chromaEnabled[2])
		{
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[2] = false;
			}
		}
		if (this.m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[3] = false;
			}
		}
		if (this.m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(combatPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[4] = false;
			}
		}
		if (this.m_hueEnabled)
		{
			this.SetHueLightstripColor(new UnityEngine.Color((float)this.m_combatPhaseColor.R, (float)this.m_combatPhaseColor.G, (float)this.m_combatPhaseColor.B));
		}
	}

	private void StartMovementPhaseEffects()
	{
		Corale.Colore.Core.Color movementPhaseColor = this.m_movementPhaseColor;
		if (this.m_chromaEnabled[5])
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.StartMovementPhaseEffects()).MethodHandle;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[5] = false;
			}
		}
		if (this.m_chromaEnabled[0])
		{
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(movementPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[0] = false;
			}
		}
		if (this.m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[1] = false;
			}
		}
		if (this.m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[2] = false;
			}
		}
		if (this.m_chromaEnabled[3])
		{
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[3] = false;
			}
		}
		if (this.m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(movementPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[4] = false;
			}
		}
		if (this.m_hueEnabled)
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
			this.SetHueLightstripColor(new UnityEngine.Color((float)this.m_movementPhaseColor.R, (float)this.m_movementPhaseColor.G, (float)this.m_movementPhaseColor.B));
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
				AbilityPriority runPriority = abilityEntry.ability.RunPriority;
				if (runPriority == AbilityPriority.Prep_Defense || runPriority == AbilityPriority.Prep_Offense)
				{
					if (!component.ValidateActionIsRequestableDisregardingQueuedActions(actionType))
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.GetColorForActionType(AbilityData.ActionType, bool)).MethodHandle;
						}
						if (checkCooldown)
						{
							return this.m_blackColor;
						}
					}
					return this.m_prepPhaseColor;
				}
				if (runPriority == AbilityPriority.Evasion)
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
					if (!component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown)
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
						return this.m_blackColor;
					}
					return this.m_evasionPhaseColor;
				}
				else
				{
					if (!component.ValidateActionIsRequestableDisregardingQueuedActions(actionType) && checkCooldown)
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
						return this.m_blackColor;
					}
					return this.m_combatPhaseColor;
				}
			}
			else
			{
				activeOwnedActorData.QueuedMovementAllowsAbility = queuedMovementAllowsAbility;
			}
		}
		return this.m_blackColor;
	}

	private void StartDecisionPhaseEffects()
	{
		Corale.Colore.Core.Color decisionPhaseColor = this.m_decisionPhaseColor;
		if (this.m_chromaEnabled[5])
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.StartDecisionPhaseEffects()).MethodHandle;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[5] = false;
			}
		}
		if (this.m_chromaEnabled[0])
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
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[0] = false;
			}
		}
		if (this.m_chromaEnabled[1])
		{
			try
			{
				bool flag = false;
				Corale.Colore.Razer.Keyboard.Effects.Custom custom = new Corale.Colore.Razer.Keyboard.Effects.Custom(this.m_blackColor);
				using (Dictionary<AbilityData.ActionType, bool>.Enumerator enumerator = this.m_tutorialGlowOn.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<AbilityData.ActionType, bool> keyValuePair = enumerator.Current;
						if (keyValuePair.Value)
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
							flag = true;
							if (keyValuePair.Key == AbilityData.ActionType.ABILITY_0)
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
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability1, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.ABILITY_1)
							{
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability2, out key))
								{
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.ABILITY_2)
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
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability3, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.ABILITY_3)
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
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability4, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.ABILITY_4)
							{
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Ability5, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.CARD_0)
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
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Card1, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.CARD_1)
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
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Card2, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
							else if (keyValuePair.Key == AbilityData.ActionType.CARD_2)
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
								Key key;
								if (InputManager.Get().GetRazorKey(KeyPreference.Card3, out key))
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
									custom[key] = this.GetColorForActionType(keyValuePair.Key, false);
								}
							}
						}
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
				if (!flag)
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
					custom = new Corale.Colore.Razer.Keyboard.Effects.Custom(this.m_decisionPhaseColor);
					Key key2;
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanUp, out key2))
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
						custom[key2] = this.m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanLeft, out key2))
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
						custom[key2] = this.m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanDown, out key2))
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
						custom[key2] = this.m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.CameraPanRight, out key2))
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
						custom[key2] = this.m_movementPhaseColor;
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability1, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.ABILITY_0, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability2, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.ABILITY_1, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability3, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.ABILITY_2, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability4, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.ABILITY_3, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Ability5, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.ABILITY_4, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card1, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.CARD_0, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card2, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.CARD_1, true);
					}
					if (InputManager.Get().GetRazorKey(KeyPreference.Card3, out key2))
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
						custom[key2] = this.GetColorForActionType(AbilityData.ActionType.CARD_2, true);
					}
				}
				Keyboard.Instance.SetCustom(custom);
			}
			catch (Exception)
			{
				this.m_chromaEnabled[1] = false;
			}
		}
		if (this.m_chromaEnabled[2])
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
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[2] = false;
			}
		}
		if (this.m_chromaEnabled[3])
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
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[3] = false;
			}
		}
		if (this.m_chromaEnabled[4])
		{
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[4] = false;
			}
		}
		if (this.m_hueEnabled)
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
			this.SetHueLightstripColor(new UnityEngine.Color((float)this.m_decisionPhaseColor.R, (float)this.m_decisionPhaseColor.G, (float)this.m_decisionPhaseColor.B));
		}
	}

	private void StartOutOfGameEffects()
	{
		Corale.Colore.Core.Color decisionPhaseColor = this.m_decisionPhaseColor;
		if (this.m_chromaEnabled[5])
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.StartOutOfGameEffects()).MethodHandle;
			}
			try
			{
				ChromaLink.Instance.SetCustom(new Corale.Colore.Razer.ChromaLink.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[5] = false;
			}
		}
		if (this.m_chromaEnabled[0])
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
			try
			{
				Headset.Instance.SetStatic(new Corale.Colore.Razer.Headset.Effects.Static(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[0] = false;
			}
		}
		if (this.m_chromaEnabled[1])
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
			try
			{
				Keyboard.Instance.SetCustom(new Corale.Colore.Razer.Keyboard.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[1] = false;
			}
		}
		if (this.m_chromaEnabled[2])
		{
			try
			{
				Keypad.Instance.SetCustom(new Corale.Colore.Razer.Keypad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[2] = false;
			}
		}
		if (this.m_chromaEnabled[3])
		{
			try
			{
				Mouse.Instance.SetCustom(new Corale.Colore.Razer.Mouse.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[3] = false;
			}
		}
		if (this.m_chromaEnabled[4])
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
			try
			{
				Mousepad.Instance.SetCustom(new Corale.Colore.Razer.Mousepad.Effects.Custom(decisionPhaseColor));
			}
			catch (Exception)
			{
				this.m_chromaEnabled[4] = false;
			}
		}
		if (this.m_hueEnabled)
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
			this.SetHueLightstripColor(new UnityEngine.Color((float)this.m_decisionPhaseColor.R, (float)this.m_decisionPhaseColor.G, (float)this.m_decisionPhaseColor.B));
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.HSVFromRGB(UnityEngine.Color)).MethodHandle;
			}
			num3 = 0f;
			y = 0f;
		}
		else
		{
			float num4 = num - num2;
			if (num == rgb.r)
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
				num3 = (rgb.g - rgb.b) / num4;
			}
			else if (num == rgb.g)
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
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.m_hueHubAddress + this.m_hueURL);
		request.Method = "PUT";
		Vector3 vector = ExternalLighting.HSVFromRGB(newColor);
		int value = (int)(vector.x / 360f * 65535f);
		int value2 = (int)(vector.y * 255f);
		int value3 = (int)vector.z;
		if (newColor.r == 255f && newColor.g == 255f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExternalLighting.SetHueLightstripColor(UnityEngine.Color)).MethodHandle;
			}
			if (newColor.b == 255f)
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
				value = 0x9B3E;
				value2 = 0x6F;
				value3 = 0xFE;
				goto IL_121;
			}
		}
		if (vector.x == 60f)
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
			if (vector.y == 1f)
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
				if (vector.z == 255f)
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
					value = 0x445C;
					value2 = 0x64;
					value3 = 0xFE;
				}
			}
		}
		IL_121:
		string s = string.Format("{{\"on\":true, \"transitiontime\":0, \"sat\":{0}, \"bri\":{1},\"hue\":{2}}}", Convert.ToString(value2), Convert.ToString(value3), Convert.ToString(value));
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		request.ContentLength = (long)bytes.Length;
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
