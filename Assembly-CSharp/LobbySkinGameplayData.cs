using System;
using System.Collections.Generic;

[Serializable]
public class LobbySkinGameplayData
{
	public int Index;

	public string Name;

	public bool Hidden;

	public LobbySkinGameplayData.LobbyPatternGameplayData[] PatternData;

	[Serializable]
	public class LobbyPatternGameplayData
	{
		public int Index;

		public string Name;

		public bool Hidden;

		public LobbySkinGameplayData.LobbyPatternGameplayData.LobbyColorGameplayData[] ColorData;

		[Serializable]
		public class LobbyColorGameplayData
		{
			public int Index;

			public string Name;

			public bool Hidden;

			public int RequiredLevelForEquip;

			public List<LobbyCharacterLinkedColor> LinkedColors;

			public CountryPrices Prices;
		}
	}
}
