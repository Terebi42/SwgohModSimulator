using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModSimulator;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace ModSimulator.Tests
{
    [TestClass]
    public class ModTests
    {

        [TestMethod]
        public void CirclesShouldOnlyRollHealthOrProtection()
        {
            for ( int i = 0; i <= 100; i++ )
            {
                var mod = new Mod()
                {
                    Slot = Slot.Circle
                };
                mod.RollPrimary();
                mod.Primary.Should().Match<Stats>( p => p == Stats.HealthPercent || p == Stats.ProtectionPercent );


            }
        }

        [TestMethod]
        public void SecondaryShouldNotDuplicatePrimary()
        {
            for ( int i = 0; i <= 100; i++ )
            {
                var mod = Mod.RollNew();
                mod.ExposeAllSecondaries(null);
                mod.Secondaries.Should().NotContain( s => s.Stat == mod.Primary );
            }
        }

        [TestMethod]
        public void ExposeAllShouldAlwaysHave4Secondaries()
        {
            for ( int i = 0; i <= 100; i++ )
            {
                var mod = Mod.RollNew();
                mod.ExposeAllSecondaries(null);
                mod.Secondaries.Count.Should().Be( 4 );
            }
        }

    }
}