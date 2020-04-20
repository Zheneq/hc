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
		if (!(UIFrontEnd.Get() == null))
		{
			if (!(UICharacterSelectWorldObjects.Get() == null))
			{
				if (characterLink != null && characterLink.m_characterType == this.m_loadedCharacters[slotIndex].type)
				{
					if (visualInfo.Equals(this.m_loadedCharacters[slotIndex].skin))
					{
						if (this.m_loadedCharacters[slotIndex].instantiatedCharacter != null)
						{
							Animator componentInChildren = this.m_loadedCharacters[slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
							UIActorModelData.SetCharSelectTrigger(componentInChildren, false, false);
						}
						return;
					}
				}
				if (characterLink != null)
				{
					if (!characterLink.IsVisualInfoSelectionValid(visualInfo))
					{
						Log.Error(string.Format("Character {0} could not find Actor Skin resource link for {1}", characterLink.m_displayName, visualInfo.ToString()), new object[0]);
						visualInfo = default(CharacterVisualInfo);
					}
				}
				GameObject instantiatedCharacter = this.m_loadedCharacters[slotIndex].instantiatedCharacter;
				bool prevCharacterLoaded = (instantiatedCharacter != null);
				string prevAnimInfoName = string.Empty;
				float prevAnimInfoNormalizedTime = 0f;
				int prevAnimInfoStateHash = 0;
				bool prevAnimInCharSelState = false;
				if (prevCharacterLoaded)
				{
					Animator componentInChildren2 = instantiatedCharacter.GetComponentInChildren<Animator>();
					AnimatorStateInfo currentAnimatorStateInfo = componentInChildren2.GetCurrentAnimatorStateInfo(0);
					prevAnimInfoName = componentInChildren2.name;
					if (componentInChildren2.runtimeAnimatorController != null)
					{
						prevAnimInfoName = componentInChildren2.runtimeAnimatorController.name;
					}
					prevAnimInfoStateHash = currentAnimatorStateInfo.fullPathHash;
					prevAnimInfoNormalizedTime = currentAnimatorStateInfo.normalizedTime;
					prevAnimInCharSelState = UIActorModelData.IsInCharSelectAnimState(componentInChildren2);
				}
				bool ResetRotation = false;
				if (slotIndex == 0)
				{
					if (characterLink != null)
					{
						if (this.m_loadedCharacters[slotIndex].type != characterLink.m_characterType)
						{
							ResetRotation = true;
						}
					}
				}
				CharacterType prevCharType = CharacterType.None;
				if (this.m_loadedCharacters[slotIndex].instantiatedCharacter != null)
				{
					prevCharType = this.m_loadedCharacters[slotIndex].type;
				}
				bool preUnloadChar;
				if (!(characterLink == null))
				{
					if (this.m_loadedCharacters[slotIndex].type == characterLink.m_characterType)
					{
						preUnloadChar = (this.m_loadedCharacters[slotIndex].skin.skinIndex != visualInfo.skinIndex);
						goto IL_390;
					}
				}
				preUnloadChar = true;
				IL_390:
				if (preUnloadChar)
				{
					this.UnloadCharacter(slotIndex, !prevCharacterLoaded || characterLink == null);
				}
				if (characterLink == null)
				{
					this.m_ringAnimations[slotIndex].PlayAnimation("ReadyOut");
				}
				if (characterLink != null)
				{
					this.m_loadedCharacters[slotIndex].resourceLink = characterLink;
					this.m_loadedCharacters[slotIndex].type = characterLink.m_characterType;
					this.m_loadedCharacters[slotIndex].skin = visualInfo;
					this.m_characterIsLoading[slotIndex] = true;
					if (AsyncManager.Get() != null)
					{
						characterLink.LoadAsync(visualInfo, out this.m_loadedCharacters[slotIndex].loadingTicket, (float)slotIndex * 0.1f, delegate(LoadedCharacterSelection loadedCharacter)
						{
							if (!(UIFrontEnd.Get() == null))
							{
								if (UIManager.Get().CurrentState != UIManager.ClientState.InGame)
								{
									if (!preUnloadChar)
									{
										int slotIndex2 = slotIndex;
										bool playAnimation;
										if (prevCharacterLoaded)
										{
											playAnimation = (characterLink == null);
										}
										else
										{
											playAnimation = true;
										}
										this.UnloadCharacter(slotIndex2, playAnimation);
									}
									else if (this.m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null)
									{
										UIManager.SetGameObjectActive(this.m_ringAnimations[slotIndex].m_charSelectSpawnVFX, true, null);
									}
									this.m_loadedCharacters[slotIndex].loadingTicket = -1;
									if (!this.m_characterIsLoading[slotIndex])
									{
										this.UnloadCharacter(slotIndex, false);
									}
									this.m_characterIsLoading[slotIndex] = false;
									if (loadedCharacter.heroPrefabLink != null)
									{
										if (!loadedCharacter.heroPrefabLink.IsEmpty)
										{
											GameObject gameObject = loadedCharacter.heroPrefabLink.InstantiatePrefab(false);
											if (gameObject == null)
											{
												throw new ApplicationException(string.Format("Failed to instantiate prefab for {0} {1}", characterLink.m_characterType, visualInfo.ToString()));
											}
											ActorModelData component = gameObject.GetComponent<ActorModelData>();
											bool flag = false;
											if (MasterSkinVfxData.Get() != null && MasterSkinVfxData.Get().m_addMasterSkinVfx)
											{
												if (characterLink.IsVisualInfoSelectionValid(visualInfo))
												{
													CharacterColor characterColor = characterLink.GetCharacterColor(visualInfo);
													flag = (characterColor.m_styleLevel == StyleLevelType.Mastery);
												}
											}
											if (flag)
											{
												MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(component.gameObject, characterLink.m_characterType, characterLink.m_loadScreenScale);
											}
											component.EnableRagdoll(false, null);
											Dictionary<int, string> animatorStateNameHashToNameMap = component.GetAnimatorStateNameHashToNameMap();
											gameObject.transform.position = this.m_ringAnimations[slotIndex].transform.position;
											gameObject.transform.SetParent(this.m_ringAnimations[slotIndex].GetContainerTransform());
											this.m_loadedCharacters[slotIndex].instantiatedCharacter = gameObject;
											this.m_loadedCharacters[slotIndex].uiActorModelData = gameObject.GetComponent<UIActorModelData>();
											this.m_loadedCharacters[slotIndex].resourceLink = characterLink;
											this.m_loadedCharacters[slotIndex].type = characterLink.m_characterType;
											this.m_loadedCharacters[slotIndex].skin = visualInfo;
											Vector3 loadScreenPosition = characterLink.m_loadScreenPosition;
											if (characterLink.m_loadScreenDistTowardsCamera != 0f)
											{
												loadScreenPosition.x = 0f;
												loadScreenPosition.z = 0f;
											}
											if (slotIndex > 0)
											{
												loadScreenPosition.y = Mathf.Min(loadScreenPosition.y, 0.15f);
											}
											gameObject.transform.localPosition = loadScreenPosition;
											gameObject.transform.localScale = new Vector3(characterLink.m_loadScreenScale, characterLink.m_loadScreenScale, characterLink.m_loadScreenScale);
											gameObject.transform.localRotation = Quaternion.identity;
											if (!prevCharacterLoaded && !isBot)
											{
												if (this.m_loadedCharacters[slotIndex].ready)
												{
													this.m_ringAnimations[slotIndex].PlayAnimation("ReadyIn");
												}
												else
												{
													this.m_ringAnimations[slotIndex].PlayAnimation("TransitionIn");
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
													if (name == prevAnimInfoName)
													{
														if (prevAnimInCharSelState)
														{
															componentInChildren3.Play(prevAnimInfoStateHash, -1, prevAnimInfoNormalizedTime);
														}
													}
													if (!(name != prevAnimInfoName))
													{
														if (prevAnimInCharSelState)
														{
															goto IL_58F;
														}
													}
													UIActorModelData.SetCharSelectTrigger(componentInChildren3, prevCharType != characterLink.m_characterType, true);
													IL_58F:
													componentInChildren3.SetBool("DecisionPhase", !this.m_loadedCharacters[slotIndex].ready);
													if (this.ParamExists(componentInChildren3, "SkinIndex"))
													{
														componentInChildren3.SetInteger("SkinIndex", visualInfo.skinIndex);
													}
													if (this.ParamExists(componentInChildren3, "PatternIndex"))
													{
														componentInChildren3.SetInteger("PatternIndex", visualInfo.patternIndex);
													}
													if (this.ParamExists(componentInChildren3, "ColorIndex"))
													{
														componentInChildren3.SetInteger("ColorIndex", visualInfo.colorIndex);
													}
												}
											}
											if (slotIndex == 0)
											{
												if (ResetRotation)
												{
													UIFrontEnd.Get().ResetCharacterRotation();
												}
											}
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
											this.m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotIN");
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
											this.ChangeLayersRecursively(gameObject.transform, "UIInWorld");
											Collider[] componentsInChildren3 = gameObject.GetComponentsInChildren<Collider>();
											for (int k = 0; k < componentsInChildren3.Length; k++)
											{
												if (componentsInChildren3[k].name != "floor_collider")
												{
													componentsInChildren3[k].enabled = true;
												}
											}
											this.m_loadedCharacters[slotIndex].uiActorModelData = gameObject.AddComponent<UIActorModelData>();
											this.m_loadedCharacters[slotIndex].uiActorModelData.DelayEnablingOfShroudInstances();
											this.m_loadedCharacters[slotIndex].uiActorModelData.SetStateNameHashToNameMap(animatorStateNameHashToNameMap);
											if (characterLink.m_loadScreenDistTowardsCamera != 0f)
											{
												float num = characterLink.m_loadScreenDistTowardsCamera;
												if (slotIndex > 0)
												{
													num = Mathf.Min(0.85f, num);
												}
												this.m_loadedCharacters[slotIndex].uiActorModelData.m_setOffsetTowardsCamera = true;
												this.m_loadedCharacters[slotIndex].uiActorModelData.m_offsetDistanceTowardsCamera = num;
												this.m_loadedCharacters[slotIndex].uiActorModelData.SetParentLocalPositionOffset();
											}
											else
											{
												this.m_ringAnimations[slotIndex].GetContainerTransform().localPosition = Vector3.zero;
											}
											if (slotIndex == 0)
											{
												UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectNotifyCharLoaded);
												UICharacterSelectScreenController.Get().NotifyCharacterDoneLoading();
												if (playSelectionChatterCue)
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
