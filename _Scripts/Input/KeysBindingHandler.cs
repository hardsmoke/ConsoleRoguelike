namespace ConsoleRoguelike.Input
{
    internal class KeysBindingHandler
    {
        private Dictionary<ConsoleKey, Action> _bindings = new Dictionary<ConsoleKey, Action>();

        public void AddBinding(ConsoleKey key, Action action)
        {
            if (action != null)
            {
                if (_bindings.ContainsKey(key))
                {
                    _bindings[key] += action;
                }
                else
                {
                    _bindings.Add(key, action);
                }
            }
        }

        public void RemoveBinding(ConsoleKey key, Action action)
        {
            if (action != null && _bindings.ContainsKey(key))
            {
                _bindings[key] -= action;
            }
        }

        public void RemoveBinding(ConsoleKey key)
        {
            _bindings.Remove(key);
        }

        public void ReadBindings()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (_bindings.ContainsKey(keyInfo.Key))
                {
                    _bindings[keyInfo.Key]?.Invoke();
                }
            }
        }
    }
}
