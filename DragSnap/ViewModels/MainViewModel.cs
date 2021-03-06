﻿namespace DragSnap.ViewModels
{
    using System;
    using Caliburn.Micro;
    using DragSnap.Events;
    using PropertyChanged;

    /// <summary>
    /// Main view model for the application
    /// </summary>
    [ImplementPropertyChanged]
    public class MainViewModel : PropertyChangedBase, IHandle<ItemMovedEvent>, IHandle<ItemSelectedEvent>
    {
        /// <summary>
        /// The threshold at which the items positions are considered equal
        /// </summary>
        private const double Threshold = 10;

        /// <summary>
        /// Stores the events aggregator
        /// </summary>
        private readonly IEventAggregator events;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class
        /// </summary>
        /// <param name="events">The events</param>
        public MainViewModel(IEventAggregator events)
        {
            this.events = events;
            this.events.Subscribe(this);

            this.Items = new BindableCollection<ItemViewModel>();
        }

        /// <summary>
        /// Gets or sets a collection of items displayed in the canvas
        /// </summary>
        public BindableCollection<ItemViewModel> Items { get; set; }

        /// <summary>
        /// Adds an item to the canvas
        /// </summary>
        public void AddItem()
        {
            this.Items.Add(new ItemViewModel(this.events));
        }

        /// <summary>
        /// Handles the item selected event
        /// </summary>
        /// <param name="message">The message</param>
        public void Handle(ItemSelectedEvent message)
        {
            foreach (ItemViewModel item in this.Items)
            {
                if (item.ID == message.ID)
                {
                    continue;
                }

                item.Selected = false;
            }
        }

        /// <summary>
        /// Handles the item released event
        /// </summary>
        /// <param name="message">The message</param>
        public void Handle(ItemMovedEvent message)
        {
            // checks if the item has coordinates within another item's coordinates
            foreach (ItemViewModel item in this.Items)
            {
                if (item.ID == message.ID)
                {
                    continue;
                }

                if (ApproximatelyEquals(item.X, message.X) ||
                    ApproximatelyEquals(item.Y, message.Y) ||
                    ApproximatelyEquals(item.Bottom, message.Bottom) ||
                    ApproximatelyEquals(item.Right, message.Right))
                {
                    throw new System.NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Calculates whether a number is approximately equal to another using the threshold
        /// </summary>
        /// <param name="x">The first value</param>
        /// <param name="y">The second value</param>
        /// <returns>Whether the values are equal within the threshold</returns>
        private static bool ApproximatelyEquals(double x, double y)
        {
            double actualDifference = Math.Abs(x - y);
            return actualDifference <= Threshold;
        }
    }
}