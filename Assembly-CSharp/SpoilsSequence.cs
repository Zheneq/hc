using System;
using UnityEngine;

public class SpoilsSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_inaccessibleFxPrefab;

	public float m_projectileSpeed;

	public float m_projectileAcceleration = 0.25f;

	[AudioEvent(false)]
	public string m_audioEvent;

	public float m_startHeight = 1f;

	public float m_maxHeight = 4f;

	private GameObject m_fx;

	private CRSpline m_spline;

	private float m_splineSpeed;

	private float m_splineTraveled;

	private float m_curSplineSpeed;

	private float m_splineAcceleration;

	public Team m_pickupTeam = Team.Objects;

	private bool m_didSetFinalPos;

	private bool m_ignoreSpawnSpline;

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
		this.m_initialized = false;
	}

	private void SpawnFX()
	{
		Vector3[] array = new Vector3[]
		{
			base.TargetPos + Vector3.down * (this.m_maxHeight - this.m_startHeight),
			base.TargetPos + Vector3.up * this.m_startHeight,
			base.TargetPos + Vector3.up * this.m_maxHeight,
			base.TargetPos,
			base.TargetPos + Vector3.down * this.m_maxHeight
		};
		this.m_spline = new CRSpline(array);
		float num = (array[1] - array[2]).magnitude + (array[2] - array[3]).magnitude;
		float num2 = num / this.m_projectileSpeed;
		this.m_splineSpeed = 1f / num2;
		this.m_splineAcceleration = this.m_projectileAcceleration * this.m_splineSpeed / this.m_projectileSpeed;
		bool flag;
		if (this.m_pickupTeam != Team.Objects)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpoilsSequence.SpawnFX()).MethodHandle;
			}
			if (!(GameFlowData.Get() == null))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(GameFlowData.Get().activeOwnedActorData == null))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(this.m_inaccessibleFxPrefab == null))
					{
						flag = (this.m_pickupTeam != GameFlowData.Get().activeOwnedActorData.\u000E());
						goto IL_1F2;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		flag = false;
		IL_1F2:
		GameObject prefab;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			prefab = this.m_inaccessibleFxPrefab;
		}
		else
		{
			prefab = this.m_fxPrefab;
		}
		this.m_fx = base.InstantiateFX(prefab, array[1], Quaternion.identity, true, true);
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioManager.PostEvent(this.m_audioEvent, (!base.Caster) ? null : base.Caster.gameObject);
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpoilsSequence.Update()).MethodHandle;
			}
			if (this.m_fx == null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.SpawnFX();
			}
			else if (!this.m_didSetFinalPos)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_ignoreSpawnSpline)
				{
					this.m_curSplineSpeed += this.m_splineAcceleration;
					this.m_curSplineSpeed = Mathf.Min(this.m_splineSpeed, this.m_curSplineSpeed);
					this.m_splineTraveled += this.m_curSplineSpeed * GameTime.deltaTime;
					if (this.m_splineTraveled < 1f)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector3 position = this.m_spline.Interp(this.m_splineTraveled);
						this.m_fx.transform.position = position;
					}
					else
					{
						this.m_fx.transform.position = base.TargetPos;
						this.m_didSetFinalPos = true;
					}
				}
				else
				{
					this.m_fx.transform.position = base.TargetPos;
					this.m_didSetFinalPos = true;
				}
			}
			base.ProcessSequenceVisibility();
		}
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			PowerUp.ExtraParams extraParams2 = extraSequenceParams as PowerUp.ExtraParams;
			if (extraParams2 != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SpoilsSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_pickupTeam = (Team)extraParams2.m_pickupTeamAsInt;
				this.m_ignoreSpawnSpline = extraParams2.m_ignoreSpawnSpline;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
	}
}
