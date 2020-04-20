using System;
using UnityEngine;

public class ActiveClashVfx
{
	public GameObject m_vfxObj;

	public BoardSquare m_square;

	public float m_timeCreated;

	public float m_timeToExpire;

	public ActiveClashVfx(BoardSquare square, GameObject vfxPrefab, float timeToExpire, string audioEvent)
	{
		this.m_square = square;
		this.m_vfxObj = UnityEngine.Object.Instantiate<GameObject>(vfxPrefab, square.GetWorldPosition(), Quaternion.identity);
		this.m_timeCreated = Time.time;
		this.m_timeToExpire = timeToExpire;
		if (square != null)
		{
			if (!audioEvent.IsNullOrEmpty())
			{
				AudioManager.PostEvent(audioEvent, square.gameObject);
			}
		}
	}

	public void OnEnd()
	{
		if (this.m_vfxObj != null)
		{
			UnityEngine.Object.Destroy(this.m_vfxObj);
		}
		this.m_vfxObj = null;
	}
}
