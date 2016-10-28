using UnityEngine;
using System.Collections;

public class AlgaeSpawner : MonoBehaviour
{
	public bool active = true;

    private AlgaeRegister register;
    private GameObject prefab;

    private GameObject[] children;
    private int xCount, yCount;
    private float spacing;
	private float algaeSize;

    void Start()
    {
        register = transform.parent.GetComponent<AlgaeRegister>();
        prefab = register.prefab;
		if(active)
        	spawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
			purge();
			if(active)
            	spawn();
		}
    }

	void purge(){
		for(int i = 0; i < children.Length; i++){
			Destroy(children[i]);
		}
	}

    void spawn()
    {
        xCount = register.xCount;
        yCount = register.yCount;
        spacing = register.spacing;
		algaeSize = register.algaeSize;

        children = new GameObject[xCount * yCount];

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {				
                Vector3 position = new Vector3(i * spacing, 0.0f, j * spacing);
				position += transform.position - new Vector3(transform.lossyScale.x/2,0,transform.lossyScale.y/2);
                GameObject temp = (GameObject)Instantiate(prefab, position, Quaternion.identity);
                temp.transform.parent = transform;
				temp.transform.localScale = new Vector3(algaeSize,algaeSize,algaeSize);
                children[i * yCount + j] = temp;
            }
        }
    }
}
