using System.Numerics;
using Arda.Core;
using Arda.ECS;
using Arda.Windowing;
using Silk.NET.Input;

class OrbitCameraController : MonoBehaviour
{
    public Vector3 Target = Vector3.Zero;
    public float Yaw = 0f;
    public float Pitch = 0.3f;
    public float Distance = 5f;
    public float FieldOfView = MathF.PI / 4f;
    public float AspectRatio = 1280f / 720f;
    public float NearPlane = 0.1f;
    public float FarPlane = 100f;

    public float OrbitSensitivity = 0.3f;
    public float PanSensitivity = 0.005f;
    public float ZoomSensitivity = 1.0f;
    public float MinDistance = 0.5f;
    public float MaxDistance = 50f;

    private static bool _bound;
    private static bool _leftHeld;
    private static bool _middleHeld;
    private static bool _shiftHeld;
    private static Vector2 _lastMousePos;
    private static Vector2 _mouseDelta;
    private static float _scrollDelta;

    public static void BindInput(SilkWindow window)
    {
        if (_bound) return;
        _bound = true;

        window.KeyDown += key => { if (key == Key.ShiftLeft || key == Key.ShiftRight) _shiftHeld = true; };
        window.KeyUp   += key => { if (key == Key.ShiftLeft || key == Key.ShiftRight) _shiftHeld = false; };

        var mouse = window.Input!.Mice[0];
        _lastMousePos = new Vector2(mouse.Position.X, mouse.Position.Y);

        mouse.MouseDown += (_, btn) =>
        {
            if (btn == MouseButton.Left)   _leftHeld = true;
            if (btn == MouseButton.Middle)  _middleHeld = true;
            _lastMousePos = new Vector2(mouse.Position.X, mouse.Position.Y);
        };

        mouse.MouseUp += (_, btn) =>
        {
            if (btn == MouseButton.Left)   _leftHeld = false;
            if (btn == MouseButton.Middle)  _middleHeld = false;
        };

        mouse.MouseMove += (_, pos) =>
        {
            var current = new Vector2(pos.X, pos.Y);
            _mouseDelta += current - _lastMousePos;
            _lastMousePos = current;
        };

        mouse.Scroll += (_, wheel) =>
        {
            _scrollDelta += wheel.Y;
        };
    }

    public override void Update(float dt)
    {
        var delta = _mouseDelta;
        var scroll = _scrollDelta;
        _mouseDelta = Vector2.Zero;
        _scrollDelta = 0f;

        bool panning = _middleHeld || (_leftHeld && _shiftHeld);
        bool orbiting = _leftHeld && !_shiftHeld;

        if (orbiting)
        {
            Yaw   += delta.X * OrbitSensitivity * MathF.PI / 180f;
            Pitch -= delta.Y * OrbitSensitivity * MathF.PI / 180f;
            Pitch  = Math.Clamp(Pitch, -89f * MathF.PI / 180f, 89f * MathF.PI / 180f);
        }

        if (panning)
        {
            var right = new Vector3(
                GetCamera().ViewMatrix.M11,
                GetCamera().ViewMatrix.M21,
                GetCamera().ViewMatrix.M31);
            var up = new Vector3(
                GetCamera().ViewMatrix.M12,
                GetCamera().ViewMatrix.M22,
                GetCamera().ViewMatrix.M32);

            Target -= right * delta.X * PanSensitivity * Distance;
            Target += up    * delta.Y * PanSensitivity * Distance;
        }

        if (scroll != 0f)
        {
            Distance -= scroll * ZoomSensitivity;
            Distance  = Math.Clamp(Distance, MinDistance, MaxDistance);
        }

        // Compute camera position from spherical coordinates
        var cam = GetCamera();
        cam.Position = Target + new Vector3(
            Distance * MathF.Cos(Pitch) * MathF.Sin(Yaw),
            Distance * MathF.Sin(Pitch),
            Distance * MathF.Cos(Pitch) * MathF.Cos(Yaw));

        cam.ViewMatrix = Matrix4x4.CreateLookAt(cam.Position, Target, Vector3.UnitY);
        cam.ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
            FieldOfView, AspectRatio, NearPlane, FarPlane);
    }

    private Camera GetCamera() => GameObject.GetComponent<Camera>()!;
}
