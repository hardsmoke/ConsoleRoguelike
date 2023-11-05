namespace ConsoleRoguelike.CreatureCondition
{
    public class Health : IReadOnlyHealth
    {
        public event Action<float, float> HealthChanged;
        public event Action Healed;
        public event Action<float> Damaged;
        public event Action<IDamager> Died;

        private float _maxValue;
        public float MaxValue => _maxValue;

        private float _value;
        public float Value => _value;

        public Health(float maxValue, float value)
        {
            _maxValue = maxValue;
            _value = value;
        }

        public void SetValue(float value, IHealthAffector affector)
        {
            if (value > 0)
            {
                float clampedValue = Math.Clamp(value, 0, _maxValue);
                float deltaValue = clampedValue - _value;
                if (deltaValue != 0)
                {
                    if (deltaValue > 0)
                    {
                        Healed?.Invoke();
                    }
                    else if (deltaValue < 0)
                    {
                        Damaged?.Invoke(deltaValue * -1);
                    }

                    _value = clampedValue;

                    HealthChanged?.Invoke(value - deltaValue, value);
                }
            }
            else
            {
                _value = 0;
                Died?.Invoke(affector as IDamager);
            }
        }

        public void Heal(float valueToAdd, IHealer healer)
        {
            if (valueToAdd > 0)
            {
                SetValue(_value + valueToAdd, healer);
            }
        }

        public void Damage(float valueToTake, IDamager damager)
        {
            if (valueToTake > 0)
            {
                SetValue(_value - valueToTake, damager);
            }
        }
    }
}
