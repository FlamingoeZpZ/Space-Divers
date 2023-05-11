using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StickerCanvasController : MonoBehaviour
{
    [SerializeField] private Image buttonTemp;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private ColorWheelController colorWheel;
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Image[] border;
     

    private readonly int textureID = Shader.PropertyToID("_Texture");
    private Transform[] texts = new Transform[5];
    private Image[] images = new Image[5];
    // Start is called before the first frame update
    private int playerLayer;
    private Material curStickerMat;
    private Transform objTrans;
    private DecalProjector dp;
    private Camera cam;
    private bool isActive;
    private int prv = -1;
    void Start()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sprites.Length * 50, 300);
        Vector2 anc = new Vector2(0.5f, 0.5f);
        for (int i = 0; i < 5; ++i)
        {
            images[i] = border[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
            texts[i] = border[i].transform.GetChild(0).GetChild(1);

            if (!StickerComponent.Stickers[i].isSet) continue;
            Texture2D t = (Texture2D)Settings.instance.stickerMats[i].GetTexture(Settings.TextureID);
            Rect r = new Rect(0, 0, t.width, t.height);
            images[i].sprite = Sprite.Create(t, r,anc,100 );
            images[i].color = Settings.instance.stickerMats[i].GetColor(Settings.ShipColA);
            images[i].gameObject.SetActive(true);
            texts[i].gameObject.SetActive(false);
        }
        
        foreach (Sprite s in sprites)
        {
            Image me = Instantiate(buttonTemp, transform);
            me.sprite = s;
            me.GetComponent<Button>().onClick.AddListener(() =>
            {
                curStickerMat.SetTexture(textureID, s.texture);
                images[prv].sprite = s;
                images[prv].gameObject.SetActive(true);
                texts[prv].gameObject.SetActive(false);
            });
        }

        


        cam = Camera.main;
        playerLayer = (1 << LayerMask.NameToLayer("Player")) + (1<< LayerMask.NameToLayer("PlayerRoot"));
        
        widthSlider.onValueChanged.AddListener((newVal) =>
        {
            Vector3 x = dp.size;
            x.x = newVal;
            dp.size = x;
        });
        heightSlider.onValueChanged.AddListener((newVal) =>
        {
            Vector3 x = dp.size;
            x.y = newVal;
            dp.size = x;
        });

        colorWheel.OnValueChanged += (col) => images[prv].color = col;
        
        Enable(0);
    }

    public void Enable(int num)
    {
        print($"Trying to access sticker {num} ");
            
        objTrans = StickerComponent.Stickers[num].transform;
        dp = objTrans.GetComponent<DecalProjector>();
        curStickerMat = StickerComponent.Stickers[num].GetComponent<DecalProjector>().material;
        colorWheel.SetMaterial(Settings.instance.stickerMats[num]);
        Vector2 x = dp.size;
        widthSlider.SetValueWithoutNotify(x.x);
        heightSlider.SetValueWithoutNotify(x.y);
        colorWheel.ResetUI();
        isActive = false;
        border[num].color = Color.yellow;
        if(prv != -1)
            border[prv].color = Color.white;
        prv = num;
    }

    public void Move()
    {
        isActive = true;
    }

    public void Delete()
    {
        isActive = false;
        objTrans.position = Vector3.up * 100;
        StickerComponent.Stickers[prv].isSet = false;
        images[prv].gameObject.SetActive(false);
        texts[prv].gameObject.SetActive(true);
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
            StickerComponent.Stickers[prv].isSet = true;
            isActive = false;
        }

    }
}
