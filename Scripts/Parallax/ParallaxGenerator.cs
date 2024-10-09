using System.Collections.Generic;
using UnityEngine;

public class ParallaxGenerator : MonoBehaviour
{
    [SerializeField] private ParallaxBackground parallaxBackground;
    [SerializeField] private int maxElementPool;
    [SerializeField] private int chunkWidth = 10;
    [SerializeField] private int yOffset = 10;
    [SerializeField] private int _drawDistanceRight = 2;
    [SerializeField] private int _drawDistanceLeft = 1;
    
    private Queue<ParallaxBackground> _parallaxBackgrounds = new Queue<ParallaxBackground>();
    private Dictionary<int, ParallaxBackground> _parallaxSectors = new Dictionary<int, ParallaxBackground>();
    private Transform _playerTransform;
    private int _lastPlayerChunk = 0;

    public void Init(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        
        for (int i = 0; i < maxElementPool + 2; i++)
        {
            ParallaxBackground background = Instantiate(parallaxBackground, transform);
            _parallaxBackgrounds.Enqueue(background);
            background.Init(_playerTransform);
            background.gameObject.SetActive(false);
        }
    }
    
    private void FixedUpdate()
    {
        if (_playerTransform == null)
        {
            return;
        }

        transform.position =
            new Vector3(transform.position.x, _playerTransform.position.y + yOffset, transform.position.z);

        var playerX = _playerTransform.position.x;
        var playerChunk = Mathf.FloorToInt(playerX / chunkWidth);
        
        if (playerChunk != _lastPlayerChunk)
        {
            ReturnChunksOutsideDrawDistance();
        }

        for (int i = playerChunk - _drawDistanceLeft; i < playerChunk + _drawDistanceRight; i++)
        {
            if (IsChunkFilled(i))
            {
                continue;
            }

            var chunk = GetChunk(i);
            chunk.transform.localPosition = new Vector3(i * chunkWidth, 0, 0);
        }

        _lastPlayerChunk = playerChunk;
    }
    
    private bool IsChunkFilled(int chunkIndex)
    {
        return _parallaxSectors.ContainsKey(chunkIndex);
    }
    
    private ParallaxBackground GetChunk(int chunkIndex)
    {
        var chunk = _parallaxBackgrounds.Dequeue();
        _parallaxSectors[chunkIndex] = chunk;
        chunk.gameObject.SetActive(true);
        return chunk;
    }
    
    private void ReturnChunk(ParallaxBackground chunk)
    {
        chunk.gameObject.SetActive(false);
        _parallaxBackgrounds.Enqueue(chunk);
    }
    
    private void ReturnChunksOutsideDrawDistance()
    {
        var chunksToRemove = new List<int>();
        var playerChunk = Mathf.FloorToInt(_playerTransform.position.x / chunkWidth);
        
        foreach (var chunk in _parallaxSectors)
        {
            if (chunk.Key < playerChunk - _drawDistanceLeft || chunk.Key > playerChunk + _drawDistanceRight)
            {
                ReturnChunk(chunk.Value);
                chunksToRemove.Add(chunk.Key);
            }
        }
        
        foreach (var chunkIndex in chunksToRemove)
        {
            _parallaxSectors.Remove(chunkIndex);
        }
    }
}
