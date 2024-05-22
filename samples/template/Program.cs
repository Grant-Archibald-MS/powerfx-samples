using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;
using Microsoft.PowerFx.Dataverse;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

var builder = new ConfigurationBuilder()
.AddJsonFile("config.json", optional: true, reloadOnChange: true)
.AddJsonFile($"config.dev.json", optional: true);

var configuration = builder.Build();

string? connectionString = configuration["connectionString"];


while(connectionString == null)
{
        // https://learn.microsoft.com/en-us/power-apps/developer/data-platform/xrm-tooling/use-connection-strings-xrm-tooling-connect
        Console.Write("Enter Dataverse Connection string: ");
        connectionString = Console.ReadLine();
}

var svcClient = new ServiceClient(connectionString) { UseWebApi = false };
var dataverse = SingleOrgPolicy.New(svcClient);
var symbolValues = dataverse.SymbolValues;


string input = "This is a {test} string with {multiple} tokens.\nWith :\n{Users}\n{=Now()}\n{=Custom(\"Hello\")}\n'{=First(Accounts).'Account Name'}'";

var config = new PowerFxConfig();
config.SymbolTable.EnableMutationFunctions();
config.AddFunction(new CustomFunction(new CustomState { Name = "World" }));

config.EnableSetFunction();
var engine = new RecalcEngine(config);

var recordType = RecordType.Empty()
        .Add(new NamedFormulaType("Name", FormulaType.String, displayName: "Name"))
        .Add(new NamedFormulaType("Value", FormulaType.Number, displayName: "Value"));

var rv1 = RecordValue.NewRecordFromFields(
        new NamedValue("Name", FormulaValue.New("Test user")),
        new NamedValue("Value", FormulaValue.New(1.0)));

var rv2 = RecordValue.NewRecordFromFields(
        new NamedValue("Name", FormulaValue.New("Another user")),
        new NamedValue("Value", FormulaValue.New(1.0)));

var table = TableValue.NewTable(recordType, rv1, rv2);
var table2 = TableValue.NewTable(recordType);

engine.UpdateVariable("test", "replacement1");
engine.UpdateVariable("multiple", "replacement2");
engine.UpdateVariable("Users", table);

var parser = new PowerFxParser(engine, symbolValues);
string result = parser.Parse(input);

Console.WriteLine("Result after parsing:");
Console.WriteLine(result);