using System.Collections.Generic;
using UnityEngine;

public class FaceVertex
{
    public Vector3 position = Vector3.zero;
    public Vector2 uv = Vector2.zero;
    public Vector3 normal = Vector3.forward;
}

public class VertexData
{
    public Face face;
    public FaceVertex vertex;
    public int faceIndex;
    public int meshIndex;
}

public class Face
{
    public List<FaceVertex> vertices = new();

    public void Add(Vector3 pos, Vector3 uv)
    {
        var vertex = new FaceVertex();

        vertex.position = pos;
        vertex.uv = uv;
        vertex.normal = Vector3.forward;

        vertices.Add(vertex);
    }

    public static Vector3 BiLerp(Vector3 a, Vector3 b, float horizontal, float vertical)
    {
        var x = Mathf.Lerp(a.x, b.x, horizontal);
        var y = Mathf.Lerp(a.y, b.y, vertical);
        var z = Mathf.Lerp(a.z, b.z, horizontal);

        return new Vector3(x, y, z);
    }

    public static Face CreateSquareQuad(Vector3 cornerStart, Vector3 cornerEnd)
    {
        var face = new Face();

        var minUV = Vector2.zero;
        var maxUV = Vector2.one;

        face.Add(cornerStart, minUV);
        face.Add(BiLerp(cornerStart, cornerEnd, 1, 0), BiLerp(minUV, maxUV, 1, 0));
        face.Add(cornerEnd, maxUV);
        face.Add(BiLerp(cornerStart, cornerEnd, 0, 1), BiLerp(minUV, maxUV, 0, 1));

        return face;
    }

    public static Face CreateQuad(Vector3 cornerStartDepth, Vector3 cornerEndDepth,
        Vector3 cornerEndHeight, Vector3 cornerStartHeight)
    {
        var face = new Face();

        face.Add(cornerStartDepth, Vector2.zero);
        face.Add(cornerEndDepth, new Vector2(1, 0));
        face.Add(cornerEndHeight, Vector2.one);
        face.Add(cornerStartHeight, new Vector2(0, 1));

        return face;
    }
}

public class MeshCreator
{
    private MeshTopology _meshTopology;

    private List<Face> _faces = new();
    public List<Face> Faces => _faces;

    private int NumVertices
    {
        get
        {
            var value = 0;

            _faces.ForEach(f => value += f.vertices.Count);

            return value;
        }
    }

    private Vector3[] _positions;
    private Vector2[] _uvs;
    private Vector3[] _normals;

    private int[] _indices;

    public MeshCreator(MeshTopology meshTopology)
    {
        _meshTopology = meshTopology;
    }

    public Mesh CreateMesh()
    {
        SetDataContainers();

        IterateVertices(
            v =>
            {
                _positions[v.meshIndex] = v.vertex.position;
                _uvs[v.meshIndex] = v.vertex.uv;
                _normals[v.meshIndex] = v.vertex.normal;

                _indices[v.meshIndex] = v.meshIndex;
            }
        );

        var mesh = new Mesh();

        mesh.vertices = _positions;
        mesh.uv = _uvs;
        mesh.normals = _normals;

        mesh.SetIndices(_indices, _meshTopology, 0);

        return mesh;
    }

    private void SetDataContainers()
    {
        var verticesLength = NumVertices;

        if (_positions == null || _positions.Length != verticesLength)
        {
            _positions = new Vector3[verticesLength];
            _uvs = new Vector2[verticesLength];
            _normals = new Vector3[verticesLength];

            _indices = new int[verticesLength];
        }
    }

    private void IterateVertices(System.Action<VertexData> vertexProcessor)
    {
        var meshIndex = 0;
        var vertexData = new VertexData();

        for (int i = 0; i < _faces.Count; i++)
        {
            var face = _faces[i];
            vertexData.face = face;

            var vertices = face.vertices;
            for (int j = 0; j < vertices.Count; j++)
            {
                vertexData.vertex = vertices[j];
                vertexData.faceIndex = j;
                vertexData.meshIndex = meshIndex;

                meshIndex++;

                vertexProcessor(vertexData);
            }
        }
    }
}