using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterResourceLink : MonoBehaviour
{
	internal delegate void CharacterResourceDelegate(LoadedCharacterSelection loadedCharacter);

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
			GameObject value = null;
			if (s_loadedActorDataPrefabCache.TryGetValue(m_actorDataResourcePath, out value))
			{
				if (!(value == null))
				{
					goto IL_006a;
				}
			}
			value = Resources.Load<GameObject>(m_actorDataResourcePath);
			if (value != null)
			{
				s_loadedActorDataPrefabCache[m_actorDataResourcePath] = value;
			}
			goto IL_006a;
			IL_006a:
			return value;
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
		return StringUtil.TR_CharacterPatternColorName(m_characterType.ToString(), skinIndex + 1, patternIndex + 1, colorIndex + 1);
	}

	public string GetPatternColorDescription(int skinIndex, int patternIndex, int colorIndex)
	{
		return StringUtil.TR_CharacterPatternColorDescription(m_characterType.ToString(), skinIndex + 1, patternIndex + 1, colorIndex + 1);
	}

	public string GetPatternColorFlavor(int skinIndex, int patternIndex, int colorIndex)
	{
		return StringUtil.TR_CharacterPatternColorFlavor(m_characterType.ToString(), skinIndex + 1, patternIndex + 1, colorIndex + 1);
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
		using (Dictionary<string, GameObject>.Enumerator enumerator = s_instantiatedInGameAudioResources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UnityEngine.Object.Destroy(enumerator.Current.Value);
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_000d;
				}
			}
			end_IL_000d:;
		}
		s_instantiatedInGameAudioResources.Clear();
		using (Dictionary<string, GameObject>.Enumerator enumerator2 = s_instantiatedFrontEndAudioResources.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				UnityEngine.Object.Destroy(enumerator2.Current.Value);
			}
		}
		s_instantiatedFrontEndAudioResources.Clear();
	}

	internal void LoadAsync(CharacterVisualInfo selection, CharacterResourceDelegate onCharacterPrefabLoaded)
	{
		LoadAsync(selection, out int _, onCharacterPrefabLoaded);
	}

	internal void LoadAsync(CharacterVisualInfo selection, CharacterResourceDelegate onCharacterPrefabLoaded, GameStatus gameStatusForAssets)
	{
		LoadAsync(selection, out int _, onCharacterPrefabLoaded, gameStatusForAssets);
	}

	internal void LoadAsync(CharacterVisualInfo selection, out int asyncTicket, CharacterResourceDelegate onCharacterPrefabLoaded)
	{
		LoadAsync(selection, out asyncTicket, onCharacterPrefabLoaded, GameManager.Get().GameStatus);
	}

	internal void LoadAsync(CharacterVisualInfo selection, out int asyncTicket, float delay, CharacterResourceDelegate onCharacterPrefabLoaded)
	{
		LoadAsync(selection, out asyncTicket, onCharacterPrefabLoaded, GameManager.Get().GameStatus, delay);
	}

	private void LoadAsync(CharacterVisualInfo selection, out int asyncTicket, CharacterResourceDelegate onCharacterPrefabLoaded, GameStatus gameStatusForAssets, float delay = 0f)
	{
		if (onCharacterPrefabLoaded == null)
		{
			throw new ArgumentNullException("onCharacterPrefabLoaded");
		}
		AsyncManager.Get().StartAsyncOperation(out asyncTicket, CharacterLoadCoroutine(selection, onCharacterPrefabLoaded, gameStatusForAssets), delay);
	}

	private IEnumerator CharacterLoadCoroutine(CharacterVisualInfo selection, CharacterResourceDelegate onCharacterPrefabLoaded, GameStatus gameStatusForAssets)
	{
		if (!IsVisualInfoSelectionValid(selection))
		{
			Log.Warning(Log.Category.Loading, "Invalid skin selection used to load CharacterType " + m_characterType.ToString() + ", reverting to default. Input = " + selection.ToString());
			selection.ResetToDefault();
		}
		if (m_loadedCharacterCache.TryGetValue(selection, out LoadedCharacterSelection loadedCharacter2))
		{
			if (loadedCharacter2.isLoading)
			{
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
		if (loadedCharacter2 != null)
		{
			while (true)
			{
				Log.Info(Log.Category.Loading, "Character " + base.name + " " + selection.ToString() + " already loading has finished - falling through");
				CharacterSkin skin2 = null;
				if (loadedCharacter2 != null)
				{
					int skinIndex = loadedCharacter2.selectedSkin.skinIndex;
					if (skinIndex >= 0 && skinIndex < m_skins.Count)
					{
						skin2 = m_skins[skinIndex];
					}
					else
					{
						Log.Error("Selected skin index is out of bounds, using default. Input value = " + skinIndex);
						if (m_skins.Count > 0)
						{
							skin2 = m_skins[0];
						}
					}
				}
				LoadPKFXForGameStatus(gameStatusForAssets, skin2);
				IEnumerator e3 = CharacterLoadAudioAssetsForGameStatus(gameStatusForAssets, skin2);
				do
				{
					yield return e3.Current;
				}
				while (e3.MoveNext());
				ClientScene.RegisterPrefab(loadedCharacter2.ActorDataPrefab);
				ActorData actorDataComp = loadedCharacter2.ActorDataPrefab.GetComponent<ActorData>();
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
				onCharacterPrefabLoaded(loadedCharacter2);
				yield return loadedCharacter2;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
		Log.Info(Log.Category.Loading, "Starting async load for Character " + base.name + " " + selection.ToString());
		loadedCharacter2 = new LoadedCharacterSelection
		{
			isLoading = true
		};
		m_loadedCharacterCache.Add(selection, loadedCharacter2);
		loadedCharacter2.ActorDataPrefab = ActorDataPrefab;
		loadedCharacter2.resourceLink = this;
		loadedCharacter2.selectedSkin = selection;
		CharacterSkin skin = null;
		loadedCharacter2.heroPrefabLink = GetHeroPrefabLinkFromSelection(selection, out skin);
		if (loadedCharacter2.heroPrefabLink != null)
		{
			if (!loadedCharacter2.heroPrefabLink.IsEmpty)
			{
				goto IL_040e;
			}
		}
		Log.Error($"Character {m_displayName} could not find Actor Skin resource link for {selection.ToString()}.  Loading default instead...");
		selection.ResetToDefault();
		loadedCharacter2.heroPrefabLink = GetHeroPrefabLinkFromSelection(selection, out skin);
		goto IL_040e;
		IL_040e:
		IEnumerator e2;
		if (!NetworkClient.active)
		{
			if (HydrogenConfig.Get().SkipCharacterModelSpawnOnServer)
			{
				e2 = CharacterLoadAudioAssetsForGameStatus(gameStatusForAssets, skin);
				yield return e2.Current;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
		Log.Info(Log.Category.Loading, "Starting async load for actor model prefab for Character " + base.name + " " + selection.ToString());
		e2 = loadedCharacter2.heroPrefabLink.PreLoadPrefabAsync();
		yield return e2.Current;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	internal void CancelLoad(CharacterVisualInfo selection, int asyncTicket)
	{
		if (!m_loadedCharacterCache.TryGetValue(selection, out LoadedCharacterSelection value))
		{
			return;
		}
		while (true)
		{
			if (value == null || !value.isLoading)
			{
				return;
			}
			while (true)
			{
				m_loadedCharacterCache.Remove(selection);
				if (AsyncManager.Get() != null)
				{
					while (true)
					{
						AsyncManager.Get().CancelAsyncOperation(asyncTicket);
						return;
					}
				}
				return;
			}
		}
	}

	internal void UnloadSkinsNotInList(List<CharacterVisualInfo> skins)
	{
		List<CharacterVisualInfo> list = new List<CharacterVisualInfo>();
		using (Dictionary<CharacterVisualInfo, LoadedCharacterSelection>.Enumerator enumerator = m_loadedCharacterCache.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LoadedCharacterSelection value = enumerator.Current.Value;
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
			m_loadedCharacterCache.Remove(list[i]);
		}
	}

	internal static void UnloadAll()
	{
		if (GameWideData.Get() != null)
		{
			CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
			foreach (CharacterResourceLink characterResourceLink in characterResourceLinks)
			{
				using (Dictionary<CharacterVisualInfo, LoadedCharacterSelection>.Enumerator enumerator = characterResourceLink.m_loadedCharacterCache.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LoadedCharacterSelection value = enumerator.Current.Value;
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
		using (Dictionary<string, GameObject>.Enumerator enumerator2 = s_loadedActorDataPrefabCache.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GameObject value2 = enumerator2.Current.Value;
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
		s_loadedActorDataPrefabCache.Clear();
		s_links.Clear();
		DestroyAudioResources();
	}

	public PrefabResourceLink GetHeroPrefabLinkFromSelection(CharacterVisualInfo selection, out CharacterSkin skin)
	{
		if (selection.skinIndex >= 0)
		{
			if (selection.patternIndex >= 0)
			{
				if (selection.colorIndex >= 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (selection.skinIndex < m_skins.Count)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										skin = m_skins[selection.skinIndex];
										if (selection.patternIndex < skin.m_patterns.Count)
										{
											CharacterPattern characterPattern = skin.m_patterns[selection.patternIndex];
											if (selection.colorIndex < characterPattern.m_colors.Count)
											{
												return characterPattern.m_colors[selection.colorIndex].m_heroPrefab;
											}
											return null;
										}
										return null;
									}
								}
							}
							skin = null;
							return null;
						}
					}
				}
			}
		}
		skin = null;
		return null;
	}

	private void LoadPKFXForGameStatus(GameStatus gamestatus, CharacterSkin skin)
	{
		if (!m_willEverHavePkfx || skin == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(skin.m_pkfxDirectory))
		{
			if (string.IsNullOrEmpty(m_pkfxDirectoryDefault))
			{
				Log.Error("Character {1} (skin: {0}) needs pkfx path set to preload VFX. Until then, you may see a hitch when spawning vfx for this character the first time.", skin.m_name, base.name);
				return;
			}
		}
		if (gamestatus < GameStatus.Launched)
		{
			return;
		}
		while (true)
		{
			ClientVFXLoader clientVFXLoader = ClientVFXLoader.Get();
			string path;
			if (string.IsNullOrEmpty(skin.m_pkfxDirectory))
			{
				path = m_pkfxDirectoryDefault;
			}
			else
			{
				path = skin.m_pkfxDirectory;
			}
			clientVFXLoader.QueuePKFXDirectoryForPreload(Path.Combine("PackFx/Character/Hero", path));
			return;
		}
	}

	private IEnumerator CharacterLoadAudioAssetsForGameStatus(GameStatus gamestatus, CharacterSkin skin)
	{
		PrefabResourceLink[] audioAssetsLinks = null;
		Dictionary<string, GameObject> instantiatedAudioResources2 = null;
		if (gamestatus >= GameStatus.Launched)
		{
			if (gamestatus.IsActiveStatus())
			{
				instantiatedAudioResources2 = s_instantiatedInGameAudioResources;
				if (skin != null)
				{
					if (skin.m_audioAssetsInGamePrefabs != null)
					{
						if (!skin.m_audioAssetsInGamePrefabs.IsNullOrEmpty())
						{
							audioAssetsLinks = skin.m_audioAssetsInGamePrefabs;
							goto IL_0201;
						}
					}
				}
				if (m_audioAssetsInGameDefaultPrefabs != null)
				{
					if (!m_audioAssetsInGameDefaultPrefabs.IsNullOrEmpty())
					{
						audioAssetsLinks = m_audioAssetsInGameDefaultPrefabs;
						goto IL_0201;
					}
				}
				if (Application.isEditor)
				{
					Log.Warning("Yannis/audio team, please set up prefabs: CharacterResourceLink {0} has no audio assets in game default, and no override for a skin.", base.name);
				}
				goto IL_0201;
			}
		}
		instantiatedAudioResources2 = s_instantiatedFrontEndAudioResources;
		if (skin != null)
		{
			if (skin.m_audioAssetsFrontEndPrefabs != null)
			{
				if (!skin.m_audioAssetsFrontEndPrefabs.IsNullOrEmpty())
				{
					audioAssetsLinks = skin.m_audioAssetsFrontEndPrefabs;
					goto IL_0201;
				}
			}
		}
		if (m_audioAssetsFrontEndDefaultPrefabs != null)
		{
			if (!m_audioAssetsFrontEndDefaultPrefabs.IsNullOrEmpty())
			{
				audioAssetsLinks = m_audioAssetsFrontEndDefaultPrefabs;
				goto IL_0201;
			}
		}
		if (Application.isEditor)
		{
			Log.Warning("Yannis/audio team, please set up prefabs: CharacterResourceLink {0} has no audio assets front end default, and no override for a skin.", base.name);
		}
		goto IL_0201;
		IL_0201:
		int num;
		if (HydrogenConfig.Get() != null)
		{
			num = (HydrogenConfig.Get().SkipAudioEvents ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num != 0)
		{
			audioAssetsLinks = null;
		}
		if (audioAssetsLinks == null)
		{
			yield break;
		}
		PrefabResourceLink[] array = audioAssetsLinks;
		int num2 = 0;
		PrefabResourceLink audioAssetsLink;
		while (true)
		{
			if (num2 < array.Length)
			{
				audioAssetsLink = array[num2];
				if (!instantiatedAudioResources2.ContainsKey(audioAssetsLink.GUID))
				{
					break;
				}
				num2++;
				continue;
			}
			yield break;
		}
		while (true)
		{
			instantiatedAudioResources2[audioAssetsLink.GUID] = null;
			IEnumerator e = audioAssetsLink.PreLoadPrefabAsync();
			yield return e.Current;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}

	public void AdvanceSelector(ref CharacterVisualInfo skinSelector)
	{
		skinSelector.colorIndex++;
		if (skinSelector.colorIndex < m_skins[skinSelector.skinIndex].m_patterns[skinSelector.patternIndex].m_colors.Count)
		{
			return;
		}
		skinSelector.colorIndex = 0;
		skinSelector.patternIndex++;
		if (skinSelector.patternIndex < m_skins[skinSelector.skinIndex].m_patterns.Count)
		{
			return;
		}
		while (true)
		{
			skinSelector.patternIndex = 0;
			skinSelector.skinIndex++;
			if (skinSelector.skinIndex >= m_skins.Count)
			{
				while (true)
				{
					skinSelector.skinIndex = 0;
					return;
				}
			}
			return;
		}
	}

	public Sprite GetCharacterRoleIcon()
	{
		return GetCharacterRoleSprite(m_characterRole);
	}

	public static Sprite GetCharacterRoleSprite(CharacterRole role)
	{
		string empty = string.Empty;
		switch (role)
		{
		case CharacterRole.Assassin:
			empty = "iconAssassin";
			break;
		case CharacterRole.Support:
			empty = "iconSupport";
			break;
		case CharacterRole.Tank:
			empty = "iconTank";
			break;
		default:
			return null;
		}
		return Resources.Load<Sprite>(empty);
	}

	public Sprite GetCharacterIcon()
	{
		return (Sprite)Resources.Load(m_characterIconResourceString, typeof(Sprite));
	}

	public Sprite GetCharacterSelectIcon()
	{
		return (Sprite)Resources.Load(m_characterSelectIconResourceString, typeof(Sprite));
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
		if (selection.skinIndex >= 0)
		{
			if (selection.skinIndex < m_skins.Count)
			{
				if (selection.patternIndex >= 0)
				{
					if (selection.patternIndex < m_skins[selection.skinIndex].m_patterns.Count)
					{
						if (selection.colorIndex >= 0)
						{
							if (selection.colorIndex < m_skins[selection.skinIndex].m_patterns[selection.patternIndex].m_colors.Count)
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
		if (!IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility0, m_vfxSwapsForAbility0))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility1, m_vfxSwapsForAbility1))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility2, m_vfxSwapsForAbility2))
		{
			return false;
		}
		if (!IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility3, m_vfxSwapsForAbility3))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!IsSelectedVfxSwapForAbilityValid(abilityVfxSwaps.VfxSwapForAbility4, m_vfxSwapsForAbility4))
		{
			return false;
		}
		return true;
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
			int num = 0;
			while (true)
			{
				if (num < resourceLinkVfxSwaps.Count)
				{
					if (resourceLinkVfxSwaps[num].m_uniqueID == selectedVfxSwap)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				break;
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
			Debug.LogError("Trying to find VFX swaps for an ability with swap ID = " + selectedVfxSwapId + " on resource link " + resourceLinkName + ", but the resource link VFX swaps list is empty.");
			result = null;
		}
		else
		{
			result = null;
			int num = 0;
			while (true)
			{
				if (num < resourceLinkVfxSwaps.Count)
				{
					if (resourceLinkVfxSwaps[num].m_uniqueID == selectedVfxSwapId)
					{
						result = resourceLinkVfxSwaps[num];
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
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

	public GameObject ReplaceSequence(GameObject originalSequencePrefab, CharacterVisualInfo visualInfo, CharacterAbilityVfxSwapInfo abilityVfxSwapsInfo)
	{
		if (originalSequencePrefab == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if (!IsVisualInfoSelectionValid(visualInfo))
		{
			Debug.LogError($"Invalid visual info ({visualInfo.ToString()}) for character resource link {ToString()}, resetting to default...");
			visualInfo.ResetToDefault();
		}
		if (!IsAbilityVfxSwapSelectionValid(abilityVfxSwapsInfo))
		{
			Debug.LogError($"Invalid ability vfx swap info ({abilityVfxSwapsInfo.ToString()}) for character resource link {ToString()}, resetting to default...");
			abilityVfxSwapsInfo.Reset();
		}
		GameObject gameObject = ReplaceSequenceViaCharacterAbilityVfxSwapInfo(originalSequencePrefab, abilityVfxSwapsInfo);
		if (gameObject != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return gameObject;
				}
			}
		}
		CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
		CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
		CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
		gameObject = ReplaceSequence(originalSequencePrefab, characterColor.m_replacementSequences);
		if (gameObject != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return gameObject;
				}
			}
		}
		gameObject = ReplaceSequence(originalSequencePrefab, characterPattern.m_replacementSequences);
		if (gameObject != null)
		{
			return gameObject;
		}
		gameObject = ReplaceSequence(originalSequencePrefab, characterSkin.m_replacementSequences);
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
			CharacterAbilityVfxSwap characterAbilityVfxSwap = FindVfxSwapForAbility(swapInfo.VfxSwapForAbility0, m_vfxSwapsForAbility0, base.name);
			if (characterAbilityVfxSwap != null)
			{
				GameObject gameObject = ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap.m_replacementSequences);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
		}
		if (swapInfo.VfxSwapForAbility1 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap2 = FindVfxSwapForAbility(swapInfo.VfxSwapForAbility1, m_vfxSwapsForAbility1, base.name);
			if (characterAbilityVfxSwap2 != null)
			{
				GameObject gameObject2 = ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap2.m_replacementSequences);
				if (gameObject2 != null)
				{
					return gameObject2;
				}
			}
		}
		if (swapInfo.VfxSwapForAbility2 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap3 = FindVfxSwapForAbility(swapInfo.VfxSwapForAbility2, m_vfxSwapsForAbility2, base.name);
			if (characterAbilityVfxSwap3 != null)
			{
				GameObject gameObject3 = ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap3.m_replacementSequences);
				if (gameObject3 != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return gameObject3;
						}
					}
				}
			}
		}
		if (swapInfo.VfxSwapForAbility3 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap4 = FindVfxSwapForAbility(swapInfo.VfxSwapForAbility3, m_vfxSwapsForAbility3, base.name);
			if (characterAbilityVfxSwap4 != null)
			{
				GameObject gameObject4 = ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap4.m_replacementSequences);
				if (gameObject4 != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return gameObject4;
						}
					}
				}
			}
		}
		if (swapInfo.VfxSwapForAbility4 != 0)
		{
			CharacterAbilityVfxSwap characterAbilityVfxSwap5 = FindVfxSwapForAbility(swapInfo.VfxSwapForAbility4, m_vfxSwapsForAbility4, base.name);
			if (characterAbilityVfxSwap5 != null)
			{
				GameObject gameObject5 = ReplaceSequence(originalSequencePrefab, characterAbilityVfxSwap5.m_replacementSequences);
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
				if (!(prefabReplacement.OriginalPrefab.GetPrefab(true) == originalSequencePrefab))
				{
					continue;
				}
				while (true)
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		if (visualInfo.skinIndex >= 0)
		{
			if (visualInfo.skinIndex < m_skins.Count)
			{
				CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
				AudioReplacement[] replacementAudio = characterSkin.m_replacementAudio;
				foreach (AudioReplacement audioReplacement in replacementAudio)
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
			if (visualInfo.skinIndex < m_skins.Count)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
						int result;
						if (characterSkin.m_replacementAudio != null)
						{
							result = ((characterSkin.m_replacementAudio.Length > 0) ? 1 : 0);
						}
						else
						{
							result = 0;
						}
						return (byte)result != 0;
					}
					}
				}
			}
		}
		return false;
	}

	public bool AllowAudioTag(string audioTag, CharacterVisualInfo visualInfo)
	{
		CharacterSkin characterSkin = null;
		if (visualInfo.skinIndex >= 0 && visualInfo.skinIndex < m_skins.Count)
		{
			characterSkin = m_skins[visualInfo.skinIndex];
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if (!IsVisualInfoSelectionValid(visualInfo))
		{
			Debug.LogError($"Invalid visual info ({visualInfo.ToString()}) for character resource link {ToString()}, resetting to default...");
			visualInfo.ResetToDefault();
		}
		CharacterSkin characterSkin = m_skins[visualInfo.skinIndex];
		CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
		CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
		PrefabResourceLink prefabResourceLink = ReplacePrefabResourceLink(originalPrefabResourceLink, characterColor.m_replacementSequences);
		if (prefabResourceLink != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return prefabResourceLink;
				}
			}
		}
		prefabResourceLink = ReplacePrefabResourceLink(originalPrefabResourceLink, characterPattern.m_replacementSequences);
		if (prefabResourceLink != null)
		{
			return prefabResourceLink;
		}
		prefabResourceLink = ReplacePrefabResourceLink(originalPrefabResourceLink, characterSkin.m_replacementSequences);
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
				if (!(prefabReplacement.OriginalPrefab.ResourcePath == originalPrefabResourceLink.ResourcePath))
				{
					continue;
				}
				while (true)
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
		characterUnlockData.character = m_characterType;
		m_charUnlockData.CopyValuesTo(characterUnlockData);
		characterUnlockData.Name = m_displayName;
		List<GameBalanceVars.SkinUnlockData> list = new List<GameBalanceVars.SkinUnlockData>();
		for (int i = 0; i < m_skins.Count; i++)
		{
			CharacterSkin characterSkin = m_skins[i];
			GameBalanceVars.SkinUnlockData skinUnlockData = new GameBalanceVars.SkinUnlockData();
			characterSkin.m_skinUnlockData.CopyValuesTo(skinUnlockData);
			skinUnlockData.m_isHidden = characterSkin.m_isHidden;
			skinUnlockData.Name = characterSkin.m_name;
			skinUnlockData.SetCharacterTypeInt((int)m_characterType);
			skinUnlockData.SetID(i);
			List<GameBalanceVars.PatternUnlockData> list2 = new List<GameBalanceVars.PatternUnlockData>();
			int num = 0;
			while (num < characterSkin.m_patterns.Count)
			{
				CharacterPattern characterPattern = characterSkin.m_patterns[num];
				GameBalanceVars.PatternUnlockData patternUnlockData = new GameBalanceVars.PatternUnlockData();
				characterPattern.m_patternUnlockData.CopyValuesTo(patternUnlockData);
				patternUnlockData.m_isHidden = characterPattern.m_isHidden;
				patternUnlockData.Name = characterPattern.m_name;
				patternUnlockData.SetCharacterTypeInt((int)m_characterType);
				patternUnlockData.SetSkinIndex(i);
				patternUnlockData.SetID(num);
				List<GameBalanceVars.ColorUnlockData> list3 = new List<GameBalanceVars.ColorUnlockData>();
				for (int j = 0; j < characterPattern.m_colors.Count; j++)
				{
					CharacterColor characterColor = characterPattern.m_colors[j];
					GameBalanceVars.ColorUnlockData colorUnlockData = new GameBalanceVars.ColorUnlockData();
					characterColor.m_colorUnlockData.CopyValuesTo(colorUnlockData);
					colorUnlockData.m_isHidden = characterColor.m_isHidden;
					colorUnlockData.m_sortOrder = characterColor.m_sortOrder;
					colorUnlockData.Name = characterColor.m_name;
					colorUnlockData.SetCharacterTypeInt((int)m_characterType);
					colorUnlockData.SetSkinIndex(i);
					colorUnlockData.SetPatternIndex(num);
					colorUnlockData.SetID(j);
					list3.Add(colorUnlockData);
				}
				while (true)
				{
					patternUnlockData.colorUnlockData = list3.ToArray();
					list2.Add(patternUnlockData);
					num++;
					goto IL_01d3;
				}
				IL_01d3:;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					goto end_IL_01e7;
				}
				continue;
				end_IL_01e7:
				break;
			}
			skinUnlockData.patternUnlockData = list2.ToArray();
			list.Add(skinUnlockData);
		}
		while (true)
		{
			characterUnlockData.skinUnlockData = list.ToArray();
			List<GameBalanceVars.TauntUnlockData> list4 = new List<GameBalanceVars.TauntUnlockData>();
			for (int k = 0; k < m_taunts.Count; k++)
			{
				GameBalanceVars.TauntUnlockData tauntUnlockData = m_taunts[k].m_tauntUnlockData.Clone();
				tauntUnlockData.Name = m_taunts[k].m_tauntName;
				tauntUnlockData.m_isHidden = m_taunts[k].m_isHidden;
				tauntUnlockData.SetCharacterTypeInt((int)m_characterType);
				tauntUnlockData.SetID(k);
				list4.Add(tauntUnlockData);
			}
			characterUnlockData.tauntUnlockData = list4.ToArray();
			List<GameBalanceVars.AbilityVfxUnlockData> list5 = new List<GameBalanceVars.AbilityVfxUnlockData>();
			GenerateVfxSwapUnlockData(m_vfxSwapsForAbility0, 0, list5);
			GenerateVfxSwapUnlockData(m_vfxSwapsForAbility1, 1, list5);
			GenerateVfxSwapUnlockData(m_vfxSwapsForAbility2, 2, list5);
			GenerateVfxSwapUnlockData(m_vfxSwapsForAbility3, 3, list5);
			GenerateVfxSwapUnlockData(m_vfxSwapsForAbility4, 4, list5);
			characterUnlockData.abilityVfxUnlockData = list5.ToArray();
			return characterUnlockData;
		}
	}

	private void GenerateVfxSwapUnlockData(List<CharacterAbilityVfxSwap> input, int abilityIndex, List<GameBalanceVars.AbilityVfxUnlockData> genUnlockDataList)
	{
		if (input != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					for (int i = 0; i < input.Count; i++)
					{
						GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = input[i].m_vfxSwapUnlockData.Clone();
						abilityVfxUnlockData.m_isHidden = input[i].m_isHidden;
						abilityVfxUnlockData.SetCharacterTypeInt((int)m_characterType);
						abilityVfxUnlockData.SetSwapAbilityId(abilityIndex);
						abilityVfxUnlockData.SetID(input[i].m_uniqueID);
						abilityVfxUnlockData.Name = input[i].m_swapName;
						genUnlockDataList.Add(abilityVfxUnlockData);
					}
					return;
				}
				}
			}
		}
		Debug.LogWarning("Vfx Swap Data is null on " + base.gameObject.name);
	}
}
