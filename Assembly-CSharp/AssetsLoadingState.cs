using System;
using UnityEngine;

public class AssetsLoadingState
{
	private float m_levelLoadProgress;

	private float m_characterLoadProgress;

	private float m_vfxPreloadProgress;

	private float m_spawningProgress;

	public float TotalProgress
	{
		get
		{
			return this.m_levelLoadProgress * 0.4f + this.m_characterLoadProgress * 0.2f + this.m_vfxPreloadProgress * 0.2f + this.m_spawningProgress * 0.2f;
		}
	}

	public float LevelLoadProgress
	{
		get
		{
			return this.m_levelLoadProgress;
		}
		set
		{
			this.m_levelLoadProgress = Mathf.Max(this.m_levelLoadProgress, value);
		}
	}

	public float CharacterLoadProgress
	{
		get
		{
			return this.m_characterLoadProgress;
		}
		set
		{
			this.m_characterLoadProgress = Mathf.Max(this.m_characterLoadProgress, value);
		}
	}

	public float VfxPreloadProgress
	{
		get
		{
			return this.m_vfxPreloadProgress;
		}
		set
		{
			this.m_vfxPreloadProgress = Mathf.Max(this.m_vfxPreloadProgress, value);
		}
	}

	public float SpawningProgress
	{
		get
		{
			return this.m_spawningProgress;
		}
		set
		{
			this.m_spawningProgress = Mathf.Max(this.m_spawningProgress, value);
		}
	}

	public void Reset()
	{
		this.m_levelLoadProgress = 0f;
		this.m_characterLoadProgress = 0f;
		this.m_vfxPreloadProgress = 0f;
		this.m_spawningProgress = 0f;
	}
}
