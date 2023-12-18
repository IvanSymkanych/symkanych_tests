namespace Core.Game
{
    public class RewardData
    {
        public int Score { get; private set; }
        public int Energy { get; private set; }
        public int Life { get; private set; }

        public RewardData(int energy, int life)
        {
            Energy = energy;
            Life = life;
        }
    }
}