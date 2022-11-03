using Pain.Compilers.Parsers.Rewriters;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var code = @"
    class Abc{
        func say(x){
            let y=func(){
                let z=2
                z=this.z+z
                return func (){
                    return func(){
                      return  x+z+this.z+super.x()+super.d()
                    }
                }
            }

            return y;
        }
    }
";

var lexer = new Pain.Compilers.Parsers.Lexer("ddd", code);
var parser = new Pain.Compilers.Parsers.Parser(lexer);
var strings = new Pain.Strings();
var classLoader = new Pain.Runtime.ClassLoader(token =>
{
    return null;
});

var vm = new Pain.Runtime.VM.VirtualMachine(classLoader, strings);
var x = parser.ParseModule();
var func = x.Classes[0].Functions[0];
new ScopedSyntaxWalker(func).Walk();
var r = new Pain.Compilers.Parsers.Rewriters.ClosureExpressionRewriter(func, x, x.Classes[0]).Rewrite();

Console.WriteLine(x.ToString());
