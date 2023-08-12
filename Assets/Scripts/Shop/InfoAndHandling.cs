using System.Globalization;
using IconSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoAndHandling : MonoBehaviour
{

    private Transform itemShowcase;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemDescription;
    private GameObject curChild;
    private readonly TextMeshProUGUI[] displayTexts = new TextMeshProUGUI[5];
    private readonly Icon[] displayImages = new Icon[5];
    private TextMeshProUGUI removalText;
    private GameObject removalButton;
    private int UILayer;
    private GameObject refItem;

    
    // Start is called before the first frame update
    void Start()
    {
        Transform itemBg = transform.GetChild(0).GetChild(0);
        itemShowcase = itemBg.GetChild(1);
        itemName = itemBg.GetChild(3).GetComponent<TextMeshProUGUI>();
        UILayer = LayerMask.NameToLayer("UI");
        
        for (int i = 0; i < 5; ++i)
        {
            Transform t = transform.GetChild(i + 1);
            displayTexts[i] = t.GetChild(0).GetComponent<TextMeshProUGUI>();
            displayImages[i] = t.GetChild(1).GetComponent<Icon>();
        }

        itemDescription = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        Transform n = transform.GetChild(7);
        removalButton = n.gameObject;
        removalText = n.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void SetItem(GameObject rt, ShipComponent item, bool markForRemoval)
    {
        gameObject.SetActive(true);
        if(curChild)
            Destroy(curChild);
        removalButton.SetActive(markForRemoval);
        if (markForRemoval)
        { 
            refItem = rt;
            Debug.LogWarning("Add the ability for items to know their total price.");
            removalText.text = $"Press to sell {3} items for {100}$ ?";
        }

        curChild = Instantiate(rt, itemShowcase).gameObject;
        curChild.layer = UILayer;
        curChild.transform.localScale = new Vector3(30, 30, 30);
        foreach (MeshRenderer unlucky in curChild.GetComponentsInChildren<MeshRenderer>())
        {
            unlucky.gameObject.layer = UILayer;
        }
        
        itemName.text = item.name;
        displayTexts[0].text = item.Displays[0];
        displayImages[0].SetTextOnly("Weight");
        for (int i = 1; i < 4; ++i)
        {
            displayTexts[i].text = item.Displays[i];
            displayImages[i].SetIcon(item.DisplayWords[i]);
        }
        displayTexts[4].text = item.CurrencyCost.ToString(CultureInfo.InvariantCulture);
        displayImages[4].SetTextOnly("Currency");

        itemDescription.text = item.Description;
    }

    public void RemoveItem()
    {
        //Give currency
        
        //Destroy object 
        Destroy(refItem);
        //Turn ourself off
        gameObject.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
