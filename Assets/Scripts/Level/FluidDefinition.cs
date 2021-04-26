using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Fluid", menuName = "Levels/Fluid", order = 3)]
    public class FluidDefinition : ScriptableObject
    {
        public Texture2D topLeftEdgeTexture;
        public Texture2D topRightEdgeTexture;
        public Texture2D bottomLeftEdgeTexture;
        public Texture2D bottomRightEdgeTexture;
        public Texture2D[] fullTextures;
        
        public float mapXTopLeft;
        public float mapYTopLeft;
        public float mapXTopRight;
        public float mapYTopRight;
        public float mapXBottomLeft;
        public float mapYBottomLeft;
        public float mapXBottomRight;
        public float mapYBottomRight;
        public float[] mapXFull;
        public float[] mapYFull;
        
        public float mapWidth;
        public float mapHeight;
    }
}