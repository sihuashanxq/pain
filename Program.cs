using Pain.Compilers.Parsers.Rewriters;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var code = @"
    class Abc{
        func say(x){
            let y=func(){
                let z=2
                return func (){
                    return x+z
                }
            }

            return y;
        }
    }
";

var lexer = new Pain.Compilers.Parsers.Lexer("ddd", code);
var parser = new Pain.Compilers.Parsers.Parser(lexer);

var x = parser.ParseModule();
var func = x.Classes[0].Functions[0];
new ScopedSyntaxWalker(func).Walk();
var r = new Pain.Compilers.Parsers.Rewriters.ClosureExpressionRewriter(func, x).Rewrite();

Console.WriteLine(x.ToString());
