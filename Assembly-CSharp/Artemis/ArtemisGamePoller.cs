using ArtemisServer.BridgeServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ArtemisServer.BridgeServer
{
	class ArtemisGamePoller : MonoBehaviour
	{
		private static readonly string PATH = @"E:\Atlas Reactor\game.json";
		
		private ArtemisBridgeServerInterface ar;

		public void Poll(ArtemisBridgeServerInterface absi)
		{
			ar = absi;
			Log.Info("Looking for a game file...");
			StartCoroutine(GameLoop());
		}

		private IEnumerator GameLoop()
		{
			while (!File.Exists(PATH))
			{
				Log.Info("File not found!");
				yield return new WaitForSeconds(1);
			}
			Log.Info("File found, starting game!");

			string json = File.ReadAllText(PATH);
			Log.Info($"File content: {json}");

			ar.StartGame(json);

			Log.Info("Deleting the file!");
			File.Delete(PATH);
			yield break;
		}
	}
}
