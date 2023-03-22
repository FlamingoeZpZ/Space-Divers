using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlacementPonits : MonoBehaviour
{   
   private Camera cam;
   private static ComponentPlacementPoint currentNode;
   private int NodeLayer;
   private static int PlayerPlayer;

   [SerializeField] private Material notSelectedNode;
    [SerializeField] private Material selectedNode;
    [SerializeField] private Material playerMaterial;

    private static Material PlayerMaterial;
   private void OnEnable()
   {
      ManageNodes(true);
   }

   private void OnDisable()
   {
      ManageNodes(false);
   }

   private void Start()
   {
      PlayerMaterial = playerMaterial;
      cam = Camera.main;
      NodeLayer = 1 << LayerMask.NameToLayer("PlacementNode");
      PlayerPlayer = 1 << LayerMask.NameToLayer("Player");
   }

   private void Update()
   {
      if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100,  NodeLayer))
      {
         if(currentNode)
            currentNode.SetMat(notSelectedNode);
         currentNode = hit.transform.GetComponent<ComponentPlacementPoint>();
         StoreItems.Instance.ValidateShop(currentNode.PlaceableTypes);
         currentNode.SetMat(selectedNode);
      }
   }
   
   private void ManageNodes(bool state)
   {
      if(UpgradeStationController.playerShip == null) return;
      
      ComponentPlacementPoint[] cx = UpgradeStationController.playerShip
         .GetComponentsInChildren<ComponentPlacementPoint>();
      foreach(ComponentPlacementPoint c in cx)
      {
         print("Toggling: " + c.gameObject.name + " to: " + state);
         c.ToggleDisplay(state); 
      }  
   }

   public static void PlaceNode(GameObject o)
   {
      Transform g = Instantiate(o, currentNode.transform).transform;
      g.GetComponent<MeshRenderer>().material = PlayerMaterial;
      g.localScale /= 50;
      g.gameObject.layer = PlayerPlayer;
   }
}
