using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

var config = new PowerFxConfig();

var engine = new RecalcEngine(config);

var text = @"
Point = Type({ x: Number, y: Number });
Line = Type({ start: Point, end: Point }); 
LineEqual(a: Line, b: Line): Boolean = And(And(a.start.x=b.start.x,a.start.y=b.start.y),And(a.end.x=b.end.x,a.end.y=b.end.y));
Intersects(a: Line, b: Line): Boolean = If(LineEqual(a,b), true, With( { slopeA: (a.end.y - a.start.y)/(a.end.x - a.start.x), slopeB: (b.end.y - b.start.y)/(b.end.x - b.start.x) },  slopeA <> slopeB) );";

engine.AddUserDefinitions(text);

var result = (BooleanValue)engine.Eval("With( { a: {start:{x:0.0, y:0.0}, end: {x:1.0, y:1.0}}, b: {start:{x:0.0, y:1.0}, end: {x:-1.0, y:-1.0}} }, Intersects(a,b))");
Console.WriteLine(result.Value);

result = (BooleanValue)engine.Eval("With( { a: {start:{x:0.0, y:0.0}, end: {x:1.0, y:1.0}} }, Intersects(a,a))");
Console.WriteLine(result.Value);

result = (BooleanValue)engine.Eval("With( { a: {start:{x:0.0, y:0.0}, end: {x:1.0, y:1.0}}, b: {start:{x:0.0, y:1.0}, end: {x:1.0, y:2.0}} }, Intersects(a,b))");
Console.WriteLine(result.Value);

