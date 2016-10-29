using UnityEngine;
using System.Collections;

public class WaterColorScript : MonoBehaviour
{
    public Material unexplored, dirty, clean;
    private bool isExplored;

    void Start()
    {
        GetComponent<Renderer>().material = unexplored;
        isExplored = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("FRONTLINER_TAG"))
            isExplored = true;
        if (isExplored)
            if (other.gameObject.tag.Equals("ALGAE_TAG"))
                GetComponent<Renderer>().material = dirty;
    }

    void OnTriggerExit(Collider other)
    {
        if (isExplored && other.gameObject.tag.Equals("ALGAE_TAG"))
            GetComponent<Renderer>().material = clean;
    }
}
