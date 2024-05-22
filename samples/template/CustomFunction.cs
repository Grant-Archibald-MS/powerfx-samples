using Microsoft.PowerFx.Types;
using Microsoft.PowerFx;

public class CustomFunction : ReflectionFunction {
    private CustomState _state;

    // NOTE: Order of calling base is name, return type then argument types
    public CustomFunction(CustomState state) : base("Custom", FormulaType.String, FormulaType.String) {
        _state = state;
    }

    public StringValue Execute(StringValue greeting) {
        return FormulaValue.New($"{greeting.Value} {_state.Name}");
    }
}