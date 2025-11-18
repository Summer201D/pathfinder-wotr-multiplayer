namespace WOTRMultiplayer.UI.Settings.Entities
{
    /// <summary>
    /// Generic MonoBehaviours are not supported by ScriptableObject.CreateInstance
    /// so we have to have this
    /// </summary>
    public class UIValidatableStringSettingsEntity : UIValidatableSettingsEntityBase<string>
    {

    }
}
