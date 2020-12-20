using System.Collections.Generic;

namespace AoC.Day20
{
    public record TileEdges(string Top, string Bottom, string Left, string Right)
    {
        public IEnumerable<string> GetAll()
        {
            yield return Top;
            yield return Bottom;
            yield return Left;
            yield return Right;
        }
    }
}
