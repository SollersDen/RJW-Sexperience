﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using rjw;




namespace RJWSexperience.Ideology
{
    [HarmonyPatch(typeof(MarriageCeremonyUtility), "Married")]
    public static class Rimworld_Patch_Marriage
    {
        public static void Postfix(Pawn firstPawn, Pawn secondPawn)
        {
            if (firstPawn.IsIncest(secondPawn))
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(VariousDefOf.Incestuos_Marriage, firstPawn.Named(HistoryEventArgsNames.Doer)));
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(VariousDefOf.Incestuos_Marriage, secondPawn.Named(HistoryEventArgsNames.Doer)));
            }
        }


    }

    [HarmonyPatch(typeof(Pawn_RelationsTracker), "SecondaryRomanceChanceFactor")]
    public static class Rimworld_Patch_SecondaryRomanceChanceFactor
    {
        public static void Postfix(Pawn otherPawn, Pawn ___pawn, ref float __result)
        {
            Ideo ideo = ___pawn.Ideo;
            if (ideo != null)
            {
                if (ideo.HasPrecept(VariousDefOf.Incestuos_IncestOnly) && ___pawn.IsIncest(otherPawn))
                {
                    __result *= 8f;
                }
            }
        }
    }

    [HarmonyPatch(typeof(RitualOutcomeEffectWorker_FromQuality), "GiveMemoryToPawn")]
    public static class Rimworld_Patch_GiveMemoryToPawn
    {
        public static bool Prefix(Pawn pawn, ThoughtDef memory, LordJob_Ritual jobRitual)
        {
            if (pawn.IsAnimal()) return false;

            return true;
        }
    }


}
