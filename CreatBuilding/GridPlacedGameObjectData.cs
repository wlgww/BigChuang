using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GridPlacedGameObjectData
{
    private Dictionary<Vector3Int, ThePlacedGameObjectData> AllPlacedGridData = new();
    
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        ThePlacedGameObjectData data = new ThePlacedGameObjectData(ID, positionToOccupy,objectSize);
        foreach (var position in positionToOccupy)
        {
            if (AllPlacedGridData.ContainsKey(position))
            {
                throw new Exception($"Dictionary already contains this cell position {position}");
            }
            AllPlacedGridData[position] = data;
        }
    }
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }
    public bool CanPlacedObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var position in positionToOccupy)
        {
            if (AllPlacedGridData.ContainsKey(position))
            {
                return false;
            }
        }
        return true;
    }
    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        if (AllPlacedGridData.ContainsKey(gridPosition)==false)
        {
            return;
        }
        foreach (var pos in AllPlacedGridData[gridPosition].OccupiedPositions)
        {
                AllPlacedGridData.Remove(pos);
        }
    }
    private class ThePlacedGameObjectData
    {
        public int ID { get; private set; }
        public List<Vector3Int> OccupiedPositions { get; private set; }
        public Vector2Int Size { get; private set; }
        internal ThePlacedGameObjectData(int iD, List<Vector3Int> occupiedPositions, Vector2Int size)
        {
            ID = iD;
            OccupiedPositions = occupiedPositions;
            Size = size;
        }
        
    }
}


