using System;
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
    private Image myImg;

    [ColorUsage(false, true)] private Color curCol;
    //[SerializeField] private int hdrGlow;
    [SerializeField] private Material mat;
    [SerializeField] private Material hiddenMat;

    [SerializeField]private string id;

    [SerializeField] private bool isEmissive;
    
    private int storedId;

    private readonly int intensityID = Shader.PropertyToID("_Intensity");
    

    private Transform colorTrans;
    private Vector2 pos;
    
    // Start is called before the first frame update
    void Start()
    {
        myImg = GetComponent<Image>();
        colorWheel = myImg.sprite.texture;
        //Multiply by the preset dimension / curDim;
        RectTransform r = GetComponent<RectTransform>();
        rt = r.sizeDelta * (new Vector2(Screen.width, Screen.height)/ new Vector2(2560, 1440));
        //pos = (Vector2)transform.position;// + r.anchoredPosition;
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
            if (isEmissive)
            {
                mat.SetFloat(intensityID, blackMult.value * 20 - 10);
                hiddenMat.SetFloat(intensityID, blackMult.value * 20 - 10);
            }
            else
            {
                myImg.color = new Color(1 * blackMult.value, 1 * blackMult.value, 1 * blackMult.value, 1);
                SetColor();
            }
        });


    }


    public void OnPointerMove(PointerEventData eventData)
    {

        if (Vector2.Distance(eventData.position, transform.position) > rt.x/2) return;
        Vector2 d = (eventData.position - (Vector2)transform.position + rt/2)/rt;
        
        curCol = colorWheel.GetPixelBilinear(d.x, d.y);
        colorTrans.position = eventData.position;
        SetColor();
        
        //MOVE THIS!
        //Settings.instance.SaveShip(0, );
    }
    

    private void SetColor()
    {
        Color c = curCol * (isEmissive?1:blackMult.value);
        colorWheelColor.color = c;
        mat.SetColor(storedId, c);
        hiddenMat.SetColor(storedId, c);
        hexText.text = ColorUtility.ToHtmlStringRGB(c);
        
        
        
    }
    
    
    
}
