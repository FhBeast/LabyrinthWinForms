namespace Labyrinth.Model;

internal class Coin : BasePlayableEntity
{
    public bool IsPicked { get; set; }
    
    public Coin(int x, int y) : base(x, y) { }
}
