    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
     
    [CreateAssetMenu(fileName = "SpriteDatabase", menuName = "Database/New Sprite Database")]
    public class SpriteDatabaseSO : ScriptableObject
    {
        public List<Sprite> sprites;
    }