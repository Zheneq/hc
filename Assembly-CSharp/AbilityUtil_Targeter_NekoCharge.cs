public class AbilityUtil_Targeter_NekoCharge : AbilityUtil_Targeter_ChargeAoE
{
	private Neko_SyncComponent m_syncComp;

	public AbilityUtil_Targeter_NekoCharge(
		Ability ability,
		float radiusAroundStart,
		float radiusAroundEnd,
		float rangeFromDir,
		int maxTargets,
		bool ignoreTargetsCover,
		bool penetrateLoS)
		: base(ability, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS)
	{
		m_syncComp = ability.GetComponent<Neko_SyncComponent>();
	}

	protected override bool UseRadiusAroundEnd(AbilityTarget currentTarget)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		return m_syncComp != null
		       && m_syncComp.GetActiveDiscSquares().Contains(targetSquare)
		       && base.UseRadiusAroundEnd(currentTarget);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}
}
