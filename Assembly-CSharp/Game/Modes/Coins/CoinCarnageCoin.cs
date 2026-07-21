// SERVER
// ROGUES
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

// same in rogues
public class CoinCarnageCoin : NetworkBehaviour
{
    private BoardSquare m_boardSquare;

    [SyncVar(hook = "HookSetPickedUp")]
    private bool m_pickedUp;

    public bool Networkm_pickedUp
    {
        get => m_pickedUp;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetPickedUp(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_pickedUp, 1u);
        }
    }

    public void Initialize(BoardSquare square)
    {
        m_boardSquare = square;
    }

    public BoardSquare GetSquare()
    {
        return m_boardSquare;
    }

    public bool IsPickedUp()
    {
        return m_pickedUp;
    }

    [Server]
    public void PickUp(ActorData actor)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("[Server] function 'System.Void CoinCarnageCoin::PickUp(ActorData)' called on client");
            return;
        }

        if (!m_pickedUp)
        {
            Networkm_pickedUp = true;
        }
    }

    private void HookSetPickedUp(bool value)
    {
        Networkm_pickedUp = value;
        if (value)
        {
            gameObject.SetActive(false);
        }
    }

    [Server]
    public void Destroy()
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("[Server] function 'System.Void CoinCarnageCoin::Destroy()' called on client");
            return;
        }

        NetworkServer.Destroy(gameObject);
    }

    private void UNetVersion()
    {
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            writer.Write(m_pickedUp);
            return true;
        }

        bool isModified = false;
        if ((syncVarDirtyBits & 1) != 0)
        {
            if (!isModified)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                isModified = true;
            }

            writer.Write(m_pickedUp);
        }

        if (!isModified)
        {
            writer.WritePackedUInt32(syncVarDirtyBits);
        }

        return isModified;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            m_pickedUp = reader.ReadBoolean();
            return;
        }

        int dirtyBits = (int)reader.ReadPackedUInt32();
        if ((dirtyBits & 1) != 0)
        {
            HookSetPickedUp(reader.ReadBoolean());
        }
    }
}