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
    private int curSticker;
    private Camera cam;
    private bool isActive;
    void Start()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sprites.Length * 180, 300);
        foreach (Sprite s in sprites)
        {
            Image me = Instantiate(buttonTemp, transform);
            me.sprite = s;
            me.GetComponent<Button>().onClick.AddListener(() =>
            {
                isActive = true;
                
            });
        }
        cam = Camera.main;
        playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    public void Enable(int num)
    {
        curSticker = num;
        isActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isActive || !Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, playerLayer)) return;
        
        
        

        if (!Input.GetMouseButtonDown(0))
        {
            
        }

    }
}
