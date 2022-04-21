using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallState
{
    All = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject wallObject;

    [SerializeField]
    GameObject floorObject;

    [SerializeField]
    GameObject pathFindObject;

    [SerializeField]
    GameObject entranceObject;

    [SerializeField]
    [Range(5, 100)]
    int mazeWidth = 10;

    [SerializeField]
    [Range(5, 100)]
    int mazeHeight = 10;

    int wallSize = 1;
    MazeCell[,] maze;
    int[] dx = new int[4] { 0, 0, -1, 1 };
    int[] dy = new int[4] { 1, -1, 0, 0 };

    int currX = 0;
    int currY = 0;
    bool allVisited = false;

    Vector3 startPos;
    Vector3 endPos;

    List<GameObject> pathObjects;
    MazePathFinder mp;
    Transform player;

    void Start()
    {
        Generate();
        Render();
        SetCharacter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RenderExitPath();
        }

        if (Vector3.Distance(player.position, endPos) < 0.8f)
		{
            Debug.Log("µµÂø!!!");
		}
    }

    void SetCharacter()
	{
        player = GameObject.Find("Player").transform.GetChild(0);
        player.position = startPos;
    }

    public void RenderExitPath()
	{
        mp = new MazePathFinder();
        List<Pos> points = mp.StartPathFind(maze, mazeWidth, mazeHeight, wallSize);

        RenderPath(points);
    }

    void CreateEntranceAndExit()
	{
        maze[0, 0].BottomWall = false;
        maze[mazeWidth - 1, mazeHeight - 1].TopWall = false;

        Transform entranceFloor = Instantiate(entranceObject, transform).transform;
        startPos = entranceFloor.position = new Vector3(-mazeWidth * 0.5f + wallSize * 0.5f, 0, -mazeHeight * 0.5f - wallSize * 0.5f);

        Transform exitFloor = Instantiate(entranceObject, transform).transform;
        endPos = exitFloor.position = new Vector3(mazeWidth * 0.5f - wallSize * 0.5f, 0, mazeHeight * 0.5f + wallSize * 0.5f);
        exitFloor.eulerAngles = new Vector3(0, 180, 0);
    }

    void Render()
    {
        Transform floor = Instantiate(floorObject, transform).transform;
        floor.localScale = new Vector3(mazeWidth, 0.1f, mazeHeight);
        float half = wallSize * 0.5f;
        for (int w = 0; w < mazeWidth; w += wallSize)
        {
            for (int h = 0; h < mazeHeight; h += wallSize)
            {
                Vector3 position = new Vector3(-mazeWidth * 0.5f + w + half, 0.5f, -mazeHeight * 0.5f + h + half);

                // top
                if (maze[w, h].TopWall)
                {
                    Transform topWall = Instantiate(wallObject, transform).transform;
                    topWall.position = position + (Vector3.forward * half);
                    topWall.localScale = new Vector3(wallSize, topWall.localScale.y, topWall.localScale.z);
                    topWall.name = "topwall_" + w + "__" + h;
                }

                //left
                if (maze[w, h].LeftWall)
                {
                    Transform leftWall = Instantiate(wallObject, transform).transform;
                    leftWall.position = position + (Vector3.right * -half);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                    leftWall.localScale = new Vector3(wallSize, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.name = "leftWall" + w + "__" + h;
                }

                // right
                if (w == mazeWidth - wallSize)
                {
                    if (maze[w, h].RightWall)
                    {
                        Transform rightWall = Instantiate(wallObject, transform).transform;
                        rightWall.position = position + (Vector3.right * half);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                        rightWall.localScale = new Vector3(wallSize, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.name = "rightWall" + w + "__" + h;
                    }
                }

                // bottom
                if (h == 0)
                {
                    if (maze[w, h].BottomWall)
                    {
                        Transform bottomWall = Instantiate(wallObject, transform).transform;
                        bottomWall.position = position + (Vector3.forward * -half);
                        bottomWall.localScale = new Vector3(wallSize, bottomWall.localScale.y, bottomWall.localScale.z);
                        bottomWall.name = "bottomWall" + w + "__" + h;
                    }
                }
            }
        }
    }

    void RenderPath(List<Pos> points)
    {
        float half = wallSize * 0.5f;
        if (pathObjects == null)
        {
            pathObjects = new List<GameObject>();
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 position = new Vector3(-mazeWidth * 0.5f + points[i].X + half, 0.5f, -mazeHeight * 0.5f + points[i].Y + half);
                pathObjects.Add(Instantiate(pathFindObject, position, Quaternion.identity, transform));
            }
        }
        else
		{
            for (int i = 0; i < pathObjects.Count; i++)
			{
                pathObjects[i].SetActive(true);
			}
		}
    }

    void RemovePath()
	{
        for (int i = 0; i < pathObjects.Count; i++)
		{
            if (pathObjects[i] != null)
			{
                Destroy(pathObjects[i]);
			}
		}
        pathObjects.Clear();
    }

    void Generate()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];
        for (int w = 0; w < mazeWidth; w += wallSize)
        {
            for (int h = 0; h < mazeHeight; h += wallSize)
            {
                maze[w, h] = new MazeCell();
                maze[w, h].Visited = false;
            }
        }

        CreateEntranceAndExit();

        HuntAndKill();
    }

	#region HuntAndKill Algorithm
	void HuntAndKill()
    {
        while (!allVisited)
        {
            Walk();
            Hunt();
        }
    }

    void Walk()
    {
        while (FindWalkablePath(currX, currY))
        {
            int direction = Random.Range(1, 5);

            int nextX = currX + dx[direction - 1];
            int nextY = currY + dy[direction - 1];

            if (IsWithInBounds(nextX, nextY) && IsCellVisited(nextX, nextY) == false)
            {
                if (direction == 1)
                {
                    maze[currX, currY].TopWall = false;
                    maze[nextX, nextY].BottomWall = false;
                }
                else if (direction == 2)
                {
                    maze[currX, currY].BottomWall = false;
                    maze[nextX, nextY].TopWall = false;
                }
                else if (direction == 3)
                {
                    maze[currX, currY].LeftWall = false;
                    maze[nextX, nextY].RightWall = false;
                }
                else if (direction == 4)
                {
                    maze[currX, currY].RightWall = false;
                    maze[nextX, nextY].LeftWall = false;
                }
                currX = nextX;
                currY = nextY;
                maze[currX, currY].Visited = true;
            }
        }
    }

    void Hunt()
    {
        for (int x = 0; x < mazeWidth; x += wallSize)
        {
            for (int y = 0; y < mazeHeight; y += wallSize)
            {
                if (IsCellVisited(x, y) == false &&
                    CheckNextCellVisited(x, y))
                {
                    List<int> neighbors = new List<int>();

                    if (y + 1 < mazeHeight && IsCellVisited(x, y + 1))
                    {
                        neighbors.Add(1);
                    }
                    if (y > 0 && IsCellVisited(x, y - 1))
                    {
                        neighbors.Add(2);
                    }                                                
                    if (x > 0 && IsCellVisited(x - 1, y))
                    {
                        neighbors.Add(3);
                    }
                    if (x + 1 < mazeWidth && IsCellVisited(x + 1, y))
                    {
                        neighbors.Add(4);
                    }

                    int direction = neighbors[Random.Range(0, neighbors.Count)];
                    int nextX = x + dx[direction - 1];
                    int nextY = y + dy[direction - 1];

                    if (direction == 1)
                    {
                        maze[x, y].TopWall = false;
                        maze[nextX, nextY].BottomWall = false;
                    }
                    else if (direction == 2)
                    {
                        maze[x, y].BottomWall = false;
                        maze[nextX, nextY].TopWall = false;
                    }
                    else if (direction == 3)
                    {
                        maze[x, y].LeftWall = false;
                        maze[nextX, nextY].RightWall = false;
                    }
                    else if (direction == 4)
                    {
                        maze[x, y].RightWall = false;
                        maze[nextX, nextY].LeftWall = false;
                    }

                    currX = x;
                    currY = y;
                    maze[x, y].Visited = true;
                    return;
                }
            }
        }
        allVisited = true;
    }
	#endregion

	bool IsCellVisited(int x, int y)
    {
        return maze[x, y].Visited;
    }

    bool CheckNextCellVisited(int x, int y)
    {
        if (y > 0 && IsCellVisited(x, y - 1)) return true;
        if (y + 2 < mazeHeight && IsCellVisited(x, y + 1)) return true;
        if (x > 0 && IsCellVisited(x - 1, y)) return true;
        if (x + 2 < mazeWidth && IsCellVisited(x + 1, y)) return true;
        return false;
    }

    bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < mazeWidth && y >= 0 && y < mazeHeight);
    }

    bool FindWalkablePath(int x, int y)
    {
        if (IsWithInBounds(x, y - 1) && !IsCellVisited(x, y - 1)) return true;
        if (IsWithInBounds(x, y + 1) && !IsCellVisited(x, y + 1)) return true;
        if (IsWithInBounds(x - 1, y) && !IsCellVisited(x - 1, y)) return true;
        if (IsWithInBounds(x + 1, y) && !IsCellVisited(x + 1, y)) return true;
        return false;
    }
}
