using System.Collections.Generic;
using UnityEngine;

public static class NPCUtils
{
	public static BoardSquare FindOpenSquareToSpawnOn(BoardSquare idealSquare, Transform actorPrefab)
	{
		BoardSquare boardSquare = null;
		ActorData component = actorPrefab.GetComponent<ActorData>();
		List<BoardSquare> list = new List<BoardSquare>();
		List<BoardSquare> list2 = new List<BoardSquare>();
		if (idealSquare != null)
		{
			list.Add(idealSquare);
		}
		while (list.Count > 0)
		{
			if (boardSquare == null)
			{
				BoardSquare boardSquare2 = list[0];
				list.RemoveAt(0);
				list2.Add(boardSquare2);
				if (SpawnPointManager.Get().CanSpawnOnSquare(component, boardSquare2))
				{
					boardSquare = boardSquare2;
				}
				else if (list.Count + list2.Count < 49)
				{
					List<BoardSquare> result = new List<BoardSquare>();
					Board.Get().GetAllAdjacentSquares(boardSquare2.x, boardSquare2.y, ref result);
					using (List<BoardSquare>.Enumerator enumerator = result.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							BoardSquare current = enumerator.Current;
							if (list.Count + list2.Count >= 49)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										goto end_IL_00aa;
									}
								}
							}
							if (!list.Contains(current))
							{
								if (!list2.Contains(current))
								{
									list.Add(current);
								}
							}
						}
						end_IL_00aa:;
					}
				}
				continue;
			}
			break;
		}
		return boardSquare;
	}
}
