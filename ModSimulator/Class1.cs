using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator
{
    public enum Slot { Square, Diamond, Circle, Arrow, Triangle, Cross }


    public enum Stats {
        AccuracyPercent, CritAvoidance, CritChance, CritDamage, DefensePercent, HealthPercent, OffencePercent, Potency, ProtectionPercent, Speed, Tenacity
    , Defense, Health, Offense, Protection }

    public enum Rarity { A, B, C, D, E}
            

public class Secondary
{
    public Stats Stat { get; set; }
    public int Rolls { get; set; }
    public int Amount { get; set; }
    public int Stars { get; set; }
}
    public class Mod
    {
        private static Random rnd => new Random();

        public Slot Slot { get; set; }
        public Stats Primary { get; set; }
        public Secondary[] Secondaries => new Secondary[4];
        public int Level { get; set; }

        public static Mod RollNew()
        {
            var mod = new Mod
            { 
             Slot = (Slot) rnd.Next(6),
             Stars = 5,
             Rarity = (Rarity) rnd.Next (5),



            };

            RollPrimary();
        }

        public RollPrimary()
        {
            var CirclePrimaries = new Stats[] { Stats.HealthPercent, Stats.ProtectionPercent }
            var ArrowPrimaries = new Stats[] { Stats.Speed, Stats.HealthPercent, Stats.ProtectionPercent, Stats.DefensePercent, Stats.OffencePercent, Stats.AccuracyPercent, Stats.CritAvoidance };
            var TrianglePrimaries = new Stats[] {Stats.HealthPercent, Stats.ProtectionPercent, Stats.OffencePercent, Stats.CritChance, Stats.CritDamage}
            var CrossPrimaries = new Stats[] {Stats.HealthPercent, Stats.ProtectionPercent, Stats.DefensePercent, Stats.OffencePercent, Stats.Potency, Stats.Tenacity}


            switch ( Slot )
            {
                case Slot.Square: Primary = Stats.OffencePercent; break;
                case Slot.Diamond: Primary = Stats.Defense; break;
                case Slot.Circle: Primary = (Stats)rng.Next( CirclePrimaries.Length );break;
                case Slot.Arrow:
                    Primary = (Stats)rng.Next( ArrowPrimaries.Length );
                    break;
                case Slot.Cross:
                    Primary = (Stats)rng.Next( CrossPrimaries.Length );
                    break;
                case Slot.Triangle:
                    Primary = (Stats)rng.Next( TrianglePrimaries.Length );
                    break;

            }


        }


        public void RollSecondary()
        {
            if ( Secondaries.All( s => s != null )
                return;


        }

    }
}
