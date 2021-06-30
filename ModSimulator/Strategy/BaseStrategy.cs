using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator.Strategy
{
    public interface IModFarmingStrategy
    {
        Mod ChooseModToSlice( Player player );
        void Expose( Player player );
        List<Mod> FilterMods( Player player, bool includeMats = true );

        void TrashMods( Player player );
    }
    public abstract class BaseModFarmingStrategy : IModFarmingStrategy
    {
        public int AllowedMisses { get; }

        protected BaseModFarmingStrategy( int allowedMisses )
        {
            AllowedMisses = allowedMisses;
        }

        public virtual void Expose( Player player )
        {
            foreach ( var mod in player.Mods )
            {
                mod.ExposeAllSecondaries( player );
                if ( mod.Speed != null )
                {
                    mod.LevelTo( player, 12 );
                }
            }
        }

        public abstract Mod ChooseModToSlice( Player player );

        public virtual List<Mod> FilterMods( Player player, bool includeMats = true )
        {
            var workingSet = player.Mods.ToArray().ToList();

            
            workingSet.RemoveAll( m => m.Secondaries.Count == 4 && m.Speed == null );
            workingSet.RemoveAll( m => m.Secondaries.Count == 4 && m.Speed.Rolls + AllowedMisses < (int)m.Tier ); //Green < 1 rolls, Blue < 2, etc
            workingSet.RemoveAll( m => m.Speed?.Rolls >= 5 ); //dont roll past 5 speed rolls

            if ( includeMats )
            {
                workingSet.RemoveAll( m => !m.CanBeSlicedBy( player ) ); //Do this one later because it calculates on every mod
            }

            return workingSet;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Allowed Misses = {AllowedMisses}";
        }

        public virtual void TrashMods( Player player )
        {
            player.Mods.RemoveAll( m => m.Secondaries.Count == 4 && m.Speed == null );
        }
    }
}
