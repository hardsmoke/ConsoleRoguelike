using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.CreatureCondition;
using ConsoleRoguelike.GameObjects;
using ConsoleRoguelike.GameObjects.AI;
using ConsoleRoguelike.GameScene;
using ConsoleRoguelike.Input;
using ConsoleRoguelike.MapGeneration;
using ConsoleRoguelike.Render;

namespace ConsoleRoguelike
{
    class Game
    {
        private static Scene _scene;
        private static Player _player;
        private static Maze _maze;
        private static KeysBindingHandler _keysBindingHandler;

        private static int _playerInitialHealthValue = 100;
        private static int _playerLayerNumber = 0;
        private static int _wallsLayerNumber = 1;
        private static Vector2Int _mazeSize = new Vector2Int(18, 6);
        private static Vector2Int _playerSpawnMazePosition;

        private static List<GameObject> _mazeWalls = new List<GameObject>();
        private static List<Enemy> _enemies = new List<Enemy>();
        private static List<FirstAidKit> _firstAidKits = new List<FirstAidKit>();

        private static SceneRenderer _sceneRenderer;
        private static HealthBarRenderer _playerHealthBarRenderer;
        private static DeathScreenRenderer _deathScreenRenderer;

        private static bool _isReadingInput = false;

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;

            InitializeScene();
            InitializePlayer();
            CreateMaze();
            CreateEnemies();
            CreateFirstAidKits();

            InitializePlayerHealthBar();
            InitializePlayerDeathScreen();
            RenderPlayerHealthBar();

            InitializePlayerInput();
            ReadPlayerInput();
        }

        private static void InitializeScene()
        {
            _scene = new Scene();
            _sceneRenderer = new SceneRenderer(_scene);
        }

        private static void InitializePlayer()
        {
            _playerSpawnMazePosition = Vector2Int.Random(Vector2Int.Zero, _mazeSize);
            SceneLayer playerSceneLayer = _scene.GetLayer(_playerLayerNumber);
            _player = new Player(MazeConverter.ConvertMazeToConsoleCoords(_playerSpawnMazePosition), _scene, playerSceneLayer, _scene.GetLayer(3));
            _player.PositionChanged += OnPlayerPositionChanged;
            _player.Health.Died += OnPlayerDied;
        }

        private static void CreateMaze()
        {
            _playerSpawnMazePosition = MazeConverter.ConvertConsoleToMazeCoords(_player.Position);
            _maze = MazeGenerator.GenerateMaze(_playerSpawnMazePosition, _mazeSize.X, _mazeSize.Y, out _);
            _player.Position = MazeConverter.ConvertMazeToConsoleCoords(_playerSpawnMazePosition);
            List<Vector2Int> wallsPositions = MazeConverter.ConvertMazeToWallsPositions(_maze);
            _mazeWalls = new List<GameObject>();
            for (int i = 0; i < wallsPositions.Count; i++)
            {
                _mazeWalls.Add(new Wall(wallsPositions[i], _scene, _scene.GetLayer(_wallsLayerNumber)));
            }

            CreateMazeExitWall();
        }

        private static void CreateMazeExitWall()
        {
            Vector2Int exitWallMazePosition = _maze.ExitCell.Position;
            Vector2Int exitWallConsolePosition = MazeConverter.ConvertMazeToConsoleCoords(exitWallMazePosition);

            if (exitWallMazePosition.X == 0)
                exitWallConsolePosition += Vector2Int.Left;
            else if (exitWallMazePosition.Y == 0)
                exitWallConsolePosition += Vector2Int.Down;
            else if (exitWallMazePosition.X == _mazeSize.X - 1)
                exitWallConsolePosition += Vector2Int.Right;
            else if (exitWallMazePosition.Y == _mazeSize.Y - 1)
                exitWallConsolePosition += Vector2Int.Up;

            _mazeWalls.Add(new Wall(exitWallConsolePosition, _scene, _scene.GetLayer(_wallsLayerNumber), ' '));
        }

        private static void CreateEnemies()
        {
            SceneLayer emenySceneLayer = _scene.GetLayer(_playerLayerNumber);

            int zombieMemoryCapacity = _mazeSize.X * _mazeSize.Y / 4;
            int zombiesNumber = _mazeSize.X * _mazeSize.Y / ((_mazeSize.X + _mazeSize.Y) * 2);
            for (int i = 0; i < zombiesNumber; i++)
            {
                Vector2Int mazeRandomPosition = Vector2Int.Random(Vector2Int.Zero, _mazeSize);
                Vector2Int consoleRandomPosition = MazeConverter.ConvertMazeToConsoleCoords(mazeRandomPosition);
                if (emenySceneLayer.Contains(mazeRandomPosition) == false)
                {
                    _enemies.Add(new Zombie(consoleRandomPosition, _scene, emenySceneLayer, zombieMemoryCapacity));
                }
            }

            int shooterMemoryCapacity = _mazeSize.X * _mazeSize.Y / 4;
            int shootersNumber = zombiesNumber / 2 + 1;
            for (int i = 0; i < shootersNumber; i++)
            {
                Vector2Int mazeRandomPosition = Vector2Int.Random(Vector2Int.Zero, _mazeSize);
                Vector2Int consoleRandomPosition = MazeConverter.ConvertMazeToConsoleCoords(mazeRandomPosition);
                if (emenySceneLayer.Contains(mazeRandomPosition) == false)
                {
                    Shooter shooter = new Shooter(consoleRandomPosition, _scene, emenySceneLayer, shooterMemoryCapacity);
                    shooter.Shooted += OnShooterShooted;

                    _enemies.Add(shooter);
                }
            }

            void OnShooterShooted(Bullet bullet)
            {
                _enemies.Add(bullet);
                bullet.Destroyed += OnBulletDestroyed;
            }

            void OnBulletDestroyed(Bullet bullet)
            {
                bullet.Destroyed -= OnBulletDestroyed;
                _enemies.Remove(bullet);
            }
        }

        private static void CreateFirstAidKits()
        {
            int numberOfFirstAidKits = _enemies.Count / 4 + 1;

            Vector2Int mazeRandomPosition = Vector2Int.Random(Vector2Int.Zero, _mazeSize);
            Vector2Int consoleRandomPosition = MazeConverter.ConvertMazeToConsoleCoords(mazeRandomPosition);
            for (int i = 0; i < numberOfFirstAidKits; i++)
            {
                FirstAidKit firstAidKit = new FirstAidKit(consoleRandomPosition, _scene, _scene.GetLayer(_playerLayerNumber));
                firstAidKit.Healed += OnFirstAidKitHealed;
                _firstAidKits.Add(firstAidKit);
            }

            static void OnFirstAidKitHealed(FirstAidKit kit)
            {
                kit.Healed -= OnFirstAidKitHealed;
                _firstAidKits.Remove(kit);
            }
        }

        private static void InitializePlayerHealthBar()
        {
            _playerHealthBarRenderer = new HealthBarRenderer(_player.Health);
            _playerHealthBarRenderer.StartRenderPosition = _sceneRenderer.GetBottomLeftRenderedPosition() + Vector2Int.Up;
        }

        private static void InitializePlayerDeathScreen()
        {
            _deathScreenRenderer = new DeathScreenRenderer(_playerHealthBarRenderer);
        }

        private static void RenderPlayerHealthBar()
        {
            _playerHealthBarRenderer.Render();
        }

        private static void InitializePlayerInput()
        {
            _isReadingInput = true;
            _keysBindingHandler = new KeysBindingHandler();
            MakeBindingsOnPlayerAlive();
        }

        private static void MakeBindingsOnPlayerAlive()
        {
            AddWalkingBindings();

            _keysBindingHandler.RemoveBinding(ConsoleKey.F, RespawnPlayer);
            _keysBindingHandler.RemoveBinding(ConsoleKey.L, LoadNewLevel);
            _keysBindingHandler.AddBinding(ConsoleKey.L, LoadNewLevel);
            _keysBindingHandler.AddBinding(ConsoleKey.H, _player.SwitchCollisionMode);
        }

        private static void MakeBindingsOnPlayerDied()
        {
            RemoveWalkingBindings();

            _keysBindingHandler.AddBinding(ConsoleKey.F, RespawnPlayer);
            _keysBindingHandler.RemoveBinding(ConsoleKey.H, _player.SwitchCollisionMode);
        }

        private static void AddWalkingBindings()
        {
            _keysBindingHandler.AddBinding(ConsoleKey.W, _player.MoveUp);
            _keysBindingHandler.AddBinding(ConsoleKey.A, _player.MoveLeft);
            _keysBindingHandler.AddBinding(ConsoleKey.S, _player.MoveDown);
            _keysBindingHandler.AddBinding(ConsoleKey.D, _player.MoveRight);
        }

        private static void RemoveWalkingBindings()
        {
            _keysBindingHandler.RemoveBinding(ConsoleKey.W, _player.MoveUp);
            _keysBindingHandler.RemoveBinding(ConsoleKey.A, _player.MoveLeft);
            _keysBindingHandler.RemoveBinding(ConsoleKey.S, _player.MoveDown);
            _keysBindingHandler.RemoveBinding(ConsoleKey.D, _player.MoveRight);
        }

        private static void OnPlayerDied(IDamager damager)
        {
            MakeBindingsOnPlayerDied();
        }

        private static void RespawnPlayer()
        {
            LoadNewLevel();
            MakeBindingsOnPlayerAlive();
        }

        private static void OnPlayerPositionChanged(Transform transform, Vector2Int previousPosition, Vector2Int newPosition)
        {
            MakeEnemiesStep();

            if (IsPlayerOnTheExitCell(newPosition))
            {
                LoadNewLevel();
            }
        }

        private static void LoadNewLevel()
        {
            _player.Health.SetValue(_playerInitialHealthValue, null);

            DestroyFirstAidKits();
            CreateFirstAidKits();

            DestroyMaze();
            CreateMaze();

            DestroyEnemies();
            CreateEnemies();
        }

        private static void MakeEnemiesStep()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].MakeNextStep();
            }
        }

        private static void DestroyFirstAidKits()
        {
            GameObject.DeinitializeGameObjects(_firstAidKits);
            _firstAidKits.Clear();
        }

        private static void DestroyMaze()
        {
            GameObject.DeinitializeGameObjects(_mazeWalls);
            _mazeWalls.Clear();
        }

        private static void DestroyEnemies()
        {
            GameObject.DeinitializeGameObjects(_enemies);
            _enemies.Clear();
        }

        private static void ReadPlayerInput()
        {
            while (_isReadingInput)
            {
                _keysBindingHandler.ReadBindings();
            }
        }

        private static bool IsPlayerOnTheExitCell(Vector2Int playerPosition)
        {
            return playerPosition == MazeConverter.ConvertMazeToConsoleCoords(_maze.ExitCell.Position);
        }
    }
}