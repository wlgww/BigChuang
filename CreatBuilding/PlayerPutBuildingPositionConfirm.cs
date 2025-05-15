using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class PlayerPutBuildingPositionConfirm 
{
    public string  RightPositionFileName {private  get; set; }
    private Dictionary<Vector3,BuildingData> RightBuildingData=new  ();
    public PlayerPutBuildingPositionConfirm(string rightPositionFileName)
    {
        RightPositionFileName = rightPositionFileName;
        List<BuildingData>StoredRightBuildingData = BuildingsInfoSaving.Load<BuildingData>(RightPositionFileName);
        if(StoredRightBuildingData.Count == 0)
        {
            throw new System.Exception("Can not find the File or the file is empty");
        }
        foreach (var item in StoredRightBuildingData)
        {
            RightBuildingData.Add(item.WorldPosition, item);
        }
    }
    public bool IfTheRightPlace(BuildingData PlayerBuildingData)
    {
        if (this.RightBuildingData.ContainsKey(PlayerBuildingData.WorldPosition) == false)
        {
            Debug.Log("Can not find");
            return false;
        }
        BuildingData TheRightBuildingData = RightBuildingData[PlayerBuildingData.WorldPosition];
        if (PlayerBuildingData.ID == TheRightBuildingData.ID&&PlayerBuildingData.WorldPosition == TheRightBuildingData.WorldPosition&&PlayerBuildingData.Rotate==TheRightBuildingData.Rotate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}   
