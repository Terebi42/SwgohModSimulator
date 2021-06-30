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


    public class Secondary
    {
        public Stats Stat { get; set; }
        public int Rolls { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"{Stat}({Rolls}) = {Amount}";
        }

    }
    public class Mod
    {
        private static Random rnd => new Random();

        public Slot Slot { get; set; }
        public Stats Primary { get; set; }
        public List<Secondary> Secondaries { get; set; } = new List<Secondary>( 4 );
        public Secondary Speed => Secondaries.FirstOrDefault( s => s.Stat == Stats.Speed );
        public int Level { get; set; }
        public int Rarity { get; set; }
        public Tier Tier { get; set; }
        public int Slices { get; set; }

        public override string ToString()
        {
            return $"{Rarity}{Tier}{Level} {Slot} {Primary} Speed({Speed.Rolls}) Slices={Slices}";
        }

        public static Mod RollNew()
        {
            var mod = new Mod
            {
                Slot = (Slot)rnd.Next( 6 ),
                Rarity = 5,
                
                Level = 1
            };
            mod.RollTier();
            mod.RollPrimary();
            mod.RollInitialSecondaries();

            return mod;
        }

        private void RollTier()
        {
           // Tier = (Tier)rnd.Next( 5 );

            var chance = rnd.Next( 1000 );

            if ( chance < 639 )
                Tier = Tier.E;
            else if ( chance < 836 )
                Tier = Tier.D;
            else if ( chance < 941 )
                Tier = Tier.C;
            else if ( chance < 978 )
                Tier = Tier.B;
            else
                Tier = Tier.A;


        }

        private void RollInitialSecondaries()
        {
            int numberOfInitialSecondaries = (int)Tier;

            for ( int i = 0; i < numberOfInitialSecondaries; i++ )
            {
                RollNewSecondary();
            }

        }

        public SlicingCost SlicingCost => SlicingCost.CostTable.FirstOrDefault( ct => ct.Rarity == Rarity && ct.Tier == Tier );


        public int LevelCost => 0;// Level <= 14 ? SlicingCost.LevelingCost[Level - 1] : 0;

        public bool CanBeSlicedBy( Player player )
        {
            if ( Rarity >= 6 )
                return false; //Dont do 6e+ mods for now

            foreach ( var cost in SlicingCost.Mats )
            {
                var playerMat = player.Mats.FirstOrDefault( pm => pm.Mat == cost.Mat );
                if ( cost.Amount > playerMat.Amount )
                    return false;
            }
            return true;
        }

        public void Slice( Player player )
        {
            if ( Level < 15 )
                LevelTo( player, 15 );

            if ( Tier == Tier.A && Rarity == 6 )
                return;

            if ( Rarity == 6 )
                return; //Don't to 6e+ slicing for now

            if ( !CanBeSlicedBy( player ) )
            {
                return;
            }

            var cost = SlicingCost;

            if ( Tier == Tier.A && Rarity == 5 )//Slice to 6e
            {
                foreach ( var secondary in Secondaries )
                {

                    //secondary.Rolls++; //Slicing from 5a to 6e gives a bonus to all stats, but does not "roll"
                    secondary.Amount++; //TODO: Make this add the proper amounts for 6e for each stat

                }

                Tier = Tier.E;
                Rarity = 6;
                Slices++;
            }
            else
            {

                //Else its a regular slice
                Tier = (Tier)( Tier + 1 );
                RollToIncreaseSecondary();
                Slices++;
            }

            if ( player != null )
            {
                foreach ( var matCost in cost.Mats )
                {
                    var playerMat = player.Mats.FirstOrDefault( pm => pm.Mat == matCost.Mat );
                    playerMat.Amount -= matCost.Amount;
                }
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

        public void LevelUp( Player player )
        {
            if ( Level >= 15 )
                return;

            if ( player != null && LevelCost <= player.Credits.Amount )
            {
                player.Credits.Amount -= LevelCost;
            }


            Level++;


            if ( Level == 3 || Level == 6 || Level == 9 || Level == 12 )
            {
                if ( Secondaries.Count < 4 )
                    RollNewSecondary();
                else
                    RollToIncreaseSecondary();
            }
        }

        private void RollToIncreaseSecondary()
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

        public void ExposeAllSecondaries( Player player )
        {
            while ( Secondaries.Count < 4 )
            {
                LevelUp( player );
            }
        }


        public void LevelTo( Player player, int target )
        {
            while ( Level < target )
            {
                LevelUp( player );
            }
        }




    }

}

