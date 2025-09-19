namespace Example
{
    public class SkinColorManager : ColorManager
    {
        protected override void OnColorSelect(ColorHash colorHash)
        {
            base.OnColorSelect(colorHash);
            characterCreationManager.SetBodyColor(colorHash.colorValue);
        }

    }
}
