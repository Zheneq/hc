using UnityEngine;

public class ActiveClashVfx
{
	public GameObject m_vfxObj;

	public BoardSquare m_square;

	public float m_timeCreated;

	public float m_timeToExpire;

	public ActiveClashVfx(BoardSquare square, GameObject vfxPrefab, float timeToExpire, string audioEvent)
	{
		m_square = square;
		m_vfxObj = Object.Instantiate(vfxPrefab, square.GetWorldPosition(), Quaternion.identity);
		m_timeCreated = Time.time;
		m_timeToExpire = timeToExpire;
		if (!(square != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!audioEvent.IsNullOrEmpty())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					AudioManager.PostEvent(audioEvent, square.gameObject);
					return;
				}
			}
			return;
		}
	}

	public void OnEnd()
	{
		if (m_vfxObj != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.Destroy(m_vfxObj);
		}
		m_vfxObj = null;
	}
}
