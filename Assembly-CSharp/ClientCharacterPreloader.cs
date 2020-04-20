using System;
using System.Collections;
using System.Collections.Generic;
using LobbyGameClientMessages;
using UnityEngine;

public class ClientCharacterPreloader : MonoBehaviour
{
	private Dictionary<CharacterResourceLink, List<CharacterVisualInfo>> m_linksToSkins = new Dictionary<CharacterResourceLink, List<CharacterVisualInfo>>();

	private Coroutine m_coroutine;

	private void Start()
	{
		if (!Application.isEditor)
		{
			ClientGameManager.Get().OnGameInfoNotification += this.HandleGameInfoNotification;
			GameManager.Get().OnGameStatusChanged += this.HandleGameStatusChanged;
		}
	}

	private void OnDestroy()
	{
		if (!Application.isEditor)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnGameInfoNotification -= this.HandleGameInfoNotification;
			}
			if (GameManager.Get() != null)
			{
				GameManager.Get().OnGameStatusChanged -= this.HandleGameStatusChanged;
			}
		}
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		if (!(gameManager == null))
		{
			if (gameManager.TeamInfo != null && gameManager.TeamInfo.TeamAPlayerInfo != null)
			{
				if (gameManager.TeamInfo.TeamBPlayerInfo != null && !(GameWideData.Get() == null) && gameManager.GameStatus >= GameStatus.FreelancerSelecting)
				{
					if (gameManager.GameStatus > GameStatus.LoadoutSelecting)
					{
					}
					else
					{
						Dictionary<CharacterResourceLink, List<CharacterVisualInfo>> dictionary = new Dictionary<CharacterResourceLink, List<CharacterVisualInfo>>(0xA);
						IEnumerator<LobbyPlayerInfo> enumerator = gameManager.TeamInfo.TeamAPlayerInfo.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
								if (lobbyPlayerInfo != null && lobbyPlayerInfo.CharacterInfo != null)
								{
									if (!lobbyPlayerInfo.CharacterInfo.CharacterType.IsValid())
									{
									}
									else
									{
										CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterInfo.CharacterType);
										if (characterResourceLink != null)
										{
											if (!dictionary.ContainsKey(characterResourceLink))
											{
												dictionary[characterResourceLink] = new List<CharacterVisualInfo>();
											}
											List<CharacterVisualInfo> list = dictionary[characterResourceLink];
											list.Add(lobbyPlayerInfo.CharacterInfo.CharacterSkin);
										}
									}
								}
							}
						}
						finally
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						IEnumerator<LobbyPlayerInfo> enumerator2 = gameManager.TeamInfo.TeamBPlayerInfo.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
								if (lobbyPlayerInfo2 != null)
								{
									if (lobbyPlayerInfo2.CharacterInfo != null)
									{
										if (!lobbyPlayerInfo2.CharacterInfo.CharacterType.IsValid())
										{
										}
										else
										{
											CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo2.CharacterInfo.CharacterType);
											if (characterResourceLink2 != null)
											{
												if (!dictionary.ContainsKey(characterResourceLink2))
												{
													dictionary[characterResourceLink2] = new List<CharacterVisualInfo>();
												}
												List<CharacterVisualInfo> list2 = dictionary[characterResourceLink2];
												list2.Add(lobbyPlayerInfo2.CharacterInfo.CharacterSkin);
											}
										}
									}
								}
							}
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						if (ClientCharacterPreloader.Different(dictionary, this.m_linksToSkins))
						{
							if (this.m_coroutine != null)
							{
								base.StopCoroutine(this.m_coroutine);
								this.m_coroutine = null;
							}
							this.m_linksToSkins = dictionary;
							this.m_coroutine = base.StartCoroutine(this.CharacterPreloadCoroutine());
							return;
						}
						return;
					}
				}
			}
		}
		this.StopPreloading();
	}

	private void HandleGameStatusChanged(GameStatus gameStatus)
	{
		if (gameStatus > GameStatus.LoadoutSelecting)
		{
			this.StopPreloading();
		}
	}

	private void StopPreloading()
	{
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
			this.m_coroutine = null;
			this.m_linksToSkins.Clear();
		}
	}

	private static bool Different(Dictionary<CharacterResourceLink, List<CharacterVisualInfo>> lhs, Dictionary<CharacterResourceLink, List<CharacterVisualInfo>> rhs)
	{
		if (lhs.Count != rhs.Count)
		{
			return true;
		}
		using (Dictionary<CharacterResourceLink, List<CharacterVisualInfo>>.Enumerator enumerator = lhs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<CharacterResourceLink, List<CharacterVisualInfo>> keyValuePair = enumerator.Current;
				if (!rhs.ContainsKey(keyValuePair.Key))
				{
					return true;
				}
				List<CharacterVisualInfo> value = keyValuePair.Value;
				List<CharacterVisualInfo> list = rhs[keyValuePair.Key];
				if (value.Count != list.Count)
				{
					return true;
				}
				for (int i = 0; i < value.Count; i++)
				{
					if (!list.Contains(value[i]))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private IEnumerator CharacterPreloadCoroutine()
	{
		yield return new WaitForSeconds(2f);
		foreach (KeyValuePair<CharacterResourceLink, List<CharacterVisualInfo>> keyValuePair in this.m_linksToSkins)
		{
			keyValuePair.Key.UnloadSkinsNotInList(keyValuePair.Value);
		}
		using (Dictionary<CharacterResourceLink, List<CharacterVisualInfo>>.Enumerator enumerator2 = this.m_linksToSkins.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<CharacterResourceLink, List<CharacterVisualInfo>> kvp = enumerator2.Current;
				for (int i = 0; i < kvp.Value.Count; i++)
				{
					CharacterVisualInfo skin = kvp.Value[i];
					kvp.Key.LoadAsync(skin, new CharacterResourceLink.CharacterResourceDelegate(this.HandleCharacterResourceLoaded), GameStatus.Loading);
					yield return new WaitForSeconds(1f);
				}
			}
		}
		yield break;
	}

	private void HandleCharacterResourceLoaded(LoadedCharacterSelection loadedCharacter)
	{
	}
}
