using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IconSystem
{
    public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
    
        //The Icon is the object attached to an icon spot.
        //Each icon must contain a child.

        private Image primaryIcon;
        private Image secondaryIcon;
        private string infoText;
        private static readonly WaitForSeconds PauseDuration = new (0.1f);

        [SerializeField] private Transform infoPos;
        
        // Start is called before the first frame update
        void Awake()
        {
            primaryIcon = GetComponent<Image>();
            secondaryIcon = transform.GetChild(0).GetComponent<Image>();
        }

        public void SetIcon(string id, float value = 0.5f, float range = 1)
        {
            IconManager.SetIcon(id, primaryIcon, secondaryIcon);
            infoText = id;

            secondaryIcon.color = IconManager.SampleColorGradient(value / range);
        }

        public void SetTextOnly(string id)
        {
            infoText = id;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(ActionTimer());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            IconManager.Hide();
        }

        private IEnumerator ActionTimer()
        {
            yield return PauseDuration;
            IconManager.ShowInfo(infoPos, infoText);
        }
        
        

    }
}
