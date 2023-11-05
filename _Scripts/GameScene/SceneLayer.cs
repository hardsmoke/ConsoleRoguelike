using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameObjects;

namespace ConsoleRoguelike.GameScene
{
    internal class SceneLayer
    {
        /// <summary>
        /// <params>Params: 
        /// 1st - layer,
        /// 2nd - previousPosition,
        /// 3rd - gameObejct,
        /// </params>
        /// </summary>
        public event Action<SceneLayer, Vector2Int, GameObject> GameObjectChangedPosition;
        public event Action<SceneLayer, GameObject> GameObjectAdded;
        public event Action<SceneLayer, Vector2Int> GameObjectRemoved;

        private List<GameObject> _gameObjects = new List<GameObject>();
        public IReadOnlyList<GameObject> GameObjects => _gameObjects;

        public bool Contains(Vector2Int position, List<GameObject> exceptedGameObjects = null)
        {
            exceptedGameObjects ??= new List<GameObject>();
            List<GameObject> gameObjects = GetGameObjectsOnPosition(position).Except(exceptedGameObjects).ToList();
            return gameObjects.Count != 0;
        }

        public List<GameObject> GetGameObjectsOnPosition(Vector2Int position, List<GameObject> exceptedGameObjects = null)
        {
            List<GameObject> gameObjects = _gameObjects;

            if (exceptedGameObjects != null)
                gameObjects = _gameObjects.Except(exceptedGameObjects).ToList();

            return gameObjects.Where(obj => obj.Position == position).ToList();
        }

        public bool AddGameObject(GameObject gameObject)
        {
            if (_gameObjects.Contains(gameObject) == false)
            {
                gameObject.PositionChanged += OnGameObjectChangedPosition;
                _gameObjects.Add(gameObject);
                GameObjectAdded?.Invoke(this, gameObject);
                return true;
            }

            return false;
        }

        public bool RemoveGameObject(GameObject gameObject)
        {
            bool removed = _gameObjects.Remove(gameObject);
            gameObject.PositionChanged -= OnGameObjectChangedPosition;
            GameObjectRemoved?.Invoke(this, gameObject.Position);
            return removed;
        }

        public GameObject? GetBottomLeftGameObject()
        {
            GameObject? bottomLeftGameObject = _gameObjects.Count > 0 ? _gameObjects[0] : null;

            for (int i = 1; i < _gameObjects.Count; i++)
            {
                if (Vector2Int.IsFirstHigherSecond(_gameObjects[i].Position, bottomLeftGameObject.Position, true) &&
                    Vector2Int.IsFirstLefterSecond(_gameObjects[i].Position, bottomLeftGameObject.Position, true))
                {
                    bottomLeftGameObject = _gameObjects[i];
                }
            }

            return bottomLeftGameObject;
        }

        private void OnGameObjectChangedPosition(Transform transform, Vector2Int previousPosition, Vector2Int newPosition)
        {
            GameObjectChangedPosition?.Invoke(this, previousPosition, transform as GameObject);
        }
    }
}
