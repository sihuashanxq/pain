using Pain.Compilers.Parsers.Rewriters;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var code = @"
    import {A,B as C} from 'abc.dfd'
    class Abc{
        func say(x){
            let y=func(){
                return x+1
            }

            return y;
        }
    }
";

var lexer = new Pain.Compilers.Parsers.Lexer("ddd", code);
var parser = new Pain.Compilers.Parsers.Parser(lexer);

var x = parser.ParseModule();
var func = x.Classes[0].Functions[0];
var captures = new ScopedSyntaxWalker(func).Walk();
var r=new Pain.Compilers.Parsers.Rewriters.ClosureExpressionRewriter(func,captures).Rewrite();
Console.WriteLine(x);