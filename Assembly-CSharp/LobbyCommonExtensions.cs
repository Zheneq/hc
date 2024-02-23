using System;
using System.Text;

public static class LobbyCommonExtensions
{
	public static string GetLocalizedString(this CurrencyType type)
	{
		switch (type)
		{
		case CurrencyType.Experience:
			return StringUtil.TR("Experience", "Inventory");
		case CurrencyType.FreelancerCurrency:
			return StringUtil.TR("FreelancerCurrency", "Global");
		case CurrencyType.GGPack:
			return StringUtil.TR("GGBoost", "Rewards");
		case CurrencyType.ISO:
			return StringUtil.TR("ISO", "Rewards");
		case CurrencyType.RankedCurrency:
			return StringUtil.TR("RankedCurrency", "Global");
		default:
			throw new Exception(new StringBuilder().Append("No string for ").Append(type).ToString());
		}
	}
}
