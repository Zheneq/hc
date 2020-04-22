using System;
using System.Collections.Generic;

[Serializable]
public class LobbySkinGameplayData
{
	[Serializable]
	public class LobbyPatternGameplayData
	{
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

		public int Index;

		public string Name;

		public bool Hidden;

		public LobbyColorGameplayData[] ColorData;
	}

	public int Index;

	public string Name;

	public bool Hidden;

	public LobbyPatternGameplayData[] PatternData;
}
