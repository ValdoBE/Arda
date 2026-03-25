using Arda.ECS;
using Arda.Renderer;
using Arda.Renderer.OpenGL;
using Arda.Windowing;

var window = new SilkWindow("Arda – Renderer Test", 1280, 720);
var scene  = new Scene();
OpenGLContext? ctx = null;

window.Load += () =>
{
    ctx = new OpenGLContext(window.Native);
    ctx.Initialize();
    RendererFactory.RegisterBackend(new OpenGLRendererBackend(ctx.GL), RendererAPI.OpenGL);

    CubeEntity.Create(scene, window, ctx);
};

window.FixedUpdate += dt => scene.FixedUpdate((float)dt);
window.Update      += dt => scene.Update((float)dt);

window.Render += dt =>
{
    ctx!.Clear(0.08f, 0.08f, 0.10f);
    scene.Render((float)dt);
};

window.Closing += () =>
{
    scene.Destroy();
    ctx?.Dispose();
};

window.Run();
