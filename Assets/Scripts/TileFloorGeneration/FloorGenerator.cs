﻿using VContainer.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileFloorGeneration
{
    public class FloorGenerator : ITickable, IStartable
    {
        private const int TilesPerFrame = 20;
        private const int Offset = 2;
        
        private readonly Tile _tile;
        private readonly Camera _mainCamera;
        private readonly Vector2 _tileSize;
        
        private Queue<Vector3Int> _positionsToFill;
        private HashSet<Vector3Int> _filledPositions;
        private Tilemap _tilemap;
        private Vector3Int _centerTilePosition;
        
        public FloorGenerator(Tile tile, Camera camera)
        {
            _tile = tile;
            _mainCamera = camera;
            _tileSize = tile.sprite.bounds.size;
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
            
            CreateTilemap();
            FillPositionsFromSet(_positionsToFill.Count);
            AddPositionsBeyondCamera(Vector2Int.zero, Offset);
        }

        public void Tick()
        {
            var direction = CheckCenterTilePositionChanged();
            if (direction != Vector2Int.zero)
            {
                AddPositionsBeyondCamera(direction, Offset);
            }
            FillPositionsFromSet(TilesPerFrame);
        }

        private void CreateTilemap()
        {
            var gridObject = new GameObject("FloorGrid");
            var grid = gridObject.AddComponent<Grid>();
            grid.cellSize = _tileSize;
            
            var tilemapObject = new GameObject("FloorTilemap");
            tilemapObject.transform.SetParent(gridObject.transform);
            _tilemap = tilemapObject.AddComponent<Tilemap>();
            var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapRenderer.sortingLayerName = "Floor";
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
            var cameraSize = new Vector2(cameraWidth, cameraHeight);
            
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
                for (var y = startY; y <= endY; y++)
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
                for (var y = startY; y <= endY; y++)
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
                for (var x = startX; x <= endX; x++)
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
                for (var x = startX; x <= endX; x++)
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
    }
}