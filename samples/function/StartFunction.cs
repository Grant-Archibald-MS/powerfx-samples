using Microsoft.PowerFx;
using Microsoft.PowerFx.Core.Utils;
using Microsoft.PowerFx.Types;

public class StartFunction : ReflectionFunction {
    public StartFunction() : base(DPath.Root.Append(new DName("Common")), "Start", StringType.String) {

    }

    public StringValue Execute() {
        return FormulaValue.New("Started");
    }
}