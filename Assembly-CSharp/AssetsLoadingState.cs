using UnityEngine;

public class AssetsLoadingState
{
	private float m_levelLoadProgress;

	private float m_characterLoadProgress;

	private float m_vfxPreloadProgress;

	private float m_spawningProgress;

	public float TotalProgress => m_levelLoadProgress * 0.4f + m_characterLoadProgress * 0.2f + m_vfxPreloadProgress * 0.2f + m_spawningProgress * 0.2f;

	public float LevelLoadProgress
	{
		get
		{
			return m_levelLoadProgress;
		}
		set
		{
			m_levelLoadProgress = Mathf.Max(m_levelLoadProgress, value);
		}
	}

	public float CharacterLoadProgress
	{
		get
		{
			return m_characterLoadProgress;
		}
		set
		{
			m_characterLoadProgress = Mathf.Max(m_characterLoadProgress, value);
		}
	}

	public float VfxPreloadProgress
	{
		get
		{
			return m_vfxPreloadProgress;
		}
		set
		{
			m_vfxPreloadProgress = Mathf.Max(m_vfxPreloadProgress, value);
		}
	}

	public float SpawningProgress
	{
		get
		{
			return m_spawningProgress;
		}
		set
		{
			m_spawningProgress = Mathf.Max(m_spawningProgress, value);
		}
	}

	public void Reset()
	{
		m_levelLoadProgress = 0f;
		m_characterLoadProgress = 0f;
		m_vfxPreloadProgress = 0f;
		m_spawningProgress = 0f;
	}
}
