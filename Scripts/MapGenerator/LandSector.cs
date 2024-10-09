using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter), typeof(EdgeCollider2D))]
public class LandSector : MonoBehaviour
{
    [SerializeField] private float _width = 10f;
    [SerializeField] private float _bottomY = -10f;
    [SerializeField] private float _depth = 5f;
    [SerializeField] private float _topLayerHeight = 1f;
    [SerializeField] private int _topSubdivisions = 4;
    [SerializeField] private float _topSpherezation = 0.5f;
    [FormerlySerializedAs("sectorItemsGenerator")]
    [Space] 
    [SerializeField] private MoneyGenerator moneyGenerator = new MoneyGenerator();
    [SerializeField] private FuelGenerator fuelGenerator = new FuelGenerator();

    private MeshFilter _meshFilter;
    private EdgeCollider2D _edgeCollider;
    private Mesh _mesh;

    private int _verticesCount;

    private List<int> _topTrianglesList = new List<int>();
    private List<int> _bottomTrianglesList = new List<int>();
    private List<Vector3> _verticesList = new List<Vector3>();
    private List<Vector2> _uvsList = new List<Vector2>();
    
    private int _verticesPerSegment;
    private Flag _flag;

    public void Init(PoolMoney poolMoney, PoolFuel poolFuel, PhysicsMaterial2D physicsMaterial2D, float depth)
    {
        _meshFilter = GetComponent<MeshFilter>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
        moneyGenerator.Init(poolMoney);
        fuelGenerator.Init(poolFuel);
        _edgeCollider.sharedMaterial = physicsMaterial2D;
        _depth = depth;
    }

    public void ClearItems()
    {
        moneyGenerator.ClearItems();
        fuelGenerator.ClearItem();
        
        if(_flag != null)
            Destroy(_flag.gameObject);
    }

    private void CleanGarbage()
    {
        
        if (_mesh != null)
        {
            DestroyImmediate(_mesh);
        }
        
        if (_meshFilter.sharedMesh != null)
        {
            DestroyImmediate(_meshFilter.sharedMesh);
        }
        
        if (_edgeCollider.points.Length > 0)
        {
            _edgeCollider.points = new Vector2[0];
        }
    }

    public void GenerateMesh(float[] heights, float step = 1, TypeItems typeItems = TypeItems.None, Flag flag = null)
    {
        CleanGarbage();

        _mesh = new Mesh();
        _mesh.subMeshCount = 2;

        _verticesPerSegment = 3 + _topSubdivisions;
        _verticesCount = heights.Length * _verticesPerSegment; // 2 for top, one for top of the side and one for the bottom of the side

        _verticesList = new List<Vector3>(_verticesCount);
        _uvsList = new List<Vector2>(_verticesCount);
        _topTrianglesList = new List<int>((heights.Length) * _topSubdivisions * 6 + 6 * heights.Length); // one face per subdiv, and on face for side
        _bottomTrianglesList = new List<int>((heights.Length) * 6);

        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < heights.Length; i++)
        {
            float x = i * step;
            float y = heights[i];
            
            for (int j = 0; j < _topSubdivisions; j++)
            {
                var t = (float) j / _topSubdivisions;
                var z = _depth * (1 - t);
                var yAddition = (-(2 * t - 1) * (2 * t - 1) + 1) * _topSpherezation;
                
                var uvY = 0f;
                if (_topSubdivisions % 2 == 0)
                {
                    uvY = j % 2 == 0 ? 0.5f : 1f;
                }
                else
                {
                    uvY = j % 2 == 0 ? 1f : 0.5f;
                }
                AddVertex(new Vector3(x, y + yAddition, z),
                    new Vector2(x / _width, uvY));
            }
            
            // Top front
            AddVertex(new Vector3(x, y, 0),
             new Vector2(x/_width, 0.5f));

            if (i < heights.Length - 1)
            {
                for (int j = 0; j < _topSubdivisions; j++)
                {
                    AddFace(ref _topTrianglesList, i, j);
                }

                AddFace(ref _topTrianglesList, i, _topSubdivisions);
                AddFace(ref _bottomTrianglesList, i, _topSubdivisions + 1);
            }

            // Side top
            AddVertex(new Vector3(x, y - _topLayerHeight, -_topSpherezation * 0.5f),
             new Vector2(x / _width, 0f));
            
            AddVertex(new Vector3(x, _bottomY, -_topSpherezation * 0.5f),
            new Vector2(x / _width, _bottomY));
            
            positions.Add(new Vector2(x, y));
        }

        switch (typeItems)
        {
            case TypeItems.Money:
                moneyGenerator.Generate(positions, transform);
                break;
            
            case TypeItems.Fuel:
                int numPos = positions.Count / 2;
                fuelGenerator.Generate(positions[numPos], transform);
                break;
        }

        if (flag != null)
        {
            int numPos = positions.Count / 2;
            flag.transform.position = transform.TransformPoint(new Vector3(positions[numPos].x, positions[numPos].y, 1));
            flag.Activate();
            _flag = flag;
        }

        _mesh.vertices = _verticesList.ToArray();
        _mesh.uv = _uvsList.ToArray();
        _mesh.SetTriangles(_topTrianglesList.ToArray(), 0);
        _mesh.SetTriangles(_bottomTrianglesList.ToArray(), 1);
        
        _mesh.RecalculateNormals();
        
        _meshFilter.mesh = _mesh;
        
        var points = new List<Vector2>(heights.Length);
        
        for (int i = 0; i < heights.Length; i++)
        {
            points.Add(new Vector2(i * step, heights[i]));
        }
        
        _edgeCollider.SetPoints(points);
    }

    private void AddFace(ref List<int> list, int segmentIndex, int subdivIndex)
    {
        var index = segmentIndex * _verticesPerSegment + subdivIndex;
        var unwrap = _verticesPerSegment;
        list.Add(index);
        list.Add(index + unwrap);
        list.Add(index + 1);
        
        list.Add(index + unwrap);
        list.Add(index + unwrap + 1);
        list.Add(index + 1);
    }

    private void AddVertex(Vector3 position, Vector2 uv)
    {
        _verticesList.Add(position);
        _uvsList.Add(uv);
    }
    
    
}