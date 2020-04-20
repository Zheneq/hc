﻿using System;
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

	public AbilityUtil_Targeter_TricksterFlare(Ability ability, TricksterAfterImageNetworkBehaviour syncComp, AbilityAreaShape shape, bool penetrateLos, bool includeEnemies, bool includeAllies, bool shapeAroundSelf) : base(ability)
	{
		this.m_shape = shape;
		this.m_penetrateLos = penetrateLos;
		this.m_afterImageSyncComp = syncComp;
		this.m_includeEnemies = includeEnemies;
		this.m_includeAllies = includeAllies;
		this.m_shapeAroundSelf = shapeAroundSelf;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		List<BoardSquare> list = new List<BoardSquare>();
		if (this.m_shapeAroundSelf)
		{
			list.Add(targetingActor.GetCurrentBoardSquare());
		}
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
				{
					if (actorData.GetCurrentBoardSquare() != null)
					{
						list.Add(actorData.GetCurrentBoardSquare());
					}
				}
			}
		}
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count == list.Count)
			{
				goto IL_147;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < list.Count; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		IL_147:
		Dictionary<ActorData, Vector3> dictionary = new Dictionary<ActorData, Vector3>();
		Dictionary<ActorData, int> dictionary2 = new Dictionary<ActorData, int>();
		for (int j = 0; j < list.Count; j++)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, list[j].ToVector3(), list[j]);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, centerOfShape, list[j], this.m_penetrateLos, targetingActor, null, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			using (List<ActorData>.Enumerator enumerator2 = actorsInShape.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
					bool flag = actorData2.GetTeam() == targetingActor.GetTeam();
					if (!flag)
					{
						if (this.m_includeEnemies)
						{
							goto IL_246;
						}
					}
					if (!flag)
					{
						continue;
					}
					if (!this.m_includeAllies || !(actorData2 != targetingActor))
					{
						continue;
					}
					if (validAfterImages.Contains(actorData2))
					{
						continue;
					}
					IL_246:
					Vector3 vector = list[j].ToVector3();
					if (dictionary.ContainsKey(actorData2))
					{
						Dictionary<ActorData, int> dictionary3;
						ActorData key;
						(dictionary3 = dictionary2)[key = actorData2] = dictionary3[key] + 1;
						ActorCover actorCover = actorData2.GetActorCover();
						if (actorCover != null && actorCover.IsInCoverWrt(dictionary[actorData2]))
						{
							if (!actorCover.IsInCoverWrt(vector))
							{
								dictionary[actorData2] = vector;
							}
						}
					}
					else
					{
						dictionary[actorData2] = vector;
						dictionary2[actorData2] = 1;
					}
				}
			}
			this.m_highlights[j].transform.position = centerOfShape;
		}
		using (Dictionary<ActorData, Vector3>.Enumerator enumerator3 = dictionary.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				KeyValuePair<ActorData, Vector3> keyValuePair = enumerator3.Current;
				ActorData key2 = keyValuePair.Key;
				Vector3 value = keyValuePair.Value;
				for (int k = 0; k < dictionary2[key2]; k++)
				{
					AbilityTooltipSubject abilityTooltipSubject;
					if (key2.GetTeam() == targetingActor.GetTeam())
					{
						abilityTooltipSubject = AbilityTooltipSubject.Secondary;
					}
					else
					{
						abilityTooltipSubject = AbilityTooltipSubject.Primary;
					}
					AbilityTooltipSubject subjectType = abilityTooltipSubject;
					base.AddActorInRange(key2, value, targetingActor, subjectType, true);
				}
			}
		}
	}
}
