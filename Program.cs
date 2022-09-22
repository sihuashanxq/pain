// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var code=@"
    import {A,B as C} from 'abc.dfd'
    class Abc{
        func say(){
            return 123+2334;
        }
    }
";

var lexer=new Pain.Compilers.Parsers.Lexer("ddd",code);
var parser=new Pain.Compilers.Parsers.Parser(lexer);

var x=parser.ParseModule();
Console.WriteLine(x);