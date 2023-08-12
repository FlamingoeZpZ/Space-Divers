
using System.Collections;
using Cinemachine;
using Managers;
using TMPro;
using UnityEngine;

public class TogglePlacementPonits : MonoBehaviour
{

   private Camera cam;
   private Transform _currentNode;
   private Transform _toPlace;
   private int _playerLayer;

    [SerializeField] private Material playerMaterial;

    [SerializeField] private InfoAndHandling selectedObject;
    [SerializeField ]private Transform componentCanvas;
    private Material _playerMaterial;
    private GameObject target;

    [SerializeField]
    private PreventGUIMovement freeLook;
    private Transform curSelectedToRemove;
    
    private static bool _changes;
    private int nodeLayer;
    private static bool _isPlacing;
    

   private void OnEnable()
   {
      ManageNodes(true);
   }

   private void OnDisable()
   {
      ManageNodes(false);
      if (_changes)
      {
         _changes = false;
         Settings.instance.SaveGameInfo();
         Settings.instance.SaveShip();
      }

   }

   private void Start()
   {
      //_textMeshProUGUI = RemoveItem.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
      _playerMaterial = playerMaterial;
      cam = Camera.main;
      _playerLayer = 1<< LayerMask.NameToLayer("Player");
      nodeLayer =  1<<LayerMask.NameToLayer("PlacementNode");
   }
   

   private void Update()
   {

      //Don't allow any of this to go on if there's UI blocking us anyways...
      if (Utilities.IsPointerOverUIObject()) return;
      
      #if UNITY_EDITOR || UNITY_STANDALONE
      if (Input.GetMouseButtonDown(0))
         StartCoroutine(_isPlacing ? PlaceLoop() : RemoveLoop());
      #elif UNITY_ANDROID || UNITY_IOS
      if (Input.GetTouch(0).phase == TouchPhase.Began)
         StartCoroutine(_isPlacing ? PlaceLoop() : RemoveLoop());
      #endif
   }

   private IEnumerator RemoveLoop()
   {
      #if UNITY_EDITOR || UNITY_STANDALONE
      while (!Input.GetMouseButtonUp(0))
      #elif UNITY_ANDROID || UNITY_IOS
      while (Input.GetTouch(0).phase != TouchPhase.Ended)
      #endif
      {

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100,  _playerLayer))
            {
               if (curSelectedToRemove != hit.transform.parent)
               {
                  
                  curSelectedToRemove = hit.transform.parent.parent;
                  print("Selected: " + curSelectedToRemove.name);
                  selectedObject.SetItem(curSelectedToRemove.gameObject, curSelectedToRemove.GetComponent<ShipComponent>(), true);
               }
            }
            else if (curSelectedToRemove)
            {
               curSelectedToRemove = null;
               selectedObject.Disable();
            }
            yield return null;
      }
      _changes = true;
   }
   
   private IEnumerator PlaceLoop()
   {
      print("SS");
      freeLook.enabled = false;
      bool inPlace = false;
#if UNITY_EDITOR || UNITY_STANDALONE
      while (!Input.GetMouseButtonUp(0))
#elif UNITY_ANDROID || UNITY_IOS
      while (Input.GetTouch(0).phase != TouchPhase.Ended)
#endif
      {
         
         Ray r = cam.ScreenPointToRay(Input.mousePosition);
         inPlace = Physics.Raycast(r, out RaycastHit hit, 100, nodeLayer);
         if (inPlace)
         {
            
            _currentNode = hit.transform;
            _toPlace.parent = _currentNode;
            _toPlace.localPosition = Vector3.zero;
            _toPlace.localRotation = Quaternion.identity;
            _toPlace.localScale = Vector3.one;
            _isPlacing = false;
         }
         else
         {
            _toPlace.position = r.origin + r.direction * 15;
         }
         yield return null;
      }

      if (inPlace)
      {
         _changes = true;
        
         
         foreach (ComponentPlacementPoint cpp in _toPlace.GetComponentsInChildren<ComponentPlacementPoint>())
         {
            cpp.ForceActivate();
         }
         _toPlace = null; //Detach self...
         _currentNode.GetComponent<ComponentPlacementPoint>().ToggleDisplay(false);
      }
      else
      {
         Destroy(_toPlace.gameObject);
         _isPlacing = false;
      }

      freeLook.enabled = true;
   }

   private void ResetUI()
   {
      if (!_currentNode) return;
      _currentNode.GetComponent<ComponentPlacementPoint>().ToggleDisplay(false);
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


   public void PlaceNode(GameObject o)
   {
      Transform g = Instantiate(o, _currentNode).transform;
      g.GetComponent<MeshRenderer>().material = _playerMaterial;
      g.localScale /= 50;
      g.gameObject.layer = _playerLayer;
      
      _changes = true;
      //_currentNode.ToggleDisplay(false);
   }

   public void SetCursorObject(GameObject prefab)
   {
      if(_toPlace) Destroy(_toPlace.gameObject);
      _toPlace = Instantiate(prefab, componentCanvas).transform;
      _toPlace.localRotation = Quaternion.identity;
      _toPlace.localScale = Vector3.one;
      _toPlace.name = _toPlace.name.Substring(0, _toPlace.name.Length - 14);
      SetGameLayerRecursive(_toPlace, 3);
      foreach (MeshRenderer mr in _toPlace.GetComponentsInChildren<MeshRenderer>())
      {
         if(1<<mr.gameObject.layer == _playerLayer)
            mr.material = _playerMaterial;
      }
      _isPlacing = true;
   }
   
   private void SetGameLayerRecursive(Transform go, int layer)
   {
      go.gameObject.layer = layer;
      foreach (Transform child in go)
      {

         if (child.TryGetComponent(out ComponentPlacementPoint cpp))
         {
            child.gameObject.layer = LayerMask.NameToLayer("PlacementNode");
            continue;
         }
         
         child.gameObject.layer = layer;
 
         Transform hasChildren = child.GetComponentInChildren<Transform>();
         if (hasChildren != null)
            SetGameLayerRecursive(child, layer);
      }
   }

   public void DeleteSelectedItem()
   {
      _changes = true;
      target.transform.parent.GetComponent<ComponentPlacementPoint>().ToggleDisplay(true);
      Destroy(target);

      ResetUI();
   }
}
