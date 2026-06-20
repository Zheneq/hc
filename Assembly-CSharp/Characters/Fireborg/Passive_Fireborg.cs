// ROGUES
// SERVER

// empty in reactor, missing in rogues. custom
public class Passive_Fireborg : Passive
{
#if SERVER
    private Fireborg_SyncComponent m_syncComp;
    private FireborgReactLasers m_reactAbility;
    
    protected override void OnStartup()
    {
        base.OnStartup();

        m_syncComp = Owner.GetComponent<Fireborg_SyncComponent>();
        m_reactAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(FireborgReactLasers)) as FireborgReactLasers;
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        
        m_syncComp.m_actorsInGroundFireOnTurnStart.Clear();
        m_syncComp.m_actorsIgnitedThisTurn.Clear();
        m_syncComp.m_actorsIgnitedThisTurn_Fake.Clear();
        m_syncComp.m_actorsHitByGroundFireThisTurn.Clear();
        m_syncComp.m_actorsHitByGroundFireThisTurn_Fake.Clear();

        if (m_syncComp.m_pendingShield > 0)
        {
            ApplyShieldEffect(m_syncComp.m_pendingShield);
            m_syncComp.m_pendingShield = 0;
        }
    }

    private void ApplyShieldEffect(int totalShielding)
    {
        StandardActorEffect shieldEffect = GenericAbility_Container.CreateShieldEffect(
            m_reactAbility,
            Owner,
            totalShielding,
            1);
                
        ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
        actorHitResults.AddEffect(shieldEffect);
        MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
            Owner,
            Owner,
            actorHitResults,
            m_reactAbility);
    }
#endif
}
