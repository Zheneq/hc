using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UICharacterWorldObjects : MonoBehaviour
{
	private bool m_shuttingDown;

	private GameObject m_characterLookAtLocation;

	public UICharacterSelectRing[] m_ringAnimations;

	private bool[] m_characterIsLoading;

	private UICharacterWorldObjects.LoadedCharacter[] m_loadedCharacters;

	private bool m_visible;

	protected Camera GetCharacterLookAtCamera()
	{
		return UIManager.Get().GetEnvirontmentCamera();
	}

	protected void Initialize()
	{
		this.m_shuttingDown = false;
		this.m_visible = true;
		this.SetVisible(false);
		this.m_loadedCharacters = new UICharacterWorldObjects.LoadedCharacter[this.m_ringAnimations.Length];
		this.m_characterIsLoading = new bool[this.m_ringAnimations.Length];
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			this.m_loadedCharacters[i].Clear();
		}
		if (this.m_loadedCharacters[0].instantiatedCharacter != null)
		{
			if (this.m_characterLookAtLocation == null)
			{
				this.m_characterLookAtLocation = new GameObject();
			}
			this.m_characterLookAtLocation.transform.position = new Vector3(this.GetCharacterLookAtCamera().transform.position.x, this.m_loadedCharacters[0].instantiatedCharacter.transform.position.y, this.GetCharacterLookAtCamera().transform.position.z);
			this.m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(this.m_characterLookAtLocation.transform);
		}
	}

	private void OnDestroy()
	{
		this.m_shuttingDown = true;
		this.UnloadAllCharacters();
	}

	private void Update()
	{
		if (this.m_loadedCharacters[0].instantiatedCharacter != null)
		{
			if (this.GetCharacterLookAtCamera() != null)
			{
				if (this.m_characterLookAtLocation == null)
				{
					this.m_characterLookAtLocation = new GameObject();
				}
				this.m_characterLookAtLocation.transform.position = new Vector3(this.GetCharacterLookAtCamera().transform.position.x, this.m_loadedCharacters[0].instantiatedCharacter.transform.position.y, this.GetCharacterLookAtCamera().transform.position.z);
				this.m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(this.m_characterLookAtLocation.transform);
				this.m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles += UIFrontEnd.Get().GetRotationOffset();
			}
		}
	}

	public bool IsCharReady(int index)
	{
		return this.m_loadedCharacters[index].ready;
	}

	public void CheckReadyBand(int index, bool isReady)
	{
		this.m_ringAnimations[index].CheckReadyBand(isReady);
	}

	public void SetCharacterReady(int index, bool isReady)
	{
		if (this.m_loadedCharacters[index].ready != isReady)
		{
			if (isReady)
			{
				this.m_ringAnimations[index].PlayAnimation("ReadyIn");
			}
			else
			{
				this.m_ringAnimations[index].PlayAnimation("ReadyOut");
			}
			this.m_loadedCharacters[index].ready = isReady;
		}
	}

	public void SetCharacterInGame(int index, bool isInGame)
	{
		this.m_loadedCharacters[index].isInGame = isInGame;
	}

	public bool CharacterIsLoading()
	{
		return this.m_characterIsLoading[0];
	}

	public int GetNumLoadedCharacters()
	{
		int num = 0;
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			if (this.m_loadedCharacters[i].uiActorModelData != null)
			{
				num++;
			}
		}
		return num;
	}

	public void SetReadyPose()
	{
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			if (this.m_loadedCharacters[i].uiActorModelData != null)
			{
				this.m_loadedCharacters[i].uiActorModelData.SetReady(this.m_loadedCharacters[i].ready);
			}
		}
	}

	public void SetSkins()
	{
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			if (this.m_loadedCharacters[i].uiActorModelData != null)
			{
				this.m_loadedCharacters[i].uiActorModelData.SetSkin(this.m_loadedCharacters[i].skin);
			}
		}
	}

	public void SetIsInGameRings()
	{
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			Component isInGameAnimation = this.m_ringAnimations[i].m_isInGameAnimation;
			bool doActive;
			if (this.m_loadedCharacters[i].isInGame)
			{
				doActive = !this.m_loadedCharacters[i].ready;
			}
			else
			{
				doActive = false;
			}
			UIManager.SetGameObjectActive(isInGameAnimation, doActive, null);
		}
		for (int j = this.m_loadedCharacters.Length; j < this.m_ringAnimations.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_ringAnimations[j].m_isInGameAnimation, false, null);
		}
	}

	public void UnloadAllCharacters()
	{
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			this.UnloadCharacter(i, true);
		}
	}

	public void UnloadCharacter(int slotIndex, bool playAnimation = true)
	{
		if (this.m_loadedCharacters[slotIndex].loadingTicket != -1)
		{
			this.m_loadedCharacters[slotIndex].resourceLink.CancelLoad(this.m_loadedCharacters[slotIndex].skin, this.m_loadedCharacters[slotIndex].loadingTicket);
		}
		if (this.m_loadedCharacters[slotIndex].instantiatedCharacter != null)
		{
			UnityEngine.Object.Destroy(this.m_loadedCharacters[slotIndex].instantiatedCharacter);
			if (playAnimation)
			{
				if (!this.m_shuttingDown)
				{
					this.m_ringAnimations[slotIndex].PlayAnimation("TransitionOut");
				}
			}
		}
		if (playAnimation)
		{
			if (!this.m_shuttingDown)
			{
				this.m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotOUT");
			}
		}
		this.m_loadedCharacters[slotIndex].Clear();
		if (this.m_ringAnimations[slotIndex].m_isInGameAnimation != null)
		{
			UIManager.SetGameObjectActive(this.m_ringAnimations[slotIndex].m_isInGameAnimation, false, null);
		}
		if (this.m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null)
		{
			UIManager.SetGameObjectActive(this.m_ringAnimations[slotIndex].m_charSelectSpawnVFX, false, null);
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
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform != trans.gameObject)
				{
					this.ChangeLayersRecursively(transform, name);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public CharacterType CharacterTypeInSlot(int slotIndex)
	{
		return this.m_loadedCharacters[slotIndex].type;
	}

	public CharacterResourceLink CharacterResourceLinkInSlot(int slotIndex)
	{
		return this.m_loadedCharacters[slotIndex].resourceLink;
	}

	public CharacterVisualInfo CharacterVisualInfoInSlot(int slotIndex)
	{
		return this.m_loadedCharacters[slotIndex].skin;
	}

	public void SetCharSelectTriggerForSlot(CharacterResourceLink characterLink, int slotIndex)
	{
		if (characterLink != null)
		{
			if (characterLink.m_characterType == this.m_loadedCharacters[slotIndex].type)
			{
				if (this.m_loadedCharacters[slotIndex].instantiatedCharacter != null)
				{
					Animator componentInChildren = this.m_loadedCharacters[slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
					UIActorModelData.SetCharSelectTrigger(componentInChildren, false, false);
				}
				return;
			}
		}
	}

	public void LoadCharacterIntoSlot(CharacterType character, int slotIndex, string characterName, CharacterVisualInfo skinSelector, bool isBot)
	{
		this.LoadCharacterIntoSlot(GameWideData.Get().GetCharacterResourceLink(character), slotIndex, characterName, skinSelector, isBot, false);
	}

	public void LoadCharacterIntoSlot(CharacterResourceLink characterLink, int slotIndex, string characterName, CharacterVisualInfo visualInfo, bool isBot, bool playSelectionChatterCue)
	{
		UICharacterWorldObjects.LoadCharacterIntoSlot_c__AnonStorey0 LoadCharacterIntoSlot_c__AnonStorey = new UICharacterWorldObjects.LoadCharacterIntoSlot_c__AnonStorey0();
		LoadCharacterIntoSlot_c__AnonStorey.slotIndex = slotIndex;
		LoadCharacterIntoSlot_c__AnonStorey.characterLink = characterLink;
		LoadCharacterIntoSlot_c__AnonStorey.visualInfo = visualInfo;
		LoadCharacterIntoSlot_c__AnonStorey.isBot = isBot;
		LoadCharacterIntoSlot_c__AnonStorey.playSelectionChatterCue = playSelectionChatterCue;
		LoadCharacterIntoSlot_c__AnonStorey._this = this;
		if (!(UIFrontEnd.Get() == null))
		{
			if (!(UICharacterSelectWorldObjects.Get() == null))
			{
				if (LoadCharacterIntoSlot_c__AnonStorey.characterLink != null && LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType == this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].type)
				{
					if (LoadCharacterIntoSlot_c__AnonStorey.visualInfo.Equals(this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].skin))
					{
						if (this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].instantiatedCharacter != null)
						{
							Animator componentInChildren = this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
							UIActorModelData.SetCharSelectTrigger(componentInChildren, false, false);
						}
						return;
					}
				}
				if (LoadCharacterIntoSlot_c__AnonStorey.characterLink != null)
				{
					if (!LoadCharacterIntoSlot_c__AnonStorey.characterLink.IsVisualInfoSelectionValid(LoadCharacterIntoSlot_c__AnonStorey.visualInfo))
					{
						Log.Error(string.Format("Character {0} could not find Actor Skin resource link for {1}", LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_displayName, LoadCharacterIntoSlot_c__AnonStorey.visualInfo.ToString()), new object[0]);
						LoadCharacterIntoSlot_c__AnonStorey.visualInfo = default(CharacterVisualInfo);
					}
				}
				GameObject instantiatedCharacter = this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].instantiatedCharacter;
				LoadCharacterIntoSlot_c__AnonStorey.prevCharacterLoaded = (instantiatedCharacter != null);
				LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoName = string.Empty;
				LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoNormalizedTime = 0f;
				LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoStateHash = 0;
				LoadCharacterIntoSlot_c__AnonStorey.prevAnimInCharSelState = false;
				if (LoadCharacterIntoSlot_c__AnonStorey.prevCharacterLoaded)
				{
					Animator componentInChildren2 = instantiatedCharacter.GetComponentInChildren<Animator>();
					AnimatorStateInfo currentAnimatorStateInfo = componentInChildren2.GetCurrentAnimatorStateInfo(0);
					LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoName = componentInChildren2.name;
					if (componentInChildren2.runtimeAnimatorController != null)
					{
						LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoName = componentInChildren2.runtimeAnimatorController.name;
					}
					LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoStateHash = currentAnimatorStateInfo.fullPathHash;
					LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoNormalizedTime = currentAnimatorStateInfo.normalizedTime;
					LoadCharacterIntoSlot_c__AnonStorey.prevAnimInCharSelState = UIActorModelData.IsInCharSelectAnimState(componentInChildren2);
				}
				LoadCharacterIntoSlot_c__AnonStorey.ResetRotation = false;
				if (LoadCharacterIntoSlot_c__AnonStorey.slotIndex == 0)
				{
					if (LoadCharacterIntoSlot_c__AnonStorey.characterLink != null)
					{
						if (this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].type != LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType)
						{
							LoadCharacterIntoSlot_c__AnonStorey.ResetRotation = true;
						}
					}
				}
				LoadCharacterIntoSlot_c__AnonStorey.prevCharType = CharacterType.None;
				if (this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].instantiatedCharacter != null)
				{
					LoadCharacterIntoSlot_c__AnonStorey.prevCharType = this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].type;
				}
				UICharacterWorldObjects.LoadCharacterIntoSlot_c__AnonStorey0 LoadCharacterIntoSlot_c__AnonStorey2 = LoadCharacterIntoSlot_c__AnonStorey;
				bool preUnloadChar;
				if (!(LoadCharacterIntoSlot_c__AnonStorey.characterLink == null))
				{
					if (this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].type == LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType)
					{
						preUnloadChar = (this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].skin.skinIndex != LoadCharacterIntoSlot_c__AnonStorey.visualInfo.skinIndex);
						goto IL_390;
					}
				}
				preUnloadChar = true;
				IL_390:
				LoadCharacterIntoSlot_c__AnonStorey2.preUnloadChar = preUnloadChar;
				if (LoadCharacterIntoSlot_c__AnonStorey.preUnloadChar)
				{
					this.UnloadCharacter(LoadCharacterIntoSlot_c__AnonStorey.slotIndex, !LoadCharacterIntoSlot_c__AnonStorey.prevCharacterLoaded || LoadCharacterIntoSlot_c__AnonStorey.characterLink == null);
				}
				if (LoadCharacterIntoSlot_c__AnonStorey.characterLink == null)
				{
					this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].PlayAnimation("ReadyOut");
				}
				if (LoadCharacterIntoSlot_c__AnonStorey.characterLink != null)
				{
					this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].resourceLink = LoadCharacterIntoSlot_c__AnonStorey.characterLink;
					this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].type = LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType;
					this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].skin = LoadCharacterIntoSlot_c__AnonStorey.visualInfo;
					this.m_characterIsLoading[LoadCharacterIntoSlot_c__AnonStorey.slotIndex] = true;
					if (AsyncManager.Get() != null)
					{
						LoadCharacterIntoSlot_c__AnonStorey.characterLink.LoadAsync(LoadCharacterIntoSlot_c__AnonStorey.visualInfo, out this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].loadingTicket, (float)LoadCharacterIntoSlot_c__AnonStorey.slotIndex * 0.1f, delegate(LoadedCharacterSelection loadedCharacter)
						{
							if (!(UIFrontEnd.Get() == null))
							{
								if (UIManager.Get().CurrentState != UIManager.ClientState.InGame)
								{
									if (!LoadCharacterIntoSlot_c__AnonStorey.preUnloadChar)
									{
										UICharacterWorldObjects _this = LoadCharacterIntoSlot_c__AnonStorey._this;
										int slotIndex2 = LoadCharacterIntoSlot_c__AnonStorey.slotIndex;
										bool playAnimation;
										if (LoadCharacterIntoSlot_c__AnonStorey.prevCharacterLoaded)
										{
											playAnimation = (LoadCharacterIntoSlot_c__AnonStorey.characterLink == null);
										}
										else
										{
											playAnimation = true;
										}
										_this.UnloadCharacter(slotIndex2, playAnimation);
									}
									else if (LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].m_charSelectSpawnVFX != null)
									{
										UIManager.SetGameObjectActive(LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].m_charSelectSpawnVFX, true, null);
									}
									LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].loadingTicket = -1;
									if (!LoadCharacterIntoSlot_c__AnonStorey._this.m_characterIsLoading[LoadCharacterIntoSlot_c__AnonStorey.slotIndex])
									{
										LoadCharacterIntoSlot_c__AnonStorey._this.UnloadCharacter(LoadCharacterIntoSlot_c__AnonStorey.slotIndex, false);
									}
									LoadCharacterIntoSlot_c__AnonStorey._this.m_characterIsLoading[LoadCharacterIntoSlot_c__AnonStorey.slotIndex] = false;
									if (loadedCharacter.heroPrefabLink != null)
									{
										if (!loadedCharacter.heroPrefabLink.IsEmpty)
										{
											GameObject gameObject = loadedCharacter.heroPrefabLink.InstantiatePrefab(false);
											if (gameObject == null)
											{
												throw new ApplicationException(string.Format("Failed to instantiate prefab for {0} {1}", LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType, LoadCharacterIntoSlot_c__AnonStorey.visualInfo.ToString()));
											}
											ActorModelData component = gameObject.GetComponent<ActorModelData>();
											bool flag = false;
											if (MasterSkinVfxData.Get() != null && MasterSkinVfxData.Get().m_addMasterSkinVfx)
											{
												if (LoadCharacterIntoSlot_c__AnonStorey.characterLink.IsVisualInfoSelectionValid(LoadCharacterIntoSlot_c__AnonStorey.visualInfo))
												{
													CharacterColor characterColor = LoadCharacterIntoSlot_c__AnonStorey.characterLink.GetCharacterColor(LoadCharacterIntoSlot_c__AnonStorey.visualInfo);
													flag = (characterColor.m_styleLevel == StyleLevelType.Mastery);
												}
											}
											if (flag)
											{
												MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(component.gameObject, LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType, LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenScale);
											}
											component.EnableRagdoll(false, null);
											Dictionary<int, string> animatorStateNameHashToNameMap = component.GetAnimatorStateNameHashToNameMap();
											gameObject.transform.position = LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].transform.position;
											gameObject.transform.SetParent(LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].GetContainerTransform());
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].instantiatedCharacter = gameObject;
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData = gameObject.GetComponent<UIActorModelData>();
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].resourceLink = LoadCharacterIntoSlot_c__AnonStorey.characterLink;
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].type = LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType;
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].skin = LoadCharacterIntoSlot_c__AnonStorey.visualInfo;
											Vector3 loadScreenPosition = LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenPosition;
											if (LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenDistTowardsCamera != 0f)
											{
												loadScreenPosition.x = 0f;
												loadScreenPosition.z = 0f;
											}
											if (LoadCharacterIntoSlot_c__AnonStorey.slotIndex > 0)
											{
												loadScreenPosition.y = Mathf.Min(loadScreenPosition.y, 0.15f);
											}
											gameObject.transform.localPosition = loadScreenPosition;
											gameObject.transform.localScale = new Vector3(LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenScale, LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenScale, LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenScale);
											gameObject.transform.localRotation = Quaternion.identity;
											if (!LoadCharacterIntoSlot_c__AnonStorey.prevCharacterLoaded && !LoadCharacterIntoSlot_c__AnonStorey.isBot)
											{
												if (LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].ready)
												{
													LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].PlayAnimation("ReadyIn");
												}
												else
												{
													LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].PlayAnimation("TransitionIn");
												}
											}
											GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
											if (gameObject2.GetComponent<FrontEndAnimationEventReceiver>() == null)
											{
												gameObject2.AddComponent<FrontEndAnimationEventReceiver>();
											}
											Animator componentInChildren3 = gameObject.GetComponentInChildren<Animator>();
											if (componentInChildren3 != null)
											{
												if (componentInChildren3.isInitialized)
												{
													string name = componentInChildren3.name;
													if (componentInChildren3.runtimeAnimatorController != null)
													{
														name = componentInChildren3.runtimeAnimatorController.name;
													}
													if (name == LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoName)
													{
														if (LoadCharacterIntoSlot_c__AnonStorey.prevAnimInCharSelState)
														{
															componentInChildren3.Play(LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoStateHash, -1, LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoNormalizedTime);
														}
													}
													if (!(name != LoadCharacterIntoSlot_c__AnonStorey.prevAnimInfoName))
													{
														if (LoadCharacterIntoSlot_c__AnonStorey.prevAnimInCharSelState)
														{
															goto IL_58F;
														}
													}
													UIActorModelData.SetCharSelectTrigger(componentInChildren3, LoadCharacterIntoSlot_c__AnonStorey.prevCharType != LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_characterType, true);
													IL_58F:
													componentInChildren3.SetBool("DecisionPhase", !LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].ready);
													if (LoadCharacterIntoSlot_c__AnonStorey._this.ParamExists(componentInChildren3, "SkinIndex"))
													{
														componentInChildren3.SetInteger("SkinIndex", LoadCharacterIntoSlot_c__AnonStorey.visualInfo.skinIndex);
													}
													if (LoadCharacterIntoSlot_c__AnonStorey._this.ParamExists(componentInChildren3, "PatternIndex"))
													{
														componentInChildren3.SetInteger("PatternIndex", LoadCharacterIntoSlot_c__AnonStorey.visualInfo.patternIndex);
													}
													if (LoadCharacterIntoSlot_c__AnonStorey._this.ParamExists(componentInChildren3, "ColorIndex"))
													{
														componentInChildren3.SetInteger("ColorIndex", LoadCharacterIntoSlot_c__AnonStorey.visualInfo.colorIndex);
													}
												}
											}
											if (LoadCharacterIntoSlot_c__AnonStorey.slotIndex == 0)
											{
												if (LoadCharacterIntoSlot_c__AnonStorey.ResetRotation)
												{
													UIFrontEnd.Get().ResetCharacterRotation();
												}
											}
											if (LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[0].instantiatedCharacter != null)
											{
												if (LoadCharacterIntoSlot_c__AnonStorey._this.GetCharacterLookAtCamera() != null)
												{
													if (LoadCharacterIntoSlot_c__AnonStorey._this.m_characterLookAtLocation == null)
													{
														LoadCharacterIntoSlot_c__AnonStorey._this.m_characterLookAtLocation = new GameObject();
													}
													LoadCharacterIntoSlot_c__AnonStorey._this.m_characterLookAtLocation.transform.position = new Vector3(LoadCharacterIntoSlot_c__AnonStorey._this.GetCharacterLookAtCamera().transform.position.x, LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[0].instantiatedCharacter.transform.position.y, LoadCharacterIntoSlot_c__AnonStorey._this.GetCharacterLookAtCamera().transform.position.z);
													LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(LoadCharacterIntoSlot_c__AnonStorey._this.m_characterLookAtLocation.transform);
													LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles += UIFrontEnd.Get().GetRotationOffset();
												}
											}
											LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].PlayBaseObjectAnimation("SlotIN");
											UnityEngine.Object.Destroy(component);
											Joint[] componentsInChildren = gameObject.GetComponentsInChildren<Joint>(true);
											foreach (Joint obj in componentsInChildren)
											{
												UnityEngine.Object.Destroy(obj);
											}
											Rigidbody[] componentsInChildren2 = gameObject.GetComponentsInChildren<Rigidbody>(true);
											foreach (Rigidbody obj2 in componentsInChildren2)
											{
												UnityEngine.Object.Destroy(obj2);
											}
											LoadCharacterIntoSlot_c__AnonStorey._this.ChangeLayersRecursively(gameObject.transform, "UIInWorld");
											Collider[] componentsInChildren3 = gameObject.GetComponentsInChildren<Collider>();
											for (int k = 0; k < componentsInChildren3.Length; k++)
											{
												if (componentsInChildren3[k].name != "floor_collider")
												{
													componentsInChildren3[k].enabled = true;
												}
											}
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData = gameObject.AddComponent<UIActorModelData>();
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData.DelayEnablingOfShroudInstances();
											LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData.SetStateNameHashToNameMap(animatorStateNameHashToNameMap);
											if (LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenDistTowardsCamera != 0f)
											{
												float num = LoadCharacterIntoSlot_c__AnonStorey.characterLink.m_loadScreenDistTowardsCamera;
												if (LoadCharacterIntoSlot_c__AnonStorey.slotIndex > 0)
												{
													num = Mathf.Min(0.85f, num);
												}
												LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData.m_setOffsetTowardsCamera = true;
												LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData.m_offsetDistanceTowardsCamera = num;
												LoadCharacterIntoSlot_c__AnonStorey._this.m_loadedCharacters[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].uiActorModelData.SetParentLocalPositionOffset();
											}
											else
											{
												LoadCharacterIntoSlot_c__AnonStorey._this.m_ringAnimations[LoadCharacterIntoSlot_c__AnonStorey.slotIndex].GetContainerTransform().localPosition = Vector3.zero;
											}
											if (LoadCharacterIntoSlot_c__AnonStorey.slotIndex == 0)
											{
												UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectNotifyCharLoaded);
												UICharacterSelectScreenController.Get().NotifyCharacterDoneLoading();
												if (LoadCharacterIntoSlot_c__AnonStorey.playSelectionChatterCue)
												{
													GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndSelectionChatterCue, null);
												}
											}
										}
									}
									return;
								}
							}
						});
					}
				}
				return;
			}
		}
	}

	private bool ParamExists(Animator animator, string paramName)
	{
		for (int i = 0; i < animator.parameterCount; i++)
		{
			if (animator.parameters[i].name == paramName)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsVisible()
	{
		return this.m_visible;
	}

	public void SetVisible(bool isVisible)
	{
		if (this.m_visible == isVisible)
		{
			return;
		}
		this.m_visible = isVisible;
		if (this.m_visible)
		{
			UIFrontEnd.Get().ResetCharacterRotation();
		}
		this.OnVisibleChange();
	}

	protected abstract void OnVisibleChange();

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
			this.type = CharacterType.None;
			this.skin = default(CharacterVisualInfo);
			this.instantiatedCharacter = null;
			this.loadingTicket = -1;
			this.uiActorModelData = null;
			this.isInGame = false;
			this.resourceLink = null;
		}
	}
}
