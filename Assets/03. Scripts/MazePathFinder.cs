using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public struct Pos
{
	public Pos(int x, int y)
	{
		X = x;
		Y = y;
	}
	public int X;
	public int Y;
}

public class MazePathFinder
{
	enum Dir
	{
		Up = 0,
		Down = 1,
		Left = 2,
		Right = 3
	}

	struct Node : IComparable<Node>
	{
		public int F;
		public int G;
		public int X;
		public int Y;

		public int CompareTo(Node other)
		{
			if (F == other.F)
			{
				return 0;
			}
			return F < other.F ? 1 : -1;
		}
	}

	List<Pos> points = new List<Pos>();
	MazeCell[,] maze;
	int width;
	int height;
	int size;

	public int X { get; private set; }
	public int Y { get; private set; }

	public List<Pos> StartPathFind(MazeCell[,] mazeCells, int mazeWidth, int mazeHeight, int wallSize)
	{
		maze = mazeCells;
		width = mazeWidth;
		height = mazeHeight;
		size = wallSize;

		AStar();

		return points;
	}

	void AStar()
	{
		int[] dx = new int[] { 0, 0, -1, 1 };
		int[] dy = new int[] { 1, -1, 0, 0 };
		int[] cost = new int[] { 10, 10, 10, 10 };
		int defaultCost = 10;

		// F = 최종 점수
		// G = 시작점에서 해당 좌표까지 이동하는데 드는 비용
		// H = 목적지에서 얼마나 가까운지

		bool[,] visited = new bool[width, height];
		int[,] open = new int[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				open[x, y] = Int32.MaxValue;
			}
		}

		Pos[,] parent = new Pos[width, height];

		PriorityQueue<Node> pq = new PriorityQueue<Node>();
		open[X, Y] = defaultCost * (Mathf.Abs((width - 1) - Y) + Mathf.Abs((height - 1) - Y));
		pq.Push(new Node() { F = open[X, Y], G = 0, Y = Y, X = X });
		parent[X, Y] = new Pos(X, Y);

		while (pq.Count > 0)
		{
			Node node = pq.Pop();

			if (visited[node.X, node.Y]) continue;

			visited[node.X, node.Y] = true;

			if (node.X == width - 1 && node.Y == height - 1) break;

			for (int i = 0; i < dy.Length; i++)
			{
				int nextX = node.X + dx[i];
				int nextY = node.Y + dy[i];

				if (nextX < 0 || nextX >= width || nextY < 0 || nextY >= height) continue;
				if (visited[nextX, nextY]) continue;
				if (i == (int)Dir.Up)
				{
					if (maze[nextX, nextY].BottomWall ||
						maze[node.X, node.Y].TopWall) continue;
				}
				else if (i == (int)Dir.Down)
				{
					if (maze[nextX, nextY].TopWall ||
						maze[node.X, node.Y].BottomWall) continue;
				}
				else if (i == (int)Dir.Left)
				{
					if (maze[nextX, nextY].RightWall ||
						maze[node.X, node.Y].LeftWall) continue;
				}
				else if (i == (int)Dir.Right)
				{
					if (maze[nextX, nextY].LeftWall ||
						maze[node.X, node.Y].RightWall) continue;
				}

				int g = node.G + cost[i];
				int h = defaultCost * (Mathf.Abs((width - 1) - Y) + Mathf.Abs((height - 1) - Y));

				if (open[nextX, nextY] < g + h) continue;
				open[nextX, nextY] = g + h;

				pq.Push(new Node() { F = g + h, G = g, X = nextX, Y = nextY });
				parent[nextX, nextY] = new Pos(node.X, node.Y);
			}
		}

		CalcPathFromParent(parent);
	}

	void CalcPathFromParent(Pos[,] parent)
	{
		int x = width - 1;
		int y = height - 1;
		while (parent[x, y].X != x || parent[x, y].Y != y)
		{
			points.Add(new Pos(x, y));
			Pos pos = parent[x, y];
			x = pos.X;
			y = pos.Y;
		}
		points.Add(new Pos(x, y));
		points.Reverse();
	}
}
