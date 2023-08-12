using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IconSystem
{
    /// <summary>
    /// Loads all the images in the Resources/Icons folder and allows for them to be called from code by name.
    /// </summary>
    public static class IconManager
    {
        private static readonly Dictionary<string, Sprite> Sprites = new();
        private static readonly Gradient ColorGradient = new();
        private static GameObject helpPopup;
        private static Transform rt;
        private static TextMeshProUGUI helpPopupText;
        public static void Build()
        {
            if (Sprites.Count != 0) return;
            ColorGradient.colorKeys = new[]
            {
                new GradientColorKey(Color.red, 0),
                new GradientColorKey(Color.white, 0.5f),
                new GradientColorKey(Color.green, 1),
            };
            ColorGradient.mode = GradientMode.Blend;
            helpPopup = Object.Instantiate(Resources.Load<GameObject>("Icons/popup"));
            rt = helpPopup.transform;
            helpPopupText = rt.GetChild(0).GetComponent<TextMeshProUGUI>();
            Object.DontDestroyOnLoad(helpPopup);
            helpPopup.SetActive(false);
            
            Sprite []tempArr = Resources.LoadAll<Sprite>("Icons");
            
#if UNITY_EDITOR
           Debug.Log($"Loaded {tempArr.Length} icons");
#endif
            
           foreach (Sprite sprite in tempArr)
           {
               string[] s = sprite.name.Split("_");
               foreach (string name in s)
               {
                   //Doing this would waste a bunch of memory by duplicating images.
                   //But it's also much easier and I can't think of a better way.
                   string n = name.ToUpperInvariant();
                   Sprites.Add(n, sprite);
               }
           }
        }


        // -------- For the icon itself ----------- 
        public static void Hide()
        {
            helpPopup.SetActive(false);
        }

        public static void ShowInfo(Transform owner, string info) //Is 
        {
            helpPopup.SetActive(true);
            helpPopupText.text = info;
            //Set it's screen position based on 



            rt.SetParent(owner, false);

        }

        public static void SetIcon(string id, Image primaryIcon, Image secondaryIcon)
        {
            string[] data = id.ToUpperInvariant().Split(" ");

            primaryIcon.sprite = Sprites[data[0]];
            bool x = data.Length == 1;
            secondaryIcon.enabled = !x;
            if (x) return;
            secondaryIcon.sprite = Sprites[data[1]];
            
        }

        public static Color SampleColorGradient(float value)
        {
            return ColorGradient.Evaluate(value);
        }
    }
}
