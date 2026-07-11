namespace Escapist.Player
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        void Interact();
    }
}