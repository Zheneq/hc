using System.Text;
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
		if (m_isoButton != null)
		{
			m_isoButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, TooltipSetup);
			m_isoButton.spriteController.SetForceHovercallback(true);
			m_isoButton.spriteController.SetForceExitCallback(true);
		}
		if (m_freelancerCurrencyButton != null)
		{
			m_freelancerCurrencyButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, TooltipSetup);
			m_freelancerCurrencyButton.spriteController.SetForceHovercallback(true);
			m_freelancerCurrencyButton.spriteController.SetForceExitCallback(true);
		}
		if (!(m_realCurrencyButton != null))
		{
			return;
		}
		while (true)
		{
			m_realCurrencyButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, TooltipSetup);
			m_realCurrencyButton.spriteController.SetForceHovercallback(true);
			m_realCurrencyButton.spriteController.SetForceExitCallback(true);
			return;
		}
	}

	private void OnEnable()
	{
		if (m_allowDuringGame)
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		int num;
		if (gameManager != null)
		{
			num = ((gameManager.GameStatus < GameStatus.FreelancerSelecting || gameManager.GameStatus == GameStatus.Stopped) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num == 0 && base.gameObject.activeSelf)
		{
			UIManager.SetGameObjectActive(base.gameObject, false);
		}
	}

	public void Setup(int isoPrice, int freelancerCurrencyPrice, float realCurrencyPrice, bool allowDuringGame = false)
	{
		m_allowDuringGame = allowDuringGame;
		int num;
		if (!m_allowDuringGame)
		{
			if (isoPrice <= 0)
			{
				if (freelancerCurrencyPrice <= 0)
				{
					num = ((realCurrencyPrice > 0f) ? 1 : 0);
					goto IL_004d;
				}
			}
			num = 1;
			goto IL_004d;
		}
		goto IL_00a8;
		IL_004d:
		bool flag = (byte)num != 0;
		GameManager gameManager = GameManager.Get();
		int num2;
		if (gameManager != null)
		{
			if (gameManager.GameStatus >= GameStatus.FreelancerSelecting)
			{
				num2 = ((gameManager.GameStatus == GameStatus.Stopped) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
		}
		else
		{
			num2 = 0;
		}
		bool flag2 = (byte)num2 != 0;
		UIManager.SetGameObjectActive(base.gameObject, flag && flag2);
		goto IL_00a8;
		IL_00a8:
		if (m_isoButton != null)
		{
			UIManager.SetGameObjectActive(m_isoButton, isoPrice > 0);
			SetPriceLabels(m_isoCostTexts, isoPrice, "iso");
		}
		if (m_freelancerCurrencyButton != null)
		{
			UIManager.SetGameObjectActive(m_freelancerCurrencyButton, freelancerCurrencyPrice > 0);
			SetPriceLabels(m_freelancerCurrencyCostTexts, freelancerCurrencyPrice, "credit");
		}
		if (!(m_realCurrencyButton != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_realCurrencyButton, realCurrencyPrice > 0f);
			SetRealPriceLabels(realCurrencyPrice);
			return;
		}
	}

	private void SetPriceLabels(TextMeshProUGUI[] costLabels, int cost, string spriteName)
	{
		string text = new StringBuilder().Append("<sprite name=\"").Append(spriteName).Append("\">").Append(UIStorePanel.FormatIntToString(cost, true)).ToString();
		for (int i = 0; i < costLabels.Length; i++)
		{
			costLabels[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private void SetRealPriceLabels(float cost)
	{
		string localizedPriceString = UIStorePanel.GetLocalizedPriceString(cost, HydrogenConfig.Get().Ticket.AccountCurrency);
		for (int i = 0; i < m_realCurrencyCostText.Length; i++)
		{
			m_realCurrencyCostText[i].text = localizedPriceString;
		}
		while (true)
		{
			return;
		}
	}

	public void SetDisabled(bool isDisabled)
	{
		if (m_isoButton != null)
		{
			m_isoButton.m_ignoreDefaultAnimationCall = isDisabled;
			m_isoButton.m_ignoreHoverAnimationCall = isDisabled;
			m_isoButton.m_ignorePressAnimationCall = isDisabled;
			m_isoButton.spriteController.SetClickable(!isDisabled);
			UIManager.SetGameObjectActive(m_isoButton.spriteController.m_defaultImage, !isDisabled);
		}
		if (m_freelancerCurrencyButton != null)
		{
			m_freelancerCurrencyButton.m_ignoreDefaultAnimationCall = isDisabled;
			m_freelancerCurrencyButton.m_ignoreHoverAnimationCall = isDisabled;
			m_freelancerCurrencyButton.m_ignorePressAnimationCall = isDisabled;
			m_freelancerCurrencyButton.spriteController.SetClickable(!isDisabled);
			UIManager.SetGameObjectActive(m_freelancerCurrencyButton.spriteController.m_defaultImage, !isDisabled);
		}
		if (!(m_realCurrencyButton != null))
		{
			return;
		}
		while (true)
		{
			m_realCurrencyButton.m_ignoreDefaultAnimationCall = isDisabled;
			m_realCurrencyButton.m_ignoreHoverAnimationCall = isDisabled;
			m_realCurrencyButton.m_ignorePressAnimationCall = isDisabled;
			m_realCurrencyButton.spriteController.SetClickable(!isDisabled);
			UIManager.SetGameObjectActive(m_realCurrencyButton.spriteController.m_defaultImage, !isDisabled);
			return;
		}
	}

	public void SetupTooltip(string tooltipDescription)
	{
		m_tooltipDescription = tooltipDescription;
	}

	private bool TooltipSetup(UITooltipBase tooltip)
	{
		if (!m_tooltipDescription.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
					uITitledTooltip.Setup(StringUtil.TR("Purchase", "Global"), m_tooltipDescription, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}
}
