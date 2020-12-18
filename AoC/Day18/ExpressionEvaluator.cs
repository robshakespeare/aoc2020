using System;
using Sprache;

namespace AoC.Day18
{
    public class ExpressionEvaluator
    {
        public long Evaluate(string expressionString)
        {
            var expression = ExpressionParser.Expression.Parse(expressionString);
            return expression.Evaluate();
        }

        public static class ExpressionParser
        {
            private static readonly Parser<IExpression> Literal =
                from literal in Parse.Digit.AtLeastOnce().Token().Text()
                select new LiteralExpr(long.Parse(literal));

            private static readonly Parser<char> Operator = Parse.Chars('+', '*').Token();

            private static readonly Parser<IExpression> SubExpression =
                from lp in Parse.Char('(').Token()
                from expr in Expression
                from rp in Parse.Char(')').Token()
                select expr;

            private static readonly Parser<IExpression> Term =
                SubExpression.XOr(Literal);

            public static readonly Parser<IExpression> Expression = Parse.ChainOperator(
                Operator,
                Term,
                (op, left, right) => new BinaryExpr(op, left, right));
        }

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
    }
}
