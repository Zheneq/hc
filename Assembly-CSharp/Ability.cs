// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
//using EffectSystem;
//using Mirror;
//using Talents;
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

	// added in rogues
	//[Tooltip("NPC Brains will add this to the ability score when evaluating what ability to use")]
	//public float m_additionalAIScore;

	private int m_overrideActorDataIndex = ActorData.s_invalidActorIndex;
	private ActorData m_actorData;
	// removed in rogues
	private bool m_searchedForActorData;
	// added in rogues
#if SERVER
	private AbilityData.ActionType m_cachedActionType = AbilityData.ActionType.INVALID_ACTION;
#endif
	// added in rogues
	//private static AbilityTooltipTokenContext s_tooltipContextData = new AbilityTooltipTokenContext();

	private List<AbilityTooltipNumber> m_abilityTooltipNumbers;
	private List<AbilityTooltipNumber> m_nameplateTargetingNumbers;
	// removed in rogues
	private bool m_lastUpdateShowingAffectedSquares;

	[Space(10f)]
	public string m_abilityName = "Base Ability";
	public string m_flavorText = "";
	public bool m_ultimate;
	public string m_previewVideo;

	// added in rogues
	//public List<string> m_scriptTags;

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

	// rogues
	//public bool m_freeActionAllowUseAfterFullAction = true;
	// rogues
	//[Header("New for PvE - can count as either Ability or Move for the turn")]
	//public bool m_quickAction;

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

	// TODO check if needed
	// added in rogues
//#if SERVER
//	public bool m_runChainAbilitiesTheFollowingTurn;  // was public in rogues
//#endif

	[Separator("Targeter Template Visual Swaps", "orange")]
	public List<TargeterTemplateSwapData> m_targeterTemplateSwaps;

	// rogues
	//[Separator("Ability Accuracy Adjust", true)]
	//public int m_baseAccuracyAdjust;
	//[Header("-- Ignore accuracy system (can only do normal hits)")]
	//public bool m_guaranteeHit;
	//[Header("-- Convert misses to glances")]
	//public bool m_convertMissToGlance;
	//[Header("-- Destructible Cover damage - aoe vs blocked shots")]
	//public int m_aoeGeometryDamageAmount;
	//public int m_blockedHitGeometryDamageAmount = 1;
	//[Header("-- Proximity based accuracy adjust override")]
	//public bool m_useProximityAccuAdjustOverride;
	//public ProximityAccuracyAdjustData m_accuAdjustOverrideData;

	private List<AbilityUtil_Targeter> m_targeters = new List<AbilityUtil_Targeter>();
	private List<GameObject> m_backupTargeterHighlights = new List<GameObject>();
	private List<AbilityTarget> m_backupTargets = new List<AbilityTarget>();

	// rogues
	//[Separator("Gear & Equipment", "orange")]
	//public TagGroup[] gearTags = new TagGroup[]
	//{
	//	new TagGroup
	//	{
	//		Negate = true
	//	}
	//};

	private AbilityMod m_currentAbilityMod;

	// rogues
	//private Gear m_currentGear;

	// added in rogues
#if SERVER
	private List<AbilityStatMod> m_statModsFromCurrentMod = new List<AbilityStatMod>();
	private EffectSource m_effectSource;
	internal Dictionary<ActorData, int> m_actorLastHitTurn = new Dictionary<ActorData, int>();
	internal Dictionary<ActorData, int> m_actorLastDamageTurn = new Dictionary<ActorData, int>();
#endif

	protected const string c_forDesignHeader = "<color=cyan>-- For Design --</color>\n";
	protected const string c_forArtHeader = "<color=cyan>-- For Art --</color>\n";

	public Sprite sprite { get; set; }

	protected ActorData ActorData  // public in rogues
	{
		get
		{
			if (m_actorData == null
				 // removed in rogues
				 && !m_searchedForActorData)
			{
				m_actorData = m_overrideActorDataIndex != ActorData.s_invalidActorIndex
					? GameFlowData.Get().FindActorByActorIndex(m_overrideActorDataIndex)
					: GetComponent<ActorData>();
				// removed in rogues
				m_searchedForActorData = true;
			}
			return m_actorData;
		}
	}

	// added in rogues
#if SERVER
	public AbilityData.ActionType CachedActionType
	{
		get
		{
			if (m_cachedActionType == AbilityData.ActionType.INVALID_ACTION)
			{
				AbilityData component = GetComponent<AbilityData>();
				if (component != null)
				{
					m_cachedActionType = component.GetActionTypeOfAbility(this);
				}
			}
			return m_cachedActionType;
		}
	}
#endif

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

	// reactor
	public AbilityMod CurrentAbilityMod => m_currentAbilityMod;
	// rogues
	//public AbilityMod CurrentAbilityMod
	//{
	//	get
	//	{
	//		if (m_currentAbilityMod == null
	//			&& m_actorData != null
	//			&& TalentManager.Get() != null)
	//		{
	//			m_currentAbilityMod = TalentManager.Get().GetAbilityMod(m_actorData.m_characterType, CachedActionType);
	//		}
	//		return m_currentAbilityMod;
	//	}
	//}

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
		// added in rogues
		m_effectSource = new EffectSource(this); // , null in rogues

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

	// reactor, changed in rogues
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

	// reactor, changed in rogues
	public virtual string GetFullTooltip()
	{
		return GetToolTipString();
	}

	// reactor, changed in rogues
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
				// rogues
				//if (Application.isPlaying && m_actorData != null)
				//{
				//	EquipmentStats equipmentStats = m_actorData.GetEquipmentStats();
				//	if (equipmentStats != null)
				//	{
				//		num = Mathf.Max(0, Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.TechPointGenerationAdjustment, (float)num, (int)CachedActionType, null)));
				//	}
				//}
				list.Add(new TooltipTokenInt(techPointInteraction.m_type.ToString(), "Energy Gain", num));
			}
		}
#if PURE_REACTOR
		if (m_techPointsCost > 0)
		{
			list.Add(new TooltipTokenInt("EnergyCost", "Energy Cost", m_techPointsCost));
		}
		if (m_maxStocks > 0)
		{
			list.Add(new TooltipTokenInt("MaxStocks", "Max Stocks/Charges", m_maxStocks));
		}
#else
		// NOTE CHANGE (rogues) were base cost and base stocks in reactor
		if (GetModdedCost() > 0)
		{
			list.Add(new TooltipTokenInt("EnergyCost", "Energy Cost", GetModdedCost()));
		}
		if (GetModdedMaxStocks() > 0)
		{
			list.Add(new TooltipTokenInt("MaxStocks", "Max Stocks/Charges", GetModdedMaxStocks()));
		}
#endif

		return list;
	}

	public virtual void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		List<TooltipTokenEntry> tooltipTokenEntries = GetTooltipTokenEntries(mod);
		// rogues
		//Ability.s_tooltipContextData.m_powerLevel = GetAdjustedPowerLevel(null);
		//Ability.s_tooltipContextData.m_strength = GetAdjustedStrength(null);
		//Ability.s_tooltipContextData.m_expertise = GetAdjustedExpertise(null);
		m_debugUnlocalizedTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(m_toolTip, tooltipTokenEntries); // , Ability.s_tooltipContextData, false in rogues
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
			return $"{GetBaseCost()} energy";
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

	// removed in rogues
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
			if (num > 0f && num < 15f && targetData2.m_targetingParadigm != TargetingParadigm.Direction)
			{
				return true;
			}
		}
		return false;
	}

	// rogues
	//public virtual int GetGeometryDamageAmount(bool blockedHit)
	//{
	//	if (blockedHit)
	//	{
	//		if (!(CurrentAbilityMod != null))
	//		{
	//			return m_blockedHitGeometryDamageAmount;
	//		}
	//		return CurrentAbilityMod.m_geometryDamageAmount.GetModifiedValue(m_blockedHitGeometryDamageAmount);
	//	}
	//	else
	//	{
	//		if (!(CurrentAbilityMod != null))
	//		{
	//			return m_aoeGeometryDamageAmount;
	//		}
	//		return CurrentAbilityMod.m_geometryDamageAmount.GetModifiedValue(m_aoeGeometryDamageAmount);
	//	}
	//}

	public virtual float GetTargetableRadiusInSquares(ActorData caster)
	{
		return AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
	}

	// removed in rogues
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
		if (cooldown >= 0)
		{
			// reactor
			if (m_currentAbilityMod != null)
			{
				cooldown = Mathf.Max(0, m_currentAbilityMod.m_maxCooldownMod.GetModifiedValue(cooldown));
			}
			// rogues
			//if (m_currentGear != null)
			//{
			//	cooldown = Mathf.Max(0, Mathf.RoundToInt(m_actorData.GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.CooldownAdjustment, (float)cooldown, (int)CachedActionType, ActorData)));
			//}
			//else if (CurrentAbilityMod != null)
			//{
			//	cooldown = Mathf.RoundToInt(m_actorData.GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.CooldownAdjustment, (float)CurrentAbilityMod.m_maxCooldownMod.GetModifiedValue(cooldown), (int)CachedActionType, ActorData));
			//}
		}
		return cooldown;
	}

	// removed in rogues
	public virtual int GetCooldownForUIDisplay()
	{
		return GetModdedCooldown();
	}

	// added in rogues
#if SERVER
	public int GetRemainingCooldown()
	{
		AbilityData abilityData = ActorData.GetAbilityData();
		AbilityData.ActionType actionTypeOfAbility = GetActionTypeOfAbility(this);
		return abilityData.GetAbilityEntryOfActionType(actionTypeOfAbility).GetCooldownRemaining();
	}
#endif

	// added in rogues
#if SERVER
	public void SetRemainingCooldown(int remainingCooldown)
	{
		AbilityData abilityData = ActorData.GetAbilityData();
		AbilityData.ActionType actionTypeOfAbility = GetActionTypeOfAbility(this);
		abilityData.GetAbilityEntryOfActionType(actionTypeOfAbility).SetCooldownRemaining(remainingCooldown);
	}
#endif

	// rogues
	//public int GetAdjustedPowerLevel(ActorData targetActor = null)
	//{
	//	if (ActorData != null)
	//	{
	//		return ActorData.GetAdjustedPowerLevel((int)CachedActionType, targetActor);
	//	}
	//	return 0;
	//}

	// rogues
	//public int GetAdjustedStrength(ActorData targetActor = null)
	//{
	//	if (ActorData != null)
	//	{
	//		return ActorData.GetAdjustedStrength((int)CachedActionType, targetActor);
	//	}
	//	return 0;
	//}

	// rogues
	//public int GetAdjustedExpertise(ActorData targetActor = null)
	//{
	//	if (ActorData != null)
	//	{
	//		return ActorData.GetAdjustedExpertise((int)CachedActionType, targetActor);
	//	}
	//	return 0;
	//}

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

		// reactor
		m_toolTipForUI = m_toolTipForUI + "\n" + GetFullTooltip();
		// rogues
		//m_toolTipForUI = m_toolTipForUI + "\n" + GetFullTooltip(GetAdjustedPowerLevel(null), GetAdjustedExpertise(null), GetAdjustedStrength(null));

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

	// added in rogues
#if SERVER
	public virtual void GetAbilityStatusData(out Dictionary<string, string> statusData, bool includeNames = false)
	{
		statusData = new Dictionary<string, string>();
	}
#endif

	// added in rogues, probably rogues-only
	//protected void GatherVisibleStatusTooltips(ref Dictionary<string, string> displayData, List<OnHitEffectTemplateField> fields, bool includeNames = false)
	//{
	//	foreach (OnHitEffectTemplateField onHitEffectTemplateField in fields)
	//	{
	//		List<TooltipTokenEntry> list = new List<TooltipTokenEntry>();
	//		SortedSet<EffectTemplate> searched = new SortedSet<EffectTemplate>();
	//		onHitEffectTemplateField.m_effectTemplate.AddTooltipTokens(searched, list, onHitEffectTemplateField.m_effectTemplate.name, false, null);
	//		AbilityTooltipTokenContext abilityTooltipTokenContext = new AbilityTooltipTokenContext();
	//		abilityTooltipTokenContext.m_powerLevel = GetAdjustedPowerLevel(null);
	//		abilityTooltipTokenContext.m_strength = GetAdjustedStrength(null);
	//		abilityTooltipTokenContext.m_expertise = GetAdjustedExpertise(null);
	//		abilityTooltipTokenContext.actorData = ActorData;
	//		abilityTooltipTokenContext.ability = this;
	//		abilityTooltipTokenContext.gearStatData = null;
	//		abilityTooltipTokenContext.effectTemplate = onHitEffectTemplateField.m_effectTemplate;
	//		abilityTooltipTokenContext.isMatchData = (AppState.IsInGame() || AppState.GetCurrent() == AppState_GameLoading.Get());
	//		foreach (TooltipTokenEntry tooltipTokenEntry in list)
	//		{
	//			if (tooltipTokenEntry is TooltipTokenInt || tooltipTokenEntry is TooltipTokenFloat)
	//			{
	//				abilityTooltipTokenContext.tooltipTokenEntries.Add(tooltipTokenEntry);
	//			}
	//			else if (tooltipTokenEntry is TooltipTokenScript)
	//			{
	//				abilityTooltipTokenContext.tooltipTokenEntries.Add(tooltipTokenEntry);
	//			}
	//		}
	//		string text = onHitEffectTemplateField.m_effectTemplate.LocalizedTooltip;
	//		if (includeNames)
	//		{
	//			text = onHitEffectTemplateField.m_effectTemplate.LocalizedName + ": " + text;
	//		}
	//		string tooltipWithSubstitutes = TooltipTokenEntry.GetTooltipWithSubstitutes(text, list, abilityTooltipTokenContext, false);
	//		if (onHitEffectTemplateField.m_effectTemplate.DisplayAsInGameStatus != EffectTemplate.StatusType.Status_NotDisplayed && !displayData.ContainsKey(tooltipWithSubstitutes))
	//		{
	//			displayData.Add(tooltipWithSubstitutes, onHitEffectTemplateField.m_effectTemplate.IconResource);
	//		}
	//	}
	//}

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
					abilityUtil_Targeter.ResetTargeter(true); // no param in rogues
				}
			}
		}
	}

	private void Update()
	{
		if (Targeter != null && IsAbilitySelected())
		{
			// removed in rogues
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
				// NOTE CHANGE custom fallback
				Team teamViewing = GameFlowData.Get().LocalPlayerData?.GetTeamViewing() ?? Team.Invalid;
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
						// reactor
						if (changedShowAffectedSquares || Targeter.MarkedForForceUpdate || !Targeter.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
						{
							Targeter.MarkedForForceUpdate = false;
							Targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
							Targeter.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
							Targeter.AdjustOpacityWhileTargeting();
							Targeter.SetupTargetingArc(activeOwnedActorData, false);
						}
						// rogues
						//Targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
						//Targeter.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
						//Targeter.AdjustOpacityWhileTargeting();
						//Targeter.SetupTargetingArc(activeOwnedActorData, false);
					}
					else
					{
						int count = actorTurnSM.GetAbilityTargets().Count;
						if (count < Targeters.Count)
						{
							AbilityUtil_Targeter targeter = Targeters[count];

							// reactor
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
							// rogues
							//abilityUtil_Targeter2.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
							//if (abilityUtil_Targeter2.IsUsingMultiTargetUpdate())
							//{
							//	abilityUtil_Targeter2.UpdateTargetingMultiTargets(abilityTargetForTargeterUpdate, ActorData, count, actorTurnSM.GetAbilityTargets());
							//}
							//else
							//{
							//	abilityUtil_Targeter2.UpdateTargeting(abilityTargetForTargeterUpdate, ActorData);
							//}
							//abilityUtil_Targeter2.AdjustOpacityWhileTargeting();
							//abilityUtil_Targeter2.SetupTargetingArc(activeOwnedActorData, false);

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

		// removed in rogues
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

	public bool CanTargetActorInDecision(ActorData caster, ActorData targetActor, bool allowEnemies, bool allowAllies, bool allowSelf, ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false)  // no ignoreLosSettingOnTargetData in rogues
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

		// reactor
		if (!isVisible
			|| ((!isAlly || !allowAllies) && (isAlly || !allowEnemies))
			|| (!allowSelf && caster == targetActor))
		{
			return false;
		}
		// rogues
		//if (!isVisible
		//	|| (isAlly || !allowEnemies)
		//		&& (!isAlly || caster == targetActor || !allowAllies)
		//		&& (!allowSelf || caster != targetActor))
		//{
		//	return false;
		//}

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
			canCharge = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetActor.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare(), passThroughInvalidSquares, out int _);
		}
		return isInRange && isValidTarget && canCharge;
	}

	public bool HasTargetableActorsInDecision(ActorData caster, bool allowEnemies, bool allowAllies, bool allowSelf, Ability.ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false) // no ignoreLosSettingOnTargetData in rogues
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

		// reactor

		List<AbilityTarget> list = new List<AbilityTarget>();
		for (int i = 0; i < Targeters.Count && i < GetExpectedNumberOfTargeters(); i++)
		{
			BackupTargeterHighlights.AddRange(Targeters[i].GetHighlightCopies(true));
			AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromWorldPos(Vector3.zero, Vector3.forward);
			abilityTarget.SetPosAndDir(Targeters[i].LastUpdatingGridPos, Targeters[i].LastUpdateFreePos, Targeters[i].LastUpdateAimDir);
			list.Add(abilityTarget);
		}
		// rogues
		//for (int i = 0; i < Targeters.Count; i++)
		//{
		//	BackupTargeterHighlights.AddRange(Targeters[i].GetHighlightCopies(true));
		//}
		//List<AbilityTarget> list = turnSM.GetAbilityTargets();
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
		AbilityData abilityData = ActorData?.GetAbilityData();
		return abilityData != null && abilityData.GetSelectedAbility() == this;
	}

	// rogues
	//public int GetAccuracyAdjust()
	//{
	//	if (!(CurrentAbilityMod != null))
	//	{
	//		return m_baseAccuracyAdjust;
	//	}
	//	return CurrentAbilityMod.m_accuracyAdjust.GetModifiedValue(m_baseAccuracyAdjust);
	//}

	// rogues
	//public bool IgnoreAccuracySystem()
	//{
	//	if (!(CurrentAbilityMod != null))
	//	{
	//		return m_guaranteeHit;
	//	}
	//	return CurrentAbilityMod.m_guaranteeHit.GetModifiedValue(m_guaranteeHit);
	//}

	// rogues
	//public bool ConvertMissToGlance()
	//{
	//	if (!(CurrentAbilityMod != null))
	//	{
	//		return m_convertMissToGlance;
	//	}
	//	return CurrentAbilityMod.m_convertMissToGlance.GetModifiedValue(m_convertMissToGlance);
	//}

	// rogues
	//public ProximityAccuracyAdjustData GetProximityAccuAdjustData(ActorData actor)
	//{
	//	if (m_useProximityAccuAdjustOverride && m_accuAdjustOverrideData != null)
	//	{
	//		return m_accuAdjustOverrideData;
	//	}
	//	return actor.GetProximityAccuAdjust();
	//}

	// rogues
	//public virtual float CalcDistForAccuracyAdjust(ActorData target, ActorData caster)
	//{
	//	Vector3 vector = target.GetFreePos() - caster.GetFreePos();
	//	vector.y = 0f;
	//	return vector.magnitude / Board.SquareSizeStatic;
	//}

	public bool IsActorInTargetRange(ActorData actor)
	{
		// reactor
		return IsActorInTargetRange(actor, out bool inCover);
		// rogues
		//HitChanceBracketType hitChanceBracketType;
		//return IsActorInTargetRange(actor, out hitChanceBracketType);
	}

	// reactor
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
					if (Targeters[i] == null || !Targeters[i].IsActorInTargetRange(actor, out bool inCover2))
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
			Log.Warning("Ability " + m_abilityName + " has no targeter, but we're checking actors in its range.");
			isInRange = Board.Get().PlayerClampedSquare == actor.GetCurrentBoardSquare();
			inCover = false;
		}
		return isInRange;
	}

	// rogues
	//public bool IsActorInTargetRange(ActorData actor, out HitChanceBracketType strongestCover)
	//{
	//	bool flag = false;
	//	strongestCover = HitChanceBracketType.Default;
	//	if (Targeter != null)
	//	{
	//		if (GetExpectedNumberOfTargeters() < 2 || Targeters.Count < 2)
	//		{
	//			flag = Targeter.IsActorInTargetRange(actor, out strongestCover);
	//		}
	//		else
	//		{
	//			strongestCover = HitChanceBracketType.FullCover;
	//			int num = 0;
	//			while (num < Targeters.Count && (!flag || strongestCover > HitChanceBracketType.Default))
	//			{
	//				HitChanceBracketType hitChanceBracketType;
	//				if (Targeters[num] != null && Targeters[num].IsActorInTargetRange(actor, out hitChanceBracketType))
	//				{
	//					flag = true;
	//					if (num == 0)
	//					{
	//						strongestCover = hitChanceBracketType;
	//					}
	//					else
	//					{
	//						strongestCover = (HitChanceBracketType)Mathf.Max((int)strongestCover, (int)hitChanceBracketType);
	//					}
	//				}
	//				num++;
	//			}
	//			if (!flag)
	//			{
	//				strongestCover = HitChanceBracketType.Default;
	//			}
	//		}
	//	}
	//	else
	//	{
	//		Log.Warning("Ability " + m_abilityName + " has no targeter, but we're checking actors in its range.");
	//		flag = (Board.Get().PlayerClampedSquare == actor.GetCurrentBoardSquare());
	//		strongestCover = HitChanceBracketType.Default;
	//	}
	//	return flag;
	//}

	// removed in rogues
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
			return "Select " + targetData[targetIndex].m_description;
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

	// rogues
	//public virtual bool IsAllowedAfterFullActionIfFreeAction()
	//{
	//	return m_freeActionAllowUseAfterFullAction;
	//}

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

	public virtual List<StatusType> GetStatusToApplyWhenRequested()  // non-virtual in rogues
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

	// added in rogues
#if SERVER
	public virtual void OnAbilityQueuedDuringDecision()
	{
	}

	// added in rogues
	public virtual void OnAbilityUnqueuedDuringDecision()
	{
	}

	// added in rogues
	public virtual List<AbilityData.ActionType> GetOtherActionsToCancelOnAbilityUnqueue(ActorData caster)
	{
		return null;
	}
#endif

	// added in rogues
#if SERVER
	public virtual void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
	}

	// added in rogues
	public virtual void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
	}

	// added in rogues
	public virtual void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
	}

	// added in rogues
	public virtual void OnExecutedActorHit_Barrier(ActorData caster, ActorData target, ActorHitResults results)
	{
	}
#endif

	// added in rogues
#if SERVER
	public virtual void UpdateStatsForActorHit(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (GameplayUtils.IsPlayerControlled(caster) && GameplayUtils.IsPlayerControlled(target) && caster.GetTeam() != target.GetTeam())
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			if (m_actorLastHitTurn.ContainsKey(target))
			{
				m_actorLastHitTurn[target] = currentTurn;
			}
			else
			{
				m_actorLastHitTurn.Add(target, currentTurn);
			}
			if (results.FinalDamage > 0)
			{
				if (m_actorLastDamageTurn.ContainsKey(target))
				{
					m_actorLastDamageTurn[target] = currentTurn;
					return;
				}
				m_actorLastDamageTurn.Add(target, currentTurn);
			}
		}
	}
#endif

#if SERVER
	// added in rogues
	public virtual void OnExecutedPositionHit_Ability(ActorData caster, PositionHitResults results)
	{
	}

	// added in rogues
	public virtual void OnExecutedPositionHit_Effect(ActorData caster, PositionHitResults results)
	{
	}

	// added in rogues
	public virtual void OnExecutedPositionHit_Barrier(ActorData caster, PositionHitResults results)
	{
	}

	// added in rogues
	public virtual bool DidAbilityParticipateInAssist(ActorData target, int oldestAssistTurn)
	{
		return m_actorLastHitTurn.ContainsKey(target) && m_actorLastHitTurn[target] >= oldestAssistTurn;
	}

	// added in rogues
	public virtual bool DidAbilityParticipateInKillingBlow(ActorData target, int oldestKillingBlowTurn)
	{
		return m_actorLastDamageTurn.ContainsKey(target) && m_actorLastDamageTurn[target] >= oldestKillingBlowTurn;
	}

	// added in rogues
	public virtual void OnAbilityAssistedKill(ActorData caster, ActorData target)
	{
	}

	// added in rogues
	public virtual void OnAbilityHadKillingBlow(ActorData caster, ActorData target)
	{
	}

	// added in rogues
	public virtual void OnExecutedAbility(AbilityResults results)
	{
	}

	// added in rogues
	public virtual void OnDodgedDamage(ActorData caster, int damageDodged)
	{
	}

	// added in rogues
	public virtual void OnInterceptedDamage(ActorData caster, int damageIntercepted)
	{
	}

	// added in rogues
	public virtual void OnCalculatedExtraDamageFromEmpoweredGrantedByMyEffect(ActorData effectCaster, ActorData empoweredActor, int extraDamage)
	{
	}

	// added in rogues
	public virtual void OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(ActorData effectCaster, ActorData weakenedActor, int damageReduced)
	{
	}

	// added in rogues
	public virtual void OnCalculatedExtraDamageFromVulnerableGrantedByMyEffect(ActorData effectCaster, ActorData vulnerableActor, int extraDamage)
	{
	}

	// added in rogues
	public virtual void OnCalculatedDamageReducedFromArmoredGrantedByMyEffect(ActorData effectCaster, ActorData armoredActor, int damageReduced)
	{
	}

	// added in rogues
	public virtual void OnEffectAbsorbedDamage(ActorData effectCaster, int damageAbsorbed)
	{
	}

	// added in rogues
	public virtual bool ShouldRevealCasterOnHostileAbilityHit()
	{
		return !m_tags.Contains(AbilityTags.DontRevealCasterOnHostileAbilityHit);
	}

	// added in rogues
	public virtual bool ShouldRevealTargetOnHostileAbilityHit()
	{
		return !m_tags.Contains(AbilityTags.DontRevealTargetOnHostileAbilityHit);
	}

	// added in rogues
	public virtual bool ShouldRevealCasterOnHostileEffectOrBarrierHit()
	{
		return m_tags.Contains(AbilityTags.RevealCasterOnHostileEffectOrBarrierHit);
	}

	// added in rogues
	public virtual bool ShouldRevealTargetOnHostileEffectOrBarrierHit()
	{
		return !m_tags.Contains(AbilityTags.DontRevealTargetOnHostileEffectOrBarrierHit);
	}

	// added in rogues
	public virtual bool ShouldRevealEffectHolderOnHostileEffectHit()
	{
		return !m_tags.Contains(AbilityTags.DontRevealEffectHolderOnHostileEffectHit);
	}

	// added in rogues
	public virtual BoardSquare GetModifiedMoveStartSquare(ActorData caster, List<AbilityTarget> targets)
	{
		return caster.GetCurrentBoardSquare();
	}
#endif

	// removed in rogues
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

	// removed in rogues
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

	// rogues
	//public Gear CurrentAbilityGear
	//{
	//	get
	//	{
	//		return m_currentGear;
	//	}
	//}

	protected virtual void OnApplyAbilityMod(AbilityMod abilityMod)
	{
	}

	public void ApplyAbilityMod(AbilityMod abilityMod, ActorData actor)
	{
		// NOTE ROGUES ability mods
		// added in rogues
		//if (!GameWideData.Get().CanApplyAbilityMods())
		//{
		//	return;
		//}
		// end added in rogues

		// removed in rogues
		if (abilityMod.GetTargetAbilityType() != GetType())
		{
			Debug.LogError("Trying to apply mod to wrong ability type. mod_ability_type: " + abilityMod.GetTargetAbilityType().ToString() + " ability_type: " + GetType().ToString());
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

		// NOTE ROGUES ability mods
		// added in rogues
		//if (m_statModsFromCurrentMod.Count > 0)
		//{
		//	Log.Error("StatMods from previous mod not cleared while trying to apply mod");
		//	m_statModsFromCurrentMod.Clear();
		//}
		//if (abilityMod.m_statModsWhileEquipped != null)
		//{
		//	ActorStats actorStats = actor.GetActorStats();
		//	if (actorStats != null)
		//	{
		//		for (int i = 0; i < abilityMod.m_statModsWhileEquipped.Length; i++)
		//		{
		//			AbilityStatMod shallowCopy = abilityMod.m_statModsWhileEquipped[i].GetShallowCopy();
		//			actorStats.AddStatMod(shallowCopy);
		//			m_statModsFromCurrentMod.Add(shallowCopy);
		//		}
		//	}
		//}
		// end added in rogues

		OnApplyAbilityMod(abilityMod);
		ResetNameplateTargetingNumbers();
	}

	// rogues
	//public void ApplyGear(Gear gear, ActorData actor)
	//{
	//	if (!GameWideData.Get().CanApplyGear())
	//	{
	//		return;
	//	}
	//	ResetAbilityTargeters();
	//	ActorTargeting actorTargeting = actor.GetActorTargeting();
	//	if (actorTargeting != null)
	//	{
	//		actorTargeting.MarkForForceRedraw();
	//	}
	//	ClearGear(actor);
	//	m_currentGear = gear;
	//	EquipmentStats equipmentStats = actor.GetEquipmentStats();
	//	if (equipmentStats != null)
	//	{
	//		foreach (GearStatData gearStatData in gear.DataStats)
	//		{
	//			int num = 0;
	//			foreach (GearStatTypeInfo gearStatTypeInfo in gearStatData.GetStatTypeInfos())
	//			{
	//				float value = gearStatTypeInfo.RatingToPercentageCurve.Evaluate(gearStatData.Ratings[num++]);
	//				if (gearStatData.GetScope() == GearStatScope.Actor)
	//				{
	//					equipmentStats.AddActorStat(gearStatTypeInfo.statType, value, gearStatTypeInfo.statOp, 0, gearStatData.template.ratingScript);
	//				}
	//			}
	//		}
	//	}
	//	foreach (GearStatData gearStatData2 in gear.DataStats)
	//	{
	//		foreach (EffectTemplate effectTemplate in gearStatData2.GetEffectTemplates())
	//		{
	//			EffectSystem.Effect effect = new EffectSystem.Effect(effectTemplate, effectTemplate, new EffectSource(this, gearStatData2), actor.CurrentBoardSquare, actor, actor);
	//			ServerEffectManager.Get().ApplyEffect(effect, 1);
	//		}
	//	}
	//}

	protected virtual void OnRemoveAbilityMod()
	{
	}

	public void ClearAbilityMod(ActorData actor)
	{
		if (m_currentAbilityMod == null)
		{
			return;
		}

		// NOTE ROGUES ability mods
		// added in rogues
		//ActorStats actorStats = actor.GetActorStats();
		//if (actorStats != null)
		//{
		//	foreach (AbilityStatMod statMod in m_statModsFromCurrentMod)
		//	{
		//		actorStats.RemoveStatMod(statMod);
		//	}
		//	m_statModsFromCurrentMod.Clear();
		//}
		// end added in rogues

		m_currentAbilityMod = null;
		OnRemoveAbilityMod();
		ResetNameplateTargetingNumbers();

		// removed in rogues
		if (Application.isEditor)
		{
			Debug.Log("Removing mod from ability " + GetDebugIdentifier("orange"));
		}
	}

	// rogues
	//protected virtual void OnRemoveGear()
	//{
	//}

	// rogues
	//public void ClearGear(ActorData actor)
	//{
	//	if (m_currentGear != null)
	//	{
	//		EquipmentStats equipmentStats = actor.GetEquipmentStats();
	//		if (equipmentStats != null)
	//		{
	//			foreach (GearStatData gearStatData in m_currentGear.DataStats)
	//			{
	//				int num = 0;
	//				foreach (GearStatTypeInfo gearStatTypeInfo in gearStatData.GetStatTypeInfos())
	//				{
	//					float value = gearStatTypeInfo.RatingToPercentageCurve.Evaluate(gearStatData.Ratings[num++]);
	//					if (gearStatData.GetScope() == GearStatScope.Actor)
	//					{
	//						equipmentStats.RemoveActorStat(gearStatTypeInfo.statType, value, gearStatTypeInfo.statOp);
	//					}
	//				}
	//			}
	//		}
	//		OnRemoveGear();
	//		m_currentGear = null;
	//		ResetNameplateTargetingNumbers();
	//	}
	//}

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

	// removed in rogues
	public virtual bool IgnoreCameraFraming()
	{
		return false;
	}

	// removed in rogues
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

	// server-only, added in rogues
#if SERVER
	public Dictionary<ActorData, int> GatherResults_Base(AbilityPriority phaseIndex, List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Log.Info($"Gathering results for {caster.DisplayName}'s {m_abilityName} in phase {phaseIndex}");
		if (additionalData.m_abilityResults.GatheredResults)
		{
			Log.Info($"Already gathered");
			return additionalData.m_abilityResults.DamageResults;
		}
		GatherAbilityResults(targets, caster, ref additionalData.m_abilityResults);
		List<ServerClientUtils.SequenceStartData> list = GetAbilityRunSequenceStartDataList(targets, caster, additionalData);
		if (list == null)
		{
			if (Application.isEditor)
			{
				Debug.LogWarning(GetDebugIdentifier("") + " returned null for Sequence Start Data List");
			}
			list = new List<ServerClientUtils.SequenceStartData>();
		}
		if (CurrentAbilityMod != null)
		{
			float num = GetModdedChanceToTriggerEffects();
			if (ModdedChanceToTriggerEffectsIsPerHit())
			{
				num *= (float)additionalData.m_abilityResults.m_actorToHitResults.Count;
			}
			AppendBaseModData(this, additionalData, ref additionalData.m_abilityResults, list, caster, GetModdedEffectForSelf(), GetModdedEffectForAllies(), GetModdedEffectForEnemies(), GetModdedUseAllyEffectForTargetedCaster(), num, CurrentAbilityMod.m_cooldownReductionsOnSelf, CurrentAbilityMod.m_selfHitTimingSequencePrefab);
		}
		else if (additionalData.m_chainModInfo != null)
		{
			ChainAbilityAdditionalModInfo chainModInfo = additionalData.m_chainModInfo;
			AppendBaseModData(this, additionalData, ref additionalData.m_abilityResults, list, caster, chainModInfo.m_effectOnSelf, chainModInfo.m_effectOnAlly, chainModInfo.m_effectOnEnemy, false, 1f, chainModInfo.m_cooldownReductionsOnSelf, chainModInfo.m_timingSequencePrefab);
		}
		additionalData.m_abilityResults.StoreAbilityRunSequenceStartData(list);
		additionalData.m_abilityResults.FinalizeAbilityResults();
		additionalData.m_abilityResults.GatheredResults = true;
		return additionalData.m_abilityResults.DamageResults;
	}
#endif

	// server-only, added in rogues
#if SERVER
	public Dictionary<ActorData, int> GatherResults_Base_Fake(AbilityPriority phaseIndex, List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (additionalData.m_abilityResults_fake.GatheredResults)
		{
			return additionalData.m_abilityResults_fake.DamageResults;
		}
		GatherAbilityResults(targets, caster, ref additionalData.m_abilityResults_fake);
		if (CurrentAbilityMod != null)
		{
			float num = GetModdedChanceToTriggerEffects();
			if (ModdedChanceToTriggerEffectsIsPerHit())
			{
				num *= (float)additionalData.m_abilityResults_fake.m_actorToHitResults.Count;
			}
			AppendBaseModData(this, additionalData, ref additionalData.m_abilityResults_fake, null, caster, GetModdedEffectForSelf(), GetModdedEffectForAllies(), GetModdedEffectForEnemies(), GetModdedUseAllyEffectForTargetedCaster(), num, CurrentAbilityMod.m_cooldownReductionsOnSelf, CurrentAbilityMod.m_selfHitTimingSequencePrefab);
		}
		else if (additionalData.m_chainModInfo != null)
		{
			ChainAbilityAdditionalModInfo chainModInfo = additionalData.m_chainModInfo;
			AppendBaseModData(this, additionalData, ref additionalData.m_abilityResults_fake, null, caster, chainModInfo.m_effectOnSelf, chainModInfo.m_effectOnAlly, chainModInfo.m_effectOnEnemy, false, 1f, chainModInfo.m_cooldownReductionsOnSelf, chainModInfo.m_timingSequencePrefab);
		}
		additionalData.m_abilityResults_fake.FinalizeAbilityResults();
		additionalData.m_abilityResults_fake.GatheredResults = true;
		return additionalData.m_abilityResults_fake.DamageResults;
	}
#endif

	// server-only, added in rogues
#if SERVER
	private static void AppendBaseModData(Ability ability, ServerAbilityUtils.AbilityRunData additionalData, ref AbilityResults abilityResults, List<ServerClientUtils.SequenceStartData> ssdList, ActorData caster, StandardEffectInfo effectOnSelf, StandardEffectInfo effectOnAlly, StandardEffectInfo effectOnEnemy, bool useAllyEffectForTargetedCaster, float chanceToApplyEffect, AbilityModCooldownReduction cooldownReductions, GameObject timingSequencePrefab)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		foreach (ActorData actorData in abilityResults.m_actorToHitResults.Keys)
		{
			StandardEffectInfo standardEffectInfo = null;
			flag |= actorData == caster;
			if (!(actorData == caster) || useAllyEffectForTargetedCaster)
			{
				if (actorData.GetTeam() == caster.GetTeam())
				{
					standardEffectInfo = effectOnAlly;
					num++;
				}
				else
				{
					standardEffectInfo = effectOnEnemy;
					num2++;
				}
			}
			if (standardEffectInfo != null && standardEffectInfo.m_applyEffect && GameplayRandom.GetUniform() <= (double)chanceToApplyEffect)
			{
				StandardActorEffect effect = new StandardActorEffect(ability.AsEffectSource(), actorData.GetCurrentBoardSquare(), actorData, caster, standardEffectInfo.m_effectData);
				abilityResults.m_actorToHitResults[actorData].AddEffect(effect);
			}
		}
		bool flag2 = effectOnSelf != null && effectOnSelf.m_applyEffect && GameplayRandom.GetUniform() <= (double)chanceToApplyEffect;
		bool flag3 = cooldownReductions.HasCooldownReduction();
		bool flag4 = false;
		//TODO CTF CTC
		//bool flag4 = CollectTheCoins.Get() != null && CollectTheCoins.Get().HasModForAbility(ability, caster);
		if (flag2 || flag3 || flag4)
		{
			ActorHitResults actorHitResults;
			if (flag)
			{
				actorHitResults = abilityResults.m_actorToHitResults[caster];
			}
			else
			{
				actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
				actorHitResults.SetIgnoreTechpointInteractionForHit(true);
				abilityResults.StoreActorHit(actorHitResults);
				if (ssdList != null)
				{
					GameObject gameObject = timingSequencePrefab;
					if (gameObject == null)
					{
						gameObject = SequenceLookup.Get().GetSimpleHitSequencePrefab();
					}
					ssdList.Add(new ServerClientUtils.SequenceStartData(gameObject, caster.GetCurrentBoardSquare(), caster.AsArray(), caster, additionalData.m_sequenceSource, null));
				}
			}
			if (flag2)
			{
				StandardActorEffect effect2 = effectOnSelf.CreateEffect(ability.AsEffectSource(), caster, caster);
				actorHitResults.AddEffect(effect2);
			}
			if (flag3)
			{
				cooldownReductions.AppendCooldownMiscEvents(actorHitResults, flag, num, num2);
			}
			if (flag4)
			{
				// TODO CTF CTC
				//AbilityModCooldownReduction abilityModCooldownReduction = CollectTheCoins.Get().CreateAbilityModCooldownReductionForAbility(ability, caster);
				//if (abilityModCooldownReduction != null)
				//{
				//	abilityModCooldownReduction.AppendCooldownMiscEvents(actorHitResults, flag, num, num2);
				//}
			}
		}
	}
#endif

#if SERVER
	// server-only, added in rogues
	public virtual void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
	}

	// server-only, added in rogues
	protected ActorHitResults MakeActorHitRes(ActorData target, Vector3 hitOrigin)
	{
		return new ActorHitResults(new ActorHitParameters(target, hitOrigin));
	}

	// server-only, added in rogues
	protected PositionHitResults MakePosHitRes(Vector3 position)
	{
		return new PositionHitResults(new PositionHitParameters(position));
	}

	// server-only, added in rogues
	protected void ProcessAndStoreBarrierNonActorHits(List<List<NonActorTargetInfo>> nonActorTargetsList, List<Vector3> posForHitList, ActorData caster, AbilityResults abilityResults)
	{
		List<Barrier> list = new List<Barrier>();
		Dictionary<Vector3, PositionHitResults> dictionary = new Dictionary<Vector3, PositionHitResults>();
		for (int i = 0; i < nonActorTargetsList.Count; i++)
		{
			List<NonActorTargetInfo> list2 = nonActorTargetsList[i];
			Vector3 vector = posForHitList[i];
			for (int j = list2.Count - 1; j >= 0; j--)
			{
				NonActorTargetInfo nonActorTargetInfo = list2[j];
				if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock)
				{
					NonActorTargetInfo_BarrierBlock nonActorTargetInfo_BarrierBlock = nonActorTargetInfo as NonActorTargetInfo_BarrierBlock;
					if (nonActorTargetInfo_BarrierBlock.m_barrier != null && !list.Contains(nonActorTargetInfo_BarrierBlock.m_barrier))
					{
						PositionHitResults posHitRes;
						if (dictionary.ContainsKey(vector))
						{
							posHitRes = dictionary[vector];
						}
						else
						{
							posHitRes = MakePosHitRes(vector);
						}
						nonActorTargetInfo_BarrierBlock.AddPositionReactionHitToAbilityResults(caster, posHitRes, abilityResults, true);
						list.Add(nonActorTargetInfo_BarrierBlock.m_barrier);
					}
					list2.RemoveAt(j);
				}
			}
		}
		foreach (Vector3 key in dictionary.Keys)
		{
			abilityResults.StorePositionHit(dictionary[key]);
		}
	}

	// server-only, added in rogues
	public virtual void OnPhaseStartWhenRequested(List<AbilityTarget> targets, ActorData caster)
	{
	}

	// server-only, added in rogues
	public virtual void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
	}

	// server-only, added in rogues
	public virtual bool ShouldTriggerCooldownOnCast(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return true;
	}

	// server-only, added in rogues
	public virtual bool SkipTheatricsAnimationEntry(ActorData caster)
	{
		return false;
	}

	// server-only, added in rogues
	public virtual bool ShouldBarrierHitThisMover(ActorData mover)
	{
		return true;
	}

	// server-only, added in rogues
	public virtual int GetBarrierDamageForActor(int baseDamage, ActorData mover, Vector3 hitPos, Barrier barrier)
	{
		return baseDamage;
	}

	// server-only, added in rogues
	public virtual List<int> GetAdditionalBrushRegionsToDisrupt(ActorData caster, List<AbilityTarget> targets)
	{
		return null;
	}

	// server-only, added in rogues
	public EffectSource AsEffectSource()
	{
		return m_effectSource;
	}

	// server-only, added in rogues
	public virtual BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return chargeSegments[0].m_pos;
	}

	// server-only, added in rogues
	public virtual Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		Vector3 result = chargeSegments[0].m_pos.ToVector3() - chargeSegments[1].m_pos.ToVector3();
		result.y = 0f;
		result.Normalize();
		return result;
	}

	// server-only, added in rogues
	public virtual bool GetChargeThroughInvalidSquares()
	{
		return false;
	}

	// server-only, added in rogues
	public virtual bool CanChargeThroughInvalidSquaresForDestination()
	{
		return GetChargeThroughInvalidSquares();
	}

	// server-only, added in rogues
	public virtual ServerEvadeUtils.ChargeSegment[] ProcessChargeDodge(List<AbilityTarget> targets, ActorData caster, ServerEvadeUtils.ChargeInfo charge, List<ServerEvadeUtils.EvadeInfo> evades)
	{
		return charge.m_chargeSegments;
	}

	// server-only, added in rogues
	protected float GetEvadeDistance(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		float num = 0f;
		for (int i = 0; i < chargeSegments.Length - 1; i++)
		{
			num += chargeSegments[i].m_pos.HorizontalDistanceOnBoardTo(chargeSegments[i + 1].m_pos);
		}
		return num;
	}

	// server-only, added in rogues
	public BoardSquare GetBounceOffSquare(List<AbilityTarget> targets, ActorData caster, ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		BoardSquare result = null;
		if (chargeSegments[chargeSegments.Length - 1].m_end == BoardSquarePathInfo.ChargeEndType.Recovery && chargeSegments[chargeSegments.Length - 2].m_end == BoardSquarePathInfo.ChargeEndType.Impact)
		{
			result = chargeSegments[chargeSegments.Length - 2].m_pos;
		}
		return result;
	}

	// server-only, added in rogues
	public virtual ServerEvadeUtils.ChargeSegment[] GetChargePath(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[2];
		array[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.Impact
		};
		array[1] = new ServerEvadeUtils.ChargeSegment
		{
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_pos = Board.Get().GetSquare(targets[0].GridPos)
		};
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
		array[0].m_segmentMovementSpeed = segmentMovementSpeed;
		array[1].m_segmentMovementSpeed = segmentMovementSpeed;
		return array;
	}

	// server-only, added in rogues
	public virtual BoardSquare GetIdealDestination(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return Board.Get().GetSquare(targets[0].GridPos);
	}

	// server-only, added in rogues
	internal virtual Vector3 GetFacingDirAfterMovement(ServerEvadeUtils.EvadeInfo evade)
	{
		return Vector3.zero;
	}

	// server-only, added in rogues
	internal virtual List<ServerEvadeUtils.NonPlayerEvadeData> GetNonPlayerEvades(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return null;
	}

	// server-only, added in rogues
	public virtual ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return null;
	}

	// server-only, added in rogues
	public virtual List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData abilityRunSequenceStartData = GetAbilityRunSequenceStartData(targets, caster, additionalData);
		if (abilityRunSequenceStartData != null)
		{
			list.Add(abilityRunSequenceStartData);
		}
		return list;
	}
#endif

	protected Passive GetPassiveOfType(Type passiveType)
	{
		return GetComponent<PassiveData>()?.GetPassiveOfType(passiveType);
	}

	protected T GetPassiveOfType<T>() where T : Passive
	{
		return GetComponent<PassiveData>()?.GetPassiveOfType<T>();
	}

	protected Ability GetAbilityOfType(Type abilityType)
	{
		return GetComponent<AbilityData>()?.GetAbilityOfType(abilityType);
	}

	protected T GetAbilityOfType<T>() where T : Ability
	{
		return GetComponent<AbilityData>()?.GetAbilityOfType<T>();
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
				text += "    [" + m_techPointInteractions[i].m_type.ToString() + "] = " + m_techPointInteractions[i].m_amount + "\n";
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
		return "";
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
			return "<color=" + colorString + ">" + text + "</color>";
		}
		return text;
	}

	// added in rogues
	//public bool HasScriptTags(List<string> tags)
	//{
	//	return tags.Except(m_scriptTags).Count<string>() == 0;
	//}

	// added in rogues
#if SERVER
	public virtual List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return new List<ActorData>();
	}
#endif
}
