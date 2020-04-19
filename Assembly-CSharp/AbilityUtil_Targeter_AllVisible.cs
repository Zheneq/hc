using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AllVisible : AbilityUtil_Targeter
{
	public AbilityUtil_Targeter_AllVisible.ShouldAddActorDelegate m_shouldAddActorDelegate;

	private AbilityUtil_Targeter_AllVisible.DamageOriginType m_damageOriginType;

	public AbilityUtil_Targeter_AllVisible(Ability ability, bool includeEnemies, bool includeAllies, bool includeSelf, AbilityUtil_Targeter_AllVisible.DamageOriginType damageOriginType = AbilityUtil_Targeter_AllVisible.DamageOriginType.CasterPos) : base(ability)
	{
		this.m_affectsEnemies = includeEnemies;
		this.m_affectsAllies = includeAllies;
		this.m_affectsTargetingActor = includeSelf;
		this.m_damageOriginType = damageOriginType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		if (GameFlowData.Get() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AllVisible.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				List<ActorData> actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
				for (int i = 0; i < actorsVisibleToActor.Count; i++)
				{
					ActorData actorData = actorsVisibleToActor[i];
					if (!actorData.\u000E())
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!actorData.IgnoreForAbilityHits)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (actorData == targetingActor && this.m_affectsTargetingActor)
							{
								goto IL_11E;
							}
							if (actorData != targetingActor && actorData.\u000E() == targetingActor.\u000E())
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.m_affectsAllies)
								{
									goto IL_11E;
								}
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							bool flag;
							if (actorData.\u000E() != targetingActor.\u000E())
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = this.m_affectsEnemies;
							}
							else
							{
								flag = false;
							}
							IL_11F:
							bool flag2 = flag;
							if (flag2)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.m_shouldAddActorDelegate != null)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!this.m_shouldAddActorDelegate(actorData, targetingActor))
									{
										goto IL_180;
									}
								}
								Vector3 vector;
								if (this.m_damageOriginType == AbilityUtil_Targeter_AllVisible.DamageOriginType.CasterPos)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									vector = targetingActor.\u0016();
								}
								else
								{
									vector = actorData.\u0016();
								}
								Vector3 damageOrigin = vector;
								base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
								goto IL_180;
							}
							goto IL_180;
							IL_11E:
							flag = true;
							goto IL_11F;
						}
					}
					IL_180:;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	public delegate bool ShouldAddActorDelegate(ActorData potentialActor, ActorData caster);

	public enum DamageOriginType
	{
		CasterPos,
		TargetPos
	}
}
