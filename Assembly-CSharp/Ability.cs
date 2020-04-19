using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ability : MonoBehaviour
{
	public const string c_abilityPreviewVideoPath = "Video/AbilityPreviews/";

	[Tooltip("Description of ability, to display in game", order = 0)]
	[TextArea(1, 0x14, order = 1)]
	public string m_toolTip;

	[HideInInspector]
	public string m_debugUnlocalizedTooltip = string.Empty;

	[HideInInspector]
	public List<StatusType> m_savedStatusTypesForTooltips;

	[TextArea(1, 0x14, order = 1)]
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
	public Ability.RotationVisibilityMode m_rotationVisibilityMode;

	[Separator("Movement", "orange")]
	public Ability.MovementAdjustment m_movementAdjustment = Ability.MovementAdjustment.ReducedMovement;

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
			if (this.m_actorData == null && !this.m_searchedForActorData)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.get_ActorData()).MethodHandle;
				}
				this.m_actorData = ((this.m_overrideActorDataIndex != ActorData.s_invalidActorIndex) ? GameFlowData.Get().FindActorByActorIndex(this.m_overrideActorDataIndex) : base.GetComponent<ActorData>());
				this.m_searchedForActorData = true;
			}
			return this.m_actorData;
		}
	}

	private void Awake()
	{
		this.InitializeAbility();
		this.SanitizeChainAbilities();
	}

	private void OnDestroy()
	{
		this.ClearTargeters();
	}

	public void InitializeAbility()
	{
		this.RebuildTooltipForUI();
		this.ResetNameplateTargetingNumbers();
	}

	protected void ClearTargeters()
	{
		if (this.Targeters != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.ClearTargeters()).MethodHandle;
			}
			this.ResetAbilityTargeters();
			this.Targeters.Clear();
		}
	}

	public void OverrideActorDataIndex(int actorDataIndex)
	{
		this.m_overrideActorDataIndex = actorDataIndex;
		this.m_actorData = null;
	}

	public string GetToolTipString(bool shortTooltip = false)
	{
		if (shortTooltip)
		{
			return this.m_shortToolTip;
		}
		List<TooltipTokenEntry> tooltipTokenEntries = this.GetTooltipTokenEntries(null);
		string text = StringUtil.TR_AbilityFinalFullTooltip(base.GetType().ToString(), this.m_abilityName);
		if (text.Length == 0 && this.m_toolTip.Length > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetToolTipString(bool)).MethodHandle;
			}
			text = this.m_toolTip;
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(text, tooltipTokenEntries, false);
	}

	public virtual string GetFullTooltip()
	{
		return this.GetToolTipString(false);
	}

	public virtual string GetUnlocalizedFullTooltip()
	{
		if (string.IsNullOrEmpty(this.m_debugUnlocalizedTooltip))
		{
			List<TooltipTokenEntry> tooltipTokenEntries = this.GetTooltipTokenEntries(null);
			return TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_toolTip, tooltipTokenEntries, false);
		}
		return this.m_debugUnlocalizedTooltip;
	}

	protected virtual void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public List<TooltipTokenEntry> GetTooltipTokenEntries(AbilityMod mod = null)
	{
		List<TooltipTokenEntry> list = new List<TooltipTokenEntry>();
		this.AddSpecificTooltipTokens(list, mod);
		if (this.m_techPointInteractions != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetTooltipTokenEntries(AbilityMod)).MethodHandle;
			}
			if (this.m_techPointInteractions.Length > 0)
			{
				foreach (TechPointInteraction techPointInteraction in this.m_techPointInteractions)
				{
					int num = techPointInteraction.m_amount;
					if (mod != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						num = mod.GetModdedTechPointForInteraction(techPointInteraction.m_type, num);
					}
					list.Add(new TooltipTokenInt(techPointInteraction.m_type.ToString(), "Energy Gain", num));
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (this.m_techPointsCost > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			list.Add(new TooltipTokenInt("EnergyCost", "Energy Cost", this.m_techPointsCost));
		}
		if (this.m_maxStocks > 0)
		{
			list.Add(new TooltipTokenInt("MaxStocks", "Max Stocks/Charges", this.m_maxStocks));
		}
		return list;
	}

	public virtual void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		List<TooltipTokenEntry> tooltipTokenEntries = this.GetTooltipTokenEntries(mod);
		this.m_debugUnlocalizedTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_toolTip, tooltipTokenEntries, false);
		this.m_savedStatusTypesForTooltips = TooltipTokenEntry.GetStatusTypesFromTooltip(this.m_toolTip);
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.AddTokenFloat(List<TooltipTokenEntry>, string, string, float, bool)).MethodHandle;
			}
			if (val <= 0f)
			{
				return;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.AddTokenFloatAsPct(List<TooltipTokenEntry>, string, string, float, bool)).MethodHandle;
			}
			if (val <= 0f)
			{
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int val2 = Mathf.RoundToInt(val * 100f);
		tokens.Add(new TooltipTokenPct(name, desc, val2));
	}

	public AbilityPriority RunPriority
	{
		get
		{
			return this.GetRunPriority();
		}
		private set
		{
			this.m_runPriority = value;
		}
	}

	public virtual AbilityPriority GetRunPriority()
	{
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetRunPriority()).MethodHandle;
			}
			if (this.m_currentAbilityMod.m_useRunPriorityOverride)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_currentAbilityMod.m_runPriorityOverride;
			}
		}
		return this.m_runPriority;
	}

	public virtual bool CanRunInPhase(AbilityPriority phase)
	{
		return phase == this.RunPriority;
	}

	public string GetRewardString()
	{
		return StringUtil.TR_AbilityReward(base.GetType().ToString(), this.m_abilityName);
	}

	public string GetPhaseStringUnLocalized()
	{
		UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.RunPriority);
		string result = uiphaseFromAbilityPriority.ToString();
		switch (uiphaseFromAbilityPriority)
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
		UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.RunPriority);
		string result = uiphaseFromAbilityPriority.ToString();
		switch (uiphaseFromAbilityPriority)
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
		if (this.GetBaseCost() > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetCostString()).MethodHandle;
			}
			return string.Format("{0} energy", this.GetBaseCost());
		}
		return string.Empty;
	}

	public int GetBaseCost()
	{
		int result;
		if (this.m_techPointsCost > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetBaseCost()).MethodHandle;
			}
			result = this.m_techPointsCost;
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public virtual int GetModdedCost()
	{
		int num = this.GetBaseCost();
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetModdedCost()).MethodHandle;
			}
			num = Mathf.Max(0, this.m_currentAbilityMod.m_techPointCostMod.GetModifiedValue(num));
		}
		return num;
	}

	public virtual TechPointInteraction[] GetBaseTechPointInteractions()
	{
		return this.m_techPointInteractions;
	}

	public string GetNameString()
	{
		string text = StringUtil.TR_AbilityName(base.GetType().ToString(), this.m_abilityName);
		if (text.Length == 0 && this.m_abilityName.Length > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetNameString()).MethodHandle;
			}
			text = this.m_abilityName;
		}
		return text;
	}

	public string GetCooldownString()
	{
		return string.Format("{0} turn cooldown", this.m_cooldown);
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
		TargetData[] targetData = this.GetTargetData();
		if (targetData.Length > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.CanShowTargetableRadiusPreview()).MethodHandle;
			}
			TargetData targetData2 = targetData[0];
			float num = Mathf.Max(0f, targetData2.m_range - 0.5f);
			if (num > 0f)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num < 15f)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (targetData2.m_targetingParadigm != Ability.TargetingParadigm.Direction)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						return true;
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
		return this.CanShowTargetableRadiusPreview() && AbilityUtils.AbilityHasTag(this, AbilityTags.Targeter_ShowRangeWhileTargeting);
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
		overridePos = aimingActor.\u0016();
		return false;
	}

	public virtual int GetBaseCooldown()
	{
		return this.m_cooldown;
	}

	public int GetModdedCooldown()
	{
		int num = this.GetBaseCooldown();
		if (num >= 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetModdedCooldown()).MethodHandle;
			}
			if (this.m_currentAbilityMod != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = Mathf.Max(0, this.m_currentAbilityMod.m_maxCooldownMod.GetModifiedValue(num));
			}
		}
		return num;
	}

	public virtual int GetCooldownForUIDisplay()
	{
		return this.GetModdedCooldown();
	}

	public StandardEffectInfo GetModdedEffectForEnemies()
	{
		if (this.m_currentAbilityMod != null)
		{
			return this.m_currentAbilityMod.m_effectToTargetEnemyOnHit;
		}
		return null;
	}

	public StandardEffectInfo GetModdedEffectForAllies()
	{
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetModdedEffectForAllies()).MethodHandle;
			}
			return this.m_currentAbilityMod.m_effectToTargetAllyOnHit;
		}
		return null;
	}

	public StandardEffectInfo GetModdedEffectForSelf()
	{
		if (this.m_currentAbilityMod != null)
		{
			return this.m_currentAbilityMod.m_effectToSelfOnCast;
		}
		return null;
	}

	public bool HasSelfEffectFromBaseMod()
	{
		StandardEffectInfo moddedEffectForSelf = this.GetModdedEffectForSelf();
		bool result;
		if (moddedEffectForSelf != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.HasSelfEffectFromBaseMod()).MethodHandle;
			}
			result = moddedEffectForSelf.m_applyEffect;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool GetModdedUseAllyEffectForTargetedCaster()
	{
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetModdedUseAllyEffectForTargetedCaster()).MethodHandle;
			}
			return this.m_currentAbilityMod.m_useAllyEffectForTargetedCaster;
		}
		return false;
	}

	public float GetModdedChanceToTriggerEffects()
	{
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetModdedChanceToTriggerEffects()).MethodHandle;
			}
			return this.m_currentAbilityMod.m_effectTriggerChance;
		}
		return 1f;
	}

	public bool ModdedChanceToTriggerEffectsIsPerHit()
	{
		return this.m_currentAbilityMod != null && this.m_currentAbilityMod.m_effectTriggerChanceMultipliedPerHit;
	}

	public int GetBaseMaxStocks()
	{
		return this.m_maxStocks;
	}

	public int GetModdedMaxStocks()
	{
		int num = this.m_maxStocks;
		if (this.m_maxStocks >= 0 && this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetModdedMaxStocks()).MethodHandle;
			}
			num = Mathf.Max(0, this.m_currentAbilityMod.m_maxStocksMod.GetModifiedValue(num));
		}
		return num;
	}

	public bool RefillAllStockOnRefresh()
	{
		bool result;
		if (this.m_currentAbilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.RefillAllStockOnRefresh()).MethodHandle;
			}
			result = this.m_currentAbilityMod.m_refillAllStockOnRefreshMod.GetModifiedValue(this.m_refillAllStockOnRefresh);
		}
		else
		{
			result = this.m_refillAllStockOnRefresh;
		}
		return result;
	}

	public int GetBaseStockRefreshDuration()
	{
		return this.m_stockRefreshDuration;
	}

	public int GetModdedStockRefreshDuration()
	{
		int num = this.m_stockRefreshDuration;
		if (this.m_currentAbilityMod != null)
		{
			num = Mathf.Max(-1, this.m_currentAbilityMod.m_stockRefreshDurationMod.GetModifiedValue(num));
		}
		return num;
	}

	private void RebuildTooltipForUI()
	{
		this.m_toolTipForUI = this.GetNameString();
		if (this.GetBaseCost() > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.RebuildTooltipForUI()).MethodHandle;
			}
			this.m_toolTipForUI += string.Format(" - {0} TP", this.GetBaseCost());
		}
		if (this.m_cooldown > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_toolTipForUI += string.Format(" - {0} turn cooldown", this.m_cooldown);
		}
		if (this.IsFreeAction())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_toolTipForUI = "This is a Free Action.\n" + this.m_toolTipForUI;
		}
		this.m_toolTipForUI = this.m_toolTipForUI + "\n" + this.GetFullTooltip();
		this.m_abilityTooltipNumbers = this.BaseCalculateAbilityTooltipNumbers();
	}

	public void SetTooltip()
	{
		this.RebuildTooltipForUI();
	}

	public List<AbilityTooltipNumber> GetAbilityTooltipNumbers()
	{
		return this.m_abilityTooltipNumbers;
	}

	protected List<AbilityTooltipNumber> BaseCalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = this.CalculateAbilityTooltipNumbers();
		this.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return result;
	}

	protected unsafe void AppendTooltipNumbersFromBaseModEffects(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemyEffectSubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allyEffectSubject = AbilityTooltipSubject.Ally, AbilityTooltipSubject selfEffectSubject = AbilityTooltipSubject.Self)
	{
		if (this.m_currentAbilityMod != null)
		{
			if (enemyEffectSubject != AbilityTooltipSubject.None)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.AppendTooltipNumbersFromBaseModEffects(List<AbilityTooltipNumber>*, AbilityTooltipSubject, AbilityTooltipSubject, AbilityTooltipSubject)).MethodHandle;
				}
				StandardEffectInfo moddedEffectForEnemies = this.GetModdedEffectForEnemies();
				if (moddedEffectForEnemies != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					moddedEffectForEnemies.ReportAbilityTooltipNumbers(ref numbers, enemyEffectSubject);
				}
			}
			StandardEffectInfo moddedEffectForAllies = this.GetModdedEffectForAllies();
			StandardEffectInfo moddedEffectForSelf = this.GetModdedEffectForSelf();
			if (allyEffectSubject != AbilityTooltipSubject.None)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (moddedEffectForAllies != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, allyEffectSubject);
				}
			}
			if (selfEffectSubject != AbilityTooltipSubject.None)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_currentAbilityMod.m_useAllyEffectForTargetedCaster && moddedEffectForAllies != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, selfEffectSubject);
				}
				if (moddedEffectForSelf != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					moddedEffectForSelf.ReportAbilityTooltipNumbers(ref numbers, selfEffectSubject);
				}
			}
		}
	}

	protected virtual List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public virtual List<int> \u001D()
	{
		List<int> list = new List<int>();
		List<AbilityTooltipNumber> list2 = this.BaseCalculateAbilityTooltipNumbers();
		if (list2 != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.\u001D()).MethodHandle;
			}
			foreach (AbilityTooltipNumber abilityTooltipNumber in list2)
			{
				list.Add(abilityTooltipNumber.m_value);
			}
		}
		return list;
	}

	public virtual List<StatusType> GetStatusTypesForTooltip()
	{
		if (this.m_savedStatusTypesForTooltips != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetStatusTypesForTooltip()).MethodHandle;
			}
			if (this.m_savedStatusTypesForTooltips.Count != 0)
			{
				return this.m_savedStatusTypesForTooltips;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return TooltipTokenEntry.GetStatusTypesFromTooltip(this.m_toolTip);
	}

	public List<AbilityTooltipNumber> GetNameplateTargetingNumbers()
	{
		return this.m_nameplateTargetingNumbers;
	}

	protected virtual List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return this.BaseCalculateAbilityTooltipNumbers();
	}

	public void ResetNameplateTargetingNumbers()
	{
		this.m_nameplateTargetingNumbers = this.CalculateNameplateTargetingNumbers();
	}

	protected void ResetTooltipAndTargetingNumbers()
	{
		this.SetTooltip();
		this.ResetNameplateTargetingNumbers();
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

	public unsafe static void AddNameplateValueForSingleHit(ref Dictionary<AbilityTooltipSymbol, int> symbolToValue, AbilityUtil_Targeter targeter, ActorData targetActor, int damageAmount, AbilityTooltipSymbol symbolOfInterest = AbilityTooltipSymbol.Damage, AbilityTooltipSubject targetSubject = AbilityTooltipSubject.Primary)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.AddNameplateValueForSingleHit(Dictionary<AbilityTooltipSymbol, int>*, AbilityUtil_Targeter, ActorData, int, AbilityTooltipSymbol, AbilityTooltipSubject)).MethodHandle;
			}
			if (tooltipSubjectTypes.Count > 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (tooltipSubjectTypes.Contains(targetSubject))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					symbolToValue[symbolOfInterest] = damageAmount;
				}
			}
		}
	}

	public unsafe static void AddNameplateValueForOverlap(ref Dictionary<AbilityTooltipSymbol, int> symbolToValue, AbilityUtil_Targeter targeter, ActorData targetActor, int currentTargeterIndex, int firstAmount, int subsequentAmount, AbilityTooltipSymbol symbolOfInterest = AbilityTooltipSymbol.Damage, AbilityTooltipSubject targetSubject = AbilityTooltipSubject.Primary)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.AddNameplateValueForOverlap(Dictionary<AbilityTooltipSymbol, int>*, AbilityUtil_Targeter, ActorData, int, int, int, AbilityTooltipSymbol, AbilityTooltipSubject)).MethodHandle;
			}
			if (tooltipSubjectTypes.Count > 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				using (List<AbilityTooltipSubject>.Enumerator enumerator = tooltipSubjectTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityTooltipSubject abilityTooltipSubject = enumerator.Current;
						if (abilityTooltipSubject == targetSubject)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!symbolToValue.ContainsKey(symbolOfInterest))
							{
								symbolToValue[symbolOfInterest] = firstAmount;
							}
							else
							{
								Dictionary<AbilityTooltipSymbol, int> dictionary;
								(dictionary = symbolToValue)[symbolOfInterest] = dictionary[symbolOfInterest] + subsequentAmount;
							}
						}
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
	}

	public void ResetAbilityTargeters()
	{
		if (this.Targeters != null)
		{
			for (int i = 0; i < this.Targeters.Count; i++)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = this.Targeters[i];
				if (abilityUtil_Targeter != null)
				{
					abilityUtil_Targeter.ResetTargeter(true);
				}
			}
		}
	}

	private void Update()
	{
		if (this.Targeter != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.Update()).MethodHandle;
			}
			if (this.IsAbilitySelected())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = HighlightUtils.Get() != null && HighlightUtils.Get().m_cachedShouldShowAffectedSquares;
				bool flag2 = this.m_lastUpdateShowingAffectedSquares != flag;
				this.m_lastUpdateShowingAffectedSquares = flag;
				bool flag3 = false;
				if (this.ActorData == GameFlowData.Get().activeOwnedActorData)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					flag3 = true;
				}
				if (GameFlowData.Get().activeOwnedActorData == null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					if (teamViewing != Team.Invalid)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (teamViewing != this.ActorData.\u000E())
						{
							goto IL_ED;
						}
					}
					flag3 = true;
				}
				IL_ED:
				if (flag3)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					ActorTurnSM actorTurnSM = this.ActorData.\u000E();
					if (actorTurnSM != null && actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						AbilityTarget.UpdateAbilityTargetForForTargeterUpdate();
						AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
						if (this.GetExpectedNumberOfTargeters() < 2)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!flag2)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!this.Targeter.MarkedForForceUpdate)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.Targeter.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
									{
										goto IL_1FC;
									}
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
								}
							}
							this.Targeter.MarkedForForceUpdate = false;
							this.Targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
							this.Targeter.UpdateTargeting(abilityTargetForTargeterUpdate, this.ActorData);
							this.Targeter.AdjustOpacityWhileTargeting();
							this.Targeter.SetupTargetingArc(activeOwnedActorData, false);
							IL_1FC:;
						}
						else
						{
							int count = actorTurnSM.GetAbilityTargets().Count;
							if (count < this.Targeters.Count)
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								AbilityUtil_Targeter abilityUtil_Targeter = this.Targeters[count];
								if (!flag2)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!this.Targeters[0].MarkedForForceUpdate)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (abilityUtil_Targeter.IsCursorStateSameAsLastUpdate(abilityTargetForTargeterUpdate))
										{
											goto IL_2FA;
										}
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
									}
								}
								this.Targeters[0].MarkedForForceUpdate = false;
								abilityUtil_Targeter.SetLastUpdateCursorState(abilityTargetForTargeterUpdate);
								if (abilityUtil_Targeter.IsUsingMultiTargetUpdate())
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									abilityUtil_Targeter.UpdateTargetingMultiTargets(abilityTargetForTargeterUpdate, this.ActorData, count, actorTurnSM.GetAbilityTargets());
								}
								else
								{
									abilityUtil_Targeter.UpdateTargeting(abilityTargetForTargeterUpdate, this.ActorData);
								}
								abilityUtil_Targeter.AdjustOpacityWhileTargeting();
								abilityUtil_Targeter.SetupTargetingArc(activeOwnedActorData, false);
								IL_2FA:
								if (HighlightUtils.Get().GetCurrentCursorType() != abilityUtil_Targeter.GetCursorType())
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									HighlightUtils.Get().SetCursorType(abilityUtil_Targeter.GetCursorType());
								}
							}
						}
						this.Targeter.UpdateArrowsForUI();
					}
				}
			}
		}
		if (this.Targeters != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.ActorData != null)
			{
				for (int i = 0; i < this.Targeters.Count; i++)
				{
					AbilityUtil_Targeter abilityUtil_Targeter2 = this.Targeters[i];
					if (abilityUtil_Targeter2 != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						abilityUtil_Targeter2.UpdateFadeOutHighlights(this.ActorData);
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
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

	public bool CanTargetActorInDecision(ActorData caster, ActorData targetActor, bool allowEnemies, bool allowAllies, bool allowSelf, Ability.ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false)
	{
		if (caster != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.CanTargetActorInDecision(ActorData, ActorData, bool, bool, bool, Ability.ValidateCheckPath, bool, bool, bool)).MethodHandle;
			}
			if (!caster.\u000E() && caster.\u0012() != null && targetActor != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!targetActor.\u000E())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (targetActor.\u0012() != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!targetActor.IgnoreForAbilityHits)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							BoardSquare boardSquare = targetActor.\u0012();
							bool flag = (!NetworkClient.active) ? targetActor.\u000E(caster, false) : targetActor.\u0018();
							bool flag2 = caster.\u000E() == targetActor.\u000E();
							if (flag)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (flag2)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (allowAllies)
									{
										goto IL_12B;
									}
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								if (flag2)
								{
									return false;
								}
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!allowEnemies)
								{
									return false;
								}
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								IL_12B:
								if (!allowSelf)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!(caster != targetActor))
									{
										return false;
									}
								}
								float currentMinRangeInSquares = AbilityUtils.GetCurrentMinRangeInSquares(this, caster, 0);
								float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
								bool flag3 = caster.\u000E().IsTargetSquareInRangeOfAbilityFromSquare(targetActor.\u0012(), caster.\u0012(), currentRangeInSquares, currentMinRangeInSquares);
								if (checkLineOfSight)
								{
									if (ignoreLosSettingOnTargetData)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										flag3 = (flag3 && caster.\u0012().\u0013(boardSquare.x, boardSquare.y));
									}
									else
									{
										flag3 = (flag3 && (!this.GetCheckLoS(0) || caster.\u0012().\u0013(boardSquare.x, boardSquare.y)));
									}
								}
								bool flag4 = true;
								ActorStatus actorStatus = targetActor.\u000E();
								if (checkStatusImmunities && actorStatus != null)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (flag2)
									{
										for (;;)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										if (actorStatus.HasStatus(StatusType.CantBeHelpedByTeam, true))
										{
											goto IL_29E;
										}
									}
									if (flag2)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (actorStatus.HasStatus(StatusType.BuffImmune, true))
										{
											goto IL_29E;
										}
										for (;;)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (!flag2)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										if (actorStatus.HasStatus(StatusType.DebuffImmune, true))
										{
											goto IL_29E;
										}
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									bool flag5;
									if (!actorStatus.HasStatus(StatusType.CantBeTargeted, true))
									{
										flag5 = !actorStatus.HasStatus(StatusType.EffectImmune, true);
										goto IL_29F;
									}
									IL_29E:
									flag5 = false;
									IL_29F:
									flag4 = flag5;
								}
								bool result = true;
								if (checkPath != Ability.ValidateCheckPath.Ignore)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (flag3)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (flag4)
										{
											bool passThroughInvalidSquares = checkPath == Ability.ValidateCheckPath.CanBuildPathAllowThroughInvalid;
											int num;
											result = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetActor.\u0012(), caster.\u0012(), passThroughInvalidSquares, out num);
										}
									}
								}
								if (flag3)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (flag4)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										return result;
									}
								}
								return false;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public bool HasTargetableActorsInDecision(ActorData caster, bool allowEnemies, bool allowAllies, bool allowSelf, Ability.ValidateCheckPath checkPath, bool checkLineOfSight, bool checkStatusImmunities, bool ignoreLosSettingOnTargetData = false)
	{
		if (GameFlowData.Get() != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.HasTargetableActorsInDecision(ActorData, bool, bool, bool, Ability.ValidateCheckPath, bool, bool, bool)).MethodHandle;
			}
			List<ActorData> actorsVisibleToActor;
			if (NetworkServer.active)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster, true);
			}
			else
			{
				actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
			}
			List<ActorData> list = actorsVisibleToActor;
			for (int i = 0; i < list.Count; i++)
			{
				ActorData targetActor = list[i];
				if (this.CanTargetActorInDecision(caster, targetActor, allowEnemies, allowAllies, allowSelf, checkPath, checkLineOfSight, checkStatusImmunities, ignoreLosSettingOnTargetData))
				{
					return true;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public void OnAbilitySelect()
	{
		if (this.Targeter != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.OnAbilitySelect()).MethodHandle;
			}
			this.Targeter.TargeterAbilitySelected();
		}
	}

	public void OnAbilityDeselect()
	{
		if (this.Targeter != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.OnAbilityDeselect()).MethodHandle;
			}
			if (this.GetExpectedNumberOfTargeters() < 2)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.Targeter.TargeterAbilityDeselected(0);
			}
			else
			{
				for (int i = 0; i < this.Targeters.Count; i++)
				{
					if (this.Targeters[i] != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.Targeters[i].TargeterAbilityDeselected(i);
					}
				}
			}
			this.Targeter.HideAllSquareIndicators();
			this.DestroyBackupTargetingInfo(true);
		}
	}

	public void BackupTargetingForRedo(ActorTurnSM turnSM)
	{
		this.BackupTargeterHighlights = new List<GameObject>();
		List<AbilityTarget> list = new List<AbilityTarget>();
		int i = 0;
		while (i < this.Targeters.Count)
		{
			if (i >= this.GetExpectedNumberOfTargeters())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.BackupTargetingForRedo(ActorTurnSM)).MethodHandle;
					break;
				}
				break;
			}
			else
			{
				this.BackupTargeterHighlights.AddRange(this.Targeters[i].GetHighlightCopies(true));
				AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromWorldPos(Vector3.zero, Vector3.forward);
				abilityTarget.SetPosAndDir(this.Targeters[i].LastUpdatingGridPos, this.Targeters[i].LastUpdateFreePos, this.Targeters[i].LastUpdateAimDir);
				list.Add(abilityTarget);
				i++;
			}
		}
		if (!list.IsNullOrEmpty<AbilityTarget>())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.BackupTargets = new List<AbilityTarget>();
			using (List<AbilityTarget>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityTarget abilityTarget2 = enumerator.Current;
					AbilityTarget copy = abilityTarget2.GetCopy();
					this.BackupTargets.Add(copy);
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	public void DestroyBackupTargetingInfo(bool highlightsOnly)
	{
		if (!this.BackupTargeterHighlights.IsNullOrEmpty<GameObject>())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.DestroyBackupTargetingInfo(bool)).MethodHandle;
			}
			using (List<GameObject>.Enumerator enumerator = this.BackupTargeterHighlights.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						HighlightUtils.DestroyObjectAndMaterials(gameObject);
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		this.BackupTargeterHighlights = null;
		if (!highlightsOnly)
		{
			this.BackupTargets = null;
		}
	}

	public bool IsAbilitySelected()
	{
		AbilityData abilityData;
		if (this.ActorData == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.IsAbilitySelected()).MethodHandle;
			}
			abilityData = null;
		}
		else
		{
			abilityData = this.ActorData.\u000E();
		}
		AbilityData abilityData2 = abilityData;
		bool result;
		if (abilityData2 != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (abilityData2.GetSelectedAbility() == this);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public List<AbilityUtil_Targeter> Targeters
	{
		get
		{
			return this.m_targeters;
		}
	}

	public AbilityUtil_Targeter Targeter
	{
		get
		{
			if (this.m_targeters.Count > 0)
			{
				return this.m_targeters[0];
			}
			return null;
		}
		set
		{
			if (this.m_targeters.Count == 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.set_Targeter(AbilityUtil_Targeter)).MethodHandle;
				}
				this.m_targeters.Add(value);
			}
			else
			{
				this.m_targeters[0] = value;
			}
		}
	}

	public List<GameObject> BackupTargeterHighlights
	{
		get
		{
			return this.m_backupTargeterHighlights;
		}
		set
		{
			this.m_backupTargeterHighlights = value;
		}
	}

	public List<AbilityTarget> BackupTargets
	{
		get
		{
			return this.m_backupTargets;
		}
		set
		{
			this.m_backupTargets = value;
		}
	}

	public bool IsActorInTargetRange(ActorData actor)
	{
		bool flag;
		return this.IsActorInTargetRange(actor, out flag);
	}

	public unsafe bool IsActorInTargetRange(ActorData actor, out bool inCover)
	{
		bool flag = false;
		inCover = false;
		if (this.Targeter != null)
		{
			if (this.GetExpectedNumberOfTargeters() >= 2)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.IsActorInTargetRange(ActorData, bool*)).MethodHandle;
				}
				if (this.Targeters.Count < 2)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					inCover = true;
					for (int i = 0; i < this.Targeters.Count; i++)
					{
						if (flag)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!inCover)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									goto IL_F8;
								}
							}
						}
						if (this.Targeters[i] != null)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							bool flag3;
							bool flag2 = this.Targeters[i].IsActorInTargetRange(actor, out flag3);
							if (flag2)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = true;
								if (i == 0)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									inCover = flag3;
								}
								else
								{
									inCover = (inCover && flag3);
								}
							}
						}
					}
					IL_F8:
					if (!flag)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						inCover = false;
						goto IL_108;
					}
					goto IL_108;
				}
			}
			flag = this.Targeter.IsActorInTargetRange(actor, out inCover);
			IL_108:;
		}
		else
		{
			Log.Warning("Ability " + this.m_abilityName + " has no targeter, but we're checking actors in its range.", new object[0]);
			flag = (Board.\u000E().PlayerClampedSquare == actor.\u0012());
			inCover = false;
		}
		return flag;
	}

	public virtual bool ActorCountTowardsEnergyGain(ActorData target, ActorData caster)
	{
		return true;
	}

	public unsafe int GetTargetCounts(ActorData caster, int upToTargeterIndex, out int numAlliesExcludingSelf, out int numEnemies, out bool hittingSelf)
	{
		numAlliesExcludingSelf = 0;
		numEnemies = 0;
		hittingSelf = false;
		HashSet<int> hashSet = new HashSet<int>();
		if (this.Targeters != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetTargetCounts(ActorData, int, int*, int*, bool*)).MethodHandle;
			}
			for (int i = 0; i < this.Targeters.Count; i++)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (i > upToTargeterIndex)
				{
					break;
				}
				List<AbilityUtil_Targeter.ActorTarget> actorsInRange = this.Targeters[i].GetActorsInRange();
				using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
						if (!this.ActorCountTowardsEnergyGain(actorTarget.m_actor, caster))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else if (!hashSet.Contains(actorTarget.m_actor.ActorIndex))
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							hashSet.Add(actorTarget.m_actor.ActorIndex);
							if (!actorTarget.m_actor.IgnoreForEnergyOnHit)
							{
								if (caster.\u000E() == actorTarget.m_actor.\u000E())
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (caster != actorTarget.m_actor)
									{
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
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
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		return hashSet.Count;
	}

	public virtual TargetData[] GetBaseTargetData()
	{
		return this.m_targetData;
	}

	public virtual TargetData[] GetTargetData()
	{
		TargetData[] result = this.GetBaseTargetData();
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetTargetData()).MethodHandle;
			}
			if (this.m_currentAbilityMod.m_useTargetDataOverrides)
			{
				result = this.m_currentAbilityMod.m_targetDataOverrides;
			}
		}
		return result;
	}

	public virtual float GetRangeInSquares(int targetIndex)
	{
		TargetData[] targetData = this.GetTargetData();
		float num = 0f;
		if (targetData != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetRangeInSquares(int)).MethodHandle;
			}
			if (targetData.Length > targetIndex)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				num = targetData[targetIndex].m_range;
			}
		}
		if (this.m_currentAbilityMod != null)
		{
			num = Mathf.Max(0f, this.m_currentAbilityMod.m_targetDataMaxRangeMod.GetModifiedValue(num));
		}
		return num;
	}

	public float GetMinRangeInSquares(int targetIndex)
	{
		TargetData[] targetData = this.GetTargetData();
		float num = 0f;
		if (targetData != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetMinRangeInSquares(int)).MethodHandle;
			}
			if (targetData.Length > targetIndex)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = targetData[targetIndex].m_minRange;
			}
		}
		if (this.m_currentAbilityMod != null)
		{
			num = Mathf.Max(0f, this.m_currentAbilityMod.m_targetDataMinRangeMod.GetModifiedValue(num));
		}
		return num;
	}

	public virtual bool GetCheckLoS(int targetIndex)
	{
		TargetData[] targetData = this.GetTargetData();
		bool flag = true;
		if (targetData != null && targetData.Length > targetIndex)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetCheckLoS(int)).MethodHandle;
			}
			flag = targetData[targetIndex].m_checkLineOfSight;
			if (this.m_currentAbilityMod != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = this.m_currentAbilityMod.m_targetDataCheckLosMod.GetModifiedValue(flag);
			}
		}
		return flag;
	}

	public string GetTargetDescription(int targetIndex)
	{
		TargetData[] targetData = this.GetTargetData();
		string result = null;
		if (targetData != null && targetData.Length > targetIndex)
		{
			result = "Select " + targetData[targetIndex].m_description;
		}
		return result;
	}

	public bool IsAutoSelect()
	{
		TargetData[] targetData = this.GetTargetData();
		bool result;
		if (targetData != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.IsAutoSelect()).MethodHandle;
			}
			result = (targetData.Length == 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public virtual bool IsFreeAction()
	{
		bool result = this.m_freeAction;
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.IsFreeAction()).MethodHandle;
			}
			result = this.m_currentAbilityMod.m_isFreeActionMod.GetModifiedValue(this.m_freeAction);
		}
		return result;
	}

	public virtual bool ShouldRotateToTargetPos()
	{
		bool result = !this.IsSimpleAction();
		if (this.m_rotationVisibilityMode == Ability.RotationVisibilityMode.OnAllyClientOnly)
		{
			result = true;
			if (NetworkClient.active)
			{
				ActorData actorData = this.ActorData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.ShouldRotateToTargetPos()).MethodHandle;
					}
					if (actorData != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						result = (activeOwnedActorData.\u000E() == actorData.\u000E());
					}
				}
			}
		}
		else if (this.m_rotationVisibilityMode == Ability.RotationVisibilityMode.Always)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = true;
		}
		else if (this.m_rotationVisibilityMode == Ability.RotationVisibilityMode.Never)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			result = false;
		}
		return result;
	}

	public virtual Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		TargetData[] targetData = this.GetTargetData();
		if (targetData != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetRotateToTargetPos(List<AbilityTarget>, ActorData)).MethodHandle;
			}
			if (targetData.Length > 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (targetData[0].m_targetingParadigm == Ability.TargetingParadigm.BoardSquare)
				{
					BoardSquare boardSquare = Board.\u000E().\u000E(targets[0].GridPos);
					if (boardSquare != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						return boardSquare.ToVector3();
					}
				}
			}
		}
		return targets[0].FreePos;
	}

	public bool IsSimpleAction()
	{
		TargetData[] targetData = this.GetTargetData();
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

	public Ability.TargetingParadigm GetTargetingParadigm(int targetIndex)
	{
		Ability.TargetingParadigm result = Ability.TargetingParadigm.Position;
		TargetData[] targetData = this.GetTargetData();
		if (targetData != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetTargetingParadigm(int)).MethodHandle;
			}
			if (targetData.Length > targetIndex)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				result = targetData[targetIndex].m_targetingParadigm;
			}
		}
		return result;
	}

	public virtual Ability.TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return this.GetTargetingParadigm(targetIndex);
	}

	public int GetNumTargets()
	{
		TargetData[] targetData = this.GetTargetData();
		int result = 0;
		if (targetData != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetNumTargets()).MethodHandle;
			}
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
		if (this.m_currentAbilityMod != null && this.m_currentAbilityMod.m_useStatusWhenRequestedOverride)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetStatusToApplyWhenRequested()).MethodHandle;
			}
			return this.m_currentAbilityMod.m_statusWhenRequestedOverride;
		}
		return this.m_statusWhenRequested;
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
		Ability[] result = this.m_chainAbilities;
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetChainAbilities()).MethodHandle;
			}
			if (this.m_currentAbilityMod.m_useChainAbilityOverrides)
			{
				result = this.m_currentAbilityMod.m_chainAbilityOverrides;
			}
		}
		return result;
	}

	public bool HasAbilityAsPartOfChain(Ability ability)
	{
		Ability[] chainAbilities = this.GetChainAbilities();
		foreach (Ability y in chainAbilities)
		{
			if (ability == y)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.HasAbilityAsPartOfChain(Ability)).MethodHandle;
				}
				return true;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		return false;
	}

	public virtual bool ShouldAutoQueueIfValid()
	{
		bool flag = AbilityUtils.AbilityHasTag(this, AbilityTags.AutoQueueIfValid);
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.ShouldAutoQueueIfValid()).MethodHandle;
			}
			if (this.m_currentAbilityMod.m_autoQueueIfValidMod.operation != AbilityModPropertyBool.ModOp.Ignore)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_currentAbilityMod.m_autoQueueIfValidMod.GetModifiedValue(flag);
			}
		}
		return flag;
	}

	public virtual bool AllowCancelWhenAutoQueued()
	{
		return false;
	}

	public virtual Ability.MovementAdjustment GetMovementAdjustment()
	{
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetMovementAdjustment()).MethodHandle;
			}
			if (this.m_currentAbilityMod.m_useMovementAdjustmentOverride)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_currentAbilityMod.m_movementAdjustmentOverride;
			}
		}
		return this.m_movementAdjustment;
	}

	public bool GetPreventsMovement()
	{
		return this.GetMovementAdjustment() == Ability.MovementAdjustment.NoMovement;
	}

	public bool GetAffectsMovement()
	{
		return this.GetMovementAdjustment() != Ability.MovementAdjustment.FullMovement;
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
		bool result;
		if (this.GetMovementType() != ActorData.MovementType.Charge)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.IsCharge()).MethodHandle;
			}
			result = (this.GetMovementType() == ActorData.MovementType.WaypointFlight);
		}
		else
		{
			result = true;
		}
		return result;
	}

	internal virtual bool IsTeleport()
	{
		bool result;
		if (this.GetMovementType() != ActorData.MovementType.Teleport)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.IsTeleport()).MethodHandle;
			}
			result = (this.GetMovementType() == ActorData.MovementType.Flight);
		}
		else
		{
			result = true;
		}
		return result;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetEvadeDestinationForTargeter(List<AbilityTarget>, ActorData)).MethodHandle;
			}
			if (targets.Count > 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.GetRunPriority() == AbilityPriority.Evasion)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					result = Board.\u000E().\u000E(targets[targets.Count - 1].GridPos);
				}
			}
		}
		return result;
	}

	internal float CalcMovementSpeed(float distance)
	{
		float result;
		if (this.m_movementDuration > 0f)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.CalcMovementSpeed(float)).MethodHandle;
			}
			result = distance / this.m_movementDuration;
		}
		else
		{
			result = this.m_movementSpeed;
		}
		return result;
	}

	public string GetTooltipForUI()
	{
		if (this.m_toolTipForUI == null)
		{
			this.RebuildTooltipForUI();
		}
		return this.m_toolTipForUI;
	}

	public virtual ActorModelData.ActionAnimationType GetActionAnimType()
	{
		ActorModelData.ActionAnimationType result = this.m_actionAnimType;
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetActionAnimType()).MethodHandle;
			}
			if (this.m_currentAbilityMod.m_useActionAnimTypeOverride)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				result = this.m_currentAbilityMod.m_actionAnimTypeOverride;
			}
		}
		return result;
	}

	public virtual ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		return this.GetActionAnimType();
	}

	public virtual bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool flag = animIndex == (int)this.GetActionAnimType();
		if (!flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.CanTriggerAnimAtIndexForTaunt(int)).MethodHandle;
			}
			Ability[] chainAbilities = this.GetChainAbilities();
			int i = 0;
			while (i < chainAbilities.Length)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						return flag;
					}
				}
				else
				{
					flag = (animIndex == (int)chainAbilities[i].GetActionAnimType());
					i++;
				}
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
		if (this.m_chainAbilities == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.SanitizeChainAbilities()).MethodHandle;
			}
			this.m_chainAbilities = new Ability[0];
		}
		foreach (Ability ability in this.GetChainAbilities())
		{
			string str = string.Concat(new string[]
			{
				"Ability '",
				this.m_abilityName,
				"'- chain ability '",
				ability.m_abilityName,
				"' "
			});
			if (ability.m_techPointsCost != 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(str + "has non-zero tech point cost.  Zeroing...", new object[0]);
				ability.m_techPointsCost = 0;
			}
			if (ability.GetAffectsMovement())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(str + "adjusts movement.  Removing...", new object[0]);
				ability.m_movementAdjustment = Ability.MovementAdjustment.FullMovement;
			}
			if (ability.m_cooldown != 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(str + "has non-zero cooldown.  Zeroing...", new object[0]);
				ability.m_cooldown = 0;
			}
			if (!ability.IsFreeAction())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(str + "is not a free action.  Liberating...", new object[0]);
				ability.m_freeAction = true;
			}
			if (ability.m_chainAbilities.Length != 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(str + "has its own chain abilities.  Breaking...", new object[0]);
				ability.m_chainAbilities = new Ability[0];
			}
			if (this.RunPriority > ability.RunPriority)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(str + "has an earlier run priority than its predecessor.  Make sure chain abilities happen later than the 'master' ability for things to look right.", new object[0]);
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

	public AbilityMod CurrentAbilityMod
	{
		get
		{
			return this.m_currentAbilityMod;
		}
	}

	protected virtual void OnApplyAbilityMod(AbilityMod abilityMod)
	{
	}

	public void ApplyAbilityMod(AbilityMod abilityMod, ActorData actor)
	{
		if (abilityMod.GetTargetAbilityType() == base.GetType())
		{
			this.ResetAbilityTargeters();
			ActorTargeting actorTargeting = actor.\u000E();
			if (actorTargeting != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.ApplyAbilityMod(AbilityMod, ActorData)).MethodHandle;
				}
				actorTargeting.MarkForForceRedraw();
			}
			this.ClearAbilityMod(actor);
			this.m_currentAbilityMod = abilityMod;
			this.OnApplyAbilityMod(abilityMod);
			this.ResetNameplateTargetingNumbers();
		}
		else
		{
			Debug.LogError("Trying to apply mod to wrong ability type. mod_ability_type: " + abilityMod.GetTargetAbilityType().ToString() + " ability_type: " + base.GetType().ToString());
		}
	}

	protected virtual void OnRemoveAbilityMod()
	{
	}

	public void ClearAbilityMod(ActorData actor)
	{
		if (this.m_currentAbilityMod != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.ClearAbilityMod(ActorData)).MethodHandle;
			}
			this.m_currentAbilityMod = null;
			this.OnRemoveAbilityMod();
			this.ResetNameplateTargetingNumbers();
			if (Application.isEditor)
			{
				Debug.Log("Removing mod from ability " + this.GetDebugIdentifier("orange"));
			}
		}
	}

	public void DrawGizmos()
	{
		if (this.Targeter != null && this.IsAbilitySelected())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.DrawGizmos()).MethodHandle;
			}
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (activeOwnedActorData == this.ActorData)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					ActorTurnSM actorTurnSM = activeOwnedActorData.\u000E();
					if (actorTurnSM != null && actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						AbilityTarget currentTarget = AbilityTarget.CreateAbilityTargetFromInterface();
						this.Targeter.DrawGizmos(currentTarget, activeOwnedActorData);
					}
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
		bool result;
		if (this.GetTargetData() != null && this.GetTargetData().Length > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.UseTargeterGridPosForCameraBounds()).MethodHandle;
			}
			result = (this.GetTargetingParadigm(0) != Ability.TargetingParadigm.Direction);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public virtual List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		if (this.m_runPriority != AbilityPriority.Evasion)
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

	public unsafe bool CalcBoundsOfInterestForCamera(out Bounds bounds, List<AbilityTarget> targets, ActorData caster)
	{
		bounds = default(Bounds);
		List<Vector3> list = this.CalcPointsOfInterestForCamera(targets, caster);
		bool flag;
		if (list != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.CalcBoundsOfInterestForCamera(Bounds*, List<AbilityTarget>, ActorData)).MethodHandle;
			}
			flag = (list.Count > 0);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
		{
			bounds.center = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				bounds.Encapsulate(list[i]);
			}
		}
		return flag2;
	}

	protected Passive GetPassiveOfType(Type passiveType)
	{
		PassiveData component = base.GetComponent<PassiveData>();
		if (component != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetPassiveOfType(Type)).MethodHandle;
			}
			return component.GetPassiveOfType(passiveType);
		}
		return null;
	}

	protected T GetPassiveOfType<T>() where T : Passive
	{
		PassiveData component = base.GetComponent<PassiveData>();
		if (component != null)
		{
			return component.GetPassiveOfType<T>();
		}
		return (T)((object)null);
	}

	protected Ability GetAbilityOfType(Type abilityType)
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			return component.GetAbilityOfType(abilityType);
		}
		return null;
	}

	protected T GetAbilityOfType<T>() where T : Ability
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetAbilityOfType()).MethodHandle;
			}
			return component.GetAbilityOfType<T>();
		}
		return (T)((object)null);
	}

	protected AbilityData.ActionType GetActionTypeOfAbility(Ability ability)
	{
		AbilityData.ActionType result = AbilityData.ActionType.INVALID_ACTION;
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			result = component.GetActionTypeOfAbility(ability);
		}
		return result;
	}

	public string GetBaseAbilityDesc()
	{
		string text = string.Empty;
		if (this.m_techPointInteractions != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetBaseAbilityDesc()).MethodHandle;
			}
			if (this.m_techPointInteractions.Length > 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				text += "TechPoint Interactions:\n";
			}
			for (int i = 0; i < this.m_techPointInteractions.Length; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"    [",
					this.m_techPointInteractions[i].m_type.ToString(),
					"] = ",
					this.m_techPointInteractions[i].m_amount,
					"\n"
				});
			}
		}
		if (this.m_tags != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_tags.Count > 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				text += "Tags:\n";
			}
			for (int j = 0; j < this.m_tags.Count; j++)
			{
				text = text + "    [" + this.m_tags[j].ToString() + "]\n";
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
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
		string text = string.Concat(new string[]
		{
			"Ability ",
			this.m_abilityName,
			"[ ",
			base.GetType().ToString(),
			" ]"
		});
		if (colorString.Length > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ability.GetDebugIdentifier(string)).MethodHandle;
			}
			return string.Concat(new string[]
			{
				"<color=",
				colorString,
				">",
				text,
				"</color>"
			});
		}
		return text;
	}

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
}
