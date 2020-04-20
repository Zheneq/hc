using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINameplateItem : MonoBehaviour, IGameEventListener
{
	public UINameplateItem.NameplateCombatText[] nameplateCombatText;

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

	public UINameplateItem.TeamBars m_selfBars;

	private EasedFloat m_HPEased;

	public UINameplateItem.TeamBars m_teamBars;

	public UINameplateItem.TeamBars m_enemyBars;

	[Range(0f, 1f)]
	public float m_tpGainedPercent;

	public Image m_tpGainBar;

	private EasedFloat m_TPEasedGained;

	[Range(0f, 1f)]
	public float m_tpGainedEasePercent;

	public Image m_tpGainEaseBar;

	private EasedFloat m_TPEased;

	public UINameplateItem.BarColor barsToShow;

	public TextMeshProUGUI m_textNameLabel;

	public TextMeshProUGUI m_healthLabel;

	public RectTransform m_maxEnergyContainer;

	public Animator m_maxEnergyAnimator;

	public UINameplateItem.AbilityModifier[] abilityModifiers;

	public UINameplateItem.AbilityModifier allyAbilityModifier;

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

	private List<UINameplateItem.StatusDisplayInfo> m_statusEffectsAnimating;

	private List<UINameplateItem.StaticStatusDisplayInfo> m_statusEffects;

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

	private UINameplateItem.BarColor lastBarColor;

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
		this.m_sortOrder = -1;
		this.m_ShieldEased = new EasedFloat(0f);
		this.m_HPDamageEased = new EasedFloat(0f);
		this.m_HPGainedEased = new EasedFloat(0f);
		this.m_HPEased = new EasedFloat(0f);
		this.m_TPEasedGained = new EasedFloat(0f);
		this.m_TPEased = new EasedFloat(0f);
		this.m_statusEffectsAnimating = new List<UINameplateItem.StatusDisplayInfo>();
		this.m_statusEffects = new List<UINameplateItem.StaticStatusDisplayInfo>();
		this.m_targetingAbilityIndicators = new List<UITargetingAbilityIndicator>();
		this.m_visible = true;
		this.m_currentStatusStackCount = 0;
		UIEventTriggerUtils.AddListener(this.m_HPMouseOverHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.MouseOverHP));
		UIEventTriggerUtils.AddListener(this.m_HPMouseOverHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.MouseExitHP));
		UIManager.SetGameObjectActive(this.m_maxEnergyContainer, false, null);
		this.m_tickShaderHelpers = base.GetComponentsInChildren<UINameplateTickShaderHelper>(true);
		Mask componentInChildren = base.GetComponentInChildren<Mask>();
		if (componentInChildren != null)
		{
			UIManager.SetGameObjectActive(componentInChildren, false, null);
		}
	}

	public void SetSortOrder(int order)
	{
		this.m_sortOrder = order;
	}

	public int GetSortOrder()
	{
		return this.m_sortOrder;
	}

	public void MouseOverHP(BaseEventData data)
	{
		this.m_mouseIsOverHP = true;
	}

	public void MouseExitHP(BaseEventData data)
	{
		this.m_mouseIsOverHP = false;
	}

	public void SetTextVisible(bool visible)
	{
		this.m_textVisible = visible;
		UIManager.SetGameObjectActive(this.m_textNameLabel, visible, null);
		UIManager.SetGameObjectActive(this.m_healthLabel, visible, null);
	}

	private void Start()
	{
		UIManager.SetGameObjectActive(this.m_coverSymbol, false, null);
		for (int i = 0; i < this.abilityModifiers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.abilityModifiers[i].m_abilityModifierText, false, null);
		}
		UIManager.SetGameObjectActive(this.allyAbilityModifier.m_abilityModifierText, false, null);
		this.SetDebugText(string.Empty);
		for (int j = 0; j < this.nameplateCombatText.Length; j++)
		{
			this.nameplateCombatText[j].m_combatTextCover.color = new Color(1f, 1f, 1f, 0f);
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilityHighlightStart);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ClientResolutionStarted);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.NormalMovementStart);
		int num = 0;
		using (List<UIOverconData.NameToOverconEntry>.Enumerator enumerator = UIOverconData.Get().m_nameToOverconEntry.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIOverconData.NameToOverconEntry nameToOverconEntry = enumerator.Current;
				if (num < nameToOverconEntry.m_overconId)
				{
					num = nameToOverconEntry.m_overconId;
				}
			}
		}
		this.m_numOverconUsesById = new int[num + 1];
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsAbilityHighlightStart);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ClientResolutionStarted);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.NormalMovementStart);
		}
	}

	private void SetMaxHPWithShield(int newVal)
	{
		bool flag = this.m_maxHPWithShield != newVal;
		this.m_maxHPWithShield = newVal;
		if (flag)
		{
			float tickDivisions = (float)this.m_maxHPWithShield / HUD_UIResources.Get().m_AmtOfHealthPerTick;
			for (int i = 0; i < this.m_tickShaderHelpers.Length; i++)
			{
				UINameplateTickShaderHelper uinameplateTickShaderHelper = this.m_tickShaderHelpers[i];
				uinameplateTickShaderHelper.SetTickDivisions(tickDivisions);
			}
		}
	}

	private void SetTeamBars(UINameplateItem.TeamBars bars, bool visible)
	{
		UIManager.SetGameObjectActive(bars.m_teamMarker, visible, null);
		UIManager.SetGameObjectActive(bars.m_currentHPBar, visible, null);
		UIManager.SetGameObjectActive(bars.m_damageEasedBar, visible, null);
		this.NotifyFlagStatusChange(this.m_isHoldingFlag);
	}

	public static UINameplateItem.BarColor GetRelationshipWithPlayer(ActorData theActor)
	{
		if (theActor == null)
		{
			return UINameplateItem.BarColor.None;
		}
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			return UINameplateItem.BarColor.None;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			if (activeOwnedActorData == theActor)
			{
				return UINameplateItem.BarColor.Self;
			}
			if (activeOwnedActorData.GetTeam() == theActor.GetTeam())
			{
				return UINameplateItem.BarColor.Team;
			}
			return UINameplateItem.BarColor.Enemy;
		}
		else
		{
			if (theActor.GetTeam() == Team.TeamA)
			{
				return UINameplateItem.BarColor.Team;
			}
			return UINameplateItem.BarColor.Enemy;
		}
	}

	public void HandleAnimCallback(Animator animationController, UINameplateAnimCallback.AnimationCallback type)
	{
		if (animationController == null)
		{
			return;
		}
		if (type != UINameplateAnimCallback.AnimationCallback.COMBAT_TEXT_COLOR)
		{
		}
		else
		{
			for (int i = 0; i < this.nameplateCombatText.Length; i++)
			{
				if (animationController == this.nameplateCombatText[i].m_CombatTextController)
				{
					this.SetCombatTextColor(i);
				}
			}
		}
	}

	public void SetCombatTextColor(int index)
	{
		if (-1 < index && index < this.nameplateCombatText.Length)
		{
			this.nameplateCombatText[index].m_combatText.color = this.nameplateCombatText[index].m_colorToChangeCombatText;
		}
	}

	public void StartTargetingNumberFadeout()
	{
		this.m_fadeoutStartTime = Time.time;
		for (int i = 0; i < this.abilityModifiers.Length; i++)
		{
			Color color = this.abilityModifiers[i].m_abilityModifierText.color;
			color.a = 1f;
			this.abilityModifiers[i].m_abilityModifierText.color = color;
			if (this.abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeSelf)
			{
				this.abilityModifiers[i].m_targetingTextAnimationController.SetTrigger("DoOff");
				this.abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", false);
			}
		}
		Color color2 = this.allyAbilityModifier.m_abilityModifierText.color;
		color2.a = 1f;
		this.allyAbilityModifier.m_abilityModifierText.color = color2;
		if (this.allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeSelf)
		{
			this.allyAbilityModifier.m_targetingTextAnimationController.SetTrigger("DoOff");
			this.allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", false);
		}
	}

	public void ShowTargetingNumberForConfirmedTargeting()
	{
		this.m_fadeoutStartTime = Time.time;
		for (int i = 0; i < this.abilityModifiers.Length; i++)
		{
			Color color = this.abilityModifiers[i].m_abilityModifierText.color;
			color.a = 1f;
			this.abilityModifiers[i].m_abilityModifierText.color = color;
			if (this.abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeSelf)
			{
				this.abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", true);
			}
		}
		Color color2 = this.allyAbilityModifier.m_abilityModifierText.color;
		color2.a = 1f;
		this.allyAbilityModifier.m_abilityModifierText.color = color2;
		if (this.allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeSelf)
		{
			this.allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
		}
	}

	public void PlayCombatText(ActorData actorData, string text, CombatTextCategory category, BuffIconToDisplay icon)
	{
		bool flag = false;
		Color c = Color.black;
		UINameplateItem.BarColor relationshipWithPlayer = UINameplateItem.GetRelationshipWithPlayer(actorData);
		bool flag2 = false;
		bool flag3 = false;
		int num = 0;
		string text2 = "CombatTextHit";
		string text3 = "CombatTextHitMedium";
		bool flag4 = false;
		for (int i = 0; i < this.nameplateCombatText.Length; i++)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = this.nameplateCombatText[i].m_CombatTextController.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip.name != text2 && currentAnimatorClipInfo[0].clip.name != text3)
			{
				num = i;
				flag4 = true;
				IL_CE:
				if (flag4)
				{
					this.m_lastUsedCombatTextIndex = num;
				}
				else
				{
					num = (this.m_lastUsedCombatTextIndex + 1) % this.nameplateCombatText.Length;
					this.m_lastUsedCombatTextIndex = num;
				}
				float number = 0f;
				switch (category)
				{
				case CombatTextCategory.Damage:
				{
					string[] array = text.Split(new char[]
					{
						'|'
					});
					this.nameplateCombatText[num].m_combatText.text = array[0];
					number = (float)int.Parse(array[0]);
					if (array[1] == "C")
					{
						flag = true;
					}
					if (relationshipWithPlayer != UINameplateItem.BarColor.Self && relationshipWithPlayer != UINameplateItem.BarColor.Team)
					{
						if (relationshipWithPlayer == UINameplateItem.BarColor.Enemy)
						{
							this.nameplateCombatText[num].m_colorToChangeCombatText = new Color(1f, 0.8352941f, 0.8352941f);
							c = new Color(0.7607843f, 0f, 0f);
							flag3 = false;
						}
					}
					else
					{
						this.nameplateCombatText[num].m_colorToChangeCombatText = new Color(1f, 0.011764f, 0.011764f);
						c = new Color(0.356862754f, 0f, 0f);
						flag3 = true;
					}
					flag2 = true;
					break;
				}
				case CombatTextCategory.Healing:
					if (relationshipWithPlayer != UINameplateItem.BarColor.Self)
					{
						if (relationshipWithPlayer != UINameplateItem.BarColor.Team)
						{
							if (relationshipWithPlayer != UINameplateItem.BarColor.Enemy)
							{
								goto IL_2D8;
							}
							this.nameplateCombatText[num].m_colorToChangeCombatText = new Color(0.0392156877f, 0.498039216f, 0.0627451f);
							c = new Color(0f, 0.105882354f, 0.003921569f);
							flag3 = true;
							goto IL_2D8;
						}
					}
					this.nameplateCombatText[num].m_colorToChangeCombatText = new Color(0.7843137f, 1f, 0.796078444f);
					c = new Color(0f, 0.458823532f, 0.0117647061f);
					flag3 = false;
					IL_2D8:
					this.nameplateCombatText[num].m_combatText.text = text;
					number = (float)int.Parse(text);
					flag2 = true;
					break;
				case CombatTextCategory.Absorb:
					this.nameplateCombatText[num].m_combatText.text = text;
					c = Color.blue;
					flag2 = true;
					break;
				case CombatTextCategory.TP_Damage:
					this.nameplateCombatText[num].m_combatText.text = text;
					c = Color.magenta;
					flag2 = true;
					break;
				case CombatTextCategory.TP_Recovery:
					this.nameplateCombatText[num].m_combatText.text = text;
					c = Color.yellow * 0.7f;
					flag2 = true;
					break;
				}
				Component combatTextCover = this.nameplateCombatText[num].m_combatTextCover;
				bool doActive;
				if (this.m_showCombatTextIcons)
				{
					doActive = flag;
				}
				else
				{
					doActive = false;
				}
				UIManager.SetGameObjectActive(combatTextCover, doActive, null);
				if (this.nameplateCombatText[num].m_iconImage != null)
				{
					if (HUD_UIResources.Get() != null)
					{
						Sprite combatTextIconSprite = HUD_UIResources.Get().GetCombatTextIconSprite(icon);
						if (combatTextIconSprite != null && this.m_showCombatTextIcons)
						{
							this.nameplateCombatText[num].m_iconImage.sprite = combatTextIconSprite;
							UIManager.SetGameObjectActive(this.nameplateCombatText[num].m_iconImage, true, null);
						}
						else
						{
							this.nameplateCombatText[num].m_iconImage.sprite = null;
							UIManager.SetGameObjectActive(this.nameplateCombatText[num].m_iconImage, false, null);
						}
					}
				}
				bool flag5 = false;
				if (flag2)
				{
					if (this.nameplateCombatText[num].m_CombatTextController != null)
					{
						if (flag3)
						{
							this.nameplateCombatText[num].m_CombatTextController.Play(text3, 0, 0f);
						}
						else
						{
							this.nameplateCombatText[num].m_CombatTextController.Play(text2, 0, 0f);
						}
						this.nameplateCombatText[num].m_combatText.color = Color.white;
						this.nameplateCombatText[num].m_combatText.outlineColor = c;
						flag5 = true;
					}
				}
				if (flag5)
				{
					this.MoveToTopOfNameplates();
				}
				float scaledCombatTextSize = HUD_UIResources.GetScaledCombatTextSize(number);
				this.nameplateCombatText[num].m_TextScalar.localScale = new Vector3(scaledCombatTextSize, scaledCombatTextSize, scaledCombatTextSize);
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
					{
						this.HighlightNameplateForAbility();
						this.m_setToDimTime = Time.time + 1f;
					}
				}
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_CE;
		}
	}

	private void HideCombatTexts()
	{
		for (int i = 0; i < this.nameplateCombatText.Length; i++)
		{
			this.nameplateCombatText[i].m_combatText.text = string.Empty;
			if (this.nameplateCombatText[i].m_iconImage != null)
			{
				UIManager.SetGameObjectActive(this.nameplateCombatText[i].m_iconImage, false, null);
			}
			if (this.nameplateCombatText[i].m_combatTextCover != null)
			{
				UIManager.SetGameObjectActive(this.nameplateCombatText[i].m_combatTextCover, false, null);
			}
		}
	}

	public void StatusFadeOutDone(StatusType newType)
	{
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(newType);
		if (iconForStatusType.displayIcon)
		{
			UINameplateStatus uinameplateStatus = UnityEngine.Object.Instantiate<UINameplateStatus>(this.m_statusPrefab);
			uinameplateStatus.transform.SetParent(this.m_statusContainer.transform);
			uinameplateStatus.transform.localScale = Vector3.one;
			uinameplateStatus.transform.localEulerAngles = Vector3.zero;
			(uinameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2((float)this.m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt, (float)this.m_currentStatusStackCount * (uinameplateStatus.transform as RectTransform).rect.height);
			Vector3 localPosition = uinameplateStatus.transform.localPosition;
			localPosition.z = 0f;
			uinameplateStatus.transform.localPosition = localPosition;
			uinameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
			uinameplateStatus.m_StatusText.text = "-" + iconForStatusType.popupText;
			float nameplateStatusFadeColorMultiplier = HUD_UIResources.Get().m_nameplateStatusFadeColorMultiplier;
			uinameplateStatus.m_StatusText.color = uinameplateStatus.m_StatusText.color * nameplateStatusFadeColorMultiplier;
			Color color = uinameplateStatus.m_StatusText.color;
			color.a = 1f;
			uinameplateStatus.m_StatusText.color = color;
			uinameplateStatus.m_StatusIcon.color = uinameplateStatus.m_StatusIcon.color * nameplateStatusFadeColorMultiplier;
			color = uinameplateStatus.m_StatusIcon.color;
			color.a = 1f;
			uinameplateStatus.m_StatusIcon.color = color;
			if (iconForStatusType.isDebuff)
			{
				uinameplateStatus.m_StatusText.font = this.m_debuffFont;
			}
			else
			{
				uinameplateStatus.m_StatusText.font = this.m_buffFont;
			}
			uinameplateStatus.DisplayAsLostStatus(this);
			UINameplateItem.StatusDisplayInfo item;
			item.statusObject = uinameplateStatus;
			item.statusType = newType;
			this.m_statusEffectsAnimating.Add(item);
			this.m_currentStatusStackCount++;
		}
	}

	public void UpdateBriefcaseThreshold(float percent)
	{
		this.m_blueFillMarker.fillAmount = percent;
		this.m_redFillMarker.fillAmount = percent;
	}

	public void NotifyFlagStatusChange(bool holdingFlag)
	{
		if (this.m_isHoldingFlag != holdingFlag)
		{
			this.m_isHoldingFlag = holdingFlag;
			if (this.m_isHoldingFlag)
			{
				UIManager.SetGameObjectActive(this.m_redBriefcaseContainer, this.barsToShow == UINameplateItem.BarColor.Enemy, null);
				UIManager.SetGameObjectActive(this.m_blueBriefcaseContainer, this.barsToShow == UINameplateItem.BarColor.Self || this.barsToShow == UINameplateItem.BarColor.Team, null);
			}
			else
			{
				if (this.m_redBriefcaseContainer.gameObject.activeInHierarchy)
				{
					this.m_redBriefcaseContainer.Play("BriefcaseUIDefaultOUT");
				}
				if (this.m_blueBriefcaseContainer.gameObject.activeInHierarchy)
				{
					this.m_blueBriefcaseContainer.Play("BriefcaseUIDefaultOUT");
				}
			}
		}
	}

	public void AddStatus(StatusType newType)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(newType);
		if (iconForStatusType.displayIcon)
		{
			UINameplateStatus uinameplateStatus = UnityEngine.Object.Instantiate<UINameplateStatus>(this.m_statusPrefab);
			uinameplateStatus.transform.SetParent(this.m_statusContainer.transform);
			uinameplateStatus.transform.localScale = Vector3.one;
			uinameplateStatus.transform.localEulerAngles = Vector3.zero;
			(uinameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2((float)this.m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt, (float)this.m_currentStatusStackCount * (uinameplateStatus.transform as RectTransform).rect.height);
			Vector3 localPosition = uinameplateStatus.transform.localPosition;
			localPosition.z = 0f;
			uinameplateStatus.transform.localPosition = localPosition;
			uinameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
			uinameplateStatus.m_StatusText.text = iconForStatusType.popupText;
			if (iconForStatusType.isDebuff)
			{
				uinameplateStatus.m_StatusText.font = this.m_debuffFont;
			}
			else
			{
				uinameplateStatus.m_StatusText.font = this.m_buffFont;
			}
			if (iconForStatusType.isDebuff)
			{
				uinameplateStatus.DisplayAsNegativeStatus(this);
			}
			else
			{
				uinameplateStatus.DisplayAsPositiveStatus(this);
			}
			UINameplateItem.StatusDisplayInfo item;
			item.statusObject = uinameplateStatus;
			item.statusType = newType;
			this.m_statusEffectsAnimating.Add(item);
			this.m_currentStatusStackCount++;
		}
	}

	public void RemoveStatus(StatusType newType)
	{
		for (int i = 0; i < this.m_statusEffects.Count; i++)
		{
			UINameplateItem.StaticStatusDisplayInfo value = this.m_statusEffects[i];
			if (value.statusType == newType)
			{
				value.m_removedBuff = true;
				this.m_statusEffects[i] = value;
			}
		}
	}

	public void UpdateStatusDuration(StatusType status, int newDuration)
	{
		for (int i = 0; i < this.m_statusEffects.Count; i++)
		{
			UINameplateItem.StaticStatusDisplayInfo staticStatusDisplayInfo = this.m_statusEffects[i];
			if (staticStatusDisplayInfo.statusType == status)
			{
				staticStatusDisplayInfo.statusObject.Setup(status, newDuration);
				break;
			}
		}
	}

	public void SetDebugText(string text)
	{
		this.m_debugText.text = text;
	}

	public void UpdateTargetingAbilityIndicator(Ability ability, AbilityData.ActionType action, int index)
	{
		if (index < 6)
		{
			while (this.m_targetingAbilityIndicators.Count <= index)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_targetingAbilityIndicatorPrefab);
				UITargetingAbilityIndicator component = gameObject.GetComponent<UITargetingAbilityIndicator>();
				component.transform.SetParent(this.m_targetingAbilityIconsGrid.transform);
				component.transform.localScale = Vector3.one;
				component.transform.localPosition = Vector3.zero;
				component.transform.localEulerAngles = Vector3.zero;
				this.m_targetingAbilityIndicators.Add(component);
			}
			this.m_targetingAbilityIndicators[index].Setup(this.m_actorData, ability, action);
			if (!this.m_targetingAbilityIndicators[index].gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_targetingAbilityIndicators[index], true, null);
			}
		}
		if (this.m_catalsystPips == null)
		{
			this.m_catalsystPips = UnityEngine.Object.Instantiate<UITargetingAbilityCatalystPipContainer>(this.m_catalystIndicatorPrefab);
			this.m_catalsystPips.transform.SetParent(this.m_targetingAbilityIconsGrid.transform);
			this.m_catalsystPips.transform.localScale = Vector3.one;
			this.m_catalsystPips.transform.localPosition = Vector3.zero;
			this.m_catalsystPips.transform.localEulerAngles = Vector3.zero;
		}
		UIUtils.SetAsLastSiblingIfNeeded(this.m_catalsystPips.transform);
		this.m_actorData.GetAbilityData().UpdateCatalystDisplay();
	}

	public void TurnOffTargetingAbilityIndicator(int fromIndex)
	{
		for (int i = fromIndex; i < this.m_targetingAbilityIndicators.Count; i++)
		{
			if (this.m_targetingAbilityIndicators[i].gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_targetingAbilityIndicators[i], false, null);
			}
		}
		if (fromIndex == 0)
		{
			this.SetCatalystsVisible(false);
		}
	}

	public void SpawnOvercon(UIOverconData.NameToOverconEntry entry, bool skipValidation)
	{
		if (this.m_overconParent != null && this.m_overconPrefab != null)
		{
			GameWideData gameWideData = GameWideData.Get();
			if (this.m_overcon != null)
			{
				UnityEngine.Object.Destroy(this.m_overcon.gameObject);
				this.m_overcon = null;
			}
			if (GameFlowData.Get().CurrentTurn > this.m_lastTurnOverconUsed)
			{
				this.m_numOverconUsesThisTurn = 0;
				this.m_lastTurnOverconUsed = GameFlowData.Get().CurrentTurn;
			}
			bool flag = true;
			string term = null;
			int num = 0;
			if (skipValidation)
			{
				flag = false;
			}
			else if (this.m_numOverconUsesThisTurn >= gameWideData.NumOverconsPerTurn)
			{
				if (this.m_actorData.isLocalPlayer)
				{
					term = "OverconLimitPerTurn";
					num = gameWideData.NumOverconsPerTurn;
				}
			}
			else if (this.m_numOverconUsesThisMatch >= gameWideData.NumOverconsPerMatch)
			{
				if (this.m_actorData.isLocalPlayer)
				{
					term = "OverconLimitPerMatch";
					num = gameWideData.NumOverconsPerMatch;
				}
			}
			else if (this.m_numOverconUsesById[entry.m_overconId] >= entry.m_maxUsesPerMatch)
			{
				if (this.m_actorData.isLocalPlayer)
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
				if (this.m_actorData.isLocalPlayer)
				{
					TextConsole.Message message = default(TextConsole.Message);
					message.MessageType = ConsoleMessageType.SystemMessage;
					message.Text = string.Format(StringUtil.TR(term, "HUDScene"), num);
					TextConsole.AllowedEmojis allowedEmojis;
					allowedEmojis.emojis = new List<int>();
					HUD_UI.Get().m_textConsole.HandleMessage(message, allowedEmojis);
				}
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_overconPrefab);
			this.m_overcon = gameObject.GetComponent<UINameplateOvercon>();
			if (this.m_overcon != null)
			{
				if (!skipValidation)
				{
					this.m_numOverconUsesThisMatch++;
					this.m_numOverconUsesThisTurn++;
					this.m_numOverconUsesById[entry.m_overconId]++;
				}
				this.m_overcon.Initialize(this.m_actorData, entry);
				this.m_overcon.transform.SetParent(this.m_overconParent.transform);
				this.m_overcon.transform.localPosition = Vector3.zero;
				this.m_overcon.transform.localScale = Vector3.one;
				if (string.IsNullOrEmpty(entry.m_audioEvent))
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.OverconUsed);
				}
				else
				{
					AudioManager.PostEvent(entry.m_audioEvent, null);
				}
			}
		}
	}

	public void UpdateSelfNameplate(Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		UIManager.SetGameObjectActive(this.m_coverSymbol, false, null);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		HashSet<TechPointInteractionType> hashSet = new HashSet<TechPointInteractionType>();
		TechPointInteraction[] baseTechPointInteractions = abilityTargeting.GetBaseTechPointInteractions();
		for (int i = 0; i < baseTechPointInteractions.Length; i++)
		{
			int num8 = baseTechPointInteractions[i].m_amount;
			TechPointInteractionType type = baseTechPointInteractions[i].m_type;
			if (!hashSet.Contains(type))
			{
				hashSet.Add(type);
				if (abilityTargeting.CurrentAbilityMod != null)
				{
					num8 = abilityTargeting.CurrentAbilityMod.GetModdedTechPointForInteraction(type, num8);
				}
				if (type == TechPointInteractionType.RewardOnCast)
				{
					num += num8;
				}
				else if (type == TechPointInteractionType.RewardOnDamage_OncePerCast)
				{
					num2 += num8;
				}
				else if (type == TechPointInteractionType.RewardOnHit_OncePerCast)
				{
					num3 += num8;
				}
				else if (type == TechPointInteractionType.RewardOnDamage_PerTarget)
				{
					num4 += num8;
				}
				else if (type == TechPointInteractionType.RewardOnHit_PerTarget)
				{
					num5 += num8;
				}
				else if (type == TechPointInteractionType.RewardOnHit_PerAllyTarget)
				{
					num6 += num8;
				}
				else if (type == TechPointInteractionType.RewardOnHit_PerEnemyTarget)
				{
					num7 += num8;
				}
			}
		}
		if (abilityTargeting.CurrentAbilityMod != null)
		{
			foreach (TechPointInteractionMod techPointInteractionMod in abilityTargeting.CurrentAbilityMod.m_techPointInteractionMods)
			{
				if (!hashSet.Contains(techPointInteractionMod.interactionType))
				{
					hashSet.Add(techPointInteractionMod.interactionType);
					int num9 = 0;
					TechPointInteractionType interactionType = techPointInteractionMod.interactionType;
					hashSet.Add(interactionType);
					if (abilityTargeting.CurrentAbilityMod != null)
					{
						num9 = Mathf.Max(0, abilityTargeting.CurrentAbilityMod.GetModdedTechPointForInteraction(interactionType, 0));
					}
					if (interactionType == TechPointInteractionType.RewardOnCast)
					{
						num += num9;
					}
					else if (interactionType == TechPointInteractionType.RewardOnDamage_OncePerCast)
					{
						num2 += num9;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_OncePerCast)
					{
						num3 += num9;
					}
					else if (interactionType == TechPointInteractionType.RewardOnDamage_PerTarget)
					{
						num4 += num9;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_PerTarget)
					{
						num5 += num9;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_PerAllyTarget)
					{
						num6 += num9;
					}
					else if (interactionType == TechPointInteractionType.RewardOnHit_PerEnemyTarget)
					{
						num7 += num9;
					}
				}
			}
		}
		int num10 = AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num);
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		int num11 = 0;
		int num12 = 0;
		bool flag = false;
		int targetCounts = abilityTargeting.GetTargetCounts(activeOwnedActorData, currentTargeterIndex, out num11, out num12, out flag);
		if (num2 > 0)
		{
			if (num12 > 0)
			{
				num10 += AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num2);
			}
		}
		if (num4 > 0)
		{
			num10 += AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num4) * num12;
		}
		if (num3 > 0)
		{
			if (targetCounts > 0)
			{
				num10 += AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num3);
			}
		}
		if (num5 > 0)
		{
			num10 += AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num5) * targetCounts;
		}
		if (num6 > 0)
		{
			num10 += AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num6) * num11;
		}
		if (num7 > 0)
		{
			num10 += AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num7) * num12;
		}
		int num13 = abilityTargeting.GetAdditionalTechPointGainForNameplateItem(this.m_actorData, currentTargeterIndex);
		bool flag2 = abilityTargeting.StatusAdjustAdditionalTechPointForTargeting();
		if (flag2)
		{
			num13 = AbilityUtils.CalculateTechPointsForTargeter(this.m_actorData, abilityTargeting, num13);
		}
		bool flag3;
		if (inConfirm)
		{
			flag3 = (Time.time - this.m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay);
		}
		else
		{
			flag3 = false;
		}
		bool fadeOut = flag3;
		bool flag4 = false;
		if (baseTechPointInteractions.Length <= 0)
		{
			if (abilityTargeting.CurrentAbilityMod != null)
			{
				if (abilityTargeting.CurrentAbilityMod.m_techPointInteractionMods.Length > 0)
				{
					goto IL_471;
				}
			}
			if (num13 <= 0)
			{
				UIManager.SetGameObjectActive(this.abilityModifiers[0].m_abilityModifierText, false, null);
				goto IL_586;
			}
		}
		IL_471:
		if (inConfirm && num10 + num13 == 0)
		{
			UIManager.SetGameObjectActive(this.abilityModifiers[0].m_abilityModifierText, false, null);
		}
		else if (this.m_textVisible)
		{
			UIManager.SetGameObjectActive(this.abilityModifiers[0].m_abilityModifierText, true, null);
			this.abilityModifiers[0].m_targetingTextAnimationController.SetBool("IsOn", true);
			flag4 = true;
			int num14 = num10 + num13;
			this.abilityModifiers[0].m_abilityModifierText.text = num14.ToString();
			Color targetingNumberColor = this.GetTargetingNumberColor(this.m_EnergyColor, this.abilityModifiers[0].m_abilityModifierText.color.a, fadeOut);
			this.abilityModifiers[0].m_abilityModifierText.color = targetingNumberColor;
			this.SetGlowTargeting(true, UINameplateItem.BarColor.Self);
		}
		IL_586:
		bool flag5 = false;
		int startModifierIndex;
		if (flag4)
		{
			startModifierIndex = 1;
		}
		else
		{
			startModifierIndex = 0;
		}
		this.SetAbilityModifiersForTargetActor(startModifierIndex, abilityTargeting, currentTargeterIndex, false, inConfirm, out flag5);
	}

	public void UpdateNameplateTargeted(ActorData targetingActor, Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		int num = 0;
		bool flag = false;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (actorData != null)
			{
				if (actorData != targetingActor)
				{
					if (actorData.GetTeam() == targetingActor.GetTeam())
					{
						ActorTargeting actorTargeting = actorData.GetActorTargeting();
						if (actorTargeting != null)
						{
							if (targetingActor.GetTeam() != this.m_actorData.GetTeam())
							{
								actorTargeting.IsTargetingActor(this.m_actorData, AbilityTooltipSymbol.Damage, ref num);
							}
							else
							{
								actorTargeting.IsTargetingActor(this.m_actorData, AbilityTooltipSymbol.Healing, ref num);
								flag = actorTargeting.IsTargetingActor(this.m_actorData, AbilityTooltipSymbol.Absorb, ref num);
							}
						}
					}
				}
			}
		}
		if (num > 0)
		{
			if (HighlightUtils.Get().m_enableAccumulatedAllyNumbers)
			{
				if (this.m_textVisible)
				{
					UIManager.SetGameObjectActive(this.allyAbilityModifier.m_abilityModifierText, true, null);
					this.allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
					this.allyAbilityModifier.m_abilityModifierText.text = num.ToString() + " +";
					bool flag2;
					if (inConfirm)
					{
						flag2 = (Time.time - this.m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay);
					}
					else
					{
						flag2 = false;
					}
					bool fadeOut = flag2;
					Color color;
					if (targetingActor.GetTeam() != this.m_actorData.GetTeam())
					{
						color = this.m_DamageColor;
					}
					else if (flag)
					{
						color = this.m_AbsorbColor;
					}
					else
					{
						color = this.m_HealingColor;
					}
					Color color2 = color;
					color2 = this.GetTargetingNumberColor(color2, this.allyAbilityModifier.m_abilityModifierText.color.a, fadeOut);
					this.allyAbilityModifier.m_abilityModifierText.color = color2;
				}
				goto IL_259;
			}
		}
		UIManager.SetGameObjectActive(this.allyAbilityModifier.m_abilityModifierText, false, null);
		IL_259:
		bool flag3 = false;
		int num2 = this.SetAbilityModifiersForTargetActor(0, abilityTargeting, currentTargeterIndex, inCover, inConfirm, out flag3);
		for (int j = num2; j < this.abilityModifiers.Length; j++)
		{
			UIManager.SetGameObjectActive(this.abilityModifiers[j].m_abilityModifierText, false, null);
		}
		Component coverSymbol = this.m_coverSymbol;
		bool doActive;
		if (inCover)
		{
			doActive = flag3;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(coverSymbol, doActive, null);
		this.SetGlowTargeting(true, UINameplateItem.GetRelationshipWithPlayer(this.m_actorData));
	}

	public void Update()
	{
		if (GameFlowData.Get() != null)
		{
			if (this.m_actorData == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (this.m_visible)
			{
				if (GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().activeOwnedActorData.GetAbilityData() != null)
				{
					if (GameFlowData.Get().activeOwnedActorData.GetAbilityData().GetSelectedAbility() == null)
					{
						this.SetStatusObjectInteractable(true);
					}
					else
					{
						this.SetStatusObjectInteractable(false);
					}
				}
			}
			else
			{
				this.SetStatusObjectInteractable(false);
			}
		}
	}

	private void SetStatusObjectInteractable(bool interactable)
	{
		for (int i = 0; i < this.m_statusEffects.Count; i++)
		{
			UINameplateItem.SetInteractable(this.m_statusEffects[i].statusObject.m_cGroup, interactable);
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
		if (this.m_catalsystPips != null)
		{
			UIManager.SetGameObjectActive(this.m_catalsystPips, visible, null);
		}
	}

	public void UpdateCatalysts(List<Ability> cardAbilities)
	{
		if (this.m_catalsystPips == null)
		{
			return;
		}
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		for (int i = 0; i < cardAbilities.Count; i++)
		{
			Ability ability = cardAbilities[i];
			if (ability != null)
			{
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
		}
		UIManager.SetGameObjectActive(this.m_catalsystPips.m_PrepPhaseOn, doActive, null);
		UIManager.SetGameObjectActive(this.m_catalsystPips.m_DashPhaseOn, doActive2, null);
		UIManager.SetGameObjectActive(this.m_catalsystPips.m_BlastPhaseOn, doActive3, null);
		UIUtils.SetAsLastSiblingIfNeeded(this.m_catalsystPips.transform);
	}

	public void UpdateNameplateUntargeted(bool doInstantHide = false)
	{
		UIManager.SetGameObjectActive(this.m_coverSymbol, false, null);
		for (int i = 0; i < this.abilityModifiers.Length; i++)
		{
			if (doInstantHide)
			{
				if (this.abilityModifiers[i].m_abilityModifierText.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(this.abilityModifiers[i].m_abilityModifierText, false, null);
				}
			}
			else if (this.abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeInHierarchy)
			{
				this.abilityModifiers[i].m_targetingTextAnimationController.SetTrigger("DoOff");
				this.abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", false);
			}
		}
		if (doInstantHide)
		{
			if (this.allyAbilityModifier.m_abilityModifierText.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.allyAbilityModifier.m_abilityModifierText, false, null);
			}
		}
		else if (this.allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeInHierarchy)
		{
			this.allyAbilityModifier.m_targetingTextAnimationController.SetTrigger("DoOff");
			this.allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", false);
		}
		this.SetGlowTargeting(false, UINameplateItem.BarColor.None);
	}

	private void SetGlowTargeting(bool setGlow, UINameplateItem.BarColor teamColor)
	{
		UIManager.SetGameObjectActive(this.m_targetGlow, false, null);
	}

	private unsafe int SetAbilityModifiersForTargetActor(int startModifierIndex, Ability abilityTargeting, int currentTargeterIndex, bool inCover, bool inConfirm, out bool hasDamage)
	{
		hasDamage = false;
		ActorData actorData;
		if (GameFlowData.Get() != null)
		{
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData actorData2 = actorData;
		bool flag;
		if (inConfirm)
		{
			flag = (Time.time - this.m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay);
		}
		else
		{
			flag = false;
		}
		bool fadeOut = flag;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		this.m_tempTargetingNumSymbolToValueMap.Clear();
		Dictionary<AbilityTooltipSymbol, int> tempTargetingNumSymbolToValueMap = this.m_tempTargetingNumSymbolToValueMap;
		ActorTargeting.GetNameplateNumbersForTargeter(actorData2, this.m_actorData, abilityTargeting, currentTargeterIndex, tempTargetingNumSymbolToValueMap);
		int num = startModifierIndex;
		using (Dictionary<AbilityTooltipSymbol, int>.Enumerator enumerator = tempTargetingNumSymbolToValueMap.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AbilityTooltipSymbol, int> keyValuePair = enumerator.Current;
				if (num < this.abilityModifiers.Length)
				{
					if (!this.m_textVisible)
					{
					}
					else
					{
						AbilityTooltipSymbol key = keyValuePair.Key;
						int value = keyValuePair.Value;
						UINameplateItem.AbilityModifier abilityModifier = this.abilityModifiers[num];
						switch (key)
						{
						case AbilityTooltipSymbol.Damage:
							if (value > 0 && !flag2)
							{
								UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true, null);
								abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
								int baseValue = value;
								string text = baseValue.ToString();
								string accessoryTargeterNumberString = abilityTargeting.GetAccessoryTargeterNumberString(this.m_actorData, key, baseValue);
								if (!string.IsNullOrEmpty(accessoryTargeterNumberString))
								{
									text += accessoryTargeterNumberString;
								}
								abilityModifier.m_abilityModifierText.text = text;
								Color targetingNumberColor = this.GetTargetingNumberColor(this.m_DamageColor, abilityModifier.m_abilityModifierText.color.a, fadeOut);
								abilityModifier.m_abilityModifierText.color = targetingNumberColor;
								flag2 = true;
								hasDamage = true;
								num++;
							}
							break;
						case AbilityTooltipSymbol.Healing:
							if (value > 0)
							{
								if (!flag3)
								{
									UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true, null);
									abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
									int num2 = value;
									abilityModifier.m_abilityModifierText.text = num2.ToString();
									Color targetingNumberColor2 = this.GetTargetingNumberColor(this.m_HealingColor, abilityModifier.m_abilityModifierText.color.a, fadeOut);
									abilityModifier.m_abilityModifierText.color = targetingNumberColor2;
									flag3 = true;
									num++;
								}
							}
							break;
						case AbilityTooltipSymbol.Absorb:
							if (!flag5)
							{
								UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true, null);
								abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
								abilityModifier.m_abilityModifierText.text = value.ToString();
								Color targetingNumberColor3 = this.GetTargetingNumberColor(this.m_AbsorbColor, abilityModifier.m_abilityModifierText.color.a, fadeOut);
								abilityModifier.m_abilityModifierText.color = targetingNumberColor3;
								flag5 = true;
								num++;
							}
							break;
						case AbilityTooltipSymbol.Energy:
							if (actorData2 != this.m_actorData && !flag4)
							{
								UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true, null);
								abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
								int num3 = value;
								abilityModifier.m_abilityModifierText.text = num3.ToString();
								Color targetingNumberColor4 = this.GetTargetingNumberColor(this.m_EnergyColor, abilityModifier.m_abilityModifierText.color.a, fadeOut);
								abilityModifier.m_abilityModifierText.color = targetingNumberColor4;
								flag4 = true;
								num++;
							}
							break;
						default:
							UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, false, null);
							break;
						}
					}
				}
			}
		}
		for (int i = num; i < this.abilityModifiers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.abilityModifiers[i].m_abilityModifierText, false, null);
		}
		return num;
	}

	private Color GetTargetingNumberColor(Color fullColor, float alphaNow, bool fadeOut)
	{
		if (fadeOut)
		{
			Color result = fullColor;
			result.a = Mathf.Max(0f, alphaNow - Time.deltaTime * HUD_UIResources.Get().m_confirmedTargetingFadeoutSpeed);
			return result;
		}
		return fullColor;
	}

	public void SetBarColors(UINameplateItem.BarColor newBarcolor)
	{
		if (this.barsToShow == newBarcolor)
		{
			return;
		}
		this.barsToShow = newBarcolor;
		this.lastBarColor = newBarcolor;
		switch (this.barsToShow)
		{
		case UINameplateItem.BarColor.Self:
			this.SetTeamBars(this.m_selfBars, true);
			this.SetTeamBars(this.m_teamBars, false);
			this.SetTeamBars(this.m_enemyBars, false);
			break;
		case UINameplateItem.BarColor.Team:
			this.SetTeamBars(this.m_selfBars, false);
			this.SetTeamBars(this.m_teamBars, true);
			this.SetTeamBars(this.m_enemyBars, false);
			break;
		case UINameplateItem.BarColor.Enemy:
			this.SetTeamBars(this.m_selfBars, false);
			this.SetTeamBars(this.m_teamBars, false);
			this.SetTeamBars(this.m_enemyBars, true);
			break;
		default:
			this.SetTeamBars(this.m_selfBars, false);
			this.SetTeamBars(this.m_teamBars, false);
			this.SetTeamBars(this.m_enemyBars, false);
			break;
		}
	}

	private bool IsInFrontOfCamera()
	{
		bool result = false;
		if (this.m_actorData != null)
		{
			Vector3 nameplatePosition = this.m_actorData.GetNameplatePosition(30f);
			if (Camera.main.WorldToViewportPoint(nameplatePosition).z < 0f)
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
		if (this.m_actorData != null)
		{
			result = (this.m_actorData.ShouldShowNameplate() && this.IsInFrontOfCamera());
		}
		return result;
	}

	public void ForceFinishStatusAnims()
	{
		UINameplateStatus[] componentsInChildren = this.m_statusContainer.GetComponentsInChildren<UINameplateStatus>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].AnimDone();
		}
	}

	private void SetVisible(bool visible)
	{
		if (!this.m_visible && visible)
		{
			this.ForceFinishStatusAnims();
		}
		if (this.m_visible)
		{
			if (!visible)
			{
				this.HideCombatTexts();
			}
		}
		this.m_visible = visible;
		if (visible)
		{
			if (this.m_alphaToUse > 0f)
			{
				this.SetAlpha(this.m_alphaToUse);
			}
			else
			{
				this.SetAlpha(1f);
			}
		}
		else
		{
			this.SetAlpha(0f);
		}
		if (!this.m_visible)
		{
			this.m_mouseIsOverHP = false;
		}
		using (List<UITargetingAbilityIndicator>.Enumerator enumerator = this.m_targetingAbilityIndicators.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UITargetingAbilityIndicator uitargetingAbilityIndicator = enumerator.Current;
				uitargetingAbilityIndicator.SetCanvasGroupVisibility(this.m_visible);
			}
		}
	}

	private void SetAlpha(float alpha)
	{
		this.m_alphaToUse = alpha;
		if (this.m_visible)
		{
			this.m_canvasGroup.alpha = this.m_alphaToUse;
			this.m_abilityPreviewCanvasGroup.alpha = this.m_alphaToUse;
		}
		else
		{
			this.m_canvasGroup.alpha = 0f;
			this.m_abilityPreviewCanvasGroup.alpha = 0f;
		}
		UINameplateItem.SetInteractable(this.m_abilityPreviewCanvasGroup, this.m_abilityPreviewCanvasGroup.alpha > 0f);
	}

	public void NotifyStatusAnimationDone(UINameplateStatus nameplateStatus, bool gainedStatus)
	{
		for (int i = 0; i < this.m_statusEffectsAnimating.Count; i++)
		{
			UINameplateItem.StatusDisplayInfo item = this.m_statusEffectsAnimating[i];
			if (item.statusObject == nameplateStatus)
			{
				if (gainedStatus)
				{
					UIBuffIndicator uibuffIndicator = UnityEngine.Object.Instantiate<UIBuffIndicator>(this.m_buffIndicatorPrefab);
					uibuffIndicator.transform.SetParent(this.m_buffIndicatorGrid.transform);
					uibuffIndicator.transform.localScale = Vector3.one;
					uibuffIndicator.transform.localPosition = Vector3.zero;
					uibuffIndicator.transform.localEulerAngles = Vector3.zero;
					uibuffIndicator.Setup(item.statusType, this.m_actorData.GetActorStatus().GetDurationOfStatus(item.statusType));
					CanvasGroup component = uibuffIndicator.gameObject.GetComponent<CanvasGroup>();
					component.alpha = 0f;
					UINameplateItem.StaticStatusDisplayInfo item2;
					item2.m_removedBuff = false;
					item2.statusObject = uibuffIndicator;
					item2.statusType = item.statusType;
					HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(item.statusType);
					bool flag = false;
					for (int j = 0; j < this.m_statusEffects.Count; j++)
					{
						if (!this.m_statusEffects[j].m_removedBuff)
						{
							if (this.m_statusEffects[j].statusType == item2.statusType)
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
							for (int k = 0; k < this.m_statusEffects.Count; k++)
							{
								if (HUD_UIResources.GetIconForStatusType(this.m_statusEffects[k].statusType).isDebuff)
								{
									index = k;
									break;
								}
							}
							this.m_statusEffects.Insert(index, item2);
							for (int l = 0; l < this.m_statusEffects.Count; l++)
							{
								this.m_statusEffects[l].statusObject.gameObject.transform.SetAsLastSibling();
							}
						}
						else
						{
							this.m_statusEffects.Add(item2);
						}
					}
				}
				this.m_statusEffectsAnimating.Remove(item);
				UnityEngine.Object.Destroy(nameplateStatus.gameObject);
				i--;
				if (!this.m_actorData.GetActorStatus().HasStatus(item.statusType, true))
				{
					this.RemoveStatus(item.statusType);
				}
			}
		}
	}

	private void SetBarPercentVisual(Image image, float percent)
	{
		this.m_selfBars.m_currentHPPercent = this.m_HPEased;
		if (image.fillAmount != percent)
		{
			image.fillAmount = percent;
		}
	}

	private void LateUpdate()
	{
		if (Camera.main == null)
		{
			return;
		}
		if (this.m_setToDimTime > 0f)
		{
			if (Time.time >= this.m_setToDimTime)
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
					{
						this.DimNameplateForAbility();
						goto IL_98;
					}
				}
				this.m_setToDimTime = -1f;
			}
		}
		IL_98:
		bool flag = this.IsVisibleInDecisionPhase();
		if (this.m_visible != flag)
		{
			this.SetVisible(flag);
		}
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				if (this.m_actorData != null)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (activeOwnedActorData != null)
					{
						if (activeOwnedActorData == this.m_actorData)
						{
							this.SetBarColors(UINameplateItem.BarColor.Self);
						}
						else if (activeOwnedActorData.GetTeam() == this.m_actorData.GetTeam())
						{
							this.SetBarColors(UINameplateItem.BarColor.Team);
						}
						else if (activeOwnedActorData.GetTeam() == this.m_actorData.GetTeam().OtherTeam())
						{
							this.SetBarColors(UINameplateItem.BarColor.Enemy);
						}
						else
						{
							this.SetBarColors(UINameplateItem.BarColor.None);
						}
					}
					else if (this.m_actorData.GetTeam() == Team.TeamA)
					{
						this.SetBarColors(UINameplateItem.BarColor.Team);
					}
					else
					{
						this.SetBarColors(UINameplateItem.BarColor.Enemy);
					}
				}
			}
		}
		if (this.lastBarColor != this.barsToShow)
		{
			this.SetBarColors(this.barsToShow);
		}
		if (this.m_actorData != null)
		{
			if (this.myCanvas == null)
			{
				this.myCanvas = HUD_UI.Get().GetTopLevelCanvas();
			}
			if (this.myCanvas != null && this.CanvasRect == null)
			{
				this.CanvasRect = (this.myCanvas.transform as RectTransform);
			}
			Vector3 vector;
			if (this.m_actorData.ShouldShowNameplate())
			{
				vector = this.m_actorData.GetNameplatePosition(-2f);
			}
			else if (this.m_actorData.IsDead())
			{
				vector = this.m_actorData.LastDeathPosition;
			}
			else if (!this.m_actorData.IsVisibleToClient())
			{
				vector = this.m_actorData.GetClientLastKnownPos();
			}
			else
			{
				vector = this.m_actorData.GetNameplatePosition(-2f);
			}
			Vector2 vector2 = Camera.main.WorldToViewportPoint(vector);
			Vector2 anchoredPosition = new Vector2(vector2.x * this.CanvasRect.sizeDelta.x, vector2.y * this.CanvasRect.sizeDelta.y);
			(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
			Vector3 position = Camera.main.transform.position;
			this.m_distanceFromCamera = (vector - position).sqrMagnitude;
			this.OnHitUpdate();
			int hitPointsAfterResolution = this.m_actorData.GetHitPointsAfterResolution();
			int clientUnappliedHoTTotal_ToDisplay_zq = this.m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
			if (hitPointsAfterResolution == 0)
			{
				if (!this.m_zeroHealthIcon.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(this.m_zeroHealthIcon, true, null);
				}
				if (this.m_healthLabel.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(this.m_healthLabel, false, null);
				}
			}
			else
			{
				if (this.m_zeroHealthIcon.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(this.m_zeroHealthIcon, false, null);
				}
				if (this.m_healthLabel.gameObject.activeSelf != this.m_textVisible)
				{
					UIManager.SetGameObjectActive(this.m_healthLabel, this.m_textVisible, null);
				}
			}
			if (!object.ReferenceEquals(this.m_textNameLabel.text, this.m_actorData.GetFancyDisplayName()))
			{
				this.m_textNameLabel.text = this.m_actorData.GetFancyDisplayName();
			}
			int num = this.m_actorData.symbol_0004();
			if (num > 0)
			{
				this.m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = (this.m_actorData.GetAbilityData().GetSelectedAbility() == null);
				if (this.m_mouseIsOverHP)
				{
					if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
					{
						this.m_healthLabel.text = string.Format("{0} + <color={1}>{2}</color> + <color={3}>{4}</color>", new object[]
						{
							hitPointsAfterResolution.ToString(),
							HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor),
							num.ToString(),
							HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextHotColor),
							clientUnappliedHoTTotal_ToDisplay_zq.ToString()
						});
					}
					else
					{
						this.m_healthLabel.text = string.Format("{0} + <color={1}>{2}</color>", hitPointsAfterResolution.ToString(), HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor), num.ToString());
					}
				}
				else
				{
					this.m_healthLabel.text = string.Format("<color={0}>{1}</color>", HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor), (hitPointsAfterResolution + num).ToString());
				}
			}
			else if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
			{
				this.m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = (this.m_actorData.GetAbilityData().GetSelectedAbility() == null);
				if (this.m_mouseIsOverHP)
				{
					this.m_healthLabel.text = string.Format("{0} + <color={1}>{2}</color>", hitPointsAfterResolution.ToString(), HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextHotColor), clientUnappliedHoTTotal_ToDisplay_zq.ToString());
				}
				else
				{
					this.m_healthLabel.color = Color.white;
					string text = hitPointsAfterResolution.ToString();
					if (!this.m_healthLabel.text.Equals(text))
					{
						this.m_healthLabel.text = text;
					}
				}
			}
			else
			{
				this.m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = false;
				this.m_healthLabel.color = Color.white;
				string text2 = hitPointsAfterResolution.ToString();
				if (!this.m_healthLabel.text.Equals(text2))
				{
					this.m_healthLabel.text = text2;
				}
			}
			int energyToDisplay = this.m_actorData.GetEnergyToDisplay();
			int actualMaxTechPoints = this.m_actorData.GetActualMaxTechPoints();
			if (energyToDisplay != this.m_previousTP || actualMaxTechPoints != this.m_previousTPMax)
			{
				float endValue = 0f;
				if (actualMaxTechPoints != 0)
				{
					endValue = (float)energyToDisplay / (float)actualMaxTechPoints;
				}
				if (energyToDisplay > this.m_previousTP)
				{
					this.m_TPEased.EaseTo(endValue, 0f);
					this.m_TPEasedGained.EaseTo(endValue, 2.5f);
				}
				else
				{
					this.m_TPEasedGained.EaseTo(endValue, 0f);
					this.m_TPEased.EaseTo(endValue, 2.5f);
				}
				List<Ability> abilitiesAsList = this.m_actorData.GetAbilityData().GetAbilitiesAsList();
				bool flag2 = false;
				if (abilitiesAsList.Count > 4 && abilitiesAsList[4] != null)
				{
					bool flag3;
					if (energyToDisplay >= abilitiesAsList[4].GetModdedCost() && !this.m_actorData.IsDead())
					{
						flag3 = (this.m_actorData.GetAbilityData().GetCooldownRemaining(AbilityData.ActionType.ABILITY_4) <= 0);
					}
					else
					{
						flag3 = false;
					}
					flag2 = flag3;
				}
				bool flag4;
				if (energyToDisplay != actualMaxTechPoints)
				{
					flag4 = flag2;
				}
				else
				{
					flag4 = true;
				}
				bool flag5 = flag4;
				Graphic tpGainBar = this.m_tpGainBar;
				Color color;
				if (flag5)
				{
					color = this.m_FullEnergyColor;
				}
				else
				{
					color = this.m_NonFullEnergyColor;
				}
				tpGainBar.color = color;
				if (flag5 != this.m_isMaxEnergy)
				{
					this.m_maxEnergyAnimator.GetComponent<_DisableGameObjectOnAnimationDoneEvent>().enabled = !flag5;
					if (flag5)
					{
						UIManager.SetGameObjectActive(this.m_maxEnergyContainer, true, null);
						if (GameFlowData.Get() != null)
						{
							if (GameFlowData.Get().activeOwnedActorData == this.m_actorData)
							{
								UIFrontEnd.PlaySound(FrontEndButtonSounds.MaxEnergyReached);
							}
						}
					}
					Animator maxEnergyAnimator = this.m_maxEnergyAnimator;
					string str = "NameplateMaxEnergyDefault";
					string str2;
					if (flag5)
					{
						str2 = "IN";
					}
					else
					{
						str2 = "OUT";
					}
					maxEnergyAnimator.Play(str + str2);
					this.m_isMaxEnergy = flag5;
				}
			}
			if (this.m_TPEased != this.m_tpGainedEasePercent)
			{
				this.m_tpGainedEasePercent = this.m_TPEased;
				this.SetBarPercentVisual(this.m_tpGainEaseBar, this.ModifyTPBarPercentForArt(this.m_tpGainedEasePercent));
			}
			if (this.m_TPEasedGained != this.m_tpGainedPercent)
			{
				this.m_tpGainedPercent = this.m_TPEasedGained;
				this.SetBarPercentVisual(this.m_tpGainBar, this.ModifyTPBarPercentForArt(this.m_tpGainedPercent));
			}
			if (this.m_shieldBar != null)
			{
				if (this.m_ShieldEased.EndValue() <= this.m_HPEased.EndValue())
				{
					this.SetBarPercentVisual(this.m_shieldBar, 0f);
				}
				else
				{
					this.SetBarPercentVisual(this.m_shieldBar, this.ModifyHPBarPercentForArt(this.m_shieldPercent));
					this.m_shieldPercent = this.m_ShieldEased;
				}
			}
			if (this.m_hpGainBar != null)
			{
				this.SetBarPercentVisual(this.m_hpGainBar, this.ModifyHPBarPercentForArt(this.m_hpGainedPercent));
				this.m_hpGainedPercent = this.m_HPGainedEased;
				if (this.m_mouseIsOverHP)
				{
					Color color2 = this.m_hpGainBar.color;
					if (this.m_hpGainedIsFading)
					{
						color2.a -= 0.05f;
						if (color2.a <= 0f)
						{
							color2.a = 0f;
							this.m_hpGainedIsFading = !this.m_hpGainedIsFading;
						}
					}
					else
					{
						color2.a += 0.05f;
						if (color2.a >= 1f)
						{
							color2.a = 1f;
							this.m_hpGainedIsFading = !this.m_hpGainedIsFading;
						}
					}
					this.m_hpGainBar.color = color2;
				}
				else
				{
					this.m_hpGainBar.color = Color.white;
				}
			}
			UINameplateItem.BarColor barColor = this.barsToShow;
			if (barColor != UINameplateItem.BarColor.Self)
			{
				if (barColor != UINameplateItem.BarColor.Team)
				{
					if (barColor != UINameplateItem.BarColor.Enemy)
					{
					}
					else
					{
						if (this.m_enemyBars.m_currentHPBar != null)
						{
							this.SetBarPercentVisual(this.m_enemyBars.m_currentHPBar, this.ModifyHPBarPercentForArt(this.m_HPEased));
							this.m_enemyBars.m_currentHPPercent = this.m_HPEased;
						}
						if (this.m_enemyBars.m_damageEasedBar != null)
						{
							this.SetBarPercentVisual(this.m_enemyBars.m_damageEasedBar, this.ModifyHPBarPercentForArt(this.m_HPDamageEased));
							this.m_enemyBars.m_damageEasePercent = this.m_HPDamageEased;
						}
					}
				}
				else
				{
					if (this.m_teamBars.m_currentHPBar != null)
					{
						this.SetBarPercentVisual(this.m_teamBars.m_currentHPBar, this.ModifyHPBarPercentForArt(this.m_HPEased));
						this.m_teamBars.m_currentHPPercent = this.m_HPEased;
					}
					if (this.m_teamBars.m_damageEasedBar != null)
					{
						this.SetBarPercentVisual(this.m_teamBars.m_damageEasedBar, this.ModifyHPBarPercentForArt(this.m_HPDamageEased));
						this.m_teamBars.m_damageEasePercent = this.m_HPDamageEased;
					}
				}
			}
			else
			{
				if (this.m_selfBars.m_currentHPBar != null)
				{
					this.SetBarPercentVisual(this.m_selfBars.m_currentHPBar, this.ModifyHPBarPercentForArt(this.m_HPEased));
					this.m_selfBars.m_currentHPPercent = this.m_HPEased;
				}
				if (this.m_selfBars.m_damageEasedBar != null)
				{
					this.SetBarPercentVisual(this.m_selfBars.m_damageEasedBar, this.ModifyHPBarPercentForArt(this.m_HPDamageEased));
					this.m_selfBars.m_damageEasePercent = this.m_HPDamageEased;
				}
			}
			if (this.m_statusEffectsAnimating.Count == 0)
			{
				this.m_currentStatusStackCount = 0;
			}
			for (int i = 0; i < this.m_statusEffects.Count; i++)
			{
				CanvasGroup component = this.m_statusEffects[i].statusObject.gameObject.GetComponent<CanvasGroup>();
				if (component != null)
				{
					if (this.m_visible)
					{
						if (!this.m_statusEffects[i].m_removedBuff)
						{
							component.alpha += Time.deltaTime * HUD_UIResources.Get().m_nameplateStaticStatusFadeSpeed;
						}
						else
						{
							component.alpha -= Time.deltaTime * HUD_UIResources.Get().m_nameplateStaticStatusFadeSpeed;
						}
						if (component.alpha <= 0f)
						{
							this.StatusFadeOutDone(this.m_statusEffects[i].statusType);
							UnityEngine.Object.Destroy(this.m_statusEffects[i].statusObject.gameObject);
							this.m_statusEffects.RemoveAt(i);
							i--;
						}
					}
					else
					{
						component.alpha = 0f;
						if (this.m_statusEffects[i].m_removedBuff)
						{
							this.StatusFadeOutDone(this.m_statusEffects[i].statusType);
							UnityEngine.Object.Destroy(this.m_statusEffects[i].statusObject.gameObject);
							this.m_statusEffects.RemoveAt(i);
							i--;
						}
					}
				}
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
		this.m_previousHP = this.m_actorData.GetHitPointsAfterResolution();
		this.m_previousHPMax = this.m_actorData.GetMaxHitPoints();
		this.m_previousShieldValue = this.m_actorData.symbol_0004();
		this.m_previousHPShieldAndHot = this.m_previousHP + this.m_previousShieldValue + this.m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
		this.SetMaxHPWithShield(this.m_previousHPMax + this.m_previousShieldValue);
		int num = this.m_previousHPMax + this.m_previousShieldValue;
		float duration = 0f;
		float num2;
		if (num == 0)
		{
			num2 = 0f;
		}
		else
		{
			num2 = (float)this.m_previousHP / (float)num;
		}
		float endValue = num2;
		this.m_HPDamageEased.EaseTo(endValue, duration);
		this.m_HPGainedEased.EaseTo((float)this.m_previousHPShieldAndHot / (float)num, duration);
		this.m_HPEased.EaseTo(endValue, duration);
		if (this.m_previousShieldValue > 0)
		{
			this.m_ShieldEased.EaseTo(((float)this.m_previousHP + (float)this.m_previousShieldValue) / (float)num, duration);
		}
		else
		{
			this.m_ShieldEased.EaseTo(0f, duration);
		}
	}

	public void OnHitUpdate()
	{
		if (this.m_actorData == null)
		{
			return;
		}
		int num = this.m_actorData.symbol_0004();
		int hitPointsAfterResolution = this.m_actorData.GetHitPointsAfterResolution();
		int hitPoints = this.m_actorData.HitPoints;
		int clientUnappliedHoTTotal_ToDisplay_zq = this.m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
		int num2 = hitPointsAfterResolution + num + clientUnappliedHoTTotal_ToDisplay_zq;
		float num3 = (float)hitPointsAfterResolution / (float)this.m_maxHPWithShield;
		float num4 = this.m_HPEased.EndValue();
		float endValue = ((float)hitPointsAfterResolution + (float)num) / (float)this.m_maxHPWithShield;
		float endValue2 = (float)num2 / (float)this.m_maxHPWithShield;
		if (hitPointsAfterResolution < this.m_previousHP)
		{
			this.m_lostHealth = true;
			this.m_ShieldEased.EaseTo(endValue, 0f);
			float num5 = Mathf.Abs(num3 - this.m_HPEased.EndValue());
			this.m_HPEased.EaseTo(this.m_HPEased.EndValue() - num5, 0f);
			num5 = Mathf.Abs(num3 - this.m_HPDamageEased.EndValue());
			this.m_HPDamageEased.EaseTo(this.m_HPDamageEased.EndValue() - num5, 2.5f);
			this.m_previousHP = hitPointsAfterResolution;
		}
		else if (this.m_previousHP < hitPointsAfterResolution)
		{
			this.m_lostHealth = false;
			float num6 = Mathf.Abs(num3 - num4);
			if (this.m_previousShieldValue != 0)
			{
				this.m_ShieldEased.EaseTo(endValue, 2.5f);
			}
			this.m_HPEased.EaseTo(this.m_HPEased.EndValue() + num6, 2.5f);
			this.m_HPDamageEased.EaseTo(this.m_HPDamageEased.EndValue() + num6, 2.5f);
			this.m_previousHP = hitPointsAfterResolution;
		}
		if (this.m_previousShieldValue < num)
		{
			this.SetMaxHPWithShield(this.m_actorData.GetMaxHitPoints() + num);
			float endValue3 = ((float)hitPointsAfterResolution + (float)num) / (float)this.m_maxHPWithShield;
			float endValue4 = (float)hitPointsAfterResolution / (float)this.m_maxHPWithShield;
			this.m_ShieldEased.EaseTo(endValue3, 0f);
			this.m_HPEased.EaseTo(endValue4, 0f);
			this.m_HPDamageEased.EaseTo(endValue3, 1f);
			this.m_previousShieldValue = num;
		}
		else if (num < this.m_previousShieldValue)
		{
			if (!this.m_lostHealth)
			{
				this.SetMaxHPWithShield(this.m_actorData.GetMaxHitPoints() + num);
				float endValue5 = (float)hitPointsAfterResolution / (float)this.m_maxHPWithShield;
				float endValue6 = ((float)hitPointsAfterResolution + (float)num) / (float)this.m_maxHPWithShield;
				this.m_ShieldEased.EaseTo(endValue6, 0f);
				this.m_HPEased.EaseTo(endValue5, 0f);
				this.m_HPDamageEased.EaseTo(endValue6, 0f);
				this.m_previousShieldValue = num;
			}
		}
		if (num2 != this.m_previousHPShieldAndHot)
		{
			this.m_HPGainedEased.EaseTo(endValue2, 0f);
			this.m_previousHPShieldAndHot = num2;
		}
		if (this.m_previousResolvedHP != hitPoints)
		{
			this.m_previousResolvedHP = hitPoints;
			this.OnResolvedHitPoints();
		}
	}

	public void MoveToTopOfNameplates()
	{
		UIUtils.SetAsLastSiblingIfNeeded(base.transform);
	}

	public void OnResolvedHitPoints()
	{
		if (this.m_actorData == null)
		{
			return;
		}
		this.m_lostHealth = false;
		int num = this.m_actorData.symbol_0004();
		int hitPointsAfterResolution = this.m_actorData.GetHitPointsAfterResolution();
		this.SetMaxHPWithShield(this.m_actorData.GetMaxHitPoints() + num);
		float endValue = (float)hitPointsAfterResolution / (float)this.m_maxHPWithShield;
		float num2 = ((float)hitPointsAfterResolution + (float)num) / (float)this.m_maxHPWithShield;
		float num3 = (float)this.m_actorData.GetClientUnappliedHoTTotal_ToDisplay_zq() / (float)this.m_maxHPWithShield;
		this.m_ShieldEased.EaseTo(num2, 2.5f);
		this.m_HPEased.EaseTo(endValue, 2.5f);
		this.m_HPGainedEased.EaseTo(num2 + num3, 2.5f);
		this.m_HPDamageEased.EaseTo(num2, 2.5f);
	}

	public void Setup(ActorData actorData)
	{
		this.m_actorData = actorData;
		ActorData actorData2 = this.m_actorData;
		actorData2.m_onResolvedHitPoints = (ActorData.ActorDataDelegate)Delegate.Combine(actorData2.m_onResolvedHitPoints, new ActorData.ActorDataDelegate(this.OnResolvedHitPoints));
		this.m_textNameLabel.text = this.m_actorData.GetFancyDisplayName();
		this.SnapBarValues();
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (this.m_actorData != null)
		{
			if (eventType != GameEventManager.EventType.TheatricsAbilityHighlightStart)
			{
				if (eventType != GameEventManager.EventType.TheatricsAbilitiesEnd)
				{
					switch (eventType)
					{
					case GameEventManager.EventType.ClientResolutionStarted:
						this.DimNameplateForAbility();
						break;
					default:
						if (eventType != GameEventManager.EventType.TurnTick)
						{
						}
						else
						{
							this.m_setToDimTime = -1f;
							this.SetAlpha(1f);
							base.transform.localScale = Vector3.one;
						}
						break;
					case GameEventManager.EventType.NormalMovementStart:
					{
						GameEventManager.NormalMovementStartAgs normalMovementStartAgs = (GameEventManager.NormalMovementStartAgs)args;
						if (normalMovementStartAgs.m_actorsBeingHitMidMovement.Contains(this.m_actorData))
						{
							this.HighlightNameplateForAbility();
						}
						else
						{
							this.DimNameplateForAbility();
						}
						break;
					}
					}
				}
			}
			else
			{
				GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs = (GameEventManager.TheatricsAbilityHighlightStartArgs)args;
				if (!theatricsAbilityHighlightStartArgs.m_casters.Contains(this.m_actorData))
				{
					if (!theatricsAbilityHighlightStartArgs.m_targets.Contains(this.m_actorData))
					{
						this.DimNameplateForAbility();
						goto IL_FF;
					}
				}
				this.HighlightNameplateForAbility();
				IL_FF:;
			}
		}
	}

	private void HighlightNameplateForAbility()
	{
		this.SetAlpha(1f);
		base.transform.localScale = this.m_scaleWhenHighlighted * Vector3.one;
		this.m_setToDimTime = -1f;
	}

	private void DimNameplateForAbility()
	{
		this.SetAlpha(this.m_alphaWhenOthersHighlighted);
		base.transform.localScale = this.m_scaleWhenOthersHighlighted * Vector3.one;
		this.m_setToDimTime = -1f;
	}

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
}
