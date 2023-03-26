
using UnityEngine;

public class TogglePlacementPonits : MonoBehaviour
{   
   private Camera cam;
   private static ComponentPlacementPoint _currentNode;
   private int nodeLayer;
   private static int _playerLayer;

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
      nodeLayer = 1 << LayerMask.NameToLayer("PlacementNode");
      _playerLayer = 1 << LayerMask.NameToLayer("Player");
   }

   private void Update()
   {
      if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100,  nodeLayer))
      {
         if(_currentNode)
            _currentNode.SetMat(notSelectedNode);
         _currentNode = hit.transform.GetComponent<ComponentPlacementPoint>();
         StoreItems.Instance.ValidateShop(_currentNode.PlaceableTypes);
         _currentNode.SetMat(selectedNode);
      }
   }
   
   private void ManageNodes(bool state)
   {
      ComponentPlacementPoint[] cx = UpgradeStationController.playerShip
         .GetComponentsInChildren<ComponentPlacementPoint>();
      foreach(ComponentPlacementPoint c in cx)
      {
         c.ToggleDisplay(state && c.transform.childCount == 0); 
      }  
   }

   public static void PlaceNode(GameObject o)
   {
      Transform g = Instantiate(o, _currentNode.transform).transform;
      g.GetComponent<MeshRenderer>().material = PlayerMaterial;
      g.localScale /= 50;
      g.gameObject.layer = _playerLayer;
      g.name = g.name.Substring(0, g.name.Length - 14);
      
      _currentNode.ToggleDisplay(false);
   }
}
