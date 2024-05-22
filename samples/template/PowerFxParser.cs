using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

/// <summary>
/// Parse a string template evaluting tokens with variables and PowerFX expressions
/// </summary>
public class PowerFxParser
{
    private RecalcEngine _engine;

    public PowerFxParser(RecalcEngine engine)
    {
        _engine = engine;
    }

    public string Parse(string input)
    {
        // Regular expression to match tokens inside curly braces
        Regex regex = new Regex(@"\{([^{}]*)\}");

        // Replace each token in the input string with its corresponding value from the dictionary
        string result = regex.Replace(input, match =>
        {
            string token = match.Groups[1].Value;

             if ( token.StartsWith("=")) {
                var evalResult = _engine.Eval(token.Substring(1));
                Console.WriteLine(evalResult.ToString());
                if ( evalResult is StringValue ) {
                    return (evalResult as StringValue).Value;
                }
                if ( evalResult is DateTimeValue ) {
                    // TODO handle format?
                    return (evalResult as DateTimeValue).Value.ToString();
                }
                // TODO hande other types
                return "Unknown";
            }

            var value = _engine.GetValue(token);
            if ( value is StringValue ) {
                var s = value as StringValue;
                return s.Value.ToString();
            }

            if ( value is TableValue ) {
                var t = value as TableValue;
                StringBuilder text = new StringBuilder();
                foreach ( var row in t.Rows) {
                    if ( text.Length > 0 ) {
                        text.Append( "\n" );
                    }
                    text.Append("- ");
                    if ( row.IsBlank ) {
                        continue;
                    }
                    RecordValue rec = row.Value;
                    foreach ( var field in row.Value.Fields ) {
                        text.Append($"{field.Name} ");
                        var val = field.Value;
                        if ( val is StringValue ) {
                            var s = val as StringValue;
                            text.Append(s.Value);
                            continue;
                        }
                        if ( val is NumberValue ) {
                            var n = val as NumberValue;
                            text.Append(n.Value);
                            continue;
                        }
                    }
                }
                return text.ToString();
            }

            //TODO handle other types e.g. NumericValue, RecordValue
            return "UNKNOWN";
        });

        return result;
    }
}