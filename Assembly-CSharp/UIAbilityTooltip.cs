using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityTooltip : UITooltipBase
{
	[Header("-- For Ability Tooltip --")]
	public TextMeshProUGUI m_tooltipTitle;

	public TextMeshProUGUI m_tooltipDescription;

	[Space(10f)]
	public GameObject m_freeActionsLabelObj;

	public TextMeshProUGUI m_cooldownText;

	[Header("-- For Mod Tooltip --")]
	public RectTransform m_modTooltipParentTransform;

	public TextMeshProUGUI m_modTitle;

	public TextMeshProUGUI m_modTooltip;

	public RectTransform m_modCostContainer;

	public Image[] m_modCostNotches;

	[Header("-- For Status Tooltip --")]
	public RectTransform m_statusEffectTransform;

	public AbilityStatusEffectEntry m_statusEffectPrefab;

	public VerticalLayoutGroup m_layoutGroup;

	[Header("-- For Phases (assuming arrays have 3 elements, idx 0 for Prep, Idx 1 for Dash, Idx 2 for Combat --")]
	public TextMeshProUGUI m_phaseName;

	public Image m_phaseIcon;

	public Image m_phaseGradient;

	public Color[] m_phaseGradientColors;

	public UIAbilityTooltip.PhaseUIElementInfo[] m_perPhaseUIElements;

	[Header("-- For Video --")]
	public bool disableVideo;

	public RectTransform m_movieContainer;

	public PlayRawImageMovieTexture m_movieTexturePlayer;

	[Header("-- For Resizing Tooltip --")]
	public float m_minAbilityTooltipDescHeight = 100f;

	public float m_minModTooltipDescHeight = 50f;

	public float m_abilityTooltipTextHeightPadding = 120f;

	public float m_modTooltipTextHeightPadding = 50f;

	private const string c_modTitleFormatString = "{0} <#8E8E8E>";

	private List<AbilityStatusEffectEntry> m_statusEffectDisplayList = new List<AbilityStatusEffectEntry>();

	private void Awake()
	{
		if (this.m_layoutGroup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.Awake()).MethodHandle;
			}
			AbilityStatusEffectEntry[] componentsInChildren = this.m_layoutGroup.GetComponentsInChildren<AbilityStatusEffectEntry>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.m_statusEffectDisplayList.Add(componentsInChildren[i]);
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public void Setup(Ability ability)
	{
		this.Setup(ability, null, null);
	}

	public void Setup(Ability ability, AbilityMod mod)
	{
		this.Setup(ability, mod, null);
	}

	public void Setup(Ability ability, AbilityMod mod, string movieAssetName)
	{
		this.m_tooltipTitle.text = ability.GetNameString();
		this.m_tooltipDescription.text = ability.GetToolTipString(false);
		if (!ability.m_flavorText.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.Setup(Ability, AbilityMod, string)).MethodHandle;
			}
			TextMeshProUGUI tooltipDescription = this.m_tooltipDescription;
			string text = tooltipDescription.text;
			tooltipDescription.text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				Environment.NewLine,
				"<i>",
				ability.m_flavorText,
				"</i>"
			});
		}
		if (this.m_freeActionsLabelObj != null)
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
			UIManager.SetGameObjectActive(this.m_freeActionsLabelObj, ability.IsFreeAction(), null);
		}
		if (this.m_cooldownText != null)
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
			int num = ability.GetCooldownForUIDisplay();
			if (ability.GetModdedMaxStocks() > 0)
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
				if (ability.GetModdedStockRefreshDuration() >= 0)
				{
					num = ability.GetModdedStockRefreshDuration();
				}
			}
			if (num > 0)
			{
				this.m_cooldownText.text = string.Format(StringUtil.TR("CooldownDuration", "Global"), num);
			}
			else
			{
				this.m_cooldownText.text = StringUtil.TR("NoCooldown", "Global");
			}
		}
		this.SetupModTooltip(mod, ability);
		this.SetupStatusTooltip(mod, ability);
		int phaseIndex = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(ability.RunPriority));
		this.SetupPhaseIndicators(phaseIndex, ability.GetPhaseString());
		this.SetupMoviePanel(movieAssetName);
	}

	private void SetupStatusTooltip(AbilityMod mod, Ability ability)
	{
		if (this.m_statusEffectTransform != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.SetupStatusTooltip(AbilityMod, Ability)).MethodHandle;
			}
			if (ability != null)
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
				List<StatusType> statusTypesForTooltip = ability.GetStatusTypesForTooltip();
				List<StatusType> list;
				if (mod != null)
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
					list = mod.GetStatusTypesForTooltip();
				}
				else
				{
					list = new List<StatusType>();
				}
				int i = 0;
				for (int j = 0; j < statusTypesForTooltip.Count; j++)
				{
					if (i >= this.m_statusEffectDisplayList.Count)
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
						AbilityStatusEffectEntry abilityStatusEffectEntry = UnityEngine.Object.Instantiate<AbilityStatusEffectEntry>(this.m_statusEffectPrefab);
						abilityStatusEffectEntry.transform.SetParent(this.m_layoutGroup.transform);
						abilityStatusEffectEntry.transform.localPosition = Vector3.zero;
						abilityStatusEffectEntry.transform.localEulerAngles = Vector3.zero;
						abilityStatusEffectEntry.transform.localScale = Vector3.one;
						this.m_statusEffectDisplayList.Add(abilityStatusEffectEntry);
					}
					this.m_statusEffectDisplayList[i].Setup(statusTypesForTooltip[j]);
					i++;
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (!statusTypesForTooltip.Contains(list[k]))
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
						if (i >= this.m_statusEffectDisplayList.Count)
						{
							AbilityStatusEffectEntry abilityStatusEffectEntry2 = UnityEngine.Object.Instantiate<AbilityStatusEffectEntry>(this.m_statusEffectPrefab);
							abilityStatusEffectEntry2.transform.SetParent(this.m_layoutGroup.transform);
							abilityStatusEffectEntry2.transform.localPosition = Vector3.zero;
							abilityStatusEffectEntry2.transform.localEulerAngles = Vector3.zero;
							abilityStatusEffectEntry2.transform.localScale = Vector3.one;
							this.m_statusEffectDisplayList.Add(abilityStatusEffectEntry2);
						}
						this.m_statusEffectDisplayList[i].Setup(list[k]);
						i++;
					}
				}
				UIManager.SetGameObjectActive(this.m_statusEffectTransform, i != 0, null);
				while (i < this.m_statusEffectDisplayList.Count)
				{
					UIManager.SetGameObjectActive(this.m_statusEffectDisplayList[i], false, null);
					i++;
				}
			}
		}
	}

	private void SetupModTooltip(AbilityMod mod, Ability ability)
	{
		if (this.m_modTooltipParentTransform != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.SetupModTooltip(AbilityMod, Ability)).MethodHandle;
			}
			if (mod != null)
			{
				this.m_modTitle.text = string.Format("{0} <#8E8E8E>", mod.GetName());
				if (this.m_modCostContainer != null)
				{
					for (int i = 0; i < this.m_modCostNotches.Length; i++)
					{
						UIManager.SetGameObjectActive(this.m_modCostNotches[i], i < mod.m_equipCost, null);
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.m_modTooltip.text = mod.GetFullTooltip(ability);
				if (!this.m_modTooltipParentTransform.gameObject.activeSelf)
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
					UIManager.SetGameObjectActive(this.m_modTooltipParentTransform, true, null);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_modTooltipParentTransform, false, null);
			}
		}
	}

	private void SetupPhaseIndicators(int phaseIndex, string phaseName)
	{
		if (phaseIndex < this.m_perPhaseUIElements.Length)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.SetupPhaseIndicators(int, string)).MethodHandle;
			}
			this.SetupPhaseUIElements(this.m_perPhaseUIElements[phaseIndex], phaseName);
		}
		else
		{
			this.SetupPhaseUIElements(null, null);
		}
		if (this.m_phaseGradient != null)
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
			if (phaseIndex < this.m_phaseGradientColors.Length)
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
				this.m_phaseGradient.color = this.m_phaseGradientColors[phaseIndex];
			}
			else
			{
				this.m_phaseGradient.color = Color.clear;
			}
		}
	}

	private void SetupPhaseUIElements(UIAbilityTooltip.PhaseUIElementInfo uiElementInfo, string phaseName)
	{
		if (this.m_phaseName != null)
		{
			TMP_Text phaseName2 = this.m_phaseName;
			string text;
			if (phaseName != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.SetupPhaseUIElements(UIAbilityTooltip.PhaseUIElementInfo, string)).MethodHandle;
				}
				text = phaseName;
			}
			else
			{
				text = string.Empty;
			}
			phaseName2.text = text;
		}
		if (this.m_phaseIcon != null)
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
			Image phaseIcon = this.m_phaseIcon;
			Sprite sprite;
			if (uiElementInfo != null)
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
				sprite = uiElementInfo.m_icon;
			}
			else
			{
				sprite = null;
			}
			phaseIcon.sprite = sprite;
			if (!this.m_phaseIcon.gameObject.activeSelf)
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
				UIManager.SetGameObjectActive(this.m_phaseIcon, true, null);
			}
		}
		for (int i = 0; i < this.m_perPhaseUIElements.Length; i++)
		{
			if (this.m_perPhaseUIElements[i].m_tickFill != null)
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
				UIManager.SetGameObjectActive(this.m_perPhaseUIElements[i].m_tickFill, this.m_perPhaseUIElements[i] == uiElementInfo, null);
			}
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

	private void SetupMoviePanel(string movieAssetName)
	{
		if (this.m_movieTexturePlayer != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityTooltip.SetupMoviePanel(string)).MethodHandle;
			}
			if (!movieAssetName.IsNullOrEmpty())
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
				this.m_movieTexturePlayer.Play(movieAssetName, true, true, true);
				UIManager.SetGameObjectActive(this.m_movieContainer, true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_movieContainer, false, null);
			}
		}
	}

	[Serializable]
	public class PhaseUIElementInfo
	{
		public string m_phaseName;

		public Sprite m_icon;

		public Image m_tickFill;
	}
}
