using Arda.Core.Components;
using Arda.Core.Logging;
using Arda.Core.Physics;
using Arda.ECS;
using Arda.ECS.Core;
using Arda.Windowing;
using Silk.NET.Input;

class ClickHandler : MonoBehaviour
{
    private static bool _bound;
    private static int _totalClicks;

    public static void BindInput(SilkWindow window, Scene scene)
    {
        if (_bound) return;
        _bound = true;

        var mouse = window.Input!.Mice[0];
        mouse.MouseDown += (_, btn) =>
        {
            if (btn != MouseButton.Left) return;

            var camera = scene.FindComponent<Camera>();
            if (camera is null) return;

            var ray = Physics.ScreenToRay(camera, mouse.Position.X, mouse.Position.Y, window.Width, window.Height);

            Clickable? closest = null;
            float closestDist = float.MaxValue;

            foreach (var clickable in scene.FindAll<Clickable>())
            {
                var mr = clickable.GameObject.GetComponent<MeshRenderer>();
                if (mr is null) continue;

                var dist = Physics.RayIntersects(ray, mr);
                if (dist.HasValue && dist.Value < closestDist)
                {
                    closestDist = dist.Value;
                    closest = clickable;
                }
            }

            if (closest is not null)
            {
                closest.ClickCount++;
                _totalClicks++;
                window.Native.Title = $"Arda – Clicks: {_totalClicks}";
                Log.Info("Hit '{0}' (click #{1}, total: {2})", closest.GameObject.Name, closest.ClickCount, _totalClicks);
            }
            else
            {
                Log.Debug("Click missed — no object hit");
            }
        };
    }
}
