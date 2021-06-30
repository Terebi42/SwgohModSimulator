using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public class SellAllGreysStrategy : BaseModFarmingStrategy, IModFarmingStrategy
    {
        public SellAllGreysStrategy( int allowedMisses ) : base( allowedMisses )
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

        public override List<Mod> FilterMods( Player player )
        {
            var workingSet =  base.FilterMods( player );
            workingSet.RemoveAll( m => m.Tier == Tier.E );
            return workingSet;
        }

        public override Mod ChooseModToSlice( Player player )
        {
            var workingSet = FilterMods( player );

            var mod = workingSet.OrderBy( m => m.Tier ).FirstOrDefault();

            if ( mod == null )
                return null;

           // mod.Slice( player );
            return mod;
        }

        public override void TrashMods( Player player )
        {
            base.TrashMods( player );
            player.Mods.RemoveAll( m => m.Tier == Tier.E );
        }
    }
}
