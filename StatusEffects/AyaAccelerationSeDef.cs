using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace AyaShameimaru.StatusEffects
{
    public sealed class AyaAccelerationSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaAccelerationSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaAccelerationSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: 1,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "AyaEvasionSe" },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(AyaAccelerationSeDef))]
        public sealed class AyaAccelerationSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(Owner.BlockShieldGaining, delegate (BlockShieldEventArgs args)
                {
                    if (args.Type == BlockShieldType.Direct)
                    {
                        return;
                    }
                    ActionCause cause = args.Cause;
                    if (cause == ActionCause.Card || cause == ActionCause.OnlyCalculate || cause == ActionCause.Us)
                    {
                        if (args.Shield != 0f)
                        {
                            args.Shield = Math.Max(args.Shield - Level, 0f);
                        }
                        args.AddModifier(this);
                    }
                });
                ReactOwnerEvent(Owner.TurnStarting, new EventSequencedReactor<UnitEventArgs>(OnOwnerTurnStarting));
            }
            private IEnumerable<BattleAction> OnOwnerTurnStarting(UnitEventArgs args)
            {
                int num = (int)Math.Truncate((float)Level / 4);
                if (num > 0)
                {
                    int num2 = Owner.HasStatusEffect(typeof(AyaEvasionSeDef.AyaEvasionSe)) ? num - Owner.GetStatusEffect(typeof(AyaEvasionSeDef.AyaEvasionSe)).Level : num;
                    if (num2 > 0)
                    {
                        yield return new ApplyStatusEffectAction<AyaEvasionSeDef.AyaEvasionSe>(Owner, num2, null, null, null, 0.2f, true);
                    }
                    else if (num2 < 0)
                    {
                        Owner.GetStatusEffect(typeof(AyaEvasionSeDef.AyaEvasionSe)).Level = num;
                    }
                }
                else if (Owner.HasStatusEffect(typeof(AyaEvasionSeDef.AyaEvasionSe)))
                {
                    yield return new RemoveStatusEffectAction(Owner.GetStatusEffect(typeof(AyaEvasionSeDef.AyaEvasionSe)), false);
                }
            }
        }
    }
}