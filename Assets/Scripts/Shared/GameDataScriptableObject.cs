using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 5)]
    public class GameDataScriptableObject : ScriptableObject
    {
        // Background image for the puzzle
        public Sprite BackgroundSprite;
        // Images that go onto the puzzle pieces
        public List<Sprite> Sprites;
        // the puzzles piece that should be replicated in the game ie. the coin for the coin game, or the crayon for the color matching game
        public GameObject Prefab;
        // the name of the game
        public SceneState.EGameNames GameName;
        // the music that should be played for the specific game
        public AudioClip PlaceholderForMusic;
    }
}
