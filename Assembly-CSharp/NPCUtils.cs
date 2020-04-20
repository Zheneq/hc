using System;
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
		IL_12E:
		while (list.Count > 0)
		{
			if (!(boardSquare == null))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					return boardSquare;
				}
			}
			else
			{
				BoardSquare boardSquare2 = list[0];
				list.RemoveAt(0);
				list2.Add(boardSquare2);
				if (SpawnPointManager.Get().CanSpawnOnSquare(component, boardSquare2, false))
				{
					boardSquare = boardSquare2;
				}
				else if (list.Count + list2.Count < 0x31)
				{
					List<BoardSquare> list3 = new List<BoardSquare>();
					Board.Get().GetAllAdjacentSquares(boardSquare2.x, boardSquare2.y, ref list3);
					using (List<BoardSquare>.Enumerator enumerator = list3.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare item = enumerator.Current;
							if (list.Count + list2.Count >= 0x31)
							{
								goto IL_12E;
							}
							if (!list.Contains(item))
							{
								if (!list2.Contains(item))
								{
									list.Add(item);
								}
							}
						}
					}
				}
			}
		}
		return boardSquare;
	}
}
