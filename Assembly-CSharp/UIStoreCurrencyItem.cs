using System;

public class UIStoreCurrencyItem : UIStorePurchaseBaseItem
{
	private void Awake()
	{
		UIManager.SetGameObjectActive(this.m_ownedIcon, false, null);
		UIManager.SetGameObjectActive(this.m_lockedIcon, false, null);
		UIManager.SetGameObjectActive(this.m_headerNameContainer, false, null);
		UIManager.SetGameObjectActive(this.m_gameCurrencyLabel, false, null);
		UIManager.SetGameObjectActive(this.m_realCurrencyIcon, false, null);
		UIManager.SetGameObjectActive(this.m_selectedCurrent, false, null);
		UIManager.SetGameObjectActive(this.m_selectedInUse, false, null);
	}
}
