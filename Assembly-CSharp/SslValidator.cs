using System;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public static class SslValidator
{
	private static SslPolicyErrors m_acceptedSslPolicyErrors = SslPolicyErrors.None;

	[CompilerGenerated]
	private static RemoteCertificateValidationCallback f__mg_cache0;

	public static SslPolicyErrors AcceptableSslPolicyErrors
	{
		get
		{
			return SslValidator.m_acceptedSslPolicyErrors;
		}
		set
		{
			SslValidator.m_acceptedSslPolicyErrors = value;
			
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(SslValidator.SslCallback);
		}
	}

	private static bool SslCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		if ((sslPolicyErrors & SslValidator.m_acceptedSslPolicyErrors) == sslPolicyErrors)
		{
			return true;
		}
		Log.Error("Certificate {0} has errors: {1}", new object[]
		{
			certificate.Subject,
			sslPolicyErrors.ToString()
		});
		return false;
	}
}
