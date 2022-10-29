using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Wolverine.Logic;

/// <summary>
///     Utility class for dealing with Pawn body parts.
/// </summary>
public static class BodypartUtility
{
    /// <summary>
    ///     Gets the first matching bodypart in each bodypart tree node.
    /// </summary>
    /// <param name="pawn">Pawn to look in.</param>
    /// <param name="startingPart">Initial bodypart to start from.</param>
    /// <param name="hediffDef">HediffDef to look for.</param>
    /// <returns>Matching bodyparts.</returns>
    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef)
    {
        //Hediff list from pawn.
        var hediffs = pawn.health.hediffSet.hediffs;

        //Working lists for recursive iteration.
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            //Seed part.
            startingPart
        };

        do
        {
            //Initialize current set.
            currentSet.AddRange(nextSet);
            nextSet.Clear();

            foreach (var part in currentSet)
            {
                var matchingPart = false;

                //Iterate through all Hediffs.
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    //Current Hediff to inspect.
                    var hediff = hediffs[i];

                    //Matching BodyPartRecord and Hediff
                    if (hediff.Part != part || hediff.def != hediffDef)
                    {
                        continue;
                    }

                    //Matching part.
                    matchingPart = true;

                    yield return part;
                }

                //If no matching parts found on this level, continue on next.
                if (matchingPart)
                {
                    continue;
                }

                //Add next set.
                foreach (var bodyPartRecord in part.parts)
                {
                    nextSet.Add(bodyPartRecord);
                }
            }

            //Clear current set.
            currentSet.Clear();
        } while (nextSet.Count > 0); //Do while we got next set of parts to go through.
    }

    /// <summary>
    ///     Gets the first matching bodypart in each bodypart tree node.
    /// </summary>
    /// <param name="pawn">Pawn to look in.</param>
    /// <param name="startingPart">Initial bodypart to start from.</param>
    /// <param name="hediffDef">HediffDef to look for.</param>
    /// <param name="hediffExceptionDef">HediffDef to stop at. Returns no bodypart.</param>
    /// <returns>Matching bodyparts.</returns>
    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef, HediffDef hediffExceptionDef)
    {
        //Hediff list from pawn.
        var hediffs = pawn.health.hediffSet.hediffs;

        //Working lists for recursive iteration.
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            //Seed part.
            startingPart
        };

        do
        {
            //Initialize current set.
            currentSet.AddRange(nextSet);
            nextSet.Clear();

            foreach (var part in currentSet)
            {
                var matchingPart = false;

                //Iterate through all Hediffs.
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    //Current Hediff to inspect.
                    var hediff = hediffs[i];

                    //Matching BodyPartRecord and Hediff
                    if (hediff.Part != part)
                    {
                        continue;
                    }

                    if (hediff.def == hediffExceptionDef)
                    {
                        //Matching part. As an exception.
                        matchingPart = true;
                        break;
                    }

                    if (hediff.def != hediffDef)
                    {
                        continue;
                    }

                    //Matching part.
                    matchingPart = true;

                    yield return part;
                    break;
                }

                //If no matching parts found on this level, continue on next.
                if (matchingPart)
                {
                    continue;
                }

                //Add next set.
                foreach (var bodyPartRecord in part.parts)
                {
                    nextSet.Add(bodyPartRecord);
                }
            }

            //Clear current set.
            currentSet.Clear();
        } while (nextSet.Count > 0); //Do while we got next set of parts to go through.
    }

    /// <summary>
    ///     Gets the first matching bodypart in each bodypart tree node.
    /// </summary>
    /// <param name="pawn">Pawn to look in.</param>
    /// <param name="startingPart">Initial bodypart to start from.</param>
    /// <param name="hediffDef">HediffDef to look for.</param>
    /// <param name="hediffExceptionDef">HediffDef to stop at. Returns no bodypart.</param>
    /// <param name="extraExceptionPredicate">Let you do an extra check to make an exception on a bodypart.</param>
    /// <returns>Matching bodyparts.</returns>
    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef, HediffDef hediffExceptionDef, Predicate<Hediff> extraExceptionPredicate)
    {
        //Hediff list from pawn.
        var hediffs = pawn.health.hediffSet.hediffs;

        //Working lists for recursive iteration.
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            //Seed part.
            startingPart
        };

        do
        {
            //Initialize current set.
            currentSet.AddRange(nextSet);
            nextSet.Clear();

            foreach (var part in currentSet)
            {
                var matchingPart = false;

                //Iterate through all Hediffs.
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    //Current Hediff to inspect.
                    var hediff = hediffs[i];

                    //Matching BodyPartRecord and Hediff
                    if (hediff.Part != part)
                    {
                        continue;
                    }

                    if (hediff.def == hediffExceptionDef || extraExceptionPredicate(hediff))
                    {
                        //Matching part. As an exception.
                        matchingPart = true;
                        break;
                    }

                    if (hediff.def != hediffDef)
                    {
                        continue;
                    }

                    //Matching part.
                    matchingPart = true;

                    yield return part;
                    break;
                }

                //If no matching parts found on this level, continue on next.
                if (matchingPart)
                {
                    continue;
                }

                //Add next set.
                foreach (var bodyPartRecord in part.parts)
                {
                    nextSet.Add(bodyPartRecord);
                }
            }

            //Clear current set.
            currentSet.Clear();
        } while (nextSet.Count > 0); //Do while we got next set of parts to go through.
    }

    /// <summary>
    ///     Gets the first matching bodypart in each bodypart tree node.
    /// </summary>
    /// <param name="pawn">Pawn to look in.</param>
    /// <param name="startingPart">Initial bodypart to start from.</param>
    /// <param name="hediffDef">HediffDef to look for.</param>
    /// <param name="hediffExceptionDefs">HediffDefs to stop at. Returns no bodypart.</param>
    /// <returns>Matching bodyparts.</returns>
    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef, HediffDef[] hediffExceptionDefs)
    {
        //Hediff list from pawn.
        var hediffs = pawn.health.hediffSet.hediffs;

        //Working lists for recursive iteration.
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            //Seed part.
            startingPart
        };

        do
        {
            //Initialize current set.
            currentSet.AddRange(nextSet);
            nextSet.Clear();

            foreach (var part in currentSet)
            {
                var matchingPart = false;

                //Iterate through all Hediffs.
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    //Current Hediff to inspect.
                    var hediff = hediffs[i];

                    //Matching BodyPartRecord and Hediff
                    if (hediff.Part != part)
                    {
                        continue;
                    }

                    if (hediffExceptionDefs.Contains(hediff.def))
                    {
                        //Matching part. As an exception.
                        matchingPart = true;
                        break;
                    }

                    if (hediff.def != hediffDef)
                    {
                        continue;
                    }

                    //Matching part.
                    matchingPart = true;

                    yield return part;
                    break;
                }

                //If no matching parts found on this level, continue on next.
                if (matchingPart)
                {
                    continue;
                }

                //Add next set.
                foreach (var bodyPartRecord in part.parts)
                {
                    nextSet.Add(bodyPartRecord);
                }
            }

            //Clear current set.
            currentSet.Clear();
        } while (nextSet.Count > 0); //Do while we got next set of parts to go through.
    }

    /// <summary>
    ///     Gets the first matching bodypart in each bodypart tree node.
    /// </summary>
    /// <param name="pawn">Pawn to look in.</param>
    /// <param name="startingPart">Initial bodypart to start from.</param>
    /// <param name="hediffDefs">HediffDefs to look for.</param>
    /// <returns>Matching bodyparts.</returns>
    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef[] hediffDefs)
    {
        //Hediff list from pawn.
        var hediffs = pawn.health.hediffSet.hediffs;

        //Working lists for recursive iteration.
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            //Seed part.
            startingPart
        };

        do
        {
            //Initialize current set.
            currentSet.AddRange(nextSet);
            nextSet.Clear();

            foreach (var part in currentSet)
            {
                var matchingPart = false;

                //Iterate through all Hediffs.
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    //Current Hediff to inspect.
                    var hediff = hediffs[i];

                    //Matching BodyPartRecord and Hediff
                    if (hediff.Part != part || !hediffDefs.Contains(hediff.def))
                    {
                        continue;
                    }

                    //Matching part.
                    matchingPart = true;

                    yield return part;
                    break;
                }

                //If no matching parts found on this level, continue on next.
                if (matchingPart)
                {
                    continue;
                }

                //Add next set.
                foreach (var bodyPartRecord in part.parts)
                {
                    nextSet.Add(bodyPartRecord);
                }
            }

            //Clear current set.
            currentSet.Clear();
        } while (nextSet.Count > 0); //Do while we got next set of parts to go through.
    }

    /// <summary>
    ///     Replaces all Hediffs with another one at the same position starting from a bodypart.
    /// </summary>
    /// <param name="pawn">Pawn to look in.</param>
    /// <param name="startingPart">Initial bodypart to start from.</param>
    /// <param name="hediffDef">HediffDefs to look for.</param>
    /// <param name="replaceWithDef">HediffDef to replace with.</param>
    public static void ReplaceHediffFromBodypart(this Pawn pawn, BodyPartRecord startingPart, HediffDef hediffDef,
        HediffDef replaceWithDef)
    {
        //Hediff list from pawn.
        var hediffs = pawn.health.hediffSet.hediffs;

        //Working lists for recursive iteration.
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            //Seed part.
            startingPart
        };

        do
        {
            //Initialize current set.
            currentSet.AddRange(nextSet);
            nextSet.Clear();

            foreach (var part in currentSet)
            {
                //Iterate through all Hediffs.
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    //Current Hediff to inspect.
                    var hediff = hediffs[i];

                    //Matching BodyPartRecord and Hediff
                    if (hediff.Part != part || hediff.def != hediffDef)
                    {
                        continue;
                    }

                    //Remove Hediff.
                    var hediff2 = hediffs[i];
                    hediffs.RemoveAt(i);
                    hediff2.PostRemoved();

                    //Add replacing Hediff.
                    var newHeDiff = HediffMaker.MakeHediff(replaceWithDef, pawn, part);
                    hediffs.Insert(i, newHeDiff);
                }

                //Add next set.
                foreach (var bodyPartRecord in part.parts)
                {
                    nextSet.Add(bodyPartRecord);
                }
            }

            //Clear current set.
            currentSet.Clear();
        } while (nextSet.Count > 0); //Do while we got next set of parts to go through.
    }
}