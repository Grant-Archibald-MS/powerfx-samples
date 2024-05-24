using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

var config = new PowerFxConfig();
config.SymbolTable.EnableMutationFunctions();
config.AddFunction(new StartFunction());

var engine = new RecalcEngine(config);

Console.WriteLine(((StringValue)engine.Eval("Common.Start()")).Value);