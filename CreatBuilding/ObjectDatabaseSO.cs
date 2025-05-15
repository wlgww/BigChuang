using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<GameObjectData> gameObjectDatas;
}
[Serializable]
public class GameObjectData
{
    [field: SerializeField]
    public string  Name { get; private  set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}