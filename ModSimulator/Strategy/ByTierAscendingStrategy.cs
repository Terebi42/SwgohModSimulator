using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public class ByTierAscendingStrategy : BaseModFarmingStrategy, IModFarmingStrategy
    {
        public ByTierAscendingStrategy( int allowedMisses ) : base( allowedMisses )
        {
        }

        public override Mod ChooseModToSlice( Player player )
        {
            var workingSet = FilterMods( player );

            var mod = workingSet.OrderBy( m => m.Tier ).FirstOrDefault();

            if ( mod == null )
                return null;

            //mod.Slice( player );
            return mod;
        }
    }
}
