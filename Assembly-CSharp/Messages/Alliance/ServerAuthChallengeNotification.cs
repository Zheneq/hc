using System;
using UnityEngine.Networking;

#if SERVER
// custom
// Sent by the lobby to a newly connected game server. The server signs the (base64-decoded) nonce
// with its RSA private key and returns the signature + public key in RegisterGameServerRequest.
[Serializable]
public class ServerAuthChallengeNotification : AllianceMessageBase
{
    public string Nonce;

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        writer.Write(Nonce ?? "");
    }

    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        Nonce = reader.ReadString();
    }
}
#endif
