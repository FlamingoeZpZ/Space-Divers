using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerComponent : MonoBehaviour
{
    public static readonly StickerComponent [] Stickers= new StickerComponent[5];
    [field: SerializeField] public int MyIdx { get; private set; }

    public bool isSet;

    private void Awake()
    {
        if (MyIdx == -1) return; // If we have reached sticker . length. start pooling.
        isSet = false;
        Stickers[MyIdx] = this;
    }

    public void SetID(int id)
    {
        isSet = true;
        //Take advantage of static reference
        if(Stickers[id] != this)
            Destroy(Stickers[id].gameObject);
        
        MyIdx = id;
        Stickers[MyIdx] = this;
    }
    
    
}
