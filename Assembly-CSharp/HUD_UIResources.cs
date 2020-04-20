using System;
using UnityEngine;

public class HUD_UIResources : MonoBehaviour
{
	[Header("-- Practice Mode Video List --")]
	public HUD_UIResources.TutorialVideoInfo[] m_practiceModeVideoList;

	[Header("-- Prologue Video List --")]
	public HUD_UIResources.TutorialVideoInfo[] m_prologueVideoList;

	[Space(5f)]
	public HUD_UIResources.StatusTypeIcon[] m_statusIconList;

	public Color m_disabledAbilityButtonIconColor;

	[Header("-- For Combat Text Icon --")]
	public Sprite m_boostedDamageIconSprite;

	public Sprite m_reducedDamageIconSprite;

	[Header("Taunt Banner")]
	public Color m_selfIndicatorBar = Color.white;

	public Color m_selfIndicatorBarGlow = Color.white;

	public Color m_allyIndicatorBar = Color.white;

	public Color m_allyIndicatorBarGlow = Color.white;

	public Color m_enemyIndicatorBar = Color.white;

	public Color m_enemyIndicatorBarGlow = Color.white;

	[Header("ChatBox Settings")]
	public float m_textPaddingAmount = 5f;

	public Color m_GlobalChatColor = Color.white;

	public Color m_GroupChatColor = Color.white;

	public Color m_GameChatColor = Color.white;

	public Color m_TeamChatColor = Color.white;

	public Color m_whisperChatColor = Color.white;

	public Color m_systemChatColor = Color.white;

	public Color m_systemErrorChatColor = Color.white;

	public Color m_combatLogChatColor = Color.white;

	[Header("Nameplate Settings")]
	public float m_nameplateStatusHorizontalShiftAmt = 20f;

	public float m_nameplateStaticStatusFadeSpeed = 2f;

	public float m_nameplateStatusFadeColorMultiplier = 0.5f;

	public float m_AmtOfHealthPerTick = 20f;

	public float m_minScaleSizeOfCombatText = 0.5f;

	public float m_maxScaleSizeOfCombatText = 2f;

	public int m_lowestScaleDisplayNumber = 5;

	public int m_highestScaleDisplayNumber = 0x3C;

	public Color m_selfColorGlow = Color.green;

	public Color m_teamColorGlow = Color.blue;

	public Color m_enemyColorGlow = Color.red;

	public Color m_nameplateHealthTextShieldColor = Color.magenta;

	public Color m_nameplateHealthTextHotColor = Color.cyan;

	[Header("Targeting Text: Confirmed Targeting")]
	public float m_confirmedTargetingFadeoutStartDelay = 0.5f;

	public float m_confirmedTargetingFadeoutSpeed = 0.25f;

	public float m_confirmedTargetingDuration = 3f;

	[Tooltip("Highlight fadeout speed, percentage per second")]
	public float m_confirmedTargetingShrinkSpeed = 0.5f;

	[Header("Spectator Options")]
	public Color m_spectatorPenDrawColor = Color.red;

	public int m_minLineWidth = 2;

	public int m_maxLineWidth = 0x14;

	[Header("Minimap")]
	public Sprite m_TileSprite;

	public Sprite m_teammateBorder;

	public Sprite m_enemyBorder;

	public Sprite m_selfBorder;

	public float m_spriteUpdateSpeed = 10f;

	public Color m_selfColorHighlight = Color.green;

	public Color m_allyColorHighlight = Color.blue;

	public Color m_enemyColorHighlight = Color.red;

	public float m_mapPingCooldown = 0.25f;

	public Color m_fogInvalidPlaySquare = Color.black;

	public Color m_fogValidPlaySquareVisible = Color.white;

	public Color m_fogValidPlaySquareNonVisible = Color.gray;

	[Header("-- Spectator HUD Toggle Option Names --")]
	public HUD_UIResources.SpectatorOptionToDisplayName[] m_spectatorToggleOptionNames;

	[Header("    Spectator toggle options to display, in order --")]
	public UISpectatorHUD.SpectatorToggleOption[] m_spectatorOptionsToShow;

	private static float avgR;

	private static float avgG;

	private static float avgB;

	private static float blurPixelCount;

	private static HUD_UIResources s_instance;

	public void Awake()
	{
		HUD_UIResources.s_instance = this;
		if (this.m_confirmedTargetingFadeoutSpeed < 0f)
		{
			this.m_confirmedTargetingFadeoutSpeed = 0f;
		}
	}

	private void OnDestroy()
	{
		HUD_UIResources.s_instance = null;
	}

	public void Update()
	{
		CanvasLayerManager.Get().Update();
	}

	public int GetPracticeModeCurrentLanguageIndex(int videoIndex, string languageCode)
	{
		for (int i = 0; i < this.m_practiceModeVideoList[videoIndex].TutorialVideos.Length; i++)
		{
			if (this.m_practiceModeVideoList[videoIndex].TutorialVideos[i].Name == languageCode)
			{
				return i;
			}
		}
		return 0;
	}

	public string GetPracticeModeVideoDisplayName(int videoIndex)
	{
		return StringUtil.TR(this.m_practiceModeVideoList[videoIndex].DisplayName);
	}

	internal string GetLocalizedVideoPath(HUD_UIResources.TutorialVideoInfo videoInfo, string languageCode)
	{
		for (int i = 0; i < videoInfo.TutorialVideos.Length; i++)
		{
			if (videoInfo.TutorialVideos[i].Name == languageCode)
			{
				return videoInfo.TutorialVideos[i].VideoPath;
			}
		}
		return null;
	}

	public static Texture2D FastBlur(Texture2D image, int radius, int iterations, float alpha)
	{
		Texture2D texture2D = image;
		for (int i = 0; i < iterations; i++)
		{
			texture2D = HUD_UIResources.BlurImage(texture2D, radius, true, alpha);
			texture2D = HUD_UIResources.BlurImage(texture2D, radius, false, alpha);
		}
		return texture2D;
	}

	private static Texture2D BlurImage(Texture2D image, int blurSize, bool horizontal, float alpha)
	{
		Texture2D texture2D = new Texture2D(image.width, image.height);
		int width = image.width;
		int height = image.height;
		if (horizontal)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					HUD_UIResources.ResetPixel();
					int k = j;
					while (k < j + blurSize)
					{
						if (k >= width)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								goto IL_91;
							}
						}
						else
						{
							HUD_UIResources.AddPixel(image.GetPixel(k, i));
							k++;
						}
					}
					IL_91:
					k = j;
					while (k > j - blurSize)
					{
						if (k <= 0)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								goto IL_CD;
							}
						}
						else
						{
							HUD_UIResources.AddPixel(image.GetPixel(k, i));
							k--;
						}
					}
					IL_CD:
					HUD_UIResources.CalcPixel();
					k = j;
					while (k < j + blurSize)
					{
						if (k >= width)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								goto IL_11C;
							}
						}
						else
						{
							texture2D.SetPixel(k, i, new Color(HUD_UIResources.avgR, HUD_UIResources.avgG, HUD_UIResources.avgB, alpha));
							k++;
						}
					}
					IL_11C:;
				}
			}
		}
		else
		{
			for (int j = 0; j < width; j++)
			{
				for (int i = 0; i < height; i++)
				{
					HUD_UIResources.ResetPixel();
					int l = i;
					while (l < i + blurSize)
					{
						if (l >= height)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								goto IL_189;
							}
						}
						else
						{
							HUD_UIResources.AddPixel(image.GetPixel(j, l));
							l++;
						}
					}
					IL_189:
					l = i;
					while (l > i - blurSize)
					{
						if (l <= 0)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_1BC;
							}
						}
						else
						{
							HUD_UIResources.AddPixel(image.GetPixel(j, l));
							l--;
						}
					}
					IL_1BC:
					HUD_UIResources.CalcPixel();
					for (l = i; l < i + blurSize; l++)
					{
						if (l >= height)
						{
							break;
						}
						texture2D.SetPixel(j, l, new Color(HUD_UIResources.avgR, HUD_UIResources.avgG, HUD_UIResources.avgB, alpha));
					}
				}
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private static void AddPixel(Color pixel)
	{
		HUD_UIResources.avgR += pixel.r;
		HUD_UIResources.avgG += pixel.g;
		HUD_UIResources.avgB += pixel.b;
		HUD_UIResources.blurPixelCount += 1f;
	}

	private static void ResetPixel()
	{
		HUD_UIResources.avgR = 0f;
		HUD_UIResources.avgG = 0f;
		HUD_UIResources.avgB = 0f;
		HUD_UIResources.blurPixelCount = 0f;
	}

	private static void CalcPixel()
	{
		HUD_UIResources.avgR /= HUD_UIResources.blurPixelCount;
		HUD_UIResources.avgG /= HUD_UIResources.blurPixelCount;
		HUD_UIResources.avgB /= HUD_UIResources.blurPixelCount;
	}

	public static string ColorToHex(Color32 color)
	{
		return "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	public static HUD_UIResources Get()
	{
		return HUD_UIResources.s_instance;
	}

	public static void SetParentAndAlign(GameObject child, GameObject parent)
	{
		if (parent == null)
		{
			return;
		}
		child.transform.SetParent(parent.transform, false);
		RectTransform rectTransform = child.transform as RectTransform;
		if (rectTransform)
		{
			rectTransform.anchoredPosition = Vector2.zero;
			Vector3 localPosition = rectTransform.localPosition;
			localPosition.z = 0f;
			rectTransform.localPosition = localPosition;
		}
		else
		{
			child.transform.localPosition = Vector3.zero;
		}
		child.transform.localRotation = Quaternion.identity;
		child.transform.localScale = Vector3.one;
		child.layer = parent.layer;
	}

	public static HUD_UIResources.StatusTypeIcon GetIconForStatusType(StatusType statusType)
	{
		HUD_UIResources.StatusTypeIcon result;
		result.icon = null;
		result.type = StatusType.INVALID;
		result.isDebuff = false;
		result.displayIcon = false;
		result.displayInStatusList = false;
		result.popupText = string.Empty;
		result.buffDescription = string.Empty;
		result.buffName = string.Empty;
		for (int i = 0; i < HUD_UIResources.Get().m_statusIconList.Length; i++)
		{
			if (HUD_UIResources.Get().m_statusIconList[i].displayIcon)
			{
				if (HUD_UIResources.Get().m_statusIconList[i].icon != null)
				{
					if (HUD_UIResources.Get().m_statusIconList[i].type == statusType)
					{
						result.displayInStatusList = HUD_UIResources.Get().m_statusIconList[i].displayInStatusList;
						result = HUD_UIResources.Get().m_statusIconList[i];
						if (HUD_UIResources.Get().m_statusIconList[i].popupText != string.Empty)
						{
							result.popupText = StringUtil.GetStatusIconPopupText(i + 1);
						}
						if (HUD_UIResources.Get().m_statusIconList[i].buffDescription == string.Empty)
						{
							result.buffDescription = statusType.ToString() + "#NotLocalized";
						}
						else
						{
							result.buffDescription = StringUtil.GetStatusIconBuffDesc(i + 1);
						}
						if (HUD_UIResources.Get().m_statusIconList[i].buffName == string.Empty)
						{
							result.buffName = statusType.ToString() + "#NotLocalized";
						}
						else
						{
							result.buffName = StringUtil.GetStatusIconBuffName(i + 1);
						}
						break;
					}
				}
			}
		}
		return result;
	}

	public static float GetScaledCombatTextSize(float number)
	{
		float result;
		if (number >= (float)HUD_UIResources.Get().m_highestScaleDisplayNumber)
		{
			result = HUD_UIResources.Get().m_maxScaleSizeOfCombatText;
		}
		else if (number <= (float)HUD_UIResources.Get().m_lowestScaleDisplayNumber)
		{
			result = HUD_UIResources.Get().m_minScaleSizeOfCombatText;
		}
		else
		{
			float num = (number - (float)HUD_UIResources.Get().m_lowestScaleDisplayNumber) / ((float)HUD_UIResources.Get().m_highestScaleDisplayNumber - (float)HUD_UIResources.Get().m_lowestScaleDisplayNumber);
			result = num * (HUD_UIResources.Get().m_maxScaleSizeOfCombatText - HUD_UIResources.Get().m_minScaleSizeOfCombatText) + HUD_UIResources.Get().m_minScaleSizeOfCombatText;
		}
		return result;
	}

	public Sprite GetCombatTextIconSprite(BuffIconToDisplay iconType)
	{
		if (iconType == BuffIconToDisplay.BoostedDamage)
		{
			return this.m_boostedDamageIconSprite;
		}
		if (iconType == BuffIconToDisplay.ReducedDamage)
		{
			return this.m_reducedDamageIconSprite;
		}
		return null;
	}

	[Serializable]
	public struct TutorialVideo
	{
		public string Name;

		public string VideoPath;
	}

	[Serializable]
	public struct TutorialVideoInfo
	{
		public string Name;

		public string DisplayName;

		public string ThumbnailResourceLocation;

		public HUD_UIResources.TutorialVideo[] TutorialVideos;

		public AccountComponent.UIStateIdentifier SeenVideo;
	}

	[Serializable]
	public struct StatusTypeIcon
	{
		public string popupText;

		public StatusType type;

		public Sprite icon;

		public bool isDebuff;

		public bool displayIcon;

		public bool displayInStatusList;

		public string buffName;

		public string buffDescription;
	}

	[Serializable]
	public class SpectatorOptionToDisplayName
	{
		public string m_displayName;

		public UISpectatorHUD.SpectatorToggleOption m_toggleOption;
	}
}
