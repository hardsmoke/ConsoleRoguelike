using ConsoleRoguelike.CoreModule;

namespace ConsoleRoguelike.MapGeneration
{
    internal static class MazeGenerator
    {
        public static Maze GenerateMaze(Vector2Int startPosition, int width, int height, out MazeGeneratorCell[,] generatorCells)
        {
            MazeCell[,] mazeCells = new MazeCell[width, height];
            generatorCells = new MazeGeneratorCell[width, height];
            Maze maze = new Maze(mazeCells);
            InitializeMaze(maze, generatorCells);
            RemoveWalls(startPosition, generatorCells);
            MakeExit(startPosition, generatorCells, out MazeGeneratorCell exitCell);
            maze.ExitCell = exitCell.Cell;

            return maze;
        }

        public static List<MazeCell> GetCellsByMinDistanceFromStartPosition(MazeGeneratorCell[,] generatorCells, int minDistance)
        {
            List<MazeCell> mazeCells = new List<MazeCell>();

            for (int x = 0; x < generatorCells.GetLength(0); x++)
            {
                for (int y = 0; y < generatorCells.GetLength(1); y++)
                {
                    if (generatorCells[x, y].DistanceFromStart > minDistance)
                    {
                        mazeCells.Add(generatorCells[x, y].Cell);
                    }
                }
            }

            return mazeCells;
        }

        private static void InitializeMaze(Maze maze, MazeGeneratorCell[,] generatorCells)
        {
            for (int x = 0; x < maze.Cells.GetLength(0); x++)
            {
                for (int y = 0; y < maze.Cells.GetLength(1); y++)
                {
                    maze.Cells[x, y] = new MazeCell(new Vector2Int(x, y));
                    generatorCells[x, y] = new MazeGeneratorCell(maze.Cells[x, y]);
                }
            }
        }

        private static void RemoveWalls(Vector2Int initialPosition, MazeGeneratorCell[,] generatorCells)
        {
            MazeGeneratorCell current = generatorCells[initialPosition.X, initialPosition.Y];
            current.IsVisited = true;
            current.DistanceFromStart = 0;

            Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
            do
            {
                Vector2Int currentCellPosition = current.Cell.Position;
                List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

                if (ShouldVisitTopNeighbour(generatorCells, currentCellPosition, out MazeGeneratorCell topNeighbour))
                    unvisitedNeighbours.Add(topNeighbour);

                if (ShouldVisitBottomNeighbour(generatorCells, currentCellPosition, out MazeGeneratorCell bottomNeighbour))
                    unvisitedNeighbours.Add(bottomNeighbour);

                if (ShouldVisitLeftNeighbour(generatorCells, currentCellPosition, out MazeGeneratorCell leftNeighbour))
                    unvisitedNeighbours.Add(leftNeighbour);

                if (ShouldVisitRightNeighbour(generatorCells, currentCellPosition, out MazeGeneratorCell rightNeighbour))
                    unvisitedNeighbours.Add(rightNeighbour);

                if (unvisitedNeighbours.Count > 0)
                {
                    Random random = new Random();
                    int randomUnvisitedNeighbourIndex = random.Next(unvisitedNeighbours.Count);
                    MazeGeneratorCell chosen = unvisitedNeighbours[randomUnvisitedNeighbourIndex];

                    RemoveWall(current, chosen);
                    chosen.IsVisited = true;
                    stack.Push(chosen);
                    current = chosen;
                    current.DistanceFromStart = stack.Count;
                }
                else
                {
                    current = stack.Pop();
                }

            } while (stack.Count > 0);
        }

        private static void MakeExit(Vector2Int startPosition, MazeGeneratorCell[,] generatorCells, out MazeGeneratorCell exitCell)
        {
            exitCell = generatorCells[startPosition.X, startPosition.Y];

            int width = generatorCells.GetLength(0);
            int height = generatorCells.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                if (generatorCells[x, 0].DistanceFromStart > exitCell.DistanceFromStart)
                    exitCell = generatorCells[x, 0];

                if (generatorCells[x, height - 1].DistanceFromStart > exitCell.DistanceFromStart)
                    exitCell = generatorCells[x, height - 1];
            }

            for (int y = 0; y < height; y++)
            {
                if (generatorCells[0, y].DistanceFromStart > exitCell.DistanceFromStart)
                    exitCell = generatorCells[0, y];

                if (generatorCells[width - 1, y].DistanceFromStart > exitCell.DistanceFromStart)
                    exitCell = generatorCells[width - 1, y];
            }

            if (exitCell.Cell.Position.X == 0)
                exitCell.Cell.RemoveLeftWall();
            else if (exitCell.Cell.Position.Y == 0)
                exitCell.Cell.RemoveDownWall();
            else if (exitCell.Cell.Position.X == width - 1)
                exitCell.Cell.RemoveRightWall();
            else if (exitCell.Cell.Position.Y == height - 1)
                exitCell.Cell.RemoveUpWall();

        }

        private static bool ShouldVisitTopNeighbour(MazeGeneratorCell[,] generatorCells, Vector2Int currentCellPosition, out MazeGeneratorCell topNeighbour)
        {
            topNeighbour = default;
            if (HasTopNeighbour(currentCellPosition, out Vector2Int topNeighbourPosition))
            {
                topNeighbour = generatorCells[topNeighbourPosition.X, topNeighbourPosition.Y];
                return IsCellOnPositionNotVisited(generatorCells, topNeighbourPosition);
            }

            return false;
        }

        private static bool HasTopNeighbour(Vector2Int currentCellPosition, out Vector2Int topNeighbourPosition)
        {
            topNeighbourPosition = currentCellPosition + Vector2Int.Down;
            return topNeighbourPosition.Y >= 0;
        }

        private static bool ShouldVisitBottomNeighbour(MazeGeneratorCell[,] generatorCells, Vector2Int currentCellPosition, out MazeGeneratorCell buttomNeighbour)
        {
            buttomNeighbour = default;
            if (HasButtomNeighbour(currentCellPosition, generatorCells.GetLength(1), out Vector2Int buttomNeighbourPosition))
            {
                buttomNeighbour = generatorCells[buttomNeighbourPosition.X, buttomNeighbourPosition.Y];
                return IsCellOnPositionNotVisited(generatorCells, buttomNeighbourPosition);
            }

            return false;
        }

        private static bool HasButtomNeighbour(Vector2Int currentCellPosition, int mazeHeight, out Vector2Int buttomNeighbourPosition)
        {
            buttomNeighbourPosition = currentCellPosition + Vector2Int.Up;
            return buttomNeighbourPosition.Y < mazeHeight;
        }

        private static bool ShouldVisitLeftNeighbour(MazeGeneratorCell[,] generatorCells, Vector2Int currentCellPosition, out MazeGeneratorCell leftNeighbour)
        {
            leftNeighbour = default;
            if (HasLeftNeighbour(currentCellPosition, out Vector2Int leftNeighbourPosition))
            {
                leftNeighbour = generatorCells[leftNeighbourPosition.X, leftNeighbourPosition.Y];
                return IsCellOnPositionNotVisited(generatorCells, leftNeighbourPosition);
            }

            return false;
        }

        private static bool HasLeftNeighbour(Vector2Int currentCellPosition, out Vector2Int leftNeighbourPosition)
        {
            leftNeighbourPosition = currentCellPosition + Vector2Int.Left;
            return leftNeighbourPosition.X >= 0;
        }

        private static bool ShouldVisitRightNeighbour(MazeGeneratorCell[,] generatorCells, Vector2Int currentCellPosition, out MazeGeneratorCell rightNeighbour)
        {
            rightNeighbour = default;
            if (HasRightNeighbour(currentCellPosition, generatorCells.GetLength(0), out Vector2Int rightNeighbourPosition))
            {
                rightNeighbour = generatorCells[rightNeighbourPosition.X, rightNeighbourPosition.Y];
                return IsCellOnPositionNotVisited(generatorCells, rightNeighbourPosition);
            }

            return false;
        }

        private static bool HasRightNeighbour(Vector2Int currentCellPosition, int mazeWidth, out Vector2Int rightNeighbourPosition)
        {
            rightNeighbourPosition = currentCellPosition + Vector2Int.Right;
            return rightNeighbourPosition.X < mazeWidth;
        }

        private static bool IsCellOnPositionNotVisited(MazeGeneratorCell[,] generatorCells, Vector2Int cellPosition)
        {
            return generatorCells[cellPosition.X, cellPosition.Y].IsVisited == false;
        }

        private static void RemoveWall(MazeGeneratorCell first, MazeGeneratorCell second)
        {
            first.Cell.RemoveWall(second.Cell.Position - first.Cell.Position);
            second.Cell.RemoveWall(first.Cell.Position - second.Cell.Position);
        }
    }
}