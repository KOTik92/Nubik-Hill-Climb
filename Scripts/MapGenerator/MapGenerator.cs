using System.Collections.Generic;
using Game.Car.AI;
using Sdk.RemoteConfig;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapData mapData;
    [SerializeField] private int _seed = 0;
    [SerializeField] private int _chunkWidth = 10;
    [SerializeField] private int _drawDistanceRight = 10;
    [SerializeField] private int _drawDistanceLeft = 2;
    [SerializeField] private int _drawDistanceRightAI = 10;
    [SerializeField] private int _drawDistanceLeftAI = 2;
    [SerializeField] private LandSector _chunkPrefab;
    [Space]
    [SerializeField] private PerlinOctave[] _octaves;
    [SerializeField] private float _chunkSectorStep = 0.2f;
    [Space]
    [SerializeField] private AnimationCurve _amplificationCurve;
    [SerializeField] private AnimationCurve _frequencyMultiplierCurve;
    [Space] 
    [SerializeField] private ItemsGenerator itemsGenerator = new ItemsGenerator();
    [SerializeField] private PoolMoney poolMoney;
    [SerializeField] private PoolFuel poolFuel;
    [SerializeField] private FlagManager flagManager;

    public MapData MapData => mapData;
    
    private Queue<LandSector> _landSectorsPool = new Queue<LandSector>();
    private Dictionary<int, LandSector> _landSectors = new Dictionary<int, LandSector>();
    private Transform _playerTransform;
    private int _amountChunk;
    private int _lastChunk;
    private PhysicsMaterial2D _physicsMaterial2D;

    private CarAI _carAI;

    public void Init(Transform playerTransform, CarAI carAI)
    {
        _playerTransform = playerTransform;
        Random.InitState(_seed);
        itemsGenerator.Init();
        flagManager.Init();

        _carAI = carAI;

        for (int i = 0; i < (_drawDistanceRight + _drawDistanceLeft) * 2; i++)
        {
            var chunk = Instantiate(_chunkPrefab, transform);
            _landSectorsPool.Enqueue(chunk);
            chunk.Init(poolMoney, poolFuel, _physicsMaterial2D, FlagController.ChunkDepth);
            chunk.gameObject.SetActive(false);
        }
    }

    public void SetPhysicsMaterial(PhysicsMaterial2D physicsMaterial2D)
    {
        _physicsMaterial2D = physicsMaterial2D;
    }
    
    /// <summary>
    /// Use this method to spawn something on the map
    /// Or to generate mesh
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public float GetY(float x)
    {
        float y = 0;
            
        foreach (var octave in _octaves)
        {
            var amplitude = _amplificationCurve.Evaluate(x) * octave.Amplitude;
            var frequency = _frequencyMultiplierCurve.Evaluate(x) * octave.Frequency;
            var offset = octave.Offset;
                
            y += amplitude * PerlinNoise.GetPerlin1D(x * frequency + offset);
        }
    
        return y;
    }
    
    private void Update()
    {
        if (_playerTransform == null)
        {
            return;
        }

        var playerX = _playerTransform.position.x;
        var playerChunk = Mathf.FloorToInt(playerX / _chunkWidth);
        
        ReturnChunksOutsideDrawDistance();
        /*
        if (_carAI != null)
        {
            var aiChunk = Mathf.FloorToInt(_carAI.transform.position.x / _chunkWidth);

            if (aiChunk <= playerChunk - _drawDistanceLeft)
            {
                _carAI.TeleportationCar(new Vector3(_landSectors[(playerChunk - _drawDistanceLeft) + 1].GetPositionCarOnChunk().x,
                    _landSectors[(playerChunk - _drawDistanceLeft) + 1].GetPositionCarOnChunk().y, _carAI.transform.position.z));
            }
            else if (aiChunk >= playerChunk + _drawDistanceRight)
            {
                _carAI.TeleportationCar(new Vector3(_landSectors[(playerChunk + _drawDistanceRight) - 1].GetPositionCarOnChunk().x,
                    _landSectors[(playerChunk + _drawDistanceRight) - 1].GetPositionCarOnChunk().y, _carAI.transform.position.z));
            }
        }
        */

        for (int i = playerChunk - _drawDistanceLeft; i < playerChunk + _drawDistanceRight; i++)
        {
            if (IsChunkFilled(i))
            {
                continue;
            }

            var chunk = GetChunk();
            chunk.transform.localPosition = new Vector3(i * _chunkWidth, 0, 0);

            if (_lastChunk <= i)
            {
                FillChunk(chunk, i, true);
                _lastChunk = i;
            }
            else if (_lastChunk > i)
                FillChunk(chunk, i, false);
        }

        if (_carAI != null)
        {
            var aiChunk = Mathf.FloorToInt(_carAI.transform.position.x / _chunkWidth);
        
            for (int i = aiChunk - _drawDistanceLeftAI; i < aiChunk + _drawDistanceRightAI; i++)
            {
                if (IsChunkFilled(i))
                {
                    continue;
                }

                var chunk = GetChunk();
                chunk.transform.localPosition = new Vector3(i * _chunkWidth, 0, 0);

                if (_lastChunk <= i)
                {
                    FillChunk(chunk, i, true);
                    _lastChunk = i;
                }
                else if (_lastChunk > i)
                    FillChunk(chunk, i, false);
            }
        }
    }
    
    private bool IsChunkFilled(int chunkIndex)
    {
        return _landSectors.ContainsKey(chunkIndex);
    }
    
    private LandSector GetChunk()
    {
        var chunk = _landSectorsPool.Dequeue();
        chunk.gameObject.SetActive(true);
        return chunk;
    }
    
    private void ReturnChunk(LandSector chunk)
    {
        chunk.ClearItems();
        chunk.gameObject.SetActive(false);
        _landSectorsPool.Enqueue(chunk);
    }
    
    private void ReturnChunksOutsideDrawDistance()
    {
        var chunksToRemove = new List<int>();
        var playerChunk = Mathf.FloorToInt(_playerTransform.position.x / _chunkWidth);
        
        foreach (var chunk in _landSectors)
        {
            if (chunk.Key <= playerChunk - _drawDistanceLeft || chunk.Key >= playerChunk + _drawDistanceRight)
            {
                if (_carAI != null)
                {
                    var aiChunk = Mathf.FloorToInt(_carAI.transform.position.x / _chunkWidth);
                    if (chunk.Key <= aiChunk - _drawDistanceLeftAI || chunk.Key >= aiChunk + _drawDistanceRightAI)
                    {
                        ReturnChunk(chunk.Value);
                        chunksToRemove.Add(chunk.Key);
                    }
                }
                else
                {
                    ReturnChunk(chunk.Value);
                    chunksToRemove.Add(chunk.Key);
                }
            }
        }
        
        foreach (var chunkIndex in chunksToRemove)
        {
            _landSectors.Remove(chunkIndex);
        }
    }
    
    private void FillChunk(LandSector chunk, int chunkIndex, bool isItemGenerate)
    {
        _landSectors[chunkIndex] = chunk;

        // +1 to include the last vertex, that connects with the next chunk
        var heightsCount = (int)(_chunkWidth / _chunkSectorStep) + 2;
        var heights = new float[heightsCount];

        for (int i = 0; i < heights.Length; i++)
        {
            var x = chunkIndex * _chunkWidth + i * _chunkSectorStep;
            var y = GetY(x);
            heights[i] = y;
        }

        if (isItemGenerate)
        {
            _amountChunk++;
            chunk.GenerateMesh(heights, _chunkSectorStep, itemsGenerator.CheckChunk(_amountChunk), flagManager.CheckChunk(_amountChunk) ? flagManager.GetFlag() : null);
        }
        else
            chunk.GenerateMesh(heights, _chunkSectorStep);
    }
}