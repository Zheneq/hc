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

	[Separator("Scale/Offset in frontend UI (character select, collections)", true)]
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

	internal const string c_heroPKFXRelativePath = "PackFx/Character/Hero";

	[LeafDirectoryPopup("Directory containing all .pkfx files for this skin", "PackFx/Character/Hero")]
	public string m_pkfxDirectoryDefault;

	private static List<CharacterResourceLink> s_links = new List<CharacterResourceLink>();

	private Dictionary<CharacterVisualInfo, LoadedCharacterSelection> m_loadedCharacterCache = new Dictionary<CharacterVisualInfo, LoadedCharacterSelection>();

	private static Dictionary<string, GameObject> s_loadedActorDataPrefabCache = new Dictionary<string, GameObject>();

	private static Dictionary<string, GameObject> s_instantiatedInGameAudioResources = new Dictionary<string, GameObject>();

	private static Dictionary<string, GameObject> s_instantiatedFrontEndAudioResources = new Dictionary<string, GameObject>();

	private const string kAssassionIcon = "iconAssassin";

	private const string kSupportIcon = "iconSupport";

	private const string kTankIcon = "iconTank";

	internal GameObject ActorDataPrefab
	{
		get
		{
			GameObject gameObject = null;
			if (CharacterResourceLink.s_loadedActorDataPrefabCache.TryGetValue(this.m_actorDataResourcePath, out gameObject))
			{
				if (!(gameObject == null))
				{
					return gameObject;
				}
			}
			gameObject = Resources.Load<GameObject>(this.m_actorDataResourcePath);
			if (gameObject != null)
			{
				CharacterResourceLink.s_loadedActorDataPrefabCache[this.m_actorDataResourcePath] = gameObject;
			}
			return gameObject;
		}
	}

	private void Awake()
	{
		CharacterResourceLink.s_links.Add(this);
	}

	public string GetDisplayName()
	{
		return StringUtil.TR_CharacterName(this.m_characterType.ToString());
	}

	public string GetCharSelectTooltipDescription()
	{
		return StringUtil.TR_CharacterSelectTooltip(this.m_characterType.ToString());
	}

	public string GetCharSelectAboutDescription()
	{
		return StringUtil.TR_CharacterSelectAboutDesc(this.m_characterType.ToString());
	}

	public string GetCharBio()
	{
		return StringUtil.TR_CharacterBio(this.m_characterType.ToString());
	}

	public string GetSkinName(int skinIndex)
	{
		return StringUtil.TR_CharacterSkinName(this.m_characterType.ToString(), skinIndex + 1);
	}

	public string GetSkinDescription(int skinIndex)
	{
		return StringUtil.TR_CharacterSkinDescription(this.m_characterType.ToString(), skinIndex + 1);
	}

	public string GetSkinFlavorText(int skinIndex)
	{
		return StringUtil.TR_CharacterSkinFlavor(this.m_characterType.ToString(), skinIndex + 1);
	}

	public string GetPatternName(int skinIndex, int patternIndex)
	{
		return StringUtil.TR_CharacterPatternName(this.m_characterType.ToString(), skinIndex + 1, patternIndex + 1);
	}

	public string GetPatternColorName(int skinIndex, int patternIndex, int colorIndex)
	{
		return StringUtil.TR_CharacterPatternColorName(this.m_characterType.ToString(), skinIndex + 1, patternIndex + 1, colorIndex + 1);
	}

	public string GetPatternColorDescription(int skinIndex, int patternIndex, int colorIndex)
	{
		return StringUtil.TR_CharacterPatternColorDescription(this.m_characterType.ToString(), skinIndex + 1, patternIndex + 1, colorIndex + 1);
	}

	public string GetPatternColorFlavor(int skinIndex, int patternIndex, int colorIndex)
	{
		return StringUtil.TR_CharacterPatternColorFlavor(this.m_characterType.ToString(), skinIndex + 1, patternIndex + 1, colorIndex + 1);
	}

	public string GetTauntName(int tauntIndex)
	{
		return StringUtil.TR_CharacterTauntName(this.m_characterType.ToString(), tauntIndex + 1);
	}

	public string GetVFXSwapName(int abilityIndex, int vfxSwapId)
	{
		return StringUtil.TR_GetCharacterVFXSwapName(this.m_characterType.ToString(), abilityIndex + 1, vfxSwapId);
	}

	internal static void DestroyAudioResources()
	{
		using (Dictionary<string, GameObject>.Enumerator enumerator = CharacterResourceLink.s_instantiatedInGameAudioResources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, GameObject> keyValuePair = enumerator.Current;
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
		}
		CharacterResourceLink.s_instantiatedInGameAudioResources.Clear();
		using (Dictionary<string, GameObject>.Enumerator enumerator2 = CharacterResourceLink.s_instantiatedFrontEndAudioResources.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<string, GameObject> keyValuePair2 = enumerator2.Current;
				UnityEngine.Object.Destroy(keyValuePair2.Value);
			}
		}
		CharacterResourceLink.s_instantiatedFrontEndAudioResources.Clear();
	}

	internal void LoadAsync(CharacterVisualInfo selection, CharacterResourceLink.CharacterResourceDelegate onCharacterPrefabLoaded)
	{
		int num;
		this.LoadAsync(selection, out num, onCharacterPrefabLoaded);
	}

	internal void LoadAsync(CharacterVisualInfo selection, CharacterResourceLink.CharacterResourceDelegate onCharacterPrefabLoaded, GameStatus gameStatusForAssets)
	{
		int num;
		this.LoadAsync(selection, out num, onCharacterPrefabLoaded, gameStatusForAssets, 0f);
	}

	internal void LoadAsync(CharacterVisualInfo selection, out int asyncTicket, CharacterResourceLink.CharacterResourceDelegate onCharacterPrefabLoaded)
	{
		this.LoadAsync(selection, out asyncTicket, onCharacterPrefabLoaded, GameManager.Get().GameStatus, 0f);
	}

	internal void LoadAsync(CharacterVisualInfo selection, out int asyncTicket, float delay, CharacterResourceLink.CharacterResourceDelegate onCharacterPrefabLoaded)
	{
		this.LoadAsync(selection, out asyncTicket, onCharacterPrefabLoaded, GameManager.Get().GameStatus, delay);
	}

	private void LoadAsync(CharacterVisualInfo selection, out int asyncTicket, CharacterResourceLink.CharacterResourceDelegate onCharacterPrefabLoaded, GameStatus gameStatusForAssets, float delay = 0f)
	{
		if (onCharacterPrefabLoaded == null)
		{
			throw new ArgumentNullException("onCharacterPrefabLoaded");
		}
		AsyncManager.Get().StartAsyncOperation(out asyncTicket, this.CharacterLoadCoroutine(selection, onCharacterPrefabLoaded, gameStatusForAssets), delay);
	}

	private IEnumerator CharacterLoadCoroutine(CharacterVisualInfo selection, CharacterResourceLink.CharacterResourceDelegate onCharacterPrefabLoaded, GameStatus gameStatusForAssets)
	{
		if (!this.IsVisualInfoSelectionValid(selection))
		{
			Log.Warning(Log.Category.Loading, "Invalid skin selection used to load CharacterType " + this.m_characterType.ToString() + ", reverting to default. Input = " + selection.ToString(), new object[0]);
			selection.ResetToDefault();
		}
		LoadedCharacterSelection loadedCharacter;
		while (this.m_loadedCharacterCache.TryGetValue(selection, out loadedCharacter))
		{
			if (!loadedCharacter.isLoading)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_117;
				}
			}
			else
			{
				yield return null;
			}
		}
		IL_117:
		if (loadedCharacter != null)
		{
			Log.Info(Log.Category.Loading, string.Concat(new string[]
			{
				"Character ",
				base.name,
				" ",
				selection.ToString(),
				" already loading has finished - falling through"
			}), new object[0]);
			CharacterSkin skin = null;
			if (loadedCharacter != null)
			{
				int skinIndex = loadedCharacter.selectedSkin.skinIndex;
				if (skinIndex >= 0 && skinIndex < this.m_skins.Count)
				{
					skin = this.m_skins[skinIndex];
				}
				else
				{
					Log.Error("Selected skin index is out of bounds, using default. Input value = " + skinIndex, new object[0]);
					if (this.m_skins.Count > 0)
					{
						skin = this.m_skins[0];
					}
				}
			}
			this.LoadPKFXForGameStatus(gameStatusForAssets, skin);
			IEnumerator e = this.CharacterLoadAudioAssetsForGameStatus(gameStatusForAssets, skin);
			do
			{
				yield return e.Current;
			}
			while (e.MoveNext());
		}
		else
		{
			Log.Info(Log.Category.Loading, "Starting async load for Character " + base.name + " " + selection.ToString(), new object[0]);
			loadedCharacter = new LoadedCharacterSelection();
			loadedCharacter.isLoading = true;
			this.m_loadedCharacterCache.Add(selection, loadedCharacter);
			loadedCharacter.ActorDataPrefab = this.ActorDataPrefab;
			loadedCharacter.resourceLink = this;
			loadedCharacter.selectedSkin = selection;
			CharacterSkin skin2 = null;
			loadedCharacter.heroPrefabLink = this.GetHeroPrefabLinkFromSelection(selection, out skin2);
			if (loadedCharacter.heroPrefabLink != null)
			{
				if (!loadedCharacter.heroPrefabLink.IsEmpty)
				{
					goto IL_40E;
				}
			}
			Log.Error(string.Format("Character {0} could not find Actor Skin resource link for {1}.  Loading default instead...", this.m_displayName, selection.ToString()), new object[0]);
			selection.ResetToDefault();
			loadedCharacter.heroPrefabLink = this.GetHeroPrefabLinkFromSelection(selection, out skin2);
			IL_40E:
			if (!NetworkClient.active)
			{
				if (HydrogenConfig.Get().SkipCharacterModelSpawnOnServer)
				{
					goto IL_649;
				}
			}
			Log.Info(Log.Category.Loading, "Starting async load for actor model prefab for Character " + base.name + " " + selection.ToString(), new object[0]);
			IEnumerator e2 = loadedCharacter.heroPrefabLink.PreLoadPrefabAsync();
			do
			{
				yield return e2.Current;
			}
			while (e2.MoveNext());
			if (!PrefabResourceLink.HasLoadedResourceLinkForPath(loadedCharacter.heroPrefabLink.GetResourcePath()))
			{
				if (!selection.IsDefaultSelection())
				{
					Log.Error(string.Format("Character {0} could not load SavedResourceLink for {1}.  Loading default instead...", this.m_displayName, selection.ToString()), new object[0]);
					selection.ResetToDefault();
					loadedCharacter.heroPrefabLink = this.GetHeroPrefabLinkFromSelection(selection, out skin2);
					Log.Error(string.Concat(new string[]
					{
						"Starting async load for actor model prefab for Character ",
						base.name,
						" ",
						selection.ToString(),
						" (as fallback)"
					}), new object[0]);
					e2 = loadedCharacter.heroPrefabLink.PreLoadPrefabAsync();
					do
					{
						yield return e2.Current;
					}
					while (e2.MoveNext());
				}
			}
			this.LoadPKFXForGameStatus(gameStatusForAssets, skin2);
			IL_649:
			e2 = this.CharacterLoadAudioAssetsForGameStatus(gameStatusForAssets, skin2);
			do
			{
				yield return e2.Current;
			}
			while (e2.MoveNext());
			loadedCharacter.isLoading = false;
		}
		ClientScene.RegisterPrefab(loadedCharacter.ActorDataPrefab);
		ActorData actorDataComp = loadedCharacter.ActorDataPrefab.GetComponent<ActorData>();
		if (actorDataComp != null)
		{
			for (int i = 0; i < actorDataComp.m_additionalNetworkObjectsToRegister.Count; i++)
			{
				GameObject gameObject = actorDataComp.m_additionalNetworkObjectsToRegister[i];
				if (gameObject != null)
				{
					ClientScene.RegisterPrefab(gameObject);
				}
			}
		}
		onCharacterPrefabLoaded(loadedCharacter);
		yield return loadedCharacter;
		yield break;
	}

	internal void CancelLoad(CharacterVisualInfo selection, int asyncTicket)
	{
		LoadedCharacterSelection loadedCharacterSelection;
		if (this.m_loadedCharacterCache.TryGetValue(selection, out loadedCharacterSelection))
		{
			if (loadedCharacterSelection != null && loadedCharacterSelection.isLoading)
			{
				this.m_loadedCharacterCache.Remove(selection);
				if (AsyncManager.Get() != null)
				{
					AsyncManager.Get().CancelAsyncOperation(asyncTicket);
				}
			}
		}
	}

	internal void UnloadSkinsNotInList(List<CharacterVisualInfo> skins)
	{
		List<CharacterVisualInfo> list = new List<CharacterVisualInfo>();
		using (Dictionary<CharacterVisualInfo, LoadedCharacterSelection>.Enumerator enumerator = this.m_loadedCharacterCache.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<CharacterVisualInfo, LoadedCharacterSelection> keyValuePair = enumerator.Current;
				LoadedCharacterSelection value = keyValuePair.Value;
				if (value != null)
				{
					if (!skins.Contains(value.selectedSkin))
					{
						if (value.heroPrefabLink != null)
						{
							value.heroPrefabLink.UnloadPrefab();
							list.Add(value.selectedSkin);
						}
					}
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.m_loadedCharacterCache.Remove(list[i]);
		}
	}

	internal static void UnloadAll()
	{
		if (GameWideData.Get() != null)
		{
			foreach (CharacterResourceLink characterResourceLink in GameWideData.Get().m_characterResourceLinks)
			{
				using (Dictionary<CharacterVisualInfo, LoadedCharacterSelection>.Enumerator enumerator = characterResourceLink.m_loadedCharacterCache.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<CharacterVisualInfo, LoadedCharacterSelection> keyValuePair = enumerator.Current;
						LoadedCharacterSelection value = keyValuePair.Value;
						if (value != null)
						{
							if (value.heroPrefabLink != null)
							{
								value.heroPrefabLink.UnloadPrefab();
							}
						}
					}
				}
				characterResourceLink.m_loadedCharacterCache.Clear();
			}
		}
		using (Dictionary<string, GameObject>.Enumerator enumerator2 = CharacterResourceLink.s_loadedActorDataPrefabCache.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<string, GameObject> keyValuePair2 = enumerator2.Current;
				GameObject value2 = keyValuePair2.Value;
				ClientScene.UnregisterPrefab(value2);
				ActorData component = value2.GetComponent<ActorData>();
				if (component != null)
				{
					for (int j = 0; j < component.m_additionalNetworkObjectsToRegister.Count; j++)
					{
						GameObject gameObject = component.m_additionalNetworkObjectsToRegister[j];
						if (gameObject != null)
						{
							ClientScene.UnregisterPrefab(gameObject);
						}
					}
				}
			}
		}
		CharacterResourceLink.s_loadedActorDataPrefabCache.Clear();
		CharacterResourceLink.s_links.Clear();
		CharacterResourceLink.DestroyAudioResources();
	}

	public unsafe PrefabResourceLink GetHeroPrefabLinkFromSelection(CharacterVisualInfo selection, out CharacterSkin skin)
	{
		PrefabResourceLink result;
		if (selection.skinIndex >= 0)
		{
			if (selection.patternIndex >= 0)
			{
				if (selection.colorIndex >= 0)
				{
					if (selection.skinIndex < this.m_skins.Count)
					{
						skin = this.m_skins[selection.skinIndex];
						if (selection.patternIndex < skin.m_patterns.Count)
						{
							CharacterPattern characterPattern = skin.m_patterns[selection.patternIndex];
							if (selection.colorIndex < characterPattern.m_colors.Count)
							{
								result = characterPattern.m_colors[selection.colorIndex].m_heroPrefab;
							}
							else
							{
								result = null;
							}
						}
						else
						{
							result = null;
						}
					}
					else
					{
						skin = null;
						result = null;
					}
					return result;
				}
			}
		}
		skin = null;
		result = null;
		return result;
	}

	private void LoadPKFXForGameStatus(GameStatus gamestatus, CharacterSkin skin)
	{
		if (this.m_willEverHavePkfx && skin != null)
		{
			if (string.IsNullOrEmpty(skin.m_pkfxDirectory))
			{
				if (string.IsNullOrEmpty(this.m_pkfxDirectoryDefault))
				{
					Log.Error("Character {1} (skin: {0}) needs pkfx path set to preload VFX. Until then, you may see a hitch when spawning vfx for this character the first time.", new object[]
					{
						skin.m_name,
						base.name
					});
					return;
				}
			}
			if (gamestatus >= GameStatus.Launched)
			{
				ClientVFXLoader clientVFXLoader = ClientVFXLoader.Get();
				string path = "PackFx/Character/Hero";
				string path2;
				if (string.IsNullOrEmpty(skin.m_pkfxDirectory))
				{
					path2 = this.m_pkfxDirectoryDefault;
				}
				else
				{
					path2 = skin.m_pkfxDirectory;
				}
				clientVFXLoader.QueuePKFXDirectoryForPreload(Path.Combine(path, path2));
			}
		}
	}

	private IEnumerator CharacterLoadAudioAssetsForGameStatus(GameStatus gamestatus, CharacterSkin skin)
	{
		PrefabResourceLink[] audioAssetsLinks = null;
		Dictionary<string, GameObject> instantiatedAudioResources = null;
		if (gamestatus >= GameStatus.Launched)
		{
			if (gamestatus.IsActiveStatus())
			{
				instantiatedAudioResources = CharacterResourceLink.s_instantiatedInGameAudioResources;
				if (skin != null)
				{
					if (skin.m_audioAssetsInGamePrefabs != null)
					{
						if (!skin.m_audioAssetsInGamePrefabs.IsNullOrEmpty<PrefabResourceLink>())
						{
							audioAssetsLinks = skin.m_audioAssetsInGamePrefabs;
							goto IL_12C;
						}
					}
				}
				if (this.m_audioAssetsInGameDefaultPrefabs != null)
				{
					if (!this.m_audioAssetsInGameDefaultPrefabs.IsNullOrEmpty<PrefabResourceLink>())
					{
						audioAssetsLinks = this.m_audioAssetsInGameDefaultPrefabs;
						goto IL_12C;
					}
				}
				if (Application.isEditor)
				{
					Log.Warning("Yannis/audio team, please set up prefabs: CharacterResourceLink {0} has no audio assets in game default, and no override for a skin.", new object[]
					{
						base.name
					});
				}
				IL_12C:
				goto IL_201;
			}
		}
		instantiatedAudioResources = CharacterResourceLink.s_instantiatedFrontEndAudioResources;
		if (skin != null)
		{
			if (skin.m_audioAssetsFrontEndPrefabs != null)
			{
				if (!skin.m_audioAssetsFrontEndPrefabs.IsNullOrEmpty<PrefabResourceLink>())
				{
					audioAssetsLinks = skin.m_audioAssetsFrontEndPrefabs;
					goto IL_201;
				}
			}
		}
		if (this.m_audioAssetsFrontEndDefaultPrefabs != null)
		{
			if (!this.m_audioAssetsFrontEndDefaultPrefabs.IsNullOrEmpty<PrefabResourceLink>())
			{
				audioAssetsLinks = this.m_audioAssetsFrontEndDefaultPrefabs;
				goto IL_201;
			}
		}
		if (Application.isEditor)
		{
			Log.Warning("Yannis/audio team, please set up prefabs: CharacterResourceLink {0} has no audio assets front end default, and no override for a skin.", new object[]
			{
				base.name
			});
		}
		IL_201:
		bool flag;
		if (HydrogenConfig.Get() != null)
		{
			flag = HydrogenConfig.Get().SkipAudioEvents;
		}
		else
		{
			flag = false;
		}
		bool skipByConfig = flag;
		if (skipByConfig)
		{
			audioAssetsLinks = null;
		}
		if (audioAssetsLinks != null)
		{
			foreach (PrefabResourceLink audioAssetsLink in audioAssetsLinks)
			{
				if (!instantiatedAudioResources.ContainsKey(audioAssetsLink.GUID))
				{
					instantiatedAudioResources[audioAssetsLink.GUID] = null;
					IEnumerator e = audioAssetsLink.PreLoadPrefabAsync();
					do
					{
						yield return e.Current;
					}
					while (e.MoveNext());
					GameObject audioPrefabsInst = audioAssetsLink.InstantiatePrefab(true);
					if (audioPrefabsInst != null)
					{
						instantiatedAudioResources[audioAssetsLink.GUID] = audioPrefabsInst;
						UnityEngine.Object.DontDestroyOnLoad(instantiatedAudioResources[audioAssetsLink.GUID]);
						foreach (ChatterComponent chatterComponent in instantiatedAudioResources[audioAssetsLink.GUID].GetComponents<ChatterComponent>())
						{
							chatterComponent.SetCharacterResourceLink(this);
						}
						AudioManager.StandardizeAudioLinkages(audioPrefabsInst);
					}
				}
			}
		}
		yield break;
	}

	public unsafe void AdvanceSelector(ref CharacterVisualInfo skinSelector)
	{
		skinSelector.colorIndex++;
		if (skinSelector.colorIndex >= this.m_skins[skinSelector.skinIndex].m_patterns[skinSelector.patternIndex].m_colors.Count)
		{
			skinSelector.colorIndex = 0;
			skinSelector.patternIndex++;
			if (skinSelector.patternIndex >= this.m_skins[skinSelector.skinIndex].m_patterns.Count)
			{
				skinSelector.patternIndex = 0;
				skinSelector.skinIndex++;
				if (skinSelector.skinIndex >= this.m_skins.Count)
				{
					skinSelector.skinIndex = 0;
				}
			}
		}
	}

	public Sprite GetCharacterRoleIcon()
	{
		return CharacterResourceLink.GetCharacterRoleSprite(this.m_characterRole);
	}

	public static Sprite GetCharacterRoleSprite(CharacterRole role)
	{
		string path = string.Empty;
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
		return (Sprite)Resources.Load(this.m_characterIconResourceString, typeof(Sprite));
	}

	public Sprite GetCharacterSelectIcon()
	{
		return (Sprite)Resources.Load(this.m_characterSelectIconResourceString, typeof(Sprite));
	}

	public Sprite GetCharacterSelectIconBW()
	{
		return (Sprite)Resources.Load(this.m_characterSelectIcon_bwResourceString, typeof(Sprite));
	}

	public Sprite GetLoadingProfileIcon()
	{
		return (Sprite)Resources.Load(this.m_loadingProfileIconResourceString, typeof(Sprite));
	}

	public CharacterColor GetCharacterColor(CharacterVisualInfo skinSelector)
	{
		return this.m_skins[skinSelector.skinIndex].m_patterns[skinSelector.patternIndex].m_colors[skinSelector.colorIndex];
	}

	public bool IsVisualInfoSelectionValid(CharacterVisualInfo selection)
	{
		if (selection.skinIndex >= 0)
		{
			if (selection.skinIndex < this.m_skins.Count)
			{
				if (selection.patternIndex >= 0)
				{
					if (selection.patternIndex < this.m_skins[selection.skinIndex].m_patterns.Count)
					{
						if (selection.colorIndex >= 0)
						{
							if (selection.colorIndex < this.m_skins[selection.skinIndex].m_patterns[selection.patternIndex].m_colors.Count)
							{
								return true;
							}
						}
						return false;
					}
				}
				return false;
			}
		}
		return false;
	}

	public bool IsAbilityVfxSwapSelectionValid(CharacterAbilityVfxSwapInfo abilityVfxSwaps)
	{
		if (!CharacterResourceLink.IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility0, this.m_vfxSwapsForAbility0))
		{
			return false;
		}
		if (!CharacterResourceLink.IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility1, this.m_vfxSwapsForAbility1))
		{
			return false;
		}
		if (!CharacterResourceLink.IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility2, this.m_vfxSwapsForAbility2))
		{
			return false;
		}
		if (!CharacterResourceLink.IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility3, this.m_vfxSwapsForAbility3))
		{
			return false;
		}
		return CharacterResourceLink.IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility4, this.m_vfxSwapsForAbility4);
	}

	private static bool IsSelectedVfxSwapForAbilityValid(int selectedVfxSwap, List<CharacterAbilityVfxSwap> resourceLinkVfxSwaps)
	{
		bool result;
		if (selectedVfxSwap == 0)
		{
			result = true;
		}
		else if (resourceLinkVfxSwaps == null)
		{
			result = false;
		}
		else
		{
			result = false;
			for (int i = 0; i < resourceLinkVfxSwaps.Count; i++)
			{
				if (resourceLinkVfxSwaps[i].m_uniqueID == selectedVfxSwap)
				{
					return true;
				}
			}
		}
		return result;
	}

	private static CharacterAbilityVfxSwap FindVfxSwapForAbility(int selectedVfxSwapId, List<CharacterAbilityVfxSwap> resourceLinkVfxSwaps, string resourceLinkName)
	{
		CharacterAbilityVfxSwap result;
		if (resourceLinkVfxSwaps == null)
		{
			Debug.LogError("Trying to find VFX swaps for an ability, but the resource link VFX swaps list is null.");
			result = null;
		}
		else if (selectedVfxSwapId == 0)
		{
			result = null;
		}
		else if (resourceLinkVfxSwaps.Count == 0)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Trying to find VFX swaps for an ability with swap ID = ",
				selectedVfxSwapId,
				" on resource link ",
				resourceLinkName,
				", but the resource link VFX swaps list is empty."
			}));
			result = null;
		}
		else
		{
			result = null;
			for (int i = 0; i < resourceLinkVfxSwaps.Count; i++)
			{
				if (resourceLinkVfxSwaps[i].m_uniqueID == selectedVfxSwapId)
				{
					return resourceLinkVfxSwaps[i];
				}
			}
		}
		return result;
	}

	public List<CharacterAbilityVfxSwap> GetAvailableVfxSwapsForAbilityIndex(int selectedAbilityIndex)
	{
		switch (selectedAbilityIndex)
		{
		case 0:
			return this.m_vfxSwapsForAbility0;
		case 1:
			return this.m_vfxSwapsForAbility1;
		case 2:
			return this.m_vfxSwapsForAbility2;
		case 3:
			return this.m_vfxSwapsForAbility3;
		case 4:
			return this.m_vfxSwapsForAbility4;
		default:
			return null;
		}
	}

	public GameObject ReplaceSequence(GameObject originalSequencePrefab, CharacterVisualInfo visualInfo, CharacterAbilityVfxSwapInfo abilityVfxSwapsInfo)
	{
		if (originalSequencePrefab == null)
		{
			return null;
		}
		if (!this.IsVisualInfoSelectionValid(visualInfo))
		{
			Debug.LogError(string.Format("Invalid visual info ({0}) for character resource link {1}, resetting to default...", visualInfo.ToString(), this.ToString()));
			visualInfo.ResetToDefault();
		}
		if (!this.IsAbilityVfxSwapSelectionValid(abilityVfxSwapsInfo))
		{
			Debug.LogError(string.Format("Invalid ability vfx swap info ({0}) for character resource link {1}, resetting to default...", abilityVfxSwapsInfo.ToString(), this.ToString()));
			abilityVfxSwapsInfo.Reset();
		}
		GameObject gameObject = this.ReplaceSequenceViaCharacterAbilityVfxSwapInfo(originalSequencePrefab, abilityVfxSwapsInfo);
		if (gameObject != null)
		{
			return gameObject;
		}
		CharacterSkin characterSkin = this.m_skins[visualInfo.skinIndex];
		CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
		CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
		gameObject = this.ReplaceSequence(originalSequencePrefab, characterColor.m_replacementSequences);
		if (gameObject != null)
		{
			return gameObject;
		}
		gameObject = this.ReplaceSequence(originalSequencePrefab, characterPattern.m_replacementSequences);
		if (gameObject != null)
		{
			return gameObject;
		}
		gameObject = this.ReplaceSequence(originalSequencePrefab, characterSkin.m_replacementSequences);
		if (gameObject != null)
		{
			return gameObject;
		}
		return originalSequencePrefab;
	}

	private GameObject ReplaceSequenceViaCharacterAbilityVfxSwapInfo(GameObject originalSequencePrefab, CharacterAbilityVfxSwapInfo swapInfo)
	{
		if (swapInfo.VfxSwapForAbility0 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap = CharacterResourceLink.FindVfxSwapForAbility(swapInfo.VfxSwapForAbility0, this.m_vfxSwapsForAbility0, base.name);
			if (characterAbilityVfxSwap != null)
			{
				GameObject gameObject = this.ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap.m_replacementSequences);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
		}
		if (swapInfo.VfxSwapForAbility1 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap2 = CharacterResourceLink.FindVfxSwapForAbility(swapInfo.VfxSwapForAbility1, this.m_vfxSwapsForAbility1, base.name);
			if (characterAbilityVfxSwap2 != null)
			{
				GameObject gameObject2 = this.ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap2.m_replacementSequences);
				if (gameObject2 != null)
				{
					return gameObject2;
				}
			}
		}
		if (swapInfo.VfxSwapForAbility2 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap3 = CharacterResourceLink.FindVfxSwapForAbility(swapInfo.VfxSwapForAbility2, this.m_vfxSwapsForAbility2, base.name);
			if (characterAbilityVfxSwap3 != null)
			{
				GameObject gameObject3 = this.ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap3.m_replacementSequences);
				if (gameObject3 != null)
				{
					return gameObject3;
				}
			}
		}
		if (swapInfo.VfxSwapForAbility3 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap4 = CharacterResourceLink.FindVfxSwapForAbility(swapInfo.VfxSwapForAbility3, this.m_vfxSwapsForAbility3, base.name);
			if (characterAbilityVfxSwap4 != null)
			{
				GameObject gameObject4 = this.ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap4.m_replacementSequences);
				if (gameObject4 != null)
				{
					return gameObject4;
				}
			}
		}
		if (swapInfo.VfxSwapForAbility4 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap5 = CharacterResourceLink.FindVfxSwapForAbility(swapInfo.VfxSwapForAbility4, this.m_vfxSwapsForAbility4, base.name);
			if (characterAbilityVfxSwap5 != null)
			{
				GameObject gameObject5 = this.ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap5.m_replacementSequences);
				if (gameObject5 != null)
				{
					return gameObject5;
				}
			}
		}
		return null;
	}

	private GameObject ReplaceSequence(GameObject originalSequencePrefab, PrefabReplacement[] replacements)
	{
		if (replacements != null)
		{
			foreach (PrefabReplacement prefabReplacement in replacements)
			{
				if (prefabReplacement.OriginalPrefab.GetPrefab(true) == originalSequencePrefab)
				{
					return prefabReplacement.Replacement.GetPrefab(true);
				}
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
		if (visualInfo.skinIndex >= 0)
		{
			if (visualInfo.skinIndex < this.m_skins.Count)
			{
				CharacterSkin characterSkin = this.m_skins[visualInfo.skinIndex];
				foreach (AudioReplacement audioReplacement in characterSkin.m_replacementAudio)
				{
					audioEvent = audioEvent.Replace(audioReplacement.OriginalString, audioReplacement.Replacement);
				}
			}
		}
		return audioEvent;
	}

	public bool HasAudioEventReplacements(CharacterVisualInfo visualInfo)
	{
		if (visualInfo.skinIndex >= 0)
		{
			if (visualInfo.skinIndex < this.m_skins.Count)
			{
				CharacterSkin characterSkin = this.m_skins[visualInfo.skinIndex];
				bool result;
				if (characterSkin.m_replacementAudio != null)
				{
					result = (characterSkin.m_replacementAudio.Length > 0);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
		return false;
	}

	public bool AllowAudioTag(string audioTag, CharacterVisualInfo visualInfo)
	{
		CharacterSkin characterSkin = null;
		if (visualInfo.skinIndex >= 0 && visualInfo.skinIndex < this.m_skins.Count)
		{
			characterSkin = this.m_skins[visualInfo.skinIndex];
		}
		if (characterSkin != null)
		{
			if (characterSkin.m_allowedAudioTags != null)
			{
				if (characterSkin.m_allowedAudioTags.Length != 0)
				{
					return characterSkin.m_allowedAudioTags.Contains(audioTag);
				}
			}
		}
		return audioTag == "default";
	}

	internal PrefabResourceLink ReplacePrefabResourceLink(PrefabResourceLink originalPrefabResourceLink, CharacterVisualInfo visualInfo)
	{
		if (originalPrefabResourceLink == null)
		{
			return null;
		}
		if (!this.IsVisualInfoSelectionValid(visualInfo))
		{
			Debug.LogError(string.Format("Invalid visual info ({0}) for character resource link {1}, resetting to default...", visualInfo.ToString(), this.ToString()));
			visualInfo.ResetToDefault();
		}
		CharacterSkin characterSkin = this.m_skins[visualInfo.skinIndex];
		CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
		CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
		PrefabResourceLink prefabResourceLink = this.ReplacePrefabResourceLink(originalPrefabResourceLink, characterColor.m_replacementSequences);
		if (prefabResourceLink != null)
		{
			return prefabResourceLink;
		}
		prefabResourceLink = this.ReplacePrefabResourceLink(originalPrefabResourceLink, characterPattern.m_replacementSequences);
		if (prefabResourceLink != null)
		{
			return prefabResourceLink;
		}
		prefabResourceLink = this.ReplacePrefabResourceLink(originalPrefabResourceLink, characterSkin.m_replacementSequences);
		if (prefabResourceLink != null)
		{
			return prefabResourceLink;
		}
		return originalPrefabResourceLink;
	}

	private PrefabResourceLink ReplacePrefabResourceLink(PrefabResourceLink originalPrefabResourceLink, PrefabReplacement[] replacements)
	{
		if (replacements != null)
		{
			foreach (PrefabReplacement prefabReplacement in replacements)
			{
				if (prefabReplacement.OriginalPrefab.ResourcePath == originalPrefabResourceLink.ResourcePath)
				{
					return prefabReplacement.Replacement;
				}
			}
		}
		return null;
	}

	public GameBalanceVars.CharacterUnlockData CreateUnlockData()
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = new GameBalanceVars.CharacterUnlockData();
		characterUnlockData.character = this.m_characterType;
		this.m_charUnlockData.CopyValuesTo(characterUnlockData);
		characterUnlockData.Name = this.m_displayName;
		List<GameBalanceVars.SkinUnlockData> list = new List<GameBalanceVars.SkinUnlockData>();
		for (int i = 0; i < this.m_skins.Count; i++)
		{
			CharacterSkin characterSkin = this.m_skins[i];
			GameBalanceVars.SkinUnlockData skinUnlockData = new GameBalanceVars.SkinUnlockData();
			characterSkin.m_skinUnlockData.CopyValuesTo(skinUnlockData);
			skinUnlockData.m_isHidden = characterSkin.m_isHidden;
			skinUnlockData.Name = characterSkin.m_name;
			skinUnlockData.SetCharacterTypeInt((int)this.m_characterType);
			skinUnlockData.SetID(i);
			List<GameBalanceVars.PatternUnlockData> list2 = new List<GameBalanceVars.PatternUnlockData>();
			for (int j = 0; j < characterSkin.m_patterns.Count; j++)
			{
				CharacterPattern characterPattern = characterSkin.m_patterns[j];
				GameBalanceVars.PatternUnlockData patternUnlockData = new GameBalanceVars.PatternUnlockData();
				characterPattern.m_patternUnlockData.CopyValuesTo(patternUnlockData);
				patternUnlockData.m_isHidden = characterPattern.m_isHidden;
				patternUnlockData.Name = characterPattern.m_name;
				patternUnlockData.SetCharacterTypeInt((int)this.m_characterType);
				patternUnlockData.SetSkinIndex(i);
				patternUnlockData.SetID(j);
				List<GameBalanceVars.ColorUnlockData> list3 = new List<GameBalanceVars.ColorUnlockData>();
				for (int k = 0; k < characterPattern.m_colors.Count; k++)
				{
					CharacterColor characterColor = characterPattern.m_colors[k];
					GameBalanceVars.ColorUnlockData colorUnlockData = new GameBalanceVars.ColorUnlockData();
					characterColor.m_colorUnlockData.CopyValuesTo(colorUnlockData);
					colorUnlockData.m_isHidden = characterColor.m_isHidden;
					colorUnlockData.m_sortOrder = characterColor.m_sortOrder;
					colorUnlockData.Name = characterColor.m_name;
					colorUnlockData.SetCharacterTypeInt((int)this.m_characterType);
					colorUnlockData.SetSkinIndex(i);
					colorUnlockData.SetPatternIndex(j);
					colorUnlockData.SetID(k);
					list3.Add(colorUnlockData);
				}
				patternUnlockData.colorUnlockData = list3.ToArray();
				list2.Add(patternUnlockData);
			}
			skinUnlockData.patternUnlockData = list2.ToArray();
			list.Add(skinUnlockData);
		}
		characterUnlockData.skinUnlockData = list.ToArray();
		List<GameBalanceVars.TauntUnlockData> list4 = new List<GameBalanceVars.TauntUnlockData>();
		for (int l = 0; l < this.m_taunts.Count; l++)
		{
			GameBalanceVars.TauntUnlockData tauntUnlockData = this.m_taunts[l].m_tauntUnlockData.Clone();
			tauntUnlockData.Name = this.m_taunts[l].m_tauntName;
			tauntUnlockData.m_isHidden = this.m_taunts[l].m_isHidden;
			tauntUnlockData.SetCharacterTypeInt((int)this.m_characterType);
			tauntUnlockData.SetID(l);
			list4.Add(tauntUnlockData);
		}
		characterUnlockData.tauntUnlockData = list4.ToArray();
		List<GameBalanceVars.AbilityVfxUnlockData> list5 = new List<GameBalanceVars.AbilityVfxUnlockData>();
		this.GenerateVfxSwapUnlockData(this.m_vfxSwapsForAbility0, 0, list5);
		this.GenerateVfxSwapUnlockData(this.m_vfxSwapsForAbility1, 1, list5);
		this.GenerateVfxSwapUnlockData(this.m_vfxSwapsForAbility2, 2, list5);
		this.GenerateVfxSwapUnlockData(this.m_vfxSwapsForAbility3, 3, list5);
		this.GenerateVfxSwapUnlockData(this.m_vfxSwapsForAbility4, 4, list5);
		characterUnlockData.abilityVfxUnlockData = list5.ToArray();
		return characterUnlockData;
	}

	private void GenerateVfxSwapUnlockData(List<CharacterAbilityVfxSwap> input, int abilityIndex, List<GameBalanceVars.AbilityVfxUnlockData> genUnlockDataList)
	{
		if (input != null)
		{
			for (int i = 0; i < input.Count; i++)
			{
				GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = input[i].m_vfxSwapUnlockData.Clone();
				abilityVfxUnlockData.m_isHidden = input[i].m_isHidden;
				abilityVfxUnlockData.SetCharacterTypeInt((int)this.m_characterType);
				abilityVfxUnlockData.SetSwapAbilityId(abilityIndex);
				abilityVfxUnlockData.SetID(input[i].m_uniqueID);
				abilityVfxUnlockData.Name = input[i].m_swapName;
				genUnlockDataList.Add(abilityVfxUnlockData);
			}
		}
		else
		{
			Debug.LogWarning("Vfx Swap Data is null on " + base.gameObject.name);
		}
	}

	internal delegate void CharacterResourceDelegate(LoadedCharacterSelection loadedCharacter);
}
