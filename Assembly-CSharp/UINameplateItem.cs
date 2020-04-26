using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINameplateItem : MonoBehaviour, IGameEventListener
{
	[Serializable]
	public struct TeamBars
	{
		public Image m_teamMarker;

		[Range(0f, 1f)]
		public float m_currentHPPercent;

		public Image m_currentHPBar;

		[Range(0f, 1f)]
		public float m_damageEasePercent;

		public Image m_damageEasedBar;
	}

	[Serializable]
	public struct AbilityModifier
	{
		public Animator m_targetingTextAnimationController;

		public TextMeshProUGUI m_abilityModifierText;
	}

	[Serializable]
	public struct NameplateCombatText
	{
		public Animator m_CombatTextController;

		public TextMeshProUGUI m_combatText;

		public Image m_combatTextCover;

		public Image m_iconImage;

		public Color m_colorToChangeCombatText;

		public RectTransform m_TextScalar;
	}

	public enum BarColor
	{
		None,
		Self,
		Team,
		Enemy
	}

	public enum AbilityModifierType
	{
		Damage,
		Healing,
		Absorb,
		Energy,
		MAX
	}

	public struct StatusDisplayInfo
	{
		public StatusType statusType;

		public UINameplateStatus statusObject;
	}

	public struct StaticStatusDisplayInfo
	{
		public StatusType statusType;

		public UIBuffIndicator statusObject;

		public bool m_removedBuff;
	}

	public NameplateCombatText[] nameplateCombatText;

	public bool m_showCombatTextIcons = true;

	[Space(10f)]
	public UIBuffIndicator m_buffIndicatorPrefab;

	public GridLayoutGroup m_buffIndicatorGrid;

	public RectTransform m_statusContainer;

	public UINameplateStatus m_statusPrefab;

	[Space(10f)]
	public GridLayoutGroup m_targetingAbilityIconsGrid;

	public GameObject m_targetingAbilityIndicatorPrefab;

	public UITargetingAbilityCatalystPipContainer m_catalystIndicatorPrefab;

	[Space(10f)]
	public GameObject m_overconParent;

	public GameObject m_overconPrefab;

	[Space(10f)]
	public Button m_HPMouseOverHitBox;

	public CanvasGroup m_mouseOverHitBoxCanvasGroup;

	[Space(10f)]
	public Animator m_redBriefcaseContainer;

	public Animator m_blueBriefcaseContainer;

	public ImageFilledSloped m_redFillMarker;

	public ImageFilledSloped m_blueFillMarker;

	[Space(10f)]
	public RectTransform m_parentTransform;

	public Image m_coverSymbol;

	public Image m_zeroHealthIcon;

	public Image m_targetGlow;

	[Range(0f, 1f)]
	public float m_shieldPercent;

	public Image m_shieldBar;

	private EasedFloat m_ShieldEased;

	private EasedFloat m_HPDamageEased;

	[Range(0f, 1f)]
	public float m_hpGainedPercent;

	public Image m_hpGainBar;

	private EasedFloat m_HPGainedEased;

	public TeamBars m_selfBars;

	private EasedFloat m_HPEased;

	public TeamBars m_teamBars;

	public TeamBars m_enemyBars;

	[Range(0f, 1f)]
	public float m_tpGainedPercent;

	public Image m_tpGainBar;

	private EasedFloat m_TPEasedGained;

	[Range(0f, 1f)]
	public float m_tpGainedEasePercent;

	public Image m_tpGainEaseBar;

	private EasedFloat m_TPEased;

	public BarColor barsToShow;

	public TextMeshProUGUI m_textNameLabel;

	public TextMeshProUGUI m_healthLabel;

	public RectTransform m_maxEnergyContainer;

	public Animator m_maxEnergyAnimator;

	public AbilityModifier[] abilityModifiers;

	public AbilityModifier allyAbilityModifier;

	public Sprite[] m_abilityModifierSprites = new Sprite[4];

	public Image m_healthTickPrefab;

	public RectTransform m_tickContainer;

	[Header("-- For when an ability is happening --")]
	public float m_alphaWhenOthersHighlighted = 0.5f;

	public float m_scaleWhenOthersHighlighted = 0.9f;

	public float m_scaleWhenHighlighted = 1.2f;

	public TextMeshProUGUI m_debugText;

	public Color m_DamageColor = Color.red;

	public Color m_HealingColor = Color.green;

	public Color m_AbsorbColor = Color.magenta;

	public Color m_EnergyColor = Color.yellow;

	public Color m_FullEnergyColor = default(Color);

	public Color m_NonFullEnergyColor = new Color(1f, 1f, 0.42745f);

	public TMP_FontAsset m_buffFont;

	public TMP_FontAsset m_debuffFont;

	public CanvasGroup m_canvasGroup;

	public CanvasGroup m_combatTextCanvasGroup;

	public CanvasGroup m_abilityPreviewCanvasGroup;

	public float m_distanceFromCamera;

	private List<StatusDisplayInfo> m_statusEffectsAnimating;

	private List<StaticStatusDisplayInfo> m_statusEffects;

	private List<UITargetingAbilityIndicator> m_targetingAbilityIndicators;

	private UITargetingAbilityCatalystPipContainer m_catalsystPips;

	public const int c_maxNumTargetingAbilityIndicators = 6;

	private ActorData m_actorData;

	private int m_currentStatusStackCount;

	private int m_previousHP;

	private int m_previousHPMax;

	private int m_previousShieldValue;

	private int m_previousHPShieldAndHot;

	private int m_previousTP;

	private int m_previousTPMax;

	private int m_previousResolvedHP;

	private int m_maxHPWithShield;

	private bool m_visible;

	private bool m_lostHealth;

	private int m_sortOrder;

	private const float BAR_BACKGROUND_ANIM_SECONDS = 2.5f;

	private const float SHIELDBAR_BACKGROUND_ANIM_SECONDS = 1f;

	private Canvas myCanvas;

	private RectTransform CanvasRect;

	private UINameplateOvercon m_overcon;

	private int m_numOverconUsesThisTurn;

	private int m_lastTurnOverconUsed;

	private int m_numOverconUsesThisMatch;

	private int[] m_numOverconUsesById;

	private BarColor lastBarColor;

	private float m_alphaToUse;

	private List<Image> m_ticks;

	private bool m_mouseIsOverHP;

	private bool m_textVisible = true;

	private bool m_hpGainedIsFading = true;

	private const float kHpGainBarBlinkRate = 0.05f;

	private bool m_isMaxEnergy;

	private UINameplateTickShaderHelper[] m_tickShaderHelpers;

	private int m_lastUsedCombatTextIndex = -1;

	private float m_fadeoutStartTime;

	private bool m_isHoldingFlag;

	private Dictionary<AbilityTooltipSymbol, int> m_tempTargetingNumSymbolToValueMap = new Dictionary<AbilityTooltipSymbol, int>();

	private float m_setToDimTime = -1f;

	private void Awake()
	{
		m_sortOrder = -1;
		m_ShieldEased = new EasedFloat(0f);
		m_HPDamageEased = new EasedFloat(0f);
		m_HPGainedEased = new EasedFloat(0f);
		m_HPEased = new EasedFloat(0f);
		m_TPEasedGained = new EasedFloat(0f);
		m_TPEased = new EasedFloat(0f);
		m_statusEffectsAnimating = new List<StatusDisplayInfo>();
		m_statusEffects = new List<StaticStatusDisplayInfo>();
		m_targetingAbilityIndicators = new List<UITargetingAbilityIndicator>();
		m_visible = true;
		m_currentStatusStackCount = 0;
		UIEventTriggerUtils.AddListener(m_HPMouseOverHitBox.gameObject, EventTriggerType.PointerEnter, MouseOverHP);
		UIEventTriggerUtils.AddListener(m_HPMouseOverHitBox.gameObject, EventTriggerType.PointerExit, MouseExitHP);
		UIManager.SetGameObjectActive(m_maxEnergyContainer, false);
		m_tickShaderHelpers = GetComponentsInChildren<UINameplateTickShaderHelper>(true);
		Mask componentInChildren = GetComponentInChildren<Mask>();
		if (!(componentInChildren != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(componentInChildren, false);
			return;
		}
	}

	public void SetSortOrder(int order)
	{
		m_sortOrder = order;
	}

	public int GetSortOrder()
	{
		return m_sortOrder;
	}

	public void MouseOverHP(BaseEventData data)
	{
		m_mouseIsOverHP = true;
	}

	public void MouseExitHP(BaseEventData data)
	{
		m_mouseIsOverHP = false;
	}

	public void SetTextVisible(bool visible)
	{
		m_textVisible = visible;
		UIManager.SetGameObjectActive(m_textNameLabel, visible);
		UIManager.SetGameObjectActive(m_healthLabel, visible);
	}

	private void Start()
	{
		UIManager.SetGameObjectActive(m_coverSymbol, false);
		for (int i = 0; i < abilityModifiers.Length; i++)
		{
			UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
		}
		while (true)
		{
			UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, false);
			SetDebugText(string.Empty);
			for (int j = 0; j < nameplateCombatText.Length; j++)
			{
				nameplateCombatText[j].m_combatTextCover.color = new Color(1f, 1f, 1f, 0f);
			}
			while (true)
			{
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilityHighlightStart);
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.ClientResolutionStarted);
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.NormalMovementStart);
				int num = 0;
				using (List<UIOverconData.NameToOverconEntry>.Enumerator enumerator = UIOverconData.Get().m_nameToOverconEntry.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIOverconData.NameToOverconEntry current = enumerator.Current;
						if (num < current.m_overconId)
						{
							num = current.m_overconId;
						}
					}
				}
				m_numOverconUsesById = new int[num + 1];
				return;
			}
		}
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsAbilityHighlightStart);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ClientResolutionStarted);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.NormalMovementStart);
			return;
		}
	}

	private void SetMaxHPWithShield(int newVal)
	{
		bool flag = m_maxHPWithShield != newVal;
		m_maxHPWithShield = newVal;
		if (!flag)
		{
			return;
		}
		while (true)
		{
			float tickDivisions = (float)m_maxHPWithShield / HUD_UIResources.Get().m_AmtOfHealthPerTick;
			for (int i = 0; i < m_tickShaderHelpers.Length; i++)
			{
				UINameplateTickShaderHelper uINameplateTickShaderHelper = m_tickShaderHelpers[i];
				uINameplateTickShaderHelper.SetTickDivisions(tickDivisions);
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
	}

	private void SetTeamBars(TeamBars bars, bool visible)
	{
		UIManager.SetGameObjectActive(bars.m_teamMarker, visible);
		UIManager.SetGameObjectActive(bars.m_currentHPBar, visible);
		UIManager.SetGameObjectActive(bars.m_damageEasedBar, visible);
		NotifyFlagStatusChange(m_isHoldingFlag);
	}

	public static BarColor GetRelationshipWithPlayer(ActorData theActor)
	{
		if (theActor == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return BarColor.None;
				}
			}
		}
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return BarColor.None;
				}
			}
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (activeOwnedActorData == theActor)
					{
						return BarColor.Self;
					}
					if (activeOwnedActorData.GetTeam() == theActor.GetTeam())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return BarColor.Team;
							}
						}
					}
					return BarColor.Enemy;
				}
			}
		}
		if (theActor.GetTeam() == Team.TeamA)
		{
			return BarColor.Team;
		}
		return BarColor.Enemy;
	}

	public void HandleAnimCallback(Animator animationController, UINameplateAnimCallback.AnimationCallback type)
	{
		if (animationController == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (type != UINameplateAnimCallback.AnimationCallback.COMBAT_TEXT_COLOR)
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
		for (int i = 0; i < nameplateCombatText.Length; i++)
		{
			if (animationController == nameplateCombatText[i].m_CombatTextController)
			{
				SetCombatTextColor(i);
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

	public void SetCombatTextColor(int index)
	{
		if (-1 < index && index < nameplateCombatText.Length)
		{
			nameplateCombatText[index].m_combatText.color = nameplateCombatText[index].m_colorToChangeCombatText;
		}
	}

	public void StartTargetingNumberFadeout()
	{
		m_fadeoutStartTime = Time.time;
		for (int i = 0; i < abilityModifiers.Length; i++)
		{
			Color color = abilityModifiers[i].m_abilityModifierText.color;
			color.a = 1f;
			abilityModifiers[i].m_abilityModifierText.color = color;
			if (abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeSelf)
			{
				abilityModifiers[i].m_targetingTextAnimationController.SetTrigger("DoOff");
				abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", false);
			}
		}
		Color color2 = allyAbilityModifier.m_abilityModifierText.color;
		color2.a = 1f;
		allyAbilityModifier.m_abilityModifierText.color = color2;
		if (!allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			allyAbilityModifier.m_targetingTextAnimationController.SetTrigger("DoOff");
			allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", false);
			return;
		}
	}

	public void ShowTargetingNumberForConfirmedTargeting()
	{
		m_fadeoutStartTime = Time.time;
		for (int i = 0; i < abilityModifiers.Length; i++)
		{
			Color color = abilityModifiers[i].m_abilityModifierText.color;
			color.a = 1f;
			abilityModifiers[i].m_abilityModifierText.color = color;
			if (abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeSelf)
			{
				abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", true);
			}
		}
		while (true)
		{
			Color color2 = allyAbilityModifier.m_abilityModifierText.color;
			color2.a = 1f;
			allyAbilityModifier.m_abilityModifierText.color = color2;
			if (allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeSelf)
			{
				allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
			}
			return;
		}
	}

	public void PlayCombatText(ActorData actorData, string text, CombatTextCategory category, BuffIconToDisplay icon)
	{
		bool flag = false;
		Color c = Color.black;
		BarColor relationshipWithPlayer = GetRelationshipWithPlayer(actorData);
		bool flag2 = false;
		bool flag3 = false;
		int num = 0;
		string text2 = "CombatTextHit";
		string text3 = "CombatTextHitMedium";
		bool flag4 = false;
		int num2 = 0;
		while (true)
		{
			if (num2 < nameplateCombatText.Length)
			{
				AnimatorClipInfo[] currentAnimatorClipInfo = nameplateCombatText[num2].m_CombatTextController.GetCurrentAnimatorClipInfo(0);
				if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip.name != text2 && currentAnimatorClipInfo[0].clip.name != text3)
				{
					num = num2;
					flag4 = true;
					break;
				}
				num2++;
				continue;
			}
			break;
		}
		if (!flag4)
		{
			num = (m_lastUsedCombatTextIndex = (m_lastUsedCombatTextIndex + 1) % nameplateCombatText.Length);
		}
		else
		{
			m_lastUsedCombatTextIndex = num;
		}
		float number = 0f;
		switch (category)
		{
		case CombatTextCategory.Damage:
		{
			string[] array = text.Split('|');
			nameplateCombatText[num].m_combatText.text = array[0];
			number = int.Parse(array[0]);
			if (array[1] == "C")
			{
				flag = true;
			}
			if (relationshipWithPlayer != BarColor.Self && relationshipWithPlayer != BarColor.Team)
			{
				if (relationshipWithPlayer == BarColor.Enemy)
				{
					nameplateCombatText[num].m_colorToChangeCombatText = new Color(1f, 71f / 85f, 71f / 85f);
					c = new Color(194f / 255f, 0f, 0f);
					flag3 = false;
				}
			}
			else
			{
				nameplateCombatText[num].m_colorToChangeCombatText = new Color(1f, 0.011764f, 0.011764f);
				c = new Color(91f / 255f, 0f, 0f);
				flag3 = true;
			}
			flag2 = true;
			break;
		}
		case CombatTextCategory.Healing:
			if (relationshipWithPlayer != BarColor.Self)
			{
				if (relationshipWithPlayer != BarColor.Team)
				{
					if (relationshipWithPlayer == BarColor.Enemy)
					{
						nameplateCombatText[num].m_colorToChangeCombatText = new Color(2f / 51f, 127f / 255f, 16f / 255f);
						c = new Color(0f, 9f / 85f, 0.003921569f);
						flag3 = true;
					}
					goto IL_02d8;
				}
			}
			nameplateCombatText[num].m_colorToChangeCombatText = new Color(0.7843137f, 1f, 203f / 255f);
			c = new Color(0f, 39f / 85f, 0.0117647061f);
			flag3 = false;
			goto IL_02d8;
		case CombatTextCategory.Absorb:
			nameplateCombatText[num].m_combatText.text = text;
			c = Color.blue;
			flag2 = true;
			break;
		case CombatTextCategory.TP_Damage:
			nameplateCombatText[num].m_combatText.text = text;
			c = Color.magenta;
			flag2 = true;
			break;
		case CombatTextCategory.TP_Recovery:
			{
				nameplateCombatText[num].m_combatText.text = text;
				c = Color.yellow * 0.7f;
				flag2 = true;
				break;
			}
			IL_02d8:
			nameplateCombatText[num].m_combatText.text = text;
			number = int.Parse(text);
			flag2 = true;
			break;
		}
		Image combatTextCover = nameplateCombatText[num].m_combatTextCover;
		int doActive;
		if (m_showCombatTextIcons)
		{
			doActive = (flag ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(combatTextCover, (byte)doActive != 0);
		if (nameplateCombatText[num].m_iconImage != null)
		{
			if (HUD_UIResources.Get() != null)
			{
				Sprite combatTextIconSprite = HUD_UIResources.Get().GetCombatTextIconSprite(icon);
				if (combatTextIconSprite != null && m_showCombatTextIcons)
				{
					nameplateCombatText[num].m_iconImage.sprite = combatTextIconSprite;
					UIManager.SetGameObjectActive(nameplateCombatText[num].m_iconImage, true);
				}
				else
				{
					nameplateCombatText[num].m_iconImage.sprite = null;
					UIManager.SetGameObjectActive(nameplateCombatText[num].m_iconImage, false);
				}
			}
		}
		bool flag5 = false;
		if (flag2)
		{
			if (nameplateCombatText[num].m_CombatTextController != null)
			{
				if (flag3)
				{
					nameplateCombatText[num].m_CombatTextController.Play(text3, 0, 0f);
				}
				else
				{
					nameplateCombatText[num].m_CombatTextController.Play(text2, 0, 0f);
				}
				nameplateCombatText[num].m_combatText.color = Color.white;
				nameplateCombatText[num].m_combatText.outlineColor = c;
				flag5 = true;
			}
		}
		if (flag5)
		{
			MoveToTopOfNameplates();
		}
		float scaledCombatTextSize = HUD_UIResources.GetScaledCombatTextSize(number);
		nameplateCombatText[num].m_TextScalar.localScale = new Vector3(scaledCombatTextSize, scaledCombatTextSize, scaledCombatTextSize);
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				while (true)
				{
					HighlightNameplateForAbility();
					m_setToDimTime = Time.time + 1f;
					return;
				}
			}
			return;
		}
	}

	private void HideCombatTexts()
	{
		for (int i = 0; i < nameplateCombatText.Length; i++)
		{
			nameplateCombatText[i].m_combatText.text = string.Empty;
			if (nameplateCombatText[i].m_iconImage != null)
			{
				UIManager.SetGameObjectActive(nameplateCombatText[i].m_iconImage, false);
			}
			if (nameplateCombatText[i].m_combatTextCover != null)
			{
				UIManager.SetGameObjectActive(nameplateCombatText[i].m_combatTextCover, false);
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void StatusFadeOutDone(StatusType newType)
	{
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(newType);
		if (!iconForStatusType.displayIcon)
		{
			return;
		}
		UINameplateStatus uINameplateStatus = UnityEngine.Object.Instantiate(m_statusPrefab);
		uINameplateStatus.transform.SetParent(m_statusContainer.transform);
		uINameplateStatus.transform.localScale = Vector3.one;
		uINameplateStatus.transform.localEulerAngles = Vector3.zero;
		(uINameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2((float)m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt, (float)m_currentStatusStackCount * (uINameplateStatus.transform as RectTransform).rect.height);
		Vector3 localPosition = uINameplateStatus.transform.localPosition;
		localPosition.z = 0f;
		uINameplateStatus.transform.localPosition = localPosition;
		uINameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
		uINameplateStatus.m_StatusText.text = "-" + iconForStatusType.popupText;
		float nameplateStatusFadeColorMultiplier = HUD_UIResources.Get().m_nameplateStatusFadeColorMultiplier;
		uINameplateStatus.m_StatusText.color = uINameplateStatus.m_StatusText.color * nameplateStatusFadeColorMultiplier;
		Color color = uINameplateStatus.m_StatusText.color;
		color.a = 1f;
		uINameplateStatus.m_StatusText.color = color;
		uINameplateStatus.m_StatusIcon.color = uINameplateStatus.m_StatusIcon.color * nameplateStatusFadeColorMultiplier;
		color = uINameplateStatus.m_StatusIcon.color;
		color.a = 1f;
		uINameplateStatus.m_StatusIcon.color = color;
		if (iconForStatusType.isDebuff)
		{
			uINameplateStatus.m_StatusText.font = m_debuffFont;
		}
		else
		{
			uINameplateStatus.m_StatusText.font = m_buffFont;
		}
		uINameplateStatus.DisplayAsLostStatus(this);
		StatusDisplayInfo item = default(StatusDisplayInfo);
		item.statusObject = uINameplateStatus;
		item.statusType = newType;
		m_statusEffectsAnimating.Add(item);
		m_currentStatusStackCount++;
	}

	public void UpdateBriefcaseThreshold(float percent)
	{
		m_blueFillMarker.fillAmount = percent;
		m_redFillMarker.fillAmount = percent;
	}

	public void NotifyFlagStatusChange(bool holdingFlag)
	{
		if (m_isHoldingFlag == holdingFlag)
		{
			return;
		}
		while (true)
		{
			m_isHoldingFlag = holdingFlag;
			if (m_isHoldingFlag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_redBriefcaseContainer, barsToShow == BarColor.Enemy);
						UIManager.SetGameObjectActive(m_blueBriefcaseContainer, barsToShow == BarColor.Self || barsToShow == BarColor.Team);
						return;
					}
				}
			}
			if (m_redBriefcaseContainer.gameObject.activeInHierarchy)
			{
				m_redBriefcaseContainer.Play("BriefcaseUIDefaultOUT");
			}
			if (m_blueBriefcaseContainer.gameObject.activeInHierarchy)
			{
				m_blueBriefcaseContainer.Play("BriefcaseUIDefaultOUT");
			}
			return;
		}
	}

	public void AddStatus(StatusType newType)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(newType);
		if (!iconForStatusType.displayIcon)
		{
			return;
		}
		StatusDisplayInfo item = default(StatusDisplayInfo);
		while (true)
		{
			UINameplateStatus uINameplateStatus = UnityEngine.Object.Instantiate(m_statusPrefab);
			uINameplateStatus.transform.SetParent(m_statusContainer.transform);
			uINameplateStatus.transform.localScale = Vector3.one;
			uINameplateStatus.transform.localEulerAngles = Vector3.zero;
			(uINameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2((float)m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt, (float)m_currentStatusStackCount * (uINameplateStatus.transform as RectTransform).rect.height);
			Vector3 localPosition = uINameplateStatus.transform.localPosition;
			localPosition.z = 0f;
			uINameplateStatus.transform.localPosition = localPosition;
			uINameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
			uINameplateStatus.m_StatusText.text = iconForStatusType.popupText;
			if (iconForStatusType.isDebuff)
			{
				uINameplateStatus.m_StatusText.font = m_debuffFont;
			}
			else
			{
				uINameplateStatus.m_StatusText.font = m_buffFont;
			}
			if (iconForStatusType.isDebuff)
			{
				uINameplateStatus.DisplayAsNegativeStatus(this);
			}
			else
			{
				uINameplateStatus.DisplayAsPositiveStatus(this);
			}
			item.statusObject = uINameplateStatus;
			item.statusType = newType;
			m_statusEffectsAnimating.Add(item);
			m_currentStatusStackCount++;
			return;
		}
	}

	public void RemoveStatus(StatusType newType)
	{
		for (int i = 0; i < m_statusEffects.Count; i++)
		{
			StaticStatusDisplayInfo value = m_statusEffects[i];
			if (value.statusType == newType)
			{
				value.m_removedBuff = true;
				m_statusEffects[i] = value;
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void UpdateStatusDuration(StatusType status, int newDuration)
	{
		int num = 0;
		StaticStatusDisplayInfo staticStatusDisplayInfo;
		while (true)
		{
			if (num < m_statusEffects.Count)
			{
				staticStatusDisplayInfo = m_statusEffects[num];
				if (staticStatusDisplayInfo.statusType == status)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		while (true)
		{
			staticStatusDisplayInfo.statusObject.Setup(status, newDuration);
			return;
		}
	}

	public void SetDebugText(string text)
	{
		m_debugText.text = text;
	}

	public void UpdateTargetingAbilityIndicator(Ability ability, AbilityData.ActionType action, int index)
	{
		if (index < 6)
		{
			while (m_targetingAbilityIndicators.Count <= index)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(m_targetingAbilityIndicatorPrefab);
				UITargetingAbilityIndicator component = gameObject.GetComponent<UITargetingAbilityIndicator>();
				component.transform.SetParent(m_targetingAbilityIconsGrid.transform);
				component.transform.localScale = Vector3.one;
				component.transform.localPosition = Vector3.zero;
				component.transform.localEulerAngles = Vector3.zero;
				m_targetingAbilityIndicators.Add(component);
			}
			m_targetingAbilityIndicators[index].Setup(m_actorData, ability, action);
			if (!m_targetingAbilityIndicators[index].gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_targetingAbilityIndicators[index], true);
			}
		}
		if (m_catalsystPips == null)
		{
			m_catalsystPips = UnityEngine.Object.Instantiate(m_catalystIndicatorPrefab);
			m_catalsystPips.transform.SetParent(m_targetingAbilityIconsGrid.transform);
			m_catalsystPips.transform.localScale = Vector3.one;
			m_catalsystPips.transform.localPosition = Vector3.zero;
			m_catalsystPips.transform.localEulerAngles = Vector3.zero;
		}
		UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
		m_actorData.GetAbilityData().UpdateCatalystDisplay();
	}

	public void TurnOffTargetingAbilityIndicator(int fromIndex)
	{
		for (int i = fromIndex; i < m_targetingAbilityIndicators.Count; i++)
		{
			if (m_targetingAbilityIndicators[i].gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_targetingAbilityIndicators[i], false);
			}
		}
		if (fromIndex != 0)
		{
			return;
		}
		while (true)
		{
			SetCatalystsVisible(false);
			return;
		}
	}

	public void SpawnOvercon(UIOverconData.NameToOverconEntry entry, bool skipValidation)
	{
		if (!(m_overconParent != null) || !(m_overconPrefab != null))
		{
			return;
		}
		TextConsole.AllowedEmojis allowedEmojis = default(TextConsole.AllowedEmojis);
		while (true)
		{
			GameWideData gameWideData = GameWideData.Get();
			if (m_overcon != null)
			{
				UnityEngine.Object.Destroy(m_overcon.gameObject);
				m_overcon = null;
			}
			if (GameFlowData.Get().CurrentTurn > m_lastTurnOverconUsed)
			{
				m_numOverconUsesThisTurn = 0;
				m_lastTurnOverconUsed = GameFlowData.Get().CurrentTurn;
			}
			bool flag = true;
			string term = null;
			int num = 0;
			if (skipValidation)
			{
				flag = false;
			}
			else if (m_numOverconUsesThisTurn >= gameWideData.NumOverconsPerTurn)
			{
				if (m_actorData.isLocalPlayer)
				{
					term = "OverconLimitPerTurn";
					num = gameWideData.NumOverconsPerTurn;
				}
			}
			else if (m_numOverconUsesThisMatch >= gameWideData.NumOverconsPerMatch)
			{
				if (m_actorData.isLocalPlayer)
				{
					term = "OverconLimitPerMatch";
					num = gameWideData.NumOverconsPerMatch;
				}
			}
			else if (m_numOverconUsesById[entry.m_overconId] >= entry.m_maxUsesPerMatch)
			{
				if (m_actorData.isLocalPlayer)
				{
					term = "SpecificOverconLimitPerMatch";
					num = entry.m_maxUsesPerMatch;
				}
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				if (m_actorData.isLocalPlayer)
				{
					TextConsole.Message message = default(TextConsole.Message);
					message.MessageType = ConsoleMessageType.SystemMessage;
					message.Text = string.Format(StringUtil.TR(term, "HUDScene"), num);
					allowedEmojis.emojis = new List<int>();
					HUD_UI.Get().m_textConsole.HandleMessage(message, allowedEmojis);
				}
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(m_overconPrefab);
			m_overcon = gameObject.GetComponent<UINameplateOvercon>();
			if (!(m_overcon != null))
			{
				return;
			}
			while (true)
			{
				if (!skipValidation)
				{
					m_numOverconUsesThisMatch++;
					m_numOverconUsesThisTurn++;
					m_numOverconUsesById[entry.m_overconId]++;
				}
				m_overcon.Initialize(m_actorData, entry);
				m_overcon.transform.SetParent(m_overconParent.transform);
				m_overcon.transform.localPosition = Vector3.zero;
				m_overcon.transform.localScale = Vector3.one;
				if (string.IsNullOrEmpty(entry.m_audioEvent))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							UIFrontEnd.PlaySound(FrontEndButtonSounds.OverconUsed);
							return;
						}
					}
				}
				AudioManager.PostEvent(entry.m_audioEvent);
				return;
			}
		}
	}

	public void UpdateSelfNameplate(Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		UIManager.SetGameObjectActive(m_coverSymbol, false);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		HashSet<TechPointInteractionType> hashSet = new HashSet<TechPointInteractionType>();
		TechPointInteraction[] baseTechPointInteractions = abilityTargeting.GetBaseTechPointInteractions();
		for (int i = 0; i < baseTechPointInteractions.Length; i++)
		{
			int num9 = baseTechPointInteractions[i].m_amount;
			TechPointInteractionType type = baseTechPointInteractions[i].m_type;
			if (hashSet.Contains(type))
			{
				continue;
			}
			hashSet.Add(type);
			if (abilityTargeting.CurrentAbilityMod != null)
			{
				num9 = abilityTargeting.CurrentAbilityMod.GetModdedTechPointForInteraction(type, num9);
			}
			if (type == TechPointInteractionType.RewardOnCast)
			{
				num2 += num9;
			}
			else if (type == TechPointInteractionType.RewardOnDamage_OncePerCast)
			{
				num3 += num9;
			}
			else if (type == TechPointInteractionType.RewardOnHit_OncePerCast)
			{
				num4 += num9;
			}
			else if (type == TechPointInteractionType.RewardOnDamage_PerTarget)
			{
				num5 += num9;
			}
			else if (type == TechPointInteractionType.RewardOnHit_PerTarget)
			{
				num6 += num9;
			}
			else if (type == TechPointInteractionType.RewardOnHit_PerAllyTarget)
			{
				num7 += num9;
			}
			else if (type == TechPointInteractionType.RewardOnHit_PerEnemyTarget)
			{
				num8 += num9;
			}
		}
		while (true)
		{
			if (abilityTargeting.CurrentAbilityMod != null)
			{
				TechPointInteractionMod[] techPointInteractionMods = abilityTargeting.CurrentAbilityMod.m_techPointInteractionMods;
				foreach (TechPointInteractionMod techPointInteractionMod in techPointInteractionMods)
				{
					if (hashSet.Contains(techPointInteractionMod.interactionType))
					{
						continue;
					}
					hashSet.Add(techPointInteractionMod.interactionType);
					int num10 = 0;
					TechPointInteractionType interactionType = techPointInteractionMod.interactionType;
					hashSet.Add(interactionType);
					if (abilityTargeting.CurrentAbilityMod != null)
					{
						num10 = Mathf.Max(0, abilityTargeting.CurrentAbilityMod.GetModdedTechPointForInteraction(interactionType, 0));
					}
					if (interactionType == TechPointInteractionType.RewardOnCast)
					{
						num2 += num10;
					}
					else if (interactionType == TechPointInteractionType.RewardOnDamage_OncePerCast)
					{
						num3 += num10;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_OncePerCast)
					{
						num4 += num10;
					}
					else if (interactionType == TechPointInteractionType.RewardOnDamage_PerTarget)
					{
						num5 += num10;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_PerTarget)
					{
						num6 += num10;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_PerAllyTarget)
					{
						num7 += num10;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_PerEnemyTarget)
					{
						num8 += num10;
					}
				}
			}
			num = AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num2);
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			int numAlliesExcludingSelf = 0;
			int numEnemies = 0;
			bool hittingSelf = false;
			int targetCounts = abilityTargeting.GetTargetCounts(activeOwnedActorData, currentTargeterIndex, out numAlliesExcludingSelf, out numEnemies, out hittingSelf);
			if (num3 > 0)
			{
				if (numEnemies > 0)
				{
					num += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num3);
				}
			}
			if (num5 > 0)
			{
				num += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num5) * numEnemies;
			}
			if (num4 > 0)
			{
				if (targetCounts > 0)
				{
					num += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num4);
				}
			}
			if (num6 > 0)
			{
				num += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num6) * targetCounts;
			}
			if (num7 > 0)
			{
				num += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num7) * numAlliesExcludingSelf;
			}
			if (num8 > 0)
			{
				num += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num8) * numEnemies;
			}
			int num11 = abilityTargeting.GetAdditionalTechPointGainForNameplateItem(m_actorData, currentTargeterIndex);
			if (abilityTargeting.StatusAdjustAdditionalTechPointForTargeting())
			{
				num11 = AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, num11);
			}
			int num12;
			if (inConfirm)
			{
				num12 = ((Time.time - m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay) ? 1 : 0);
			}
			else
			{
				num12 = 0;
			}
			bool fadeOut = (byte)num12 != 0;
			bool flag = false;
			if (baseTechPointInteractions.Length <= 0)
			{
				if (abilityTargeting.CurrentAbilityMod != null)
				{
					if (abilityTargeting.CurrentAbilityMod.m_techPointInteractionMods.Length > 0)
					{
						goto IL_0471;
					}
				}
				if (num11 <= 0)
				{
					UIManager.SetGameObjectActive(abilityModifiers[0].m_abilityModifierText, false);
					goto IL_0586;
				}
			}
			goto IL_0471;
			IL_0586:
			bool hasDamage = false;
			int startModifierIndex;
			if (flag)
			{
				startModifierIndex = 1;
			}
			else
			{
				startModifierIndex = 0;
			}
			SetAbilityModifiersForTargetActor(startModifierIndex, abilityTargeting, currentTargeterIndex, false, inConfirm, out hasDamage);
			return;
			IL_0471:
			if (inConfirm && num + num11 == 0)
			{
				UIManager.SetGameObjectActive(abilityModifiers[0].m_abilityModifierText, false);
			}
			else if (m_textVisible)
			{
				UIManager.SetGameObjectActive(abilityModifiers[0].m_abilityModifierText, true);
				abilityModifiers[0].m_targetingTextAnimationController.SetBool("IsOn", true);
				flag = true;
				abilityModifiers[0].m_abilityModifierText.text = (num + num11).ToString();
				Color energyColor = m_EnergyColor;
				Color color = abilityModifiers[0].m_abilityModifierText.color;
				Color targetingNumberColor = GetTargetingNumberColor(energyColor, color.a, fadeOut);
				abilityModifiers[0].m_abilityModifierText.color = targetingNumberColor;
				SetGlowTargeting(true, BarColor.Self);
			}
			goto IL_0586;
		}
	}

	public void UpdateNameplateTargeted(ActorData targetingActor, Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		int targetedActorValue = 0;
		bool flag = false;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (!(actorData != null))
			{
				continue;
			}
			if (!(actorData != targetingActor))
			{
				continue;
			}
			if (actorData.GetTeam() != targetingActor.GetTeam())
			{
				continue;
			}
			ActorTargeting actorTargeting = actorData.GetActorTargeting();
			if (!(actorTargeting != null))
			{
				continue;
			}
			if (targetingActor.GetTeam() != m_actorData.GetTeam())
			{
				actorTargeting.IsTargetingActor(m_actorData, AbilityTooltipSymbol.Damage, ref targetedActorValue);
			}
			else
			{
				actorTargeting.IsTargetingActor(m_actorData, AbilityTooltipSymbol.Healing, ref targetedActorValue);
				flag = actorTargeting.IsTargetingActor(m_actorData, AbilityTooltipSymbol.Absorb, ref targetedActorValue);
			}
		}
		while (true)
		{
			if (targetedActorValue > 0)
			{
				if (HighlightUtils.Get().m_enableAccumulatedAllyNumbers)
				{
					if (m_textVisible)
					{
						UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, true);
						allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
						allyAbilityModifier.m_abilityModifierText.text = targetedActorValue + " +";
						int num;
						if (inConfirm)
						{
							num = ((Time.time - m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						bool fadeOut = (byte)num != 0;
						Color color;
						if (targetingActor.GetTeam() != m_actorData.GetTeam())
						{
							color = m_DamageColor;
						}
						else if (flag)
						{
							color = m_AbsorbColor;
						}
						else
						{
							color = m_HealingColor;
						}
						Color color2 = color;
						Color fullColor = color2;
						Color color3 = allyAbilityModifier.m_abilityModifierText.color;
						color2 = GetTargetingNumberColor(fullColor, color3.a, fadeOut);
						allyAbilityModifier.m_abilityModifierText.color = color2;
					}
					goto IL_0259;
				}
			}
			UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, false);
			goto IL_0259;
			IL_0259:
			bool hasDamage = false;
			int num2 = SetAbilityModifiersForTargetActor(0, abilityTargeting, currentTargeterIndex, inCover, inConfirm, out hasDamage);
			for (int j = num2; j < abilityModifiers.Length; j++)
			{
				UIManager.SetGameObjectActive(abilityModifiers[j].m_abilityModifierText, false);
			}
			while (true)
			{
				Image coverSymbol = m_coverSymbol;
				int doActive;
				if (inCover)
				{
					doActive = (hasDamage ? 1 : 0);
				}
				else
				{
					doActive = 0;
				}
				UIManager.SetGameObjectActive(coverSymbol, (byte)doActive != 0);
				SetGlowTargeting(true, GetRelationshipWithPlayer(m_actorData));
				return;
			}
		}
	}

	public void Update()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		if (m_actorData == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (m_visible)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().activeOwnedActorData.GetAbilityData() != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								if (GameFlowData.Get().activeOwnedActorData.GetAbilityData().GetSelectedAbility() == null)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											SetStatusObjectInteractable(true);
											return;
										}
									}
								}
								SetStatusObjectInteractable(false);
								return;
							}
						}
					}
					return;
				}
			}
		}
		SetStatusObjectInteractable(false);
	}

	private void SetStatusObjectInteractable(bool interactable)
	{
		for (int i = 0; i < m_statusEffects.Count; i++)
		{
			StaticStatusDisplayInfo staticStatusDisplayInfo = m_statusEffects[i];
			SetInteractable(staticStatusDisplayInfo.statusObject.m_cGroup, interactable);
		}
	}

	private static void SetInteractable(CanvasGroup canvasGroup, bool interactable)
	{
		if (canvasGroup.blocksRaycasts != interactable)
		{
			canvasGroup.blocksRaycasts = interactable;
		}
		if (canvasGroup.interactable != interactable)
		{
			canvasGroup.interactable = interactable;
		}
	}

	public void SetCatalystsVisible(bool visible)
	{
		if (m_catalsystPips != null)
		{
			UIManager.SetGameObjectActive(m_catalsystPips, visible);
		}
	}

	public void UpdateCatalysts(List<Ability> cardAbilities)
	{
		if (m_catalsystPips == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		for (int i = 0; i < cardAbilities.Count; i++)
		{
			Ability ability = cardAbilities[i];
			if (!(ability != null))
			{
				continue;
			}
			AbilityRunPhase abilityRunPhase = Card.AbilityPriorityToRunPhase(ability.GetRunPriority());
			if (abilityRunPhase == AbilityRunPhase.Prep)
			{
				doActive = true;
			}
			else if (abilityRunPhase == AbilityRunPhase.Dash)
			{
				doActive2 = true;
			}
			else if (abilityRunPhase == AbilityRunPhase.Combat)
			{
				doActive3 = true;
			}
		}
		UIManager.SetGameObjectActive(m_catalsystPips.m_PrepPhaseOn, doActive);
		UIManager.SetGameObjectActive(m_catalsystPips.m_DashPhaseOn, doActive2);
		UIManager.SetGameObjectActive(m_catalsystPips.m_BlastPhaseOn, doActive3);
		UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
	}

	public void UpdateNameplateUntargeted(bool doInstantHide = false)
	{
		UIManager.SetGameObjectActive(m_coverSymbol, false);
		for (int i = 0; i < abilityModifiers.Length; i++)
		{
			if (doInstantHide)
			{
				if (abilityModifiers[i].m_abilityModifierText.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
				}
			}
			else if (abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeInHierarchy)
			{
				abilityModifiers[i].m_targetingTextAnimationController.SetTrigger("DoOff");
				abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", false);
			}
		}
		while (true)
		{
			if (doInstantHide)
			{
				if (allyAbilityModifier.m_abilityModifierText.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, false);
				}
			}
			else if (allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeInHierarchy)
			{
				allyAbilityModifier.m_targetingTextAnimationController.SetTrigger("DoOff");
				allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", false);
			}
			SetGlowTargeting(false, BarColor.None);
			return;
		}
	}

	private void SetGlowTargeting(bool setGlow, BarColor teamColor)
	{
		UIManager.SetGameObjectActive(m_targetGlow, false);
	}

	private int SetAbilityModifiersForTargetActor(int startModifierIndex, Ability abilityTargeting, int currentTargeterIndex, bool inCover, bool inConfirm, out bool hasDamage)
	{
		hasDamage = false;
		object obj;
		if (GameFlowData.Get() != null)
		{
			obj = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData = (ActorData)obj;
		int num;
		if (inConfirm)
		{
			num = ((Time.time - m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool fadeOut = (byte)num != 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		m_tempTargetingNumSymbolToValueMap.Clear();
		Dictionary<AbilityTooltipSymbol, int> tempTargetingNumSymbolToValueMap = m_tempTargetingNumSymbolToValueMap;
		ActorTargeting.GetNameplateNumbersForTargeter(actorData, m_actorData, abilityTargeting, currentTargeterIndex, tempTargetingNumSymbolToValueMap);
		int num2 = startModifierIndex;
		using (Dictionary<AbilityTooltipSymbol, int>.Enumerator enumerator = tempTargetingNumSymbolToValueMap.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AbilityTooltipSymbol, int> current = enumerator.Current;
				if (num2 < abilityModifiers.Length)
				{
					if (!m_textVisible)
					{
					}
					else
					{
						AbilityTooltipSymbol key = current.Key;
						int value = current.Value;
						AbilityModifier abilityModifier = abilityModifiers[num2];
						switch (key)
						{
						case AbilityTooltipSymbol.Damage:
							if (value > 0 && !flag)
							{
								UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
								abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
								int baseValue = value;
								string text = baseValue.ToString();
								string accessoryTargeterNumberString = abilityTargeting.GetAccessoryTargeterNumberString(m_actorData, key, baseValue);
								if (!string.IsNullOrEmpty(accessoryTargeterNumberString))
								{
									text += accessoryTargeterNumberString;
								}
								abilityModifier.m_abilityModifierText.text = text;
								Color damageColor = m_DamageColor;
								Color color4 = abilityModifier.m_abilityModifierText.color;
								Color targetingNumberColor4 = GetTargetingNumberColor(damageColor, color4.a, fadeOut);
								abilityModifier.m_abilityModifierText.color = targetingNumberColor4;
								flag = true;
								hasDamage = true;
								num2++;
							}
							break;
						case AbilityTooltipSymbol.Healing:
							if (value > 0)
							{
								if (!flag2)
								{
									UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
									abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
									int num4 = value;
									abilityModifier.m_abilityModifierText.text = num4.ToString();
									Color healingColor = m_HealingColor;
									Color color3 = abilityModifier.m_abilityModifierText.color;
									Color targetingNumberColor3 = GetTargetingNumberColor(healingColor, color3.a, fadeOut);
									abilityModifier.m_abilityModifierText.color = targetingNumberColor3;
									flag2 = true;
									num2++;
								}
							}
							break;
						case AbilityTooltipSymbol.Absorb:
							if (!flag4)
							{
								UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
								abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
								abilityModifier.m_abilityModifierText.text = value.ToString();
								Color absorbColor = m_AbsorbColor;
								Color color2 = abilityModifier.m_abilityModifierText.color;
								Color targetingNumberColor2 = GetTargetingNumberColor(absorbColor, color2.a, fadeOut);
								abilityModifier.m_abilityModifierText.color = targetingNumberColor2;
								flag4 = true;
								num2++;
							}
							break;
						case AbilityTooltipSymbol.Energy:
							if (actorData != m_actorData && !flag3)
							{
								UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
								abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
								int num3 = value;
								abilityModifier.m_abilityModifierText.text = num3.ToString();
								Color energyColor = m_EnergyColor;
								Color color = abilityModifier.m_abilityModifierText.color;
								Color targetingNumberColor = GetTargetingNumberColor(energyColor, color.a, fadeOut);
								abilityModifier.m_abilityModifierText.color = targetingNumberColor;
								flag3 = true;
								num2++;
							}
							break;
						default:
							UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, false);
							break;
						}
					}
				}
			}
		}
		for (int i = num2; i < abilityModifiers.Length; i++)
		{
			UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
		}
		while (true)
		{
			return num2;
		}
	}

	private Color GetTargetingNumberColor(Color fullColor, float alphaNow, bool fadeOut)
	{
		if (fadeOut)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					Color result = fullColor;
					result.a = Mathf.Max(0f, alphaNow - Time.deltaTime * HUD_UIResources.Get().m_confirmedTargetingFadeoutSpeed);
					return result;
				}
				}
			}
		}
		return fullColor;
	}

	public void SetBarColors(BarColor newBarcolor)
	{
		if (barsToShow == newBarcolor)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		barsToShow = newBarcolor;
		lastBarColor = newBarcolor;
		switch (barsToShow)
		{
		case BarColor.Self:
			SetTeamBars(m_selfBars, true);
			SetTeamBars(m_teamBars, false);
			SetTeamBars(m_enemyBars, false);
			break;
		case BarColor.Team:
			SetTeamBars(m_selfBars, false);
			SetTeamBars(m_teamBars, true);
			SetTeamBars(m_enemyBars, false);
			break;
		case BarColor.Enemy:
			SetTeamBars(m_selfBars, false);
			SetTeamBars(m_teamBars, false);
			SetTeamBars(m_enemyBars, true);
			break;
		default:
			SetTeamBars(m_selfBars, false);
			SetTeamBars(m_teamBars, false);
			SetTeamBars(m_enemyBars, false);
			break;
		}
	}

	private bool IsInFrontOfCamera()
	{
		bool result = false;
		if (m_actorData != null)
		{
			Vector3 nameplatePosition = m_actorData.GetNameplatePosition(30f);
			Vector3 vector = Camera.main.WorldToViewportPoint(nameplatePosition);
			if (vector.z < 0f)
			{
				result = false;
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	private bool IsVisibleInDecisionPhase()
	{
		bool result = false;
		if (m_actorData != null)
		{
			result = (m_actorData.ShouldShowNameplate() && IsInFrontOfCamera());
		}
		return result;
	}

	public void ForceFinishStatusAnims()
	{
		UINameplateStatus[] componentsInChildren = m_statusContainer.GetComponentsInChildren<UINameplateStatus>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].AnimDone();
		}
		while (true)
		{
			return;
		}
	}

	private void SetVisible(bool visible)
	{
		if (!m_visible && visible)
		{
			ForceFinishStatusAnims();
		}
		if (m_visible)
		{
			if (!visible)
			{
				HideCombatTexts();
			}
		}
		m_visible = visible;
		if (visible)
		{
			if (m_alphaToUse > 0f)
			{
				SetAlpha(m_alphaToUse);
			}
			else
			{
				SetAlpha(1f);
			}
		}
		else
		{
			SetAlpha(0f);
		}
		if (!m_visible)
		{
			m_mouseIsOverHP = false;
		}
		using (List<UITargetingAbilityIndicator>.Enumerator enumerator = m_targetingAbilityIndicators.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UITargetingAbilityIndicator current = enumerator.Current;
				current.SetCanvasGroupVisibility(m_visible);
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void SetAlpha(float alpha)
	{
		m_alphaToUse = alpha;
		if (m_visible)
		{
			m_canvasGroup.alpha = m_alphaToUse;
			m_abilityPreviewCanvasGroup.alpha = m_alphaToUse;
		}
		else
		{
			m_canvasGroup.alpha = 0f;
			m_abilityPreviewCanvasGroup.alpha = 0f;
		}
		SetInteractable(m_abilityPreviewCanvasGroup, m_abilityPreviewCanvasGroup.alpha > 0f);
	}

	public void NotifyStatusAnimationDone(UINameplateStatus nameplateStatus, bool gainedStatus)
	{
		StaticStatusDisplayInfo item2 = default(StaticStatusDisplayInfo);
		for (int i = 0; i < m_statusEffectsAnimating.Count; i++)
		{
			StatusDisplayInfo item = m_statusEffectsAnimating[i];
			if (!(item.statusObject == nameplateStatus))
			{
				continue;
			}
			if (gainedStatus)
			{
				UIBuffIndicator uIBuffIndicator = UnityEngine.Object.Instantiate(m_buffIndicatorPrefab);
				uIBuffIndicator.transform.SetParent(m_buffIndicatorGrid.transform);
				uIBuffIndicator.transform.localScale = Vector3.one;
				uIBuffIndicator.transform.localPosition = Vector3.zero;
				uIBuffIndicator.transform.localEulerAngles = Vector3.zero;
				uIBuffIndicator.Setup(item.statusType, m_actorData.GetActorStatus().GetDurationOfStatus(item.statusType));
				CanvasGroup component = uIBuffIndicator.gameObject.GetComponent<CanvasGroup>();
				component.alpha = 0f;
				item2.m_removedBuff = false;
				item2.statusObject = uIBuffIndicator;
				item2.statusType = item.statusType;
				HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(item.statusType);
				bool flag = false;
				for (int j = 0; j < m_statusEffects.Count; j++)
				{
					StaticStatusDisplayInfo staticStatusDisplayInfo = m_statusEffects[j];
					if (!staticStatusDisplayInfo.m_removedBuff)
					{
						StaticStatusDisplayInfo staticStatusDisplayInfo2 = m_statusEffects[j];
						if (staticStatusDisplayInfo2.statusType == item2.statusType)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					if (!iconForStatusType.isDebuff)
					{
						int index = 0;
						for (int k = 0; k < m_statusEffects.Count; k++)
						{
							StaticStatusDisplayInfo staticStatusDisplayInfo3 = m_statusEffects[k];
							HUD_UIResources.StatusTypeIcon iconForStatusType2 = HUD_UIResources.GetIconForStatusType(staticStatusDisplayInfo3.statusType);
							if (iconForStatusType2.isDebuff)
							{
								index = k;
								break;
							}
						}
						m_statusEffects.Insert(index, item2);
						for (int l = 0; l < m_statusEffects.Count; l++)
						{
							StaticStatusDisplayInfo staticStatusDisplayInfo4 = m_statusEffects[l];
							staticStatusDisplayInfo4.statusObject.gameObject.transform.SetAsLastSibling();
						}
					}
					else
					{
						m_statusEffects.Add(item2);
					}
				}
			}
			m_statusEffectsAnimating.Remove(item);
			UnityEngine.Object.Destroy(nameplateStatus.gameObject);
			i--;
			if (!m_actorData.GetActorStatus().HasStatus(item.statusType))
			{
				RemoveStatus(item.statusType);
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void SetBarPercentVisual(Image image, float percent)
	{
		m_selfBars.m_currentHPPercent = m_HPEased;
		if (image.fillAmount == percent)
		{
			return;
		}
		while (true)
		{
			image.fillAmount = percent;
			return;
		}
	}

	private void LateUpdate()
	{
		if (Camera.main == null)
		{
			while (true)
			{
				return;
			}
		}
		if (m_setToDimTime > 0f)
		{
			if (Time.time >= m_setToDimTime)
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
					{
						DimNameplateForAbility();
						goto IL_0098;
					}
				}
				m_setToDimTime = -1f;
			}
		}
		goto IL_0098;
		IL_0098:
		bool flag = IsVisibleInDecisionPhase();
		if (m_visible != flag)
		{
			SetVisible(flag);
		}
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				if (m_actorData != null)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (activeOwnedActorData != null)
					{
						if (activeOwnedActorData == m_actorData)
						{
							SetBarColors(BarColor.Self);
						}
						else if (activeOwnedActorData.GetTeam() == m_actorData.GetTeam())
						{
							SetBarColors(BarColor.Team);
						}
						else if (activeOwnedActorData.GetTeam() == m_actorData.GetTeam().OtherTeam())
						{
							SetBarColors(BarColor.Enemy);
						}
						else
						{
							SetBarColors(BarColor.None);
						}
					}
					else if (m_actorData.GetTeam() == Team.TeamA)
					{
						SetBarColors(BarColor.Team);
					}
					else
					{
						SetBarColors(BarColor.Enemy);
					}
				}
			}
		}
		if (lastBarColor != barsToShow)
		{
			SetBarColors(barsToShow);
		}
		if (!(m_actorData != null))
		{
			return;
		}
		if (myCanvas == null)
		{
			myCanvas = HUD_UI.Get().GetTopLevelCanvas();
		}
		if (myCanvas != null && CanvasRect == null)
		{
			CanvasRect = (myCanvas.transform as RectTransform);
		}
		Vector3 vector;
		if (m_actorData.ShouldShowNameplate())
		{
			vector = m_actorData.GetNameplatePosition(-2f);
		}
		else if (!m_actorData.IsDead())
		{
			vector = (m_actorData.IsVisibleToClient() ? m_actorData.GetNameplatePosition(-2f) : m_actorData.GetClientLastKnownPos());
		}
		else
		{
			vector = m_actorData.LastDeathPosition;
		}
		Vector2 vector2 = Camera.main.WorldToViewportPoint(vector);
		float x = vector2.x;
		Vector2 sizeDelta = CanvasRect.sizeDelta;
		float x2 = x * sizeDelta.x;
		float y = vector2.y;
		Vector2 sizeDelta2 = CanvasRect.sizeDelta;
		Vector2 anchoredPosition = new Vector2(x2, y * sizeDelta2.y);
		(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
		Vector3 position = Camera.main.transform.position;
		m_distanceFromCamera = (vector - position).sqrMagnitude;
		OnHitUpdate();
		int hitPointsAfterResolution = m_actorData.GetHitPointsAfterResolution();
		int clientUnappliedHoTTotal_ToDisplay_zq = m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
		if (hitPointsAfterResolution == 0)
		{
			if (!m_zeroHealthIcon.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_zeroHealthIcon, true);
			}
			if (m_healthLabel.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_healthLabel, false);
			}
		}
		else
		{
			if (m_zeroHealthIcon.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_zeroHealthIcon, false);
			}
			if (m_healthLabel.gameObject.activeSelf != m_textVisible)
			{
				UIManager.SetGameObjectActive(m_healthLabel, m_textVisible);
			}
		}
		if (!object.ReferenceEquals(m_textNameLabel.text, m_actorData.GetFancyDisplayName()))
		{
			m_textNameLabel.text = m_actorData.GetFancyDisplayName();
		}
		int num = m_actorData._0004();
		if (num > 0)
		{
			m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = (m_actorData.GetAbilityData().GetSelectedAbility() == null);
			if (m_mouseIsOverHP)
			{
				if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
				{
					m_healthLabel.text = $"{hitPointsAfterResolution.ToString()} + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor)}>{num.ToString()}</color> + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextHotColor)}>{clientUnappliedHoTTotal_ToDisplay_zq.ToString()}</color>";
				}
				else
				{
					m_healthLabel.text = $"{hitPointsAfterResolution.ToString()} + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor)}>{num.ToString()}</color>";
				}
			}
			else
			{
				m_healthLabel.text = $"<color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor)}>{(hitPointsAfterResolution + num).ToString()}</color>";
			}
		}
		else if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
		{
			m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = (m_actorData.GetAbilityData().GetSelectedAbility() == null);
			if (m_mouseIsOverHP)
			{
				m_healthLabel.text = $"{hitPointsAfterResolution.ToString()} + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextHotColor)}>{clientUnappliedHoTTotal_ToDisplay_zq.ToString()}</color>";
			}
			else
			{
				m_healthLabel.color = Color.white;
				string text = hitPointsAfterResolution.ToString();
				if (!m_healthLabel.text.Equals(text))
				{
					m_healthLabel.text = text;
				}
			}
		}
		else
		{
			m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = false;
			m_healthLabel.color = Color.white;
			string text2 = hitPointsAfterResolution.ToString();
			if (!m_healthLabel.text.Equals(text2))
			{
				m_healthLabel.text = text2;
			}
		}
		int energyToDisplay = m_actorData.GetEnergyToDisplay();
		int actualMaxTechPoints = m_actorData.GetActualMaxTechPoints();
		if (energyToDisplay != m_previousTP || actualMaxTechPoints != m_previousTPMax)
		{
			float endValue = 0f;
			if (actualMaxTechPoints != 0)
			{
				endValue = (float)energyToDisplay / (float)actualMaxTechPoints;
			}
			if (energyToDisplay > m_previousTP)
			{
				m_TPEased.EaseTo(endValue, 0f);
				m_TPEasedGained.EaseTo(endValue, 2.5f);
			}
			else
			{
				m_TPEasedGained.EaseTo(endValue, 0f);
				m_TPEased.EaseTo(endValue, 2.5f);
			}
			List<Ability> abilitiesAsList = m_actorData.GetAbilityData().GetAbilitiesAsList();
			bool flag2 = false;
			if (abilitiesAsList.Count > 4 && abilitiesAsList[4] != null)
			{
				int num2;
				if (energyToDisplay >= abilitiesAsList[4].GetModdedCost() && !m_actorData.IsDead())
				{
					num2 = ((m_actorData.GetAbilityData().GetCooldownRemaining(AbilityData.ActionType.ABILITY_4) <= 0) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				flag2 = ((byte)num2 != 0);
			}
			int num3;
			if (energyToDisplay != actualMaxTechPoints)
			{
				num3 = (flag2 ? 1 : 0);
			}
			else
			{
				num3 = 1;
			}
			bool flag3 = (byte)num3 != 0;
			Image tpGainBar = m_tpGainBar;
			Color color;
			if (flag3)
			{
				color = m_FullEnergyColor;
			}
			else
			{
				color = m_NonFullEnergyColor;
			}
			tpGainBar.color = color;
			if (flag3 != m_isMaxEnergy)
			{
				m_maxEnergyAnimator.GetComponent<_DisableGameObjectOnAnimationDoneEvent>().enabled = !flag3;
				if (flag3)
				{
					UIManager.SetGameObjectActive(m_maxEnergyContainer, true);
					if (GameFlowData.Get() != null)
					{
						if (GameFlowData.Get().activeOwnedActorData == m_actorData)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.MaxEnergyReached);
						}
					}
				}
				Animator maxEnergyAnimator = m_maxEnergyAnimator;
				object str;
				if (flag3)
				{
					str = "IN";
				}
				else
				{
					str = "OUT";
				}
				maxEnergyAnimator.Play("NameplateMaxEnergyDefault" + (string)str);
				m_isMaxEnergy = flag3;
			}
		}
		if ((float)m_TPEased != m_tpGainedEasePercent)
		{
			m_tpGainedEasePercent = m_TPEased;
			SetBarPercentVisual(m_tpGainEaseBar, ModifyTPBarPercentForArt(m_tpGainedEasePercent));
		}
		if ((float)m_TPEasedGained != m_tpGainedPercent)
		{
			m_tpGainedPercent = m_TPEasedGained;
			SetBarPercentVisual(m_tpGainBar, ModifyTPBarPercentForArt(m_tpGainedPercent));
		}
		if (m_shieldBar != null)
		{
			if (m_ShieldEased.EndValue() <= m_HPEased.EndValue())
			{
				SetBarPercentVisual(m_shieldBar, 0f);
			}
			else
			{
				SetBarPercentVisual(m_shieldBar, ModifyHPBarPercentForArt(m_shieldPercent));
				m_shieldPercent = m_ShieldEased;
			}
		}
		if (m_hpGainBar != null)
		{
			SetBarPercentVisual(m_hpGainBar, ModifyHPBarPercentForArt(m_hpGainedPercent));
			m_hpGainedPercent = m_HPGainedEased;
			if (m_mouseIsOverHP)
			{
				Color color2 = m_hpGainBar.color;
				if (m_hpGainedIsFading)
				{
					color2.a -= 0.05f;
					if (color2.a <= 0f)
					{
						color2.a = 0f;
						m_hpGainedIsFading = !m_hpGainedIsFading;
					}
				}
				else
				{
					color2.a += 0.05f;
					if (color2.a >= 1f)
					{
						color2.a = 1f;
						m_hpGainedIsFading = !m_hpGainedIsFading;
					}
				}
				m_hpGainBar.color = color2;
			}
			else
			{
				m_hpGainBar.color = Color.white;
			}
		}
		BarColor barColor = barsToShow;
		if (barColor != BarColor.Self)
		{
			if (barColor != BarColor.Team)
			{
				if (barColor != BarColor.Enemy)
				{
				}
				else
				{
					if (m_enemyBars.m_currentHPBar != null)
					{
						SetBarPercentVisual(m_enemyBars.m_currentHPBar, ModifyHPBarPercentForArt(m_HPEased));
						m_enemyBars.m_currentHPPercent = m_HPEased;
					}
					if (m_enemyBars.m_damageEasedBar != null)
					{
						SetBarPercentVisual(m_enemyBars.m_damageEasedBar, ModifyHPBarPercentForArt(m_HPDamageEased));
						m_enemyBars.m_damageEasePercent = m_HPDamageEased;
					}
				}
			}
			else
			{
				if (m_teamBars.m_currentHPBar != null)
				{
					SetBarPercentVisual(m_teamBars.m_currentHPBar, ModifyHPBarPercentForArt(m_HPEased));
					m_teamBars.m_currentHPPercent = m_HPEased;
				}
				if (m_teamBars.m_damageEasedBar != null)
				{
					SetBarPercentVisual(m_teamBars.m_damageEasedBar, ModifyHPBarPercentForArt(m_HPDamageEased));
					m_teamBars.m_damageEasePercent = m_HPDamageEased;
				}
			}
		}
		else
		{
			if (m_selfBars.m_currentHPBar != null)
			{
				SetBarPercentVisual(m_selfBars.m_currentHPBar, ModifyHPBarPercentForArt(m_HPEased));
				m_selfBars.m_currentHPPercent = m_HPEased;
			}
			if (m_selfBars.m_damageEasedBar != null)
			{
				SetBarPercentVisual(m_selfBars.m_damageEasedBar, ModifyHPBarPercentForArt(m_HPDamageEased));
				m_selfBars.m_damageEasePercent = m_HPDamageEased;
			}
		}
		if (m_statusEffectsAnimating.Count == 0)
		{
			m_currentStatusStackCount = 0;
		}
		for (int i = 0; i < m_statusEffects.Count; i++)
		{
			StaticStatusDisplayInfo staticStatusDisplayInfo = m_statusEffects[i];
			CanvasGroup component = staticStatusDisplayInfo.statusObject.gameObject.GetComponent<CanvasGroup>();
			if (!(component != null))
			{
				continue;
			}
			if (m_visible)
			{
				StaticStatusDisplayInfo staticStatusDisplayInfo2 = m_statusEffects[i];
				if (!staticStatusDisplayInfo2.m_removedBuff)
				{
					component.alpha += Time.deltaTime * HUD_UIResources.Get().m_nameplateStaticStatusFadeSpeed;
				}
				else
				{
					component.alpha -= Time.deltaTime * HUD_UIResources.Get().m_nameplateStaticStatusFadeSpeed;
				}
				if (component.alpha <= 0f)
				{
					StaticStatusDisplayInfo staticStatusDisplayInfo3 = m_statusEffects[i];
					StatusFadeOutDone(staticStatusDisplayInfo3.statusType);
					StaticStatusDisplayInfo staticStatusDisplayInfo4 = m_statusEffects[i];
					UnityEngine.Object.Destroy(staticStatusDisplayInfo4.statusObject.gameObject);
					m_statusEffects.RemoveAt(i);
					i--;
				}
				continue;
			}
			component.alpha = 0f;
			StaticStatusDisplayInfo staticStatusDisplayInfo5 = m_statusEffects[i];
			if (staticStatusDisplayInfo5.m_removedBuff)
			{
				StaticStatusDisplayInfo staticStatusDisplayInfo6 = m_statusEffects[i];
				StatusFadeOutDone(staticStatusDisplayInfo6.statusType);
				StaticStatusDisplayInfo staticStatusDisplayInfo7 = m_statusEffects[i];
				UnityEngine.Object.Destroy(staticStatusDisplayInfo7.statusObject.gameObject);
				m_statusEffects.RemoveAt(i);
				i--;
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private float ModifyHPBarPercentForArt(float actualPct)
	{
		return actualPct;
	}

	private float ModifyTPBarPercentForArt(float actualPct)
	{
		return actualPct;
	}

	private void SnapBarValues()
	{
		m_previousHP = m_actorData.GetHitPointsAfterResolution();
		m_previousHPMax = m_actorData.GetMaxHitPoints();
		m_previousShieldValue = m_actorData._0004();
		m_previousHPShieldAndHot = m_previousHP + m_previousShieldValue + m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
		SetMaxHPWithShield(m_previousHPMax + m_previousShieldValue);
		int num = m_previousHPMax + m_previousShieldValue;
		float duration = 0f;
		float num2;
		if (num == 0)
		{
			num2 = 0f;
		}
		else
		{
			num2 = (float)m_previousHP / (float)num;
		}
		float endValue = num2;
		m_HPDamageEased.EaseTo(endValue, duration);
		m_HPGainedEased.EaseTo((float)m_previousHPShieldAndHot / (float)num, duration);
		m_HPEased.EaseTo(endValue, duration);
		if (m_previousShieldValue > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_ShieldEased.EaseTo(((float)m_previousHP + (float)m_previousShieldValue) / (float)num, duration);
					return;
				}
			}
		}
		m_ShieldEased.EaseTo(0f, duration);
	}

	public void OnHitUpdate()
	{
		if (m_actorData == null)
		{
			return;
		}
		int num = m_actorData._0004();
		int hitPointsAfterResolution = m_actorData.GetHitPointsAfterResolution();
		int hitPoints = m_actorData.HitPoints;
		int clientUnappliedHoTTotal_ToDisplay_zq = m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
		int num2 = hitPointsAfterResolution + num + clientUnappliedHoTTotal_ToDisplay_zq;
		float num3 = (float)hitPointsAfterResolution / (float)m_maxHPWithShield;
		float num4 = m_HPEased.EndValue();
		float endValue = ((float)hitPointsAfterResolution + (float)num) / (float)m_maxHPWithShield;
		float endValue2 = (float)num2 / (float)m_maxHPWithShield;
		if (hitPointsAfterResolution < m_previousHP)
		{
			m_lostHealth = true;
			m_ShieldEased.EaseTo(endValue, 0f);
			float num5 = Mathf.Abs(num3 - m_HPEased.EndValue());
			m_HPEased.EaseTo(m_HPEased.EndValue() - num5, 0f);
			num5 = Mathf.Abs(num3 - m_HPDamageEased.EndValue());
			m_HPDamageEased.EaseTo(m_HPDamageEased.EndValue() - num5, 2.5f);
			m_previousHP = hitPointsAfterResolution;
		}
		else if (m_previousHP < hitPointsAfterResolution)
		{
			m_lostHealth = false;
			float num6 = Mathf.Abs(num3 - num4);
			if (m_previousShieldValue != 0)
			{
				m_ShieldEased.EaseTo(endValue, 2.5f);
			}
			m_HPEased.EaseTo(m_HPEased.EndValue() + num6, 2.5f);
			m_HPDamageEased.EaseTo(m_HPDamageEased.EndValue() + num6, 2.5f);
			m_previousHP = hitPointsAfterResolution;
		}
		if (m_previousShieldValue < num)
		{
			SetMaxHPWithShield(m_actorData.GetMaxHitPoints() + num);
			float endValue3 = ((float)hitPointsAfterResolution + (float)num) / (float)m_maxHPWithShield;
			float endValue4 = (float)hitPointsAfterResolution / (float)m_maxHPWithShield;
			m_ShieldEased.EaseTo(endValue3, 0f);
			m_HPEased.EaseTo(endValue4, 0f);
			m_HPDamageEased.EaseTo(endValue3, 1f);
			m_previousShieldValue = num;
		}
		else if (num < m_previousShieldValue)
		{
			if (!m_lostHealth)
			{
				SetMaxHPWithShield(m_actorData.GetMaxHitPoints() + num);
				float endValue5 = (float)hitPointsAfterResolution / (float)m_maxHPWithShield;
				float endValue6 = ((float)hitPointsAfterResolution + (float)num) / (float)m_maxHPWithShield;
				m_ShieldEased.EaseTo(endValue6, 0f);
				m_HPEased.EaseTo(endValue5, 0f);
				m_HPDamageEased.EaseTo(endValue6, 0f);
				m_previousShieldValue = num;
			}
		}
		if (num2 != m_previousHPShieldAndHot)
		{
			m_HPGainedEased.EaseTo(endValue2, 0f);
			m_previousHPShieldAndHot = num2;
		}
		if (m_previousResolvedHP == hitPoints)
		{
			return;
		}
		while (true)
		{
			m_previousResolvedHP = hitPoints;
			OnResolvedHitPoints();
			return;
		}
	}

	public void MoveToTopOfNameplates()
	{
		UIUtils.SetAsLastSiblingIfNeeded(base.transform);
	}

	public void OnResolvedHitPoints()
	{
		if (m_actorData == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_lostHealth = false;
		int num = m_actorData._0004();
		int hitPointsAfterResolution = m_actorData.GetHitPointsAfterResolution();
		SetMaxHPWithShield(m_actorData.GetMaxHitPoints() + num);
		float endValue = (float)hitPointsAfterResolution / (float)m_maxHPWithShield;
		float num2 = ((float)hitPointsAfterResolution + (float)num) / (float)m_maxHPWithShield;
		float num3 = (float)m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq() / (float)m_maxHPWithShield;
		m_ShieldEased.EaseTo(num2, 2.5f);
		m_HPEased.EaseTo(endValue, 2.5f);
		m_HPGainedEased.EaseTo(num2 + num3, 2.5f);
		m_HPDamageEased.EaseTo(num2, 2.5f);
	}

	public void Setup(ActorData actorData)
	{
		m_actorData = actorData;
		ActorData actorData2 = m_actorData;
		actorData2.m_onResolvedHitPoints = (ActorData.ActorDataDelegate)Delegate.Combine(actorData2.m_onResolvedHitPoints, new ActorData.ActorDataDelegate(OnResolvedHitPoints));
		m_textNameLabel.text = m_actorData.GetFancyDisplayName();
		SnapBarValues();
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (!(m_actorData != null))
		{
			return;
		}
		while (true)
		{
			if (eventType != GameEventManager.EventType.TheatricsAbilityHighlightStart)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (eventType != GameEventManager.EventType.TheatricsAbilitiesEnd)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									switch (eventType)
									{
									default:
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
									case GameEventManager.EventType.TurnTick:
										m_setToDimTime = -1f;
										SetAlpha(1f);
										base.transform.localScale = Vector3.one;
										break;
									case GameEventManager.EventType.ClientResolutionStarted:
										DimNameplateForAbility();
										break;
									case GameEventManager.EventType.NormalMovementStart:
									{
										GameEventManager.NormalMovementStartAgs normalMovementStartAgs = (GameEventManager.NormalMovementStartAgs)args;
										if (normalMovementStartAgs.m_actorsBeingHitMidMovement.Contains(m_actorData))
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													HighlightNameplateForAbility();
													return;
												}
											}
										}
										DimNameplateForAbility();
										break;
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
			GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = (GameEventManager.TheatricsAbilityHighlightStartArgs)args;
			if (!theatricsAbilityHighlightStartArgs.m_casters.Contains(m_actorData))
			{
				if (!theatricsAbilityHighlightStartArgs.m_targets.Contains(m_actorData))
				{
					DimNameplateForAbility();
					return;
				}
			}
			HighlightNameplateForAbility();
			return;
		}
	}

	private void HighlightNameplateForAbility()
	{
		SetAlpha(1f);
		base.transform.localScale = m_scaleWhenHighlighted * Vector3.one;
		m_setToDimTime = -1f;
	}

	private void DimNameplateForAbility()
	{
		SetAlpha(m_alphaWhenOthersHighlighted);
		base.transform.localScale = m_scaleWhenOthersHighlighted * Vector3.one;
		m_setToDimTime = -1f;
	}
}
