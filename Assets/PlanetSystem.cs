using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSystem : MonoBehaviour
{
    //Each child attache dis aplanet
    private readonly int _playerPosID = Shader.PropertyToID("_PlayerPos");

    [SerializeField] private Material temp;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp.SetVector(_playerPosID, ModularPlayerScript.Position);
        //Shader.SetGlobalVector(_playerPosID, ModularPlayerScript.Position);
    }
}
