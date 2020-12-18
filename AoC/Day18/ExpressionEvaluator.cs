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
        public static ExpressionEvaluator Part1Evaluator { get; } = new("+*");

        public static ExpressionEvaluator Part2Evaluator { get; } = new("+", "*");

        private readonly string[] _operators;
        private readonly Parser<IExpression> _expressionParser;

        public ExpressionEvaluator(params string[] operators)
        {
            _operators = operators;
            _expressionParser = Expression;
        }

        public long Evaluate(string expression) => _expressionParser.Parse(expression).Evaluate();

        protected Parser<IExpression> Literal =
            from literal in Parse.Digit.AtLeastOnce().Text().Token()
            select new LiteralExpression(long.Parse(literal));

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
    }
}
