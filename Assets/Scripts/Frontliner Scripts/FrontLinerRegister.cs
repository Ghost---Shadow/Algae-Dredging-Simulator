using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrontLinerRegister : MonoBehaviour
{
    public int sizeX = 10;
    public int sizeY = 10;

    public int[,] map;

    public static int UNEXPLORED = 0;
    public static int DIRTY = 1;
    public static int CLEAN = 2;
    public static int BLOCKED = 2;

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
        List<Vector3> wps = new List<Vector3>();

        wps.AddRange(findPathToClosestUnexploredTile(bot.transform.position));
        if (wps.Count > 0)
            wps.Add(findLeastExploredDirection(wps[wps.Count - 1]) * Mathf.Max(sizeX, sizeY));
        else
            wps.Add(bot.transform.position);

        bot.setWaypoints(wps.ToArray());
    }

    private Vector3[] findPathToClosestUnexploredTile(Vector3 pos)
    {
        float leastDistance = Mathf.Infinity;
        Vector3 closest = Vector3.zero;
        int minI = 0, minJ = 0;
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
                        minI = i;
                        minJ = j;
                        leastDistance = distance;
                    }
                }
            }
        }
        map[minI, minJ] = DIRTY;
        return findPath(pos, closest).ToArray();
    }

    private List<Vector3> findPath(Vector3 source, Vector3 destination)
    {
        int[] sourceXY = vectorToInt(source);
        int sourceInt = sourceXY[0] * sizeY + sourceXY[1];

        int[] destinationXY = vectorToInt(destination);
        int destinationInt = destinationXY[0] * sizeY + destinationXY[1];

        int[] distance = new int[sizeX * sizeY];
        bool[] visited = new bool[sizeX * sizeY];
        int[] parent = new int[sizeX * sizeY];

        for (int i = 0; i < sizeX * sizeY; i++)
        {
            distance[i] = 999;
            visited[i] = false;
            parent[i] = -1;
        }

        distance[sourceInt] = 0;
        for (int count = 0; count < sizeX * sizeY - 1; count++)
        {
            int u = minDistance(distance, visited);
            visited[u] = true;

            List<int> adj = getAdjacent(u);
            foreach (int v in adj)
            {
                if (!visited[v] && distance[u] + 1 < distance[v])
                {
                    distance[v] = distance[u] + 1;
                    parent[v] = u;
                }
            }
        }

        List<Vector3> path = new List<Vector3>();
        int node = destinationInt;
        while (true)
        {
            if (node == -1)
            {
                path.Clear();
                break;
            }
            else if (node == sourceInt)
            {
                break;
            }
            else
            {
                int i = node / sizeY;
                int j = node % sizeY;
                path.Add(intToVec(i, j));
                node = parent[node];
            }
        }
        path.Reverse();
        return path;
    }

    private List<int> getAdjacent(int u)
    {
        int x = u / sizeY;
        int y = u % sizeY;

        List<int> adj = new List<int>();

        for (int i = x - 1; i <= x + 1; i++)
            for (int j = y - 1; j <= y + 1; j++)
                if (i > 0 && j > 0 && i < sizeX && j < sizeY)
                    if (map[i, j] == DIRTY || map[i, j] == CLEAN)
                        adj.Add(i * sizeY + j);
        return adj;
    }

    private int minDistance(int[] distance, bool[] visited)
    {
        int minDistance = 999, minIndex = 0;
        for (int i = 0; i < distance.Length; i++)
        {
            if (!visited[i] && distance[i] < minDistance)
            {
                minDistance = distance[i];
                minIndex = i;
            }
        }
        return minIndex;
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
        for (int i = xy[0] + 1; i < sizeX; i++)
            if (map[i, xy[1]] == UNEXPLORED || map[i, xy[1]] == DIRTY)
                c++;

        int max = c;
        //Debug.Log("Right: "+c);
        // Back
        c = 0;
        for (int i = 0; i < xy[0]; i++)
            if (map[i, xy[1]] == UNEXPLORED || map[i, xy[1]] == DIRTY)
                c++;

        if (c > max)
        {
            max = c;
            dirIndex = LEFT;
        }
        //Debug.Log("Left: "+c);
        // Left
        c = 0;
        for (int i = 0; i < xy[1]; i++)
            if (map[xy[0], i] == UNEXPLORED || map[i, xy[1]] == DIRTY)
                c++;

        if (c > max)
        {
            max = c;
            dirIndex = BACK;
        }
        //Debug.Log("Back: "+c);
        // Forward
        c = 0;
        for (int i = xy[1] + 1; i < sizeY; i++)
            if (map[xy[0], i] == UNEXPLORED || map[i, xy[1]] == DIRTY)
                c++;

        if (c > max)
        {
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

