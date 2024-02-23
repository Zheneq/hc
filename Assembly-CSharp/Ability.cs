using System;
using System.Collections.Generic;
using System.Text;
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
	public string m_debugUnlocalizedTooltip = "";
	[HideInInspector]
	public List<StatusType> m_savedStatusTypesForTooltips;

	[TextArea(1, 20, order = 1)]
	public string m_shortToolTip;
	private string m_toolTipForUI;
	public string m_rewardString = "";

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
	public string m_flavorText = "";
	public bool m_ultimate;
	public string m_previewVideo;

	[Header("-- for Sequence prefab naming prefix, optional")]
	public string m_expectedSequencePrefixForEditor = "";

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

	public Sprite sprite { get; set; }

	protected ActorData ActorData
	{
		get
		{
			if (m_actorData == null && !m_searchedForActorData)
			{
				m_actorData = m_overrideActorDataIndex != ActorData.s_invalidActorIndex
					? GameFlowData.Get().FindActorByActorIndex(m_overrideActorDataIndex)
					: GetComponent<ActorData>();
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

	public List<AbilityUtil_Targeter> Targeters
	{
		get { return m_targeters; }
	}

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
				m_targeters.Add(value);
			}
			else
			{
				m_targeters[0] = value;
			}
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

	public AbilityMod CurrentAbilityMod
	{
		get { return m_currentAbilityMod; }
	}

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
		if (Targeters != null)
		{
			ResetAbilityTargeters();
			Targeters.Clear();
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
		if (m_techPointInteractions != null && m_techPointInteractions.Length > 0)
		{
			foreach (TechPointInteraction techPointInteraction in m_techPointInteractions)
			{
				int num = techPointInteraction.m_amount;
				if (mod != null)
				{
					num = mod.GetModdedTechPointForInteraction(techPointInteraction.m_type, num);
				}
				list.Add(new TooltipTokenInt(techPointInteraction.m_type.ToString(), "Energy Gain", num));
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
		if (addForNonPositive || val > 0f)
		{
			tokens.Add(new TooltipTokenFloat(name, desc, val));
		}
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
		if (addForNonPositive || val > 0f)
		{
			int val2 = Mathf.RoundToInt(val * 100f);
			tokens.Add(new TooltipTokenPct(name, desc, val2));
		}
	}

	public virtual AbilityPriority GetRunPriority()
	{
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_useRunPriorityOverride)
		{
			return CurrentAbilityMod.m_runPriorityOverride;
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
			case UIQueueListPanel.UIPhase.Prep:
				result = "Prep";
				break;
			case UIQueueListPanel.UIPhase.Evasion:
				result = "Dash";
				break;
			case UIQueueListPanel.UIPhase.Combat:
				result = "Blast";
				break;
			case UIQueueListPanel.UIPhase.Movement:
				result = "Movement";
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
			case UIQueueListPanel.UIPhase.Prep:
				result = StringUtil.TR("Prep", "Global");
				break;
			case UIQueueListPanel.UIPhase.Evasion:
				result = StringUtil.TR("Dash", "Global");
				break;
			case UIQueueListPanel.UIPhase.Combat:
				result = StringUtil.TR("Blast", "Global");
				break;
			case UIQueueListPanel.UIPhase.Movement:
				result = StringUtil.TR("Movement", "Global");
				break;
		}
		return result;
	}

	public string GetCostString()
	{
		if (GetBaseCost() > 0)
		{
			return new StringBuilder().Append(GetBaseCost()).Append(" energy").ToString();
		}
		return "";
	}

	public int GetBaseCost()
	{
		if (m_techPointsCost <= 0)
		{
			return 0;
		}
		return m_techPointsCost;
	}

	public virtual int GetModdedCost()
	{
		int num = GetBaseCost();
		if (CurrentAbilityMod != null)
		{
			num = Mathf.Max(0, CurrentAbilityMod.m_techPointCostMod.GetModifiedValue(num));
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
		return new StringBuilder().Append(m_cooldown).Append(" turn cooldown").ToString();
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
			if (num > 0f && num < 15f && targetData2.m_targetingParadigm != TargetingParadigm.Direction)
			{
				return true;
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
		overridePos = aimingActor.GetFreePos();
		return false;
	}

	public virtual int GetBaseCooldown()
	{
		return m_cooldown;
	}

	public int GetModdedCooldown()
	{
		int cooldown = GetBaseCooldown();
		if (cooldown >= 0 && m_currentAbilityMod != null)
		{
			cooldown = Mathf.Max(0, m_currentAbilityMod.m_maxCooldownMod.GetModifiedValue(cooldown));
		}
		return cooldown;
	}

	public virtual int GetCooldownForUIDisplay()
	{
		return GetModdedCooldown();
	}

	public StandardEffectInfo GetModdedEffectForEnemies()
	{
		if (CurrentAbilityMod != null)
		{
			return CurrentAbilityMod.m_effectToTargetEnemyOnHit;
		}
		return null;
	}

	public StandardEffectInfo GetModdedEffectForAllies()
	{
		if (CurrentAbilityMod != null)
		{
			return CurrentAbilityMod.m_effectToTargetAllyOnHit;
		}
		return null;
	}

	public StandardEffectInfo GetModdedEffectForSelf()
	{
		if (CurrentAbilityMod != null)
		{
			return CurrentAbilityMod.m_effectToSelfOnCast;
		}
		return null;
	}

	public bool HasSelfEffectFromBaseMod()
	{
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		return moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect;
	}

	public bool GetModdedUseAllyEffectForTargetedCaster()
	{
		return CurrentAbilityMod != null && CurrentAbilityMod.m_useAllyEffectForTargetedCaster;
	}

	public float GetModdedChanceToTriggerEffects()
	{
		if (CurrentAbilityMod != null)
		{
			return CurrentAbilityMod.m_effectTriggerChance;
		}
		return 1f;
	}

	public bool ModdedChanceToTriggerEffectsIsPerHit()
	{
		return CurrentAbilityMod != null && CurrentAbilityMod.m_effectTriggerChanceMultipliedPerHit;
	}

	public int GetBaseMaxStocks()
	{
		return m_maxStocks;
	}

	public int GetModdedMaxStocks()
	{
		int num = m_maxStocks;
		if (m_maxStocks >= 0 && CurrentAbilityMod != null)
		{
			num = Mathf.Max(0, CurrentAbilityMod.m_maxStocksMod.GetModifiedValue(num));
		}
		return num;
	}

	public bool RefillAllStockOnRefresh()
	{
		if (CurrentAbilityMod)
		{
			return CurrentAbilityMod.m_refillAllStockOnRefreshMod.GetModifiedValue(m_refillAllStockOnRefresh);
		}
		return m_refillAllStockOnRefresh;
	}

	public int GetBaseStockRefreshDuration()
	{
		return m_stockRefreshDuration;
	}

	public int GetModdedStockRefreshDuration()
	{
		int num = m_stockRefreshDuration;
		if (CurrentAbilityMod != null)
		{
			num = Mathf.Max(-1, CurrentAbilityMod.m_stockRefreshDurationMod.GetModifiedValue(num));
		}
		return num;
	}

	private void RebuildTooltipForUI()
	{
		m_toolTipForUI = GetNameString();
		if (GetBaseCost() > 0)
		{
			m_toolTipForUI += new StringBuilder().Append(" - ").Append(GetBaseCost()).Append(" TP").ToString();
		}
		if (m_cooldown > 0)
		{
			m_toolTipForUI += new StringBuilder().Append(" - ").Append(m_cooldown).Append(" turn cooldown").ToString();
		}
		if (IsFreeAction())
		{
			m_toolTipForUI = new StringBuilder().Append("This is a Free Action.\n").Append(m_toolTipForUI).ToString();
		}

		m_toolTipForUI = new StringBuilder().Append(m_toolTipForUI).Append("\n").Append(GetFullTooltip()).ToString();
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
		if (CurrentAbilityMod == null)
		{
			return;
		}
		if (enemyEffectSubject != AbilityTooltipSubject.None)
		{
			StandardEffectInfo moddedEffectForEnemies = GetModdedEffectForEnemies();
			if (moddedEffectForEnemies != null)
			{
				moddedEffectForEnemies.ReportAbilityTooltipNumbers(ref numbers, enemyEffectSubject);
			}
		}
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (allyEffectSubject != AbilityTooltipSubject.None && moddedEffectForAllies != null)
		{
			moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, allyEffectSubject);
		}
		if (selfEffectSubject == AbilityTooltipSubject.None)
		{
			return;
		}
		if (CurrentAbilityMod.m_useAllyEffectForTargetedCaster && moddedEffectForAllies != null)
		{
			moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, selfEffectSubject);
		}
		if (moddedEffectForSelf != null)
		{
			moddedEffectForSelf.ReportAbilityTooltipNumbers(ref numbers, selfEffectSubject);
		}
	}

	protected virtual List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public virtual List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = new List<int>();
		List<AbilityTooltipNumber> baseNumbers = BaseCalculateAbilityTooltipNumbers();
		if (baseNumbers != null)
		{
			foreach (AbilityTooltipNumber item in baseNumbers)
			{
				list.Add(item.m_value);
			}
		}
		return list;
	}

	public virtual List<StatusType> GetStatusTypesForTooltip()
	{
		if (m_savedStatusTypesForTooltips != null && m_savedStatusTypesForTooltips.Count != 0)
		{
			return m_savedStatusTypesForTooltips;
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
		if (tooltipSubjectTypes != null
			&& tooltipSubjectTypes.Count > 0
			&& tooltipSubjectTypes.Contains(targetSubject))
		{
			symbolToValue[symbolOfInterest] = damageAmount;
		}
	}

	public static void AddNameplateValueForOverlap(ref Dictionary<AbilityTooltipSymbol, int> symbolToValue, AbilityUtil_Targeter targeter, ActorData targetActor, int currentTargeterIndex, int firstAmount, int subsequentAmount, AbilityTooltipSymbol symbolOfInterest = AbilityTooltipSymbol.Damage, AbilityTooltipSubject targetSubject = AbilityTooltipSubject.Primary)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Count > 0)
		{
			foreach (AbilityTooltipSubject current in tooltipSubjectTypes)
			{
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
		}
	}

	public void ResetAbilityTargeters()
	{
		if (Targeters != null)
		{
			for (int i = 0; i < Targeters.Count; i++)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = Targeters[i];
				if (abilityUtil_Targeter != null)
				{
					abilityUtil_Targeter.ResetTargeter(true);
				}
			}
		}
	}

	private void Update()
	{
		if (Targeter != null && IsAbilitySelected())
		{
			bool shouldShowAffectedSquares = HighlightUtils.Get() != null && HighlightUtils.Get().m_cachedShouldShowAffectedSquares;
			bool changedShowAffectedSquares = m_lastUpdateShowingAffectedSquares != shouldShowAffectedSquares;
			m_lastUpdateShowingAffectedSquares = shouldShowAffectedSquares;
			bool flag3 = false;
			if (ActorData == GameFlowData.Get().activeOwnedActorData)
			{
				flag3 = true;
			}
			if (GameFlowData.Get().activeOwnedActorData == null)
			{
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				if (teamViewing == Team.Invalid || teamViewing == ActorData.GetTeam())
				{
					flag3 = true;
				}
			}
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
						if (changedShowAffectedSquares || Targeter.MarkedForForceUpdate || !Targeter.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
						{
							Targeter.MarkedForForceUpdate = false;
							Targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
							Targeter.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
							Targeter.AdjustOpacityWhileTargeting();
							Targeter.SetupTargetingArc(activeOwnedActorData, false);
						}
					}
					else
					{
						int count = actorTurnSM.GetAbilityTargets().Count;
						if (count < Targeters.Count)
						{
							AbilityUtil_Targeter targeter = Targeters[count];
							if (changedShowAffectedSquares
								|| Targeters[0].MarkedForForceUpdate
								|| !targeter.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
							{
								Targeters[0].MarkedForForceUpdate = false;
								targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
								if (targeter.IsUsingMultiTargetUpdate())
								{
									targeter.UpdateTargetingMultiTargets(abilityTargetForTargeterUpdate, ActorData, count, actorTurnSM.GetAbilityTargets());
								}
								else
								{
									targeter.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
								}
								targeter.AdjustOpacityWhileTargeting();
								targeter.SetupTargetingArc(activeOwnedActorData, false);
							}
							if (HighlightUtils.Get().GetCurrentCursorType() != targeter.GetCursorType())
							{
								HighlightUtils.Get().SetCursorType(targeter.GetCursorType());
							}
						}
					}
					Targeter.UpdateArrowsForUI();
				}
			}
		}
		if (Targeters != null && ActorData != null)
		{
			for (int i = 0; i < Targeters.Count; i++)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = Targeters[i];
				if (abilityUtil_Targeter != null)
				{
					abilityUtil_Targeter.UpdateFadeOutHighlights(ActorData);
				}
			}
		}
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
		if (caster == null
			|| caster.IsDead()
			|| caster.GetCurrentBoardSquare() == null
			|| targetActor == null
			|| targetActor.IsDead()
			|| targetActor.GetCurrentBoardSquare() == null
			|| targetActor.IgnoreForAbilityHits)
		{
			return false;
		}

		BoardSquare currentBoardSquare = targetActor.GetCurrentBoardSquare();
		bool isVisible = NetworkClient.active ? targetActor.IsActorVisibleToClient() : targetActor.IsActorVisibleToActor(caster);
		bool isAlly = caster.GetTeam() == targetActor.GetTeam();

		if (!isVisible
			|| ((!isAlly || !allowAllies) && (isAlly || !allowEnemies))
			|| (!allowSelf && caster == targetActor))
		{
			return false;
		}

		float currentMinRangeInSquares = AbilityUtils.GetCurrentMinRangeInSquares(this, caster, 0);
		float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
		bool isInRange = caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(targetActor.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare(), currentRangeInSquares, currentMinRangeInSquares);
		if (checkLineOfSight)
		{
			if (ignoreLosSettingOnTargetData)
			{
				isInRange = isInRange && caster.GetCurrentBoardSquare().GetLOS(currentBoardSquare.x, currentBoardSquare.y);
			}
			else
			{
				isInRange = isInRange && (!GetCheckLoS(0) || caster.GetCurrentBoardSquare().GetLOS(currentBoardSquare.x, currentBoardSquare.y));
			}
		}

		bool isValidTarget = true;
		ActorStatus actorStatus = targetActor.GetActorStatus();
		if (checkStatusImmunities && actorStatus != null)
		{
			isValidTarget = (!isAlly || !actorStatus.HasStatus(StatusType.CantBeHelpedByTeam))
				&& (!isAlly || !actorStatus.HasStatus(StatusType.BuffImmune))
				&& (isAlly || !actorStatus.HasStatus(StatusType.DebuffImmune))
				&& !actorStatus.HasStatus(StatusType.CantBeTargeted)
				&& (!actorStatus.HasStatus(StatusType.EffectImmune));
		}

		bool canCharge = true;
		if (checkPath != ValidateCheckPath.Ignore && isInRange && isValidTarget)
		{
			bool passThroughInvalidSquares = checkPath == ValidateCheckPath.CanBuildPathAllowThroughInvalid;
			int x;
			canCharge = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetActor.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare(), passThroughInvalidSquares, out x);
		}
		return isInRange && isValidTarget && canCharge;
	}

	public bool HasTargetableActorsInDecision(ActorData caster, bool allowEnemies, bool allowAllies, bool allowSelf, ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false)
	{
		if (GameFlowData.Get() != null)
		{
			List<ActorData> actorsVisibleToActor = NetworkServer.active
				? GameFlowData.Get().GetActorsVisibleToActor(caster)
				: GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
			for (int i = 0; i < actorsVisibleToActor.Count; i++)
			{
				ActorData targetActor = actorsVisibleToActor[i];
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
		if (Targeter != null)
		{
			Targeter.TargeterAbilitySelected();
		}
	}

	public void OnAbilityDeselect()
	{
		if (Targeter != null)
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
		}
	}

	public void BackupTargetingForRedo(ActorTurnSM turnSM)
	{
		BackupTargeterHighlights = new List<GameObject>();
		List<AbilityTarget> list = new List<AbilityTarget>();
		for (int i = 0; i < Targeters.Count && i < GetExpectedNumberOfTargeters(); i++)
		{
			BackupTargeterHighlights.AddRange(Targeters[i].GetHighlightCopies(true));
			AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromWorldPos(Vector3.zero, Vector3.forward);
			abilityTarget.SetPosAndDir(Targeters[i].LastUpdatingGridPos, Targeters[i].LastUpdateFreePos, Targeters[i].LastUpdateAimDir);
			list.Add(abilityTarget);
		}
		if (!list.IsNullOrEmpty())
		{
			BackupTargets = new List<AbilityTarget>();
			foreach (AbilityTarget current in list)
			{
				AbilityTarget copy = current.GetCopy();
				BackupTargets.Add(copy);
			}
		}
	}

	public void DestroyBackupTargetingInfo(bool highlightsOnly)
	{
		if (!BackupTargeterHighlights.IsNullOrEmpty())
		{
			foreach (GameObject current in BackupTargeterHighlights)
			{
				if (current != null)
				{
					HighlightUtils.DestroyObjectAndMaterials(current);
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
		AbilityData abilityData = ActorData != null ? ActorData.GetAbilityData() : null;
		return abilityData != null && abilityData.GetSelectedAbility() == this;
	}

	public bool IsActorInTargetRange(ActorData actor)
	{
		bool inCover;
		return IsActorInTargetRange(actor, out inCover);
	}

	public bool IsActorInTargetRange(ActorData actor, out bool inCover)
	{
		bool isInRange = false;
		inCover = false;
		if (Targeter != null)
		{
			if (GetExpectedNumberOfTargeters() >= 2 && Targeters.Count >= 2)
			{
				inCover = true;
				for (int i = 0; i < Targeters.Count; i++)
				{
					if (isInRange && !inCover)
					{
						break;
					}

					bool inCover2;
					if (Targeters[i] == null || !Targeters[i].IsActorInTargetRange(actor, out inCover2))
					{
						continue;
					}

					isInRange = true;
					if (i == 0)
					{
						inCover = inCover2;
					}
					else
					{
						inCover = inCover && inCover2;
					}
				}
				if (!isInRange)
				{
					inCover = false;
				}
			}
			else
			{
				isInRange = Targeter.IsActorInTargetRange(actor, out inCover);
			}
		}
		else
		{
			Log.Warning(new StringBuilder().Append("Ability ").Append(m_abilityName).Append(" has no targeter, but we're checking actors in its range.").ToString());
			isInRange = Board.Get().PlayerClampedSquare == actor.GetCurrentBoardSquare();
			inCover = false;
		}
		return isInRange;
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

			for (int i = 0; i < Targeters.Count && i <= upToTargeterIndex; i++)
			{
				foreach (AbilityUtil_Targeter.ActorTarget current in Targeters[i].GetActorsInRange())
				{
					if (ActorCountTowardsEnergyGain(current.m_actor, caster)
						&& !hashSet.Contains(current.m_actor.ActorIndex))
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
		return hashSet.Count;
	}

	public virtual TargetData[] GetBaseTargetData()
	{
		return m_targetData;
	}

	public virtual TargetData[] GetTargetData()
	{
		TargetData[] result = GetBaseTargetData();
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_useTargetDataOverrides)
		{
			result = CurrentAbilityMod.m_targetDataOverrides;
		}
		return result;
	}

	public virtual float GetRangeInSquares(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		float range = 0f;
		if (targetData != null && targetData.Length > targetIndex)
		{
			range = targetData[targetIndex].m_range;
		}
		if (CurrentAbilityMod != null)
		{
			range = Mathf.Max(0f, CurrentAbilityMod.m_targetDataMaxRangeMod.GetModifiedValue(range));
		}
		return range;
	}

	public float GetMinRangeInSquares(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		float minRange = 0f;
		if (targetData != null && targetData.Length > targetIndex)
		{
			minRange = targetData[targetIndex].m_minRange;
		}
		if (CurrentAbilityMod != null)
		{
			minRange = Mathf.Max(0f, CurrentAbilityMod.m_targetDataMinRangeMod.GetModifiedValue(minRange));
		}
		return minRange;
	}

	public virtual bool GetCheckLoS(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		bool checkLoS = true;
		if (targetData != null && targetData.Length > targetIndex)
		{
			checkLoS = targetData[targetIndex].m_checkLineOfSight;
			if (CurrentAbilityMod != null)
			{
				checkLoS = CurrentAbilityMod.m_targetDataCheckLosMod.GetModifiedValue(checkLoS);
			}
		}
		return checkLoS;
	}

	public string GetTargetDescription(int targetIndex)
	{
		TargetData[] targetData = GetTargetData();
		if (targetData != null && targetData.Length > targetIndex)
		{
			return new StringBuilder().Append("Select ").Append(targetData[targetIndex].m_description).ToString();
		}
		return null;
	}

	public bool IsAutoSelect()
	{
		TargetData[] targetData = GetTargetData();
		return targetData == null || targetData.Length == 0;
	}

	public virtual bool IsFreeAction()
	{
		bool result = m_freeAction;
		if (CurrentAbilityMod != null)
		{
			result = CurrentAbilityMod.m_isFreeActionMod.GetModifiedValue(m_freeAction);
		}
		return result;
	}

	public virtual bool ShouldRotateToTargetPos()
	{
		bool shouldRotate = !IsSimpleAction();
		if (m_rotationVisibilityMode == RotationVisibilityMode.OnAllyClientOnly)
		{
			shouldRotate = true;
			if (NetworkClient.active)
			{
				ActorData actorData = ActorData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null && actorData != null)
				{
					shouldRotate = (activeOwnedActorData.GetTeam() == actorData.GetTeam());
				}
			}
		}
		else if (m_rotationVisibilityMode == RotationVisibilityMode.Always)
		{
			shouldRotate = true;
		}
		else if (m_rotationVisibilityMode == RotationVisibilityMode.Never)
		{
			shouldRotate = false;
		}
		return shouldRotate;
	}

	public virtual Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		TargetData[] targetData = GetTargetData();
		if (targetData != null
			&& targetData.Length > 0
			&& targetData[0].m_targetingParadigm == TargetingParadigm.BoardSquare)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[0].GridPos);
			if (boardSquareSafe != null)
			{
				return boardSquareSafe.ToVector3();
			}
		}
		return targets[0].FreePos;
	}

	public bool IsSimpleAction()
	{
		return GetTargetData().Length == 0;
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
		if (targetData != null && targetData.Length > targetIndex)
		{
			result = targetData[targetIndex].m_targetingParadigm;
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
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_useStatusWhenRequestedOverride)
		{
			return CurrentAbilityMod.m_statusWhenRequestedOverride;
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
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_useChainAbilityOverrides)
		{
			result = CurrentAbilityMod.m_chainAbilityOverrides;
		}
		return result;
	}

	public bool HasAbilityAsPartOfChain(Ability ability)
	{
		foreach (Ability y in GetChainAbilities())
		{
			if (ability == y)
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool ShouldAutoQueueIfValid()
	{
		bool flag = AbilityUtils.AbilityHasTag(this, AbilityTags.AutoQueueIfValid);
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_autoQueueIfValidMod.operation != AbilityModPropertyBool.ModOp.Ignore)
		{
			return CurrentAbilityMod.m_autoQueueIfValidMod.GetModifiedValue(flag);
		}
		return flag;
	}

	public virtual bool AllowCancelWhenAutoQueued()
	{
		return false;
	}

	public virtual MovementAdjustment GetMovementAdjustment()
	{
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_useMovementAdjustmentOverride)
		{
			return CurrentAbilityMod.m_movementAdjustmentOverride;
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
		return GetMovementType() == ActorData.MovementType.Charge
			|| GetMovementType() == ActorData.MovementType.WaypointFlight;
	}

	internal virtual bool IsTeleport()
	{
		return GetMovementType() == ActorData.MovementType.Teleport
			|| GetMovementType() == ActorData.MovementType.Flight;
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
		if (targets != null
			&& targets.Count > 0
			&& GetRunPriority() == AbilityPriority.Evasion)
		{
			return Board.Get().GetSquare(targets[targets.Count - 1].GridPos);
		}
		return null;
	}

	internal float CalcMovementSpeed(float distance)
	{
		if (m_movementDuration > 0f)
		{
			return distance / m_movementDuration;
		}
		else
		{
			return m_movementSpeed;
		}
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
		if (CurrentAbilityMod != null && CurrentAbilityMod.m_useActionAnimTypeOverride)
		{
			return CurrentAbilityMod.m_actionAnimTypeOverride;
		}
		return m_actionAnimType;
	}

	public virtual ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		return GetActionAnimType();
	}

	public virtual bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool result = animIndex == (int)GetActionAnimType();
		if (!result)
		{
			Ability[] chainAbilities = GetChainAbilities();
			for (int i = 0; i < chainAbilities.Length; i++)
			{
				if (result)
				{
					break;
				}
				result = animIndex == (int)chainAbilities[i].GetActionAnimType();
			}
		}
		return result;
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
		foreach (Ability ability in GetChainAbilities())
		{
			string str = new StringBuilder().Append("Ability '").Append(m_abilityName).Append("'- chain ability '").Append(ability.m_abilityName).Append("' ").ToString();
			if (ability.m_techPointsCost != 0)
			{
				Log.Warning(new StringBuilder().Append(str).Append("has non-zero tech point cost.  Zeroing...").ToString());
				ability.m_techPointsCost = 0;
			}
			if (ability.GetAffectsMovement())
			{
				Log.Warning(new StringBuilder().Append(str).Append("adjusts movement.  Removing...").ToString());
				ability.m_movementAdjustment = MovementAdjustment.FullMovement;
			}
			if (ability.m_cooldown != 0)
			{
				Log.Warning(new StringBuilder().Append(str).Append("has non-zero cooldown.  Zeroing...").ToString());
				ability.m_cooldown = 0;
			}
			if (!ability.IsFreeAction())
			{
				Log.Warning(new StringBuilder().Append(str).Append("is not a free action.  Liberating...").ToString());
				ability.m_freeAction = true;
			}
			if (ability.m_chainAbilities.Length != 0)
			{
				Log.Warning(new StringBuilder().Append(str).Append("has its own chain abilities.  Breaking...").ToString());
				ability.m_chainAbilities = new Ability[0];
			}
			if (RunPriority > ability.RunPriority)
			{
				Log.Warning(new StringBuilder().Append(str).Append("has an earlier run priority than its predecessor.  Make sure chain abilities happen later than the 'master' ability for things to look right.").ToString());
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
		if (abilityMod.GetTargetAbilityType() != GetType())
		{
			Debug.LogError(new StringBuilder().Append("Trying to apply mod to wrong ability type. mod_ability_type: ").Append(abilityMod.GetTargetAbilityType().ToString()).Append(" ability_type: ").Append(GetType().ToString()).ToString());
			return;
		}
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

	protected virtual void OnRemoveAbilityMod()
	{
	}

	public void ClearAbilityMod(ActorData actor)
	{
		if (m_currentAbilityMod == null)
		{
			return;
		}

		m_currentAbilityMod = null;
		OnRemoveAbilityMod();
		ResetNameplateTargetingNumbers();

		if (Application.isEditor)
		{
			Debug.Log(new StringBuilder().Append("Removing mod from ability ").Append(GetDebugIdentifier("orange")).ToString());
		}
	}

	public void DrawGizmos()
	{
		if (Targeter == null || !IsAbilitySelected())
		{
			return;
		}

		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData == null || activeOwnedActorData != ActorData)
		{
			return;
		}

		ActorTurnSM actorTurnSM = activeOwnedActorData.GetActorTurnSM();
		if (actorTurnSM == null || actorTurnSM.CurrentState != TurnStateEnum.TARGETING_ACTION)
		{
			return;
		}

		AbilityTarget currentTarget = AbilityTarget.CreateAbilityTargetFromInterface();
		Targeter.DrawGizmos(currentTarget, activeOwnedActorData);
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
		return GetTargetData() != null
			&& GetTargetData().Length > 0
			&& GetTargetingParadigm(0) != TargetingParadigm.Direction;
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
		bool flag = list != null && list.Count > 0;
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
		PassiveData passiveData = GetComponent<PassiveData>();
		return passiveData != null ? passiveData.GetPassiveOfType(passiveType) : null;
	}

	protected T GetPassiveOfType<T>() where T : Passive
	{
		PassiveData passiveData = GetComponent<PassiveData>();
		return passiveData != null ? passiveData.GetPassiveOfType<T>() : null;
	}

	protected Ability GetAbilityOfType(Type abilityType)
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		return abilityData != null ? abilityData.GetAbilityOfType(abilityType) : null;
	}

	protected T GetAbilityOfType<T>() where T : Ability
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		return abilityData != null ? abilityData.GetAbilityOfType<T>() : null;
	}

	protected AbilityData.ActionType GetActionTypeOfAbility(Ability ability)
	{
		AbilityData component = GetComponent<AbilityData>();
		return component != null
			? component.GetActionTypeOfAbility(ability)
			: AbilityData.ActionType.INVALID_ACTION;
	}

	public string GetBaseAbilityDesc()
	{
		string text = "";
		if (m_techPointInteractions != null)
		{
			if (m_techPointInteractions.Length > 0)
			{
				text += "TechPoint Interactions:\n";
			}
			for (int i = 0; i < m_techPointInteractions.Length; i++)
			{
				text += new StringBuilder().Append("    [").Append(m_techPointInteractions[i].m_type.ToString()).Append("] = ").Append(m_techPointInteractions[i].m_amount).Append("\n").ToString();
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
				text = new StringBuilder().Append(text).Append("    [").Append(m_tags[j].ToString()).Append("]\n").ToString();
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
		return "";
	}

	public static string SetupNoteVarName(string input)
	{
		return new StringBuilder().Append("<color=white>[ ").Append(input).Append(" ]</color>").ToString();
	}

	public string GetDebugIdentifier(string colorString = "")
	{
		string text = new StringBuilder().Append("Ability ").Append(m_abilityName).Append("[ ").Append(GetType().ToString()).Append(" ]").ToString();
		if (colorString.Length > 0)
		{
			return new StringBuilder().Append("<color=").Append(colorString).Append(">").Append(text).Append("</color>").ToString();
		}
		return text;
	}
}
