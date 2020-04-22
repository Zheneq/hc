using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TricksterFlare : AbilityUtil_Targeter
{
	private AbilityAreaShape m_shape;

	private bool m_penetrateLos;

	private bool m_shapeAroundSelf;

	private bool m_includeEnemies;

	private bool m_includeAllies;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	public AbilityUtil_Targeter_TricksterFlare(Ability ability, TricksterAfterImageNetworkBehaviour syncComp, AbilityAreaShape shape, bool penetrateLos, bool includeEnemies, bool includeAllies, bool shapeAroundSelf)
		: base(ability)
	{
		m_shape = shape;
		m_penetrateLos = penetrateLos;
		m_afterImageSyncComp = syncComp;
		m_includeEnemies = includeEnemies;
		m_includeAllies = includeAllies;
		m_shapeAroundSelf = shapeAroundSelf;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		List<BoardSquare> list = new List<BoardSquare>();
		if (m_shapeAroundSelf)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list.Add(targetingActor.GetCurrentBoardSquare());
		}
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (current.GetCurrentBoardSquare() != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						list.Add(current.GetCurrentBoardSquare());
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_highlights != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_highlights.Count == list.Count)
			{
				goto IL_0147;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < list.Count; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		goto IL_0147;
		IL_0147:
		Dictionary<ActorData, Vector3> dictionary = new Dictionary<ActorData, Vector3>();
		Dictionary<ActorData, int> dictionary2 = new Dictionary<ActorData, int>();
		for (int j = 0; j < list.Count; j++)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, list[j].ToVector3(), list[j]);
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, centerOfShape, list[j], m_penetrateLos, targetingActor, null, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					bool flag = current2.GetTeam() == targetingActor.GetTeam();
					if (!flag)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_includeEnemies)
						{
							goto IL_0246;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_includeAllies || !(current2 != targetingActor))
					{
						continue;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (validAfterImages.Contains(current2))
					{
						continue;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					goto IL_0246;
					IL_0246:
					Vector3 vector = list[j].ToVector3();
					if (dictionary.ContainsKey(current2))
					{
						dictionary2[current2]++;
						ActorCover actorCover = current2.GetActorCover();
						if (actorCover != null && actorCover.IsInCoverWrt(dictionary[current2]))
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!actorCover.IsInCoverWrt(vector))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								dictionary[current2] = vector;
							}
						}
					}
					else
					{
						dictionary[current2] = vector;
						dictionary2[current2] = 1;
					}
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_highlights[j].transform.position = centerOfShape;
		}
		using (Dictionary<ActorData, Vector3>.Enumerator enumerator3 = dictionary.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				KeyValuePair<ActorData, Vector3> current3 = enumerator3.Current;
				ActorData key = current3.Key;
				Vector3 value = current3.Value;
				for (int k = 0; k < dictionary2[key]; k++)
				{
					int num;
					if (key.GetTeam() == targetingActor.GetTeam())
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						num = 2;
					}
					else
					{
						num = 1;
					}
					AbilityTooltipSubject subjectType = (AbilityTooltipSubject)num;
					AddActorInRange(key, value, targetingActor, subjectType, true);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
