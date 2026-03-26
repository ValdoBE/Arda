# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Arda is a modular game engine written in C# targeting .NET 10.0. It uses Silk.NET for cross-platform graphics (OpenGL 3.3) and windowing (GLFW). The architecture is designed to support multiple rendering backends (OpenGL, Vulkan, DirectX12, Metal) through abstraction, though only OpenGL is currently implemented.

## Build & Run Commands

```bash
# Build the entire solution
dotnet build Arda.slnx

# Build a specific project
dotnet build src/Arda.ECS/Arda.ECS.csproj

# Run the Sandbox demo application
dotnet run --project src/Sandbox/Sandbox.csproj
```

There are no test projects yet.

## Architecture

The engine is organized into layered modules under `src/`, each in the `Arda.*` namespace:

- **Arda.ECS** — Unity-style GameObject/Component framework. `Scene` manages GameObjects and drives MonoBehaviour lifecycle (`Start`, `Update`, `FixedUpdate`, `Render`, `OnDestroy`). `Transform` is a built-in component on every GameObject. Scene provides `FindComponent<T>()` and `FindAll<T>()` for querying components across the scene.
- **Arda.Core** — Built-in engine components: `Camera` (view/projection matrices, position) and `MeshRenderer` (vertex array, shader, color). Depends on Arda.ECS and Arda.Renderer.
- **Arda.Renderer** — Abstract rendering API. Defines `IRendererBackend`, `IGraphicsContext`, `IVertexBuffer`, `IVertexArray`, `IIndexBuffer`, `IShader`, and `BufferLayout`. `RendererFactory` is the service locator for plugging in backends. No dependencies.
- **Arda.Renderer.OpenGL** — OpenGL 3.3 implementation of the renderer interfaces using Silk.NET. Includes `RenderSystem` which finds the scene's `Camera`, iterates all `MeshRenderer` components, and draws them. Uses unsafe code for vertex attribute pointer setup.
- **Arda.Windowing** — Window abstraction. `SilkWindow` wraps Silk.NET windowing with an event-based lifecycle: Load, Update, FixedUpdate (60 FPS accumulator), Render, Closing, KeyDown, KeyUp. Handles fullscreen toggle (F key) and escape to windowed. No engine dependencies.
- **Sandbox** — Executable demo app rendering two interactive 3D cubes with Blinn-Phong lighting and an orbit camera. Organized into `Cube/` (entity factory, rotator) and `Camera/` (orbit controller with mouse input).

### Dependency flow

```
Sandbox → Arda.Renderer.OpenGL, Arda.Windowing, Arda.ECS, Arda.Core
Arda.Renderer.OpenGL → Arda.Renderer, Arda.ECS, Arda.Core
Arda.Core → Arda.ECS, Arda.Renderer
Arda.Windowing → (Silk.NET only)
Arda.ECS → (no dependencies)
Arda.Renderer → (no dependencies)
```

### Key design decisions

- Uses **Unity-style GameObject/MonoBehaviour** pattern, not a data-oriented ECS.
- `Component` is the base class; `MonoBehaviour` extends it with lifecycle methods.
- `Transform` lives in Arda.ECS (tightly coupled to GameObject). `Camera` and `MeshRenderer` live in Arda.Core.
- Rendering is centralized in `RenderSystem` — MeshRenderer is a passive data component, not a MonoBehaviour.
- All GPU resources are created through `IRendererBackend` — never instantiate OpenGL types directly.
- Math uses `System.Numerics` (Vector3, Matrix4x4) — pass to OpenGL with `transpose=false` (row-major memory matches GLSL column-major layout).
- GLSL shaders are embedded as raw string literals (`"""..."""`) in C# source.
- Input uses Silk.NET.Input via `SilkWindow.Input`. Keyboard events are exposed as `KeyDown`/`KeyUp` on the window. Mouse input is accessed directly through `window.Input.Mice[0]`.
