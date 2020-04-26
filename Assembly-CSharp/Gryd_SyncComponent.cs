using UnityEngine.Networking;

public class Gryd_SyncComponent : NetworkBehaviour
{
	public GridPos m_bombLocation = GridPos.s_invalid;

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		bool result = base.OnSerialize(writer, initialState);
		if (m_bombLocation.x <= 0)
		{
			if (m_bombLocation.y <= 0)
			{
				writer.Write((byte)0);
				writer.Write((byte)0);
				goto IL_0074;
			}
		}
		writer.Write((byte)m_bombLocation.x);
		writer.Write((byte)m_bombLocation.y);
		goto IL_0074;
		IL_0074:
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		base.OnDeserialize(reader, initialState);
		int num = reader.ReadByte();
		int num2 = reader.ReadByte();
		if (num > 0)
		{
			if (num2 > 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_bombLocation = new GridPos(num, num2, Board.Get().BaselineHeight);
						return;
					}
				}
			}
		}
		m_bombLocation = GridPos.s_invalid;
	}

	private void UNetVersion()
	{
	}
}
