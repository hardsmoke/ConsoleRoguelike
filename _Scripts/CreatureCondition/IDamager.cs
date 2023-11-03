namespace ConsoleRoguelike.CreatureCondition
{
    public interface IDamager : IHealthAffector
    {
        public string DeathReason { get; }
    }
}
