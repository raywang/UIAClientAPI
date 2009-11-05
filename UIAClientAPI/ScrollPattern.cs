//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Automation;

//namespace UIAClientAPI
//{
//        class ScrollPatternTest
//        {
//                ScrollPattern pattern;

//                public ScrollPatternTest (ScrollPattern pattern)
//                {
//                        this.pattern = pattern;
//                }

//                public void ScrollTest ()
//                {
//                        var oldValue = pattern.Current.HorizontalScrollPercent;
//                        pattern.Scroll (ScrollAmount.SmallIncrement, ScrollAmount.NoAmount);
//                        var newValue = pattern.Current.HorizontalScrollPercent;
//                        Assert (newValue > oldValue);
			
//                } 

//                //the ScrollPattern's method
//                public void Scroll (ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
//                {
//                        ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
//                        sp.Scroll (horizontalAmount, verticalAmount);
//                }

//                public void ScrollHorizontal (ScrollAmount amount)
//                {
//                        ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
//                        sp.ScrollHorizontal (amount);
//                }

//                public void ScrollVertical (ScrollAmount amount)
//                {
//                        ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
//                        sp.ScrollVertical (amount);
//                }

//                public void SetScrollPercent (double horizontalPercent, double verticalPercent)
//                {
//                        ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
//                        sp.SetScrollPercent (horizontalPercent, verticalPercent);
//                }

//                //the ScrollPattern's property
//                public bool HorizontallyScrollable
//                {
//                        get { return (bool) element.GetCurrentPropertyValue (ScrollPattern.HorizontallyScrollableProperty); }
//                }

//                public double HorizontalScrollPercent
//                {
//                        get { return (double) element.GetCurrentPropertyValue (ScrollPattern.HorizontalScrollPercentProperty); }
//                }

//                public double HorizontalViewSize
//                {
//                        get { return (double) element.GetCurrentPropertyValue (ScrollPattern.HorizontalViewSizeProperty); }
//                }

//                public bool VerticallyScrollable
//                {
//                        get { return (bool) element.GetCurrentPropertyValue (ScrollPattern.VerticallyScrollableProperty); }
//                }

//                public double VerticalScrollPercent
//                {
//                        get { return (double) element.GetCurrentPropertyValue (ScrollPattern.VerticalScrollPercentProperty); }
//                }

//                public double VerticalViewSize
//                {
//                        get { return (double) element.GetCurrentPropertyValue (ScrollPattern.VerticalViewSizeProperty); }
//                }

//        }
//}
