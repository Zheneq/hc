using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class TargetFilterHelper
	{
		public static bool ActorMeetsConditions(TargetFilterConditions filters, ActorData targetActor, ActorData caster, ActorHitContext actorHitContext, ContextVars abilityContext)
		{
			return targetActor != null
				&& caster != null
				&& TargetFilterHelper.PassesTeamFilter(filters.m_teamFilter, targetActor, caster)
				&& TargetFilterHelper.PassContextCompareFilters(filters.m_numCompareConditions, actorHitContext, abilityContext);
		}

		public static bool PassesTeamFilter(TeamFilter teamFilter, ActorData targetActor, ActorData caster)
		{
			if (targetActor != null && caster != null)
			{
				bool isAlly = targetActor.GetTeam() == caster.GetTeam();
				bool isSelf = targetActor == caster;
				return teamFilter == TeamFilter.Any
                    || teamFilter == TeamFilter.EnemyIncludingTarget && !isAlly
                    || teamFilter == TeamFilter.AllyIncludingSelf && isAlly
                    || teamFilter == TeamFilter.AllyExcludingSelf && isAlly && !isSelf
					|| teamFilter == TeamFilter.SelfOnly && isSelf;

			}
			return false;
		}

		public static bool PassContextCompareFilters(List<NumericContextValueCompareCond> conditions, ActorHitContext actorHitContext, ContextVars abilityContext)
		{
			bool result = true;
			if (conditions == null)
			{
				return true;
			}
			for (int i = 0; i < conditions.Count; i++)
			{
				if (!result)
				{
					return false;
				}
				NumericContextValueCompareCond condition = conditions[i];
				if (condition.m_compareOp == ContextCompareOp.Ignore
					|| string.IsNullOrEmpty(condition.m_contextName))
				{
					continue;
				}

				int contextKey = condition.GetContextKey();

				ContextVars contextVars = actorHitContext.m_contextVars;
				if (condition.m_nonActorSpecificContext)
				{
					contextVars = abilityContext;
				}

				float actualValue = 0f;
				bool isValuePresent = false;
				if (contextVars.HasVar(contextKey, ContextValueType.Int))
				{
					actualValue = contextVars.GetValueInt(contextKey);
					isValuePresent = true;
				}
				else if (contextVars.HasVar(contextKey, ContextValueType.Float))
				{
					actualValue = contextVars.GetValueFloat(contextKey);
					isValuePresent = true;
				}

				float testValue = condition.m_testValue;
				ContextCompareOp compareOp = condition.m_compareOp;
				if (isValuePresent)
				{
					if (!(compareOp == ContextCompareOp.Equals && testValue == actualValue
						|| compareOp == ContextCompareOp.EqualsRoundToInt && Mathf.RoundToInt(testValue) == Mathf.RoundToInt(actualValue)
						|| compareOp == ContextCompareOp.GreaterThan && actualValue > testValue
						|| compareOp == ContextCompareOp.GreaterThanOrEqual && actualValue >= testValue
						|| compareOp == ContextCompareOp.LessThan && actualValue < testValue
						|| compareOp == ContextCompareOp.LessThanOrEqual && actualValue <= testValue))
					{
						result = false;
					}
				}
				if (!isValuePresent && !condition.m_ignoreIfNoContext)
				{
					result = false;
				}
			}
			return result;
		}
	}
}
