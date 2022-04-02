using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public int x;
    public int y;
}

public class MazeCell : MonoBehaviour
{
    public bool TopWall { get; set; } = true;
    public bool BottomWall { get; set; } = true;
    public bool LeftWall { get; set; } = true;
    public bool RightWall { get; set; } = true;
    public bool Visited { get; set; }

    public Position Pos { get; set; }
}
