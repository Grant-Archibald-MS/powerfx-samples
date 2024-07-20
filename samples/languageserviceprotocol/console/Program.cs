using Microsoft.PowerFx;
using Microsoft.PowerFx.Core;
using Microsoft.PowerFx.Types;
using Microsoft.PowerFx.LanguageServerProtocol;
using Microsoft.PowerFx.LanguageServerProtocol.Protocol;
using Moq;
using System.Text.Json;

var config = new PowerFxConfig();
var engine = new RecalcEngine(config);

engine.UpdateVariable("AValue", 1);


var suggest  =engine.Suggest(engine.Check("A", RecordType.Empty()), 1);

Console.WriteLine(suggest.Suggestions.Count());


var scopeFactory = new Mock<IPowerFxScopeFactory>(MockBehavior.Strict);
scopeFactory.Setup(x => x.GetOrCreateInstance(It.IsAny<string>())).Returns(engine.CreateEditorScope());
var client = new Mock<IClient>();

client.Setup(x => x.SendToClient(It.IsAny<string>())).Callback<string>(data =>
{
    Console.WriteLine(data);
});

var server = new TestLanguageServer(client.Object.SendToClient, scopeFactory.Object);


var text = File.ReadAllText("samples/languageserviceprotocol/server/test.pfx");

var testParams = new CompletionParams()
{
    TextDocument = GetTextDocument(GetUri("expression=" + text)),
    Text = text,
    Position = GetPosition(text.Length),
    Context = GetCompletionContext()
};

server.OnDataReceived(GetRequestPayload(testParams, TextDocumentNames.Completion, Guid.NewGuid().ToString()));

scopeFactory.VerifyAll();

Position GetPosition(int offset, int line = 0)
{
    return new Position()
    {
        Line = line,
        Character = offset
    };
}

string GetUri(string queryParams = null)
{
    var uriBuilder = new UriBuilder("powerfx://app")
    {
        Query = queryParams ?? string.Empty
    };
    return uriBuilder.Uri.AbsoluteUri;
}

CompletionContext GetCompletionContext()
{
    return new CompletionContext();
}

TextDocumentIdentifier GetTextDocument(string uri = null)
{
    return new TextDocumentIdentifier() { Uri = uri ?? GetUri() };
}

string GetRequestPayload(object paramsObj, string method, string id = null)
{

    var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    var payload = JsonSerializer.Serialize(
        new
        {
            jsonrpc = "2.0",
            id,
            method,
            @params = paramsObj
        }, options);
    return payload;
}

public class TestLanguageServer : LanguageServer
{
    public TestLanguageServer(SendToClient sendToClient, IPowerFxScopeFactory scopeFactory, Action<string> logger = null) : base(sendToClient, scopeFactory, (string s) => Console.WriteLine(s))
    {
    }
}

public interface IClient {
    void SendToClient(string data);
}