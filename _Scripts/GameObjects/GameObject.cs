using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameScene;
using ConsoleRoguelike.Render;

namespace ConsoleRoguelike.GameObjects
{
    internal class GameObject : Transform
    {
        private TransformRenderer _renderer;
        public TransformRenderer Renderer => _renderer;

        public IReadOnlyScene Scene;

        private SceneLayer _sceneLayer;
        public SceneLayer SceneLayer => _sceneLayer;

        public GameObject(Vector2Int position, char renderedChar, IReadOnlyScene scene, SceneLayer sceneLayer, ConsoleColor color = ConsoleColor.White) : base(position)
        {
            Initialize(renderedChar, scene, sceneLayer, color);
        }

        public virtual void OnCollision(GameObject collideGameObject) { }

        public override void OnPositionChanged()
        {
            List<GameObject> collisionObjects = GetCollisionObjects();

            for (int i = 0; i < collisionObjects.Count; i++)
            {
                collisionObjects[i].OnCollision(this);
            }

            base.OnPositionChanged();
        }

        public override bool CanMove(Vector2Int toMove)
        {
            List<SceneLayer> lowerLayers = Scene.GetLayersThatHigher(_sceneLayer);
            for (int i = 0; i < lowerLayers.Count; i++)
            {
                if (lowerLayers[i].Contains(toMove))
                {
                    return false;
                }
            }

            return base.CanMove(toMove);
        }

        public virtual List<GameObject> GetCollisionObjects()
        {
            List<GameObject> collisionObjects = new List<GameObject>();
            List<GameObject> gameObjects = SceneLayer.GetGameObjectsOnPosition(Position, new List<GameObject>() { this });

            for (int i = 0; i < gameObjects.Count; i++)
            {
                collisionObjects.Add(gameObjects[i]);
            }

            return collisionObjects;
        }

        public void Initialize(char renderedChar, IReadOnlyScene scene, SceneLayer sceneLayer, ConsoleColor color = ConsoleColor.White)
        {
            Scene = scene;
            _renderer = new TransformRenderer(this, renderedChar, color);

            _sceneLayer = sceneLayer;
            _sceneLayer.AddGameObject(this);
        }

        public void Deinitialize()
        {
            _sceneLayer.RemoveGameObject(this);
            _sceneLayer = null;

            _renderer = null;
        }

        public void ChangeSceneLayer(SceneLayer sceneLayer)
        {
            _sceneLayer.RemoveGameObject(this);
            _sceneLayer = sceneLayer;
            _sceneLayer.AddGameObject(this);
        }

        public static void DeinitializeGameObjects<TGameObject>(List<TGameObject> gameObjects) where TGameObject : GameObject
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Deinitialize();
            }
        }
    }
}
