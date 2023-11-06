using ConsoleRoguelike.CoreModule;

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
                    var offsets = new Vector2Int[]
                    {
                        Vector2Int.Up,
                        Vector2Int.Down,
                        Vector2Int.Left,
                        Vector2Int.Right
                    };

                    var positions = new Vector2Int[5];
                    positions[0] = ConvertMazeToConsoleCoords(maze.Cells[x, y].Position);

                    for (var i = 0; i < 4; i++)
                    {
                        positions[i + 1] = positions[0] + offsets[i];
                    }

                    wallsPositions.RemoveAll(pos => positions.Contains(pos));
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
