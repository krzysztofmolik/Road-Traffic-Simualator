using System;
using System.Windows.Markup;
using Caliburn.Micro;

namespace Common.Wpf
{
    public class ResolveExtension : MarkupExtension
    {
        private static bool? isInDesignMode;

        public static bool IsInDesignMode
        {
            get
            {
                if ( isInDesignMode == null )
                {
                    if ( System.Windows.Application.Current != null )
                    {
                        var app = System.Windows.Application.Current.ToString();

                        if ( app == "System.Windows.Application" || app == "Microsoft.Expression.Blend.BlendApplication" )
                            isInDesignMode = true;
                        else isInDesignMode = false;
                    }
                    else isInDesignMode = true;
                }

                return isInDesignMode.GetValueOrDefault( false );
            }
            set { isInDesignMode = value; }
        }

        public ResolveExtension() { }

        public ResolveExtension( Type type )
        {
            Type = type;
        }

        public Type Type { get; set; }

        public string Key { get; set; }

        public object DesignTimeValue { get; set; }

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            if ( IsInDesignMode ) return DesignTimeValue;

            if ( string.IsNullOrEmpty( Key ) ) return IoC.GetInstance( Type, null );
            return Type == null ? IoC.GetInstance( null, Key ) : IoC.GetInstance( Type, Key );
        }
    }
}
