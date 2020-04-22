using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UICharacterWorldObjects : MonoBehaviour
{
	private struct LoadedCharacter
	{
		public CharacterType type;

		public CharacterVisualInfo skin;

		public CharacterResourceLink resourceLink;

		public int loadingTicket;

		public bool ready;

		public bool isInGame;

		public GameObject instantiatedCharacter;

		public UIActorModelData uiActorModelData;

		public void Clear()
		{
			type = CharacterType.None;
			skin = default(CharacterVisualInfo);
			instantiatedCharacter = null;
			loadingTicket = -1;
			uiActorModelData = null;
			isInGame = false;
			resourceLink = null;
		}
	}

	private bool m_shuttingDown;

	private GameObject m_characterLookAtLocation;

	public UICharacterSelectRing[] m_ringAnimations;

	private bool[] m_characterIsLoading;

	private LoadedCharacter[] m_loadedCharacters;

	private bool m_visible;

	protected Camera GetCharacterLookAtCamera()
	{
		return UIManager.Get().GetEnvirontmentCamera();
	}

	protected void Initialize()
	{
		m_shuttingDown = false;
		m_visible = true;
		SetVisible(false);
		m_loadedCharacters = new LoadedCharacter[m_ringAnimations.Length];
		m_characterIsLoading = new bool[m_ringAnimations.Length];
		for (int i = 0; i < m_loadedCharacters.Length; i++)
		{
			m_loadedCharacters[i].Clear();
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
			if (!(m_loadedCharacters[0].instantiatedCharacter != null))
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
				if (m_characterLookAtLocation == null)
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
					m_characterLookAtLocation = new GameObject();
				}
				Transform transform = m_characterLookAtLocation.transform;
				Vector3 position = GetCharacterLookAtCamera().transform.position;
				float x = position.x;
				Vector3 position2 = m_loadedCharacters[0].instantiatedCharacter.transform.position;
				float y = position2.y;
				Vector3 position3 = GetCharacterLookAtCamera().transform.position;
				transform.position = new Vector3(x, y, position3.z);
				m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(m_characterLookAtLocation.transform);
				return;
			}
		}
	}

	private void OnDestroy()
	{
		m_shuttingDown = true;
		UnloadAllCharacters();
	}

	private void Update()
	{
		if (!(m_loadedCharacters[0].instantiatedCharacter != null))
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
			if (!(GetCharacterLookAtCamera() != null))
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
				if (m_characterLookAtLocation == null)
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
					m_characterLookAtLocation = new GameObject();
				}
				Transform transform = m_characterLookAtLocation.transform;
				Vector3 position = GetCharacterLookAtCamera().transform.position;
				float x = position.x;
				Vector3 position2 = m_loadedCharacters[0].instantiatedCharacter.transform.position;
				float y = position2.y;
				Vector3 position3 = GetCharacterLookAtCamera().transform.position;
				transform.position = new Vector3(x, y, position3.z);
				m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(m_characterLookAtLocation.transform);
				m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles += UIFrontEnd.Get().GetRotationOffset();
				return;
			}
		}
	}

	public bool IsCharReady(int index)
	{
		return m_loadedCharacters[index].ready;
	}

	public void CheckReadyBand(int index, bool isReady)
	{
		m_ringAnimations[index].CheckReadyBand(isReady);
	}

	public void SetCharacterReady(int index, bool isReady)
	{
		if (m_loadedCharacters[index].ready == isReady)
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
			if (isReady)
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
				m_ringAnimations[index].PlayAnimation("ReadyIn");
			}
			else
			{
				m_ringAnimations[index].PlayAnimation("ReadyOut");
			}
			m_loadedCharacters[index].ready = isReady;
			return;
		}
	}

	public void SetCharacterInGame(int index, bool isInGame)
	{
		m_loadedCharacters[index].isInGame = isInGame;
	}

	public bool CharacterIsLoading()
	{
		return m_characterIsLoading[0];
	}

	public int GetNumLoadedCharacters()
	{
		int num = 0;
		for (int i = 0; i < m_loadedCharacters.Length; i++)
		{
			if (m_loadedCharacters[i].uiActorModelData != null)
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
				num++;
			}
		}
		return num;
	}

	public void SetReadyPose()
	{
		for (int i = 0; i < m_loadedCharacters.Length; i++)
		{
			if (m_loadedCharacters[i].uiActorModelData != null)
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
				m_loadedCharacters[i].uiActorModelData.SetReady(m_loadedCharacters[i].ready);
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void SetSkins()
	{
		for (int i = 0; i < m_loadedCharacters.Length; i++)
		{
			if (m_loadedCharacters[i].uiActorModelData != null)
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
				m_loadedCharacters[i].uiActorModelData.SetSkin(m_loadedCharacters[i].skin);
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void SetIsInGameRings()
	{
		for (int i = 0; i < m_loadedCharacters.Length; i++)
		{
			Animator isInGameAnimation = m_ringAnimations[i].m_isInGameAnimation;
			int doActive;
			if (m_loadedCharacters[i].isInGame)
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
				doActive = ((!m_loadedCharacters[i].ready) ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(isInGameAnimation, (byte)doActive != 0);
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			for (int j = m_loadedCharacters.Length; j < m_ringAnimations.Length; j++)
			{
				UIManager.SetGameObjectActive(m_ringAnimations[j].m_isInGameAnimation, false);
			}
			return;
		}
	}

	public void UnloadAllCharacters()
	{
		for (int i = 0; i < m_loadedCharacters.Length; i++)
		{
			UnloadCharacter(i);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void UnloadCharacter(int slotIndex, bool playAnimation = true)
	{
		if (m_loadedCharacters[slotIndex].loadingTicket != -1)
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
			m_loadedCharacters[slotIndex].resourceLink.CancelLoad(m_loadedCharacters[slotIndex].skin, m_loadedCharacters[slotIndex].loadingTicket);
		}
		if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
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
			UnityEngine.Object.Destroy(m_loadedCharacters[slotIndex].instantiatedCharacter);
			if (playAnimation)
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
				if (!m_shuttingDown)
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
					m_ringAnimations[slotIndex].PlayAnimation("TransitionOut");
				}
			}
		}
		if (playAnimation)
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
			if (!m_shuttingDown)
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
				m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotOUT");
			}
		}
		m_loadedCharacters[slotIndex].Clear();
		if (m_ringAnimations[slotIndex].m_isInGameAnimation != null)
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
			UIManager.SetGameObjectActive(m_ringAnimations[slotIndex].m_isInGameAnimation, false);
		}
		if (!(m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null))
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
			UIManager.SetGameObjectActive(m_ringAnimations[slotIndex].m_charSelectSpawnVFX, false);
			return;
		}
	}

	public void ChangeLayersRecursively(Transform trans, string name)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(name);
		IEnumerator enumerator = trans.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform != trans.gameObject)
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
					ChangeLayersRecursively(transform, name);
				}
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_006b;
					}
				}
			}
			end_IL_006b:;
		}
	}

	public CharacterType CharacterTypeInSlot(int slotIndex)
	{
		return m_loadedCharacters[slotIndex].type;
	}

	public CharacterResourceLink CharacterResourceLinkInSlot(int slotIndex)
	{
		return m_loadedCharacters[slotIndex].resourceLink;
	}

	public CharacterVisualInfo CharacterVisualInfoInSlot(int slotIndex)
	{
		return m_loadedCharacters[slotIndex].skin;
	}

	public void SetCharSelectTriggerForSlot(CharacterResourceLink characterLink, int slotIndex)
	{
		if (!(characterLink != null))
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
			if (characterLink.m_characterType != m_loadedCharacters[slotIndex].type)
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
				if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						Animator componentInChildren = m_loadedCharacters[slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
						UIActorModelData.SetCharSelectTrigger(componentInChildren, false, false);
						return;
					}
				}
				return;
			}
		}
	}

	public void LoadCharacterIntoSlot(CharacterType character, int slotIndex, string characterName, CharacterVisualInfo skinSelector, bool isBot)
	{
		LoadCharacterIntoSlot(GameWideData.Get().GetCharacterResourceLink(character), slotIndex, characterName, skinSelector, isBot, false);
	}

	public void LoadCharacterIntoSlot(CharacterResourceLink characterLink, int slotIndex, string characterName, CharacterVisualInfo visualInfo, bool isBot, bool playSelectionChatterCue)
	{
		if (UIFrontEnd.Get() == null)
		{
			return;
		}
		bool prevCharacterLoaded;
		string prevAnimInfoName;
		float prevAnimInfoNormalizedTime;
		int prevAnimInfoStateHash;
		bool prevAnimInCharSelState;
		bool ResetRotation;
		CharacterType prevCharType;
		bool preUnloadChar;
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (UICharacterSelectWorldObjects.Get() == null)
			{
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			if (characterLink != null && characterLink.m_characterType == m_loadedCharacters[slotIndex].type)
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
				if (visualInfo.Equals(m_loadedCharacters[slotIndex].skin))
				{
					if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							Animator componentInChildren = m_loadedCharacters[slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
							UIActorModelData.SetCharSelectTrigger(componentInChildren, false, false);
							return;
						}
					}
					return;
				}
			}
			if (characterLink != null)
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
				if (!characterLink.IsVisualInfoSelectionValid(visualInfo))
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
					Log.Error($"Character {characterLink.m_displayName} could not find Actor Skin resource link for {visualInfo.ToString()}");
					visualInfo = default(CharacterVisualInfo);
				}
			}
			Animator animator = null;
			GameObject instantiatedCharacter = m_loadedCharacters[slotIndex].instantiatedCharacter;
			prevCharacterLoaded = (instantiatedCharacter != null);
			prevAnimInfoName = string.Empty;
			prevAnimInfoNormalizedTime = 0f;
			prevAnimInfoStateHash = 0;
			prevAnimInCharSelState = false;
			if (prevCharacterLoaded)
			{
				animator = instantiatedCharacter.GetComponentInChildren<Animator>();
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				prevAnimInfoName = animator.name;
				if (animator.runtimeAnimatorController != null)
				{
					prevAnimInfoName = animator.runtimeAnimatorController.name;
				}
				prevAnimInfoStateHash = currentAnimatorStateInfo.fullPathHash;
				prevAnimInfoNormalizedTime = currentAnimatorStateInfo.normalizedTime;
				prevAnimInCharSelState = UIActorModelData.IsInCharSelectAnimState(animator);
			}
			ResetRotation = false;
			if (slotIndex == 0)
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
				if (characterLink != null)
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
					if (m_loadedCharacters[slotIndex].type != characterLink.m_characterType)
					{
						ResetRotation = true;
					}
				}
			}
			prevCharType = CharacterType.None;
			if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
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
				prevCharType = m_loadedCharacters[slotIndex].type;
			}
			int num;
			if (!(characterLink == null))
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
				if (m_loadedCharacters[slotIndex].type == characterLink.m_characterType)
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
					num = ((m_loadedCharacters[slotIndex].skin.skinIndex != visualInfo.skinIndex) ? 1 : 0);
					goto IL_0390;
				}
			}
			num = 1;
			goto IL_0390;
			IL_0390:
			preUnloadChar = ((byte)num != 0);
			if (preUnloadChar)
			{
				UnloadCharacter(slotIndex, !prevCharacterLoaded || characterLink == null);
			}
			if (characterLink == null)
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
				m_ringAnimations[slotIndex].PlayAnimation("ReadyOut");
			}
			if (!(characterLink != null))
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
				m_loadedCharacters[slotIndex].resourceLink = characterLink;
				m_loadedCharacters[slotIndex].type = characterLink.m_characterType;
				m_loadedCharacters[slotIndex].skin = visualInfo;
				m_characterIsLoading[slotIndex] = true;
				if (AsyncManager.Get() != null)
				{
					characterLink.LoadAsync(visualInfo, out m_loadedCharacters[slotIndex].loadingTicket, (float)slotIndex * 0.1f, delegate(LoadedCharacterSelection loadedCharacter)
					{
						if (!(UIFrontEnd.Get() == null))
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
									if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
									{
										while (true)
										{
											switch (7)
											{
											default:
												return;
											case 0:
												break;
											}
										}
									}
									if (!preUnloadChar)
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
										UICharacterWorldObjects uICharacterWorldObjects = this;
										int slotIndex2 = slotIndex;
										int playAnimation;
										if (prevCharacterLoaded)
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
											playAnimation = ((characterLink == null) ? 1 : 0);
										}
										else
										{
											playAnimation = 1;
										}
										uICharacterWorldObjects.UnloadCharacter(slotIndex2, (byte)playAnimation != 0);
									}
									else if (m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null)
									{
										UIManager.SetGameObjectActive(m_ringAnimations[slotIndex].m_charSelectSpawnVFX, true);
									}
									m_loadedCharacters[slotIndex].loadingTicket = -1;
									if (!m_characterIsLoading[slotIndex])
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
										UnloadCharacter(slotIndex, false);
									}
									m_characterIsLoading[slotIndex] = false;
									if (loadedCharacter.heroPrefabLink != null)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												if (!loadedCharacter.heroPrefabLink.IsEmpty)
												{
													while (true)
													{
														GameObject gameObject;
														ActorModelData component;
														Dictionary<int, string> animatorStateNameHashToNameMap;
														Animator componentInChildren2;
														Joint[] componentsInChildren;
														Joint[] array;
														Rigidbody[] componentsInChildren2;
														Rigidbody[] array2;
														switch (1)
														{
														case 0:
															break;
														default:
															{
																gameObject = loadedCharacter.heroPrefabLink.InstantiatePrefab();
																if (gameObject == null)
																{
																	throw new ApplicationException($"Failed to instantiate prefab for {characterLink.m_characterType} {visualInfo.ToString()}");
																}
																component = gameObject.GetComponent<ActorModelData>();
																bool flag = false;
																if (MasterSkinVfxData.Get() != null && MasterSkinVfxData.Get().m_addMasterSkinVfx)
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
																	if (characterLink.IsVisualInfoSelectionValid(visualInfo))
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
																		CharacterColor characterColor = characterLink.GetCharacterColor(visualInfo);
																		flag = (characterColor.m_styleLevel == StyleLevelType.Mastery);
																	}
																}
																if (flag)
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
																	MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(component.gameObject, characterLink.m_characterType, characterLink.m_loadScreenScale);
																}
																component.EnableRagdoll(false);
																animatorStateNameHashToNameMap = component.GetAnimatorStateNameHashToNameMap();
																gameObject.transform.position = m_ringAnimations[slotIndex].transform.position;
																gameObject.transform.SetParent(m_ringAnimations[slotIndex].GetContainerTransform());
																m_loadedCharacters[slotIndex].instantiatedCharacter = gameObject;
																m_loadedCharacters[slotIndex].uiActorModelData = gameObject.GetComponent<UIActorModelData>();
																m_loadedCharacters[slotIndex].resourceLink = characterLink;
																m_loadedCharacters[slotIndex].type = characterLink.m_characterType;
																m_loadedCharacters[slotIndex].skin = visualInfo;
																Vector3 loadScreenPosition = characterLink.m_loadScreenPosition;
																if (characterLink.m_loadScreenDistTowardsCamera != 0f)
																{
																	loadScreenPosition.x = 0f;
																	loadScreenPosition.z = 0f;
																}
																if (slotIndex > 0)
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
																	loadScreenPosition.y = Mathf.Min(loadScreenPosition.y, 0.15f);
																}
																gameObject.transform.localPosition = loadScreenPosition;
																gameObject.transform.localScale = new Vector3(characterLink.m_loadScreenScale, characterLink.m_loadScreenScale, characterLink.m_loadScreenScale);
																gameObject.transform.localRotation = Quaternion.identity;
																if (!prevCharacterLoaded && !isBot)
																{
																	if (m_loadedCharacters[slotIndex].ready)
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
																		m_ringAnimations[slotIndex].PlayAnimation("ReadyIn");
																	}
																	else
																	{
																		m_ringAnimations[slotIndex].PlayAnimation("TransitionIn");
																	}
																}
																GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
																if (gameObject2.GetComponent<FrontEndAnimationEventReceiver>() == null)
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
																	gameObject2.AddComponent<FrontEndAnimationEventReceiver>();
																}
																componentInChildren2 = gameObject.GetComponentInChildren<Animator>();
																if (componentInChildren2 != null)
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
																	if (componentInChildren2.isInitialized)
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
																		string name = componentInChildren2.name;
																		if (componentInChildren2.runtimeAnimatorController != null)
																		{
																			name = componentInChildren2.runtimeAnimatorController.name;
																		}
																		if (name == prevAnimInfoName)
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
																			if (prevAnimInCharSelState)
																			{
																				componentInChildren2.Play(prevAnimInfoStateHash, -1, prevAnimInfoNormalizedTime);
																			}
																		}
																		if (!(name != prevAnimInfoName))
																		{
																			if (prevAnimInCharSelState)
																			{
																				goto IL_058f;
																			}
																			while (true)
																			{
																				switch (6)
																				{
																				case 0:
																					continue;
																				}
																				break;
																			}
																		}
																		UIActorModelData.SetCharSelectTrigger(componentInChildren2, prevCharType != characterLink.m_characterType, true);
																		goto IL_058f;
																	}
																}
																goto IL_064a;
															}
															IL_058f:
															componentInChildren2.SetBool("DecisionPhase", !m_loadedCharacters[slotIndex].ready);
															if (ParamExists(componentInChildren2, "SkinIndex"))
															{
																componentInChildren2.SetInteger("SkinIndex", visualInfo.skinIndex);
															}
															if (ParamExists(componentInChildren2, "PatternIndex"))
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
																componentInChildren2.SetInteger("PatternIndex", visualInfo.patternIndex);
															}
															if (ParamExists(componentInChildren2, "ColorIndex"))
															{
																componentInChildren2.SetInteger("ColorIndex", visualInfo.colorIndex);
															}
															goto IL_064a;
															IL_064a:
															if (slotIndex == 0)
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
																if (ResetRotation)
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
																	UIFrontEnd.Get().ResetCharacterRotation();
																}
															}
															if (m_loadedCharacters[0].instantiatedCharacter != null)
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
																if (GetCharacterLookAtCamera() != null)
																{
																	if (m_characterLookAtLocation == null)
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
																		m_characterLookAtLocation = new GameObject();
																	}
																	Transform transform = m_characterLookAtLocation.transform;
																	Vector3 position = GetCharacterLookAtCamera().transform.position;
																	float x = position.x;
																	Vector3 position2 = m_loadedCharacters[0].instantiatedCharacter.transform.position;
																	float y = position2.y;
																	Vector3 position3 = GetCharacterLookAtCamera().transform.position;
																	transform.position = new Vector3(x, y, position3.z);
																	m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(m_characterLookAtLocation.transform);
																	m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles += UIFrontEnd.Get().GetRotationOffset();
																}
															}
															m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotIN");
															UnityEngine.Object.Destroy(component);
															componentsInChildren = gameObject.GetComponentsInChildren<Joint>(true);
															array = componentsInChildren;
															foreach (Joint obj in array)
															{
																UnityEngine.Object.Destroy(obj);
															}
															componentsInChildren2 = gameObject.GetComponentsInChildren<Rigidbody>(true);
															array2 = componentsInChildren2;
															foreach (Rigidbody obj2 in array2)
															{
																UnityEngine.Object.Destroy(obj2);
															}
															while (true)
															{
																switch (1)
																{
																case 0:
																	break;
																default:
																{
																	ChangeLayersRecursively(gameObject.transform, "UIInWorld");
																	Collider[] componentsInChildren3 = gameObject.GetComponentsInChildren<Collider>();
																	for (int k = 0; k < componentsInChildren3.Length; k++)
																	{
																		if (componentsInChildren3[k].name != "floor_collider")
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
																			componentsInChildren3[k].enabled = true;
																		}
																	}
																	while (true)
																	{
																		switch (1)
																		{
																		case 0:
																			break;
																		default:
																			m_loadedCharacters[slotIndex].uiActorModelData = gameObject.AddComponent<UIActorModelData>();
																			m_loadedCharacters[slotIndex].uiActorModelData.DelayEnablingOfShroudInstances();
																			m_loadedCharacters[slotIndex].uiActorModelData.SetStateNameHashToNameMap(animatorStateNameHashToNameMap);
																			if (characterLink.m_loadScreenDistTowardsCamera != 0f)
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
																				float num2 = characterLink.m_loadScreenDistTowardsCamera;
																				if (slotIndex > 0)
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
																					num2 = Mathf.Min(0.85f, num2);
																				}
																				m_loadedCharacters[slotIndex].uiActorModelData.m_setOffsetTowardsCamera = true;
																				m_loadedCharacters[slotIndex].uiActorModelData.m_offsetDistanceTowardsCamera = num2;
																				m_loadedCharacters[slotIndex].uiActorModelData.SetParentLocalPositionOffset();
																			}
																			else
																			{
																				m_ringAnimations[slotIndex].GetContainerTransform().localPosition = Vector3.zero;
																			}
																			if (slotIndex == 0)
																			{
																				while (true)
																				{
																					switch (2)
																					{
																					case 0:
																						break;
																					default:
																						UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectNotifyCharLoaded);
																						UICharacterSelectScreenController.Get().NotifyCharacterDoneLoading();
																						if (playSelectionChatterCue)
																						{
																							GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndSelectionChatterCue, null);
																						}
																						return;
																					}
																				}
																			}
																			return;
																		}
																	}
																}
																}
															}
														}
													}
												}
												return;
											}
										}
									}
									return;
								}
							}
						}
					});
				}
				return;
			}
		}
	}

	private bool ParamExists(Animator animator, string paramName)
	{
		for (int i = 0; i < animator.parameterCount; i++)
		{
			if (!(animator.parameters[i].name == paramName))
			{
				continue;
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
				return true;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return false;
		}
	}

	public bool IsVisible()
	{
		return m_visible;
	}

	public void SetVisible(bool isVisible)
	{
		if (m_visible != isVisible)
		{
			m_visible = isVisible;
			if (m_visible)
			{
				UIFrontEnd.Get().ResetCharacterRotation();
			}
			OnVisibleChange();
		}
	}

	protected abstract void OnVisibleChange();
}
