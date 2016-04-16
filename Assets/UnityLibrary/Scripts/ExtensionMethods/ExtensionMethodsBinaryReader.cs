using System.IO;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsBinaryReader
    {
        // -- PUBLIC

        // .. EXTENSION_METHODS

        public static float ReadFloat(
            this BinaryReader binary_reader
            )
        {
            return binary_reader.ReadSingle();
        }

        // ~~

        public static Vector3 ReadVector3(
            this BinaryReader binary_reader
            )
        {
            Vector3
                result;

            result.x = binary_reader.ReadFloat();
            result.y = binary_reader.ReadFloat();
            result.z = binary_reader.ReadFloat();

            return result;
        }

        // ~~

        public static Vector3 ReadVector2(
            this BinaryReader binary_reader
            )
        {
            Vector2
                result;

            result.x = binary_reader.ReadFloat();
            result.y = binary_reader.ReadFloat();

            return result;
        }

        // ~~

        public static Quaternion ReadQuaternion(
            this BinaryReader binary_reader
            )
        {
            Quaternion
                result;

            result.x = binary_reader.ReadFloat();
            result.y = binary_reader.ReadFloat();
            result.z = binary_reader.ReadFloat();
            result.w = binary_reader.ReadFloat();

            return result;
        }

        // ~~

        public static Texture2D ReadTexture2D(
            this BinaryReader binary_reader
            )
        {
            Texture2D
                result;


            result = new Texture2D( 1, 1 );
            result.LoadImage( binary_reader.ReadBytes( binary_reader.ReadInt32() ) );

            return result;
        }
    }
}
