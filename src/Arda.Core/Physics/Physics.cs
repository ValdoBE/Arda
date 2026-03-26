using System.Numerics;
using Arda.Core.Components;
using Arda.ECS.Core;

namespace Arda.Core.Physics;

public static class Physics
{
    /// <summary>
    /// Creates a ray from screen coordinates using the camera's view/projection matrices.
    /// </summary>
    public static Ray ScreenToRay(Camera camera, float screenX, float screenY, float screenWidth, float screenHeight)
    {
        // Convert screen coords to NDC [-1, 1]
        float ndcX = (2f * screenX / screenWidth) - 1f;
        float ndcY = 1f - (2f * screenY / screenHeight);

        // Unproject near and far points
        Matrix4x4.Invert(camera.ViewMatrix, out var invView);
        Matrix4x4.Invert(camera.ProjectionMatrix, out var invProj);
        var invVP = invProj * invView;

        var nearPoint = Unproject(new Vector3(ndcX, ndcY, 0f), invVP);
        var farPoint  = Unproject(new Vector3(ndcX, ndcY, 1f), invVP);

        return new Ray(nearPoint, farPoint - nearPoint);
    }

    /// <summary>
    /// Tests a ray against an axis-aligned bounding box defined by min/max corners.
    /// Returns the distance along the ray on hit, or null on miss.
    /// </summary>
    public static float? RayIntersectsAABB(Ray ray, Vector3 min, Vector3 max)
    {
        float tMin = float.NegativeInfinity;
        float tMax = float.PositiveInfinity;

        for (int i = 0; i < 3; i++)
        {
            float origin = GetComponent(ray.Origin, i);
            float dir    = GetComponent(ray.Direction, i);
            float bMin   = GetComponent(min, i);
            float bMax   = GetComponent(max, i);

            if (MathF.Abs(dir) < 1e-8f)
            {
                if (origin < bMin || origin > bMax)
                    return null;
            }
            else
            {
                float t1 = (bMin - origin) / dir;
                float t2 = (bMax - origin) / dir;
                if (t1 > t2) (t1, t2) = (t2, t1);
                tMin = MathF.Max(tMin, t1);
                tMax = MathF.Min(tMax, t2);
                if (tMin > tMax) return null;
            }
        }

        return tMin >= 0 ? tMin : (tMax >= 0 ? tMax : null);
    }

    /// <summary>
    /// Tests a ray against a MeshRenderer's bounding box (unit cube transformed by its Transform).
    /// </summary>
    public static float? RayIntersects(Ray ray, MeshRenderer renderer)
    {
        // The cube mesh spans [-0.5, 0.5] in local space.
        // Transform it to world space AABB.
        var transform = renderer.Transform;
        var model = transform.LocalToWorldMatrix;

        // Transform all 8 corners of the local AABB to world space
        Vector3 localMin = new(-0.5f, -0.5f, -0.5f);
        Vector3 localMax = new(0.5f, 0.5f, 0.5f);

        Vector3 worldMin = new(float.MaxValue);
        Vector3 worldMax = new(float.MinValue);

        for (int i = 0; i < 8; i++)
        {
            var corner = new Vector3(
                (i & 1) == 0 ? localMin.X : localMax.X,
                (i & 2) == 0 ? localMin.Y : localMax.Y,
                (i & 4) == 0 ? localMin.Z : localMax.Z);

            var world = Vector3.Transform(corner, model);
            worldMin = Vector3.Min(worldMin, world);
            worldMax = Vector3.Max(worldMax, world);
        }

        return RayIntersectsAABB(ray, worldMin, worldMax);
    }

    private static Vector3 Unproject(Vector3 ndc, Matrix4x4 invViewProj)
    {
        var clip = Vector4.Transform(new Vector4(ndc, 1f), invViewProj);
        return new Vector3(clip.X, clip.Y, clip.Z) / clip.W;
    }

    private static float GetComponent(Vector3 v, int i) => i switch
    {
        0 => v.X,
        1 => v.Y,
        _ => v.Z
    };
}
