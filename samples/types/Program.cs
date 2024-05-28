using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

var config = new PowerFxConfig();

var engine = new RecalcEngine(config);

var text = @"
Point = Type({ x: Number, y: Number });
Line = Type({ start: Point, end: Point }); 
SlopeIntercept = Type({ slope: Number, intercept: Number });
SlopeInterceptForm(l: Line): SlopeIntercept = With( { m: ((l.end.y - l.start.y)/(l.end.x - l.start.x)) }, { slope: m, intercept: (l.start.y - (m*l.start.x)) });
LineEqual(a: Line, b: Line): Boolean = With( { line1: SlopeInterceptForm(a), line2: SlopeInterceptForm(b) }, And(line1.slope=line2.slope,line1.intercept=line2.intercept) );
Intersects(a: Line, b: Line): Boolean = If(LineEqual(a,b), true, With( { line1: SlopeInterceptForm(a), line2: SlopeInterceptForm(b)}, line1.slope <> line2.slope) );";

engine.AddUserDefinitions(text);

var result = (BooleanValue)engine.Eval("With( { a: {start:{x:0.0, y:0.0}, end: {x:1.0, y:1.0}}, b: {start:{x:0.0, y:1.0}, end: {x:-1.0, y:-1.0}} }, Intersects(a,b))");
Console.WriteLine(result.Value);

result = (BooleanValue)engine.Eval("With( { a: {start:{x:0.0, y:0.0}, end: {x:1.0, y:1.0}} }, Intersects(a,a))");
Console.WriteLine(result.Value);

result = (BooleanValue)engine.Eval("With( { a: {start:{x:0.0, y:0.0}, end: {x:1.0, y:1.0}}, b: {start:{x:0.0, y:1.0}, end: {x:1.0, y:2.0}} }, Intersects(a,b))");
Console.WriteLine(result.Value);
