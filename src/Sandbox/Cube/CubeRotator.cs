using Arda.ECS.Core;
using Arda.Windowing;
using Silk.NET.Input;

class CubeRotator : MonoBehaviour
{
    private static readonly HashSet<Key> _keysHeld = new();
    private static bool _bound;

    public float Speed = 90f;

    public static void BindInput(SilkWindow window)
    {
        if (_bound) return;
        _bound = true;
        window.KeyDown += key => _keysHeld.Add(key);
        window.KeyUp   += key => _keysHeld.Remove(key);
    }

    public override void FixedUpdate(float dt)
    {
        if (_keysHeld.Contains(Key.Left))  Transform.Rotation.Y -= dt * Speed;
        if (_keysHeld.Contains(Key.Right)) Transform.Rotation.Y += dt * Speed;
        if (_keysHeld.Contains(Key.Up))    Transform.Rotation.X -= dt * Speed;
        if (_keysHeld.Contains(Key.Down))  Transform.Rotation.X += dt * Speed;
    }
}
