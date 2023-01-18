// ROGUES
// SERVER
using System.Collections.Generic;

// empty in reactor
public class Passive_Samurai : Passive
{
#if SERVER
	// added in rogues
	private SamuraiSelfBuff m_selfBuffAbility;
	// added in rogues
	private SamuraiWindBlade m_windBladeAbility;

	// added in rogues
	public int NumEnemyHitWindBlade { get; set; }

	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_selfBuffAbility = Owner.GetAbilityData().GetAbilityOfType<SamuraiSelfBuff>();
		m_windBladeAbility = Owner.GetAbilityData().GetAbilityOfType<SamuraiWindBlade>();
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		HandleTurnStartForWindBlade();
	}

	// added in rogues
	private void HandleTurnStartForWindBlade()
	{
		if (!Owner.IsDead()
		    && m_windBladeAbility != null
		    && m_windBladeAbility.GetShieldingPerEnemyHitNextTurn() > 0
		    && NumEnemyHitWindBlade > 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
			standardActorEffectData.InitWithDefaultValues();
			int absorbAmount = NumEnemyHitWindBlade * m_windBladeAbility.GetShieldingPerEnemyHitNextTurn();
			standardActorEffectData.m_absorbAmount = absorbAmount;
			standardActorEffectData.m_duration = m_windBladeAbility.GetShieldingDuration();
			StandardActorEffect effect = new StandardActorEffect(AsEffectSource(), Owner.GetCurrentBoardSquare(), Owner, Owner, standardActorEffectData);
			actorHitResults.AddEffect(effect);
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_windBladeAbility);
		}
		NumEnemyHitWindBlade = 0;
	}

	// added in rogues
	public override void OnAbilityCastResolved(Ability ability)
	{
		base.OnAbilityCastResolved(ability);
		if (m_selfBuffAbility != null
		    && m_selfBuffAbility.m_selfBuffLastsUntilYouDealDamage
		    && (ability is SamuraiDoubleSlash || ability is SamuraiWindBlade || ability is SamuraiDashAndAimedSlash || ability is SamuraiAfterimageStrike))
		{
			if (ServerEffectManager.Get().GetEffect(Owner, typeof(SamuraiSelfBuffEffect)) is SamuraiSelfBuffEffect samuraiSelfBuffEffect)
			{
				samuraiSelfBuffEffect.SetReadyToEnd();
			}
		}
	}

	// added in rogues
	public override void AddInvalidEvadeDestinations(List<ServerEvadeUtils.EvadeInfo> evades, List<BoardSquare> invalidSquares)
	{
		if (ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(SamuraiAfterimageStrike)))
		{
			invalidSquares.Add(Owner.GetCurrentBoardSquare());
		}
	}
#endif
}
