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
        {
            for (int j = 0; j < sizeY; j++)
            {
                map[i, j] = UNEXPLORED;
            }
        }
    }

    public void requestWaypoints(FrontLinerScript bot)
    {
        Vector3[] wps = new Vector3[3];
        wps[0] = new Vector3(8, 0, -8);
        wps[1] = new Vector3(8, 0, 8);
        wps[2] = new Vector3(-8, 0, 8);

        bot.setWaypoints(wps);
    }
}

