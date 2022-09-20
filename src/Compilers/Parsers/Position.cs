
namespace Pain.Compilers.Parsers;

public class Position
{
    public int Line { get; }

    public int Cell { get; }
    public string FileName { get; }

    public Position(string fileName, int line, int cell)
    {
        Line = line;
        Cell = cell;
        FileName = fileName;
    }

    public override string ToString()
    {
        return $"File:{FileName} Line:{Line} Cell:{Cell}";
    }
}