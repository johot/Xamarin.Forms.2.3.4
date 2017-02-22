using System;
using System.ComponentModel;
using System.Linq;

#if FORMS
using NativeView = Xamarin.Forms.View;

namespace Xamarin.Forms
#else
namespace Xamarin.FlexLayout
#endif
{
    public partial class FlexLayout
    {
        const string _FlexOrderPropertyName = "Order";
        const string _FlexPropertyName = "Flex";
        const string _FlexGrowPropertyName = "Grow";
        const string _FlexShrinkPropertyName = "Shrink";
        const string _FlexBasisPropertyName = "Basis";
        const string _FlexAlignSelfPropertyName = "AlignSelf";
        const string _FlexIsIncludedPropertyName = "IsIncluded";
        const string _FlexNodePropertyName = "Node";

        IFlexNode _root;
        public FlexLayout()
        {
            InitNode();
        }
        public void ApplyLayout(double x, double y, double width, double height)
        {
            _root.Left = (float)x;
            _root.Top = (float)y;

            AttachNodesFromViewHierachy(this);
            CalculateLayoutWithSize((float)width, (float)height);
            ApplyLayoutToViewHierarchy(this);
        }


        static float SanitizeMeasurement(float constrainedSize, float measuredSize, FlexMeasureMode measureMode)
        {
            float result;
            if (measureMode == FlexMeasureMode.Exactly)
            {
                result = constrainedSize;
            }
            else if (measureMode == FlexMeasureMode.AtMost)
            {
                result = Math.Min(constrainedSize, measuredSize);
            }
            else
            {
                result = measuredSize;
            }

            return result;
        }


        static void ApplyLayoutToViewHierarchy(NativeView view)
        {
            if (!GetIsIncluded(view))
                return;

            var node = GetNode(view);

            if (!(view is FlexLayout))
                ApplyLayoutToNativeView(view, node);

            if (view.IsLeaf())
                return;
            foreach (var subView in FlexLayoutExtensions.GetChildren(view))
                ApplyLayoutToViewHierarchy(subView);
        }

        static bool NodeHasExactSameChildren(IFlexNode node, NativeView[] subviews)
        {
            if (node.Count() != subviews.Length)
                return false;

            for (int i = 0; i < subviews.Length; i++)
            {
                var childNode = GetNode(subviews[i]);
                if (node.ElementAt(i) != childNode)
                {
                    return false;
                }
            }
            return true;
        }

        protected virtual IFlexNode InitNode()
        {
            _root = GetNewFlexNode();
            SetNode(this, _root);
            FlexLayoutExtensions.Bridges.Add(_root, this);
            UpdateRootNode();
            return _root;
        }

        void RegisterChild(NativeView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            IFlexNode node = GetNewFlexNode();
            SetNode(view, node);
            FlexLayoutExtensions.Bridges.Add(node, view);
            var viewINPC = view as INotifyPropertyChanged;
            if (viewINPC != null)
                viewINPC.PropertyChanged += ChildPropertyChanged;

        }
        void UnregisterChild(NativeView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            var node = GetNode(view);
            SetNode(view, null);
            FlexLayoutExtensions.Bridges.Remove(node);
            var viewINPC = view as INotifyPropertyChanged;
            if (viewINPC != null)
                viewINPC.PropertyChanged -= ChildPropertyChanged;
        }
        void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateNode(this, sender, e.PropertyName);
        }

        void CalculateLayoutWithSize(float width, float height)
        {
            var node = _root;

            node.Width = width;
            node.Height = height;
            node.CalculateLayout();
        }


        private static Type _engineType;

        IFlexNode GetNewFlexNode()
        {
            if (_engineType == null)
                throw new InvalidOperationException("You must call FlexLayout.RegisterEngine");
            var instance = Activator.CreateInstance(_engineType);
            return instance as IFlexNode;
        }

        public static void RegisterEngine(Type engineType)
        {
            _engineType = engineType;
        }

    }
}
