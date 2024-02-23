using System;
using System.Text;
using UnityEngine;

public class CameraShotSequence : ScriptableObject
{
	[Serializable]
	public class AlternativeCamShotData
	{
		public string m_name;

		public int m_altAnimIndexTauntTrigger;

		public CameraShot[] m_altCameraShots;
	}

	public CharacterType m_characterType;

	public string m_name = "Camera Sequence Name";

	[Tooltip("To differentiate between multiple taunts for the same ability.")]
	[Space(10f)]
	public int m_tauntNumber = 1;

	[Tooltip("The anim index specified on ability, used to determine a match.")]
	public int m_animIndex;

	[Tooltip("Anim index passed to anim network.")]
	public int m_animIndexTauntTrigger;

	public CameraTransitionType m_transitionOutType;

	public CameraShot[] m_cameraShots;

	[HideInInspector]
	public int m_uniqueTauntID;

	[Header("-- Alternate Camera Shots, use if ability can trigger different taunt depending on situation --", order = 1)]
	[Space(20f, order = 0)]
	public AlternativeCamShotData[] m_alternateCameraShots;

	private float m_startDelay;

	private uint m_shotIndex;

	private float m_startTime;

	private int m_altCamShotIndex = -1;

	internal ActorData Actor
	{
		get;
		private set;
	}

	public void OnValidate()
	{
		if (m_characterType > CharacterType.None)
		{
			if (m_characterType < CharacterType.Last)
			{
				return;
			}
		}
		Debug.LogError(new StringBuilder().Append("Taunt ").Append(m_name).Append(" has invalid character type ").Append(m_characterType.ToString()).Append(" and therefore has an invalid id ").Append(m_uniqueTauntID).Append(".").ToString());
	}

	internal void Begin(ActorData actor, int altCamShotIndex)
	{
		m_startDelay = 0f;
		m_shotIndex = 0u;
		m_altCamShotIndex = altCamShotIndex;
		if (m_altCamShotIndex >= 0)
		{
			if (m_alternateCameraShots != null)
			{
				if (m_alternateCameraShots.Length > m_altCamShotIndex)
				{
					goto IL_005e;
				}
			}
			m_altCamShotIndex = -1;
		}
		goto IL_005e;
		IL_005e:
		Actor = actor;
		m_startTime = Time.time;
		if (_001D())
		{
			Debug.LogWarning(GetDebugDescription());
		}
		if (m_startDelay != 0f)
		{
			return;
		}
		while (true)
		{
			CameraShot[] runtimeCameraShotsArray = GetRuntimeCameraShotsArray();
			CameraShot cameraShot = runtimeCameraShotsArray[m_shotIndex];
			cameraShot.Begin(m_shotIndex, Actor);
			if (_001D())
			{
				while (true)
				{
					Debug.LogWarning(new StringBuilder().Append("[Camera Shot] BEGIN ").Append(Time.time - m_startTime).Append(" with 0 delay").ToString());
					return;
				}
			}
			return;
		}
	}

	internal bool Update()
	{
		bool flag = false;
		CameraShot[] runtimeCameraShotsArray = GetRuntimeCameraShotsArray();
		CameraShot cameraShot = runtimeCameraShotsArray[m_shotIndex];
		if (m_startDelay > 0f)
		{
			m_startDelay -= Time.deltaTime;
			if (m_startDelay > 0f)
			{
				while (true)
				{
					return true;
				}
			}
			cameraShot.Begin(m_shotIndex, Actor);
			cameraShot.SetElapsedTime(Time.time - m_startTime);
			if (_001D())
			{
				Debug.LogWarning(new StringBuilder().Append("[Camera Shot] BEGIN ").Append(Time.time - m_startTime).Append(" seconds after begin").ToString());
			}
		}
		else if (!cameraShot.Update())
		{
			object obj;
			if (m_shotIndex + 1 == runtimeCameraShotsArray.Length)
			{
				obj = null;
			}
			else
			{
				obj = runtimeCameraShotsArray[m_shotIndex + 1];
			}
			CameraShot cameraShot2 = (CameraShot)obj;
			cameraShot.End(Actor);
			if (_001D())
			{
				Debug.LogWarning(new StringBuilder().Append("[Camera Shot] END ").Append(Time.time - m_startTime).Append(" seconds after begin").ToString());
			}
			if (cameraShot2 != null)
			{
				m_shotIndex++;
				cameraShot2.Begin(m_shotIndex, Actor);
				if (_001D())
				{
					Debug.LogWarning(new StringBuilder().Append("[Camera Shot] BEGIN ").Append(Time.time - m_startTime).Append(" seconds after begin").ToString());
				}
			}
			else
			{
				flag = true;
				CameraManager.Get().OnSpecialCameraShotBehaviorDisable(m_transitionOutType);
			}
		}
		return !flag;
	}

	private CameraShot[] GetRuntimeCameraShotsArray()
	{
		if (m_altCamShotIndex >= 0 && m_alternateCameraShots.Length > m_altCamShotIndex)
		{
			CameraShot[] altCameraShots = m_alternateCameraShots[m_altCamShotIndex].m_altCameraShots;
			if (altCameraShots != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return altCameraShots;
					}
				}
			}
		}
		return m_cameraShots;
	}

	private bool _001D()
	{
		int result;
		if (Application.isEditor && DebugParameters.Get() != null)
		{
			result = (DebugParameters.Get().GetParameterAsBool("TraceCameraTransitions") ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public string GetDebugDescription()
	{
		string str = "-------------------------------------------------\n";
		str = new StringBuilder().Append(str).Append("[Shot Sequence Name] ").Append(m_name).Append("\n").ToString();
		str = new StringBuilder().Append(str).Append("[Index] ").Append(m_animIndex).Append("\n").ToString();
		str = new StringBuilder().Append(str).Append("[Transition Out Type] ").Append(m_transitionOutType.ToString()).Append("\n").ToString();
		float num = 0f;
		for (int i = 0; i < m_cameraShots.Length; i++)
		{
			str = new StringBuilder().Append(str).Append("-- Shot ").Append(i + 1).Append(" --\n").ToString();
			str += m_cameraShots[i].GetDebugDescription("    ");
			num += m_cameraShots[i].m_duration;
			str = new StringBuilder().Append(str).Append("(ends at time ").Append(num).Append(")\n\n").ToString();
		}
		while (true)
		{
			return str;
		}
	}
}
