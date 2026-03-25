namespace Arda.ECS;

public class Scene
{
    private readonly List<GameObject> _gameObjects = new();
    private readonly List<MonoBehaviour> _behaviours = new();
    private readonly List<MonoBehaviour> _pendingStart = new();

    public GameObject CreateGameObject(string name = "GameObject")
    {
        var go = new GameObject(name, this);
        _gameObjects.Add(go);
        return go;
    }

    internal void RegisterBehaviour(MonoBehaviour mb) => _pendingStart.Add(mb);

    public void Update(float dt)
    {
        FlushPendingStarts();
        for (int i = 0; i < _behaviours.Count; i++)
            _behaviours[i].Update(dt);
    }

    public void FixedUpdate(float dt)
    {
        FlushPendingStarts();
        for (int i = 0; i < _behaviours.Count; i++)
            _behaviours[i].FixedUpdate(dt);
    }

    public void Render(float dt)
    {
        for (int i = 0; i < _behaviours.Count; i++)
            _behaviours[i].Render(dt);
    }

    public void Destroy()
    {
        foreach (var mb in _behaviours)
            mb.OnDestroy();
        _behaviours.Clear();
        _pendingStart.Clear();
        _gameObjects.Clear();
    }

    private void FlushPendingStarts()
    {
        if (_pendingStart.Count == 0) return;
        foreach (var mb in _pendingStart)
        {
            mb.Start();
            _behaviours.Add(mb);
        }
        _pendingStart.Clear();
    }
}
