using Pain.Compilers.Parsers.Rewriters;
var strings = new Pain.Strings();
var classLoader = null as Pain.Runtime.ClassLoader;
classLoader = new Pain.Runtime.ClassLoader(token =>
{
   var sourceCode = Pain.FileReader.ReadModule(token);
   var lexer = new Pain.Compilers.Parsers.Lexer(token.Substring(0, token.LastIndexOf(".")), sourceCode);
   var parser = new Pain.Compilers.Parsers.Parser(lexer);
   var module = parser.ParseModule();
   Console.WriteLine(module.ToString());
   var moduleCompiler = new Pain.Compilers.CodeGen.ModuleCompiler(module, classLoader, strings);
   return moduleCompiler.Compile();
});

var vm = new Pain.Runtime.VM.VirtualMachine(classLoader, strings);
Console.WriteLine(vm.Execute("program", "program", "main", Array.Empty<Pain.Runtime.IObject>()).ToString());

var st=new System.Diagnostics.Stopwatch();
st.Start();
var x=vm.Execute("program", "program", "main", Array.Empty<Pain.Runtime.IObject>()).ToString();
st.Stop();
Console.WriteLine(st.ElapsedMilliseconds);