﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Taiko.Objects.Drawables
{
    public class DrawableDrumRollTick : DrawableTaikoHitObject<DrumRollTick>
    {
        public DrawableDrumRollTick(DrumRollTick tick)
            : base(tick)
        {
            FillMode = FillMode.Fit;
        }

        public override bool DisplayResult => false;

        protected override TaikoPiece CreateMainPiece() => new TickPiece
        {
            Filled = HitObject.FirstTick
        };

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (timeOffset > HitObject.HitWindow)
                    ApplyResult(r => r.Type = HitResult.Miss);
                return;
            }

            if (Math.Abs(timeOffset) > HitObject.HitWindow)
                return;

            ApplyResult(r => r.Type = HitResult.Great);
        }

        protected override void UpdateState(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    this.ScaleTo(0, 100, Easing.OutQuint).Expire();
                    break;
            }
        }

        public override bool OnPressed(TaikoAction action) => UpdateResult(true);

        protected override DrawableStrongHandler CreateStrongHandler(StrongHitObject hitObject) => new StrongHandler(hitObject, this);

        private class StrongHandler : DrawableStrongHandler
        {
            public StrongHandler(StrongHitObject strong, DrawableDrumRollTick tick)
                : base(strong, tick)
            {
            }

            protected override void CheckForResult(bool userTriggered, double timeOffset)
            {
                if (!MainObject.Judged)
                    return;

                ApplyResult(r => r.Type = MainObject.IsHit ? HitResult.Great : HitResult.Miss);
            }

            public override bool OnPressed(TaikoAction action) => false;
        }
    }
}
