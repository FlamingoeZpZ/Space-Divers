using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSystem : MonoBehaviour
{
    //Each child attache dis aplanet
    private readonly int _playerPosID = Shader.PropertyToID("_PlayerPos");
    

    private static readonly Vector3[] PlanetPositions = 
    {
        new(1000,1000,1000), // SUN
        new(-436.83f, -1625.51f, 173.80f), // Homeworld
        new(79.23f,-3932.82f,-692.45f), // Nucron
        new(2566.32f,170.2f,-189.7f) // Akelllin
    };

    public static Dictionary<int, Vector3> PlanetDirs; // This feels like a violation of law.

    // Start is called before the first frame update
    void Awake()
    {
        int WorldID = SceneManager.GetActiveScene().buildIndex;
        PlanetDirs = new Dictionary<int, Vector3>();
        //Disable currentPlanet
        //Place all planets in correct position
        for (int i = 0; i < PlanetPositions.Length; ++ i)
        {
            if (i == 0)
            {
                transform.GetChild(0).LookAt(Vector3.zero);
                continue;
            }
            //print(Random.insideUnitSphere * Random.Range(2000,5000));
            Vector3 v = PlanetPositions[i] - PlanetPositions[WorldID];
            float mag = v.magnitude;
            if (mag > ModularPlayerScript.maxTravelDist)
            {
                print("planet is too far: " + i);
                transform.GetChild(i).gameObject.SetActive(false);
                continue;   
            }
            transform.GetChild(i).gameObject.SetActive(true);

            transform.GetChild(i).localPosition = v;

            
            
            PlanetDirs.Add(i, v / mag); // Normalized Vector.
        }
        transform.GetChild(WorldID).gameObject.SetActive(false);

        

    }

    // Update is called once per frame
    void Update()
    {
        
        Shader.SetGlobalVector(_playerPosID, ModularPlayerScript.Position);
        
    }
}
