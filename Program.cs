// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var code=@"for let i=0;i<10;i=i+1{
    if i>10{
        println(i)
    }else{
        println(10)
    }
}";

var lexer=new Pain.Compilers.Parsers.Lexer("ddd",code);
var parser=new Pain.Compilers.Parsers.Parser(lexer);

parser.Next();
var x=parser.ParseExpression();
Console.WriteLine(x);