using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatedbuildingManager : MonoBehaviour
{
    [SerializeField]
    SetABuildingSystem setABuilding;
    [SerializeField]
    ObjectDatabaseSO objectDatabase;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private MouseClickTimeJudge mouseClickTimeJudge;
    [SerializeField]
    private IfMouseOnTheUI ifMouseOnTheUI;

    private Dictionary<GameObject, BuildingData> PlacedBuildingsInfo=new Dictionary<GameObject, BuildingData>();
    private bool TheBuildingIsSetting = false;
    private int CreatingBuildingType = -1;
    private GameObject TheBuilding;
    private BuildingData TheBuildingData;
    private GameObject TheBuildingsFather;
    private const string BuildingTag = "Building";
    private List<BuildingData> StoredBuildingData;
    private void Start()
    {
        TheBuildingsFather = GameObject.Find("Buildings");
        LoadPlacedBuildingsInfo();
        mouseClickTimeJudge.onClick += EMouseOnClick;
    }
    
    private void OnDisable()
    {
        StoredBuildingData=PlacedBuildingsInfo.Values.ToList(); 
        BuildingsInfoSaving.Save(StoredBuildingData);
    }
    private void LoadPlacedBuildingsInfo()
    {
        StoredBuildingData=BuildingsInfoSaving.Load<BuildingData>();
        setABuilding.StopTheSystem(false);
        foreach (var item in StoredBuildingData)
        {
            if(item==null)
            {
                Debug.Assert(false, "TheBuildingData is null");
                continue;
            }
            
            if (item.ID >= objectDatabase.gameObjectDatas.Count || item.ID < 0)
            {
                throw new Exception("Array index out of bounds of the objectDatabase");
            }
            TheBuildingIsSetting = true;
            CreatingBuildingType = item.ID;
            TheBuilding = Instantiate(objectDatabase.gameObjectDatas[item.ID].Prefab, item.WorldPosition, Quaternion.Euler(0, 0, 0), TheBuildingsFather.transform);
            TheBuildingData = new BuildingData(item.ID, objectDatabase.gameObjectDatas[item.ID].Size,item.Rotate,item.WorldPosition,item.Index);
            Debug.Log("wb:"+TheBuildingData.WorldPosition);
            setABuilding.StartTheSystem(ref TheBuilding, TheBuildingData, false, false,true);
        }
        
    }
    private void EMouseOnClick()
    {
        if (!ifMouseOnTheUI.IsMouseOverUI())
        {
            CheckTheMouseClick();
        }
    }
    private void CheckTheMouseClick()
    {
        GameObject SelectedBuilding=inputManager.GetSelectedMapBuilding();
        if (SelectedBuilding == null)
        {
            setABuilding.StopTheSystem(false);
            Debug.Log("is null");
            return;
        }
        if (SelectedBuilding.transform.parent == null)
        {
            Debug.Log("do not have parent");
            return;
        }
        setABuilding.StopTheSystem(false);
        
        SelectedBuilding =SelectedBuilding.transform.parent.gameObject;
        tag =SelectedBuilding.tag;
        
        if(tag == BuildingTag)
        {
            TheBuilding=SelectedBuilding;
            ResetABuilding();
            Debug.Log("is a building");
        }
        else
        {
            setABuilding.StopTheSystem(false);
            Debug.Log("is not a building"+SelectedBuilding.tag);
        }
    }
    private void ResetABuilding()
    {
        
        if (TheBuilding==null)
        {
            throw new Exception("TheBuilding is null");
        }
        if (PlacedBuildingsInfo.ContainsKey(TheBuilding) == false)
        {
            throw new Exception("TheResetBuilding is not in the PlacedBuildingsInfo");
        }
        
        TheBuildingData = new BuildingData(PlacedBuildingsInfo[TheBuilding].ID, PlacedBuildingsInfo[TheBuilding].Size, PlacedBuildingsInfo[TheBuilding].Rotate, PlacedBuildingsInfo[TheBuilding].WorldPosition, PlacedBuildingsInfo.Count - 1);
        
        setABuilding.StartTheSystem(ref TheBuilding, TheBuildingData,false,false);
    }
    public void LongPressed(int BuildingType)
    {
        TryToCreatAbuilding(BuildingType,true);
    }
    public void ShortPressed(int BuildingType)
    {
        TryToCreatAbuilding(BuildingType, false);
    }

    private  void TryToCreatAbuilding(int BuildingType,bool IfLongPressed)
    {
        setABuilding.StopTheSystem(false);
        if (BuildingType >= objectDatabase.gameObjectDatas.Count || BuildingType < 0)
        {
            throw new Exception("Array index out of bounds of the objectDatabase");
        }
        TheBuildingIsSetting=true;
        CreatingBuildingType=BuildingType;
        TheBuilding =Instantiate(objectDatabase.gameObjectDatas[BuildingType].Prefab, new Vector3(Screen.width / 2, Screen.height / 2, 0),Quaternion.Euler(0, 0, 0), TheBuildingsFather.transform);
        TheBuildingData = new BuildingData(objectDatabase.gameObjectDatas[BuildingType].ID, objectDatabase.gameObjectDatas[BuildingType].Size, Vector3.zero,Vector3.zero,PlacedBuildingsInfo.Count-1);
        PlacedBuildingsInfo[TheBuilding]=TheBuildingData;
        setABuilding.StartTheSystem(ref TheBuilding,TheBuildingData, true,!IfLongPressed);
    }
   public void EUpdatePlacedBuildingsInfo(OverCreatEventArgs overCreatEventArgs)
    {
        if (TheBuilding==null)
        {
            throw new Exception("The building is null");
        }
        if (overCreatEventArgs.IfSuccess == false)
        {
            if (PlacedBuildingsInfo.ContainsKey(TheBuilding))
            {
                PlacedBuildingsInfo.Remove(TheBuilding);
            }
        }
        else
        {
            int index;
            if (PlacedBuildingsInfo.ContainsKey(TheBuilding))
            {
                index=PlacedBuildingsInfo[TheBuilding].Index;
            }
            else
            {
                index = PlacedBuildingsInfo.Count - 1;
            }
            PlacedBuildingsInfo.Remove(TheBuilding);
            
            if (TheBuilding == null)
            {
                Debug.Log("null");
            }
            Debug.Log(TheBuilding.tag);
            
            PlacedBuildingsInfo[TheBuilding] = overCreatEventArgs.buildingData;
        }
    }
}

[Serializable]
public class BuildingData
{
    // 改为公共字段（或保留属性但添加SerializeField）
    [SerializeField]
    public int ID;

    [SerializeField]
    public Vector2Int Size;

    [SerializeField]
    public Vector3 Rotate;

    [SerializeField]
    public Vector3 WorldPosition;

    [SerializeField]
    public int Index;



    // 添加无参构造器（必须）
    public BuildingData() { }

    // 原构造器保留
    public BuildingData(int id, Vector2Int size, Vector3 rotate, Vector3 worldPosition, int index)
    {
        ID = id;
        Size = size;
        WorldPosition = worldPosition;
        Index = index;
        Rotate = rotate;
    }
}

