namespace Escapist.Persistence
{
    public interface ISaveable
    {
        /// <summary>
        /// Populates the incoming unified SaveData payload with this component's active states.
        /// </summary>
        void CaptureState(SaveData data);

        /// <summary>
        /// Reads back saved structural state fragments out of the payload and restores gameplay properties.
        /// </summary>
        void RestoreState(SaveData data);
    }
}