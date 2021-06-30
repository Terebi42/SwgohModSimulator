using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModSimulator;
using ModSimulator.Strategy;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;


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
    }

    [TestClass]
    public class StrategyComparison
    {
        [TestMethod]
        public void CompareStrategies()
        {
            var strategies = new List<IModFarmingStrategy>() {
                new ByTierAscendingStrategy(0),
                new BySpeedRollsThenTierAscendingStrategy(0),
                new BySpeedRollsThenTierAscendingStrategy(1),
                new BySpeedRollsThenTierAscendingStrategy(2),
                new SellAllGreysStrategy(0),
                new SellAllGreysSpeedFirstStrategy(0),
                new SellAllGreysSpeedFirstStrategy(1),

                };

            var results = new List<Result>();

            int totalPlayers = 200;

            foreach ( var strategy in strategies )
            {
                bool runParallel = true;

                if ( runParallel )
                {
                    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 12 };
                    Parallel.For( 0, totalPlayers, parallelOptions, ( i, state ) => { RunPlayer( results, strategy ); } );
                }
                else
                {
                    for ( int playerIteration = 0; playerIteration < totalPlayers; playerIteration++ )
                    {
                        RunPlayer( results, strategy );
                    }
                }
            }

            foreach ( var result in results )
            {
                Console.WriteLine( result.Strategy.ToString() );
                Console.WriteLine( $"Total Mods {result.ModCount} = {result.ModCount / totalPlayers}/player" );
                Console.WriteLine( $"Total Slices {result.Slices} = {result.Slices / totalPlayers}/player" );
                Console.WriteLine( $"Total Speed Hits {result.SpeedHits} = {result.SpeedHits / totalPlayers}/player" );
                Console.WriteLine( $"(5) =  {result.Speed5} = {result.Speed5 / totalPlayers}/player" );
                Console.WriteLine( $"(4) =  {result.Speed4}  = {result.Speed4 / totalPlayers}/player" );
                Console.WriteLine( $"(3) =  {result.Speed3} = {result.Speed3 / totalPlayers}/player" );
                Console.WriteLine( $"(2) = {result.Speed2} = {result.Speed2 / totalPlayers}/player" );
                Console.WriteLine( $"(1) = {result.Speed1} = {result.Speed1 / totalPlayers}/player" );
                Console.WriteLine( $"(0) = {result.Speed0} = {result.Speed0 / totalPlayers}/player" );
            }
        }

        private void RunPlayer( List<Result> results, IModFarmingStrategy strategy )
        {
            int cyclesPerPlayer = 200;
            int modsToSpawn = 100;
            int initialMats = 200;
            int maxCostToSliceMod = 407000;
            var startCredits = 10000000;//maxCostToSliceMod * modsToSpawn;

            int sliceCount = 0;
            int speedHits = 0;
            var player = new Player();

            for ( int playerCycle = 0; playerCycle < cyclesPerPlayer; playerCycle++ )
            {
                GiveMats( initialMats, startCredits, player );

                GiveMods( modsToSpawn, player );

                strategy.Expose( player );

                Mod mod = null;

                while ( true )
                {
                    mod = strategy.ChooseModToSlice( player );
                    if ( mod == null )
                    {
                        break;
                    }
                    var initialSpeed = mod.Speed.Rolls;
                    sliceCount++;
                    mod.Slice( player );
                    var newSpeed = mod.Speed.Rolls;
                    if ( newSpeed > initialSpeed )
                    {
                        speedHits++;
                    }

                }
            }

            var result = results.FirstOrDefault( r => r.Strategy == strategy );
            if ( result == null )
            {
                result = new Result
                {
                    Strategy = strategy
                };
                results.Add( result );

            }

            lock ( this )
            {
                result.Speed0 += player.Mods.Where( m => m.Speed?.Rolls == 0 || m.Speed == null ).Count();
                result.Speed1 += player.Mods.Where( m => m.Speed?.Rolls == 1 ).Count();
                result.Speed2 += player.Mods.Where( m => m.Speed?.Rolls == 2 ).Count();
                result.Speed3 += player.Mods.Where( m => m.Speed?.Rolls == 3 ).Count();
                result.Speed4 += player.Mods.Where( m => m.Speed?.Rolls == 4 ).Count();
                result.Speed5 += player.Mods.Where( m => m.Speed?.Rolls == 5 ).Count();
                result.Slices += sliceCount;
                result.SpeedHits += speedHits;

                result.ModCount += player.Mods.Count();
            }
        }

        private static void GiveMods( int modsToSpawn, Player player )
        {
            for ( int i = 0; i < modsToSpawn; i++ )
                player.Mods.Add( Mod.RollNew() );
        }

        private static void GiveMats( int initialMats, int startCredits, Player player )
        {
            foreach ( SlicingMats mat in (SlicingMats[])Enum.GetValues( typeof( SlicingMats ) ) )
            {
                player.Mats.Add( new MatCost( mat, initialMats ) );
            }


            player.Mats.FirstOrDefault( m => m.Mat == SlicingMats.Credits ).Amount = startCredits;
        }
    }
}
