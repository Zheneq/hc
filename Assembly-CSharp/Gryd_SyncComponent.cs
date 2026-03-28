using UnityEngine.Networking;

public class Gryd_SyncComponent : NetworkBehaviour
{
    public GridPos m_bombLocation = GridPos.s_invalid;

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        bool result = base.OnSerialize(writer, initialState);
        if (m_bombLocation.x <= 0 && m_bombLocation.y <= 0)
        {
            writer.Write((byte)0);
            writer.Write((byte)0);
        }
        else
        {
            writer.Write((byte)m_bombLocation.x);
            writer.Write((byte)m_bombLocation.y);
        }

        return result;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);
        int x = reader.ReadByte();
        int y = reader.ReadByte();
        if (x > 0 && y > 0)
        {
            m_bombLocation = new GridPos(x, y, Board.Get().BaselineHeight);
        }
        else
        {
            m_bombLocation = GridPos.s_invalid;
        }
    }

    private void UNetVersion()
    {
    }
}