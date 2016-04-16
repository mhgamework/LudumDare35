using System.IO;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsBinaryWriter
    {
        // -- PUBLIC

        // .. EXTENSION_METHODS

        public static void WriteVector3(
            this BinaryWriter binary_writer,
            Vector3 vector
            )
        {
            binary_writer.Write( vector.x );
            binary_writer.Write( vector.y );
            binary_writer.Write( vector.z );
        }

        // ~~

        public static void WriteVector2(
            this BinaryWriter binary_writer,
            Vector2 vector
            )
        {
            binary_writer.Write( vector.x );
            binary_writer.Write( vector.y );
        }

        // ~~

        public static void WriteQuaternion(
            this BinaryWriter binary_writer,
            Quaternion quaternion
            )
        {
            binary_writer.Write( quaternion.x );
            binary_writer.Write( quaternion.y );
            binary_writer.Write( quaternion.z );
            binary_writer.Write( quaternion.w );
        }

        // ~~

        public static void WriteTexture2D(
            this BinaryWriter binary_writer,
            Texture2D texture_png
            )
        {
            byte[]
                texture_table;

            texture_table = texture_png.EncodeToPNG();

            binary_writer.Write( texture_table.Length );
            binary_writer.Write( texture_table );
        }

        // .. ACCESSORS

        // .. OPERATIONS

        // -- PRIVATE

        // .. OPERATIONS

        // .. COROUTINES

        // .. ATTRIBUTES

    }
}
