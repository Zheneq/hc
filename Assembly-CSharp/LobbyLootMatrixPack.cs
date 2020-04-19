using System;

[Serializable]
public class LobbyLootMatrixPack
{
	public int Index;

	public int NumberOfMatrixes;

	public BonusLootMatrixes[] BonusMatrixes;

	public CountryPrices Prices;

	public string ProductCode;

	public bool IsBundle;

	public bool NonEventHidden;

	public bool EventHidden;

	public string EventStartPacific;

	public string EventEndPacific;
}
