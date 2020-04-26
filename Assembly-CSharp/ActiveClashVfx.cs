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
			if (!audioEvent.IsNullOrEmpty())
			{
				while (true)
				{
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
			Object.Destroy(m_vfxObj);
		}
		m_vfxObj = null;
	}
}
