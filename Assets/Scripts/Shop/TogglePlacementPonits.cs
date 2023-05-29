
using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TogglePlacementPonits : MonoBehaviour
{   
   private Camera cam;
   private static ComponentPlacementPoint _currentNode;
   private static int _playerLayer;

    [SerializeField] private Material notSelectedNode;
    [SerializeField] private Material selectedNode;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material playerMaterialHidden;

    [SerializeField] private GameObject SelectionUI;
    [SerializeField] private GameObject RemoveItem;

    [SerializeField] private LayerMask checkLayers;
    

    private static Material PlayerMaterial;
    private GameObject target;
    
    private static bool changes;
    private TextMeshProUGUI _textMeshProUGUI;
    private int _nodeLayer;
    int state = -1;
    private int _UILayer;
    
    private static TogglePlacementPonits instance;

   private void OnEnable()
   {
      ManageNodes(true);
   }

   private void OnDisable()
   {
      ManageNodes(false);
      if (changes)
      {
         changes = false;
         Settings.instance.SaveGameInfo();
         Settings.instance.SaveShip();
      }

   }

   private void Start()
   {
      instance = this;
      _textMeshProUGUI = RemoveItem.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
      PlayerMaterial = playerMaterial;
      cam = Camera.main;
      _playerLayer =  LayerMask.NameToLayer("Player");
      _nodeLayer =  LayerMask.NameToLayer("PlacementNode");
      _UILayer =  LayerMask.NameToLayer("UI");
ResetUI();
     
   }
   

   private void Update()
   {
      //Inpur helps PC
      #if UNITY_EDITOR || UNITY_STANDALONE
      if (!Input.GetMouseButtonDown(0)) return;
      Debug.Log("Editor logged mouse button down");
      #endif
      if (Utilities.IsPointerOverUIObject()) return;
      if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, checkLayers))
      {
         CustomPool.Instance.Explode(hit.point);
         Transform t = hit.transform;
         print(t.gameObject.layer + " vs " + _playerLayer + " | " + _nodeLayer + " | " + _UILayer);
         if (state != 3 && t.gameObject.layer == _UILayer)
         {
            state = 3;
            return;
         }

         if (state != 0 && t.gameObject.layer == _nodeLayer)
         {
            state = 0;
            if (_currentNode) _currentNode.SetMat(notSelectedNode);
            _currentNode = t.GetComponent<ComponentPlacementPoint>();
            StoreItems.Instance.ValidateShop(_currentNode.PlaceableTypes);
            _currentNode.SetMat(selectedNode);
            RemoveItem.SetActive(false);
            SelectionUI.SetActive(true);
         }
         else if (state != 1 && t.gameObject.layer == _playerLayer)
         {
            state = 1;
            print("On player layer");
            Transform o = RemoveItem.transform.GetChild(0).GetChild(2);
            

            target = t.parent.gameObject;
            _textMeshProUGUI.text = target.name;
            
            Instantiate(target, o).GetComponent<MeshRenderer>().material = playerMaterialHidden;
            Destroy(o.GetChild(0).gameObject); // delete prv
            RemoveItem.SetActive(true);
            SelectionUI.SetActive(false);
         }
      }

      else if (state != 2)
      {
         ResetUI();
      }
   }

   private void ResetUI()
   {
      SelectionUI.SetActive(false);
      RemoveItem.SetActive(false);
      state = 2;
      if (!_currentNode) return;
      _currentNode.SetMat(notSelectedNode);
      _currentNode = null;
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
      changes = true;
      _currentNode.ToggleDisplay(false);
      instance.ResetUI();
   }

   public void DeleteSelectedItem()
   {
      state = -1;
      changes = true;
      target.transform.parent.GetComponent<ComponentPlacementPoint>().ToggleDisplay(true);
      Destroy(target);

      ResetUI();
   }
}
