using System;
using UnityEngine;

public class HUD_UIResources : MonoBehaviour
{
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
        public TutorialVideo[] TutorialVideos;
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

    [Header("-- Practice Mode Video List --")]
    public TutorialVideoInfo[] m_practiceModeVideoList;
    [Header("-- Prologue Video List --")]
    public TutorialVideoInfo[] m_prologueVideoList;
    [Space(5f)]
    public StatusTypeIcon[] m_statusIconList;
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
    public int m_highestScaleDisplayNumber = 60;
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
    public int m_maxLineWidth = 20;
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
    public SpectatorOptionToDisplayName[] m_spectatorToggleOptionNames;
    [Header("    Spectator toggle options to display, in order --")]
    public UISpectatorHUD.SpectatorToggleOption[] m_spectatorOptionsToShow;

    private static float avgR;
    private static float avgG;
    private static float avgB;
    private static float blurPixelCount;

    private static HUD_UIResources s_instance;

    public void Awake()
    {
        s_instance = this;
        if (m_confirmedTargetingFadeoutSpeed < 0f)
        {
            m_confirmedTargetingFadeoutSpeed = 0f;
        }
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public void Update()
    {
        CanvasLayerManager.Get().Update();
    }

    public int GetPracticeModeCurrentLanguageIndex(int videoIndex, string languageCode)
    {
        for (int i = 0; i < m_practiceModeVideoList[videoIndex].TutorialVideos.Length; i++)
        {
            if (m_practiceModeVideoList[videoIndex].TutorialVideos[i].Name == languageCode)
            {
                return i;
            }
        }

        return 0;
    }

    public string GetPracticeModeVideoDisplayName(int videoIndex)
    {
        return StringUtil.TR(m_practiceModeVideoList[videoIndex].DisplayName);
    }

    internal string GetLocalizedVideoPath(TutorialVideoInfo videoInfo, string languageCode)
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
            texture2D = BlurImage(texture2D, radius, true, alpha);
            texture2D = BlurImage(texture2D, radius, false, alpha);
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
                    ResetPixel();
                    for (int k = j; k < j + blurSize; k++)
                    {
                        if (k >= width)
                        {
                            break;
                        }

                        AddPixel(image.GetPixel(k, i));
                    }

                    for (int k = j; k > j - blurSize; k--)
                    {
                        if (k <= 0)
                        {
                            break;
                        }

                        AddPixel(image.GetPixel(k, i));
                    }

                    CalcPixel();
                    for (int k = j; k < j + blurSize; k++)
                    {
                        if (k >= width)
                        {
                            break;
                        }

                        texture2D.SetPixel(k, i, new Color(avgR, avgG, avgB, alpha));
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    ResetPixel();
                    for (int k = i; k < i + blurSize; k++)
                    {
                        if (k >= height)
                        {
                            break;
                        }

                        AddPixel(image.GetPixel(j, k));
                    }

                    for (int k = i; k > i - blurSize; k--)
                    {
                        if (k <= 0)
                        {
                            break;
                        }

                        AddPixel(image.GetPixel(j, k));
                    }

                    CalcPixel();
                    for (int k = i; k < i + blurSize; k++)
                    {
                        if (k >= height)
                        {
                            break;
                        }

                        texture2D.SetPixel(j, k, new Color(avgR, avgG, avgB, alpha));
                    }
                }
            }
        }

        texture2D.Apply();
        return texture2D;
    }

    private static void AddPixel(Color pixel)
    {
        avgR += pixel.r;
        avgG += pixel.g;
        avgB += pixel.b;
        blurPixelCount += 1f;
    }

    private static void ResetPixel()
    {
        avgR = 0f;
        avgG = 0f;
        avgB = 0f;
        blurPixelCount = 0f;
    }

    private static void CalcPixel()
    {
        avgR /= blurPixelCount;
        avgG /= blurPixelCount;
        avgB /= blurPixelCount;
    }

    public static string ColorToHex(Color32 color)
    {
        return "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
    }

    public static HUD_UIResources Get()
    {
        return s_instance;
    }

    public static void SetParentAndAlign(GameObject child, GameObject parent)
    {
        if (parent == null)
        {
            return;
        }

        child.transform.SetParent(parent.transform, false);
        RectTransform rectTransform = child.transform as RectTransform;
        if (rectTransform != null)
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

    public static StatusTypeIcon GetIconForStatusType(StatusType statusType)
    {
        StatusTypeIcon result = new StatusTypeIcon
        {
            icon = null,
            type = StatusType.INVALID,
            isDebuff = false,
            displayIcon = false,
            displayInStatusList = false,
            popupText = string.Empty,
            buffDescription = string.Empty,
            buffName = string.Empty
        };
        for (int i = 0; i < Get().m_statusIconList.Length; i++)
        {
            StatusTypeIcon statusTypeIcon = Get().m_statusIconList[i];
            if (!statusTypeIcon.displayIcon
                || statusTypeIcon.icon == null
                || statusTypeIcon.type != statusType)
            {
                continue;
            }

            result = statusTypeIcon;
            if (statusTypeIcon.popupText != string.Empty)
            {
                result.popupText = StringUtil.GetStatusIconPopupText(i + 1);
            }

            result.buffDescription = statusTypeIcon.buffDescription == string.Empty
                ? statusType + "#NotLocalized"
                : StringUtil.GetStatusIconBuffDesc(i + 1);

            result.buffName = statusTypeIcon.buffName == string.Empty
                ? statusType + "#NotLocalized"
                : StringUtil.GetStatusIconBuffName(i + 1);

            break;
        }

        return result;
    }

    public static float GetScaledCombatTextSize(float number)
    {
        if (number >= Get().m_highestScaleDisplayNumber)
        {
            return Get().m_maxScaleSizeOfCombatText;
        }

        if (number <= Get().m_lowestScaleDisplayNumber)
        {
            return Get().m_minScaleSizeOfCombatText;
        }

        float alpha = (number - Get().m_lowestScaleDisplayNumber)
                      / (Get().m_highestScaleDisplayNumber - (float)Get().m_lowestScaleDisplayNumber);
        return alpha * (Get().m_maxScaleSizeOfCombatText - Get().m_minScaleSizeOfCombatText)
               + Get().m_minScaleSizeOfCombatText;
    }

    public Sprite GetCombatTextIconSprite(BuffIconToDisplay iconType)
    {
        switch (iconType)
        {
            case BuffIconToDisplay.BoostedDamage:
                return m_boostedDamageIconSprite;
            case BuffIconToDisplay.ReducedDamage:
                return m_reducedDamageIconSprite;
            default:
                return null;
        }
    }
}