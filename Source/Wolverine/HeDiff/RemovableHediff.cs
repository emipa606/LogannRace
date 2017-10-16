using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Wolverine
{
    /// <summary>
    /// Temporary Hediff for replacing Hediffs like MissingPartHeDiff.
    /// </summary>
    public class RemovableHediff : Hediff
    {
        public override bool ShouldRemove => true;
    }
}
