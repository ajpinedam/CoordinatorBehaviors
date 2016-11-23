using System;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace CoordinatorBehaviors.Animators
{
    public class AvatarTitleAnimator : Java.Lang.Object, AppBarLayout.IOnOffsetChangedListener
    {
        private const float PERCENTAGE_TO_SHOW_TITLE_AT_TOOLBAR = 0.9f;
        private const float PERCENTAGE_TO_HIDE_TITLE_DETAILS = 0.3f;
        private const int ALPHA_ANIMATIONS_DURATION = 200;

        private bool _isTheTitleVisible = false;
        private bool _isTheTitleContainerVisible = true;

        private LinearLayout _titleContainer;
        private TextView _collapsedTitle;

        public void StartAlphaAnimation(View view, long duration, ViewStates visibility)
        {
            AlphaAnimation alphaAnimation = (visibility == ViewStates.Visible)
                ? new AlphaAnimation(0f, 1f)
                : new AlphaAnimation(1f, 0f);

            alphaAnimation.Duration = duration;
            alphaAnimation.FillAfter = true;
            view.StartAnimation(alphaAnimation);
        }

        public void OnOffsetChanged(AppBarLayout appBarLayout, int verticalOffset)
        {
            int maxScroll = appBarLayout.TotalScrollRange;
            float percentage = (float)Math.Abs(verticalOffset) / (float)maxScroll;
            HandleAlphaOnTitle(percentage);
            HandleToolbarTitleVisibility(percentage);
        }

        public void SetExpandedTitleContainer(LinearLayout titleContainer)
        {
            _titleContainer = titleContainer;
        }

        public void SetCollapsedTitleView(TextView title)
        {
            _collapsedTitle = title;
        }

        private void HandleToolbarTitleVisibility(float percentage)
        {
            if (percentage >= PERCENTAGE_TO_SHOW_TITLE_AT_TOOLBAR)
            {
                if (!_isTheTitleVisible)
                {
                    StartAlphaAnimation(_collapsedTitle, ALPHA_ANIMATIONS_DURATION, ViewStates.Visible);
                    _isTheTitleVisible = true;
                }
            }
            else
            {
                if (_isTheTitleVisible)
                {
                    StartAlphaAnimation(_collapsedTitle, ALPHA_ANIMATIONS_DURATION, ViewStates.Invisible);
                    _isTheTitleVisible = false;
                }
            }
        }

        private void HandleAlphaOnTitle(float percentage)
        {
            if (percentage >= PERCENTAGE_TO_HIDE_TITLE_DETAILS)
            {
                if (_isTheTitleContainerVisible)
                {
                    StartAlphaAnimation(_titleContainer, ALPHA_ANIMATIONS_DURATION, ViewStates.Invisible);
                    _isTheTitleContainerVisible = false;
                }
            }
            else
            {
                if (!_isTheTitleContainerVisible)
                {
                    StartAlphaAnimation(_titleContainer, ALPHA_ANIMATIONS_DURATION, ViewStates.Visible);
                    _isTheTitleContainerVisible = true;
                }
            }
        }
    }
}