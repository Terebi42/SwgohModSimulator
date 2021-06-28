using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public class SellAllGreysStrategy : IModFarmingStrategy
    {
        public void Expose( Player player )
        {
            foreach (var mod in player.Mods.Where(m=>m.Tier!=Tier.E))
            {
                mod.ExposeAllSecondaries( player );
                if ( mod.Speed != null )
                {
                    mod.LevelTo( player, 12 );
                }
            }
        }

        public Mod Slice( Player player )
        {
            var workingSet = player.Mods.ToArray().ToList();
            
            workingSet.RemoveAll( m => !m.CanBeSlicedBy( player ) );
            workingSet.RemoveAll( m => m.Speed == null );
            workingSet.RemoveAll( m => m.Tier == Tier.E);
            workingSet.RemoveAll( m => m.Speed.Rolls < (int)m.Tier+1 ); //Green < 2 rolls, Blue < 3, etc

            workingSet.RemoveAll( m => m.Speed.Rolls >=5 ); //dont roll past 5 speed rolls
            var mod = workingSet.OrderBy( m => m.Tier ).FirstOrDefault();

            if ( mod == null )
                return null;

            mod.Slice( player );
            return mod;



        }
    }
}
