using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Arda.Windowing;

public class SilkWindow : IEngineWindow
{
    private readonly IWindow _nativeWindow;
    private double _accumulator;

    public string Title { get; }
    public int Width { get; }
    public int Height { get; }
    public double FixedTimestep { get; set; } = 1.0 / 60.0;

    public event Action? Load;
    public event Action<double>? Update;
    public event Action<double>? FixedUpdate;
    public event Action<double>? Render;
    public event Action? Closing;

    public event Action<Key>? KeyDown;
    public event Action<Key>? KeyUp;

    public IWindow Native => _nativeWindow;
    public IInputContext? Input { get; private set; }

    public SilkWindow(string title, int width, int height)
    {
        Title  = title;
        Width  = width;
        Height = height;

        var options = WindowOptions.Default with
        {
            Title = title,
            Size  = new Vector2D<int>(width, height),
            API   = new GraphicsAPI(
                        ContextAPI.OpenGL,
                        ContextProfile.Core,
                        ContextFlags.Default,
                        new APIVersion(3, 3))
        };

        _nativeWindow = Window.Create(options);
        _nativeWindow.Load    += () =>
        {
            Input = _nativeWindow.CreateInput();
            foreach (var kb in Input.Keyboards)
            {
                kb.KeyDown += (_, key, _) =>
                {
                    switch (key)
                    {
                        case Key.F:
                            _nativeWindow.WindowState = WindowState.Fullscreen;
                            break;
                        case Key.Escape:
                            _nativeWindow.WindowState = WindowState.Normal;
                            break;
                    }
                    KeyDown?.Invoke(key);
                };
                kb.KeyUp += (_, key, _) => KeyUp?.Invoke(key);
            }
            Load?.Invoke();
        };
        _nativeWindow.Update  += dt =>
        {
            _accumulator += dt;
            while (_accumulator >= FixedTimestep)
            {
                FixedUpdate?.Invoke(FixedTimestep);
                _accumulator -= FixedTimestep;
            }
            Update?.Invoke(dt);
        };
        _nativeWindow.Render  += dt => Render?.Invoke(dt);
        _nativeWindow.Closing += () => Closing?.Invoke();
    }

    public void Run() => _nativeWindow.Run();

    public void Dispose()
    {
        Input?.Dispose();
        _nativeWindow.Dispose();
    }
}
