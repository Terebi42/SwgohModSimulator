using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModSimulator;
using ModSimulator.Strategy;
using System.Linq;

namespace ModSimulatorTests
{
    public class Result
    {
        public IModFarmingStrategy Strategy { get; set; }
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
            var strategies = new List<IModFarmingStrategy>() { new ByTierAscendingStrategy(), new BySpeedRollsThenTierDescendingStrategy(), new SellAllGreysStrategy(), new BySpeedRollsAllow1MissThenTierDescendingStrategy() };

            var results = new List<Result>();

            for ( int iteration = 0; iteration < 1000; iteration++ )
            {
                int modsToSpawn = 200;
                foreach ( var strategy in strategies )
                {
                    var player = new Player();
                    foreach ( SlicingMats mat in (SlicingMats[])Enum.GetValues( typeof( SlicingMats ) ) )
                    {

                        player.Mats.Add( new MatCost( mat, 1 * modsToSpawn ) );

                    }

                    var startCredits = 407000 * modsToSpawn;
                    player.Mats.FirstOrDefault( m => m.Mat == SlicingMats.Credits ).Amount = startCredits;

                    for ( int i = 0; i < modsToSpawn; i++ )
                        player.Mods.Add( Mod.RollNew() );


                    strategy.Expose( player );

                    while ( player.Mods.Any( m => m.CanBeSlicedBy( player ) ) )
                    {
                        var mod = strategy.Slice( player );
                        if ( mod == null )
                            break;

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

                    result.Speed0 += player.Mods.Where( m => m.Speed?.Rolls == 0 || m.Speed==null ).Count();
                    result.Speed1 += player.Mods.Where( m => m.Speed?.Rolls == 1 ).Count();
                    result.Speed2 += player.Mods.Where( m => m.Speed?.Rolls == 2 ).Count();
                    result.Speed3 += player.Mods.Where( m => m.Speed?.Rolls == 3 ).Count();
                    result.Speed4 += player.Mods.Where( m => m.Speed?.Rolls == 4 ).Count();
                    result.Speed5 += player.Mods.Where( m => m.Speed?.Rolls == 5 ).Count();


                }
            }

            foreach ( var result in results )
            {
                Console.WriteLine( result.Strategy.ToString() );
                Console.WriteLine( $"5 speed rolls {result.Speed5}" );
                Console.WriteLine( $"4 speed rolls {result.Speed4}" );
                Console.WriteLine( $"3 speed rolls {result.Speed3}" );
                Console.WriteLine( $"2 speed rolls {result.Speed2}" );
                Console.WriteLine( $"1 speed rolls {result.Speed1}" );
                Console.WriteLine( $"0 speed rolls {result.Speed0}" );
            }



        }
    }
}
