using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Wolverine
{
    public class HealingFactorProperties : DefModExtension
    {
        public int tendWoundStage = 2;
        public int regrowBodypartStage = 3;
        public int healOldWoundStage = 4;

        public float healWoundFactorMin = 0.01f;
        public float healWoundFactorMax = 5.00f;

        public int ticksBetweenMajorHeal = 2000;
        public int ticksBetweenMinorHeal = 20;
        

    }
}
