﻿using System;
using System.Collections.Generic;

public class TeamBalanceElement
{
	public static BestSolutions.BalanceResults Balance(List<ELOProvider> players, BestSolutions.ILegalityValidator validator, float percentMaxTeamEloDifference, double rnd, FreelancerDuplicationRuleTypes dupRule)
	{
		if (players.Count >= 2)
		{
			if (players.Count % 2 != 1)
			{
				float num = 0f;
				using (List<ELOProvider>.Enumerator enumerator = players.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ELOProvider eloprovider = enumerator.Current;
						if (eloprovider.ELO <= 0f)
						{
							return BestSolutions.BalanceResults.MakeError("InternalError", "Global");
						}
						num += eloprovider.ELO;
					}
				}
				List<int> list = new List<int>();
				for (int i = 1; i < players.Count; i++)
				{
					if (i < players.Count / 2)
					{
						list.Add(1);
					}
					else
					{
						list.Add(0);
					}
				}
				float maxTeamDifference = num * percentMaxTeamEloDifference / 200f;
				BestSolutions bestSolutions = new BestSolutions
				{
					m_duplicationRule = dupRule
				};
				bool flag = false;
				while (!flag)
				{
					float num2 = players[players.Count - 1].ELO;
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j] == 1)
						{
							num2 += players[j].ELO;
						}
					}
					float num3 = num - num2;
					float aVbEloDifference = num2 - num3;
					bestSolutions.Register(list, players, aVbEloDifference, maxTeamDifference, validator);
					int num4 = -1;
					int num5 = 0;
					int num6 = 0;
					bool flag2 = false;
					while (num4 == -1)
					{
						if (num5 >= list.Count)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								goto IL_21E;
							}
						}
						else if (list[num5] == 1)
						{
							if (!flag2)
							{
								flag2 = true;
								num6 = 0;
							}
							else
							{
								num6++;
							}
							num5++;
						}
						else if (!flag2)
						{
							num5++;
						}
						else
						{
							num4 = num5;
						}
					}
					IL_21E:
					if (num4 != -1)
					{
						for (int k = 0; k < num4; k++)
						{
							if (k < num6)
							{
								list[k] = 1;
							}
							else
							{
								list[k] = 0;
							}
						}
						list[num4] = 1;
					}
					else
					{
						flag = true;
					}
				}
				BestSolutions.BalanceResults result = bestSolutions.Resolve(rnd);
				for (int l = 0; l < players.Count; l++)
				{
					players[l].Team = ((!bestSolutions.OnTeamA(l)) ? Team.TeamB : Team.TeamA);
				}
				return result;
			}
		}
		return BestSolutions.BalanceResults.MakeError("OnlyBalanceEvenTeams", "TeamBalance");
	}
}
