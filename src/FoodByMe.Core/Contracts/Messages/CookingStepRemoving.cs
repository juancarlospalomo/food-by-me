using System;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Contracts.Messages
{
    public class CookingStepRemoving : MvxMessage
    {
        public CookingStepRemoving(object sender, Guid id) : base(sender)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
