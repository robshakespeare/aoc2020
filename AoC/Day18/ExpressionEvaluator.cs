using System;
using Sprache;

namespace AoC.Day18
{
    public interface IExpression
    {
        long Evaluate();
    }

    public record LiteralExpr(long Value) : IExpression
    {
        public long Evaluate() => Value;
    }

    public record BinaryExpr(char Op, IExpression Left, IExpression Right) : IExpression
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
            select new LiteralExpr(long.Parse(literal));

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
            Parse.ChainOperator(Add.Or(Multiply), Term, (op, left, right) => new BinaryExpr(op, left, right));
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
            Parse.ChainOperator(Add, Term, (op, left, right) => new BinaryExpr(op, left, right));

        public static readonly Parser<IExpression> Expression =
            Parse.ChainOperator(Multiply, InnerTerm, (op, left, right) => new BinaryExpr(op, left, right));
    }
}
