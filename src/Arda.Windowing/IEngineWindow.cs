using Silk.NET.Input;
using Silk.NET.Windowing;

namespace Arda.Windowing;

public interface IEngineWindow : IDisposable
{
    string Title { get; }
    int Width { get; }
    int Height { get; }

    event Action? Load;
    event Action<double>? Update;
    event Action<double>? FixedUpdate;
    event Action<double>? Render;
    event Action? Closing;

    event Action<Key>? KeyDown;
    event Action<Key>? KeyUp;

    double FixedTimestep { get; set; }

    IWindow Native { get; }
    void Run();
}
