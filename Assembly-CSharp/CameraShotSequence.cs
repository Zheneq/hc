using System;
using UnityEngine;

public class CameraShotSequence : ScriptableObject
{
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
	public CameraShotSequence.AlternativeCamShotData[] m_alternateCameraShots;

	private float m_startDelay;

	private uint m_shotIndex;

	private float m_startTime;

	private int m_altCamShotIndex = -1;

	internal ActorData Actor { get; private set; }

	public void OnValidate()
	{
		if (this.m_characterType > CharacterType.None)
		{
			if (this.m_characterType < CharacterType.Last)
			{
				return;
			}
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Taunt ",
			this.m_name,
			" has invalid character type ",
			this.m_characterType.ToString(),
			" and therefore has an invalid id ",
			this.m_uniqueTauntID,
			"."
		}));
	}

	internal void Begin(ActorData actor, int altCamShotIndex)
	{
		this.m_startDelay = 0f;
		this.m_shotIndex = 0U;
		this.m_altCamShotIndex = altCamShotIndex;
		if (this.m_altCamShotIndex >= 0)
		{
			if (this.m_alternateCameraShots != null)
			{
				if (this.m_alternateCameraShots.Length > this.m_altCamShotIndex)
				{
					goto IL_5E;
				}
			}
			this.m_altCamShotIndex = -1;
		}
		IL_5E:
		this.Actor = actor;
		this.m_startTime = Time.time;
		if (this.symbol_001D())
		{
			Debug.LogWarning(this.GetDebugDescription());
		}
		if (this.m_startDelay == 0f)
		{
			CameraShot[] runtimeCameraShotsArray = this.GetRuntimeCameraShotsArray();
			CameraShot cameraShot = runtimeCameraShotsArray[(int)((UIntPtr)this.m_shotIndex)];
			cameraShot.Begin(this.m_shotIndex, this.Actor);
			if (this.symbol_001D())
			{
				Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - this.m_startTime) + " with 0 delay");
			}
		}
	}

	internal bool Update()
	{
		bool flag = false;
		CameraShot[] runtimeCameraShotsArray = this.GetRuntimeCameraShotsArray();
		CameraShot cameraShot = runtimeCameraShotsArray[(int)((UIntPtr)this.m_shotIndex)];
		if (this.m_startDelay > 0f)
		{
			this.m_startDelay -= Time.deltaTime;
			if (this.m_startDelay > 0f)
			{
				return true;
			}
			cameraShot.Begin(this.m_shotIndex, this.Actor);
			cameraShot.SetElapsedTime(Time.time - this.m_startTime);
			if (this.symbol_001D())
			{
				Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - this.m_startTime) + " seconds after begin");
			}
		}
		else if (!cameraShot.Update())
		{
			CameraShot cameraShot2;
			if ((ulong)(this.m_shotIndex + 1U) == (ulong)((long)runtimeCameraShotsArray.Length))
			{
				cameraShot2 = null;
			}
			else
			{
				cameraShot2 = runtimeCameraShotsArray[(int)((UIntPtr)(this.m_shotIndex + 1U))];
			}
			CameraShot cameraShot3 = cameraShot2;
			cameraShot.End(this.Actor);
			if (this.symbol_001D())
			{
				Debug.LogWarning("[Camera Shot] END " + (Time.time - this.m_startTime) + " seconds after begin");
			}
			if (cameraShot3 != null)
			{
				this.m_shotIndex += 1U;
				cameraShot3.Begin(this.m_shotIndex, this.Actor);
				if (this.symbol_001D())
				{
					Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - this.m_startTime) + " seconds after begin");
				}
			}
			else
			{
				flag = true;
				CameraManager.Get().OnSpecialCameraShotBehaviorDisable(this.m_transitionOutType);
			}
		}
		return !flag;
	}

	private CameraShot[] GetRuntimeCameraShotsArray()
	{
		if (this.m_altCamShotIndex >= 0 && this.m_alternateCameraShots.Length > this.m_altCamShotIndex)
		{
			CameraShot[] altCameraShots = this.m_alternateCameraShots[this.m_altCamShotIndex].m_altCameraShots;
			if (altCameraShots != null)
			{
				return altCameraShots;
			}
		}
		return this.m_cameraShots;
	}

	private bool symbol_001D()
	{
		bool result;
		if (Application.isEditor && DebugParameters.Get() != null)
		{
			result = DebugParameters.Get().GetParameterAsBool("TraceCameraTransitions");
		}
		else
		{
			result = false;
		}
		return result;
	}

	public string GetDebugDescription()
	{
		string text = "-------------------------------------------------\n";
		text = text + "[Shot Sequence Name] " + this.m_name + "\n";
		text = text + "[Index] " + this.m_animIndex.ToString() + "\n";
		text = text + "[Transition Out Type] " + this.m_transitionOutType.ToString() + "\n";
		float num = 0f;
		for (int i = 0; i < this.m_cameraShots.Length; i++)
		{
			text = text + "-- Shot " + (i + 1).ToString() + " --\n";
			text += this.m_cameraShots[i].GetDebugDescription("    ");
			num += this.m_cameraShots[i].m_duration;
			text = text + "(ends at time " + num.ToString() + ")\n\n";
		}
		return text;
	}

	[Serializable]
	public class AlternativeCamShotData
	{
		public string m_name;

		public int m_altAnimIndexTauntTrigger;

		public CameraShot[] m_altCameraShots;
	}
}
