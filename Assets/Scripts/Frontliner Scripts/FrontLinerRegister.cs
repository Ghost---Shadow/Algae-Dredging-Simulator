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
        //Debug.Log(wps[0] + " " + bot.transform.position);
        wps[1] = findLeastExploredDirection(bot.transform.position) * Mathf.Max(sizeX, sizeY);

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

    // TODO
    private Vector3 findLeastExploredDirection(Vector3 pos)
    {
        //int[] xy = vectorToInt(pos);
        int dir1 = Random.Range(0, 1);
        int sign = Random.Range(0, 1) > 0 ? -1 : 1;
        if (dir1 > 0)
            return sign * Vector3.forward * pos.z;
        else
            return sign * Vector3.right * pos.x;
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

