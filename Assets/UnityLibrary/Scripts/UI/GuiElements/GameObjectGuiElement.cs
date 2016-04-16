namespace UI.GuiElements
{
    public class GameObjectGuiElement : AGuiElement
    {
        public override void Show(float animation_time = 0)
        {
            gameObject.SetActive(true);
        }

        public override void Hide(float animation_time = 0)
        {
            gameObject.SetActive(false);
        }

        public override bool IsHidden()
        {
            return gameObject.activeSelf;
        }
    }
}
