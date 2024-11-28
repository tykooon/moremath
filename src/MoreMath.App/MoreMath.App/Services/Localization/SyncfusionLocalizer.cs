namespace MoreMath.App.Services.Localization;

using Syncfusion.Blazor;

public class SyncfusionLocalizer : ISyncfusionStringLocalizer
{
    public string GetText(string key)
    {
        return this.ResourceManager.GetString(key);
    }

    public System.Resources.ResourceManager ResourceManager
    {
        get
        {
            return MoreMath.App.Resources.SfResources.ResourceManager;
        }
    }
}
