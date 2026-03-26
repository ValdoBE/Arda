using System.Numerics;
using Arda.Core.Components;
using Arda.Core.Logging;
using Arda.ECS;
using Arda.Renderer;
using Arda.Renderer.OpenGL;
using Arda.Windowing;

var window = new SilkWindow("Arda – Clicks: 0", 1280, 720);
var scene  = new Scene();
OpenGLContext? ctx = null;

window.Load += () =>
{
    ctx = new OpenGLContext(window.Native);
    ctx.Initialize();
    RendererFactory.RegisterBackend(new OpenGLRendererBackend(ctx.GL), RendererAPI.OpenGL);
    Log.Info("Renderer initialized (OpenGL)");

    // Camera
    OrbitCameraController.BindInput(window);
    var cameraGo = scene.CreateGameObject("MainCamera");
    cameraGo.AddComponent<Camera>();
    cameraGo.AddComponent<OrbitCameraController>();

    // Cubes
    CubeEntity.Create(scene, window, new Vector3(-1.5f, 0f, 0f), new Vector3(0.49f, 0.29f, 0.78f));
    CubeEntity.Create(scene, window, new Vector3( 1.5f, 0f, 0f), new Vector3(0.29f, 0.78f, 0.49f));

    // Click picking
    ClickHandler.BindInput(window, scene);

    Log.Info("Scene loaded: {0} GameObjects", 3);
};

window.FixedUpdate += dt => scene.FixedUpdate((float)dt);
window.Update      += dt => scene.Update((float)dt);

window.Render += _ =>
{
    ctx!.Clear(0.08f, 0.08f, 0.10f);
    RenderSystem.Draw(scene, ctx);
};

window.Closing += () =>
{
    scene.Destroy();
    ctx?.Dispose();
};

window.Run();
