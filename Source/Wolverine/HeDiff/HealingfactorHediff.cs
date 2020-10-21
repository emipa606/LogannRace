using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Wolverine.Logic;

namespace Wolverine
{
    /// <summary>
    /// HeDiff Worker Class for the Angelium drug.
    /// </summary>
    public class HealingfactorHediff : HediffWithComps
    {
        public int ticksUntilNextMinorHeal;
        public int ticksUntilNextMajorHeal;

        protected HealingFactorProperties healingFactorProps;

        public HealingFactorProperties HealingFactorProps
        {
            get
            {
                if (healingFactorProps == null)
                {
                    //Try get def properties.
                    healingFactorProps = def.GetModExtension<HealingFactorProperties>();

                    //Get default properties.
                    if (healingFactorProps == null)
                        healingFactorProps = new HealingFactorProperties(); 
                }
                
                //Return properties.
                return healingFactorProps;
            }
        }

        public override void PostMake()
        {
            base.PostMake();

            //Set next tick.
            SetNextMajorTick();
            SetNextMinorTick();
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref ticksUntilNextMinorHeal, "ticksUntilNextMinorHeal");
            Scribe_Values.Look(ref ticksUntilNextMajorHeal, "ticksUntilNextMajorHeal");
        }

        public override void Tick()
        {
            base.Tick();

            if (Current.Game.tickManager.TicksGame >= ticksUntilNextMinorHeal)
            {
                //Heal.
                TryHealWounds();

                SetNextMinorTick();
            }

            if (Current.Game.tickManager.TicksGame >= ticksUntilNextMajorHeal)
            {
                //Remove.
                if (CurStageIndex >= HealingFactorProps.healOldWoundStage)
                    TryHealRandomOldWound();

                //Tend.
                if (CurStageIndex >= HealingFactorProps.tendWoundStage)
                    TrySealWounds();

                //Regrow.
                if (CurStageIndex >= HealingFactorProps.regrowBodypartStage)
                    TryRegrowBodyparts();

                //Set next tick.
                SetNextMajorTick();
            }
        }

        /// <summary>
        /// Tries to heal any wounds by a set amount.
        /// </summary>
        public void TryHealWounds()
        {
            IEnumerable<Hediff> wounds = (from hd in pawn.health.hediffSet.hediffs
                                          where hd is Hediff_Injury && !hd.IsTended()
                                          select hd);

            if (wounds != null)
            {
                foreach (Hediff wound in wounds)
                {
                    wound.Severity -= GenMath.LerpDouble(0.0f, def.maxSeverity, HealingFactorProps.healWoundFactorMin, HealingFactorProps.healWoundFactorMax, Severity);
                }
            }
        }

        /// <summary>
        /// Tries to a old wound. Much like Luciferium.
        /// </summary>
        void TryHealRandomOldWound()
        {
            if (!(from hd in pawn.health.hediffSet.hediffs
                  where hd.IsTended()
                  select hd).TryRandomElement(out Hediff hediff))
            {
                return;
            }

            hediff.Severity = 0f;
        }

        /// <summary>
        /// Tries to seal any bleeding wounds on the pawn. Does not patch up blunt wounds.
        /// </summary>
        public void TrySealWounds()
        {
            IEnumerable<Hediff> wounds = (from hd in pawn.health.hediffSet.hediffs
                                          where hd.Bleeding
                                          select hd);

            if(wounds != null)
            {
                foreach(Hediff wound in wounds)
                {
                    if (wound is HediffWithComps hediffWithComps)
                    {
                        HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();

                        //Equivalent to Glitterworld Medicine.
                        hediffComp_TendDuration.tendQuality = 2.0f;                    //Sets the tending quality.
                        hediffComp_TendDuration.tendTicksLeft = Find.TickManager.TicksGame; //Sets the last tend tick.

                        pawn.health.Notify_HediffChanged(wound);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to regrow any bodyparts that fit criteria.
        /// </summary>
        public void TryRegrowBodyparts()
        {
            //Iterate through the whole body from Core.
            //Stop at first and try to regrow there.
            foreach(BodyPartRecord part in pawn.GetFirstMatchingBodyparts(pawn.RaceProps.body.corePart, HediffDefOf.MissingBodyPart, WolverineHediffDefOf.WolverineProtoBodypart, hediff => hediff is Hediff_AddedPart))
            {
                //Get the bodypart it is on.
                Hediff missingHediff = pawn.health.hediffSet.hediffs.First(hediff => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart);

                if(missingHediff != null)
                {
                    //Remove the missing body part.
                    pawn.health.RemoveHediff(missingHediff);

                    //Insert fake body part.
                    pawn.health.AddHediff(WolverineHediffDefOf.WolverineProtoBodypart, part);
                    pawn.health.hediffSet.DirtyCache();
                }
            }
        }

        /// <summary>
        /// Sets the next major heal tick.
        /// </summary>
        public void SetNextMajorTick()
        {
            //Set next tick to heal.
            ticksUntilNextMajorHeal = Current.Game.tickManager.TicksGame + HealingFactorProps.ticksBetweenMajorHeal;
        }

        /// <summary>
        /// Sets the next major heal tick.
        /// </summary>
        public void SetNextMinorTick()
        {
            //Set next tick to heal.
            ticksUntilNextMinorHeal = Current.Game.tickManager.TicksGame + HealingFactorProps.ticksBetweenMinorHeal;
        }
    }
}
