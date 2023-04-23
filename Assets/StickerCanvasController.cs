using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StickerCanvasController : MonoBehaviour
{
    [SerializeField] private Image buttonTemp;
    [SerializeField] private Sprite[] sprites;

    private readonly int textureID = Shader.PropertyToID("_Texture");
    // Start is called before the first frame update
    private int playerLayer;
    private Material curStickerMat;
    private Transform objTrans;
    private DecalProjector dp;
    private Camera cam;
    private bool isActive;
    private GameObject root;
    void Start()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sprites.Length * 180, 300);
        foreach (Sprite s in sprites)
        {
            Image me = Instantiate(buttonTemp, transform);
            me.sprite = s;
            me.GetComponent<Button>().onClick.AddListener(() =>
            {
                print("sticker selected");
                isActive = true;
                curStickerMat.SetTexture(textureID, s.texture);
                
            });
        }

        root = transform.parent.parent.parent.gameObject;
        cam = Camera.main;
        playerLayer = (1 << LayerMask.NameToLayer("Player")) + (1<< LayerMask.NameToLayer("PlayerRoot"));
    }

    public void Enable(int num)
    {
        print($"Trying to access sticker {num} ");
            
        objTrans = StickerComponent.Stickers[num].transform;
        dp = objTrans.GetComponent<DecalProjector>();
        curStickerMat = StickerComponent.Stickers[num].GetComponent<DecalProjector>().material;
        isActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        //If not active. OR we're not hitting the ship return
        if (!isActive || !Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, playerLayer)) return;


        objTrans.position = hit.point;
        //objTrans.forward = -hit.normal;
        objTrans.LookAt(Camera.main.transform);
        //dp.size
        
        objTrans.parent = hit.transform;
        
        
        

        //If hold is released.
        if (Input.GetMouseButtonDown(0))
        {
            print("You let go: parented to:" + objTrans.parent);
            root.SetActive(false);
            
        }

    }
}
