using System.Text.RegularExpressions;
using UnityEditor;

namespace Grabber.Scripts.Postprocessors
{
    public class BuildPostprocessor : AssetPostprocessor
    {
        private const string PropertyGroupPattern = "<PropertyGroup>";
        private static readonly Regex PropertyGroupRegex = new(PropertyGroupPattern);

        public static string OnGeneratedCSProject(string path, string content)
        {
            if (!path.EndsWith("Assembly-CSharp.csproj"))
            {
                return content;
            }

            content = PropertyGroupRegex.Replace(
                content,
                $"{PropertyGroupPattern}\n\t\t<Nullable>enable</Nullable>",
                1);

            return content;
        }
    }
}
