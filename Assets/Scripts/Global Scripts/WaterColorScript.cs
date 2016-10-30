using UnityEngine;
using System.Collections;

public class WaterColorScript : MonoBehaviour
{
    public Material[] materials;
    public int x,y;

    private bool isExplored;
    private FrontLinerRegister registry;

    void Start()
    {
        //GetComponent<Renderer>().material = unexplored;
        registry = GameObject.Find("FrontlinerContainer").GetComponent<FrontLinerRegister>();
        isExplored = false;
        x = ((int)transform.position.x + (registry.sizeX-1))/2;
        y = ((int)transform.position.z + (registry.sizeY-1))/2;
    }

    void Update(){
        GetComponent<Renderer>().material = materials[registry.map[x,y]];
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("FRONTLINER_TAG"))
            isExplored = true;
        if (isExplored)
            if (other.gameObject.tag.Equals("ALGAE_TAG"))
                registry.map[x,y] = FrontLinerRegister.DIRTY;
    }

    void OnTriggerExit(Collider other)
    {
        if (isExplored && other.gameObject.tag.Equals("ALGAE_TAG"))
            registry.map[x,y] = FrontLinerRegister.CLEAN;
    }
}
