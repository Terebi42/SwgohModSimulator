using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModSimulator
{
    public class Player
    {
        public List<Mod> Mods { get; set; } = new List<Mod>();

        public List<MatCost> Mats { get; set; } = new List<MatCost>();

        public MatCost Credits => Mats.FirstOrDefault( m => m.Mat == SlicingMats.Credits );

    }
}
