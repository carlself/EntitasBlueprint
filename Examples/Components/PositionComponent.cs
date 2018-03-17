using Entitas;

[Game]
public class PositionComponent : IComponent
{
    public Vector2D value;
}

public struct Vector2D
{
    public float x;
    public float y;
}