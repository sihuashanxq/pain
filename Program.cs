// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var code=@" var x=1;
if x>3&& x>4";

var lexer=new Pain.Compilers.Parsers.Lexer("ddd",code);

while (true)
{
    var token = lexer.ScanNext();
    if (token.Type == Pain.Compilers.Parsers.TokenType.EOF) {
        break;
    }

    Console.WriteLine(token.Value);
}