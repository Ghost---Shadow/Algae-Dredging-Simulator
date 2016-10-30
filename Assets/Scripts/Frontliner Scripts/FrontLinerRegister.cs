using UnityEngine;
using System.Collections;

public class FrontLinerRegister : MonoBehaviour
{
    public int sizeX = 10;
    public int sizeY = 10;

    public int[,] map;

    public static int UNEXPLORED = 0;
    public static int DIRTY = 1;
    public static int CLEAN = 2;
    public static int BLOCKED = 3;

    void Start()
    {
        reset();
    }

    public void reset()
    {
        map = new int[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
                map[i, j] = UNEXPLORED;

        map[0, 0] = map[sizeX - 1, 0] = map[sizeX - 1, sizeY - 1] = map[0, sizeY - 1] = BLOCKED;
    }

    public void requestWaypoints(FrontLinerScript bot)
    {
        Vector3[] wps = new Vector3[2];

        wps[0] = findClosestUnexploredTile(bot.transform.position);
        wps[1] = findLeastExploredDirection(wps[0]) * Mathf.Max(sizeX, sizeY);

        bot.setWaypoints(wps);
    }

    private Vector3 findClosestUnexploredTile(Vector3 pos)
    {
        float leastDistance = Mathf.Infinity;
        Vector3 closest = Vector3.zero;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (map[i, j] == UNEXPLORED)
                {
                    float distance = Vector3.Distance(intToVec(i, j), pos);
                    if (distance < leastDistance)
                    {
                        closest = intToVec(i, j);
                        leastDistance = distance;
                    }
                }
            }
        }
        return closest;
    }

    private Vector3 findLeastExploredDirection(Vector3 pos)
    {
        int FORWARD = 0, RIGHT = 1, BACK = 2, LEFT = 3;

        Vector3[] dirs = new Vector3[4];
        dirs[FORWARD] = Vector3.forward * Mathf.Abs(pos.z);
        dirs[RIGHT] = Vector3.right * Mathf.Abs(pos.x);
        dirs[BACK] = -Vector3.forward * Mathf.Abs(pos.z); 
        dirs[LEFT] = -Vector3.right * Mathf.Abs(pos.x); 

        int dirIndex = RIGHT;        
        int c = 0;

        int[] xy = vectorToInt(pos);

        // Forward
        for(int i = xy[0] + 1; i < sizeX; i++)
            if(map[i,xy[1]] == UNEXPLORED || map[i,xy[1]] == DIRTY)
                c++;   

        int max = c;
        //Debug.Log("Right: "+c);
        // Back
        c = 0;
        for(int i = 0; i < xy[0]; i++)
            if(map[i,xy[1]] == UNEXPLORED || map[i,xy[1]] == DIRTY)
                c++;
        
        if(c > max){
            max = c;
            dirIndex = LEFT;
        }
        //Debug.Log("Left: "+c);
        // Left
        c = 0;
        for(int i = 0; i < xy[1]; i++)
            if(map[xy[0],i] == UNEXPLORED || map[i,xy[1]] == DIRTY)
                c++;
        
        if(c > max){
            max = c;
            dirIndex = BACK;
        }
        //Debug.Log("Back: "+c);
        // Forward
        c = 0;
        for(int i = xy[1] + 1; i < sizeY; i++)
            if(map[xy[0],i] == UNEXPLORED || map[i,xy[1]] == DIRTY)
                c++; 

        if(c > max){
            max = c;
            dirIndex = FORWARD;
        }
        //Debug.Log("Forward: "+c);
        //Debug.Log(dirIndex);
        return dirs[dirIndex];
    }

    public int[] vectorToInt(Vector3 pos)
    {
        int[] p = new int[2];
        p[0] = ((int)pos.x + (sizeX - 1)) / 2;
        p[1] = ((int)pos.z + (sizeY - 1)) / 2;
        return p;
    }
    public Vector3 intToVec(int x, int y)
    {
        return new Vector3(2 * x - (sizeX - 1), 0.0f, 2 * y - (sizeY - 1));
    }

}

