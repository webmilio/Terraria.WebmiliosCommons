﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using WebmilioCommons.Commons.Players;
using WebmilioCommons.Extensions;
using WebmilioCommons.Transformations;

namespace WebmilioCommons.Players
{
    public partial class WCPlayer
    {
        internal readonly List<Type> activeTransformationTypes = new List<Type>();
        internal readonly List<PlayerTransformation> activeTransformations = new List<PlayerTransformation>();


        public bool IsTransformed<T>() where T : PlayerTransformation => activeTransformationTypes.Contains(typeof(T));
        public bool IsTransformed(PlayerTransformation transformation) => activeTransformations.Contains(transformation);


        public bool Transform<T>() where T : PlayerTransformation => Transform(Activator.CreateInstance<T>());

        public bool Transform(PlayerTransformation transformation)
        {
            transformation.Player = player;
            transformation.WCPlayer = this;

            if (transformation.Unique && IsTransformed(transformation) ||
                !transformation.PreTransform() || !activeTransformations.TrueForAll(t => t.PreAnyTransform(this, player, transformation)))
                return false;

            activeTransformations.Add(transformation);
            activeTransformationTypes.Add(transformation.GetType());

            transformation.PostTransform();
            activeTransformations.Do(t => t.PostAnyTransform(this, player, transformation));

            return true;
        }


        /// <summary>Removes all transformation from the player.</summary>
        public void DeTransform() => activeTransformations.DoInverted((transformation, i) => DoDeTransform(i));

        public bool DeTransform<T>() where T :PlayerTransformation
        {
            for (int i = 0; i < activeTransformations.Count; i++)
                if (activeTransformations[i] is T)
                    return DoDeTransform(i);

            return false;
        }

        public bool DeTransform(PlayerTransformation transformation)
        {
            for (int i = 0; i < activeTransformations.Count; i++)
                if (activeTransformations[i] == transformation)
                    return DoDeTransform(i);

            return false;
        }


        private bool DoDeTransform(int transformationIndex, bool death = false)
        {
            var transformation = activeTransformations[transformationIndex];

            if (!transformation.PreDeTransform(death) || !activeTransformations.TrueForAll(t => t.PreAnyDeTransform(this, player, transformation, death)))
                return false;

            activeTransformations.RemoveAt(transformationIndex);
            activeTransformationTypes.RemoveAt(transformationIndex);

            transformation.PostDeTransform(death);
            activeTransformations.Do(t => t.PostAnyDeTransform(this, player, transformation, death));

            return true;
        }


        #region Hooks

        private void UpdateBadLifeRegenTransformation()
        {
            if (!activeTransformations.TrueForAll(t => t.PreUpdateBadLifeRegen()))
                return;

            activeTransformations.Do(t => t.UpdateBadLifeRegen());
        }

        private void UpdateLifeRegenTransformation()
        {
            if (!activeTransformations.TrueForAll(t => t.PreUpdateLifeRegen()))
                return;

            activeTransformations.Do(t => t.UpdateLifeRegen());
        }

        private void UpdateDeadTransformation()
        {
            activeTransformations.DoInverted((t, i) =>
            {
                if (t.DeTransformOnDeath())
                    DoDeTransform(i, true);
            });
        }

        #endregion
    }
}