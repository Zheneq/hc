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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.Initialize()).MethodHandle;
		}
		if (this.m_loadedCharacters[0].instantiatedCharacter != null)
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
			if (this.m_characterLookAtLocation == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.Update()).MethodHandle;
			}
			if (this.GetCharacterLookAtCamera() != null)
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
				if (this.m_characterLookAtLocation == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.SetCharacterReady(int, bool)).MethodHandle;
			}
			if (isReady)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.GetNumLoadedCharacters()).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.SetReadyPose()).MethodHandle;
				}
				this.m_loadedCharacters[i].uiActorModelData.SetReady(this.m_loadedCharacters[i].ready);
			}
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
	}

	public void SetSkins()
	{
		for (int i = 0; i < this.m_loadedCharacters.Length; i++)
		{
			if (this.m_loadedCharacters[i].uiActorModelData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.SetSkins()).MethodHandle;
				}
				this.m_loadedCharacters[i].uiActorModelData.SetSkin(this.m_loadedCharacters[i].skin);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.SetIsInGameRings()).MethodHandle;
				}
				doActive = !this.m_loadedCharacters[i].ready;
			}
			else
			{
				doActive = false;
			}
			UIManager.SetGameObjectActive(isInGameAnimation, doActive, null);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.UnloadAllCharacters()).MethodHandle;
		}
	}

	public void UnloadCharacter(int slotIndex, bool playAnimation = true)
	{
		if (this.m_loadedCharacters[slotIndex].loadingTicket != -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.UnloadCharacter(int, bool)).MethodHandle;
			}
			this.m_loadedCharacters[slotIndex].resourceLink.CancelLoad(this.m_loadedCharacters[slotIndex].skin, this.m_loadedCharacters[slotIndex].loadingTicket);
		}
		if (this.m_loadedCharacters[slotIndex].instantiatedCharacter != null)
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
			UnityEngine.Object.Destroy(this.m_loadedCharacters[slotIndex].instantiatedCharacter);
			if (playAnimation)
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
				if (!this.m_shuttingDown)
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
					this.m_ringAnimations[slotIndex].PlayAnimation("TransitionOut");
				}
			}
		}
		if (playAnimation)
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
			if (!this.m_shuttingDown)
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
				this.m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotOUT");
			}
		}
		this.m_loadedCharacters[slotIndex].Clear();
		if (this.m_ringAnimations[slotIndex].m_isInGameAnimation != null)
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
			UIManager.SetGameObjectActive(this.m_ringAnimations[slotIndex].m_isInGameAnimation, false, null);
		}
		if (this.m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.ChangeLayersRecursively(Transform, string)).MethodHandle;
					}
					this.ChangeLayersRecursively(transform, name);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.SetCharSelectTriggerForSlot(CharacterResourceLink, int)).MethodHandle;
			}
			if (characterLink.m_characterType == this.m_loadedCharacters[slotIndex].type)
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
				if (this.m_loadedCharacters[slotIndex].instantiatedCharacter != null)
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
		UICharacterWorldObjects.<LoadCharacterIntoSlot>c__AnonStorey0 <LoadCharacterIntoSlot>c__AnonStorey = new UICharacterWorldObjects.<LoadCharacterIntoSlot>c__AnonStorey0();
		<LoadCharacterIntoSlot>c__AnonStorey.slotIndex = slotIndex;
		<LoadCharacterIntoSlot>c__AnonStorey.characterLink = characterLink;
		<LoadCharacterIntoSlot>c__AnonStorey.visualInfo = visualInfo;
		<LoadCharacterIntoSlot>c__AnonStorey.isBot = isBot;
		<LoadCharacterIntoSlot>c__AnonStorey.playSelectionChatterCue = playSelectionChatterCue;
		<LoadCharacterIntoSlot>c__AnonStorey.$this = this;
		if (!(UIFrontEnd.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.LoadCharacterIntoSlot(CharacterResourceLink, int, string, CharacterVisualInfo, bool, bool)).MethodHandle;
			}
			if (!(UICharacterSelectWorldObjects.Get() == null))
			{
				if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink != null && <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType == this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].type)
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
					if (<LoadCharacterIntoSlot>c__AnonStorey.visualInfo.Equals(this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].skin))
					{
						if (this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].instantiatedCharacter != null)
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
							Animator componentInChildren = this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
							UIActorModelData.SetCharSelectTrigger(componentInChildren, false, false);
						}
						return;
					}
				}
				if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink != null)
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
					if (!<LoadCharacterIntoSlot>c__AnonStorey.characterLink.IsVisualInfoSelectionValid(<LoadCharacterIntoSlot>c__AnonStorey.visualInfo))
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
						Log.Error(string.Format("Character {0} could not find Actor Skin resource link for {1}", <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_displayName, <LoadCharacterIntoSlot>c__AnonStorey.visualInfo.ToString()), new object[0]);
						<LoadCharacterIntoSlot>c__AnonStorey.visualInfo = default(CharacterVisualInfo);
					}
				}
				GameObject instantiatedCharacter = this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].instantiatedCharacter;
				<LoadCharacterIntoSlot>c__AnonStorey.prevCharacterLoaded = (instantiatedCharacter != null);
				<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoName = string.Empty;
				<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoNormalizedTime = 0f;
				<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoStateHash = 0;
				<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInCharSelState = false;
				if (<LoadCharacterIntoSlot>c__AnonStorey.prevCharacterLoaded)
				{
					Animator componentInChildren2 = instantiatedCharacter.GetComponentInChildren<Animator>();
					AnimatorStateInfo currentAnimatorStateInfo = componentInChildren2.GetCurrentAnimatorStateInfo(0);
					<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoName = componentInChildren2.name;
					if (componentInChildren2.runtimeAnimatorController != null)
					{
						<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoName = componentInChildren2.runtimeAnimatorController.name;
					}
					<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoStateHash = currentAnimatorStateInfo.fullPathHash;
					<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoNormalizedTime = currentAnimatorStateInfo.normalizedTime;
					<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInCharSelState = UIActorModelData.IsInCharSelectAnimState(componentInChildren2);
				}
				<LoadCharacterIntoSlot>c__AnonStorey.ResetRotation = false;
				if (<LoadCharacterIntoSlot>c__AnonStorey.slotIndex == 0)
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
					if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink != null)
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
						if (this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].type != <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType)
						{
							<LoadCharacterIntoSlot>c__AnonStorey.ResetRotation = true;
						}
					}
				}
				<LoadCharacterIntoSlot>c__AnonStorey.prevCharType = CharacterType.None;
				if (this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].instantiatedCharacter != null)
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
					<LoadCharacterIntoSlot>c__AnonStorey.prevCharType = this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].type;
				}
				UICharacterWorldObjects.<LoadCharacterIntoSlot>c__AnonStorey0 <LoadCharacterIntoSlot>c__AnonStorey2 = <LoadCharacterIntoSlot>c__AnonStorey;
				bool preUnloadChar;
				if (!(<LoadCharacterIntoSlot>c__AnonStorey.characterLink == null))
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
					if (this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].type == <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType)
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
						preUnloadChar = (this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].skin.skinIndex != <LoadCharacterIntoSlot>c__AnonStorey.visualInfo.skinIndex);
						goto IL_390;
					}
				}
				preUnloadChar = true;
				IL_390:
				<LoadCharacterIntoSlot>c__AnonStorey2.preUnloadChar = preUnloadChar;
				if (<LoadCharacterIntoSlot>c__AnonStorey.preUnloadChar)
				{
					this.UnloadCharacter(<LoadCharacterIntoSlot>c__AnonStorey.slotIndex, !<LoadCharacterIntoSlot>c__AnonStorey.prevCharacterLoaded || <LoadCharacterIntoSlot>c__AnonStorey.characterLink == null);
				}
				if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink == null)
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
					this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].PlayAnimation("ReadyOut");
				}
				if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink != null)
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
					this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].resourceLink = <LoadCharacterIntoSlot>c__AnonStorey.characterLink;
					this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].type = <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType;
					this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].skin = <LoadCharacterIntoSlot>c__AnonStorey.visualInfo;
					this.m_characterIsLoading[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex] = true;
					if (AsyncManager.Get() != null)
					{
						<LoadCharacterIntoSlot>c__AnonStorey.characterLink.LoadAsync(<LoadCharacterIntoSlot>c__AnonStorey.visualInfo, out this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].loadingTicket, (float)<LoadCharacterIntoSlot>c__AnonStorey.slotIndex * 0.1f, delegate(LoadedCharacterSelection loadedCharacter)
						{
							if (!(UIFrontEnd.Get() == null))
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
									RuntimeMethodHandle runtimeMethodHandle2 = methodof(UICharacterWorldObjects.<LoadCharacterIntoSlot>c__AnonStorey0.<>m__0(LoadedCharacterSelection)).MethodHandle;
								}
								if (UIManager.Get().CurrentState != UIManager.ClientState.InGame)
								{
									if (!<LoadCharacterIntoSlot>c__AnonStorey.preUnloadChar)
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
										UICharacterWorldObjects $this = <LoadCharacterIntoSlot>c__AnonStorey.$this;
										int slotIndex2 = <LoadCharacterIntoSlot>c__AnonStorey.slotIndex;
										bool playAnimation;
										if (<LoadCharacterIntoSlot>c__AnonStorey.prevCharacterLoaded)
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
											playAnimation = (<LoadCharacterIntoSlot>c__AnonStorey.characterLink == null);
										}
										else
										{
											playAnimation = true;
										}
										$this.UnloadCharacter(slotIndex2, playAnimation);
									}
									else if (<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].m_charSelectSpawnVFX != null)
									{
										UIManager.SetGameObjectActive(<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].m_charSelectSpawnVFX, true, null);
									}
									<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].loadingTicket = -1;
									if (!<LoadCharacterIntoSlot>c__AnonStorey.$this.m_characterIsLoading[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex])
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
										<LoadCharacterIntoSlot>c__AnonStorey.$this.UnloadCharacter(<LoadCharacterIntoSlot>c__AnonStorey.slotIndex, false);
									}
									<LoadCharacterIntoSlot>c__AnonStorey.$this.m_characterIsLoading[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex] = false;
									if (loadedCharacter.heroPrefabLink != null)
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
										if (!loadedCharacter.heroPrefabLink.IsEmpty)
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
											GameObject gameObject = loadedCharacter.heroPrefabLink.InstantiatePrefab(false);
											if (gameObject == null)
											{
												throw new ApplicationException(string.Format("Failed to instantiate prefab for {0} {1}", <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType, <LoadCharacterIntoSlot>c__AnonStorey.visualInfo.ToString()));
											}
											ActorModelData component = gameObject.GetComponent<ActorModelData>();
											bool flag = false;
											if (MasterSkinVfxData.Get() != null && MasterSkinVfxData.Get().m_addMasterSkinVfx)
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
												if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink.IsVisualInfoSelectionValid(<LoadCharacterIntoSlot>c__AnonStorey.visualInfo))
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
													CharacterColor characterColor = <LoadCharacterIntoSlot>c__AnonStorey.characterLink.GetCharacterColor(<LoadCharacterIntoSlot>c__AnonStorey.visualInfo);
													flag = (characterColor.m_styleLevel == StyleLevelType.Mastery);
												}
											}
											if (flag)
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
												MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(component.gameObject, <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType, <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenScale);
											}
											component.EnableRagdoll(false, null);
											Dictionary<int, string> animatorStateNameHashToNameMap = component.GetAnimatorStateNameHashToNameMap();
											gameObject.transform.position = <LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].transform.position;
											gameObject.transform.SetParent(<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].GetContainerTransform());
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].instantiatedCharacter = gameObject;
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData = gameObject.GetComponent<UIActorModelData>();
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].resourceLink = <LoadCharacterIntoSlot>c__AnonStorey.characterLink;
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].type = <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType;
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].skin = <LoadCharacterIntoSlot>c__AnonStorey.visualInfo;
											Vector3 loadScreenPosition = <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenPosition;
											if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenDistTowardsCamera != 0f)
											{
												loadScreenPosition.x = 0f;
												loadScreenPosition.z = 0f;
											}
											if (<LoadCharacterIntoSlot>c__AnonStorey.slotIndex > 0)
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
												loadScreenPosition.y = Mathf.Min(loadScreenPosition.y, 0.15f);
											}
											gameObject.transform.localPosition = loadScreenPosition;
											gameObject.transform.localScale = new Vector3(<LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenScale, <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenScale, <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenScale);
											gameObject.transform.localRotation = Quaternion.identity;
											if (!<LoadCharacterIntoSlot>c__AnonStorey.prevCharacterLoaded && !<LoadCharacterIntoSlot>c__AnonStorey.isBot)
											{
												if (<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].ready)
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
													<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].PlayAnimation("ReadyIn");
												}
												else
												{
													<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].PlayAnimation("TransitionIn");
												}
											}
											GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
											if (gameObject2.GetComponent<FrontEndAnimationEventReceiver>() == null)
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
												gameObject2.AddComponent<FrontEndAnimationEventReceiver>();
											}
											Animator componentInChildren3 = gameObject.GetComponentInChildren<Animator>();
											if (componentInChildren3 != null)
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
												if (componentInChildren3.isInitialized)
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
													string name = componentInChildren3.name;
													if (componentInChildren3.runtimeAnimatorController != null)
													{
														name = componentInChildren3.runtimeAnimatorController.name;
													}
													if (name == <LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoName)
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
														if (<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInCharSelState)
														{
															componentInChildren3.Play(<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoStateHash, -1, <LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoNormalizedTime);
														}
													}
													if (!(name != <LoadCharacterIntoSlot>c__AnonStorey.prevAnimInfoName))
													{
														if (<LoadCharacterIntoSlot>c__AnonStorey.prevAnimInCharSelState)
														{
															goto IL_58F;
														}
														for (;;)
														{
															switch (6)
															{
															case 0:
																continue;
															}
															break;
														}
													}
													UIActorModelData.SetCharSelectTrigger(componentInChildren3, <LoadCharacterIntoSlot>c__AnonStorey.prevCharType != <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_characterType, true);
													IL_58F:
													componentInChildren3.SetBool("DecisionPhase", !<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].ready);
													if (<LoadCharacterIntoSlot>c__AnonStorey.$this.ParamExists(componentInChildren3, "SkinIndex"))
													{
														componentInChildren3.SetInteger("SkinIndex", <LoadCharacterIntoSlot>c__AnonStorey.visualInfo.skinIndex);
													}
													if (<LoadCharacterIntoSlot>c__AnonStorey.$this.ParamExists(componentInChildren3, "PatternIndex"))
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
														componentInChildren3.SetInteger("PatternIndex", <LoadCharacterIntoSlot>c__AnonStorey.visualInfo.patternIndex);
													}
													if (<LoadCharacterIntoSlot>c__AnonStorey.$this.ParamExists(componentInChildren3, "ColorIndex"))
													{
														componentInChildren3.SetInteger("ColorIndex", <LoadCharacterIntoSlot>c__AnonStorey.visualInfo.colorIndex);
													}
												}
											}
											if (<LoadCharacterIntoSlot>c__AnonStorey.slotIndex == 0)
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
												if (<LoadCharacterIntoSlot>c__AnonStorey.ResetRotation)
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
													UIFrontEnd.Get().ResetCharacterRotation();
												}
											}
											if (<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[0].instantiatedCharacter != null)
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
												if (<LoadCharacterIntoSlot>c__AnonStorey.$this.GetCharacterLookAtCamera() != null)
												{
													if (<LoadCharacterIntoSlot>c__AnonStorey.$this.m_characterLookAtLocation == null)
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
														<LoadCharacterIntoSlot>c__AnonStorey.$this.m_characterLookAtLocation = new GameObject();
													}
													<LoadCharacterIntoSlot>c__AnonStorey.$this.m_characterLookAtLocation.transform.position = new Vector3(<LoadCharacterIntoSlot>c__AnonStorey.$this.GetCharacterLookAtCamera().transform.position.x, <LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[0].instantiatedCharacter.transform.position.y, <LoadCharacterIntoSlot>c__AnonStorey.$this.GetCharacterLookAtCamera().transform.position.z);
													<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(<LoadCharacterIntoSlot>c__AnonStorey.$this.m_characterLookAtLocation.transform);
													<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles += UIFrontEnd.Get().GetRotationOffset();
												}
											}
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].PlayBaseObjectAnimation("SlotIN");
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
											for (;;)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												break;
											}
											<LoadCharacterIntoSlot>c__AnonStorey.$this.ChangeLayersRecursively(gameObject.transform, "UIInWorld");
											Collider[] componentsInChildren3 = gameObject.GetComponentsInChildren<Collider>();
											for (int k = 0; k < componentsInChildren3.Length; k++)
											{
												if (componentsInChildren3[k].name != "floor_collider")
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
													componentsInChildren3[k].enabled = true;
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
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData = gameObject.AddComponent<UIActorModelData>();
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData.DelayEnablingOfShroudInstances();
											<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData.SetStateNameHashToNameMap(animatorStateNameHashToNameMap);
											if (<LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenDistTowardsCamera != 0f)
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
												float num = <LoadCharacterIntoSlot>c__AnonStorey.characterLink.m_loadScreenDistTowardsCamera;
												if (<LoadCharacterIntoSlot>c__AnonStorey.slotIndex > 0)
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
													num = Mathf.Min(0.85f, num);
												}
												<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData.m_setOffsetTowardsCamera = true;
												<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData.m_offsetDistanceTowardsCamera = num;
												<LoadCharacterIntoSlot>c__AnonStorey.$this.m_loadedCharacters[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].uiActorModelData.SetParentLocalPositionOffset();
											}
											else
											{
												<LoadCharacterIntoSlot>c__AnonStorey.$this.m_ringAnimations[<LoadCharacterIntoSlot>c__AnonStorey.slotIndex].GetContainerTransform().localPosition = Vector3.zero;
											}
											if (<LoadCharacterIntoSlot>c__AnonStorey.slotIndex == 0)
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
												UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectNotifyCharLoaded);
												UICharacterSelectScreenController.Get().NotifyCharacterDoneLoading();
												if (<LoadCharacterIntoSlot>c__AnonStorey.playSelectionChatterCue)
												{
													GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndSelectionChatterCue, null);
												}
											}
										}
									}
									return;
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
						});
					}
				}
				return;
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
		}
	}

	private bool ParamExists(Animator animator, string paramName)
	{
		for (int i = 0; i < animator.parameterCount; i++)
		{
			if (animator.parameters[i].name == paramName)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterWorldObjects.ParamExists(Animator, string)).MethodHandle;
				}
				return true;
			}
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
