using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

internal class ReplayPlayManager : MonoBehaviour, IGameEventListener
{
	private static ReplayPlayManager s_instance;

	private bool m_appStateLoadingPassed;

	private Replay m_playingReplay;

	private Replay m_prospectiveReplay;

	private bool m_fastForward;

	private ReplayTimestamp m_seekTarget = default(ReplayTimestamp);

	internal static ReplayPlayManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		ResetState();
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.AppStateChanged);
	}

	private void ResetState()
	{
		m_seekTarget = default(ReplayTimestamp);
	}

	private void Update()
	{
		if (m_playingReplay == null)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get() != null)
			{
				if (ReplayTimestamp.Current() < m_seekTarget)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							Log.Info("Fastforwarding replay to {0} . . .", m_seekTarget);
							FastForward(m_seekTarget);
							return;
						}
					}
				}
			}
			m_playingReplay.PlaybackUpdate();
			return;
		}
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.AppStateChanged);
		s_instance = null;
	}

	public void StartReplay(string filename)
	{
		if (AppState.IsInGame())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					TextConsole.Get().Write("Failed to start replay: already ingame");
					return;
				}
			}
		}
		ResetState();
		try
		{
			m_prospectiveReplay = JsonUtility.FromJson<Replay>(File.ReadAllText(filename));
		}
		catch (Exception ex)
		{
			Debug.LogFormat("Failed; {0}", ex);
			try
			{
				m_prospectiveReplay = JsonUtility.FromJson<Replay>(File.ReadAllText(new StringBuilder().Append(filename).Append(".arr").ToString()));
			}
			catch (Exception ex2)
			{
				Debug.LogFormat("Failed; {0}", ex2);
				TextConsole.Get().Write("Failed to start replay: file could not be read");
			}
		}
		if (m_prospectiveReplay == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("InvalidReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"));
					return;
				}
			}
		}
		if (m_prospectiveReplay.m_versionMini != BuildVersion.MiniVersionString)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("ObsoleteReplay", "FrontEnd"), StringUtil.TR("Ok", "Global"));
					return;
				}
			}
		}
		if (m_prospectiveReplay.m_versionFull != BuildVersion.FullVersionString)
		{
			UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("ReplayIssue", "FrontEnd"), StringUtil.TR("OldReplay", "FrontEnd"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
			{
				m_playingReplay = m_prospectiveReplay;
				m_prospectiveReplay = null;
				m_appStateLoadingPassed = false;
				m_playingReplay.StartPlayback();
			});
			return;
		}
		m_playingReplay = m_prospectiveReplay;
		m_prospectiveReplay = null;
		m_appStateLoadingPassed = false;
		m_playingReplay.StartPlayback();
	}

	public bool IsPlayback()
	{
		return m_playingReplay != null;
	}

	public PersistedCharacterMatchData GetPlaybackMatchData()
	{
		if (m_playingReplay != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_playingReplay.GetMatchData();
				}
			}
		}
		return null;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.AppStateChanged)
		{
			return;
		}
		while (true)
		{
			if (m_appStateLoadingPassed)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (m_playingReplay != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									if (!AppState.IsInGame())
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
												if (AppState.GetCurrent() != AppState_GameLoading.Get())
												{
													while (true)
													{
														switch (6)
														{
														case 0:
															break;
														default:
															m_playingReplay.FinishPlayback();
															m_playingReplay = null;
															return;
														}
													}
												}
												return;
											}
										}
									}
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (AppState.GetCurrent() == AppState_GameLoading.Get())
			{
				while (true)
				{
					m_appStateLoadingPassed = true;
					return;
				}
			}
			return;
		}
	}

	private void FastForward(ReplayTimestamp target)
	{
		if (m_playingReplay == null)
		{
			return;
		}
		while (true)
		{
			m_fastForward = true;
			try
			{
				m_playingReplay.PlaybackFastForward(target);
			}
			finally
			{
				m_fastForward = false;
			}
			Log.Info("{0}: Fastforwarded", Time.unscaledTime);
			if (!(GameFlowData.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (ReplayTimestamp.Current() >= target)
				{
					while (true)
					{
						GameEventManager.Get().FireEvent(GameEventManager.EventType.ReplaySeekFinished, null);
						return;
					}
				}
				return;
			}
		}
	}

	public void Restart()
	{
		if (m_playingReplay == null)
		{
			return;
		}
		while (true)
		{
			if ((bool)GameFlowData.Get())
			{
				using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						current.OnReplayRestart();
					}
				}
			}
			NetworkIdentity[] array = UnityEngine.Object.FindObjectsOfType<NetworkIdentity>();
			foreach (NetworkIdentity networkIdentity in array)
			{
				if (networkIdentity.sceneId.IsEmpty())
				{
					if (!(networkIdentity.gameObject.GetComponent<CameraManager>() != null))
					{
						UnityEngine.Object.Destroy(networkIdentity.gameObject);
					}
				}
			}
			if ((bool)SequenceManager.Get())
			{
				SequenceManager.Get().ClearAllSequences();
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ReplayRestart, null);
			m_playingReplay.PlaybackRestart();
			return;
		}
	}

	public void Seek(ReplayTimestamp target)
	{
		Log.Info("{0}: Setting replay seek to {1} . . .", Time.unscaledTime, target);
		m_seekTarget = target;
		if (target <= ReplayTimestamp.Current())
		{
			Restart();
		}
		Update();
	}

	public bool IsFastForward()
	{
		return m_fastForward;
	}

	public void Pause()
	{
		GameTime.scale = 0f;
	}

	public void Resume()
	{
		GameTime.scale = 1f;
	}
}
