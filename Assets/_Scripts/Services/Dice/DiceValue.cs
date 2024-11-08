using UnityEngine;

namespace _Scripts.Services.Dice
{
    public class DiceValue
    {
        private const float FaceDetectionThreshold = 0.6f;
        private readonly int[] _faceValues = { 3, 6, 1, 4, 5, 2 };

        private readonly Vector3[] _faceUpDirections = new Vector3[6]
        {
            Vector3.right,         
            Vector3.back,         
            Vector3.forward,       
            Vector3.left,         
            Vector3.up,            
            Vector3.down        
        };

        public int GetTopFace(Transform dice)
        {
            var maxDot = -1f;
            var bestFaceIndex = -1;

            for (int i = 0; i < _faceUpDirections.Length; i++)
            {
                var worldFaceDirection = dice.TransformDirection(_faceUpDirections[i]);
                
                var dot = Vector3.Dot(worldFaceDirection, Vector3.up);

                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestFaceIndex = i;
                }
            }

            return maxDot >= FaceDetectionThreshold ? _faceValues[bestFaceIndex] : 0;
        }
    }
}