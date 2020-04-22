using System;
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
			while (true)
			{
				switch (6)
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
		}
		Debug.LogError("Taunt " + m_name + " has invalid character type " + m_characterType.ToString() + " and therefore has an invalid id " + m_uniqueTauntID + ".");
	}

	internal void Begin(ActorData actor, int altCamShotIndex)
	{
		m_startDelay = 0f;
		m_shotIndex = 0u;
		m_altCamShotIndex = altCamShotIndex;
		if (m_altCamShotIndex >= 0)
		{
			while (true)
			{
				switch (2)
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
			if (m_alternateCameraShots != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			Debug.LogWarning(GetDebugDescription());
		}
		if (m_startDelay != 0f)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			CameraShot[] runtimeCameraShotsArray = GetRuntimeCameraShotsArray();
			CameraShot cameraShot = runtimeCameraShotsArray[m_shotIndex];
			cameraShot.Begin(m_shotIndex, Actor);
			if (_001D())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - m_startTime) + " with 0 delay");
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
			while (true)
			{
				switch (7)
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
			m_startDelay -= Time.deltaTime;
			if (m_startDelay > 0f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					return true;
				}
			}
			cameraShot.Begin(m_shotIndex, Actor);
			cameraShot.SetElapsedTime(Time.time - m_startTime);
			if (_001D())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - m_startTime) + " seconds after begin");
			}
		}
		else if (!cameraShot.Update())
		{
			object obj;
			if (m_shotIndex + 1 == runtimeCameraShotsArray.Length)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
				Debug.LogWarning("[Camera Shot] END " + (Time.time - m_startTime) + " seconds after begin");
			}
			if (cameraShot2 != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_shotIndex++;
				cameraShot2.Begin(m_shotIndex, Actor);
				if (_001D())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - m_startTime) + " seconds after begin");
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
			while (true)
			{
				switch (5)
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
			while (true)
			{
				switch (5)
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
		str = str + "[Shot Sequence Name] " + m_name + "\n";
		str = str + "[Index] " + m_animIndex + "\n";
		str = str + "[Transition Out Type] " + m_transitionOutType.ToString() + "\n";
		float num = 0f;
		for (int i = 0; i < m_cameraShots.Length; i++)
		{
			str = str + "-- Shot " + (i + 1) + " --\n";
			str += m_cameraShots[i].GetDebugDescription("    ");
			num += m_cameraShots[i].m_duration;
			str = str + "(ends at time " + num + ")\n\n";
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return str;
		}
	}
}
