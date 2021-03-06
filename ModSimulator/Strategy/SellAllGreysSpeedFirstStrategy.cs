using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public class SellAllGreysSpeedFirstStrategy : BaseModFarmingStrategy, IModFarmingStrategy
    {
        public SellAllGreysSpeedFirstStrategy( int allowedMisses ) : base( allowedMisses )
        {
        }

        public override void Expose( Player player )
        {
            foreach (var mod in FilterMods(  player ) )
            {
                mod.ExposeAllSecondaries( player );
                if ( mod.Speed != null )
                {
                    mod.LevelTo( player, 12 );
                }
            }
        }

        public override List<Mod> FilterMods( Player player, bool includeMats = true )
        {
            var workingSet =  base.FilterMods( player, includeMats );
            workingSet.RemoveAll( m => m.Tier == Tier.E );
            return workingSet;
        }

        public override Mod ChooseModToSlice( Player player )
        {
            var workingSet = FilterMods( player );

            var mod = workingSet.OrderByDescending( m => m.Speed.Rolls ).ThenBy( m => m.Tier ).FirstOrDefault();

            if ( mod == null )
                return null;

           // mod.Slice( player );
            return mod;
        }
    }
}
