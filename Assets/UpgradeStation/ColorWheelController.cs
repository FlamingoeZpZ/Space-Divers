using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

public class ColorWheelController : MonoBehaviour, IPointerMoveHandler
{
    
    [SerializeField] private Image colorWheelColor;
    private Vector2 originalScale;
    private Texture2D colorWheel;
    [SerializeField] private TMP_InputField hexText;
    [SerializeField] private Slider blackMult;
    private Vector2 rt;
    private Camera cam;
    private Image myImg;

    [ColorUsage(false, true)] private Color curCol;
    //[SerializeField] private int hdrGlow;
    [SerializeField] private Material mat;

    [SerializeField]private string id;
    private int storedId;
    

    private Transform colorTrans;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        myImg = GetComponent<Image>();
        colorWheel = myImg.sprite.texture;
        //Multiply by the preset dimension / curDim;
        rt = GetComponent<RectTransform>().sizeDelta * (new Vector2(Screen.width, Screen.height)/ new Vector2(2560, 1440));
        colorTrans = colorWheelColor.transform.parent;
        // posOffset.x += rt.sizeDelta.x;
        storedId = Shader.PropertyToID(id);
        
        hexText.onSubmit.AddListener(text =>
        {
           ColorUtility.TryParseHtmlString("#" + text, out curCol);
           SetColor();
        });
        
        blackMult.onValueChanged.AddListener(_ =>
        {
            myImg.color = new Color(1 * blackMult.value,1* blackMult.value,1* blackMult.value,1); 
            SetColor();
        });
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (Vector2.Distance(eventData.position, transform.position) > rt.x/2-16) return;
        Vector2 d = (eventData.position - (Vector2)transform.position + rt/2)/rt;
        
        curCol = colorWheel.GetPixelBilinear(d.x, d.y);
        colorTrans.position = eventData.position;
        SetColor();
    }
    

    private void SetColor()
    {
        Color c = curCol * blackMult.value;

        colorWheelColor.color = c;
        mat.SetColor(storedId, c);
        hexText.text = ColorUtility.ToHtmlStringRGB(c);
    }

}
