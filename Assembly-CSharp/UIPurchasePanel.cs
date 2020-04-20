using System;
using TMPro;
using UnityEngine;

public class UIPurchasePanel : MonoBehaviour
{
	public _SelectableBtn m_isoButton;

	public _SelectableBtn m_freelancerCurrencyButton;

	public _SelectableBtn m_realCurrencyButton;

	public TextMeshProUGUI[] m_isoCostTexts;

	public TextMeshProUGUI[] m_freelancerCurrencyCostTexts;

	public TextMeshProUGUI[] m_realCurrencyCostText;

	public TextMeshProUGUI m_unlockText;

	private string m_tooltipDescription;

	public bool m_allowDuringGame;

	public void Awake()
	{
		if (this.m_isoButton != null)
		{
			this.m_isoButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.TooltipSetup), null);
			this.m_isoButton.spriteController.SetForceHovercallback(true);
			this.m_isoButton.spriteController.SetForceExitCallback(true);
		}
		if (this.m_freelancerCurrencyButton != null)
		{
			this.m_freelancerCurrencyButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.TooltipSetup), null);
			this.m_freelancerCurrencyButton.spriteController.SetForceHovercallback(true);
			this.m_freelancerCurrencyButton.spriteController.SetForceExitCallback(true);
		}
		if (this.m_realCurrencyButton != null)
		{
			this.m_realCurrencyButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.TooltipSetup), null);
			this.m_realCurrencyButton.spriteController.SetForceHovercallback(true);
			this.m_realCurrencyButton.spriteController.SetForceExitCallback(true);
		}
	}

	private void OnEnable()
	{
		if (!this.m_allowDuringGame)
		{
			GameManager gameManager = GameManager.Get();
			bool flag;
			if (gameManager != null)
			{
				flag = (gameManager.GameStatus < GameStatus.FreelancerSelecting || gameManager.GameStatus == GameStatus.Stopped);
			}
			else
			{
				flag = false;
			}
			if (!flag && base.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(base.gameObject, false, null);
			}
		}
	}

	public void Setup(int isoPrice, int freelancerCurrencyPrice, float realCurrencyPrice, bool allowDuringGame = false)
	{
		this.m_allowDuringGame = allowDuringGame;
		if (!this.m_allowDuringGame)
		{
			bool flag;
			if (isoPrice <= 0)
			{
				if (freelancerCurrencyPrice <= 0)
				{
					flag = (realCurrencyPrice > 0f);
					goto IL_4D;
				}
			}
			flag = true;
			IL_4D:
			bool flag2 = flag;
			GameManager gameManager = GameManager.Get();
			bool flag3;
			if (gameManager != null)
			{
				if (gameManager.GameStatus >= GameStatus.FreelancerSelecting)
				{
					flag3 = (gameManager.GameStatus == GameStatus.Stopped);
				}
				else
				{
					flag3 = true;
				}
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			UIManager.SetGameObjectActive(base.gameObject, flag2 && flag4, null);
		}
		if (this.m_isoButton != null)
		{
			UIManager.SetGameObjectActive(this.m_isoButton, isoPrice > 0, null);
			this.SetPriceLabels(this.m_isoCostTexts, isoPrice, "iso");
		}
		if (this.m_freelancerCurrencyButton != null)
		{
			UIManager.SetGameObjectActive(this.m_freelancerCurrencyButton, freelancerCurrencyPrice > 0, null);
			this.SetPriceLabels(this.m_freelancerCurrencyCostTexts, freelancerCurrencyPrice, "credit");
		}
		if (this.m_realCurrencyButton != null)
		{
			UIManager.SetGameObjectActive(this.m_realCurrencyButton, realCurrencyPrice > 0f, null);
			this.SetRealPriceLabels(realCurrencyPrice);
		}
	}

	private void SetPriceLabels(TextMeshProUGUI[] costLabels, int cost, string spriteName)
	{
		string text = "<sprite name=\"" + spriteName + "\">" + UIStorePanel.FormatIntToString(cost, true);
		for (int i = 0; i < costLabels.Length; i++)
		{
			costLabels[i].text = text;
		}
	}

	private void SetRealPriceLabels(float cost)
	{
		string localizedPriceString = UIStorePanel.GetLocalizedPriceString(cost, HydrogenConfig.Get().Ticket.AccountCurrency);
		for (int i = 0; i < this.m_realCurrencyCostText.Length; i++)
		{
			this.m_realCurrencyCostText[i].text = localizedPriceString;
		}
	}

	public void SetDisabled(bool isDisabled)
	{
		if (this.m_isoButton != null)
		{
			this.m_isoButton.m_ignoreDefaultAnimationCall = isDisabled;
			this.m_isoButton.m_ignoreHoverAnimationCall = isDisabled;
			this.m_isoButton.m_ignorePressAnimationCall = isDisabled;
			this.m_isoButton.spriteController.SetClickable(!isDisabled);
			UIManager.SetGameObjectActive(this.m_isoButton.spriteController.m_defaultImage, !isDisabled, null);
		}
		if (this.m_freelancerCurrencyButton != null)
		{
			this.m_freelancerCurrencyButton.m_ignoreDefaultAnimationCall = isDisabled;
			this.m_freelancerCurrencyButton.m_ignoreHoverAnimationCall = isDisabled;
			this.m_freelancerCurrencyButton.m_ignorePressAnimationCall = isDisabled;
			this.m_freelancerCurrencyButton.spriteController.SetClickable(!isDisabled);
			UIManager.SetGameObjectActive(this.m_freelancerCurrencyButton.spriteController.m_defaultImage, !isDisabled, null);
		}
		if (this.m_realCurrencyButton != null)
		{
			this.m_realCurrencyButton.m_ignoreDefaultAnimationCall = isDisabled;
			this.m_realCurrencyButton.m_ignoreHoverAnimationCall = isDisabled;
			this.m_realCurrencyButton.m_ignorePressAnimationCall = isDisabled;
			this.m_realCurrencyButton.spriteController.SetClickable(!isDisabled);
			UIManager.SetGameObjectActive(this.m_realCurrencyButton.spriteController.m_defaultImage, !isDisabled, null);
		}
	}

	public void SetupTooltip(string tooltipDescription)
	{
		this.m_tooltipDescription = tooltipDescription;
	}

	private bool TooltipSetup(UITooltipBase tooltip)
	{
		if (!this.m_tooltipDescription.IsNullOrEmpty())
		{
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(StringUtil.TR("Purchase", "Global"), this.m_tooltipDescription, string.Empty);
			return true;
		}
		return false;
	}
}
