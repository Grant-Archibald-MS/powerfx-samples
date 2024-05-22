using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

string input = "This is a {test} string with {multiple} tokens.\nWith :\n{Users}\n{=Now()}";

var config = new PowerFxConfig();
config.SymbolTable.EnableMutationFunctions();

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

var parser = new PowerFxParser(engine);
string result = parser.Parse(input);

Console.WriteLine("Result after parsing:");
Console.WriteLine(result);