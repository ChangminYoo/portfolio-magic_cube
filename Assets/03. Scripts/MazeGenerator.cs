using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallState
{
    All = 0,
    Left = 1,
    Right = 2,
    Up = 3,
    Down = 4,

    Visited = 100
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject wallObject;

    [SerializeField]
    GameObject floorObject;

    [SerializeField]
    [Range(5, 100)]
    int mazeWidth = 10;

    [SerializeField]
    [Range(5, 100)]
    int mazeHeight = 10;

    int wallSize = 1;
    
    void Start()
    {
        Draw();
    }

    void Draw()
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
                Transform topWall = Instantiate(wallObject, transform).transform;
                topWall.position = position + (Vector3.forward * half);
                topWall.localScale = new Vector3(wallSize, topWall.localScale.y, topWall.localScale.z);

                //left
                Transform leftWall = Instantiate(wallObject, transform).transform;
                leftWall.position = position + (Vector3.right * -half);
                leftWall.eulerAngles = new Vector3(0, 90, 0);
                leftWall.localScale = new Vector3(wallSize, leftWall.localScale.y, leftWall.localScale.z);

                // right
                if (w == mazeWidth - wallSize)
                {
                    Transform rightWall = Instantiate(wallObject, transform).transform;
                    rightWall.position = position + (Vector3.right * half);
                    rightWall.eulerAngles = new Vector3(0, 90, 0);
                    rightWall.localScale = new Vector3(wallSize, rightWall.localScale.y, rightWall.localScale.z);
                }

                // bottom
                if (h == 0)
                {
                    Transform bottomWall = Instantiate(wallObject, transform).transform;
                    bottomWall.position = position + (Vector3.forward * -half);
                    bottomWall.localScale = new Vector3(wallSize, bottomWall.localScale.y, bottomWall.localScale.z);
                }
            }
        }
    }
}
