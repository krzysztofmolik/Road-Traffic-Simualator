using Microsoft.Xna.Framework;
using XnaVs10.Utils;

namespace XnaRoadTrafficConstructor.MathHelpers
{
    public class IsVectorCross
    {
        public static Vector2 IntersectionPoint(Line firstLine, Line secondLine)
        {

            double Ua, Ub;

 

            // Equations to determine whether lines intersect

            Ua = ((secondLine.End.X - secondLine.Begin.X) * (firstLine.Begin.Y - secondLine.Begin.Y) - (secondLine.End.Y - secondLine.Begin.Y) * (firstLine.Begin.X - secondLine.Begin.X)) /

                    ((secondLine.End.Y - secondLine.Begin.Y) * (firstLine.End.X - firstLine.Begin.X) - (secondLine.End.X - secondLine.Begin.X) * (firstLine.End.Y - firstLine.Begin.Y));

 

            Ub = ((firstLine.End.X - firstLine.Begin.X) * (firstLine.Begin.Y - secondLine.Begin.Y) - (firstLine.End.Y - firstLine.Begin.Y) * (firstLine.Begin.X - secondLine.Begin.X)) /

                    ((secondLine.End.Y - secondLine.Begin.Y) * (firstLine.End.X - firstLine.Begin.X) - (secondLine.End.X - secondLine.Begin.X) * (firstLine.End.Y - firstLine.Begin.Y));

 

            if (Ua >= 0.0f && Ua <= 1.0f && Ub >= 0.0f && Ub <= 1.0f)

            {

                double x = firstLine.Begin.X + Ua*(firstLine.End.X - firstLine.Begin.X);

                double y = firstLine.Begin.Y + Ua*(firstLine.End.Y - firstLine.Begin.Y);

               

                return new Vector2((float)x, (float)y);

 

            }

            else

            {

                return new Vector2();

            }

        }

        /*
           Sprawdzenie, czy wektory przecinaja sie, gdzie:
              'vector_1' - pierwszy wektor,
              'vector_2' - drugi wektor

           Zwracany wynik65:
              true - wektory przecinaja sie,
              false - wektory nie przecinaja sie

           Obliczenia sa bezpieczne dla wspolrzednych wektorow mieszczacych sie w 
           granicach (-k,k), gdzie k = sqrt(max(int) / 4).

           Dlugosci wektorow nie moga byc rowne 0.
        */
        public static bool IsVectorCrossed( Line vector_1, Line vector_2 )
        {
            float a, b, c;
            float k1, k2;

            /*
               Znalezienie prostej, na ktorej polozony jest wektor 'vector_1'. 
               Prosta taka ma wzor66:   a * x + b * y + c = 0
            */

            a = vector_1.End.Y - vector_2.Begin.Y;
            b = -( vector_1.End.X - vector_2.Begin.X );
            c = -a * vector_1.Begin.X - b * vector_1.Begin.Y;

            /*
               Obliczenie odleglosci punktow poczatkowego i koncowego wektora 'vector_2' od tej prostej.
               Zeby wektory sie przecinaly, to punkty te' musza znajdowac sie po obu stronach tej prostej.

               W rzeczywistosci liczby te powinny byc jeszcze podzielone przez 'sqrt(a * a + b *b)', aby 
               dawaly odleglosc punktu od danej prostej, ale tu potrzebne sa tylko znaki liczb 'k1' i 'k2', a
               nie ich konkretne wartosci.
            */

            k1 = a * vector_2.Begin.X + b * vector_2.Begin.Y + c;   // Odleglosc punktu poczatkowego
            k2 = a * vector_2.End.X + b * vector_2.End.Y + c;   // Odleglosc punktu koncowego

            if ( ( k1 > 0 && k2 > 0 ) || ( k1 < 0 && k2 < 0 ) )
                return false;    // Punkty wektora 'vector_2' leza tylko po jednej stronie prostej, wiec wektory nie moga sie przecinac.

            /*
               Znalezienie prostej, na ktorej leza punkt poczatkowy wektora 'vector_1' i punkt poczatkowy 
               wektora 'vector_2'. Nastepnie obliczamy odleglosc punktu koncowego wektora 'vector_2' od 
               tej prostej.
            */

            a = vector_2.Begin.Y - vector_1.Begin.Y;
            b = -( vector_2.Begin.X - vector_1.Begin.X );
            c = -a * vector_2.Begin.X - b * vector_2.Begin.Y;

            k1 = a * vector_2.End.X + b * vector_2.End.Y + c;

            /*
               Znalezienie prostej, na ktorej leza punkt koncowy wektora 'vector_1' i punkt poczatkowy 
               wektora 'vector_2'. Nastepnie obliczamy odleglosc punktu koncowego wektora 'vector_2' 
               od tej prostej.
            */

            a = vector_2.Begin.Y - vector_1.End.Y;
            b = -( vector_2.Begin.X - vector_1.End.X );
            c = -a * vector_2.Begin.X - b * vector_2.Begin.Y;

            k2 = a * vector_2.End.X + b * vector_2.End.Y + c;

            /*
               Aby wektory sie przecinaly, pozostaly punkt koncowy wektora 'vector_2' musi zawierac 
               sie miedzy obydwiema prostymi. Proste te tworza obszar, w ktorym musi znajdowac sie
               ten punkt. Znaki odleglosci od obydwu prostych musza byc wtedy rozne.

               Jezeli bedzie punkt ten lezal na ktorejs z prostej (odleglosc bedzie rowna 0), to tez dobrze.
            */

            if ( k1 == 0 || k2 == 0 )
                return true;    // Wektory przecinaja sie, punkt lezy na jednej z prostych

            if ( ( k1 > 0 && k2 < 0 ) || ( k1 < 0 && k2 > 0 ) )
                return true;    // Wektory przecinaja sie

            // Punkt koncowy wektora 'vector_2' nie lezy w wyznaczonym obszarze.
            return false;
        }
    }
}