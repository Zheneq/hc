using System;
using UnityEngine;

public class FriendlyEnemyVFXSelector : MonoBehaviour
{
	public GameObject m_friendlyVFX;

	public GameObject m_enemyVFX;

	public GameObject m_sharedVFX;

	public bool m_hideByMoveOffScreen;

	private Team m_casterTeam;

	private Team m_cachedLocalPlayerTeam;

	private bool m_friendly = true;

	private bool m_initialized;

	private GameObject m_allyFxInstance;

	private GameObject m_enemyFxInstance;

	private GameObject m_currentSharedVFX;

	private void UpdateVFX()
	{
		if (this.m_friendly == FriendlyEnemyVFXSelector.IsFriendly(this.m_casterTeam, this.m_cachedLocalPlayerTeam))
		{
			if (this.m_initialized)
			{
				return;
			}
		}
		this.m_initialized = true;
		this.m_friendly = FriendlyEnemyVFXSelector.IsFriendly(this.m_casterTeam, this.m_cachedLocalPlayerTeam);
		if (this.m_hideByMoveOffScreen)
		{
			if (this.m_friendly)
			{
				if (this.m_enemyFxInstance != null)
				{
					this.HideFxObject(this.m_enemyFxInstance);
				}
			}
			else if (this.m_allyFxInstance != null)
			{
				this.HideFxObject(this.m_allyFxInstance);
			}
		}
		else if (this.m_friendly)
		{
			if (this.m_enemyFxInstance != null)
			{
				UnityEngine.Object.Destroy(this.m_enemyFxInstance);
				this.m_enemyFxInstance = null;
			}
		}
		else if (this.m_allyFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_allyFxInstance);
			this.m_allyFxInstance = null;
		}
		GameObject gameObject;
		if (this.m_friendly)
		{
			gameObject = this.m_friendlyVFX;
		}
		else
		{
			gameObject = this.m_enemyVFX;
		}
		GameObject gameObject2 = gameObject;
		if (gameObject2 != null)
		{
			if (this.m_friendly)
			{
				this.CreateInstanceIfNeeded(gameObject2, ref this.m_allyFxInstance);
			}
			else
			{
				this.CreateInstanceIfNeeded(gameObject2, ref this.m_enemyFxInstance);
			}
		}
	}

	private void HideFxObject(GameObject obj)
	{
		if (obj != null)
		{
			obj.transform.localPosition = new Vector3(-10000f, -10000f, -10000f);
		}
	}

	private unsafe void CreateInstanceIfNeeded(GameObject fxPrefab, ref GameObject instance)
	{
		if (instance == null)
		{
			if (fxPrefab != null)
			{
				instance = UnityEngine.Object.Instantiate<GameObject>(fxPrefab);
				instance.transform.parent = base.transform;
				instance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
			}
		}
		if (instance != null)
		{
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localRotation = Quaternion.identity;
		}
	}

	public void Setup(Team casterTeam)
	{
		this.m_casterTeam = casterTeam;
		Team cachedLocalPlayerTeam;
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			cachedLocalPlayerTeam = GameFlowData.Get().activeOwnedActorData.GetTeam();
		}
		else
		{
			cachedLocalPlayerTeam = Team.TeamA;
		}
		this.m_cachedLocalPlayerTeam = cachedLocalPlayerTeam;
		if (this.m_currentSharedVFX == null && this.m_sharedVFX != null)
		{
			this.m_currentSharedVFX = UnityEngine.Object.Instantiate<GameObject>(this.m_sharedVFX);
			this.m_currentSharedVFX.transform.parent = base.transform;
			this.m_currentSharedVFX.transform.localPosition = Vector3.zero;
			this.m_currentSharedVFX.transform.localRotation = Quaternion.identity;
			this.m_currentSharedVFX.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
		}
		this.UpdateVFX();
	}

	public void SetAttribute(string attributeName, float value)
	{
		GameObject currentActiveVfxInstance = this.GetCurrentActiveVfxInstance();
		if (currentActiveVfxInstance != null)
		{
			Sequence.SetAttribute(currentActiveVfxInstance, attributeName, value);
		}
	}

	private GameObject GetCurrentActiveVfxInstance()
	{
		GameObject result;
		if (this.m_friendly)
		{
			result = this.m_allyFxInstance;
		}
		else
		{
			result = this.m_enemyFxInstance;
		}
		return result;
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
		{
			Team team;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				team = GameFlowData.Get().activeOwnedActorData.GetTeam();
			}
			else
			{
				team = Team.TeamA;
			}
			Team team2 = team;
			if (team2 != this.m_cachedLocalPlayerTeam)
			{
				this.m_cachedLocalPlayerTeam = team2;
				this.UpdateVFX();
			}
		}
	}

	private static bool IsFriendly(Team team0, Team team1)
	{
		bool result;
		if (team0 != team1)
		{
			if (team0 != Team.Invalid)
			{
				result = (team1 == Team.Invalid);
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}
}
