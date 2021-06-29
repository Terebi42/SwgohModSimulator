using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public class BySpeedRollsThenTierAscendingStrategy : IModFarmingStrategy
    {
        public int AllowedMisses { get; }

        public BySpeedRollsThenTierAscendingStrategy( int allowedMisses)
        {
            AllowedMisses = allowedMisses;
        }

        public override string ToString()
        {
            return $"BySpeedRollsThenTierAscendingStrategy Allowed Misses = {AllowedMisses}";
        }

        public void Expose( Player player )
        {
            foreach (var mod in player.Mods)
            {
                mod.ExposeAllSecondaries( player );
                if ( mod.Speed != null )
                {
                    mod.LevelTo( player, 12 );
                }
            }
        }

        public Mod ChooseModToSlice( Player player )
        {
            

            var workingSet = player.Mods.ToArray().ToList();
            
            workingSet.RemoveAll( m => !m.CanBeSlicedBy( player ) );
            workingSet.RemoveAll( m => m.Speed == null );
            workingSet.RemoveAll( m => m.Speed.Rolls + AllowedMisses < (int)m.Tier ); //Green < 1 rolls, Blue < 2, etc

            workingSet.RemoveAll( m => m.Speed.Rolls >=5 ); //dont roll past 5 speed rolls
            var mod = workingSet.OrderByDescending(m=>m.Speed.Rolls).ThenBy( m => m.Tier ).FirstOrDefault();

            if ( mod == null )
                return null;

           // mod.Slice( player );
            return mod;



        }
    }
}
