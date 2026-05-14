using UnityEngine;

namespace Uphill.Scripts.Utils
{
    public class ImageBlender : MonoBehaviour
    {
        [SerializeField] private Material _material = default!;

        [SerializeField] private Texture2D _leftImage = default!;
        [SerializeField] private Texture2D _rightImage = default!;

        [SerializeField] private float _width = 0.2f;

        // private void Start()
        // {
        //     var result = BlendImages(_leftImage, _rightImage, _width);
        // }

        // private static Texture2D BlendImages(
        //     Texture2D left, Texture2D right, float width)
        // {
        //     var dimensions = new Vector2(
        //         left.width + right.width,
        //         left.height + right.height);

        //     left.GetPixels();
        // }
    }
}
