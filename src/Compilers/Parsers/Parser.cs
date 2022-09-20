﻿using System.Diagnostics.Metrics;
using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers;
using static System.Net.Mime.MediaTypeNames;

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

    private Syntax ParseUnitExpression()
    {
        var lExpr = ParseBitOrExpresion();
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

        var rExpr = Parse(Next, ParseUnitExpression, ThrowNullError);
        switch (token.Type)
        {
            case TokenType.Assign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.Assign);
            case TokenType.AddAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.AddAssign);
            case TokenType.BitAndAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.BitAndAssign);
            case TokenType.LeftShiftAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.BitLeftShiftAssign);
            case TokenType.BitOrAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.BitOrAssign);
            case TokenType.RightShiftAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.BitRightShiftAssign);
            case TokenType.BitXorAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.BitXorAssign);
            case TokenType.DivideAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.DivideAssign);
            case TokenType.ModuloAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.ModuloAssign);
            case TokenType.MultiplyAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.MultiplyAssign);
            case TokenType.SubtractAssign:
                return new BinaryExpression(lExpr, rExpr, SyntaxType.Assign);
            default:
                ThrowError();
                throw new Exception();
        }
    }

    private Syntax ParseBitOrExpresion()
    {
        var lExpr = Parse(null, ParseBitXorExpresion, ThrowNullErrorMatch(TokenType.BitOr));
        while (Match(TokenType.BitOr))
        {
            var rExpr = Parse(Next, ParseBitXorExpresion, ThrowNullError);
            lExpr = new BinaryExpression(lExpr, rExpr, SyntaxType.BitOr);
        }

        return lExpr;
    }

    private Syntax ParseBitXorExpresion()
    {
        var lExpr = Parse(null, ParseBitAndExpresion, ThrowNullErrorMatch(TokenType.BitXor));
        while (Match(TokenType.BitXor))
        {
            var rExpr = Parse(Next, ParseBitAndExpresion, ThrowNullError);
            lExpr = new BinaryExpression(lExpr, rExpr, SyntaxType.BitXor);
        }

        return lExpr;
    }

    private Syntax ParseBitAndExpresion()
    {
        var lExpr = Parse(null, ParseEqualityExpresion, ThrowNullErrorMatch(TokenType.BitAnd));
        while (Match(TokenType.BitAnd))
        {
            var rExpr = Parse(Next, ParseEqualityExpresion, ThrowNullError);
            lExpr = new BinaryExpression(lExpr, rExpr, SyntaxType.BitLeftShift);
        }

        return lExpr;
    }

    private Syntax ParseEqualityExpresion()
    {
        var tokens = new[] { TokenType.Equal, TokenType.NotEqual, TokenType.Is };
        var lExpr = Parse(null, ParseReleationExpresion, ThrowNullErrorMatch(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseReleationExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Is => new BinaryExpression(lExpr, rExpr, SyntaxType.Is),
                TokenType.Equal => new BinaryExpression(lExpr, rExpr, SyntaxType.EqualTo),
                _ => new BinaryExpression(lExpr, rExpr, SyntaxType.NotEqualTo),
            };
        }

        return lExpr;
    }

    private Syntax ParseReleationExpresion()
    {
        var tokens = new[] { TokenType.Less, TokenType.LessOrEqual, TokenType.Greater, TokenType.GreaterOrEqual };
        var lExpr = Parse(null, ParseShiftExpresion, ThrowNullErrorMatch(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseShiftExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Less => new BinaryExpression(lExpr, rExpr, SyntaxType.LessThan),
                TokenType.LessOrEqual => new BinaryExpression(lExpr, rExpr, SyntaxType.LessThanOrEqual),
                TokenType.Greater => new BinaryExpression(lExpr, rExpr, SyntaxType.GreaterThan),
                _ => new BinaryExpression(lExpr, rExpr, SyntaxType.GreaterThanOrEqual),
            };
        }

        return lExpr;
    }

    private Syntax ParseShiftExpresion()
    {
        var tokens = new[] { TokenType.LeftShift, TokenType.RightShift };
        var lExpr = Parse(null, ParseAdditiveExpresion, ThrowNullErrorMatch(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseAdditiveExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.LeftShift => new BinaryExpression(lExpr, rExpr, SyntaxType.BitLeftShift),
                _ => new BinaryExpression(lExpr, rExpr, SyntaxType.BitRightShift),
            };
        }

        return lExpr;
    }

    private Syntax ParseAdditiveExpresion()
    {
        var tokens = new[] { TokenType.Add, TokenType.Subtract };
        var lExpr = Parse(null, ParseMultiplicativeExpresion, ThrowNullErrorMatch(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseMultiplicativeExpresion, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Add => new BinaryExpression(lExpr, rExpr, SyntaxType.Add),
                _ => new BinaryExpression(lExpr, rExpr, SyntaxType.Subtract),
            };
        }

        return lExpr;
    }

    private Syntax ParseMultiplicativeExpresion()
    {
        var tokens = new[] { TokenType.Multiply, TokenType.Divide, TokenType.Modulo };
        var lExpr = Parse(null, ParseUnaryExpression, ThrowNullErrorMatch(tokens));
        while (Match(tokens))
        {
            var token = _token;
            var rExpr = Parse(Next, ParseUnaryExpression, ThrowNullError);
            lExpr = token.Type switch
            {
                TokenType.Multiply => new BinaryExpression(lExpr, rExpr, SyntaxType.Multiply),
                TokenType.Divide => new BinaryExpression(lExpr, rExpr, SyntaxType.Divide),
                _ => new BinaryExpression(lExpr, rExpr, SyntaxType.Modulo),
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
            TokenType.Add => new UnaryExpression(expr, SyntaxType.Add),
            TokenType.Subtract => new UnaryExpression(expr, SyntaxType.Subtract),
            TokenType.BitNot => new UnaryExpression(expr, SyntaxType.BitNot),
            _ => new UnaryExpression(expr, SyntaxType.Not),
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
        switch (_token.Type)
        {
            case TokenType.Identifier:
                Next();
                return new NameExpression(_token.Value.ToString()!);
            case TokenType.True:
            case TokenType.False:
                Next();
                return new ConstantExpression(_token.Value, SyntaxType.ConstBoolean);
            case TokenType.Number:
                Next();
                return new ConstantExpression(_token.Value, SyntaxType.ConstNumber);
            case TokenType.Null:
                Next();
                return new ConstantExpression(_token.Value, SyntaxType.ConstNull);
            case TokenType.Super:
                Next();
                return new SuperExpression();
            case TokenType.This:
                Next();
                return new ThisExpression();
            case TokenType.Native:
                Next();
                return new NameExpression("native");
            case TokenType.Static:
                Next();
                return new NameExpression("static");
            case TokenType.New:
                return ParseNewExpression();
            case TokenType.Func:
                return ParseLocalFunctionExpression();
            default:
                return null!;
        }
    }

    private Syntax ParseMemberExpression(Syntax @object)
    {
        try
        {
            if (Match(TokenType.Dot))
            {
                Next();
                ThrowError(!Match(TokenType.Identifier));
                return new MemberExpression(@object, new ConstantExpression(_token.Value.ToString()!, SyntaxType.ConstString));
            }

            var member = Parse(Next, ParseUnitExpression, ThrowNullError);
            ThrowError(!Match(TokenType.CloseSquare));
            return new MemberExpression(@object, member);
        }
        finally
        {
            Next();
        }
    }

    private Syntax ParseCallExpression(Syntax function)
    {
        return new CallExpression(function, ParseCallArguments().ToArray());
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
        var @class = Parse(null, ParseUnitExpression, ThrowNullError);
        var arguments = ParseCallArguments();
        return new NewExpression(@class, arguments.ToArray());
    }

    private Syntax ParseFunctionExpression()
    {
        var name = string.Empty;
        ThrowError(!Match(TokenType.Func));
        Next();
        ThrowError(!Match(TokenType.Identifier));
        name = _token.Value.ToString();
        Next();
        var parameters = ParseFunctionParameters();
        var functionBody = ParseFunctionBodyExpression();
        return new FunctionExpression(name, false, false, false, parameters, functionBody);
    }

    private Syntax ParseLocalFunctionExpression()
    {
        var name = string.Empty;
        ThrowError(!Match(TokenType.Func));
        Next();
        if (Match(TokenType.Identifier))
        {
            name = _token.Value.ToString();
            Next();
        }

        var parameters = ParseFunctionParameters();
        var functionBody = ParseFunctionBodyExpression();
        return new FunctionExpression(name, false, false, false, parameters, functionBody);
    }

    private Syntax ParseFunctionBodyExpression()
    {
        if (Match(TokenType.Arrow))
        {
            Next();
            return ParseExpression();
        }

        ThrowError(!Match(TokenType.OpenBrace));
        return ParseExpression();
    }

    private Syntax[] ParseFunctionParameters()
    {
        ThrowError(!Match(TokenType.OpenParen));
        Next();
        var list = new List<Syntax>();

        for (var i = 0; !Match(TokenType.CloseParen); i++)
        {
            if (i % 2 == 0)
            {
                ThrowError(!Match(TokenType.Comma));
                Next();
                continue;
            }

            ThrowError(!Match(TokenType.Identifier));
            list.Add(new ParameterExpression(_token.Value.ToString()!, list.Count));
        }

        return list.ToArray();
    }

    private Syntax ParseExpression()
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
        return new BlockExpression(expressions.ToArray());
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

        return new IfExpression(test, ifTrue, null!);
    }

    private Syntax ParseBreakExpression()
    {
        ThrowError(!Match(TokenType.Break));
        Next();
        return new BreakExpression();
    }

    private Syntax ParseContinueExpression()
    {
        ThrowError(!Match(TokenType.Continue));
        Next();
        return new ContinueExpression();
    }

    private Syntax ParseReturnExpression()
    {
        ThrowError(!Match(TokenType.Return));
        Next();
        var value = ParseUnitExpression();
        return new ReturnExpression(value);
    }

    private Syntax ParseSemicolonExpression()
    {
        ThrowError(!Match(TokenType.Semicolon));
        Next();
        return new EmptyExpression();
    }

    private Syntax ParseLetExpression()
    {
        ThrowError(!Match(TokenType.Let));
        Next();

        var variables = new List<VaraibleDefinition>();
        for (var i = 0; ; i++)
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

            var name = _token.Value.ToString()!;
            ThrowError(!Match(TokenType.Identifier));
            Next();

            if (!Match(TokenType.Assign))
            {
                variables.Add(new VaraibleDefinition(name, null!));
            }
            else
            {
                Next();
                var value = ParseUnitExpression();
                variables.Add(new VaraibleDefinition(name, value));
            }
        }

        ThrowError(variables.Count == 0);
        return new VariableExpression(variables.ToArray());
    }

    private Syntax ParseForExpression()
    {
        ThrowError(!Match(TokenType.For));
        Next();
        var initializers = ParseForInitializers();
        var condition = ParseForCondition();
        var iterators = ParseForIterators();
        var body = ParseExpression();
        return new ForExpression(initializers, condition, iterators, body);
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
        for (var i = 0; !Match(TokenType.Semicolon); i++)
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

            initializers.Add(expr);
        }

        ThrowError(!Match(TokenType.Semicolon));
        Next();
        return initializers.ToArray();
    }

    private Syntax ParseForCondition()
    {
        var test = ParseUnaryExpression();
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

    private Action<Syntax> ThrowNullErrorMatch(params TokenType[] types)
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