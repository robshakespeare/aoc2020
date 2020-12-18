using System;
using System.Linq;
using Sprache;

namespace AoC.Day18
{
    public interface IExpression
    {
        long Evaluate();
    }

    public record LiteralExpression(long Value) : IExpression
    {
        public long Evaluate() => Value;
    }

    public record BinaryExpression(char Op, IExpression Left, IExpression Right) : IExpression
    {
        public long Evaluate() => Op switch
        {
            '+' => Left.Evaluate() + Right.Evaluate(),
            '*' => Left.Evaluate() * Right.Evaluate(),
            _ => throw new InvalidOperationException($"Unsupported operator: {Op}")
        };
    }

    public class ExpressionEvaluator
    {
        public virtual long Evaluate(string expressionString) => Expression.Parse(expressionString).Evaluate();

        protected static readonly Parser<IExpression> Literal =
            from literal in Parse.Digit.AtLeastOnce().Token().Text()
            select new LiteralExpression(long.Parse(literal));

        protected static readonly Parser<char> Add = Parse.Char('+').Token();

        protected static readonly Parser<char> Multiply = Parse.Char('*').Token();

        private static readonly Parser<IExpression> SubExpression =
            from lp in Parse.Char('(').Token()
            from expr in Expression
            from rp in Parse.Char(')').Token()
            select expr;

        private static readonly Parser<IExpression> Term =
            SubExpression.XOr(Literal);

        private static readonly Parser<IExpression> Expression =
            Parse.ChainOperator(Add.Or(Multiply), Term, (op, left, right) => new BinaryExpression(op, left, right));
    }

    public class ExpressionEvaluator2 : ExpressionEvaluator
    {
        public override long Evaluate(string expressionString) => Expression.Parse(expressionString).Evaluate();

        private static readonly Parser<IExpression> SubExpression =
            from lp in Parse.Char('(').Token()
            from expr in Expression
            from rp in Parse.Char(')').Token()
            select expr;

        private static readonly Parser<IExpression> Term =
            SubExpression.XOr(Literal);

        private static readonly Parser<IExpression> InnerTerm =
            Parse.ChainOperator(Add, Term, (op, left, right) => new BinaryExpression(op, left, right));

        public static readonly Parser<IExpression> Expression =
            Parse.ChainOperator(Multiply, InnerTerm, (op, left, right) => new BinaryExpression(op, left, right));
    }

    public class ExpressionEvaluator3
    {
        private readonly string[] _operators;
        private readonly Parser<IExpression> _expressionParser;

        private ExpressionEvaluator3(params string[] operators)
        {
            _operators = operators;
            _expressionParser = Expression;
            //Parser<IExpression> Literal() =>
            //    from literal in Parse.Digit.AtLeastOnce().Token().Text()
            //    select new LiteralExpression(long.Parse(literal));

            //Parser<char> Add() => Parse.Char('+').Token();

            //Parser<char> Multiply() => Parse.Char('*').Token();

            //Parser<IExpression> SubExpression() =>
            //    from lp in Parse.Char('(').Token()
            //    from expr in Expression()
            //    from rp in Parse.Char(')').Token()
            //    select expr;

            //Parser<IExpression> Term() =>
            //    SubExpression().XOr(Literal());

            //Parser<IExpression> Expression() =>
            //    Parse.ChainOperator(Add().Or(Multiply()), Term(), (op, left, right) => new BinaryExpression(op, left, right));

            //Expression = Parse.ChainOperator(Add.Or(Multiply), Term, (op, left, right) => new BinaryExpression(op, left, right));
        }

        public static ExpressionEvaluator3 Part1Evaluator = new("+*");

        public static ExpressionEvaluator3 Part2Evaluator = new("+", "*");

        public long Evaluate(string expression) => _expressionParser.Parse(expression).Evaluate();

        protected Parser<IExpression> Literal =
            from literal in Parse.Digit.AtLeastOnce().Token().Text()
            select new LiteralExpression(long.Parse(literal));

        //protected Parser<char> Add => Parse.Char('+').Token();

        //protected Parser<char> Multiply => Parse.Char('*').Token();

        private Parser<IExpression> SubExpression =>
            from lp in Parse.Char('(').Token()
            from expr in Expression
            from rp in Parse.Char(')').Token()
            select expr;

        private Parser<IExpression> Term =>
            SubExpression.XOr(Literal);

        private Parser<IExpression> Expression =>
            _operators.Aggregate(
                Term,
                (term, ops) => Parse.ChainOperator(Parse.Chars(ops).Token(), term, (op, left, right) => new BinaryExpression(op, left, right)));

        //.Select(ops => Parse.ChainOperator(Parse.Chars(ops).Token(), Term, (op, left, right) => new BinaryExpression(op, left, right)))
        //Parse.ChainOperator(Add.Or(Multiply), Term, (op, left, right) => new BinaryExpression(op, left, right));
    }
}
