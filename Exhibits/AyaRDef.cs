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
using LBoL.EntityLib.Exhibits;
using JetBrains.Annotations;
using AyaShameimaru.StatusEffects;

namespace AyaShameimaru.Exhibits
{
    public sealed class AyaRDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaR);
        }
        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }
        public override ExhibitSprites LoadSprite()
        {
            // embedded resource folders are separated by a dot
            var folder = "";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }
        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: sequenceTable.Next(typeof(ExhibitConfig)),
                Id: "",
                Order: 10,
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "AyaPlayerUnit",
                LosableType: ExhibitLosableType.DebutLosable,
                Rarity: Rarity.Shining,
                Value1: 4,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Red,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "AyaAccelerationSe" },
                RelativeCards: new List<string>() { "AyaNews" }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(AyaRDef))]
        [UsedImplicitly]
        public sealed class AyaR : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
            }
            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
            {
                if (!Owner.HasStatusEffect<AyaPassiveSeDef.AyaPassiveSe>())
                {
                    yield return new ApplyStatusEffectAction(typeof(AyaPassiveSeDef.AyaPassiveSe), Owner, null, null, null, null, 0f, true);
                }
                yield return new ApplyStatusEffectAction(typeof(AyaAccelerationSeDef.AyaAccelerationSe), Owner, Value1, null, null, null, 0f, true);
            }
        }
    }
}