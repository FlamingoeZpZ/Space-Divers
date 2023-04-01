using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerComponent : MonoBehaviour
{

    public static readonly StickerComponent [] Stickers= new StickerComponent[5];

    private static int _curidx;

    public int MyIdx { get; private set; }

    private void Awake()
    {
        if (_curidx == Stickers.Length)
            _curidx = 0;
        MyIdx = _curidx;
        if(Stickers[_curidx])
            Destroy(Stickers[_curidx]);
        Stickers[_curidx++] = this;
        print($"Set a new sticker with id: {_curidx} on object: {transform.root}");
    }

    private void OnDestroy()
    {
        print("Sticker was deleted: " + MyIdx);
    }
}
