namespace ConsoleRoguelike.MapGeneration
{
    internal static class MazeConverter
    {
        public static List<Vector2Int> ConvertMazeToWallsPositions(Maze maze)
        {
            List<Vector2Int> walls = new List<Vector2Int>();

            FillMazeAreaWithWalls(maze, walls);
            DestroyWallsOnPath(maze, walls);
            PlaceMazeWalls(maze, walls);

            walls.Add(Vector2Int.Zero);

            return walls;
        }

        public static Vector2Int ConvertMazeToConsoleCoords(Vector2Int mazeCoords)
        {
            return mazeCoords * 2 + Vector2Int.One;
        }

        public static Vector2Int ConvertConsoleToMazeCoords(Vector2Int consoleCoords)
        {
            return (consoleCoords - Vector2Int.One) / 2;
        }

        private static void FillMazeAreaWithWalls(Maze maze, List<Vector2Int> wallsPositions)
        {
            int width = maze.Cells.GetLength(0);
            int height = maze.Cells.GetLength(1);

            for (int x = 0; x < width * 2 + 1; x++)
            {
                for (int y = 0; y < height * 2 + 1; y++)
                {
                    wallsPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        private static void DestroyWallsOnPath(Maze maze, List<Vector2Int> wallsPositions)
        {
            int width = maze.Cells.GetLength(0);
            int height = maze.Cells.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2Int mazeWallPosition = ConvertMazeToConsoleCoords(maze.Cells[x, y].Position);
                    Vector2Int mazeCell = wallsPositions.Where((pos) => pos == mazeWallPosition).FirstOrDefault();
                    Vector2Int upWall = wallsPositions.Where((pos) => pos == mazeWallPosition + Vector2Int.Up).FirstOrDefault();
                    Vector2Int downWall = wallsPositions.Where((pos) => pos == mazeWallPosition + Vector2Int.Down).FirstOrDefault();
                    Vector2Int leftWall = wallsPositions.Where((pos) => pos == mazeWallPosition + Vector2Int.Left).FirstOrDefault();
                    Vector2Int rightWall = wallsPositions.Where((pos) => pos == mazeWallPosition + Vector2Int.Right).FirstOrDefault();
                    if (mazeCell != null)
                    {
                        wallsPositions.Remove(mazeCell);
                    }

                    if (upWall != null)
                    {
                        wallsPositions.Remove(upWall);
                    }

                    if (downWall != null)
                    {
                        wallsPositions.Remove(downWall);
                    }

                    if (leftWall != null)
                    {
                        wallsPositions.Remove(leftWall);
                    }

                    if (rightWall != null)
                    {
                        wallsPositions.Remove(rightWall);
                    }
                }
            }
        }

        private static void PlaceMazeWalls(Maze maze, List<Vector2Int> wallsPositions)
        {
            int width = maze.Cells.GetLength(0);
            int height = maze.Cells.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MazeCell mazeCell = maze.Cells[x, y];
                    for (int i = 0; i < mazeCell.WallsLocalPositions.Count; i++)
                    {
                        wallsPositions.Add(ConvertMazeToConsoleCoords(mazeCell.Position) + mazeCell.WallsLocalPositions[i]);
                    }
                }
            }
        }
    }
}
