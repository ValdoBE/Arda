using Arda.Core;
using Arda.ECS;
using Arda.Renderer;
using Arda.Windowing;

static class CubeEntity
{
    private const string VertSrc = """
        #version 330 core
        layout(location = 0) in vec3 aPosition;
        layout(location = 1) in vec3 aNormal;

        out vec3 vFragPos;
        out vec3 vNormal;

        uniform mat4 uModel;
        uniform mat4 uView;
        uniform mat4 uProjection;

        void main()
        {
            vFragPos    = vec3(uModel * vec4(aPosition, 1.0));
            vNormal     = mat3(transpose(inverse(uModel))) * aNormal;
            gl_Position = uProjection * uView * vec4(vFragPos, 1.0);
        }
        """;

    private const string FragSrc = """
        #version 330 core
        in  vec3 vFragPos;
        in  vec3 vNormal;
        out vec4 FragColor;

        uniform vec3 uLightPos;
        uniform vec3 uLightColor;
        uniform vec3 uObjectColor;
        uniform vec3 uViewPos;

        void main()
        {
            vec3 ambient = 0.12 * uLightColor;

            vec3 norm     = normalize(vNormal);
            vec3 lightDir = normalize(uLightPos - vFragPos);
            vec3 diffuse  = max(dot(norm, lightDir), 0.0) * uLightColor;

            vec3  viewDir  = normalize(uViewPos - vFragPos);
            vec3  halfDir  = normalize(lightDir + viewDir);
            float spec     = pow(max(dot(norm, halfDir), 0.0), 64.0);
            vec3  specular = 0.6 * spec * uLightColor;

            FragColor = vec4((ambient + diffuse + specular) * uObjectColor, 1.0);
        }
        """;

    private static float[] Vertices() =>
    [
        // Front  (z = +0.5,  n = 0, 0, 1)
        -0.5f, -0.5f,  0.5f,  0, 0, 1,
         0.5f, -0.5f,  0.5f,  0, 0, 1,
         0.5f,  0.5f,  0.5f,  0, 0, 1,
         0.5f,  0.5f,  0.5f,  0, 0, 1,
        -0.5f,  0.5f,  0.5f,  0, 0, 1,
        -0.5f, -0.5f,  0.5f,  0, 0, 1,
        // Back   (z = -0.5,  n = 0, 0, -1)
         0.5f, -0.5f, -0.5f,  0, 0, -1,
        -0.5f, -0.5f, -0.5f,  0, 0, -1,
        -0.5f,  0.5f, -0.5f,  0, 0, -1,
        -0.5f,  0.5f, -0.5f,  0, 0, -1,
         0.5f,  0.5f, -0.5f,  0, 0, -1,
         0.5f, -0.5f, -0.5f,  0, 0, -1,
        // Left   (x = -0.5,  n = -1, 0, 0)
        -0.5f, -0.5f, -0.5f, -1, 0, 0,
        -0.5f, -0.5f,  0.5f, -1, 0, 0,
        -0.5f,  0.5f,  0.5f, -1, 0, 0,
        -0.5f,  0.5f,  0.5f, -1, 0, 0,
        -0.5f,  0.5f, -0.5f, -1, 0, 0,
        -0.5f, -0.5f, -0.5f, -1, 0, 0,
        // Right  (x = +0.5,  n = 1, 0, 0)
         0.5f, -0.5f,  0.5f,  1, 0, 0,
         0.5f, -0.5f, -0.5f,  1, 0, 0,
         0.5f,  0.5f, -0.5f,  1, 0, 0,
         0.5f,  0.5f, -0.5f,  1, 0, 0,
         0.5f,  0.5f,  0.5f,  1, 0, 0,
         0.5f, -0.5f,  0.5f,  1, 0, 0,
        // Bottom (y = -0.5,  n = 0, -1, 0)
        -0.5f, -0.5f, -0.5f,  0, -1, 0,
         0.5f, -0.5f, -0.5f,  0, -1, 0,
         0.5f, -0.5f,  0.5f,  0, -1, 0,
         0.5f, -0.5f,  0.5f,  0, -1, 0,
        -0.5f, -0.5f,  0.5f,  0, -1, 0,
        -0.5f, -0.5f, -0.5f,  0, -1, 0,
        // Top    (y = +0.5,  n = 0, 1, 0)
        -0.5f,  0.5f,  0.5f,  0, 1, 0,
         0.5f,  0.5f,  0.5f,  0, 1, 0,
         0.5f,  0.5f, -0.5f,  0, 1, 0,
         0.5f,  0.5f, -0.5f,  0, 1, 0,
        -0.5f,  0.5f, -0.5f,  0, 1, 0,
        -0.5f,  0.5f,  0.5f,  0, 1, 0,
    ];

    public static GameObject Create(
        Scene scene, SilkWindow window,
        System.Numerics.Vector3 position, System.Numerics.Vector3 color)
    {
        var vbo = RendererFactory.CreateVertexBuffer(Vertices());
        vbo.Layout = new BufferLayout([
            new BufferElement("aPosition", ShaderDataType.Float3),
            new BufferElement("aNormal",   ShaderDataType.Float3),
        ]);

        var va = RendererFactory.CreateVertexArray();
        va.AddVertexBuffer(vbo);
        var shader = RendererFactory.CreateShader(VertSrc, FragSrc);

        var cube = scene.CreateGameObject("Cube");
        cube.Transform.Position = position;

        CubeRotator.BindInput(window);
        cube.AddComponent<CubeRotator>();

        var renderer = cube.AddComponent<MeshRenderer>();
        renderer.VertexArray = va;
        renderer.VertexCount = 36;
        renderer.Shader = shader;
        renderer.ObjectColor = color;

        return cube;
    }
}
