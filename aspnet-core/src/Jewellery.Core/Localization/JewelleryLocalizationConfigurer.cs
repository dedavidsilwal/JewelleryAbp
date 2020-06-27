using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Jewellery.Localization
{
    public static class JewelleryLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(JewelleryConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(JewelleryLocalizationConfigurer).GetAssembly(),
                        "Jewellery.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
