namespace Example
{
    public class HairColorManager : ColorManager
    {
        protected override void OnColorSelect(ColorHash colorHash)
        {
            base.OnColorSelect(colorHash);
            characterCreationManager.SetHairColor(colorHash.colorValue);
        }

    }
}
