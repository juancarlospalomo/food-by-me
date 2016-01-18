using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using MvvmCross.Localization;

namespace FoodByMe.Core.Framework
{
    public class ResxTextProvider : IMvxTextProvider
    {
        private readonly ResourceManager _manager;
        private CultureInfo _culture;

        public ResxTextProvider(ResourceManager manager) : this(manager, CultureInfo.CurrentUICulture)
        {
        }

        public ResxTextProvider(ResourceManager manager, CultureInfo culture)
        {
            _manager = manager;
            Culture = culture;
        }

        public CultureInfo Culture
        {
            get { return _culture; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Culture));
                }
                _culture = value;
            }
        }

        public string GetText(string namespaceKey, string typeKey, string name)
        {
            var resource = ResolveKeys(namespaceKey, typeKey, name)
                .Select(s => _manager.GetString(s, Culture))
                .FirstOrDefault(s => s != null) ?? $"[MISSING {name}]";
            return resource;
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            var format = GetText(namespaceKey, typeKey, name);
            return string.Format(format, formatArgs);
        }

        private static IEnumerable<string> ResolveKeys(string namespaceKey, string typeKey, string name)
        {
            var components = (new List<string> { namespaceKey, typeKey, name })
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
            return Enumerable.Range(0, components.Count)
                .Select(i => string.Join(".", components.Skip(i)))
                .ToList();
        }
    }
}
