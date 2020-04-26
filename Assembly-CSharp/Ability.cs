using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ability : MonoBehaviour
{
	public enum TargetingParadigm
	{
		Position = 1,
		Direction,
		BoardSquare
	}

	public enum ValidateCheckPath
	{
		Ignore,
		CanBuildPath,
		CanBuildPathAllowThroughInvalid
	}

	public enum RotationVisibilityMode
	{
		Default_NonFreeAction,
		OnAllyClientOnly,
		Always,
		Never
	}

	public enum MovementAdjustment
	{
		FullMovement,
		ReducedMovement,
		NoMovement
	}

	public const string c_abilityPreviewVideoPath = "Video/AbilityPreviews/";

	[Tooltip("Description of ability, to display in game", order = 0)]
	[TextArea(1, 20, order = 1)]
	public string m_toolTip;

	[HideInInspector]
	public string m_debugUnlocalizedTooltip = string.Empty;

	[HideInInspector]
	public List<StatusType> m_savedStatusTypesForTooltips;

	[TextArea(1, 20, order = 1)]
	public string m_shortToolTip;

	private string m_toolTipForUI;

	public string m_rewardString = string.Empty;

	[Tooltip("Prefab of sequences used to display primary shot fx")]
	public GameObject m_sequencePrefab;

	private int m_overrideActorDataIndex = ActorData.s_invalidActorIndex;

	private ActorData m_actorData;

	private bool m_searchedForActorData;

	private List<AbilityTooltipNumber> m_abilityTooltipNumbers;

	private List<AbilityTooltipNumber> m_nameplateTargetingNumbers;

	private bool m_lastUpdateShowingAffectedSquares;

	[Space(10f)]
	public string m_abilityName = "Base Ability";

	public string m_flavorText = string.Empty;

	public bool m_ultimate;

	public string m_previewVideo;

	[Header("-- for Sequence prefab naming prefix, optional")]
	public string m_expectedSequencePrefixForEditor = string.Empty;

	[Separator("Cooldown / Stock (if Max Stock > 0, Cooldown is ignored)", "orange")]
	public int m_cooldown;

	public int m_maxStocks;

	public int m_stockRefreshDuration;

	public bool m_refillAllStockOnRefresh;

	[Tooltip("-1 => have Max Stocks on start")]
	public int m_initialStockAmount = -1;

	public int m_stockConsumedOnCast = 1;

	[Tooltip("Ability will manage stock count, will not change based on refresh time")]
	public bool m_abilityManagedStockCount;

	[Separator("Run Phase", "orange")]
	public AbilityPriority m_runPriority = AbilityPriority.Combat_Damage;

	public bool m_freeAction;

	[Separator("Energy Interactions", "orange")]
	public int m_techPointsCost;

	public TechPointInteraction[] m_techPointInteractions;

	[Separator("Animation Index", "orange")]
	public ActorModelData.ActionAnimationType m_actionAnimType = ActorModelData.ActionAnimationType.Ability1;

	[Header("-- Rotation Visibility --")]
	public RotationVisibilityMode m_rotationVisibilityMode;

	[Separator("Movement", "orange")]
	public MovementAdjustment m_movementAdjustment = MovementAdjustment.ReducedMovement;

	[Tooltip("Speed of charge or evasion movement. Ignored if Movement Duration is greater than zero.")]
	public float m_movementSpeed = 8f;

	[Tooltip("Duration, in seconds, of charge or evasion movement. Ignored if less than or equal to zero.")]
	public float m_movementDuration;

	[Space(5f)]
	public float m_cameraBoundsMinHeight = 2.5f;

	[Separator("Target Data for Targeters / Tags", "orange")]
	public TargetData[] m_targetData;

	[Space(5f)]
	public List<AbilityTags> m_tags = new List<AbilityTags>();

	[Header("-- Status when Requested (will not work if it's a chained ability) --")]
	public List<StatusType> m_statusWhenRequested = new List<StatusType>();

	[Separator("Chain Abilities", "orange")]
	public Ability[] m_chainAbilities;

	[Separator("Targeter Template Visual Swaps", "orange")]
	public List<TargeterTemplateSwapData> m_targeterTemplateSwaps;

	private List<AbilityUtil_Targeter> m_targeters = new List<AbilityUtil_Targeter>();

	private List<GameObject> m_backupTargeterHighlights = new List<GameObject>();

	private List<AbilityTarget> m_backupTargets = new List<AbilityTarget>();

	private AbilityMod m_currentAbilityMod;

	protected const string c_forDesignHeader = "<color=cyan>-- For Design --</color>\n";

	protected const string c_forArtHeader = "<color=cyan>-- For Art --</color>\n";

	public Sprite sprite
	{
		get;
		set;
	}

	protected ActorData ActorData
	{
		get
		{
			if (m_actorData == null && !m_searchedForActorData)
			{
				m_actorData = ((m_overrideActorDataIndex != ActorData.s_invalidActorIndex) ? GameFlowData.Get().FindActorByActorIndex(m_overrideActorDataIndex) : GetComponent<ActorData>());
				m_searchedForActorData = true;
			}
			return m_actorData;
		}
	}

	public AbilityPriority RunPriority
	{
		get
		{
			return GetRunPriority();
		}
		private set
		{
			m_runPriority = value;
		}
	}

	public List<AbilityUtil_Targeter> Targeters => m_targeters;

	public AbilityUtil_Targeter Targeter
	{
		get
		{
			if (m_targeters.Count > 0)
			{
				return m_targeters[0];
			}
			return null;
		}
		set
		{
			if (m_targeters.Count == 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_targeters.Add(value);
						return;
					}
				}
			}
			m_targeters[0] = value;
		}
	}

	public List<GameObject> BackupTargeterHighlights
	{
		get
		{
			return m_backupTargeterHighlights;
		}
		set
		{
			m_backupTargeterHighlights = value;
		}
	}

	public List<AbilityTarget> BackupTargets
	{
		get
		{
			return m_backupTargets;
		}
		set
		{
			m_backupTargets = value;
		}
	}

	public AbilityMod CurrentAbilityMod => m_currentAbilityMod;

	private void Awake()
	{
		InitializeAbility();
		SanitizeChainAbilities();
	}

	private void OnDestroy()
	{
		ClearTargeters();
	}

	public void InitializeAbility()
	{
		RebuildTooltipForUI();
		ResetNameplateTargetingNumbers();
	}

	protected void ClearTargeters()
	{
		if (Targeters == null)
		{
			return;
		}
		while (true)
		{
			ResetAbilityTargeters();
			Targeters.Clear();
			return;
		}
	}

	public void OverrideActorDataIndex(int actorDataIndex)
	{
		m_overrideActorDataIndex = actorDataIndex;
		m_actorData = null;
	}

	public string GetToolTipString(bool shortTooltip = false)
	{
		if (shortTooltip)
		{
			return m_shortToolTip;
		}
		List<TooltipTokenEntry> tooltipTokenEntries = GetTooltipTokenEntries();
		string text = StringUtil.TR_AbilityFinalFullTooltip(GetType().ToString(), m_abilityName);
		if (text.Length == 0 && m_toolTip.Length > 0)
		{
			text = m_toolTip;
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(text, tooltipTokenEntries);
	}

	public virtual string GetFullTooltip()
	{
		return GetToolTipString();
	}

	public virtual string GetUnlocalizedFullTooltip()
	{
		if (string.IsNullOrEmpty(m_debugUnlocalizedTooltip))
		{
			List<TooltipTokenEntry> tooltipTokenEntries = GetTooltipTokenEntries();
			return TooltipTokenEntry.GetTooltipWithSubstitutes(m_toolTip, tooltipTokenEntries);
		}
		return m_debugUnlocalizedTooltip;
	}

	protected virtual void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public List<TooltipTokenEntry> GetTooltipTokenEntries(AbilityMod mod = null)
	{
		List<TooltipTokenEntry> list = new List<TooltipTokenEntry>();
		AddSpecificTooltipTokens(list, mod);
		if (m_techPointInteractions != null)
		{
			if (m_techPointInteractions.Length > 0)
			{
				TechPointInteraction[] techPointInteractions = m_techPointInteractions;
				for (int i = 0; i < techPointInteractions.Length; i++)
				{
					TechPointInteraction techPointInteraction = techPointInteractions[i];
					int num = techPointInteraction.m_amount;
					if (mod != null)
					{
						num = mod.GetModdedTechPointForInteraction(techPointInteraction.m_type, num);
					}
					list.Add(new TooltipTokenInt(techPointInteraction.m_type.ToString(), "Energy Gain", num));
				}
			}
		}
		if (m_techPointsCost > 0)
		{
			list.Add(new TooltipTokenInt("EnergyCost", "Energy Cost", m_techPointsCost));
		}
		if (m_maxStocks > 0)
		{
			list.Add(new TooltipTokenInt("MaxStocks", "Max Stocks/Charges", m_maxStocks));
		}
		return list;
	}

	public virtual void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		List<TooltipTokenEntry> tooltipTokenEntries = GetTooltipTokenEntries(mod);
		m_debugUnlocalizedTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(m_toolTip, tooltipTokenEntries);
		m_savedStatusTypesForTooltips = TooltipTokenEntry.GetStatusTypesFromTooltip(m_toolTip);
	}

	protected void AddTokenInt(List<TooltipTokenEntry> tokens, string name, string desc, int val, bool addForNonPositive = false)
	{
		if (addForNonPositive || val > 0)
		{
			tokens.Add(new TooltipTokenInt(name, desc, val));
		}
	}

	protected void AddTokenFloat(List<TooltipTokenEntry> tokens, string name, string desc, float val, bool addForNonPositive = false)
	{
		if (!addForNonPositive)
		{
			if (!(val > 0f))
			{
				return;
			}
		}
		tokens.Add(new TooltipTokenFloat(name, desc, val));
	}

	protected void AddTokenFloatRounded(List<TooltipTokenEntry> tokens, string name, string desc, float val, bool addForNonPositive = false)
	{
		int num = Mathf.RoundToInt(val);
		if (addForNonPositive || num > 0)
		{
			tokens.Add(new TooltipTokenInt(name, desc, num));
		}
	}

	protected void AddTokenFloatAsPct(List<TooltipTokenEntry> tokens, string name, string desc, float val, bool addForNonPositive = false)
	{
		if (!addForNonPositive)
		{
			if (!(val > 0f))
			{
				return;
			}
		}
		int val2 = Mathf.RoundToInt(val * 100f);
		tokens.Add(new TooltipTokenPct(name, desc, val2));
	}

	public virtual AbilityPriority GetRunPriority()
	{
		if (m_currentAbilityMod != null)
		{
			if (m_currentAbilityMod.m_useRunPriorityOverride)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_currentAbilityMod.m_runPriorityOverride;
					}
				}
			}
		}
		return m_runPriority;
	}

	public virtual bool CanRunInPhase(AbilityPriority phase)
	{
		return phase == RunPriority;
	}

	public string GetRewardString()
	{
		return StringUtil.TR_AbilityReward(GetType().ToString(), m_abilityName);
	}

	public string GetPhaseStringUnLocalized()
	{
		UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(RunPriority);
		string result = uIPhaseFromAbilityPriority.ToString();
		switch (uIPhaseFromAbilityPriority)
		{
		case UIQueueListPanel.UIPhase.Combat:
			result = "Blast";
			break;
		case UIQueueListPanel.UIPhase.Evasion:
			result = "Dash";
			break;
		case UIQueueListPanel.UIPhase.Movement:
			result = "Movement";
			break;
		case UIQueueListPanel.UIPhase.Prep:
			result = "Prep";
			break;
		}
		return result;
	}

	public string GetPhaseString()
	{
		UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(RunPriority);
		string result = uIPhaseFromAbilityPriority.ToString();
		switch (uIPhaseFromAbilityPriority)
		{
		case UIQueueListPanel.UIPhase.Combat:
			result = StringUtil.TR("Blast", "Global");
			break;
		case UIQueueListPanel.UIPhase.Evasion:
			result = StringUtil.TR("Dash", "Global");
			break;
		case UIQueueListPanel.UIPhase.Movement:
			result = StringUtil.TR("Movement", "Global");
			break;
		case UIQueueListPanel.UIPhase.Prep:
			result = StringUtil.TR("Prep", "Global");
			break;
		}
		return result;
	}

	public string GetCostString()
	{
		if (GetBaseCost() > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return $"{GetBaseCost()} energy";
				}
			}
		}
		return string.Empty;
	}

	public int GetBaseCost()
	{
		int result;
		if (m_techPointsCost > 0)
		{
			result = m_techPointsCost;
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public virtual int GetModdedCost()
	{
		int num = GetBaseCost();
		if (m_currentAbilityMod != null)
		{
			num = Mathf.Max(0, m_currentAbilityMod.m_techPointCostMod.GetModifiedValue(num));
		}
		return num;
	}

	public virtual TechPointInteraction[] GetBaseTechPointInteractions()
	{
		return m_techPointInteractions;
	}

	public string GetNameString()
	{
		string text = StringUtil.TR_AbilityName(GetType().ToString(), m_abilityName);
		if (text.Length == 0 && m_abilityName.Length > 0)
		{
			text = m_abilityName;
		}
		return text;
	}

	public string GetCooldownString()
	{
		return $"{m_cooldown} turn cooldown";
	}

	public virtual bool UseCustomAbilityIconColor()
	{
		return false;
	}

	public virtual Color GetCustomAbilityIconColor(ActorData actor)
	{
		return Color.white;
	}

	public virtual bool ShouldShowPersistentAuraUI()
	{
		return false;
	}

	public virtual bool CanShowTargetableRadiusPreview()
	{
		TargetData[] targetData = GetTargetData();
		if (targetData.Length > 0)
		{
			TargetData targetData2 = targetData[0];
			float num = Mathf.Max(0f, targetData2.m_range - 0.5f);
			if (num > 0f)
			{
				if (num < 15f)
				{
					if (targetData2.m_targetingParadigm != TargetingParadigm.Direction)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public virtual float GetTargetableRadiusInSquares(ActorData caster)
	{
		return AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
	}

	public bool ShowTargetableRadiusWhileTargeting()
	{
		return CanShowTargetableRadiusPreview() && AbilityUtils.AbilityHasTag(this, AbilityTags.Targeter_ShowRangeWhileTargeting);
	}

	public virtual bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = 1f;
		max = 1f;
		return false;
	}

	public virtual bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = 0f;
		max = 360f;
		return false;
	}

	public virtual bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		overridePos = aimingActor.GetTravelBoardSquareWorldPosition();
		return false;
	}

	public virtual int GetBaseCooldown()
	{
		return m_cooldown;
	}

	public int GetModdedCooldown()
	{
		int num = GetBaseCooldown();
		if (num >= 0)
		{
			if (m_currentAbilityMod != null)
			{
				num = Mathf.Max(0, m_currentAbilityMod.m_maxCooldownMod.GetModifiedValue(num));
			}
		}
		return num;
	}

	public virtual int GetCooldownForUIDisplay()
	{
		return GetModdedCooldown();
	}

	public StandardEffectInfo GetModdedEffectForEnemies()
	{
		if (m_currentAbilityMod != null)
		{
			return m_currentAbilityMod.m_effectToTargetEnemyOnHit;
		}
		return null;
	}

	public StandardEffectInfo GetModdedEffectForAllies()
	{
		if (m_currentAbilityMod != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_currentAbilityMod.m_effectToTargetAllyOnHit;
				}
			}
		}
		return null;
	}

	public StandardEffectInfo GetModdedEffectForSelf()
	{
		if (m_currentAbilityMod != null)
		{
			return m_currentAbilityMod.m_effectToSelfOnCast;
		}
		return null;
	}

	public bool HasSelfEffectFromBaseMod()
	{
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		int result;
		if (moddedEffectForSelf != null)
		{
			result = (moddedEffectForSelf.m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool GetModdedUseAllyEffectForTargetedCaster()
	{
		if (m_currentAbilityMod != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_currentAbilityMod.m_useAllyEffectForTargetedCaster;
				}
			}
		}
		return false;
	}

	public float GetModdedChanceToTriggerEffects()
	{
		if (m_currentAbilityMod != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_currentAbilityMod.m_effectTriggerChance;
				}
			}
		}
		return 1f;
	}

	public bool ModdedChanceToTriggerEffectsIsPerHit()
	{
		if (m_currentAbilityMod != null)
		{
			return m_currentAbilityMod.m_effectTriggerChanceMultipliedPerHit;
		}
		return false;
	}

	public int GetBaseMaxStocks()
	{
		return m_maxStocks;
	}

	public int GetModdedMaxStocks()
	{
		int num = m_maxStocks;
		if (m_maxStocks >= 0 && m_currentAbilityMod != null)
		{
			num = Mathf.Max(0, m_currentAbilityMod.m_maxStocksMod.GetModifiedValue(num));
		}
		return num;
	}

	public bool RefillAllStockOnRefresh()
	{
		bool result;
		if ((bool)m_currentAbilityMod)
		{
			result = m_currentAbilityMod.m_refillAllStockOnRefreshMod.GetModifiedValue(m_refillAllStockOnRefresh);
		}
		else
		{
			result = m_refillAllStockOnRefresh;
		}
		return result;
	}

	public int GetBaseStockRefreshDuration()
	{
		return m_stockRefreshDuration;
	}

	public int GetModdedStockRefreshDuration()
	{
		int num = m_stockRefreshDuration;
		if (m_currentAbilityMod != null)
		{
			num = Mathf.Max(-1, m_currentAbilityMod.m_stockRefreshDurationMod.GetModifiedValue(num));
		}
		return num;
	}

	private void RebuildTooltipForUI()
	{
		m_toolTipForUI = GetNameString();
		if (GetBaseCost() > 0)
		{
			m_toolTipForUI += $" - {GetBaseCost()} TP";
		}
		if (m_cooldown > 0)
		{
			m_toolTipForUI += $" - {m_cooldown} turn cooldown";
		}
		if (IsFreeAction())
		{
			m_toolTipForUI = "This is a Free Action.\n" + m_toolTipForUI;
		}
		m_toolTipForUI = m_toolTipForUI + "\n" + GetFullTooltip();
		m_abilityTooltipNumbers = BaseCalculateAbilityTooltipNumbers();
	}

	public void SetTooltip()
	{
		RebuildTooltipForUI();
	}

	public List<AbilityTooltipNumber> GetAbilityTooltipNumbers()
	{
		return m_abilityTooltipNumbers;
	}

	protected List<AbilityTooltipNumber> BaseCalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = CalculateAbilityTooltipNumbers();
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	protected void AppendTooltipNumbersFromBaseModEffects(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemyEffectSubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allyEffectSubject = AbilityTooltipSubject.Ally, AbilityTooltipSubject selfEffectSubject = AbilityTooltipSubject.Self)
	{
		if (!(m_currentAbilityMod != null))
		{
			return;
		}
		if (enemyEffectSubject != 0)
		{
			StandardEffectInfo moddedEffectForEnemies = GetModdedEffectForEnemies();
			if (moddedEffectForEnemies != null)
			{
				moddedEffectForEnemies.ReportAbilityTooltipNumbers(ref numbers, enemyEffectSubject);
			}
		}
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (allyEffectSubject != 0)
		{
			if (moddedEffectForAllies != null)
			{
				moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, allyEffectSubject);
			}
		}
		if (selfEffectSubject == AbilityTooltipSubject.None)
		{
			return;
		}
		while (true)
		{
			if (m_currentAbilityMod.m_useAllyEffectForTargetedCaster && moddedEffectForAllies != null)
			{
				moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, selfEffectSubject);
			}
			if (moddedEffectForSelf != null)
			{
				while (true)
				{
					moddedEffectForSelf.ReportAbilityTooltipNumbers(ref numbers, selfEffectSubject);
					return;
				}
			}
			return;
		}
	}

	protected virtual List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public virtual List<int> _001D()
	{
		List<int> list = new List<int>();
		List<AbilityTooltipNumber> list2 = BaseCalculateAbilityTooltipNumbers();
		if (list2 != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					{
						foreach (AbilityTooltipNumber item in list2)
						{
							list.Add(item.m_value);
						}
						return list;
					}
				}
			}
		}
		return list;
	}

	public virtual List<StatusType> GetStatusTypesForTooltip()
	{
		if (m_savedStatusTypesForTooltips != null)
		{
			if (m_savedStatusTypesForTooltips.Count != 0)
			{
				return m_savedStatusTypesForTooltips;
			}
		}
		return TooltipTokenEntry.GetStatusTypesFromTooltip(m_toolTip);
	}

	public List<AbilityTooltipNumber> GetNameplateTargetingNumbers()
	{
		return m_nameplateTargetingNumbers;
	}

	protected virtual List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return BaseCalculateAbilityTooltipNumbers();
	}

	public void ResetNameplateTargetingNumbers()
	{
		m_nameplateTargetingNumbers = CalculateNameplateTargetingNumbers();
	}

	protected void ResetTooltipAndTargetingNumbers()
	{
		SetTooltip();
		ResetNameplateTargetingNumbers();
	}

	public virtual bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		return false;
	}

	public virtual bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		return false;
	}

	public virtual Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		return null;
	}

	public virtual string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return null;
	}

	public virtual int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return 0;
	}

	public virtual bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return true;
	}

	public static void AddNameplateValueForSingleHit(ref Dictionary<AbilityTooltipSymbol, int> symbolToValue, AbilityUtil_Targeter targeter, ActorData targetActor, int damageAmount, AbilityTooltipSymbol symbolOfInterest = AbilityTooltipSymbol.Damage, AbilityTooltipSubject targetSubject = AbilityTooltipSubject.Primary)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		while (true)
		{
			if (tooltipSubjectTypes.Count <= 0)
			{
				return;
			}
			while (true)
			{
				if (tooltipSubjectTypes.Contains(targetSubject))
				{
					while (true)
					{
						symbolToValue[symbolOfInterest] = damageAmount;
						return;
					}
				}
				return;
			}
		}
	}

	public static void AddNameplateValueForOverlap(ref Dictionary<AbilityTooltipSymbol, int> symbolToValue, AbilityUtil_Targeter targeter, ActorData targetActor, int currentTargeterIndex, int firstAmount, int subsequentAmount, AbilityTooltipSymbol symbolOfInterest = AbilityTooltipSymbol.Damage, AbilityTooltipSubject targetSubject = AbilityTooltipSubject.Primary)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		while (true)
		{
			if (tooltipSubjectTypes.Count > 0)
			{
				while (true)
				{
					using (List<AbilityTooltipSubject>.Enumerator enumerator = tooltipSubjectTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AbilityTooltipSubject current = enumerator.Current;
							if (current == targetSubject)
							{
								if (!symbolToValue.ContainsKey(symbolOfInterest))
								{
									symbolToValue[symbolOfInterest] = firstAmount;
								}
								else
								{
									symbolToValue[symbolOfInterest] += subsequentAmount;
								}
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
				}
			}
			return;
		}
	}

	public void ResetAbilityTargeters()
	{
		if (Targeters != null)
		{
			for (int i = 0; i < Targeters.Count; i++)
			{
				Targeters[i]?.ResetTargeter(true);
			}
		}
	}

	private void Update()
	{
		bool flag2;
		bool flag3;
		if (Targeter != null)
		{
			if (IsAbilitySelected())
			{
				bool flag = HighlightUtils.Get() != null && HighlightUtils.Get().m_cachedShouldShowAffectedSquares;
				flag2 = (m_lastUpdateShowingAffectedSquares != flag);
				m_lastUpdateShowingAffectedSquares = flag;
				flag3 = false;
				if (ActorData == GameFlowData.Get().activeOwnedActorData)
				{
					flag3 = true;
				}
				if (GameFlowData.Get().activeOwnedActorData == null)
				{
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					if (teamViewing != Team.Invalid)
					{
						if (teamViewing != ActorData.GetTeam())
						{
							goto IL_00ed;
						}
					}
					flag3 = true;
				}
				goto IL_00ed;
			}
		}
		goto IL_033f;
		IL_033f:
		if (Targeters == null)
		{
			return;
		}
		while (true)
		{
			if (!(ActorData != null))
			{
				return;
			}
			for (int i = 0; i < Targeters.Count; i++)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = Targeters[i];
				if (abilityUtil_Targeter != null)
				{
					abilityUtil_Targeter.UpdateFadeOutHighlights(ActorData);
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
		IL_0332:
		Targeter.UpdateArrowsForUI();
		goto IL_033f;
		IL_02fa:
		AbilityUtil_Targeter abilityUtil_Targeter2;
		if (HighlightUtils.Get().GetCurrentCursorType() != abilityUtil_Targeter2.GetCursorType())
		{
			HighlightUtils.Get().SetCursorType(abilityUtil_Targeter2.GetCursorType());
		}
		goto IL_0332;
		IL_00ed:
		if (flag3)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			ActorTurnSM actorTurnSM = ActorData.GetActorTurnSM();
			if (actorTurnSM != null && actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
			{
				AbilityTarget.UpdateAbilityTargetForForTargeterUpdate();
				AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
				if (GetExpectedNumberOfTargeters() < 2)
				{
					if (!flag2)
					{
						if (!Targeter.MarkedForForceUpdate)
						{
							if (Targeter.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
							{
								goto IL_0332;
							}
						}
					}
					Targeter.MarkedForForceUpdate = false;
					Targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
					Targeter.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
					Targeter.AdjustOpacityWhileTargeting();
					Targeter.SetupTargetingArc(activeOwnedActorData, false);
				}
				else
				{
					int count = actorTurnSM.GetAbilityTargets().Count;
					if (count < Targeters.Count)
					{
						abilityUtil_Targeter2 = Targeters[count];
						if (!flag2)
						{
							if (!Targeters[0].MarkedForForceUpdate)
							{
								if (abilityUtil_Targeter2.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
								{
									goto IL_02fa;
								}
							}
						}
						Targeters[0].MarkedForForceUpdate = false;
						abilityUtil_Targeter2.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
						if (abilityUtil_Targeter2.IsUsingMultiTargetUpdate())
						{
							abilityUtil_Targeter2.UpdateTargetingMultiTargets(abilityTargetForTargeterUpdate, ActorData, count, actorTurnSM.GetAbilityTargets());
						}
						else
						{
							abilityUtil_Targeter2.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
						}
						abilityUtil_Targeter2.AdjustOpacityWhileTargeting();
						abilityUtil_Targeter2.SetupTargetingArc(activeOwnedActorData, false);
						goto IL_02fa;
					}
				}
				goto IL_0332;
			}
		}
		goto IL_033f;
	}

	public virtual bool CustomCanCastValidation(ActorData caster)
	{
		return true;
	}

	public virtual bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return true;
	}

	public virtual bool AllowInvalidSquareForSquareBasedTarget()
	{
		return false;
	}

	public bool CanTargetActorInDecision(ActorData caster, ActorData targetActor, bool allowEnemies, bool allowAllies, bool allowSelf, ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false)
	{
		BoardSquare currentBoardSquare;
		bool flag2;
		if (caster != null)
		{
			if (!caster.IsDead() && caster.GetCurrentBoardSquare() != null && targetActor != null)
			{
				if (!targetActor.IsDead())
				{
					if (targetActor.GetCurrentBoardSquare() != null)
					{
						if (!targetActor.IgnoreForAbilityHits)
						{
							currentBoardSquare = targetActor.GetCurrentBoardSquare();
							bool flag = (!NetworkClient.active) ? targetActor.IsActorVisibleToActor(caster) : targetActor.IsVisibleToClient();
							flag2 = (caster.GetTeam() == targetActor.GetTeam());
							if (flag)
							{
								if (flag2)
								{
									if (allowAllies)
									{
										goto IL_012b;
									}
								}
								if (!flag2)
								{
									if (allowEnemies)
									{
										goto IL_012b;
									}
								}
							}
						}
					}
				}
			}
		}
		goto IL_030b;
		IL_02a1:
		bool flag3 = true;
		bool flag4;
		bool flag5;
		if (checkPath != 0)
		{
			if (flag4)
			{
				if (flag5)
				{
					bool passThroughInvalidSquares = checkPath == ValidateCheckPath.CanBuildPathAllowThroughInvalid;
					flag3 = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetActor.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare(), passThroughInvalidSquares, out int _);
				}
			}
		}
		int result;
		if (flag4)
		{
			if (flag5)
			{
				result = (flag3 ? 1 : 0);
				goto IL_030a;
			}
		}
		result = 0;
		goto IL_030a;
		IL_012b:
		if (!allowSelf)
		{
			if (!(caster != targetActor))
			{
				goto IL_030b;
			}
		}
		float currentMinRangeInSquares = AbilityUtils.GetCurrentMinRangeInSquares(this, caster, 0);
		float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
		flag4 = caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(targetActor.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare(), currentRangeInSquares, currentMinRangeInSquares);
		if (checkLineOfSight)
		{
			if (ignoreLosSettingOnTargetData)
			{
				flag4 = (flag4 && caster.GetCurrentBoardSquare()._0013(currentBoardSquare.x, currentBoardSquare.y));
			}
			else
			{
				flag4 = (flag4 && (!GetCheckLoS(0) || caster.GetCurrentBoardSquare()._0013(currentBoardSquare.x, currentBoardSquare.y)));
			}
		}
		flag5 = true;
		ActorStatus actorStatus = targetActor.GetActorStatus();
		int num;
		if (checkStatusImmunities && actorStatus != null)
		{
			if (flag2)
			{
				if (actorStatus.HasStatus(StatusType.CantBeHelpedByTeam))
				{
					goto IL_029e;
				}
			}
			if (flag2)
			{
				if (actorStatus.HasStatus(StatusType.BuffImmune))
				{
					goto IL_029e;
				}
			}
			if (!flag2)
			{
				if (actorStatus.HasStatus(StatusType.DebuffImmune))
				{
					goto IL_029e;
				}
			}
			if (actorStatus.HasStatus(StatusType.CantBeTargeted))
			{
				goto IL_029e;
			}
			num = ((!actorStatus.HasStatus(StatusType.EffectImmune)) ? 1 : 0);
			goto IL_029f;
		}
		goto IL_02a1;
		IL_030b:
		return false;
		IL_029e:
		num = 0;
		goto IL_029f;
		IL_030a:
		return (byte)result != 0;
		IL_029f:
		flag5 = ((byte)num != 0);
		goto IL_02a1;
	}

	public bool HasTargetableActorsInDecision(ActorData caster, bool allowEnemies, bool allowAllies, bool allowSelf, ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false)
	{
		if (GameFlowData.Get() != null)
		{
			List<ActorData> actorsVisibleToActor;
			if (NetworkServer.active)
			{
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster);
			}
			else
			{
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
			}
			List<ActorData> list = actorsVisibleToActor;
			for (int i = 0; i < list.Count; i++)
			{
				ActorData targetActor = list[i];
				if (CanTargetActorInDecision(caster, targetActor, allowEnemies, allowAllies, allowSelf, checkPath, checkLineOfSight, checkStatusImmunities, ignoreLosSettingOnTargetData))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void OnAbilitySelect()
	{
		if (Targeter == null)
		{
			return;
		}
		while (true)
		{
			Targeter.TargeterAbilitySelected();
			return;
		}
	}

	public void OnAbilityDeselect()
	{
		if (Targeter == null)
		{
			return;
		}
		while (true)
		{
			if (GetExpectedNumberOfTargeters() < 2)
			{
				Targeter.TargeterAbilityDeselected(0);
			}
			else
			{
				for (int i = 0; i < Targeters.Count; i++)
				{
					if (Targeters[i] != null)
					{
						Targeters[i].TargeterAbilityDeselected(i);
					}
				}
			}
			Targeter.HideAllSquareIndicators();
			DestroyBackupTargetingInfo(true);
			return;
		}
	}

	public void BackupTargetingForRedo(ActorTurnSM turnSM)
	{
		BackupTargeterHighlights = new List<GameObject>();
		List<AbilityTarget> list = new List<AbilityTarget>();
		for (int i = 0; i < Targeters.Count; i++)
		{
			if (i < GetExpectedNumberOfTargeters())
			{
				BackupTargeterHighlights.AddRange(Targeters[i].GetHighlightCopies(true));
				AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromWorldPos(Vector3.zero, Vector3.forward);
				abilityTarget.SetPosAndDir(Targeters[i].LastUpdatingGridPos, Targeters[i].LastUpdateFreePos, Targeters[i].LastUpdateAimDir);
				list.Add(abilityTarget);
				continue;
			}
			break;
		}
		if (list.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			BackupTargets = new List<AbilityTarget>();
			using (List<AbilityTarget>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityTarget current = enumerator.Current;
					AbilityTarget copy = current.GetCopy();
					BackupTargets.Add(copy);
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
	}

	public void DestroyBackupTargetingInfo(bool highlightsOnly)
	{
		if (!BackupTargeterHighlights.IsNullOrEmpty())
		{
			using (List<GameObject>.Enumerator enumerator = BackupTargeterHighlights.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					if (current != null)
					{
						HighlightUtils.DestroyObjectAndMaterials(current);
					}
				}
			}
		}
		BackupTargeterHighlights = null;
		if (!highlightsOnly)
		{
			BackupTargets = null;
		}
	}

	public bool IsAbilitySelected()
	{
		object obj;
		if (ActorData == null)
		{
			obj = null;
		}
		else
		{
			obj = ActorData.GetAbilityData();
		}
		AbilityData abilityData = (AbilityData)obj;
		int result;
		if (abilityData != null)
		{
			result = ((abilityData.GetSelectedAbility() == this) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool IsActorInTargetRange(ActorData actor)
	{
		bool inCover;
		return IsActorInTargetRange(actor, out inCover);
	}

	public bool IsActorInTargetRange(ActorData actor, out bool inCover)
	{
		bool flag = false;
		inCover = false;
		if (Targeter != null)
		{
			if (GetExpectedNumberOfTargeters() >= 2)
			{
				if (Targeters.Count >= 2)
				{
					inCover = true;
					for (int i = 0; i < Targeters.Count; i++)
					{
						if (flag)
						{
							if (!inCover)
							{
								break;
							}
						}
						if (Targeters[i] == null)
						{
							continue;
						}
						if (!Targeters[i].IsActorInTargetRange(actor, out bool inCover2))
						{
							continue;
						}
						flag = true;
						if (i == 0)
						{
							inCover = inCover2;
						}
						else
						{
							inCover = (inCover && inCover2);
						}
					}
					if (!flag)
					{
						inCover = false;
					}
					goto IL_0149;
				}
			}
			flag = Targeter.IsActorInTargetRange(actor, out inCover);
		}
		else
		{
			Log.Warning("Ability " + m_abilityName + " has no targeter, but we're checking actors in its range.");
			flag = (Board.Get().PlayerClampedSquare == actor.GetCurrentBoardSquare());
			inCover = false;
		}
		goto IL_0149;
		IL_0149:
		return flag;
	}

	public virtual bool ActorCountTowardsEnergyGain(ActorData target, ActorData caster)
	{
		return true;
	}

	public int GetTargetCounts(ActorData caster, int upToTargeterIndex, out int numAlliesExcludingSelf, out int numEnemies, out bool hittingSelf)
	{
		numAlliesExcludingSelf = 0;
		numEnemies = 0;
		hittingSelf = false;
		HashSet<int> hashSet = new HashSet<int>();
		if (Targeters != null)
		{
			for (int i = 0; i < Targeters.Count; i++)
			{
				if (i > upToTargeterIndex)
				{
					break;
				}
				List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeters[i].GetActorsInRange();
				using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
						if (!ActorCountTowardsEnergyGain(current.m_actor, caster))
						{
						}
						else if (!hashSet.Contains(current.m_actor.ActorIndex))
						{
							hashSet.Add(current.m_actor.ActorIndex);
							if (!current.m_actor.IgnoreForEnergyOnHit)
							{
								if (caster.GetTeam() == current.m_actor.GetTeam())
								{
									if (caster != current.m_actor)
									{
										numAlliesExcludingSelf++;
									}
									else
									{
										hittingSelf = true;
									}
								}
								else
								{
									numEnemies++;
								}
							}
						}
					}
				}
			}
		}
		return hashSet.Count;
	}

	public virtual TargetData[] GetBaseTargetData()
	{
		return m_targetData;
	}

	public virtual TargetData[] GetTargetData()
	{
		TargetData[] result = GetBaseTargetData();
		if (m_currentAbilityMod != null)
		{
			if (m_currentAbilityMod.m_useTargetDataOverrides)
			{
				result = m_currentAbilityMod.m_targetDataOverrides;
			}
		}
		return result;
	}

	public virtual float GetRangeInSquares(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		float num = 0f;
		if (targetData != null)
		{
			if (targetData.Length > targetIndex)
			{
				num = targetData[targetIndex].m_range;
			}
		}
		if (m_currentAbilityMod != null)
		{
			num = Mathf.Max(0f, m_currentAbilityMod.m_targetDataMaxRangeMod.GetModifiedValue(num));
		}
		return num;
	}

	public float GetMinRangeInSquares(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		float num = 0f;
		if (targetData != null)
		{
			if (targetData.Length > targetIndex)
			{
				num = targetData[targetIndex].m_minRange;
			}
		}
		if (m_currentAbilityMod != null)
		{
			num = Mathf.Max(0f, m_currentAbilityMod.m_targetDataMinRangeMod.GetModifiedValue(num));
		}
		return num;
	}

	public virtual bool GetCheckLoS(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		bool flag = true;
		if (targetData != null && targetData.Length > targetIndex)
		{
			flag = targetData[targetIndex].m_checkLineOfSight;
			if (m_currentAbilityMod != null)
			{
				flag = m_currentAbilityMod.m_targetDataCheckLosMod.GetModifiedValue(flag);
			}
		}
		return flag;
	}

	public string GetTargetDescription(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		string result = null;
		if (targetData != null && targetData.Length > targetIndex)
		{
			result = "Select " + targetData[targetIndex].m_description;
		}
		return result;
	}

	public bool IsAutoSelect()
	{
		TargetData[] targetData = GetTargetData();
		int result;
		if (targetData != null)
		{
			result = ((targetData.Length == 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public virtual bool IsFreeAction()
	{
		bool result = m_freeAction;
		if (m_currentAbilityMod != null)
		{
			result = m_currentAbilityMod.m_isFreeActionMod.GetModifiedValue(m_freeAction);
		}
		return result;
	}

	public virtual bool ShouldRotateToTargetPos()
	{
		bool result = !IsSimpleAction();
		if (m_rotationVisibilityMode == RotationVisibilityMode.OnAllyClientOnly)
		{
			result = true;
			if (NetworkClient.active)
			{
				ActorData actorData = ActorData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
				{
					if (actorData != null)
					{
						result = (activeOwnedActorData.GetTeam() == actorData.GetTeam());
					}
				}
			}
		}
		else if (m_rotationVisibilityMode == RotationVisibilityMode.Always)
		{
			result = true;
		}
		else if (m_rotationVisibilityMode == RotationVisibilityMode.Never)
		{
			result = false;
		}
		return result;
	}

	public virtual Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		TargetData[] targetData = GetTargetData();
		if (targetData != null)
		{
			if (targetData.Length > 0)
			{
				if (targetData[0].m_targetingParadigm == TargetingParadigm.BoardSquare)
				{
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targets[0].GridPos);
					if (boardSquareSafe != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return boardSquareSafe.ToVector3();
							}
						}
					}
				}
			}
		}
		return targets[0].FreePos;
	}

	public bool IsSimpleAction()
	{
		TargetData[] targetData = GetTargetData();
		return targetData.Length == 0;
	}

	public virtual AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		return AbilityTarget.CreateSimpleAbilityTarget(caster);
	}

	public virtual bool IsDamageUnpreventable()
	{
		return false;
	}

	public TargetingParadigm GetTargetingParadigm(int targetIndex)
	{
		TargetingParadigm result = TargetingParadigm.Position;
		TargetData[] targetData = GetTargetData();
		if (targetData != null)
		{
			if (targetData.Length > targetIndex)
			{
				result = targetData[targetIndex].m_targetingParadigm;
			}
		}
		return result;
	}

	public virtual TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return GetTargetingParadigm(targetIndex);
	}

	public int GetNumTargets()
	{
		TargetData[] targetData = GetTargetData();
		int result = 0;
		if (targetData != null)
		{
			result = targetData.Length;
		}
		return result;
	}

	public virtual int GetExpectedNumberOfTargeters()
	{
		return 1;
	}

	public virtual List<StatusType> GetStatusToApplyWhenRequested()
	{
		if (m_currentAbilityMod != null && m_currentAbilityMod.m_useStatusWhenRequestedOverride)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_currentAbilityMod.m_statusWhenRequestedOverride;
				}
			}
		}
		return m_statusWhenRequested;
	}

	public virtual bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return false;
	}

	public virtual bool HasPassivePendingStatus(StatusType status, ActorData owner)
	{
		return false;
	}

	public virtual bool ForceIgnoreCover(ActorData targetActor)
	{
		return false;
	}

	public virtual bool ForceReduceCoverEffectiveness(ActorData targetActor)
	{
		return false;
	}

	public virtual bool CanOverrideMoveStartSquare()
	{
		return false;
	}

	public virtual void OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction> resolutionActions, ActorData caster)
	{
	}

	public Ability[] GetChainAbilities()
	{
		Ability[] result = m_chainAbilities;
		if (m_currentAbilityMod != null)
		{
			if (m_currentAbilityMod.m_useChainAbilityOverrides)
			{
				result = m_currentAbilityMod.m_chainAbilityOverrides;
			}
		}
		return result;
	}

	public bool HasAbilityAsPartOfChain(Ability ability)
	{
		Ability[] chainAbilities = GetChainAbilities();
		Ability[] array = chainAbilities;
		foreach (Ability y in array)
		{
			if (!(ability == y))
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public virtual bool ShouldAutoQueueIfValid()
	{
		bool flag = AbilityUtils.AbilityHasTag(this, AbilityTags.AutoQueueIfValid);
		if (m_currentAbilityMod != null)
		{
			if (m_currentAbilityMod.m_autoQueueIfValidMod.operation != 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_currentAbilityMod.m_autoQueueIfValidMod.GetModifiedValue(flag);
					}
				}
			}
		}
		return flag;
	}

	public virtual bool AllowCancelWhenAutoQueued()
	{
		return false;
	}

	public virtual MovementAdjustment GetMovementAdjustment()
	{
		if (m_currentAbilityMod != null)
		{
			if (m_currentAbilityMod.m_useMovementAdjustmentOverride)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_currentAbilityMod.m_movementAdjustmentOverride;
					}
				}
			}
		}
		return m_movementAdjustment;
	}

	public bool GetPreventsMovement()
	{
		return GetMovementAdjustment() == MovementAdjustment.NoMovement;
	}

	public bool GetAffectsMovement()
	{
		return GetMovementAdjustment() != MovementAdjustment.FullMovement;
	}

	public virtual bool ShouldUpdateDrawnTargetersOnQueueChange()
	{
		return false;
	}

	internal virtual ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.None;
	}

	internal virtual ActorData.TeleportType GetEvasionTeleportType()
	{
		return ActorData.TeleportType.Evasion_DontAdjustToVision;
	}

	internal virtual bool IsCharge()
	{
		int result;
		if (GetMovementType() != ActorData.MovementType.Charge)
		{
			result = ((GetMovementType() == ActorData.MovementType.WaypointFlight) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	internal virtual bool IsTeleport()
	{
		int result;
		if (GetMovementType() != ActorData.MovementType.Teleport)
		{
			result = ((GetMovementType() == ActorData.MovementType.Flight) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	internal virtual bool IsEvadeDestinationReserved()
	{
		return false;
	}

	internal virtual bool IsStealthEvade()
	{
		return false;
	}

	internal virtual BoardSquare GetEvadeDestinationForTargeter(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare result = null;
		if (targets != null)
		{
			if (targets.Count > 0)
			{
				if (GetRunPriority() == AbilityPriority.Evasion)
				{
					result = Board.Get().GetBoardSquareSafe(targets[targets.Count - 1].GridPos);
				}
			}
		}
		return result;
	}

	internal float CalcMovementSpeed(float distance)
	{
		float result;
		if (m_movementDuration > 0f)
		{
			result = distance / m_movementDuration;
		}
		else
		{
			result = m_movementSpeed;
		}
		return result;
	}

	public string GetTooltipForUI()
	{
		if (m_toolTipForUI == null)
		{
			RebuildTooltipForUI();
		}
		return m_toolTipForUI;
	}

	public virtual ActorModelData.ActionAnimationType GetActionAnimType()
	{
		ActorModelData.ActionAnimationType result = m_actionAnimType;
		if (m_currentAbilityMod != null)
		{
			if (m_currentAbilityMod.m_useActionAnimTypeOverride)
			{
				result = m_currentAbilityMod.m_actionAnimTypeOverride;
			}
		}
		return result;
	}

	public virtual ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		return GetActionAnimType();
	}

	public virtual bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool flag = animIndex == (int)GetActionAnimType();
		if (!flag)
		{
			Ability[] chainAbilities = GetChainAbilities();
			for (int i = 0; i < chainAbilities.Length; i++)
			{
				if (!flag)
				{
					flag = (animIndex == (int)chainAbilities[i].GetActionAnimType());
					continue;
				}
				break;
			}
		}
		return flag;
	}

	public virtual bool UseAbilitySequenceSourceForEvadeOrKnockbackTaunt()
	{
		return false;
	}

	public void SanitizeChainAbilities()
	{
		if (m_chainAbilities == null)
		{
			m_chainAbilities = new Ability[0];
		}
		Ability[] chainAbilities = GetChainAbilities();
		foreach (Ability ability in chainAbilities)
		{
			string str = "Ability '" + m_abilityName + "'- chain ability '" + ability.m_abilityName + "' ";
			if (ability.m_techPointsCost != 0)
			{
				Log.Warning(str + "has non-zero tech point cost.  Zeroing...");
				ability.m_techPointsCost = 0;
			}
			if (ability.GetAffectsMovement())
			{
				Log.Warning(str + "adjusts movement.  Removing...");
				ability.m_movementAdjustment = MovementAdjustment.FullMovement;
			}
			if (ability.m_cooldown != 0)
			{
				Log.Warning(str + "has non-zero cooldown.  Zeroing...");
				ability.m_cooldown = 0;
			}
			if (!ability.IsFreeAction())
			{
				Log.Warning(str + "is not a free action.  Liberating...");
				ability.m_freeAction = true;
			}
			if (ability.m_chainAbilities.Length != 0)
			{
				Log.Warning(str + "has its own chain abilities.  Breaking...");
				ability.m_chainAbilities = new Ability[0];
			}
			if (RunPriority > ability.RunPriority)
			{
				Log.Warning(str + "has an earlier run priority than its predecessor.  Make sure chain abilities happen later than the 'master' ability for things to look right.");
			}
		}
	}

	public virtual bool IsFlashAbility()
	{
		return false;
	}

	public virtual int GetTechPointRegenContribution()
	{
		return 0;
	}

	protected virtual void OnApplyAbilityMod(AbilityMod abilityMod)
	{
	}

	public void ApplyAbilityMod(AbilityMod abilityMod, ActorData actor)
	{
		if (abilityMod.GetTargetAbilityType() == GetType())
		{
			ResetAbilityTargeters();
			ActorTargeting actorTargeting = actor.GetActorTargeting();
			if (actorTargeting != null)
			{
				actorTargeting.MarkForForceRedraw();
			}
			ClearAbilityMod(actor);
			m_currentAbilityMod = abilityMod;
			OnApplyAbilityMod(abilityMod);
			ResetNameplateTargetingNumbers();
		}
		else
		{
			Debug.LogError("Trying to apply mod to wrong ability type. mod_ability_type: " + abilityMod.GetTargetAbilityType().ToString() + " ability_type: " + GetType().ToString());
		}
	}

	protected virtual void OnRemoveAbilityMod()
	{
	}

	public void ClearAbilityMod(ActorData actor)
	{
		if (!(m_currentAbilityMod != null))
		{
			return;
		}
		while (true)
		{
			m_currentAbilityMod = null;
			OnRemoveAbilityMod();
			ResetNameplateTargetingNumbers();
			if (Application.isEditor)
			{
				Debug.Log("Removing mod from ability " + GetDebugIdentifier("orange"));
			}
			return;
		}
	}

	public void DrawGizmos()
	{
		if (Targeter == null || !IsAbilitySelected())
		{
			return;
		}
		while (true)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (!(activeOwnedActorData != null))
			{
				return;
			}
			while (true)
			{
				if (!(activeOwnedActorData == ActorData))
				{
					return;
				}
				while (true)
				{
					ActorTurnSM actorTurnSM = activeOwnedActorData.GetActorTurnSM();
					if (actorTurnSM != null && actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
					{
						while (true)
						{
							AbilityTarget currentTarget = AbilityTarget.CreateAbilityTargetFromInterface();
							Targeter.DrawGizmos(currentTarget, activeOwnedActorData);
							return;
						}
					}
					return;
				}
			}
		}
	}

	public virtual void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
	}

	public virtual void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
	}

	public virtual void OnEvasionMoveStartEvent(ActorData caster)
	{
	}

	public virtual void OnAbilityAnimationReleaseFocus(ActorData caster)
	{
	}

	public virtual bool UseTargeterGridPosForCameraBounds()
	{
		int result;
		if (GetTargetData() != null && GetTargetData().Length > 0)
		{
			result = ((GetTargetingParadigm(0) != TargetingParadigm.Direction) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public virtual List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		if (m_runPriority != AbilityPriority.Evasion)
		{
			for (int i = 0; i < targets.Count; i++)
			{
				list.Add(targets[i].FreePos);
			}
		}
		return list;
	}

	public virtual bool IgnoreCameraFraming()
	{
		return false;
	}

	public virtual int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 0;
	}

	public bool CalcBoundsOfInterestForCamera(out Bounds bounds, List<AbilityTarget> targets, ActorData caster)
	{
		bounds = default(Bounds);
		List<Vector3> list = CalcPointsOfInterestForCamera(targets, caster);
		int num;
		if (list != null)
		{
			num = ((list.Count > 0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (flag)
		{
			bounds.center = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				bounds.Encapsulate(list[i]);
			}
		}
		return flag;
	}

	protected Passive GetPassiveOfType(Type passiveType)
	{
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return component.GetPassiveOfType(passiveType);
				}
			}
		}
		return null;
	}

	protected T GetPassiveOfType<T>() where T : Passive
	{
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			return component.GetPassiveOfType<T>();
		}
		return (T)null;
	}

	protected Ability GetAbilityOfType(Type abilityType)
	{
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			return component.GetAbilityOfType(abilityType);
		}
		return null;
	}

	protected T GetAbilityOfType<T>() where T : Ability
	{
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return component.GetAbilityOfType<T>();
				}
			}
		}
		return (T)null;
	}

	protected AbilityData.ActionType GetActionTypeOfAbility(Ability ability)
	{
		AbilityData.ActionType result = AbilityData.ActionType.INVALID_ACTION;
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			result = component.GetActionTypeOfAbility(ability);
		}
		return result;
	}

	public string GetBaseAbilityDesc()
	{
		string text = string.Empty;
		if (m_techPointInteractions != null)
		{
			if (m_techPointInteractions.Length > 0)
			{
				text += "TechPoint Interactions:\n";
			}
			for (int i = 0; i < m_techPointInteractions.Length; i++)
			{
				string text2 = text;
				text = text2 + "    [" + m_techPointInteractions[i].m_type.ToString() + "] = " + m_techPointInteractions[i].m_amount + "\n";
			}
		}
		if (m_tags != null)
		{
			if (m_tags.Count > 0)
			{
				text += "Tags:\n";
			}
			for (int j = 0; j < m_tags.Count; j++)
			{
				text = text + "    [" + m_tags[j].ToString() + "]\n";
			}
		}
		if (text.Length > 0)
		{
			text += "\n";
		}
		return text;
	}

	public virtual string GetSetupNotesForEditor()
	{
		return string.Empty;
	}

	public static string SetupNoteVarName(string input)
	{
		return "<color=white>[ " + input + " ]</color>";
	}

	public string GetDebugIdentifier(string colorString = "")
	{
		string text = "Ability " + m_abilityName + "[ " + GetType().ToString() + " ]";
		if (colorString.Length > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return "<color=" + colorString + ">" + text + "</color>";
				}
			}
		}
		return text;
	}
}
