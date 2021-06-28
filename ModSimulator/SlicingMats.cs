using System;
using System.Collections.Generic;

namespace ModSimulator
{
    public enum SlicingMats
    {
        Credits,
        Mk1BondingPin, Mk1FusionDisc, Mk1PowerFlowControlChip, Mk1FusionCoil, M1Amplifier, Mk1Capacitor, Mk2PulseModulator,
        Mk2CircuitBreaker, Mk2ThermalExchange, Mk2VariableResistor, Mk2Microprocessor, 
    }


    public class MatCost
    {
        public MatCost( SlicingMats mat, int amount )
        {
            Mat = mat;
            Amount = amount;
        }

        public SlicingMats Mat { get; set; }
        public long Amount { get; set; }

    }

    public class SlicingCost
    {
        public int Rarity { get; set; }
        public Tier Tier { get; set; }


        public List<MatCost> Mats { get; set; } = new List<MatCost>();



        public static List<SlicingCost> CostTable = new List<SlicingCost>
        {
            new SlicingCost {Rarity=5, Tier=Tier.E, Mats= new List<MatCost>{new MatCost(SlicingMats.Credits, 18000), new MatCost(SlicingMats.Mk1BondingPin, 10) } },
            new SlicingCost {Rarity=5, Tier=Tier.D, Mats= new List<MatCost>{new MatCost(SlicingMats.Credits, 36000), new MatCost(SlicingMats.Mk1BondingPin, 5), new MatCost( SlicingMats.Mk1FusionDisc, 15 ) } },
            new SlicingCost {Rarity=5, Tier=Tier.C, Mats= new List<MatCost>{new MatCost(SlicingMats.Credits, 63000), new MatCost( SlicingMats.Mk1FusionDisc, 10 ), new MatCost( SlicingMats.Mk1PowerFlowControlChip, 25 ) } },
            new SlicingCost {Rarity=5, Tier=Tier.B, Mats= new List<MatCost>{new MatCost(SlicingMats.Credits, 90000), new MatCost( SlicingMats.Mk1PowerFlowControlChip, 15 ), new MatCost( SlicingMats.Mk1FusionCoil, 35 ) } },
            new SlicingCost {Rarity=5, Tier=Tier.A, Mats= new List<MatCost>{new MatCost(SlicingMats.Credits, 200000), new MatCost( SlicingMats.M1Amplifier, 50 ), new MatCost( SlicingMats.Mk1Capacitor, 50 ), new MatCost( SlicingMats.Mk2PulseModulator, 20 ) } },


        };

        public static List<int> LevelingCost = new List<int> { 3450, 6900, 10300, 13800, 18400, 24100, 29900, 37900, 48300, 58600, 86200, 121900, 157500, 248400 };
    }
}

