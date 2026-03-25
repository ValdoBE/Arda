namespace Arda.ECS;

public class GameObject
{
    public string Name { get; set; }
    public Transform Transform { get; }

    internal Scene Scene { get; }
    private readonly List<Component> _components = new();

    internal GameObject(string name, Scene scene)
    {
        Name = name;
        Scene = scene;
        Transform = new Transform { GameObject = this };
        _components.Add(Transform);
    }

    public T AddComponent<T>() where T : Component, new()
    {
        var component = new T();
        component.GameObject = this;
        _components.Add(component);

        if (component is MonoBehaviour mb)
            Scene.RegisterBehaviour(mb);

        return component;
    }

    public T? GetComponent<T>() where T : Component
    {
        foreach (var c in _components)
            if (c is T t) return t;
        return null;
    }

    public bool TryGetComponent<T>(out T? component) where T : Component
    {
        component = GetComponent<T>();
        return component is not null;
    }
}
