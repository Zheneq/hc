// ROGUES
// SERVER
//empty in reactor & rogues

using UnityEngine.Networking;

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
    public override void OnTurnEnd()
    {
        if (!NetworkServer.active)
        {
            return;
        }
        base.OnTurnEnd();
        if (!ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(NekoFanOfDiscs)))
        {
            m_syncComp.Networkm_numUltConsecUsedTurns = 0;
        }
    }
#endif
}
