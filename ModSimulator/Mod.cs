using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator
{
    public enum Slot { Square, Diamond, Circle, Arrow, Triangle, Cross }


    public enum Stats
    {
        AccuracyPercent, CritAvoidance, CritChance, CritDamage, DefensePercent, HealthPercent, OffencePercent, Potency, ProtectionPercent, Speed, Tenacity
    , Defense, Health, Offense, Protection
    }

    public enum Rarity { E, D, C, B, A }


    public class Secondary
    {
        public Stats Stat { get; set; }
        public int Rolls { get; set; }
        public int Amount { get; set; }

    }
    public class Mod
    {
        private static Random rnd => new Random();

        public Slot Slot { get; set; }
        public Stats Primary { get; set; }
        public List<Secondary> Secondaries { get; set; }=  new List<Secondary>( 4 );
        public int Level { get; set; }
        public int Stars { get; set; }
        public Rarity Rarity { get; set; }

        public static Mod RollNew()
        {
            var mod = new Mod
            {
                Slot = (Slot)rnd.Next( 6 ),
                Stars = 5,
                Rarity = (Rarity)rnd.Next( 5 ),
            };

            mod.RollPrimary();
            mod.RollInitialSecondaries();

            return mod;
        }

        private void RollInitialSecondaries()
        {
            int numberOfInitialSecondaries = (int)Rarity;

            for ( int i = 0; i < numberOfInitialSecondaries; i++ )
            {
                RollNewSecondary();
            }

        }

        public Stats RollPrimary()
        {
            var CirclePrimaries = new Stats[] { Stats.HealthPercent, Stats.ProtectionPercent };
            var ArrowPrimaries = new Stats[] { Stats.Speed, Stats.HealthPercent, Stats.ProtectionPercent, Stats.DefensePercent, Stats.OffencePercent, Stats.AccuracyPercent, Stats.CritAvoidance };
            var TrianglePrimaries = new Stats[] { Stats.HealthPercent, Stats.ProtectionPercent, Stats.OffencePercent, Stats.CritChance, Stats.CritDamage };
            var CrossPrimaries = new Stats[] { Stats.HealthPercent, Stats.ProtectionPercent, Stats.DefensePercent, Stats.OffencePercent, Stats.Potency, Stats.Tenacity };


            switch ( Slot )
            {
                case Slot.Square:
                    Primary = Stats.OffencePercent;
                    break;
                case Slot.Diamond:
                    Primary = Stats.Defense;
                    break;
                case Slot.Circle:
                    Primary = CirclePrimaries[rnd.Next( CirclePrimaries.Length )];
                    break;
                case Slot.Arrow:
                    Primary = ArrowPrimaries[rnd.Next( ArrowPrimaries.Length )];
                    break;
                case Slot.Cross:
                    Primary = CrossPrimaries[rnd.Next( CrossPrimaries.Length )];
                    break;
                case Slot.Triangle:
                    Primary = TrianglePrimaries[rnd.Next( TrianglePrimaries.Length )];
                    break;

            }

            return Primary;


        }


        public Secondary RollNewSecondary()
        {
            var SecondaryStats = new Stats[] { Stats.CritChance, Stats.Defense, Stats.DefensePercent, Stats.Health, Stats.HealthPercent, Stats.OffencePercent, Stats.Offense, Stats.Potency, Stats.Protection, Stats.ProtectionPercent, Stats.Speed, Stats.Tenacity };

            if ( Secondaries.Count >= 4 )
                return null;

            while ( true )
            {
                var potentialStat = SecondaryStats[rnd.Next( SecondaryStats.Length )];

                if ( potentialStat == Primary )
                    continue;

                if ( Secondaries.Any( s => s.Stat == potentialStat ) )
                    continue;

                var secondary = new Secondary
                {
                    Stat = potentialStat,
                    Rolls = 1,
                };
                Secondaries.Add( secondary );
                return secondary;
            }
        }

        public void LevelUp()
        {
            if ( Level >= 15 )
                return;

            Level++;
            if ( Level == 3 || Level == 6 || Level == 9 || Level == 12 )
            {
                if ( Secondaries.Count < 4 )
                    RollNewSecondary();
                else
                    RollSecondary();
            }
        }

        private void RollSecondary()
        {
            while ( true )
            {
                var secondaryToRoll = Secondaries[rnd.Next( 4 )];
                if ( secondaryToRoll.Rolls >= 5 )
                    continue;

                secondaryToRoll.Rolls++;
                return;
            }

        }

        public void ExposeAllSecondaries()
        {
            while (Secondaries.Count<4)
            {
                LevelUp();
            }
        }

        public void LevelToMax()
        { 
            while(Level<15)
            { 
                LevelUp();
            }
        }




    }

}

