namespace ORMish
{
    public class EyeColorManager : ColorManager
    {
        protected override void OnColorSelect(ColorHash colorHash)
        {
            base.OnColorSelect(colorHash);
            characterCreationManager.SetEyeColor(colorHash.colorValue);
        }

    }
}