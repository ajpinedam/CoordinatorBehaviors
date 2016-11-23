using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;

namespace CoordinatorBehaviors.Behaviors
{
    [Android.Runtime.Register("com.coordinatorbehaviors.AvatarImageBehavior")]
    public class AvatarImageBehavior : CoordinatorLayout.Behavior
    {
        private static float MIN_AVATAR_PERCENTAGE_SIZE = 0.3f;
        private static int EXTRA_FINAL_AVATAR_PADDING = 80;

        private static string TAG = "behavior";
        private Context context;

        private float customFinalYPosition;
        private float customStartXPosition;
        private float customStartToolbarPosition;
        private float customStartHeight;
        private float customFinalHeight;

        private float avatarMaxSize;
        private float finalLeftAvatarPadding;
        private float startPosition;
        private int startXPosition;
        private float startToolbarPosition;
        private int startYPosition;
        private int finalYPosition;
        private int startHeight;
        private int finalXPosition;
        private float changeBehaviorPoint;

        public AvatarImageBehavior(Context Context, IAttributeSet attrs)
        {
            context = Context;

            if (attrs != null)
            {
                TypedArray typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.AvatarImageBehavior);
                customFinalYPosition = typedArray.GetDimension(Resource.Styleable.AvatarImageBehavior_finalYPosition, 0);
                customStartXPosition = typedArray.GetDimension(Resource.Styleable.AvatarImageBehavior_startXPosition, 0);
                customStartToolbarPosition = typedArray.GetDimension(Resource.Styleable.AvatarImageBehavior_startToolbarPosition, 0);
                customStartHeight = typedArray.GetDimension(Resource.Styleable.AvatarImageBehavior_startHeight, 0);
                customFinalHeight = typedArray.GetDimension(Resource.Styleable.AvatarImageBehavior_finalHeight, 0);

                typedArray.Recycle();
            }

            Init();

            finalLeftAvatarPadding = context.Resources.GetDimension(
                Resource.Dimension.spacing_normal);
        }

        private void Init()
        {
            BindDimensions();
        }

        private void BindDimensions()
        {
            avatarMaxSize = context.Resources.GetDimension(Resource.Dimension.image_width);
        }

        public override bool LayoutDependsOn(CoordinatorLayout parent, Java.Lang.Object child, View dependency)
        {
            return IsInstanceOf<Android.Support.V7.Widget.Toolbar>(dependency);
        }

        public override bool OnDependentViewChanged(CoordinatorLayout parent, Java.Lang.Object paramChild, View dependency)
        {
            var child = paramChild as View;

            MaybeInitProperties(child, dependency);

            
            int maxScrollDistance = (int)(startToolbarPosition);
            float expandedPercentageFactor = dependency.GetY() / maxScrollDistance;

            if (expandedPercentageFactor < changeBehaviorPoint)
            {
                float heightFactor = (changeBehaviorPoint - expandedPercentageFactor) / changeBehaviorPoint;

                float distanceXToSubtract = ((startXPosition - finalXPosition)
                        * heightFactor) + (child.Height / 2);
                float distanceYToSubtract = ((startYPosition - finalYPosition)
                        * (1f - expandedPercentageFactor)) + (child.Height / 2);

                child.SetX(startXPosition - distanceXToSubtract);
                child.SetY(startYPosition - distanceYToSubtract);

                float heightToSubtract = ((startHeight - customFinalHeight) * heightFactor);

                CoordinatorLayout.LayoutParams layoutParameters = (CoordinatorLayout.LayoutParams)child.LayoutParameters;
                layoutParameters.Width = (int)(startHeight - heightToSubtract);
                layoutParameters.Height = (int)(startHeight - heightToSubtract);
                child.LayoutParameters = layoutParameters;
            }
            else
            {
                float distanceYToSubtract = ((startYPosition - finalYPosition)
                        * (1f - expandedPercentageFactor)) + (startHeight / 2);

                child.SetX(startXPosition - child.Width / 2);
                child.SetY(startYPosition - distanceYToSubtract);

                CoordinatorLayout.LayoutParams lp = (CoordinatorLayout.LayoutParams)child.LayoutParameters;
                lp.Width = (int)(startHeight);
                lp.Height = (int)(startHeight);
                child.LayoutParameters = lp;
            }
            return true;
        }

        private void MaybeInitProperties(View paramChild, View dependency)
        {
            var child = paramChild as View;

            if (startYPosition == 0)
                startYPosition = (int)(dependency.GetY());

            if (finalYPosition == 0)
                finalYPosition = (dependency.Height / 2);

            if (startHeight == 0)
                startHeight = child.Height;

            if (startXPosition == 0)
                startXPosition = (int)(child.GetX() + (child.Width / 2));

            if (finalXPosition == 0)
                finalXPosition = context.Resources.GetDimensionPixelOffset(Resource.Dimension.abc_action_bar_content_inset_material) + ((int)customFinalHeight / 2);

            if (startToolbarPosition == 0)
                startToolbarPosition = dependency.GetY();

            if (changeBehaviorPoint == 0)
            {
                changeBehaviorPoint = (child.Height - customFinalHeight) / (2f * (startYPosition - finalYPosition));
            }
        }

        public int GetStatusBarHeight()
        {
            int result = 0;
            int resourceId = context.Resources.GetIdentifier("status_bar_height", "Dimension", "android");

            if (resourceId > 0)
            {
                result = context.Resources.GetDimensionPixelSize(resourceId);
            }
            return result;
        }

        private bool IsInstanceOf<T>(object instance)
        {
            return instance.GetType().IsAssignableFrom(typeof(T));
        }
    }
}