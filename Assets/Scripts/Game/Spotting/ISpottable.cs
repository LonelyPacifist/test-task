namespace test_sber
{
    public interface ISpottable
    {
        public void TrySpot(float duration);
        public bool CanBeSpotted { get; }
    }
}