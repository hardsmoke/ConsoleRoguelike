namespace ConsoleRoguelike.CreatureCondition
{
    [Serializable]
    public class Health
    {
        public event Action<float> OnHealthChange;
        public event Action OnHeal;
        public event Action<float> OnDamage;
        public event Action<IDamager> OnDie;

        private float _maxValue;
        public float MaxValue => _maxValue;

        private float _value;
        public float Value => _value;

        public float NormalizedValue => _value / _maxValue;

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
                        OnHeal?.Invoke();
                    }
                    else if (deltaValue < 0)
                    {
                        OnDamage?.Invoke(deltaValue * -1);
                    }

                    _value = clampedValue;

                    OnHealthChange?.Invoke(deltaValue);
                }
            }
            else
            {
                _value = 0;
                OnDie?.Invoke(affector as IDamager);
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
