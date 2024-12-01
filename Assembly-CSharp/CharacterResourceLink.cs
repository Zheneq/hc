using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterResourceLink : MonoBehaviour
{
    public string m_displayName;
    [TextArea(1, 5, order = 1)]
    public string m_charSelectTooltipDescription = "Edit this in the inspector";
    [TextArea(1, 5, order = 1)]
    public string m_charSelectAboutDescription = "Edit this in the inspector";
    [TextArea(1, 5, order = 1)]
    public string m_characterBio = "Edit this in the inspector";
    [Header("-- Character Icons --")]
    [AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
    public string m_characterIconResourceString;
    [AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
    public string m_characterSelectIconResourceString;
    [AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
    public string m_characterSelectIcon_bwResourceString;
    [AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
    public string m_loadingProfileIconResourceString;
    public string m_actorDataResourcePath;
    [Separator("Scale/Offset in frontend UI (character select, collections)")]
    public Vector3 m_loadScreenPosition;
    public float m_loadScreenScale;
    public float m_loadScreenDistTowardsCamera;
    [Space(10f)]
    public CharacterType m_characterType;
    public CharacterRole m_characterRole;
    public Color m_characterColor;
    public int m_factionBannerID;
    public bool m_allowForBots;
    public bool m_allowForPlayers;
    public bool m_isHidden;
    public GameBalanceVars.CharResourceLinkCharUnlockData m_charUnlockData;
    public string m_isHiddenFromFreeRotationUntil;
    public string m_twitterHandle;
    [Space(10f)]
    public CountryPrices Prices;
    [Space(10f)]
    [Range(0f, 10f)]
    public int m_statHealth;
    [Range(0f, 10f)]
    public int m_statDamage;
    [Range(0f, 10f)]
    public int m_statSurvival;
    [Range(0f, 10f)]
    public int m_statDifficulty;
    public List<CharacterSkin> m_skins = new List<CharacterSkin>();
    public List<CharacterTaunt> m_taunts = new List<CharacterTaunt>();
    public List<CharacterAbilityVfxSwap> m_vfxSwapsForAbility0 = new List<CharacterAbilityVfxSwap>();
    public List<CharacterAbilityVfxSwap> m_vfxSwapsForAbility1 = new List<CharacterAbilityVfxSwap>();
    public List<CharacterAbilityVfxSwap> m_vfxSwapsForAbility2 = new List<CharacterAbilityVfxSwap>();
    public List<CharacterAbilityVfxSwap> m_vfxSwapsForAbility3 = new List<CharacterAbilityVfxSwap>();
    public List<CharacterAbilityVfxSwap> m_vfxSwapsForAbility4 = new List<CharacterAbilityVfxSwap>();
    public string camSequenceFolderName;

    [Tooltip("Audio assets default prefabs. (For front end)")]
    [Header("-- Audio Assets --")]
    public PrefabResourceLink[] m_audioAssetsFrontEndDefaultPrefabs;

    [Tooltip("Audio assets default prefabs. (For in game)")]
    public PrefabResourceLink[] m_audioAssetsInGameDefaultPrefabs;

    [Header("-- FX preloading --")]
    [Tooltip("Checked if this character will ever have any VFX made with .pkfx files")]
    public bool m_willEverHavePkfx = true;

    [LeafDirectoryPopup("Directory containing all .pkfx files for this skin", "PackFx/Character/Hero")]
    public string m_pkfxDirectoryDefault;

    private Dictionary<CharacterVisualInfo, LoadedCharacterSelection> m_loadedCharacterCache =
        new Dictionary<CharacterVisualInfo, LoadedCharacterSelection>();

    private static List<CharacterResourceLink> s_links = new List<CharacterResourceLink>();
    private static Dictionary<string, GameObject> s_loadedActorDataPrefabCache = new Dictionary<string, GameObject>();

    private static Dictionary<string, GameObject> s_instantiatedInGameAudioResources =
        new Dictionary<string, GameObject>();

    private static Dictionary<string, GameObject> s_instantiatedFrontEndAudioResources =
        new Dictionary<string, GameObject>();

    internal const string c_heroPKFXRelativePath = "PackFx/Character/Hero";
    private const string kAssassionIcon = "iconAssassin";
    private const string kSupportIcon = "iconSupport";
    private const string kTankIcon = "iconTank";

    internal GameObject ActorDataPrefab
    {
        get
        {
            if (s_loadedActorDataPrefabCache.TryGetValue(m_actorDataResourcePath, out GameObject prefab)
                && prefab != null)
            {
                return prefab;
            }

            prefab = Resources.Load<GameObject>(m_actorDataResourcePath);
            if (prefab != null)
            {
                s_loadedActorDataPrefabCache[m_actorDataResourcePath] = prefab;
            }

            return prefab;
        }
    }

    private void Awake()
    {
        s_links.Add(this);
    }

    public string GetDisplayName()
    {
        return StringUtil.TR_CharacterName(m_characterType.ToString());
    }

    public string GetCharSelectTooltipDescription()
    {
        return StringUtil.TR_CharacterSelectTooltip(m_characterType.ToString());
    }

    public string GetCharSelectAboutDescription()
    {
        return StringUtil.TR_CharacterSelectAboutDesc(m_characterType.ToString());
    }

    public string GetCharBio()
    {
        return StringUtil.TR_CharacterBio(m_characterType.ToString());
    }

    public string GetSkinName(int skinIndex)
    {
        return StringUtil.TR_CharacterSkinName(m_characterType.ToString(), skinIndex + 1);
    }

    public string GetSkinDescription(int skinIndex)
    {
        return StringUtil.TR_CharacterSkinDescription(m_characterType.ToString(), skinIndex + 1);
    }

    public string GetSkinFlavorText(int skinIndex)
    {
        return StringUtil.TR_CharacterSkinFlavor(m_characterType.ToString(), skinIndex + 1);
    }

    public string GetPatternName(int skinIndex, int patternIndex)
    {
        return StringUtil.TR_CharacterPatternName(m_characterType.ToString(), skinIndex + 1, patternIndex + 1);
    }

    public string GetPatternColorName(int skinIndex, int patternIndex, int colorIndex)
    {
        return StringUtil.TR_CharacterPatternColorName(m_characterType.ToString(), skinIndex + 1, patternIndex + 1,
            colorIndex + 1);
    }

    public string GetPatternColorDescription(int skinIndex, int patternIndex, int colorIndex)
    {
        return StringUtil.TR_CharacterPatternColorDescription(m_characterType.ToString(), skinIndex + 1,
            patternIndex + 1, colorIndex + 1);
    }

    public string GetPatternColorFlavor(int skinIndex, int patternIndex, int colorIndex)
    {
        return StringUtil.TR_CharacterPatternColorFlavor(m_characterType.ToString(), skinIndex + 1, patternIndex + 1,
            colorIndex + 1);
    }

    public string GetTauntName(int tauntIndex)
    {
        return StringUtil.TR_CharacterTauntName(m_characterType.ToString(), tauntIndex + 1);
    }

    public string GetVFXSwapName(int abilityIndex, int vfxSwapId)
    {
        return StringUtil.TR_GetCharacterVFXSwapName(m_characterType.ToString(), abilityIndex + 1, vfxSwapId);
    }

    internal static void DestroyAudioResources()
    {
        foreach (KeyValuePair<string, GameObject> audioRes in s_instantiatedInGameAudioResources)
        {
            Destroy(audioRes.Value);
        }

        s_instantiatedInGameAudioResources.Clear();
        foreach (KeyValuePair<string, GameObject> audioRes in s_instantiatedFrontEndAudioResources)
        {
            Destroy(audioRes.Value);
        }

        s_instantiatedFrontEndAudioResources.Clear();
    }

    internal void LoadAsync(CharacterVisualInfo selection, CharacterResourceDelegate onCharacterPrefabLoaded)
    {
        LoadAsync(selection, out _, onCharacterPrefabLoaded);
    }

    internal void LoadAsync(
        CharacterVisualInfo selection,
        CharacterResourceDelegate onCharacterPrefabLoaded,
        GameStatus gameStatusForAssets)
    {
        LoadAsync(selection, out _, onCharacterPrefabLoaded, gameStatusForAssets);
    }

    internal void LoadAsync(
        CharacterVisualInfo selection,
        out int asyncTicket,
        CharacterResourceDelegate onCharacterPrefabLoaded)
    {
        LoadAsync(selection, out asyncTicket, onCharacterPrefabLoaded, GameManager.Get().GameStatus);
    }

    internal void LoadAsync(
        CharacterVisualInfo selection,
        out int asyncTicket,
        float delay,
        CharacterResourceDelegate onCharacterPrefabLoaded)
    {
        LoadAsync(selection, out asyncTicket, onCharacterPrefabLoaded, GameManager.Get().GameStatus, delay);
    }

    private void LoadAsync(
        CharacterVisualInfo selection,
        out int asyncTicket,
        CharacterResourceDelegate onCharacterPrefabLoaded,
        GameStatus gameStatusForAssets,
        float delay = 0f)
    {
        if (onCharacterPrefabLoaded == null)
        {
            throw new ArgumentNullException(nameof(onCharacterPrefabLoaded));
        }

        AsyncManager.Get().StartAsyncOperation(out asyncTicket,
            CharacterLoadCoroutine(selection, onCharacterPrefabLoaded, gameStatusForAssets), delay);
    }

    private IEnumerator CharacterLoadCoroutine(
        CharacterVisualInfo selection,
        CharacterResourceDelegate onCharacterPrefabLoaded,
        GameStatus gameStatusForAssets)
    {
        if (!IsVisualInfoSelectionValid(selection))
        {
            Log.Warning(
                Log.Category.Loading,
                $"Invalid skin selection used to load CharacterType {m_characterType}, reverting to default. Input = {selection}");
            selection.ResetToDefault();
        }

        LoadedCharacterSelection loadedCharacter;
        while (m_loadedCharacterCache.TryGetValue(selection, out loadedCharacter))
        {
            if (!loadedCharacter.isLoading)
            {
                break;
            }

            yield return null;
        }

        if (loadedCharacter != null)
        {
            Log.Info(Log.Category.Loading,
                $"Character {name} {selection} already loading has finished - falling through");
            CharacterSkin skin = null;
            if (loadedCharacter != null)
            {
                int skinIndex = loadedCharacter.selectedSkin.skinIndex;
                if (skinIndex >= 0 && skinIndex < m_skins.Count)
                {
                    skin = m_skins[skinIndex];
                }
                else
                {
                    Log.Error("Selected skin index is out of bounds, using default. Input value = " + skinIndex);
                    if (m_skins.Count > 0)
                    {
                        skin = m_skins[0];
                    }
                }
            }

            LoadPKFXForGameStatus(gameStatusForAssets, skin);
            IEnumerator e = CharacterLoadAudioAssetsForGameStatus(gameStatusForAssets, skin);
            do
            {
                yield return e.Current;
            } while (e.MoveNext());
        }
        else
        {
            Log.Info(Log.Category.Loading, "Starting async load for Character " + name + " " + selection);
            loadedCharacter = new LoadedCharacterSelection();
            loadedCharacter.isLoading = true;
            m_loadedCharacterCache.Add(selection, loadedCharacter);
            loadedCharacter.ActorDataPrefab = ActorDataPrefab;
            loadedCharacter.resourceLink = this;
            loadedCharacter.selectedSkin = selection;
            loadedCharacter.heroPrefabLink = GetHeroPrefabLinkFromSelection(selection, out CharacterSkin skin);
            if (loadedCharacter.heroPrefabLink == null || loadedCharacter.heroPrefabLink.IsEmpty)
            {
                Log.Error(
                    $"Character {m_displayName} could not find Actor Skin resource link for {selection.ToString()}.  " +
                    $"Loading default instead...");
                selection.ResetToDefault();
                loadedCharacter.heroPrefabLink = GetHeroPrefabLinkFromSelection(selection, out skin);
            }

            if (NetworkClient.active || !HydrogenConfig.Get().SkipCharacterModelSpawnOnServer)
            {
                Log.Info(Log.Category.Loading,
                    $"Starting async load for actor model prefab for Character {name} {selection}");
                IEnumerator heroPrefabLoader = loadedCharacter.heroPrefabLink.PreLoadPrefabAsync();
                do
                {
                    yield return heroPrefabLoader.Current;
                } while (heroPrefabLoader.MoveNext());

                if (!PrefabResourceLink.HasLoadedResourceLinkForPath(loadedCharacter.heroPrefabLink.GetResourcePath())
                    && !selection.IsDefaultSelection())
                {
                    Log.Error(
                        $"Character {m_displayName} could not load SavedResourceLink for {selection.ToString()}.  " +
                        "Loading default instead...");
                    selection.ResetToDefault();
                    loadedCharacter.heroPrefabLink = GetHeroPrefabLinkFromSelection(selection, out skin);
                    Log.Error(
                        $"Starting async load for actor model prefab for Character {name} {selection} (as fallback)");
                    heroPrefabLoader = loadedCharacter.heroPrefabLink.PreLoadPrefabAsync();
                    do
                    {
                        yield return heroPrefabLoader.Current;
                    } while (heroPrefabLoader.MoveNext());
                }

                LoadPKFXForGameStatus(gameStatusForAssets, skin);
            }

            IEnumerator heroAudioLoader = CharacterLoadAudioAssetsForGameStatus(gameStatusForAssets, skin);
            do
            {
                yield return heroAudioLoader.Current;
            } while (heroAudioLoader.MoveNext());

            loadedCharacter.isLoading = false;
        }

        ClientScene.RegisterPrefab(loadedCharacter.ActorDataPrefab);
        ActorData actorDataComp = loadedCharacter.ActorDataPrefab.GetComponent<ActorData>();
        if (actorDataComp != null)
        {
            foreach (GameObject netObject in actorDataComp.m_additionalNetworkObjectsToRegister)
            {
                if (netObject != null)
                {
                    ClientScene.RegisterPrefab(netObject);
                }
            }
        }

        onCharacterPrefabLoaded(loadedCharacter);
        yield return loadedCharacter;
    }

    internal void CancelLoad(CharacterVisualInfo selection, int asyncTicket)
    {
        if (m_loadedCharacterCache.TryGetValue(selection, out LoadedCharacterSelection loadedCharacterSelection)
            && loadedCharacterSelection != null
            && loadedCharacterSelection.isLoading)
        {
            m_loadedCharacterCache.Remove(selection);
            if (AsyncManager.Get() != null)
            {
                AsyncManager.Get().CancelAsyncOperation(asyncTicket);
            }
        }
    }

    internal void UnloadSkinsNotInList(List<CharacterVisualInfo> skins)
    {
        List<CharacterVisualInfo> toUnload = new List<CharacterVisualInfo>();
        foreach (LoadedCharacterSelection charSelection in m_loadedCharacterCache.Values)
        {
            if (charSelection != null
                && !skins.Contains(charSelection.selectedSkin)
                && charSelection.heroPrefabLink != null)
            {
                charSelection.heroPrefabLink.UnloadPrefab();
                toUnload.Add(charSelection.selectedSkin);
            }
        }

        foreach (CharacterVisualInfo info in toUnload)
        {
            m_loadedCharacterCache.Remove(info);
        }
    }

    internal static void UnloadAll()
    {
        if (GameWideData.Get() != null)
        {
            foreach (CharacterResourceLink characterResourceLink in GameWideData.Get().m_characterResourceLinks)
            {
                foreach (LoadedCharacterSelection value in characterResourceLink.m_loadedCharacterCache.Values)
                {
                    if (value != null && value.heroPrefabLink != null)
                    {
                        value.heroPrefabLink.UnloadPrefab();
                    }
                }

                characterResourceLink.m_loadedCharacterCache.Clear();
            }
        }

        foreach (GameObject prefab in s_loadedActorDataPrefabCache.Values)
        {
            ClientScene.UnregisterPrefab(prefab);
            ActorData actorData = prefab.GetComponent<ActorData>();
            if (actorData != null)
            {
                foreach (GameObject netObject in actorData.m_additionalNetworkObjectsToRegister)
                {
                    if (netObject != null)
                    {
                        ClientScene.UnregisterPrefab(netObject);
                    }
                }
            }
        }

        s_loadedActorDataPrefabCache.Clear();
        s_links.Clear();
        DestroyAudioResources();
    }

    public PrefabResourceLink GetHeroPrefabLinkFromSelection(CharacterVisualInfo selection, out CharacterSkin skin)
    {
        if (selection.skinIndex < 0
            || selection.patternIndex < 0
            || selection.colorIndex < 0)
        {
            skin = null;
            return null;
        }

        if (selection.skinIndex >= m_skins.Count)
        {
            skin = null;
            return null;
        }

        skin = m_skins[selection.skinIndex];
        if (selection.patternIndex >= skin.m_patterns.Count)
        {
            return null;
        }

        CharacterPattern characterPattern = skin.m_patterns[selection.patternIndex];
        return selection.colorIndex < characterPattern.m_colors.Count
            ? characterPattern.m_colors[selection.colorIndex].m_heroPrefab
            : null;
    }

    private void LoadPKFXForGameStatus(GameStatus gamestatus, CharacterSkin skin)
    {
        if (!m_willEverHavePkfx || skin == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(skin.m_pkfxDirectory) && string.IsNullOrEmpty(m_pkfxDirectoryDefault))
        {
            Log.Error($"Character {name} (skin: {skin.m_name}) needs pkfx path set to preload VFX. " +
                      "Until then, you may see a hitch when spawning vfx for this character the first time.");
            return;
        }

        if (gamestatus >= GameStatus.Launched)
        {
            ClientVFXLoader clientVFXLoader = ClientVFXLoader.Get();
            string pathRoot = "PackFx/Character/Hero";
            string pathDir = string.IsNullOrEmpty(skin.m_pkfxDirectory)
                ? m_pkfxDirectoryDefault
                : skin.m_pkfxDirectory;
            clientVFXLoader.QueuePKFXDirectoryForPreload(Path.Combine(pathRoot, pathDir));
        }
    }

    private IEnumerator CharacterLoadAudioAssetsForGameStatus(GameStatus gamestatus, CharacterSkin skin)
    {
        PrefabResourceLink[] audioAssetsLinks = null;
        Dictionary<string, GameObject> instantiatedAudioResources;
        if (gamestatus >= GameStatus.Launched && gamestatus.IsActiveStatus())
        {
            instantiatedAudioResources = s_instantiatedInGameAudioResources;
            if (skin != null
                && skin.m_audioAssetsInGamePrefabs != null
                && !skin.m_audioAssetsInGamePrefabs.IsNullOrEmpty())
            {
                audioAssetsLinks = skin.m_audioAssetsInGamePrefabs;
            }
            else if (m_audioAssetsInGameDefaultPrefabs != null
                     && !m_audioAssetsInGameDefaultPrefabs.IsNullOrEmpty())
            {
                audioAssetsLinks = m_audioAssetsInGameDefaultPrefabs;
            }
            else if (Application.isEditor)
            {
                Log.Warning("Yannis/audio team, please set up prefabs: " +
                            $"CharacterResourceLink {name} has no audio assets in game default, and no override for a skin.");
            }
        }
        else
        {
            instantiatedAudioResources = s_instantiatedFrontEndAudioResources;
            if (skin != null
                && skin.m_audioAssetsFrontEndPrefabs != null
                && !skin.m_audioAssetsFrontEndPrefabs.IsNullOrEmpty())
            {
                audioAssetsLinks = skin.m_audioAssetsFrontEndPrefabs;
            }
            else if (m_audioAssetsFrontEndDefaultPrefabs != null
                     && !m_audioAssetsFrontEndDefaultPrefabs.IsNullOrEmpty())
            {
                audioAssetsLinks = m_audioAssetsFrontEndDefaultPrefabs;
            }
            else if (Application.isEditor)
            {
                Log.Warning("Yannis/audio team, please set up prefabs: " +
                            $"CharacterResourceLink {name} has no audio assets front end default, and no override for a skin.");
            }
        }

        bool skipByConfig = HydrogenConfig.Get() != null && HydrogenConfig.Get().SkipAudioEvents;
        if (skipByConfig)
        {
            audioAssetsLinks = null;
        }

        if (audioAssetsLinks != null)
        {
            foreach (PrefabResourceLink audioAssetsLink in audioAssetsLinks)
            {
                if (instantiatedAudioResources.ContainsKey(audioAssetsLink.GUID))
                {
                    continue;
                }

                instantiatedAudioResources[audioAssetsLink.GUID] = null;
                IEnumerator e = audioAssetsLink.PreLoadPrefabAsync();
                do
                {
                    yield return e.Current;
                } while (e.MoveNext());

                GameObject audioPrefabsInst = audioAssetsLink.InstantiatePrefab(true);
                if (audioPrefabsInst != null)
                {
                    instantiatedAudioResources[audioAssetsLink.GUID] = audioPrefabsInst;
                    DontDestroyOnLoad(instantiatedAudioResources[audioAssetsLink.GUID]);
                    foreach (ChatterComponent chatterComponent in instantiatedAudioResources[audioAssetsLink.GUID]
                                 .GetComponents<ChatterComponent>())
                    {
                        chatterComponent.SetCharacterResourceLink(this);
                    }

                    AudioManager.StandardizeAudioLinkages(audioPrefabsInst);
                }
            }
        }
    }

    public void AdvanceSelector(ref CharacterVisualInfo skinSelector)
    {
        skinSelector.colorIndex++;
        if (skinSelector.colorIndex >=
            m_skins[skinSelector.skinIndex].m_patterns[skinSelector.patternIndex].m_colors.Count)
        {
            skinSelector.colorIndex = 0;
            skinSelector.patternIndex++;
            if (skinSelector.patternIndex >= m_skins[skinSelector.skinIndex].m_patterns.Count)
            {
                skinSelector.patternIndex = 0;
                skinSelector.skinIndex++;
                if (skinSelector.skinIndex >= m_skins.Count)
                {
                    skinSelector.skinIndex = 0;
                }
            }
        }
    }

    public Sprite GetCharacterRoleIcon()
    {
        return GetCharacterRoleSprite(m_characterRole);
    }

    public static Sprite GetCharacterRoleSprite(CharacterRole role)
    {
        string path;
        switch (role)
        {
            case CharacterRole.Tank:
                path = "iconTank";
                break;
            case CharacterRole.Assassin:
                path = "iconAssassin";
                break;
            case CharacterRole.Support:
                path = "iconSupport";
                break;
            default:
                return null;
        }

        return Resources.Load<Sprite>(path);
    }

    public Sprite GetCharacterIcon()
    {
        return (Sprite)Resources.Load(m_characterIconResourceString, typeof(Sprite));
    }

    public Sprite GetCharacterSelectIcon()
    {
#if EVOS
        //Fill in draft
        string iconPath;

        switch (m_characterType)
        {
            // Override icons only shows in draft
            case CharacterType.PendingWillFill:
                iconPath = "iconAssassin";
                break;
            case CharacterType.TestFreelancer1:
                iconPath = "iconTank";
                break;
            case CharacterType.TestFreelancer2:
                iconPath = "iconSupport";
                break;
            default:
                iconPath = m_characterSelectIconResourceString;
                break;
        }

        return (Sprite)Resources.Load(iconPath, typeof(Sprite));
#else
        return (Sprite)Resources.Load(m_characterSelectIconResourceString, typeof(Sprite));
#endif
    }

    public Sprite GetCharacterSelectIconBW()
    {
        return (Sprite)Resources.Load(m_characterSelectIcon_bwResourceString, typeof(Sprite));
    }

    public Sprite GetLoadingProfileIcon()
    {
        return (Sprite)Resources.Load(m_loadingProfileIconResourceString, typeof(Sprite));
    }

    public CharacterColor GetCharacterColor(CharacterVisualInfo skinSelector)
    {
        return m_skins[skinSelector.skinIndex].m_patterns[skinSelector.patternIndex].m_colors[skinSelector.colorIndex];
    }

    public bool IsVisualInfoSelectionValid(CharacterVisualInfo selection)
    {
        return selection.skinIndex >= 0
               && selection.skinIndex < m_skins.Count
               && selection.patternIndex >= 0
               && selection.patternIndex < m_skins[selection.skinIndex].m_patterns.Count
               && selection.colorIndex >= 0
               && selection.colorIndex < m_skins[selection.skinIndex].m_patterns[selection.patternIndex].m_colors.Count;
    }

    public bool IsAbilityVfxSwapSelectionValid(CharacterAbilityVfxSwapInfo abilityVfxSwaps)
    {
        return IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility0, m_vfxSwapsForAbility0)
               && IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility1, m_vfxSwapsForAbility1)
               && IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility2, m_vfxSwapsForAbility2)
               && IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility3, m_vfxSwapsForAbility3)
               && IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility4, m_vfxSwapsForAbility4);
    }

    private static bool IsSelectedVfxSwapForAbilityValid(int selectedVfxSwap,
        List<CharacterAbilityVfxSwap> resourceLinkVfxSwaps)
    {
        if (selectedVfxSwap == 0)
        {
            return true;
        }

        if (resourceLinkVfxSwaps == null)
        {
            return false;
        }

        foreach (CharacterAbilityVfxSwap vfxSwap in resourceLinkVfxSwaps)
        {
            if (vfxSwap.m_uniqueID == selectedVfxSwap)
            {
                return true;
            }
        }

        return false;
    }

    private static CharacterAbilityVfxSwap FindVfxSwapForAbility(
        int selectedVfxSwapId,
        List<CharacterAbilityVfxSwap> resourceLinkVfxSwaps,
        string resourceLinkName)
    {
        if (resourceLinkVfxSwaps == null)
        {
            Debug.LogError("Trying to find VFX swaps for an ability, but the resource link VFX swaps list is null.");
            return null;
        }

        if (selectedVfxSwapId == 0)
        {
            return null;
        }

        if (resourceLinkVfxSwaps.Count == 0)
        {
            Debug.LogError($"Trying to find VFX swaps for an ability with swap ID = {selectedVfxSwapId} " +
                           $"on resource link {resourceLinkName}, but the resource link VFX swaps list is empty.");
            return null;
        }

        foreach (CharacterAbilityVfxSwap vfxSwap in resourceLinkVfxSwaps)
        {
            if (vfxSwap.m_uniqueID == selectedVfxSwapId)
            {
                return vfxSwap;
            }
        }

        return null;
    }

    public List<CharacterAbilityVfxSwap> GetAvailableVfxSwapsForAbilityIndex(int selectedAbilityIndex)
    {
        switch (selectedAbilityIndex)
        {
            case 0:
                return m_vfxSwapsForAbility0;
            case 1:
                return m_vfxSwapsForAbility1;
            case 2:
                return m_vfxSwapsForAbility2;
            case 3:
                return m_vfxSwapsForAbility3;
            case 4:
                return m_vfxSwapsForAbility4;
            default:
                return null;
        }
    }

    public GameObject ReplaceSequence(
        GameObject originalSequencePrefab,
        CharacterVisualInfo visualInfo,
        CharacterAbilityVfxSwapInfo abilityVfxSwapsInfo)
    {
        if (originalSequencePrefab == null)
        {
            return null;
        }

        if (!IsVisualInfoSelectionValid(visualInfo))
        {
            Debug.LogError($"Invalid visual info ({visualInfo.ToString()}) for character resource link {ToString()}, " +
                           "resetting to default...");
            visualInfo.ResetToDefault();
        }

        if (!IsAbilityVfxSwapSelectionValid(abilityVfxSwapsInfo))
        {
            Debug.LogError(
                $"Invalid ability vfx swap info ({abilityVfxSwapsInfo.ToString()}) for character resource link {ToString()}, " +
                "resetting to default...");
            abilityVfxSwapsInfo.Reset();
        }

        GameObject prefab = ReplaceSequenceViaCharacterAbilityVfxSwapInfo(originalSequencePrefab, abilityVfxSwapsInfo);
        if (prefab != null)
        {
            return prefab;
        }

        CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
        CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
        CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
        prefab = ReplaceSequence(originalSequencePrefab, characterColor.m_replacementSequences);
        if (prefab != null)
        {
            return prefab;
        }

        prefab = ReplaceSequence(originalSequencePrefab, characterPattern.m_replacementSequences);
        if (prefab != null)
        {
            return prefab;
        }

        prefab = ReplaceSequence(originalSequencePrefab, characterSkin.m_replacementSequences);
        if (prefab != null)
        {
            return prefab;
        }

        return originalSequencePrefab;
    }

    private GameObject ReplaceSequenceViaCharacterAbilityVfxSwapInfo(GameObject originalSequencePrefab,
        CharacterAbilityVfxSwapInfo swapInfo)
    {
        if (swapInfo.VfxSwapForAbility0 != 0)
        {
            CharacterAbilityVfxSwap characterAbilityVfxSwap =
                FindVfxSwapForAbility(swapInfo.VfxSwapForAbility0, m_vfxSwapsForAbility0, name);
            if (characterAbilityVfxSwap != null)
            {
                GameObject prefab =
                    ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap.m_replacementSequences);
                if (prefab != null)
                {
                    return prefab;
                }
            }
        }

        if (swapInfo.VfxSwapForAbility1 != 0)
        {
            CharacterAbilityVfxSwap characterAbilityVfxSwap2 =
                FindVfxSwapForAbility(swapInfo.VfxSwapForAbility1, m_vfxSwapsForAbility1, name);
            if (characterAbilityVfxSwap2 != null)
            {
                GameObject prefab = ReplaceSequence(originalSequencePrefab,
                    characterAbilityVfxSwap2.m_replacementSequences);
                if (prefab != null)
                {
                    return prefab;
                }
            }
        }

        if (swapInfo.VfxSwapForAbility2 != 0)
        {
            CharacterAbilityVfxSwap characterAbilityVfxSwap3 =
                FindVfxSwapForAbility(swapInfo.VfxSwapForAbility2, m_vfxSwapsForAbility2, name);
            if (characterAbilityVfxSwap3 != null)
            {
                GameObject prefab = ReplaceSequence(originalSequencePrefab,
                    characterAbilityVfxSwap3.m_replacementSequences);
                if (prefab != null)
                {
                    return prefab;
                }
            }
        }

        if (swapInfo.VfxSwapForAbility3 != 0)
        {
            CharacterAbilityVfxSwap characterAbilityVfxSwap4 =
                FindVfxSwapForAbility(swapInfo.VfxSwapForAbility3, m_vfxSwapsForAbility3, name);
            if (characterAbilityVfxSwap4 != null)
            {
                GameObject prefab = ReplaceSequence(originalSequencePrefab,
                    characterAbilityVfxSwap4.m_replacementSequences);
                if (prefab != null)
                {
                    return prefab;
                }
            }
        }

        if (swapInfo.VfxSwapForAbility4 != 0)
        {
            CharacterAbilityVfxSwap characterAbilityVfxSwap5 =
                FindVfxSwapForAbility(swapInfo.VfxSwapForAbility4, m_vfxSwapsForAbility4, name);
            if (characterAbilityVfxSwap5 != null)
            {
                GameObject prefab = ReplaceSequence(originalSequencePrefab,
                    characterAbilityVfxSwap5.m_replacementSequences);
                if (prefab != null)
                {
                    return prefab;
                }
            }
        }

        return null;
    }

    private GameObject ReplaceSequence(GameObject originalSequencePrefab, PrefabReplacement[] replacements)
    {
        if (replacements == null)
        {
            return null;
        }

        foreach (PrefabReplacement prefabReplacement in replacements)
        {
            if (prefabReplacement.OriginalPrefab.GetPrefab(true) == originalSequencePrefab)
            {
                return prefabReplacement.Replacement.GetPrefab(true);
            }
        }

        return null;
    }

    public string ReplaceAudioEvent(string audioEvent, CharacterVisualInfo visualInfo)
    {
        if (string.IsNullOrEmpty(audioEvent))
        {
            return string.Empty;
        }

        if (visualInfo.skinIndex >= 0 && visualInfo.skinIndex < m_skins.Count)
        {
            CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
            foreach (AudioReplacement audioReplacement in characterSkin.m_replacementAudio)
            {
                audioEvent = audioEvent.Replace(audioReplacement.OriginalString, audioReplacement.Replacement);
            }
        }

        return audioEvent;
    }

    public bool HasAudioEventReplacements(CharacterVisualInfo visualInfo)
    {
        if (visualInfo.skinIndex < 0
            || visualInfo.skinIndex >= m_skins.Count)
        {
            return false;
        }

        CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
        return characterSkin.m_replacementAudio != null
               && characterSkin.m_replacementAudio.Length > 0;
    }

    public bool AllowAudioTag(string audioTag, CharacterVisualInfo visualInfo)
    {
        CharacterSkin characterSkin = null;
        if (visualInfo.skinIndex >= 0 && visualInfo.skinIndex < m_skins.Count)
        {
            characterSkin = m_skins[visualInfo.skinIndex];
        }

        return characterSkin != null
               && characterSkin.m_allowedAudioTags != null
               && characterSkin.m_allowedAudioTags.Length != 0
            ? characterSkin.m_allowedAudioTags.Contains(audioTag)
            : audioTag == "default";
    }

    internal PrefabResourceLink ReplacePrefabResourceLink(
        PrefabResourceLink originalPrefabResourceLink,
        CharacterVisualInfo visualInfo)
    {
        if (originalPrefabResourceLink == null)
        {
            return null;
        }

        if (!IsVisualInfoSelectionValid(visualInfo))
        {
            Debug.LogError($"Invalid visual info ({visualInfo.ToString()}) for character resource link {ToString()}, " +
                           $"resetting to default...");
            visualInfo.ResetToDefault();
        }

        CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
        CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
        CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
        PrefabResourceLink prefabResourceLink =
            ReplacePrefabResourceLink(originalPrefabResourceLink, characterColor.m_replacementSequences);
        if (prefabResourceLink != null)
        {
            return prefabResourceLink;
        }

        prefabResourceLink =
            ReplacePrefabResourceLink(originalPrefabResourceLink, characterPattern.m_replacementSequences);
        if (prefabResourceLink != null)
        {
            return prefabResourceLink;
        }

        prefabResourceLink =
            ReplacePrefabResourceLink(originalPrefabResourceLink, characterSkin.m_replacementSequences);
        if (prefabResourceLink != null)
        {
            return prefabResourceLink;
        }

        return originalPrefabResourceLink;
    }

    private PrefabResourceLink ReplacePrefabResourceLink(
        PrefabResourceLink originalPrefabResourceLink,
        PrefabReplacement[] replacements)
    {
        if (replacements == null)
        {
            return null;
        }

        foreach (PrefabReplacement prefabReplacement in replacements)
        {
            if (prefabReplacement.OriginalPrefab.ResourcePath == originalPrefabResourceLink.ResourcePath)
            {
                return prefabReplacement.Replacement;
            }
        }

        return null;
    }

    public GameBalanceVars.CharacterUnlockData CreateUnlockData()
    {
        GameBalanceVars.CharacterUnlockData characterUnlockData = new GameBalanceVars.CharacterUnlockData();
        characterUnlockData.character = m_characterType;
        m_charUnlockData.CopyValuesTo(characterUnlockData);
        characterUnlockData.Name = m_displayName;
        List<GameBalanceVars.SkinUnlockData> skinUnlocks = new List<GameBalanceVars.SkinUnlockData>();
        for (int skinIdx = 0; skinIdx < m_skins.Count; skinIdx++)
        {
            CharacterSkin characterSkin = m_skins[skinIdx];
            GameBalanceVars.SkinUnlockData skinUnlockData = new GameBalanceVars.SkinUnlockData();
            characterSkin.m_skinUnlockData.CopyValuesTo(skinUnlockData);
            skinUnlockData.m_isHidden = characterSkin.m_isHidden;
            skinUnlockData.Name = characterSkin.m_name;
            skinUnlockData.SetCharacterTypeInt((int)m_characterType);
            skinUnlockData.SetID(skinIdx);
            List<GameBalanceVars.PatternUnlockData> patternUnlocks = new List<GameBalanceVars.PatternUnlockData>();
            for (int patternIdx = 0; patternIdx < characterSkin.m_patterns.Count; patternIdx++)
            {
                CharacterPattern characterPattern = characterSkin.m_patterns[patternIdx];
                GameBalanceVars.PatternUnlockData patternUnlockData = new GameBalanceVars.PatternUnlockData();
                characterPattern.m_patternUnlockData.CopyValuesTo(patternUnlockData);
                patternUnlockData.m_isHidden = characterPattern.m_isHidden;
                patternUnlockData.Name = characterPattern.m_name;
                patternUnlockData.SetCharacterTypeInt((int)m_characterType);
                patternUnlockData.SetSkinIndex(skinIdx);
                patternUnlockData.SetID(patternIdx);
                List<GameBalanceVars.ColorUnlockData> colorUnlocks = new List<GameBalanceVars.ColorUnlockData>();
                for (int colorIdx = 0; colorIdx < characterPattern.m_colors.Count; colorIdx++)
                {
                    CharacterColor characterColor = characterPattern.m_colors[colorIdx];
                    GameBalanceVars.ColorUnlockData colorUnlockData = new GameBalanceVars.ColorUnlockData();
                    characterColor.m_colorUnlockData.CopyValuesTo(colorUnlockData);
                    colorUnlockData.m_isHidden = characterColor.m_isHidden;
                    colorUnlockData.m_sortOrder = characterColor.m_sortOrder;
                    colorUnlockData.Name = characterColor.m_name;
                    colorUnlockData.SetCharacterTypeInt((int)m_characterType);
                    colorUnlockData.SetSkinIndex(skinIdx);
                    colorUnlockData.SetPatternIndex(patternIdx);
                    colorUnlockData.SetID(colorIdx);
                    colorUnlocks.Add(colorUnlockData);
                }

                patternUnlockData.colorUnlockData = colorUnlocks.ToArray();
                patternUnlocks.Add(patternUnlockData);
            }

            skinUnlockData.patternUnlockData = patternUnlocks.ToArray();
            skinUnlocks.Add(skinUnlockData);
        }

        characterUnlockData.skinUnlockData = skinUnlocks.ToArray();
        List<GameBalanceVars.TauntUnlockData> tauntUnlocks = new List<GameBalanceVars.TauntUnlockData>();
        for (int tauntIdx = 0; tauntIdx < m_taunts.Count; tauntIdx++)
        {
            GameBalanceVars.TauntUnlockData tauntUnlockData = m_taunts[tauntIdx].m_tauntUnlockData.Clone();
            tauntUnlockData.Name = m_taunts[tauntIdx].m_tauntName;
            tauntUnlockData.m_isHidden = m_taunts[tauntIdx].m_isHidden;
            tauntUnlockData.SetCharacterTypeInt((int)m_characterType);
            tauntUnlockData.SetID(tauntIdx);
            tauntUnlocks.Add(tauntUnlockData);
        }

        characterUnlockData.tauntUnlockData = tauntUnlocks.ToArray();
        List<GameBalanceVars.AbilityVfxUnlockData> vfxUnlocks = new List<GameBalanceVars.AbilityVfxUnlockData>();
        GenerateVfxSwapUnlockData(m_vfxSwapsForAbility0, 0, vfxUnlocks);
        GenerateVfxSwapUnlockData(m_vfxSwapsForAbility1, 1, vfxUnlocks);
        GenerateVfxSwapUnlockData(m_vfxSwapsForAbility2, 2, vfxUnlocks);
        GenerateVfxSwapUnlockData(m_vfxSwapsForAbility3, 3, vfxUnlocks);
        GenerateVfxSwapUnlockData(m_vfxSwapsForAbility4, 4, vfxUnlocks);
        characterUnlockData.abilityVfxUnlockData = vfxUnlocks.ToArray();
        return characterUnlockData;
    }

    private void GenerateVfxSwapUnlockData(
        List<CharacterAbilityVfxSwap> input,
        int abilityIndex,
        List<GameBalanceVars.AbilityVfxUnlockData> genUnlockDataList)
    {
        if (input == null)
        {
            Debug.LogWarning("Vfx Swap Data is null on " + gameObject.name);
            return;
        }

        foreach (CharacterAbilityVfxSwap vfxSwap in input)
        {
            GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = vfxSwap.m_vfxSwapUnlockData.Clone();
            abilityVfxUnlockData.m_isHidden = vfxSwap.m_isHidden;
            abilityVfxUnlockData.SetCharacterTypeInt((int)m_characterType);
            abilityVfxUnlockData.SetSwapAbilityId(abilityIndex);
            abilityVfxUnlockData.SetID(vfxSwap.m_uniqueID);
            abilityVfxUnlockData.Name = vfxSwap.m_swapName;
            genUnlockDataList.Add(abilityVfxUnlockData);
        }
    }

    internal delegate void CharacterResourceDelegate(LoadedCharacterSelection loadedCharacter);
}