using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BacodePrint
{
    internal class CanvasAdorner : Adorner
    {

        //4条边
        private readonly Thumb _leftThumb;
        private readonly Thumb _topThumb;
        private readonly Thumb _rightThumb;
        private readonly Thumb _bottomThumb;

        //4个角
        private readonly Thumb _lefTopThumb;
        private readonly Thumb _rightTopThumb;
        private readonly Thumb _rightBottomThumb;
        private readonly Thumb _leftbottomThumb;

        //布局容器，如果不使用布局容器，则需要给上述8个控件布局，实现和Grid布局定位是一样的，会比较繁琐且意义不大。
        private readonly Grid _grid;
        private readonly UIElement _adornedElement;

        public Path mPath { get; set; }
        public FrameworkElement mFrameworkElement { get; set; }
        //对象可见状态
        public Visibility MVisibility { get; set; }
        public CanvasAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _adornedElement = adornedElement;
            //初始化thumb
            _leftThumb = new Thumb();
            _leftThumb.HorizontalAlignment = HorizontalAlignment.Left;
            _leftThumb.VerticalAlignment = VerticalAlignment.Center;
            _leftThumb.Cursor = Cursors.SizeWE;
            _topThumb = new Thumb();
            _topThumb.HorizontalAlignment = HorizontalAlignment.Center;
            _topThumb.VerticalAlignment = VerticalAlignment.Top;
            _topThumb.Cursor = Cursors.SizeNS;
            _rightThumb = new Thumb();
            _rightThumb.HorizontalAlignment = HorizontalAlignment.Right;
            _rightThumb.VerticalAlignment = VerticalAlignment.Center;
            _rightThumb.Cursor = Cursors.SizeWE;
            _bottomThumb = new Thumb();
            _bottomThumb.HorizontalAlignment = HorizontalAlignment.Center;
            _bottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            _bottomThumb.Cursor = Cursors.SizeNS;
            _lefTopThumb = new Thumb();
            _lefTopThumb.HorizontalAlignment = HorizontalAlignment.Left;
            _lefTopThumb.VerticalAlignment = VerticalAlignment.Top;
            _lefTopThumb.Cursor = Cursors.SizeNWSE;
            _rightTopThumb = new Thumb();
            _rightTopThumb.HorizontalAlignment = HorizontalAlignment.Right;
            _rightTopThumb.VerticalAlignment = VerticalAlignment.Top;
            _rightTopThumb.Cursor = Cursors.SizeNESW;
            _rightBottomThumb = new Thumb();
            _rightBottomThumb.HorizontalAlignment = HorizontalAlignment.Right;
            _rightBottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            _rightBottomThumb.Cursor = Cursors.SizeNWSE;
            _leftbottomThumb = new Thumb();
            _leftbottomThumb.HorizontalAlignment = HorizontalAlignment.Left;
            _leftbottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            _leftbottomThumb.Cursor = Cursors.SizeNESW;
            _grid = new Grid();
            _grid.Children.Add(_leftThumb);
            _grid.Children.Add(_topThumb);
            _grid.Children.Add(_rightThumb);
            _grid.Children.Add(_bottomThumb);
            _grid.Children.Add(_lefTopThumb);
            _grid.Children.Add(_rightTopThumb);
            _grid.Children.Add(_rightBottomThumb);
            _grid.Children.Add(_leftbottomThumb);
            AddVisualChild(_grid);
            foreach (Thumb thumb in _grid.Children)
            {
                
                thumb.Width = 16;
                thumb.Height = 16;
                thumb.Background = Brushes.Green;
                thumb.Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(new SolidColorBrush(Colors.White))
                };
                thumb.DragDelta += Thumb_DragDelta;
            }

            mPath = null;
            mFrameworkElement = null;
        }
        public void SetPosition(int pLeft,int pTop ,int pWidth,int pHeight)
        {
            var c = _adornedElement as FrameworkElement;
            Canvas.SetLeft(c, pLeft);
            Canvas.SetTop(c, pTop);
            c.Width = pWidth;
            c.Height = pHeight;
        }
        //设置可见状态
        public void  SetVisibleState(Visibility pVisibility)
        {
            MVisibility = pVisibility;
            _adornedElement.Visibility = MVisibility;

            foreach (Thumb thumb in _grid.Children)
            {
                thumb.Visibility = MVisibility;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            return _grid;
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            //直接给grid布局，grid内部的thumb会自动布局。
            _grid.Arrange(new Rect(new Point(-_leftThumb.Width / 2, -_leftThumb.Height / 2), new Size(finalSize.Width + _leftThumb.Width, finalSize.Height + _leftThumb.Height)));
            return finalSize;
        }
        //拖动逻辑
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var c = _adornedElement as FrameworkElement;
            var thumb = sender as FrameworkElement;
            double left, top, width, height;
            if (thumb.HorizontalAlignment == HorizontalAlignment.Left)
            {

                left = double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c) + e.HorizontalChange;
                width = c.Width - e.HorizontalChange;
            }
            else
            {
                left = Canvas.GetLeft(c);
                width = c.Width + e.HorizontalChange;
            }
            if (thumb.VerticalAlignment == VerticalAlignment.Top)
            {
                top = double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c) + e.VerticalChange;
                height = c.Height - e.VerticalChange;
            }
            else
            {
                top = Canvas.GetTop(c);
                height = c.Height + e.VerticalChange;
            }
            if (thumb.HorizontalAlignment != HorizontalAlignment.Center)
            {
                if (width >= 0)
                {
                    Canvas.SetLeft(c, left);
                    c.Width = width;
                    ChangeMpathSize(left, top, width, height);
                }
            }
            if (thumb.VerticalAlignment != VerticalAlignment.Center)
            {
                if (height >= 0)
                {
                    Canvas.SetTop(c, top);
                    c.Height = height;

                    ChangeMpathSize(left, top, width, height);
                }
            }

            
        }

        public void ChangeMpathSize(double left, double top, double width, double height)
        {
            if (mPath != null)
            {
                var c = _adornedElement as FrameworkElement;

                left = Canvas.GetLeft(c);
                top = Canvas.GetTop(c);
                width = c.Width;
                height = c.Height;

                //width = width < 0 ? 0 : width;
                //height = height < 0 ? 0 : height;

                RectangleGeometry rg = new RectangleGeometry
                {
                    Rect = new Rect(left, top, width, height)
                };
                mPath.Data = rg;
            }

            if (mFrameworkElement != null)
            {
                var c = _adornedElement as FrameworkElement;

                Canvas.SetLeft(mFrameworkElement, Canvas.GetLeft(c));
                Canvas.SetTop(mFrameworkElement, Canvas.GetTop(c));
                mFrameworkElement.Width = c.Width;
                mFrameworkElement.Height = c.Height;
            }
        }

        //thumb的样式
        FrameworkElementFactory GetFactory(Brush back)
        {
            var fef = new FrameworkElementFactory(typeof(Ellipse));
            fef.SetValue(Ellipse.FillProperty, back);
            fef.SetValue(Ellipse.StrokeProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999")));
            fef.SetValue(Ellipse.StrokeThicknessProperty, (double)2);
            return fef;
        }
    }
}
