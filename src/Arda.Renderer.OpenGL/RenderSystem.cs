using System.Numerics;
using Arda.Core.Components;
using Arda.ECS;
using Arda.ECS.Core;
using Silk.NET.OpenGL;

namespace Arda.Renderer.OpenGL;

public static class RenderSystem
{
    public static void Draw(Scene scene, OpenGLContext ctx)
    {
        var camera = scene.FindComponent<Camera>();
        if (camera is null) return;

        var renderers = scene.FindAll<MeshRenderer>();

        foreach (var r in renderers)
        {
            var model = r.Transform.LocalToWorldMatrix;

            r.Shader.Bind();
            r.Shader.SetMatrix4("uModel",       model);
            r.Shader.SetMatrix4("uView",        camera.ViewMatrix);
            r.Shader.SetMatrix4("uProjection",  camera.ProjectionMatrix);
            r.Shader.SetVector3("uLightPos",    new Vector3(2f, 3f, 2f));
            r.Shader.SetVector3("uLightColor",  Vector3.One);
            r.Shader.SetVector3("uObjectColor", r.ObjectColor);
            r.Shader.SetVector3("uViewPos",     camera.Position);

            r.VertexArray.Bind();
            ctx.GL.DrawArrays(PrimitiveType.Triangles, 0, r.VertexCount);
        }
    }
}
