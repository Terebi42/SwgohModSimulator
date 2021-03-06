using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public class BySpeedRollsThenTierAscendingStrategy : BaseModFarmingStrategy, IModFarmingStrategy
    {
        public BySpeedRollsThenTierAscendingStrategy( int allowedMisses ) : base( allowedMisses )
        {
        }

        public override Mod ChooseModToSlice( Player player )
        {
            var workingSet = FilterMods( player );

            var mod = workingSet.OrderByDescending(m=>m.Speed.Rolls).ThenBy( m => m.Tier ).FirstOrDefault();

            if ( mod == null )
                return null;
           
            return mod;

        }
    }
}
