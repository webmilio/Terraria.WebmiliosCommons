using System;
using System.Collections.Generic;
using WebmilioCommons.Animations;

namespace WebmilioCommons.Players
{
    public sealed partial class WCPlayer
    {
        private List<PlayerAnimation> _currentAnimations;


        public bool BeginAnimation(PlayerAnimation animation)
        {
            if (_currentAnimations.Find(a => a.UnlocalizedName == animation.UnlocalizedName && a.Unique) == null)
                return false;

            _currentAnimations.Add(animation);
            animation.Begin();

            return true;
        }

        public bool EndAnimation(PlayerAnimation animation)
        {
            if (!_currentAnimations.Contains(animation))
                return false;

            animation.End();
            _currentAnimations.Remove(animation);

            return true;
        }

        public void EndAllAnimations() => ForAllAnimations(animation => EndAnimation(animation));


        public bool HasAnimation(PlayerAnimation animation) => _currentAnimations.Contains(animation);

        public void ForAllAnimations(Action<PlayerAnimation> action)
        {
            for (int i = 0; i < _currentAnimations.Count; i++)
                action(_currentAnimations[i]);
        }


        private void InitializeAnimations()
        {
            _currentAnimations = new List<PlayerAnimation>();
        }
    }
}
