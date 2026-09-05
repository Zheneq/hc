// ROGUES
// SERVER

using UnityEngine.Networking;

#if SERVER
// added in rogues
public class RegisterGameServerRequest : AllianceMessageBase
{
    public LobbySessionInfo SessionInfo;
    public bool isPrivate;
    // custom. RSA public key (RSA.ToXmlString(false)) identifying this game server.
    public string PublicKey;
    // custom. Base64 signature over the lobby-issued challenge nonce (RSA SHA-256, PKCS#1).
    public string Signature;

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        SerializeObject(SessionInfo, writer);
        writer.Write(isPrivate);
        writer.Write(PublicKey ?? ""); // custom
        writer.Write(Signature ?? ""); // custom
    }

    // custom
    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        DeserializeObject(out SessionInfo, reader);
        isPrivate = reader.ReadBoolean();
        PublicKey = reader.ReadString();
        Signature = reader.ReadString();
    }
}
#endif