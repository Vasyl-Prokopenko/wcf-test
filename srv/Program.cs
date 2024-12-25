using System.Net;
using System.Xml;
using CoreWCF.Security;

namespace srv;

public static class Program
{
	private static Binding Binding { get; } = new NetTcpBinding
	{
		MaxReceivedMessageSize = ServiceConfig.MaxNumberOfBytes,
		MaxBufferSize = ServiceConfig.MaxNumberOfBytes,
		TransferMode = TransferMode.Buffered,
		MaxBufferPoolSize = 20_000_000,
		Security = new NetTcpSecurity
		{
			Mode = SecurityMode.None,
			Transport = new TcpTransportSecurity
			{
				ClientCredentialType = TcpClientCredentialType.None
			},
			Message = new MessageSecurityOverTcp
			{
				ClientCredentialType = MessageCredentialType.None
			}
		},
		ReaderQuotas = XmlDictionaryReaderQuotas.Max,
		ReceiveTimeout = TimeSpan.FromMinutes(10),
		SendTimeout = TimeSpan.FromMinutes(10)
	};

	public static async Task Main()
	{
		var builder = WebApplication.CreateBuilder();
		builder.WebHost.UseKestrel().UseNetTcp(IPAddress.Any, ServiceConfig.Uri.Port);

		builder.Services.AddServiceModelServices();
		builder.Services.AddServiceModelMetadata();
		builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

		var app = builder.Build();

		app.UseServiceModel(serviceBuilder =>
		{
			serviceBuilder.AddService<Service>(options => options.DebugBehavior.IncludeExceptionDetailInFaults = true);
			serviceBuilder.AddServiceEndpoint<Service, IService>(Binding, ServiceConfig.Uri.ToString());
			var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
			serviceMetadataBehavior.HttpsGetEnabled = true;
		});

		await app.RunAsync();
	}
}