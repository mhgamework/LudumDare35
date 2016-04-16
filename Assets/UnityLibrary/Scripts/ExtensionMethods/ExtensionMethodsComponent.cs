using System;
using System.Reflection;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsComponent
    {
        // -- PUBLIC

        // .. EXTENSION METHODS

        public static bool FindComponent<T>(
            this Component component,
            out T find_component
            )
        {
            bool
                result;
            Component
                ref_result;

            find_component = component.GetComponent<T>();
            ref_result = find_component as Component;

            result = ref_result != null;
            return result;
        }

        // ~~

        public static T GetCopyOf<T>(
            this Component comp,
            T other
            ) where T : Component
        {
            Type
                type;
            BindingFlags
                flags;
            PropertyInfo[]
                property_info_table;
            FieldInfo[]
                field_info_table;

            type = comp.GetType();

            if ( type != other.GetType() )
            {
                return null;
            } // type mis-match

            flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

            property_info_table = type.GetProperties( flags );

            foreach ( PropertyInfo property_info in property_info_table )
            {
                if ( property_info.CanWrite )
                {
                    try
                    {
                        property_info.SetValue( comp, property_info.GetValue( other, null ), null );
                    }
                    catch
                    {
                    } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }

            field_info_table = type.GetFields( flags );

            foreach ( FieldInfo field_info in field_info_table )
            {
                field_info.SetValue( comp, field_info.GetValue( other ) );
            }
            return comp as T;
        }
    }
}

