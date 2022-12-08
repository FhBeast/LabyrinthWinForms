using Labyrinth.Model;

namespace Labyrinth.Util;

internal static class CollisionVerificator
{
    public static bool Collision(BasePlayableEntity entityFirst, BasePlayableEntity entitySecond)
    {

        if (entityFirst is null)
        {
            throw new ArgumentNullException($"{nameof(entityFirst)}");
        }

        if (entitySecond is null)
        {
            throw new ArgumentNullException($"{nameof(entitySecond)}");
        }

        if (entityFirst.PositionX == entitySecond.PositionX &&
            entityFirst.PositionY == entitySecond.PositionY)
        {
            entityFirst.CollisionAction(entitySecond);
            entitySecond.CollisionAction(entityFirst);
            return true;
        }
        return false;
    }

    public static bool Collision(List<BasePlayableEntity> list, BasePlayableEntity entity)
    {

        if (entity is null)
        {
            throw new ArgumentNullException($"{nameof(entity)}");
        }

        if (list is null)
        {
            throw new ArgumentNullException($"{nameof(list)}");
        }

        foreach (var ent in list)
        {
            if (ent.PositionX == entity.PositionX &&
            ent.PositionY == entity.PositionY)
            {
                ent.CollisionAction(entity);
                entity.CollisionAction(ent);
                return true;
            }
        }

        return false;
    }

    public static bool Collision(BasePlayableEntity entity, List<BasePlayableEntity> list)
    {

        if (entity is null)
        {
            throw new ArgumentNullException($"{nameof(entity)}");
        }

        if (list is null)
        {
            throw new ArgumentNullException($"{nameof(list)}");
        }

        return Collision(list, entity);
    }

    public static bool Collision(List<BasePlayableEntity> listFirst, List<BasePlayableEntity> listSecond)
    {

        if (listFirst is null)
        {
            throw new ArgumentNullException($"{nameof(listFirst)}");
        }

        if (listSecond is null)
        {
            throw new ArgumentNullException($"{nameof(listSecond)}");
        }

        foreach (var entityFirst in listFirst)
        {
            foreach (var entitySecond in listSecond)
            {
                if (entityFirst.PositionX == entitySecond.PositionX &&
                    entityFirst.PositionY == entitySecond.PositionY)
                {
                    entityFirst.CollisionAction(entitySecond);
                    entitySecond.CollisionAction(entityFirst);
                    return true;
                }
            }
        }
        return Collision(listFirst, listSecond);
    }
}
