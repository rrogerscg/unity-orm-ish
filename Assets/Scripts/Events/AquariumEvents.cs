using System;
using UnityEngine;

namespace ORMish 
{
    public static class AquariumEvents
    {
        public static Action<GameObject> OnPaintableSelectedClicked;
        public static Action SaveRawImage;
        public static Action DeleteSavedTexture;
        public static Action ActivatePreviousScene;
        public static Action OnNextClicked; // Selects next paintable creature
        public static Action OnPreviousClicked;  // Selects previous paintable creature
        public static Action<float> OnAquariumClicked;
        public static Action<Collider> OnThingEnteredMouth;
        public static Action<RaycastHit> OnSpawnOnSurface;
        public static Action<GameObject> OnRemoveSoapSud;
        public static Action StartPlaySession;
        public static Action OnViewAquariumStart;
        public static Action OnPaintCharacterStart;
        public static Action OnExitGameAction;
    }
}
