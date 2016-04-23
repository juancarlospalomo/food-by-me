using System;
using System.Collections.Generic;
using FoodByMe.Android.Views;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Shared.Caching;

namespace FoodByMe.Android.Framework.Caching
{
    internal class MainActivityFragmentCacheInfoFactory : MvxCachedFragmentInfoFactory
    {
        private static readonly Dictionary<string, CustomFragmentInfo> MyFragmentsInfo = new Dictionary
            <string, CustomFragmentInfo>
        {
            {
                typeof (RecipeCategoryMenuViewModel).ToString(),
                new CustomFragmentInfo(typeof (RecipeCategoryMenuViewModel).Name, typeof (SidebarFragment), typeof (RecipeCategoryMenuViewModel), cacheFragment:false)
            },
            {
                typeof (RecipeDetailedListViewModel).ToString(),
                new CustomFragmentInfo(typeof (RecipeDetailedListViewModel).Name, typeof (RecipeDetailedListFragment),
                    typeof (RecipeDetailedListViewModel), isRoot: false, addToBackstack: true, cacheFragment:false)
            },
            {
                typeof (RecipeListViewModel).ToString(),
                new CustomFragmentInfo(typeof (RecipeListViewModel).Name, typeof (RecipeListFragment),
                    typeof (RecipeListViewModel), isRoot: true, cacheFragment:false)
            },
            {
                typeof (RecipeSearchListViewModel).ToString(),
                new CustomFragmentInfo(typeof (RecipeSearchListViewModel).Name, typeof (RecipeSearchListFragment),
                    typeof (RecipeSearchListViewModel), isRoot: false, addToBackstack: true, cacheFragment:false)
            },
            {
                typeof (RecipeEditViewModel).ToString(),
                new CustomFragmentInfo(typeof (RecipeEditViewModel).Name, typeof (RecipeEditFragment),
                    typeof (RecipeEditViewModel), isRoot: false, addToBackstack: true, cacheFragment:false)
            },
            {
                typeof (RecipeDisplayViewModel).ToString(),
                new CustomFragmentInfo(typeof (RecipeDisplayViewModel).Name, typeof (RecipeDisplayFragment),
                    typeof (RecipeDisplayViewModel), isRoot: false, addToBackstack: true, cacheFragment:false)
            }
        };

        public Dictionary<string, CustomFragmentInfo> GetFragmentsRegistrationData()
        {
            return MyFragmentsInfo;
        }


        public override IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true,
            bool addToBackstack = false)
        {
            var viewModelTypeString = viewModelType.ToString();
            if (!MyFragmentsInfo.ContainsKey(viewModelTypeString))
                return base.CreateFragmentInfo(tag, fragmentType, viewModelType, cacheFragment, addToBackstack);

            var fragInfo = MyFragmentsInfo[viewModelTypeString];
            return fragInfo;
        }

        public override SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(
            IMvxCachedFragmentInfo objectToSerialize)
        {
            var baseSerializableCachedFragmentInfo = base.GetSerializableFragmentInfo(objectToSerialize);
            var customFragmentInfo = objectToSerialize as CustomFragmentInfo;

            return new SerializableCustomFragmentInfo(baseSerializableCachedFragmentInfo)
            {
                IsRoot = customFragmentInfo?.IsRoot ?? false
            };
        }

        public override IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(
            SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo)
        {
            var serializableCustomFragmentInfo = fromSerializableMvxCachedFragmentInfo as SerializableCustomFragmentInfo;
            var baseCachedFragmentInfo = base.ConvertSerializableFragmentInfo(fromSerializableMvxCachedFragmentInfo);

            return new CustomFragmentInfo(baseCachedFragmentInfo.Tag, baseCachedFragmentInfo.FragmentType,
                baseCachedFragmentInfo.ViewModelType, baseCachedFragmentInfo.AddToBackStack,
                serializableCustomFragmentInfo?.IsRoot ?? false)
            {
                ContentId = baseCachedFragmentInfo.ContentId,
                CachedFragment = baseCachedFragmentInfo.CachedFragment
            };
        }

        internal class SerializableCustomFragmentInfo : SerializableMvxCachedFragmentInfo
        {
            public SerializableCustomFragmentInfo()
            {
                
            }

            public SerializableCustomFragmentInfo(SerializableMvxCachedFragmentInfo baseFragmentInfo)
            {
                AddToBackStack = baseFragmentInfo.AddToBackStack;
                ContentId = baseFragmentInfo.ContentId;
                FragmentType = baseFragmentInfo.FragmentType;
                Tag = baseFragmentInfo.Tag;
                ViewModelType = baseFragmentInfo.ViewModelType;
            }

            public bool IsRoot { get; set; }
        }
    }
}