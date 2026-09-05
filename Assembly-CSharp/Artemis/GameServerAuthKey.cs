#if SERVER
using System;
using System.IO;
using System.Security.Cryptography;

// custom
namespace ArtemisServer.BridgeServer
{
    /// <summary>
    /// Persistent RSA identity for this game server. Generated on first run and stored on disk; the
    /// public key is presented to the lobby (which an admin approves once), and the private key signs
    /// the lobby's challenge nonce on every connect. Runs under Unity's Mono runtime, where
    /// RSACryptoServiceProvider SHA-256 signing is supported.
    /// </summary>
    public static class GameServerAuthKey
    {
        private static RSACryptoServiceProvider s_rsa;

        private static string KeyPath()
        {
            string configured = HydrogenConfig.Get().BridgeAuthKeyPath;
            if (!string.IsNullOrEmpty(configured))
            {
                return configured;
            }
            return Path.Combine(HydrogenConfig.Get().ConfigPath, "bridge_key.xml");
        }

        private static RSACryptoServiceProvider Rsa()
        {
            if (s_rsa != null)
            {
                return s_rsa;
            }

            string path = KeyPath();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            if (File.Exists(path))
            {
                rsa.FromXmlString(File.ReadAllText(path));
                Log.Info("Loaded game server auth key from " + path);
            }
            else
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllText(path, rsa.ToXmlString(true));
                Log.Info("Generated new game server auth key at " + path);
            }
            s_rsa = rsa;
            return s_rsa;
        }

        /// <summary>RSA public key as ToXmlString(false); this is the server's identity.</summary>
        public static string PublicKey => Rsa().ToXmlString(false);

        /// <summary>Signs <paramref name="data"/> (RSA SHA-256, PKCS#1) and returns a base64 signature.</summary>
        public static string SignBase64(byte[] data)
        {
            byte[] signature = Rsa().SignData(data, new SHA256Managed());
            return Convert.ToBase64String(signature);
        }
    }
}
#endif
