using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceReminder : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicatorPrefab;
    [SerializeField]
    private GameObject cellIndicatorPrefab;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    private bool IfDrugPlace=false;
    private bool CanPlaceThere=true;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        SetABuildingSystem.EFinishPlacing += ShowInMiddleScreen;
        SetABuildingSystem.EPlacingTheBuilidng += ShowInMousePosition;
        SetABuildingSystem.ECanNotPlaceThere += ChangeCellIndicatorToRed;
        SetABuildingSystem.ECanPlaceThere += ChangeCellIndicatorToWhite;
        meshRenderer = cellIndicatorPrefab.GetComponentInChildren<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition;
        if (IfDrugPlace)
        {
            mousePosition = inputManager.GetSelectedMapPosition();
        }
        else
        {
            mousePosition = inputManager.GetSelectedMapPosition(true);
        }
        Vector3Int GridPosition = grid.WorldToCell(mousePosition);
        mouseIndicatorPrefab.transform.position = mousePosition;
        cellIndicatorPrefab.transform.position = grid.CellToWorld(GridPosition);
    }
    internal void ShowInMiddleScreen()
    {
        IfDrugPlace = false;
        cellIndicatorPrefab.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }
    internal void ShowInMousePosition()
    {
        IfDrugPlace = true;
    }
    internal void ChangeCellIndicatorToRed(Vector2Int GameObjectSize)
    {
        meshRenderer.material.color = Color.red;
        cellIndicatorPrefab.transform.localScale=new Vector3(GameObjectSize.x,1,GameObjectSize.y);
    }
    internal void ChangeCellIndicatorToWhite(Vector2Int GameObjectSize)
    {
        meshRenderer.material.color = Color.white;
        cellIndicatorPrefab.transform.localScale = new Vector3(GameObjectSize.x, 1, GameObjectSize.y);
    }
}
