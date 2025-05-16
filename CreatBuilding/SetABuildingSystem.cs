using System;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class SetABuildingSystem : MonoBehaviour
{
    
    private bool IfFirstCreat;
    private GameObject TheGround;
    private Vector3 PutWorldPosition = Vector3.zero;
    private GameObject TheSetBuilding;//�������õĽ���
    private BuildingData TheBuildingData;//�������õĽ�������Ϣ
    private Vector3 OriginalPosition;
    private GameObject OriginalTheBuildingPerfab;
    
    private GridPlacedGameObjectData gridPlacedGameObjectData;//grid�з�������Ĵ洢
    private BackToOriginalMaterial.MaterialBackup[] backups;
    [Header("���ܶ���")]
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private InputManager inputManagerForBuilding;
    [SerializeField]
    private IfMouseOnTheUI ifMouseOnTheUI;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private CreatedbuildingManager createdbuildingManager;

    private PlayerPutBuildingPositionConfirm playerPutBuildingPositionConfirm;

    [Header("UI����")]
    [SerializeField]
    private GameObject SelectBuildingUI;
    [SerializeField]
    private GameObject SetBuildingUI;

    [Header("͸������")]
    [SerializeField]
    private Material PreviewMaterial;
    private Material PreviewMaterialInstance;
    [Header("��ؾ���")]
    [SerializeField]
    private float PreviewPrefabUpset = 0.06f;

    private bool IfSystemIsWorking = false;
    private bool IsMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        TheGround = GameObject.Find("Buildings");
        gridPlacedGameObjectData = new GridPlacedGameObjectData();
        PreviewMaterialInstance = new(PreviewMaterial);
        SetBuildingUI.SetActive(false);
        EFinishToSet+= createdbuildingManager.EUpdatePlacedBuildingsInfo;
        playerPutBuildingPositionConfirm = new("RightBuildingPosition.json");
    }
    void Update()
    {
        if (IfSystemIsWorking&&Input.GetMouseButtonDown(0)&&ifMouseOnTheUI.IsMouseOverUI()==false)
        {
            Debug.Log("StartToMoveTheBuilding!!!");
            GameObject SelectedBuilding = inputManagerForBuilding.GetSelectedMapBuilding();
            if(SelectedBuilding == null||SelectedBuilding.transform.parent == null)
            {
                return;
            }
            SelectedBuilding=SelectedBuilding.transform.parent.gameObject;
            if (SelectedBuilding == TheSetBuilding)
            {
                StartToMoveTheBuilding();
            }
        }
        if (IsMoving)
        {
            if (Input.GetMouseButtonUp(0))
            {
                StopToMoveTheBuilding();
            }
            else
            {
                MoveTheBuilding();
            }
        }
    }
    private void OnDisable()
    {
        StopTheSystem(false);
    }
    public void StartTheSystem(ref GameObject TheBuilding,BuildingData buildingData,bool IsTheFirstToCreat,bool IfPutInMiddle,bool LoadMode=false)
    {
        if (IfSystemIsWorking==true)
        {
            throw new Exception("SetABuildingSystem failure to quit last time");
        }
        IfSystemIsWorking=true;
        TheBuildingData = buildingData;
        TheSetBuilding = TheBuilding;
       
        IfFirstCreat = IsTheFirstToCreat;
        OriginalPosition = TheSetBuilding.transform.position;
        if (LoadMode)
        {
            RotateTheBuilding((int)TheBuildingData.Rotate.y);
            PutWorldPosition=TheSetBuilding.transform.position;
            StopTheSystem(false,false);
            return;
        }

        SetBuildingUI.SetActive(true);

        if (IsTheFirstToCreat)
        {
            JustWorldPosition(IfPutInMiddle);
            TheSetBuilding.transform.position = PutWorldPosition + new Vector3(0, PreviewPrefabUpset, 0);
            OriginalPosition = TheSetBuilding.transform.position;
        }
        else
        {
            gridPlacedGameObjectData.RemoveObjectAt(grid.WorldToCell(OriginalPosition));
        }

        StoreTheOriginalPrefab();
        StartToPreview();
        ChangePreviewObjectColor(OriginalPosition);

        if (IfFirstCreat&&IfPutInMiddle==false)
        {
            StartToMoveTheBuilding();
        }
         
    }
    //private void StoreTheOriginalPrefab()
    //{
    //    OriginalTheBuildingPerfab = Instantiate(TheSetBuilding,TheGround.transform);
    //    OriginalTheBuildingPerfab.SetActive(false);
    //}
   
    

    // ��������Renderer�Ĳ���
    public void StoreTheOriginalPrefab()
    {
        backups = BackToOriginalMaterial.BackupMaterials(new GameObject[] {TheSetBuilding});
    }

    //private void UseTheOriginalPrefab()
    //{
    //    Destroy(TheSetBuilding);
    //    TheSetBuilding= Instantiate(OriginalTheBuildingPerfab,TheGround.transform);
    //    TheSetBuilding.SetActive(true);
    //    Destroy(OriginalTheBuildingPerfab);
    //}
    public void UseTheOriginalPrefab()
    {
        if (backups == null)
        {
            return;
        }
        BackToOriginalMaterial.RestoreMaterials(backups);
    }

    public event Action <OverCreatEventArgs> EFinishToSet;//�˳�����ϵͳ�¼�
    
    public void StopTheSystem(bool IfBackToOriginalPosition,bool IfToDestroy=false)
    {
        if (IfSystemIsWorking == false)
        {
            return;
        }
        IfSystemIsWorking =false;
        UseTheOriginalPrefab();
        StopToMoveTheBuilding();
        if (SetBuildingUI != null)
        {
            SetBuildingUI.SetActive(false);
        }
        TheSetBuilding.tag = "Building";
       
        if (IfToDestroy)//����û�Ҫ�ݻ�����
        {
            if (!IfFirstCreat)
            {
                gridPlacedGameObjectData.RemoveObjectAt(grid.WorldToCell(OriginalPosition));
            }
            Destroy(TheSetBuilding);
            EFinishToSet?.Invoke(new OverCreatEventArgs(false, TheBuildingData));
            return;
        }

        gridPlacedGameObjectData.RemoveObjectAt(grid.WorldToCell(OriginalPosition));//�ȴӸ���ϵͳ���Ƴ�

        if (IfBackToOriginalPosition)
        {
            BackToOriginalPosition();
        }
        else
        {
            if (ToPutGameObjectOnGrid(PutWorldPosition))
            {
                if (TheSetBuilding==null )
                {
                     throw new Exception("TheSetBuilding is NULL");
                }
                EFinishToSet?.Invoke(new OverCreatEventArgs(true, TheBuildingData));
                Debug.Log("ws:" + TheSetBuilding.transform.position);
            }
            else
            {
                Debug.Log("wawa");
                BackToOriginalPosition();
            }
         }
        Debug.Log("SetBuildingSystem is quit");
        
    }
    private void BackToOriginalPosition()
    {
        if (IfFirstCreat)
        {
            Destroy(TheSetBuilding);
           
            EFinishToSet?.Invoke(new OverCreatEventArgs(false, TheBuildingData));
        }
        else
        {
            TheSetBuilding.transform.position = OriginalPosition;
            if (ToPutGameObjectOnGrid(TheSetBuilding.transform.position))
            {
                EFinishToSet?.Invoke(new OverCreatEventArgs(true, TheBuildingData));
            }
            else
            {
                Destroy(TheSetBuilding);
                EFinishToSet?.Invoke(new OverCreatEventArgs(false, TheBuildingData));
             }
            
        }
    }
    private bool ToPutGameObjectOnGrid(Vector3 PutPosition)
    {

        if (gridPlacedGameObjectData.CanPlacedObjectAt(TheSetBuilding) == false)
        {
            return false;
        }
        else
        {
            if (TheSetBuilding == null)
            {
                throw new Exception("TheSetBuilding is NULL");
            }
            TheSetBuilding.transform.position = PutPosition;
            TheBuildingData.WorldPosition = TheSetBuilding.transform.position;
            gridPlacedGameObjectData.AddObjectAt(grid.WorldToCell(PutPosition), TheBuildingData.Size, TheBuildingData.ID);
            return true;
        }
    }
    public void RotateTheBuilding(int WantToRotateDu=90)
    {
        if (TheSetBuilding == null)
        {
            throw new Exception("TheSetBuilding is NULL");
        }
        while (WantToRotateDu < 0)
        {
            WantToRotateDu += 360;
        }
        while (WantToRotateDu >= 360)
        {
            WantToRotateDu -= 360;
        }
        int RotateTime = WantToRotateDu / 90;
        for (int i = 0; i < RotateTime; i++)
        {
            DoTheRotate(TheSetBuilding.transform, TheBuildingData.Size);
        }
     }
    private  void DoTheRotate(Transform parent, Vector2 childSize)
    {
        // ��ȡ��һ��������
        Transform child = parent.GetChild(0);
        child.RotateAround(parent.position, Vector3.up,90);
        child.Translate(0, 0, childSize.x * grid.cellSize.x, Space.World);
        TheBuildingData.Size = new Vector2Int(TheBuildingData.Size.y, TheBuildingData.Size.x);
        TheBuildingData.Rotate=child.rotation.eulerAngles;
        ChangePreviewObjectColor(PutWorldPosition);
    }
    public static event Action EPlacingTheBuilidng;
    public static event Action EFinishPlacing;
    public static event Action<Vector2Int> ECanNotPlaceThere;
    public static event Action<Vector2Int> ECanPlaceThere;

    private void StartToMoveTheBuilding()
    {
        CameraRotate.SetCameraRotate(false);
        EPlacingTheBuilidng?.Invoke();
        SelectBuildingUI.SetActive(false);
        SetBuildingUI.SetActive(false);
        IsMoving = true;
    }
    private void StopToMoveTheBuilding()
    {
        CameraRotate.SetCameraRotate(true);
        EFinishPlacing?.Invoke();
        IsMoving = false;
        if(SelectBuildingUI != null)
        {
            SelectBuildingUI.SetActive(true);
        }
        if (SetBuildingUI != null)
        {
            SetBuildingUI.SetActive(true);
        }
        
    }
    private void MoveTheBuilding(bool IfWantToMove=true)
    {
        JustWorldPosition(!IfWantToMove);
        CameraRotate.SetCameraRotate(false);
        TheSetBuilding.transform.position = PutWorldPosition + new Vector3(0, PreviewPrefabUpset, 0);
        ChangePreviewObjectColor(PutWorldPosition);
    }
    private void ChangePreviewObjectColor(Vector3 Position)
    {
        TheBuildingData.WorldPosition= Position;
        Color color = new Color();
        
        if (gridPlacedGameObjectData.CanPlacedObjectAt(TheSetBuilding) == false)
        {
            ECanNotPlaceThere.Invoke(TheBuildingData.Size);
            color = Color.red;
        }
        else if(playerPutBuildingPositionConfirm.IfTheRightPlace(TheBuildingData) == false)
        {
            ECanNotPlaceThere.Invoke(TheBuildingData.Size);
            color = Color.yellow;
        }
        else
        {
            ECanPlaceThere.Invoke(TheBuildingData.Size);
            color = Color.white;
        }
        color.a = 0.5f;
        PreviewMaterialInstance.color = color;
    }
    private void JustWorldPosition(bool IfPutInMiddle = false)
    {
        Vector3 MousePosition = inputManager.GetSelectedMapPosition(IfPutInMiddle);
        Vector3Int GridPosition = grid.WorldToCell(MousePosition);
        PutWorldPosition = grid.CellToWorld(GridPosition);
        PutWorldPosition = MousePosition;
    }
    
    //private void StartToPreview()
    //{
    //    Renderer[] renderers = TheSetBuilding.GetComponentsInChildren<Renderer>();
    //    foreach (Renderer renderer in renderers)
    //    {
    //        Material[] materials = renderer.materials;
    //        for (int i = 0; i < materials.Length; i++)
    //        {
    //            materials[i] = PreviewMaterialInstance;
    //        }
    //        renderer.materials = materials;
    //    }
    //}
    // ����1��ֱ���滻�������飨�ʺϾ�̬�޸ģ�
    public void StartToPreview( )
    {
        BackToOriginalMaterial.ApplyTempMaterials(new GameObject[] {TheSetBuilding}, PreviewMaterialInstance);
    }

    


    public void ERemoveTheBuilding()//�û��ݻٽ������¼�������
    {
        StopTheSystem(false,true);
    }
 }
public class OverCreatEventArgs : EventArgs
{
    public OverCreatEventArgs(bool ifSuccess,BuildingData buildingData)
    {
        IfSuccess = ifSuccess;
        this.buildingData = buildingData;
        
        
    }

    public bool IfSuccess { get; private set; }
   
    public BuildingData buildingData { get; private set; }= new BuildingData();
    
}