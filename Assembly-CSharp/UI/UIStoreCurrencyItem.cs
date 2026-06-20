public class UIStoreCurrencyItem : UIStorePurchaseBaseItem
{
	private void Awake()
	{
		UIManager.SetGameObjectActive(m_ownedIcon, false);
		UIManager.SetGameObjectActive(m_lockedIcon, false);
		UIManager.SetGameObjectActive(m_headerNameContainer, false);
		UIManager.SetGameObjectActive(m_gameCurrencyLabel, false);
		UIManager.SetGameObjectActive(m_realCurrencyIcon, false);
		UIManager.SetGameObjectActive(m_selectedCurrent, false);
		UIManager.SetGameObjectActive(m_selectedInUse, false);
	}
}
