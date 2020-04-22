using System.Collections.Generic;

public class AbilityUtil_Targeter_NekoCharge : AbilityUtil_Targeter_ChargeAoE
{
	private Neko_SyncComponent m_syncComp;

	public AbilityUtil_Targeter_NekoCharge(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS)
		: base(ability, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS)
	{
		m_syncComp = ability.GetComponent<Neko_SyncComponent>();
	}

	protected override bool UseRadiusAroundEnd(AbilityTarget currentTarget)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Contains(boardSquareSafe))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return base.UseRadiusAroundEnd(currentTarget);
					}
				}
			}
		}
		return false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
	}
}
