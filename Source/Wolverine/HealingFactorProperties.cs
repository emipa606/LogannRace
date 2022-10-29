using Verse;

namespace Wolverine;

public class HealingFactorProperties : DefModExtension
{
    public int healOldWoundStage = 4;
    public float healWoundFactorMax = 5.00f;

    public float healWoundFactorMin = 0.01f;
    public int regrowBodypartStage = 3;
    public int tendWoundStage = 2;

    public int ticksBetweenMajorHeal = 2000;
    public int ticksBetweenMinorHeal = 20;
}