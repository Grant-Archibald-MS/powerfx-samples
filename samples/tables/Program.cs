using System;
using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

public class Test {
    public static void Main(string[] args) {
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

        engine.UpdateVariable("Users", table);
        // Example of how to parse and add definitions 
        // https://github.com/microsoft/Power-Fx/blob/fc229ab412f7392f9e0993ee516b304774732dad/src/libraries/Microsoft.PowerFx.Repl/Repl.cs#L345
        engine.UpdateVariable("Matches", table2);

        var options =  new ParserOptions() { AllowsSideEffects = true };

        var result = engine.Eval("CountRows(Users)", options: options);

        if ( result is DecimalValue ) {
            var d = result as DecimalValue;
            Console.WriteLine(d.Value);
        }

        engine.Eval("Set(Matches,Filter(Users,StartsWith(ThisRecord.Name,\"A\")));", options: options);
        
        result = engine.GetValue("Matches");
        Console.WriteLine(result.GetType());
        if ( result is TableValue ) {
            var t = result as TableValue;
            foreach ( var row in t.Rows) {
                if ( row.IsBlank ) {
                    continue;
                }
                RecordValue rec = row.Value;
                foreach ( var field in row.Value.Fields ) {
                    Console.Write($"{field.Name} ");
                    var val = field.Value;
                    if ( val is StringValue ) {
                        var s = val as StringValue;
                        Console.WriteLine(s.Value);
                        continue;
                    }
                    if ( val is NumberValue ) {
                        var n = val as NumberValue;
                        Console.WriteLine(n.Value);
                        continue;
                    }
                    Console.WriteLine("Unknown");
                }
            }
        }

    }
}