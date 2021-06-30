
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
    [TestClass]
    public abstract class StrategyComparisonBase
    {

        public void GiveMats( int initialMats, int startCredits, Player player )
        {
            foreach ( SlicingMats mat in (SlicingMats[])Enum.GetValues( typeof( SlicingMats ) ) )
            {
                player.Mats.Add( new MatCost( mat, initialMats ) );
            }


            player.Mats.FirstOrDefault( m => m.Mat == SlicingMats.Credits ).Amount = startCredits;
        }

        public void GiveMods( int modsToSpawn, Player player )
        {
            for ( int i = 0; i < modsToSpawn; i++ )
                player.Mods.Add( Mod.RollNew() );
        }

        [TestMethod]
        public void CompareStrategies()
        {
            var strategies = new List<IModFarmingStrategy>() {
                new ByTierAscendingStrategy(0),
                new BySpeedRollsThenTierAscendingStrategy(0),
                new BySpeedRollsThenTierAscendingStrategy(1),
                new SellAllGreysStrategy(0),
                new SellAllGreysSpeedFirstStrategy(0),
                new SellAllGreysSpeedFirstStrategy(1),
                };

            var results = new List<Result>();

            int totalPlayers = 1000;

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

        public abstract void RunPlayer( List<Result> results, IModFarmingStrategy strategy );
    }
}