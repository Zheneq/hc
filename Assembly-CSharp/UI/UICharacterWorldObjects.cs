using System;
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

        if (m_loadedCharacters[0].instantiatedCharacter != null)
        {
            if (m_characterLookAtLocation == null)
            {
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
        }
    }

    private void OnDestroy()
    {
        m_shuttingDown = true;
        UnloadAllCharacters();
    }

    private void Update()
    {
        if (m_loadedCharacters[0].instantiatedCharacter == null)
        {
            return;
        }

        if (GetCharacterLookAtCamera() == null)
        {
            return;
        }

        if (m_characterLookAtLocation == null)
        {
            m_characterLookAtLocation = new GameObject();
        }

        Transform transform = m_characterLookAtLocation.transform;
        transform.position = new Vector3(
            GetCharacterLookAtCamera().transform.position.x,
            m_loadedCharacters[0].instantiatedCharacter.transform.position.y,
            GetCharacterLookAtCamera().transform.position.z);
        m_loadedCharacters[0].instantiatedCharacter.transform.LookAt(m_characterLookAtLocation.transform);
        m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles += UIFrontEnd.Get().GetRotationOffset();
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

        m_ringAnimations[index].PlayAnimation(isReady ? "ReadyIn" : "ReadyOut");
        m_loadedCharacters[index].ready = isReady;
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
                m_loadedCharacters[i].uiActorModelData.SetReady(m_loadedCharacters[i].ready);
            }
        }
    }

    public void SetSkins()
    {
        for (int i = 0; i < m_loadedCharacters.Length; i++)
        {
            if (m_loadedCharacters[i].uiActorModelData != null)
            {
                m_loadedCharacters[i].uiActorModelData.SetSkin(m_loadedCharacters[i].skin);
            }
        }
    }

    public void SetIsInGameRings()
    {
        for (int i = 0; i < m_loadedCharacters.Length; i++)
        {
            UIManager.SetGameObjectActive(
                m_ringAnimations[i].m_isInGameAnimation,
                m_loadedCharacters[i].isInGame && !m_loadedCharacters[i].ready);
        }

        for (int j = m_loadedCharacters.Length; j < m_ringAnimations.Length; j++)
        {
            UIManager.SetGameObjectActive(m_ringAnimations[j].m_isInGameAnimation, false);
        }
    }

    public void UnloadAllCharacters()
    {
        for (int i = 0; i < m_loadedCharacters.Length; i++)
        {
            UnloadCharacter(i);
        }
    }

    public void UnloadCharacter(int slotIndex, bool playAnimation = true)
    {
        if (m_loadedCharacters[slotIndex].loadingTicket != -1)
        {
            m_loadedCharacters[slotIndex].resourceLink.CancelLoad(
                m_loadedCharacters[slotIndex].skin,
                m_loadedCharacters[slotIndex].loadingTicket);
        }

        if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
        {
            Destroy(m_loadedCharacters[slotIndex].instantiatedCharacter);
            if (playAnimation && !m_shuttingDown)
            {
                m_ringAnimations[slotIndex].PlayAnimation("TransitionOut");
            }
        }

        if (playAnimation && !m_shuttingDown)
        {
            m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotOUT");
        }

        m_loadedCharacters[slotIndex].Clear();
        if (m_ringAnimations[slotIndex].m_isInGameAnimation != null)
        {
            UIManager.SetGameObjectActive(m_ringAnimations[slotIndex].m_isInGameAnimation, false);
        }

        if (m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null)
        {
            UIManager.SetGameObjectActive(m_ringAnimations[slotIndex].m_charSelectSpawnVFX, false);
        }
    }

    public void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
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
        if (characterLink == null)
        {
            return;
        }

        if (characterLink.m_characterType != m_loadedCharacters[slotIndex].type)
        {
            return;
        }

        if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
        {
            Animator animators = m_loadedCharacters[slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
            UIActorModelData.SetCharSelectTrigger(animators, false, false);
        }
    }

    public void LoadCharacterIntoSlot(
        CharacterType character,
        int slotIndex,
        string characterName,
        CharacterVisualInfo skinSelector,
        bool isBot)
    {
        LoadCharacterIntoSlot(
            GameWideData.Get().GetCharacterResourceLink(character),
            slotIndex,
            characterName,
            skinSelector,
            isBot,
            false);
    }

    public void LoadCharacterIntoSlot(
        CharacterResourceLink characterLink,
        int slotIndex,
        string characterName,
        CharacterVisualInfo visualInfo,
        bool isBot,
        bool playSelectionChatterCue)
    {
        if (UIFrontEnd.Get() == null)
        {
            return;
        }

        if (UICharacterSelectWorldObjects.Get() == null)
        {
            return;
        }

        if (characterLink != null
            && characterLink.m_characterType == m_loadedCharacters[slotIndex].type
            && visualInfo.Equals(m_loadedCharacters[slotIndex].skin))
        {
            if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
            {
                Animator animators = m_loadedCharacters[slotIndex].instantiatedCharacter.GetComponentInChildren<Animator>();
                UIActorModelData.SetCharSelectTrigger(animators, false, false);
            }

            return;
        }

        if (characterLink != null && !characterLink.IsVisualInfoSelectionValid(visualInfo))
        {
            Log.Error($"Character {characterLink.m_displayName} could not find Actor Skin resource link for {visualInfo.ToString()}");
            visualInfo = default(CharacterVisualInfo);
        }

        GameObject instantiatedCharacter = m_loadedCharacters[slotIndex].instantiatedCharacter;
        bool prevCharacterLoaded = instantiatedCharacter != null;
        string prevAnimInfoName = string.Empty;
        float prevAnimInfoNormalizedTime = 0f;
        int prevAnimInfoStateHash = 0;
        bool prevAnimInCharSelState = false;
        if (prevCharacterLoaded)
        {
            Animator animator = instantiatedCharacter.GetComponentInChildren<Animator>();
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

        bool ResetRotation = slotIndex == 0
                             && characterLink != null
                             && m_loadedCharacters[slotIndex].type != characterLink.m_characterType;
        CharacterType prevCharType = CharacterType.None;
        if (m_loadedCharacters[slotIndex].instantiatedCharacter != null)
        {
            prevCharType = m_loadedCharacters[slotIndex].type;
        }

        bool preUnloadChar = characterLink == null
                             || m_loadedCharacters[slotIndex].type != characterLink.m_characterType
                             || m_loadedCharacters[slotIndex].skin.skinIndex != visualInfo.skinIndex;
        if (preUnloadChar)
        {
            UnloadCharacter(slotIndex, !prevCharacterLoaded || characterLink == null);
        }

        if (characterLink == null)
        {
            m_ringAnimations[slotIndex].PlayAnimation("ReadyOut");
            return;
        }

        m_loadedCharacters[slotIndex].resourceLink = characterLink;
        m_loadedCharacters[slotIndex].type = characterLink.m_characterType;
        m_loadedCharacters[slotIndex].skin = visualInfo;
        m_characterIsLoading[slotIndex] = true;
        if (AsyncManager.Get() != null)
        {
            characterLink.LoadAsync(
                visualInfo,
                out m_loadedCharacters[slotIndex].loadingTicket,
                (float)slotIndex * 0.1f,
                delegate(LoadedCharacterSelection loadedCharacter)
                {
                    if (UIFrontEnd.Get() == null)
                    {
                        return;
                    }

                    if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
                    {
                        return;
                    }

                    if (!preUnloadChar)
                    {
                        UnloadCharacter(slotIndex, !prevCharacterLoaded || characterLink == null);
                    }
                    else if (m_ringAnimations[slotIndex].m_charSelectSpawnVFX != null)
                    {
                        UIManager.SetGameObjectActive(m_ringAnimations[slotIndex].m_charSelectSpawnVFX, true);
                    }

                    m_loadedCharacters[slotIndex].loadingTicket = -1;
                    if (!m_characterIsLoading[slotIndex])
                    {
                        UnloadCharacter(slotIndex, false);
                    }

                    m_characterIsLoading[slotIndex] = false;
                    if (loadedCharacter.heroPrefabLink == null || loadedCharacter.heroPrefabLink.IsEmpty)
                    {
                        return;
                    }

                    GameObject heroObject = loadedCharacter.heroPrefabLink.InstantiatePrefab();
                    if (heroObject == null)
                    {
                        throw new ApplicationException(
                            $"Failed to instantiate prefab for {characterLink.m_characterType} {visualInfo.ToString()}");
                    }

                    ActorModelData component = heroObject.GetComponent<ActorModelData>();
                    bool isMasterSkin = false;
                    if (MasterSkinVfxData.Get() != null && MasterSkinVfxData.Get().m_addMasterSkinVfx)
                    {
                        if (characterLink.IsVisualInfoSelectionValid(visualInfo))
                        {
                            CharacterColor characterColor = characterLink.GetCharacterColor(visualInfo);
                            isMasterSkin = characterColor.m_styleLevel == StyleLevelType.Mastery;
                        }
                    }

                    if (isMasterSkin)
                    {
                        MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(
                            component.gameObject,
                            characterLink.m_characterType,
                            characterLink.m_loadScreenScale);
                    }

                    component.EnableRagdoll(false);
                    Dictionary<int, string> animatorStateNameHashToNameMap =
                        component.GetAnimatorStateNameHashToNameMap();
                    heroObject.transform.position = m_ringAnimations[slotIndex].transform.position;
                    heroObject.transform.SetParent(m_ringAnimations[slotIndex].GetContainerTransform());
                    m_loadedCharacters[slotIndex].instantiatedCharacter = heroObject;
                    m_loadedCharacters[slotIndex].uiActorModelData = heroObject.GetComponent<UIActorModelData>();
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
                        loadScreenPosition.y = Mathf.Min(loadScreenPosition.y, 0.15f);
                    }

                    heroObject.transform.localPosition = loadScreenPosition;
                    heroObject.transform.localScale = new Vector3(
                        characterLink.m_loadScreenScale,
                        characterLink.m_loadScreenScale,
                        characterLink.m_loadScreenScale);
                    heroObject.transform.localRotation = Quaternion.identity;
                    if (!prevCharacterLoaded && !isBot)
                    {
                        m_ringAnimations[slotIndex].PlayAnimation(
                            m_loadedCharacters[slotIndex].ready ? "ReadyIn" : "TransitionIn");
                    }

                    GameObject heroChildObject = heroObject.transform.GetChild(0).gameObject;
                    if (heroChildObject.GetComponent<FrontEndAnimationEventReceiver>() == null)
                    {
                        heroChildObject.AddComponent<FrontEndAnimationEventReceiver>();
                    }

                    Animator animator = heroObject.GetComponentInChildren<Animator>();
                    if (animator != null && animator.isInitialized)
                    {
                        string animInfoName = animator.name;
                        if (animator.runtimeAnimatorController != null)
                        {
                            animInfoName = animator.runtimeAnimatorController.name;
                        }

                        if (animInfoName == prevAnimInfoName && prevAnimInCharSelState)
                        {
                            animator.Play(prevAnimInfoStateHash, -1, prevAnimInfoNormalizedTime);
                        }
                        else
                        {
                            UIActorModelData.SetCharSelectTrigger(
                                animator,
                                prevCharType != characterLink.m_characterType,
                                true);
                        }

                        animator.SetBool("DecisionPhase", !m_loadedCharacters[slotIndex].ready);
                        if (ParamExists(animator, "SkinIndex"))
                        {
                            animator.SetInteger("SkinIndex", visualInfo.skinIndex);
                        }

                        if (ParamExists(animator, "PatternIndex"))
                        {
                            animator.SetInteger("PatternIndex", visualInfo.patternIndex);
                        }

                        if (ParamExists(animator, "ColorIndex"))
                        {
                            animator.SetInteger("ColorIndex", visualInfo.colorIndex);
                        }
                    }

                    if (slotIndex == 0 && ResetRotation)
                    {
                        UIFrontEnd.Get().ResetCharacterRotation();
                    }

                    if (m_loadedCharacters[0].instantiatedCharacter != null && GetCharacterLookAtCamera() != null)
                    {
                        if (m_characterLookAtLocation == null)
                        {
                            m_characterLookAtLocation = new GameObject();
                        }

                        Transform transform = m_characterLookAtLocation.transform;
                        transform.position = new Vector3(
                            GetCharacterLookAtCamera().transform.position.x,
                            m_loadedCharacters[0].instantiatedCharacter.transform.position.y,
                            GetCharacterLookAtCamera().transform.position.z);
                        m_loadedCharacters[0].instantiatedCharacter.transform
                            .LookAt(m_characterLookAtLocation.transform);
                        m_loadedCharacters[0].instantiatedCharacter.transform.localEulerAngles +=
                            UIFrontEnd.Get().GetRotationOffset();
                    }

                    m_ringAnimations[slotIndex].PlayBaseObjectAnimation("SlotIN");
                    Destroy(component);
                    foreach (Joint obj in heroObject.GetComponentsInChildren<Joint>(true))
                    {
                        Destroy(obj);
                    }

                    foreach (Rigidbody obj2 in heroObject.GetComponentsInChildren<Rigidbody>(true))
                    {
                        Destroy(obj2);
                    }

                    ChangeLayersRecursively(heroObject.transform, "UIInWorld");
                    foreach (Collider collider in heroObject.GetComponentsInChildren<Collider>())
                    {
                        if (collider.name != "floor_collider")
                        {
                            collider.enabled = true;
                        }
                    }

                    m_loadedCharacters[slotIndex].uiActorModelData = heroObject.AddComponent<UIActorModelData>();
                    m_loadedCharacters[slotIndex].uiActorModelData.DelayEnablingOfShroudInstances();
                    m_loadedCharacters[slotIndex].uiActorModelData
                        .SetStateNameHashToNameMap(animatorStateNameHashToNameMap);
                    if (characterLink.m_loadScreenDistTowardsCamera != 0f)
                    {
                        float offset = characterLink.m_loadScreenDistTowardsCamera;
                        if (slotIndex > 0)
                        {
                            offset = Mathf.Min(0.85f, offset);
                        }

                        m_loadedCharacters[slotIndex].uiActorModelData.m_setOffsetTowardsCamera = true;
                        m_loadedCharacters[slotIndex].uiActorModelData.m_offsetDistanceTowardsCamera = offset;
                        m_loadedCharacters[slotIndex].uiActorModelData.SetParentLocalPositionOffset();
                    }
                    else
                    {
                        m_ringAnimations[slotIndex].GetContainerTransform().localPosition = Vector3.zero;
                    }

                    if (slotIndex == 0)
                    {
                        UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectNotifyCharLoaded);
                        UICharacterSelectScreenController.Get().NotifyCharacterDoneLoading();
                        if (playSelectionChatterCue)
                        {
                            GameEventManager.Get().FireEvent(
                                GameEventManager.EventType.FrontEndSelectionChatterCue,
                                null);
                        }
                    }
                });
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