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
    public class StrategyComparison : StrategyComparisonBase
    {

        public override  void RunPlayer( List<Result> results, IModFarmingStrategy strategy )
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
    }
}
