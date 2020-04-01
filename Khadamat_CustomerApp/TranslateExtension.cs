using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Khadamat_CustomerApp
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            // Do your translation lookup here, using whatever method you require
            var translated = L10n.Localize(Text, Text);

            return translated;
        }
    }
}