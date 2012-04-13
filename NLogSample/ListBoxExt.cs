﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;

namespace NLogSample
{
    public static class ListBoxExt  
    {  
        public static readonly DependencyProperty AutoScrollProperty =  
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(System.Windows.Controls.ListBox),  
            new PropertyMetadata(false));  
 
        public static readonly DependencyProperty AutoScrollHandlerProperty =  
            DependencyProperty.RegisterAttached("AutoScrollHandler", typeof(AutoScrollHandler), typeof(System.Windows.Controls.ListBox));  
 
        public static bool GetAutoScroll(System.Windows.Controls.ListBox instance)  
        {  
            return (bool)instance.GetValue(AutoScrollProperty);  
        }  
 
        public static void SetAutoScroll(System.Windows.Controls.ListBox instance, bool value)  
        {  
            var oldHandler = (AutoScrollHandler)instance.GetValue(AutoScrollHandlerProperty);  
            if (oldHandler != null)  
            {  
                oldHandler.Dispose();  
                instance.SetValue(AutoScrollHandlerProperty, null);  
            }  
            instance.SetValue(AutoScrollProperty, value);  
            if (value)  
                instance.SetValue(AutoScrollHandlerProperty, new AutoScrollHandler(instance));  
        }  
    }  
 
    public class AutoScrollHandler : DependencyObject, IDisposable  
    {  
 
        public static readonly DependencyProperty ItemsSourceProperty =  
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable),  
            typeof(AutoScrollHandler), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,  
                ItemsSourcePropertyChanged));  
 
        private readonly System.Windows.Controls.ListBox _target;  
 
        public AutoScrollHandler(System.Windows.Controls.ListBox target)  
        {  
            _target = target;  
            var binding = new Binding("ItemsSource") {Source = _target};
            BindingOperations.SetBinding(this, ItemsSourceProperty, binding);  
        }  
 
        public void Dispose()  
        {  
            BindingOperations.ClearBinding(this, ItemsSourceProperty);  
        }  
 
        public IEnumerable ItemsSource  
        {  
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }  
            set { SetValue(ItemsSourceProperty, value); }  
        }  
 
        static void ItemsSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)  
        {  
            ((AutoScrollHandler)o).ItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);  
        }  
 
        void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)  
        {  
            var collection = oldValue as INotifyCollectionChanged;  
            if (collection != null)  
                collection.CollectionChanged -= CollectionCollectionChanged;  
            collection = newValue as INotifyCollectionChanged;  
            if (collection != null)  
                collection.CollectionChanged += CollectionCollectionChanged;  
        }  
 
        void CollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)  
        {  
            if (e.Action != NotifyCollectionChangedAction.Add || e.NewItems == null || e.NewItems.Count < 1)  
                return;  
            _target.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);  
        }  
    }  
} 
