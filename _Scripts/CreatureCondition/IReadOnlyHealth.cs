namespace ConsoleRoguelike.CreatureCondition
{
    internal interface IReadOnlyHealth
    {
        public event Action<float, float> HealthChanged;
        public event Action Healed;
        public event Action<float> Damaged;
        public event Action<IDamager> Died;

        public float MaxValue { get; }
        public float Value { get; }

        public float NormalizedValue => Value / MaxValue;
    }
}
