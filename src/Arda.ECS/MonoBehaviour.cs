namespace Arda.ECS;

public abstract class MonoBehaviour : Component
{
    public virtual void Start() { }
    public virtual void Update(float dt) { }
    public virtual void FixedUpdate(float dt) { }
    public virtual void Render(float dt) { }
    public virtual void OnDestroy() { }
}
