using AOT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class PKFxSoundManager : MonoBehaviour
{
	private delegate void VoidFPtr(IntPtr ptr);

	public delegate void StartSoundDelegate(PKFxManager.SoundDescriptor soundDesc);

	private static StartSoundDelegate m_onStartSoundDelegate = null;

	private static List<AudioSource> m_spawnedSound = new List<AudioSource>();

	private void OnDestroy()
	{
		m_spawnedSound.Clear();
	}

	private void Update()
	{
		if (m_onStartSoundDelegate != null)
		{
			return;
		}
		while (true)
		{
			if (m_spawnedSound == null)
			{
				return;
			}
			for (int num = m_spawnedSound.Count - 1; num >= 0; num--)
			{
				AudioSource audioSource = m_spawnedSound[num];
				if (!audioSource.isPlaying)
				{
					UnityEngine.Object.Destroy(audioSource.gameObject);
					m_spawnedSound.RemoveAt(num);
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	[MonoPInvokeCallback(typeof(VoidFPtr))]
	public static void OnStartSound(IntPtr actionFactorySound)
	{
		PKFxManager.S_SoundDescriptor desc = (PKFxManager.S_SoundDescriptor)Marshal.PtrToStructure(actionFactorySound, typeof(PKFxManager.S_SoundDescriptor));
		PKFxManager.SoundDescriptor soundDescriptor = new PKFxManager.SoundDescriptor(desc);
		if (m_onStartSoundDelegate != null)
		{
			m_onStartSoundDelegate(soundDescriptor);
			return;
		}
		string text = "PKFxSounds/" + Path.ChangeExtension(soundDescriptor.Path, null);
		AudioClip audioClip = Resources.Load(text) as AudioClip;
		if (audioClip != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					GameObject gameObject = new GameObject("FxSound");
					if (gameObject != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
							{
								gameObject.transform.position = soundDescriptor.WorldPosition;
								AudioSource audioSource = gameObject.AddComponent<AudioSource>();
								if (audioSource != null)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											audioSource.clip = audioClip;
											audioSource.Play();
											audioSource.volume = soundDescriptor.Volume;
											audioSource.time = soundDescriptor.StartTimeOffsetInSeconds;
											audioSource.spatialBlend = 1f;
											if (soundDescriptor.PlayTimeInSeconds != 0f)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														UnityEngine.Object.Destroy(audioSource, soundDescriptor.PlayTimeInSeconds);
														return;
													}
												}
											}
											m_spawnedSound.Add(audioSource);
											return;
										}
									}
								}
								return;
							}
							}
						}
					}
					return;
				}
				}
			}
		}
		Debug.LogError("[PKFX] Could not load sound layer " + text);
	}

	public static void RegisterCustomHandler(StartSoundDelegate customDelegate)
	{
		m_onStartSoundDelegate = customDelegate;
	}
}
