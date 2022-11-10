using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers.Definitions;

namespace Pain.Compilers.Parsers;

public class Parser
{
    private readonly Lexer _lexer;

    private Token _token;

    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        _token = null!;
    }

    public static Module Parse(string fileName, string sourceCode)
    {
        return new Parser(new Lexer(fileName, sourceCode)).ParseModule();
    }

    private Syntax ParseUnitExpression()
    {
        var lExpr = ParseOrExpression();
        var token = _token;
        if (!token.Type.IsAssign())
        {
            return lExpr;
        }

        if (lExpr == null)
        {
            ThrowError();
        }

        if (lExpr!.Type != SyntaxType.Name && lExpr.Type != SyntaxType.Member)
        {
            ThrowError();
        }

        var rExpr = Parse(Next, ParseOrExpression, ThrowNullError);
        switch (token.Type)
        {
            case TokenType.Assign:
                return Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Assign);
            case TokenType.AddAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Add), SyntaxType.Assign);
            case TokenType.BitAndAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitAnd), SyntaxType.Assign);
            case TokenType.LeftShiftAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitLeftShift), SyntaxType.Assign);
            case TokenType.BitOrAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitOr), SyntaxType.Assign);
            case TokenType.RightShiftAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitRightShift), SyntaxType.Assign);
            case TokenType.BitXorAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitXor), SyntaxType.Assign);
            case TokenType.DivideAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Divide), SyntaxType.Assign);
            case TokenType.ModuloAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Modulo), SyntaxType.Assign);
            case TokenType.MultiplyAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Multiply), SyntaxType.Assign);
            case TokenType.SubtractAssign:
                return Syntax.MakeBinary(lExpr, Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Subtract), SyntaxType.Assign);
            default:
                ThrowError();
                throw new Exception();
        }
    }

    private Syntax ParseOrExpression()
    {
        var lExpr = Parse(null, ParseAndExpression, ThrowNullError(TokenType.Or));
        while (Match(TokenType.Or))
        {
            var rExpr = Parse(Next, ParseBitXorExpresion, ThrowNullError);
            lExpr = Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Or);
        }

        return lExpr;
    }

    private Syntax ParseAndExpression()
    {
        var lExpr = Parse(null, ParseBitOrExpresion, ThrowNullError(TokenType.And));
        while (Match(TokenType.And))
        {
            var rExpr = Parse(Next, ParseBitXorExpresion, ThrowNullError);
            lExpr = Syntax.MakeBinary(lExpr, rExpr, SyntaxType.And);
        }

        return lExpr;
    }

    private Syntax ParseBitOrExpresion()
    {
        var lExpr = Parse(null, ParseBitXorExpresion, ThrowNullError(TokenType.BitOr));
        while (Match(TokenType.BitOr))
        {
            var rExpr = Parse(Next, ParseBitXorExpresion, ThrowNullError);
            lExpr = Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitOr);
        }

        return lExpr;
    }

    private Syntax ParseBitXorExpresion()
    {
        var lExpr = Parse(null, ParseBitAndExpresion, ThrowNullError(TokenType.BitXor));
        while (Match(TokenType.BitXor))
        {
            var rExpr = Parse(Next, ParseBitAndExpresion, ThrowNullError);
            lExpr = Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitXor);
        }

        return lExpr;
    }

    private Syntax ParseBitAndExpresion()
    {
        var lExpr = Parse(null, ParseEqualityExpresion, ThrowNullError(TokenType.BitAnd));
        while (Match(TokenType.BitAnd))
        {
            var rExpr = Parse(Next, ParseEqualityExpresion, ThrowNullError);
            lExpr = Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitLeftShift);
        }

        return lExpr;
    }

    private Syntax ParseEqualityExpresion()
    {
        var tokens = new[] { TokenType.Equal, TokenType.NotEqual, TokenType.Is };
        var lExpr = Parse(null, ParseReleationExpresion, ThrowNullError(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseReleationExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Is => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Is),
                TokenType.Equal => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.EqualTo),
                _ => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.NotEqualTo),
            };
        }

        return lExpr;
    }

    private Syntax ParseReleationExpresion()
    {
        var tokens = new[] { TokenType.Less, TokenType.LessOrEqual, TokenType.Greater, TokenType.GreaterOrEqual };
        var lExpr = Parse(null, ParseShiftExpresion, ThrowNullError(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseShiftExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Less => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.LessThan),
                TokenType.LessOrEqual => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.LessThanOrEqual),
                TokenType.Greater => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.GreaterThan),
                _ => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.GreaterThanOrEqual),
            };
        }

        return lExpr;
    }

    private Syntax ParseShiftExpresion()
    {
        var tokens = new[] { TokenType.LeftShift, TokenType.RightShift };
        var lExpr = Parse(null, ParseAdditiveExpresion, ThrowNullError(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseAdditiveExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.LeftShift => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitLeftShift),
                _ => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.BitRightShift),
            };
        }

        return lExpr;
    }

    private Syntax ParseAdditiveExpresion()
    {
        var tokens = new[] { TokenType.Add, TokenType.Subtract };
        var lExpr = Parse(null, ParseMultiplicativeExpresion, ThrowNullError(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseMultiplicativeExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Add => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Add),
                _ => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Subtract),
            };
        }

        return lExpr;
    }

    private Syntax ParseMultiplicativeExpresion()
    {
        var tokens = new[] { TokenType.Multiply, TokenType.Divide, TokenType.Modulo };
        var lExpr = Parse(null, ParseUnaryExpression, ThrowNullError(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseUnaryExpression, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Multiply => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Multiply),
                TokenType.Divide => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Divide),
                _ => Syntax.MakeBinary(lExpr, rExpr, SyntaxType.Modulo),
            };
        }

        return lExpr;
    }

    private Syntax ParseUnaryExpression()
    {
        if (!Match(TokenType.Add, TokenType.BitNot, TokenType.Subtract, TokenType.Not))
        {
            return ParseMemberOrCallExpression();
        }

        var token = _token;
        var expr = Parse(Next, ParseUnaryExpression, ThrowNullError);
        return token.Type switch
        {
            TokenType.Add => Syntax.MakeUnary(expr, SyntaxType.Add),
            TokenType.Subtract => Syntax.MakeUnary(expr, SyntaxType.Subtract),
            TokenType.BitNot => Syntax.MakeUnary(expr, SyntaxType.BitNot),
            _ => Syntax.MakeUnary(expr, SyntaxType.Not),
        };
    }

    private Syntax ParseMemberOrCallExpression()
    {
        var expr = Parse(null, ParsePrimaryExpression, ThrowNullError);
        while (true)
        {
            if (!Match(TokenType.OpenParen, TokenType.Dot, TokenType.OpenSquare))
            {
                break;
            }

            switch (_token.Type)
            {
                case TokenType.OpenParen:
                    expr = ParseCallExpression(expr);
                    break;
                case TokenType.OpenSquare:
                case TokenType.Dot:
                    expr = ParseMemberExpression(expr);
                    break;
            }
        }

        return expr;
    }

    private Syntax ParsePrimaryExpression()
    {
        var token = _token;
        switch (token.Type)
        {
            case TokenType.Identifier:
                Next();
                return Syntax.MakeName(token.Value.ToString()!);
            case TokenType.True:
            case TokenType.False:
                Next();
                return Syntax.MakeConstant(token.Value, SyntaxType.ConstBoolean);
            case TokenType.Number:
                Next();
                return Syntax.MakeConstant(token.Value, SyntaxType.ConstNumber);
            case TokenType.LiteralString:
                Next();
                return Syntax.MakeConstant(token.Value, SyntaxType.ConstString);
            case TokenType.Null:
                Next();
                return Syntax.MakeConstant(token.Value, SyntaxType.ConstNull);
            case TokenType.Super:
                Next();
                return Syntax.MakeSuper();
            case TokenType.This:
                Next();
                return Syntax.MakeThis();
            case TokenType.New:
                return ParseNewExpression();
            case TokenType.Func:
                return ParseLocalFunctionExpression();
            case TokenType.Throw:
                return ParseThrowExpression();
            case TokenType.OpenParen:
                Next();
                return Parse(null, ParseExpression, i =>
                {
                    ThrowNullError(i);
                    ThrowError(_token.Type != TokenType.CloseParen);
                    Next();
                });
            default:
                return null!;
        }
    }

    private Syntax ParseThrowExpression()
    {
        ThrowError(!Match(TokenType.Throw));
        Next();
        var expr = Parse(null, ParseUnitExpression, ThrowNullError);
        return new ThrowExpression(new BinaryExpression(Syntax.MakeName("%catch%"), expr, SyntaxType.Assign));
    }

    private Syntax ParseMemberExpression(Syntax @object)
    {
        try
        {
            if (Match(TokenType.Dot))
            {
                Next();
                ThrowError(!Match(TokenType.Identifier));
                return Syntax.MakeMember(@object, Syntax.MakeConstant(_token.Value, SyntaxType.ConstString));
            }

            var member = Parse(Next, ParseUnitExpression, ThrowNullError);
            ThrowError(!Match(TokenType.CloseSquare));
            return Syntax.MakeMember(@object, member);
        }
        finally
        {
            Next();
        }
    }

    private Syntax ParseCallExpression(Syntax function)
    {
        return Syntax.MakeCall(function, ParseCallArguments().ToArray());
    }

    private List<Syntax> ParseCallArguments()
    {
        try
        {
            var arguments = new List<Syntax>();

            ThrowError(!Match(TokenType.OpenParen));
            Next();

            for (var i = 0; !Match(TokenType.CloseParen); i++)
            {
                if (i % 2 == 1)
                {
                    ThrowError(!Match(TokenType.Comma));
                    Next();
                    continue;
                }

                var argument = Parse(null, ParseUnitExpression, ThrowNullError);
                arguments.Add(argument);
            }

            ThrowError(!Match(TokenType.CloseParen));
            return arguments;
        }
        finally
        {
            Next();
        }
    }

    private Syntax ParseNewExpression()
    {
        ThrowError(!Match(TokenType.New));
        Next();
        var expr = Parse(null, ParseMemberOrCallExpression, ThrowNullError) as CallExpression;
        return Syntax.MakeNew(expr!.Function, expr.Arguments);
    }

    private FunctionExpression ParseFunctionExpression()
    {
        var name = string.Empty;
        var native = false;
        ThrowError(!Match(TokenType.Func));
        Next();
        if (Match(TokenType.Native))
        {
            native = true;
            Next();
        }
        ThrowError(!Match(TokenType.Identifier));
        name = _token.Value.ToString();
        Next();
        var parameters = ParseFunctionParameters();
        if (native)
        {
            return Syntax.MakeFunction(name!, native, false, parameters, Syntax.MakeBlock());
        }
        else
        {
            var functionBody = ParseFunctionBodyExpression();
            var parameterInits = Syntax.MakeVariable(parameters.Select(i => new Variable(i.Name, Syntax.MakeName(i.Name))).ToArray()) as Syntax;
            if (parameters.Length == 0)
            {
                parameterInits = Syntax.MakeEmpty();
            }
            return Syntax.MakeFunction(name!, native, false, parameters, Syntax.MakeBlock(parameterInits, functionBody));
        }
    }

    private Syntax ParseLocalFunctionExpression()
    {
        var name = Guid.NewGuid().ToString();
        ThrowError(!Match(TokenType.Func));
        Next();
        var parameters = ParseFunctionParameters();
        var functionBody = ParseFunctionBodyExpression();
        var parameterInits = Syntax.MakeVariable(parameters.Select(i => new Variable(i.Name, Syntax.MakeName(i.Name))).ToArray()) as Syntax;
        if (parameters.Length == 0)
        {
            parameterInits = Syntax.MakeEmpty();
        }

        return Syntax.MakeFunction(name!, false, true, parameters, Syntax.MakeBlock(parameterInits, functionBody));
    }

    private Syntax ParseFunctionBodyExpression()
    {
        if (Match(TokenType.Arrow))
        {
            Next();
            return Syntax.MakeReturn(ParseExpression());
        }

        ThrowError(!Match(TokenType.OpenBrace));
        return ParseExpression();
    }

    private ParameterExpression[] ParseFunctionParameters()
    {
        ThrowError(!Match(TokenType.OpenParen));
        Next();
        var list = new List<ParameterExpression>();

        for (var i = 0; !Match(TokenType.CloseParen); i++)
        {
            if (i % 2 == 1)
            {
                ThrowError(!Match(TokenType.Comma));
                Next();
                continue;
            }

            ThrowError(!Match(TokenType.Identifier));
            list.Add(new ParameterExpression(_token.Value.ToString()!, list.Count));
            Next();
        }

        ThrowError(!Match(TokenType.CloseParen));
        Next();
        return list.ToArray();
    }

    public Syntax ParseExpression()
    {
        while (true)
        {
            switch (_token.Type)
            {
                case TokenType.Switch:
                    return ParseSwitchExpression();
                case TokenType.Let:
                    return ParseLetExpression();
                case TokenType.OpenBrace:
                    return ParseBlockExpression();
                case TokenType.If:
                    return ParseIfExpression();
                case TokenType.For:
                    return ParseForExpression();
                case TokenType.Break:
                    return ParseBreakExpression();
                case TokenType.Continue:
                    return ParseContinueExpression();
                case TokenType.Return:
                    return ParseReturnExpression();
                case TokenType.Semicolon:
                    return ParseSemicolonExpression();
                case TokenType.Try:
                    return ParseTryExpression();
                case TokenType.EOF:
                    return null!;
                default:
                    return ParseUnitExpression();
            }
        }
    }

    private Syntax ParseBlockExpression()
    {
        ThrowError(!Match(TokenType.OpenBrace));
        Next();

        var expressions = new List<Syntax>();
        while (!Match(TokenType.CloseBrace))
        {
            var expr = ParseExpression();
            if (expr == null)
            {
                break;
            }

            expressions.Add(expr);
        }

        ThrowError(!Match(TokenType.CloseBrace));
        Next();
        return Syntax.MakeBlock(expressions.ToArray());
    }

    private Syntax ParseTryExpression()
    {
        ThrowError(!Match(TokenType.Try));
        var variable = new VariableExpression(new[] { new Variable("%catch%", null!) });
        var tryExpr = Parse(Next, ParseBlockExpression, ThrowNullError);
        var catchExpr = ParseCatchExpression();
        var finallyExpr = ParseFinallyExpression();
        return Syntax.MakeBlock(variable, new TryExpression(tryExpr, catchExpr, finallyExpr));
    }

    private Syntax ParseCatchExpression()
    {
        if (!Match(TokenType.Catch))
        {
            return new CatchExpression(null!, Syntax.MakeBlock(new ThrowExpression(Syntax.MakeName("%catch%"))));
        }

        Next();
        ThrowError(!Match(TokenType.Identifier));
        var variable = new Variable(_token.Value.ToString()!, Syntax.MakeName("%catch%"));
        return new CatchExpression(variable, Parse(Next, ParseBlockExpression, ThrowNullError));
    }

    private Syntax ParseFinallyExpression()
    {
        if (Match(TokenType.Finally))
        {
            Next();
            return new FinallyExpression(Parse(null, ParseBlockExpression, ThrowNullError));
        }
        else
        {
            return new FinallyExpression(Syntax.MakeBlock());
        }
    }

    private Syntax ParseIfExpression()
    {
        ThrowError(!Match(TokenType.If));
        Next();

        var test = ParseUnitExpression();
        var ifTrue = ParseExpression();
        if (Match(TokenType.Else))
        {
            Next();
            var ifFalse = ParseExpression();
            return new IfExpression(test, ifTrue, ifFalse);
        }

        return Syntax.MakeIf(test, ifTrue, null!);
    }

    private Syntax ParseBreakExpression()
    {
        ThrowError(!Match(TokenType.Break));
        Next();
        return Syntax.MakeBreak();
    }

    private Syntax ParseContinueExpression()
    {
        ThrowError(!Match(TokenType.Continue));
        Next();
        return Syntax.MakeContinue();
    }

    private Syntax ParseReturnExpression()
    {
        ThrowError(!Match(TokenType.Return));
        Next();
        var value = ParseUnitExpression();
        return Syntax.MakeReturn(value);
    }

    private Syntax ParseSemicolonExpression()
    {
        ThrowError(!Match(TokenType.Semicolon));
        Next();
        return Syntax.MakeEmpty();
    }

    private Syntax ParseLetExpression()
    {
        ThrowError(!Match(TokenType.Let));
        Next();

        var variables = new List<Variable>();
        for (var comma = false; ; comma = !comma)
        {
            if (comma)
            {
                if (!Match(TokenType.Comma))
                {
                    break;
                }

                Next();
                continue;
            }

            var name = string.Empty;
            ThrowError(!Match(TokenType.Identifier));
            Next(token => name = token.Value.ToString());

            if (!Match(TokenType.Assign))
            {
                variables.Add(new Variable(name, null!));
            }
            else
            {
                Next();
                var value = ParseUnitExpression();
                variables.Add(new Variable(name, value));
            }
        }

        ThrowError(variables.Count == 0);
        return Syntax.MakeVariable(variables.ToArray());
    }

    private Syntax ParseForExpression()
    {
        ThrowError(!Match(TokenType.For));
        Next();
        var initializers = ParseForInitializers();
        var condition = ParseForCondition();
        var iterators = ParseForIterators();
        var body = ParseExpression();
        return Syntax.MakeFor(initializers, condition, iterators, body);
    }

    private Syntax[] ParseForInitializers()
    {
        if (Match(TokenType.Semicolon))
        {
            Next();
            return new[] { new EmptyExpression() };
        }

        if (Match(TokenType.Let))
        {
            var varExpr = ParseLetExpression();
            ThrowError((!Match(TokenType.Semicolon)));
            Next();
            return new[] { varExpr };
        }

        var initializers = new List<Syntax>();
        for (var comma = false; !Match(TokenType.Semicolon); comma = !comma)
        {
            if (comma)
            {
                if (!Match(TokenType.Comma))
                {
                    break;
                }

                Next();
                continue;
            }

            var expr = ParseUnitExpression();
            if (expr == null)
            {
                break;
            }

            initializers.Add(expr);
        }

        ThrowError(!Match(TokenType.Semicolon));
        Next();
        return initializers.ToArray();
    }

    private Syntax ParseForCondition()
    {
        var test = ParseUnitExpression();
        ThrowError(!Match(TokenType.Semicolon));
        Next();
        return test;
    }

    private Syntax[] ParseForIterators()
    {
        if (Match(TokenType.OpenBrace))
        {
            Next();
            return new[] { new EmptyExpression() };
        }

        var iterators = new List<Syntax>();
        for (var i = 0; !Match(TokenType.OpenBrace); i++)
        {
            if (i % 2 != 0)
            {
                if (!Match(TokenType.Comma))
                {
                    break;
                }

                Next();
                continue;
            }

            var expr = ParseUnitExpression();
            if (expr == null)
            {
                break;
            }

            iterators.Add(expr);
        }

        ThrowError(!Match(TokenType.OpenBrace));
        return iterators.ToArray();
    }

    private Syntax ParseSwitchExpression()
    {
        throw new Exception("Not implemented");
    }

    private Module ParseModule()
    {
        var module = new Module(_lexer.FileName);
        Next();
        ParseImprots(module);
        ParseClasses(module);
        return module;
    }

    private void ParseClasses(Module module)
    {
        while (Match(TokenType.Class))
        {
            var name = string.Empty;
            var super = "Object";
            Next();
            ThrowError(!Match(TokenType.Identifier));
            Next(token => name = token.Value.ToString());

            if (Match(TokenType.Extends))
            {
                Next();
                ThrowError(!Match(TokenType.Identifier));
                Next(token => super = token.Value.ToString());
            }

            ThrowError(!Match(TokenType.OpenBrace));
            Next();
            var @class = new Class(name, super);

            while (Match(TokenType.Func))
            {
                var functionExpr = ParseFunctionExpression();
                if (functionExpr == null)
                {
                    break;
                }

                var rewriter = new Rewriters.ClosureExpressionRewriter(functionExpr, module, @class);
                var function = rewriter.Rewrite();
                @class.AddFunction(function!);
            }

            ThrowError(!Match(TokenType.CloseBrace));
            Next();
            module.AddClass(@class);
        }
    }

    private void ParseImprots(Module module)
    {
        while (Match(TokenType.Import))
        {
            var path = string.Empty;
            var classes = new List<KeyValuePair<string, string>>();
            Next();
            ThrowError(!Match(TokenType.OpenBrace));
            Next();

            while (!Match(TokenType.CloseBrace))
            {
                var name = string.Empty;
                var alias = string.Empty;
                ThrowError(!Match(TokenType.Identifier));
                Next(token =>
                {
                    name = _token.Value.ToString();
                    alias = name;
                });

                if (Match(TokenType.As))
                {
                    Next();
                    ThrowError(!Match(TokenType.Identifier));
                    Next(token => alias = token.Value.ToString());
                }

                if (Match(TokenType.Comma))
                {
                    Next();
                }
                else
                {
                    ThrowError(!Match(TokenType.CloseBrace));
                }

                classes.Add(new KeyValuePair<string, string>(name!, alias!));
            }

            ThrowError(!Match(TokenType.CloseBrace));
            Next();
            ThrowError(!Match(TokenType.From));
            Next();
            ThrowError(!Match(TokenType.LiteralString));
            Next(token => path = token.Value.ToString());
            module.AddImported(classes.Select(i => new Import(i.Key, i.Value, path)).ToArray());
        }
    }

    private static Syntax Parse(Action? onParsing, Func<Syntax> parser, Action<Syntax>? onParsed)
    {
        onParsing?.Invoke();
        var syntax = parser();
        onParsed?.Invoke(syntax);
        return syntax;
    }

    private void Next()
    {
        _token = _lexer.ScanNext();
    }

    private void Next(Action<Token> before)
    {
        before?.Invoke(_token);
        Next();
    }

    private void Next(Action<Token> before, Action<Token> after)
    {
        before?.Invoke(_token);
        Next();
        after?.Invoke(_token);
    }

    private bool Match(params TokenType[] types)
    {
        if (_token == null)
        {
            return false;
        }

        return types?.Any(item => item == _token.Type) ?? false;
    }

    private void ThrowError()
    {
        throw new Exception($"error at {_token?.Position?.ToString()}");
    }

    private void ThrowError(bool test)
    {
        if (test)
        {
            ThrowError();
        }
    }

    private void ThrowNullError(Syntax syntax)
    {
        ThrowError(syntax == null);
    }

    private Action<Syntax> ThrowNullError(params TokenType[] types)
    {
        return syntax =>
        {
            if (Match(types))
            {
                ThrowError(syntax == null);
            }
        };
    }
}