using System;
using System.Windows.Input;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class CookingStepEditViewModel : BaseViewModel, IPositionable
    {
        private readonly IMvxMessenger _messenger;
        private string _text;
        private int _position;

        public CookingStepEditViewModel(IMvxMessenger messenger, int position)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            if (position < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }
            _messenger = messenger;
            Id = Guid.NewGuid();
            Position = position;
        }

        public Guid Id { get; }

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => Label);
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }

        public string Label => $"{TextSource.GetText("StepNo")} {Position}";

        public ICommand DeleteCommand => new MvxCommand(Delete);

        private void Delete()
        {
            var message = new CookingStepRemoving(this, Id);
            _messenger.Publish(message);
        }
    }
}