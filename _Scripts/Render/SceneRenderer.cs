namespace ConsoleRoguelike.Render
{
    using GameObjects;
    using GameScene;

    internal class SceneRenderer : IConsoleRenderer
    {
        private readonly Scene _scene;

        public Vector2Int StartRenderPosition { get => Vector2Int.Zero; }

        public SceneRenderer(Scene scene)
        {
            _scene = scene;
            _scene.GameObjectChangedPosition += OnGameObjectChangedPosition;
            _scene.GameObjectAdded += OnGameObjectAdded;
            _scene.GameObjectRemoved += OnGameObjectRemoved;
        }

        public void Render(SceneLayer layer)
        {
            IReadOnlyList<GameObject> gameObjects = layer.GameObjects;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Renderer.Render();
            }
        }

        public void Render(int layerNumber)
        {
            Render(_scene.GetLayer(layerNumber));
        }

        public void RenderAll()
        {
            List<SceneLayer> sceneLayersAscending = _scene.GetLayersAscending();

            for (int i = 0; i < sceneLayersAscending.Count; i++)
            {
                Render(sceneLayersAscending[i]);
            }
        }

        public void Render()
        {
            RenderAll();
        }

        public Vector2Int GetBottomLeftRenderedPosition()
        {
            GameObject bottomLeftGameObject = _scene.GetBottomLeftGameObject();
            return bottomLeftGameObject == null ? Vector2Int.One * -1 : bottomLeftGameObject.Position;
        }

        private void OnGameObjectChangedPosition(SceneLayer layer, Vector2Int previousPosition, GameObject gameObject)
        {
            if (_scene.GetHighestLayer() == layer)
            {
                gameObject.Renderer.Render();
            }

            RenderPosition(previousPosition);
        }

        private void OnGameObjectAdded(SceneLayer layer, GameObject gameObject)
        {
            RenderPosition(gameObject.Position);
        }

        private void OnGameObjectRemoved(SceneLayer layer, Vector2Int position)
        {
            RenderPosition(position);
        }

        private void RenderPosition(Vector2Int position)
        {
            GameObject highestGameObjectOnPosition = _scene.GetHighestGameObjectOnPosition(position);
            if (highestGameObjectOnPosition != null)
            {
                highestGameObjectOnPosition.Renderer.Render();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(position.X, position.Y);
                Console.Write(' ');
            }
        }
    }
}
