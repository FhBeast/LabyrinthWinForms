namespace Labyrinth.Model;

internal class Cell
{
    public bool UpAvailable { get; set; }
    public bool DownAvailable { get; set; }
    public bool LeftAvailable { get; set; }
    public bool RightAvailable { get; set; }
    public int StepsForEnd { get; set; }

    public Cell()
    {
        StepsForEnd = int.MaxValue;
    }

    public bool IsFound()
    {
        return StepsForEnd != int.MaxValue;
    }

    public int GetWaysNumber()
    {
        var ways = 0;

        if (UpAvailable)
        {
            ways++;
        }
        if (DownAvailable)
        {
            ways++;
        }
        if (LeftAvailable)
        {
            ways++;
        }
        if (RightAvailable)
        {
            ways++;
        }

        return ways;
    }
}
