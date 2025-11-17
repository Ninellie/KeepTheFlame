using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer.Unity;

namespace TileFloorGeneration
{
    public class FloorGenerator : ITickable, IStartable
    {
        private const int TilesPerFrame = 20;
        private const int Offset = 2;
        private const int RemovalOffset = 3;
        
        private readonly Tile _tile;
        private readonly Camera _mainCamera;
        private readonly Vector2 _tileSize;
        
        private Queue<Vector3Int> _positionsToFill;
        private HashSet<Vector3Int> _filledPositions;
        private Vector3Int _centerTilePosition;
        
        private readonly Tilemap _tilemap;
        
        public FloorGenerator(Tile tile, Tilemap tilemap)
        {
            _tile = tile;
            _tileSize = tilemap.layoutGrid.cellSize;
            _tilemap = tilemap;
            _mainCamera = Camera.main;
        }

        public void Start()
        {
            var positions = TilePositionCalculator.GetTilePositionsInCameraBounds(_tile, _mainCamera, Offset);
            _positionsToFill = new Queue<Vector3Int>();
            _filledPositions = new HashSet<Vector3Int>();
            
            // Добавляем начальные позиции
            foreach (var position in positions)
            {
                if (_filledPositions.Add(position))
                {
                    _positionsToFill.Enqueue(position);
                }
            }
            
            FillPositionsFromSet(_positionsToFill.Count);
            AddPositionsBeyondCamera(Vector2Int.zero, Offset);
        }

        public void Tick()
        {
            var direction = CheckCenterTilePositionChanged();
            if (direction != Vector2Int.zero)
            {
                AddPositionsBeyondCamera(direction, Offset);
                RemoveTilesBeyondCamera(RemovalOffset);
            }
            FillPositionsFromSet(TilesPerFrame);
        }

        private void FillPositionsFromSet(int count)
        {
            var filledCount = 0;
            
            while (_positionsToFill.Count > 0 && filledCount < count)
            {
                var position = _positionsToFill.Dequeue();
                _tilemap.SetTile(position, _tile);
                filledCount++;
            }
        }

        private void UpdateCenterTilePosition()
        {
            var cameraPosition = _mainCamera.transform.position;
            var tileSizeX = _tileSize.x;
            var tileSizeY = _tileSize.y;
            
            var centerX = Mathf.RoundToInt(cameraPosition.x / tileSizeX);
            var centerY = Mathf.RoundToInt(cameraPosition.y / tileSizeY);
            
            _centerTilePosition = new Vector3Int(centerX, centerY, 0);
        }

        private Vector2Int CheckCenterTilePositionChanged()
        {
            var previousPosition = _centerTilePosition;
            UpdateCenterTilePosition();
            var deltaX = _centerTilePosition.x - previousPosition.x;
            var deltaY = _centerTilePosition.y - previousPosition.y;
            
            return new Vector2Int(deltaX, deltaY);
        }

        private void AddPositionsBeyondCamera(Vector2Int direction, int offset)
        {
            var cameraHeight = 2f * _mainCamera.orthographicSize;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            
            var cameraPosition = _mainCamera.transform.position;
            
            // Вычисляем точные границы камеры в мировых координатах
            var cameraLeft = cameraPosition.x - cameraWidth / 2;
            var cameraRight = cameraPosition.x + cameraWidth / 2;
            var cameraBottom = cameraPosition.y - cameraHeight / 2;
            var cameraTop = cameraPosition.y + cameraHeight / 2;
            
            // Конвертируем в координаты тайлов, включая частично видимые
            var startX = Mathf.FloorToInt(cameraLeft / _tileSize.x);
            var endX = Mathf.CeilToInt(cameraRight / _tileSize.x);
            var startY = Mathf.FloorToInt(cameraBottom / _tileSize.y);
            var endY = Mathf.CeilToInt(cameraTop / _tileSize.y);
            
            // Добавляем тайлы справа при движении вправо
            if (direction.x > 0)
            {
                for (var y = startY - 1; y <= endY + 1; y++)
                {
                    for (var x = endX + 1; x <= endX + offset; x++)
                    {
                        var pos = new Vector3Int(x, y, 0);
                        if (_filledPositions.Add(pos))
                        {
                            _positionsToFill.Enqueue(pos);
                        }
                    }
                }
            }
            
            // Добавляем тайлы слева при движении влево
            if (direction.x < 0)
            {
                for (var y = startY - 1; y <= endY + 1; y++)
                {
                    for (var x = startX - offset; x <= startX - 1; x++)
                    {
                        var pos = new Vector3Int(x, y, 0);
                        if (_filledPositions.Add(pos))
                        {
                            _positionsToFill.Enqueue(pos);
                        }
                    }
                }
            }
            
            // Добавляем тайлы сверху при движении вверх
            if (direction.y > 0)
            {
                for (var x = startX - 1; x <= endX + 1; x++)
                {
                    for (var y = endY + 1; y <= endY + offset; y++)
                    {
                        var pos = new Vector3Int(x, y, 0);
                        if (_filledPositions.Add(pos))
                        {
                            _positionsToFill.Enqueue(pos);
                        }
                    }
                }
            }
            
            // Добавляем тайлы снизу при движении вниз
            if (direction.y < 0)
            {
                for (var x = startX - 1; x <= endX + 1; x++)
                {
                    for (var y = startY - offset; y <= startY - 1; y++)
                    {
                        var pos = new Vector3Int(x, y, 0);
                        if (_filledPositions.Add(pos))
                        {
                            _positionsToFill.Enqueue(pos);
                        }
                    }
                }
            }
        }

        private void RemoveTilesBeyondCamera(int removalOffset)
        {
            var cameraHeight = 2f * _mainCamera.orthographicSize;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            var cameraPosition = _mainCamera.transform.position;
            
            var cameraLeft = cameraPosition.x - cameraWidth / 2;
            var cameraRight = cameraPosition.x + cameraWidth / 2;
            var cameraBottom = cameraPosition.y - cameraHeight / 2;
            var cameraTop = cameraPosition.y + cameraHeight / 2;
            
            var startX = Mathf.FloorToInt(cameraLeft / _tileSize.x) - removalOffset;
            var endX = Mathf.CeilToInt(cameraRight / _tileSize.x) + removalOffset;
            var startY = Mathf.FloorToInt(cameraBottom / _tileSize.y) - removalOffset;
            var endY = Mathf.CeilToInt(cameraTop / _tileSize.y) + removalOffset;
            
            var bounds = _tilemap.cellBounds;
            var positionsToRemove = new List<Vector3Int>();
            
            foreach (var position in bounds.allPositionsWithin)
            {
                if (position.x < startX || position.x > endX || position.y < startY || position.y > endY)
                {
                    if (_tilemap.GetTile(position) != null)
                    {
                        positionsToRemove.Add(position);
                    }
                }
            }
            
            foreach (var position in positionsToRemove)
            {
                _tilemap.SetTile(position, null);
                _filledPositions.Remove(position);
            }
        }
    }
}