using System;

public static class LobbyCommonExtensions
{
	public static string GetLocalizedString(this CurrencyType type)
	{
		switch (type)
		{
		case CurrencyType.ISO:
			return StringUtil.TR("ISO", "Rewards");
		case CurrencyType.GGPack:
			return StringUtil.TR("GGBoost", "Rewards");
		case CurrencyType.Experience:
			return StringUtil.TR("Experience", "Inventory");
		case CurrencyType.RankedCurrency:
			return StringUtil.TR("RankedCurrency", "Global");
		case CurrencyType.FreelancerCurrency:
			return StringUtil.TR("FreelancerCurrency", "Global");
		}
		throw new Exception(string.Format("No string for {0}", type));
	}
}
