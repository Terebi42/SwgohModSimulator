using ModSimulator.Strategy;


namespace ModSimulatorTests
{
    public class Result
    {
        public IModFarmingStrategy Strategy { get; set; }
        public long Slices { get; set; }
        public long SpeedHits { get; set; }
        public long ModCount { get; set; }
        public int Speed0 { get; set; }
        public int Speed1 { get; set; }
        public int Speed2 { get; set; }
        public int Speed3 { get; set; }
        public int Speed4 { get; set; }
        public int Speed5 { get; set; }
        public int ModEnergyConsumed { get; set; }
        public int MatEnergyConsumed { get; set; }
    }
}
