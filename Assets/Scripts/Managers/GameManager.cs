using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private List<MeshRenderer> mr = new();
    private List<Material> saved = new();
    private bool toggled;
    [SerializeField] private LayerMask removeLayer;
    private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mr = FindObjectsByType<MeshRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            saved.Clear();
            foreach (MeshRenderer m in mr)
            {
                saved.Add(m.material);
            }
            
        }

        
        if (Input.GetKeyDown(KeyCode.K))
        {


            for (int i = 0; i < mr.Count; ++i)
            {
                if (((1 << mr[i].gameObject.layer) & removeLayer) == 0)
                {
                    continue;
                }

                mr[i].material = toggled ? saved[i] : null;
            }



            toggled = !toggled;
        }
    }
}
