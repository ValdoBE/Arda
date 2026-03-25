# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Arda is a modular game engine written in C# targeting .NET 10.0. It uses Silk.NET for cross-platform graphics (OpenGL 3.3) and windowing (GLFW). The architecture is designed to support multiple rendering backends (OpenGL, Vulkan, DirectX12, Metal) through abstraction, though only OpenGL is currently implemented.

## Build & Run Commands

```bash
# Build the entire solution
dotnet build Arda.slnx

# Build a specific project
dotnet build src/Engine.ECS/Engine.ECS.csproj

# Run the Sandbox demo application
dotnet run --project src/Sandbox/Sandbox.csproj
```

There are no test projects yet.

## Architecture

The engine is organized into layered modules under `src/`, each in the `Arda.Engine.*` namespace:

- **Engine.Core** — Placeholder library, currently empty.
- **Engine.ECS** — Entity Component System. `World` manages entities and stores components in `SparseSet<T>` (sparse-dense array for O(1) access). Components are value-type structs. Querying is done via `World.Query<T1,T2,...>()` with delegate callbacks.
- **Engine.Renderer** — Abstract rendering API. Defines `IRendererBackend`, `IGraphicsContext`, `IVertexBuffer`, `IVertexArray`, `IIndexBuffer`, `IShader`, and `BufferLayout`. `RendererFactory` is the service locator for plugging in backends.
- **Engine.Renderer.OpenGL** — OpenGL 3.3 implementation of the renderer interfaces using Silk.NET. Uses unsafe code for vertex attribute pointer setup.
- **Engine.Windowing** — Window abstraction. `SilkWindow` wraps Silk.NET windowing with an event-based lifecycle: Load, Update, FixedUpdate (60 FPS accumulator), Render, Closing.
- **Sandbox** — Executable demo app rendering a rotating 3D cube with Blinn-Phong lighting. Wires together the ECS, renderer, and windowing layers.

### Dependency flow

```
Sandbox → Engine.ECS, Engine.Renderer.OpenGL, Engine.Windowing
Engine.Renderer.OpenGL → Engine.Renderer (implements interfaces)
Engine.Windowing → Engine.Renderer (uses IGraphicsContext, RendererFactory)
```

### Key design decisions

- Components are **structs** (not classes) for cache-friendly ECS iteration.
- `Entity` is a `readonly record struct` wrapping a `uint` ID.
- All GPU resources are created through `IRendererBackend` — never instantiate OpenGL types directly.
- Math uses `System.Numerics` (Vector3, Matrix4x4) — row-major memory layout.
- GLSL shaders are embedded as raw string literals (`"""..."""`) in C# source.
