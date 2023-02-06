using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    
    public static GameManager instance { get; private set; }
    [field: SerializeField]   public Transform bulletParent { get; private set; }
    [SerializeField] private GameObject TEMP_WIN_OBJECT;
    [SerializeField] private GameObject TEMP_LOSS_OBJECT;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        }

        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void TEMP_END_GAME(bool win)
    {
        Instantiate(win?TEMP_WIN_OBJECT:TEMP_LOSS_OBJECT);
    }
}
