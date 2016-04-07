using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class IngredientEditViewModel : BaseViewModel, IPositionable
    {
        private readonly IMvxMessenger _messenger;
        private Measure _measure;
        private string _title;
        private double? _quantity;
        private int _position;

        public IngredientEditViewModel(IMvxMessenger messenger, IReadOnlyList<Measure> measures, int position)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            if (measures == null)
            {
                throw new ArgumentNullException(nameof(measures));
            }
            if (position < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }
            _messenger = messenger;
            _measure = measures.FirstOrDefault();
            Measures = measures;
            Position = position;
            Id = Guid.NewGuid();
        }

        public IReadOnlyList<Measure> Measures { get; }

        public Measure Measure
        {
            get { return _measure; }
            set
            {
                _measure = value;
                RaisePropertyChanged();
            }
        }

        public Guid Id { get; }

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public double? Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged();
            }
        }

        public ICommand DeleteCommand => new MvxCommand(Delete);

        private void Delete()
        {
            var message = new IngredientRemoving(this, Id);
            _messenger.Publish(message);
        }
    }
}