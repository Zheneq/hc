// ROGUES
// SERVER
//empty in reactor & rogues
public class Passive_Neko : Passive
{
#if SERVER
    // custom
    private Neko_SyncComponent m_syncComp;
    
    // custom
    protected override void OnStartup()
    {
        base.OnStartup();
        m_syncComp = Owner.GetComponent<Neko_SyncComponent>();
    }
    
    // custom
    public override void OnAbilityPhaseEnd(AbilityPriority phase)
    {
        base.OnAbilityPhaseEnd(phase);
        if (phase == AbilityPriority.Evasion)
        {
            m_syncComp?.ServerUpdateActorsInDiscPath();  // TODO NEKO what about mouse trap?
            // TODO NEKO verify it works with dashes
        }
    }
#endif
}
