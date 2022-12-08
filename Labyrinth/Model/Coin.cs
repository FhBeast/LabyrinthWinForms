namespace Labyrinth.Model;

internal class Coin : BasePlayableEntity
{
    public bool IsPicked { get; set; }
    
    public Coin(int x, int y) : base(x, y) { }

    public override void CollisionAction(BasePlayableEntity entity)
    {
        if (entity is Player)
        {
            IsPicked = true;
        }
    }
}
