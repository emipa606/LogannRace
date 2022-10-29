using Verse;

namespace Wolverine;

/// <summary>
///     Temporary Hediff for replacing Hediffs like MissingPartHeDiff.
/// </summary>
public class RemovableHediff : Hediff
{
    public override bool ShouldRemove => true;
}