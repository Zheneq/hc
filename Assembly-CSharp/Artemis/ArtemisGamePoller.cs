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
		private ArtemisBridgeServerInterface ar;

		public void Poll(ArtemisBridgeServerInterface absi)
		{
			ar = absi;
			Log.Info("Looking for a game file...");
			StartCoroutine(GameLoop());
		}

		private IEnumerator GameLoop()
		{
			var path = @"E:\Atlas Reactor\game.json";
			while (!File.Exists(path))
			{
				Log.Info("File not found!");
				yield return new WaitForSeconds(1);
			}
			Log.Info("File found, starting game!");

			string json = File.ReadAllText(path);
			Log.Info($"File content: {json}");

			ar.StartGame(json);

			Log.Info("Deleting the file!");
			File.Delete(path);
			yield break;
		}
	}
}
