namespace Arda.ECS;

public abstract class Component
{
    public GameObject GameObject { get; internal set; } = null!;
    public Transform Transform => GameObject.Transform;
}
