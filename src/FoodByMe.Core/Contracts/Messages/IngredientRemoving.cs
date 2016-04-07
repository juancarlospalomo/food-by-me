using System;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Contracts.Messages
{
    public class IngredientRemoving : MvxMessage
    {
        public IngredientRemoving(object sender, Guid id) : base(sender)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
