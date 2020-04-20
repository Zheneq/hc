using System;
using UnityEngine.Networking;

public class Gryd_SyncComponent : NetworkBehaviour
{
	public GridPos m_bombLocation = GridPos.s_invalid;

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		bool result = base.OnSerialize(writer, initialState);
		if (this.m_bombLocation.x <= 0)
		{
			if (this.m_bombLocation.y <= 0)
			{
				writer.Write(0);
				writer.Write(0);
				return result;
			}
		}
		writer.Write((byte)this.m_bombLocation.x);
		writer.Write((byte)this.m_bombLocation.y);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		base.OnDeserialize(reader, initialState);
		int num = (int)reader.ReadByte();
		int num2 = (int)reader.ReadByte();
		if (num > 0)
		{
			if (num2 > 0)
			{
				this.m_bombLocation = new GridPos(num, num2, Board.Get().BaselineHeight);
				return;
			}
		}
		this.m_bombLocation = GridPos.s_invalid;
	}

	private void UNetVersion()
	{
	}
}
