using System;

namespace Common
{
    public static class ExtesnionsForEnumType
    {
        public static int EnumCount( this Type enumType )
        {
            if( !enumType.IsEnum)
            {
                throw new ArgumentException( "Only enum type are alowed" );
            }

            return Enum.GetValues( enumType ).Length;
        }
    }
}