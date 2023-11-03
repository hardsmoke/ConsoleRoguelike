namespace ConsoleRoguelike.Input
{
    internal class KeysBindingHandler
    {
        private Dictionary<char, Action> _bindings = new Dictionary<char, Action>();

        public void AddBinding(char key, Action action)
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

        public void RemoveBinding(char key, Action action)
        {
            if (action != null && _bindings.ContainsKey(key))
            {
                _bindings[key] -= action;
            }
        }

        public void RemoveBinding(char key)
        {
            _bindings.Remove(key);
        }

        public void ReadBindings()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (_bindings.ContainsKey(keyInfo.KeyChar))
                {
                    _bindings[keyInfo.KeyChar]?.Invoke();
                }
            }
        }
    }
}
