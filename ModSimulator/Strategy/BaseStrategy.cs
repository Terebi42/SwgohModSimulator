using System;
using System.Collections.Generic;
using System.Text;

namespace ModSimulator.Strategy
{
    public interface IModFarmingStrategy
    {
        Mod ChooseModToSlice( Player player );
        void Expose( Player player );
    }
    public abstract class BaseModFarmingStrategy : IModFarmingStrategy
    {
        public abstract void Expose( Player player );
        public abstract Mod ChooseModToSlice( Player player );
    }
}
