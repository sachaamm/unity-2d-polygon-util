using _Model;

namespace _Utility
{
    public class MyIntersection
    {
        public static bool Line2dIntersect(MyPolyLine a, MyPolyLine b)
        {
            float Ax = a.a.x;
            float Ay = a.a.y;
            float Bx = a.b.x;
            float By = a.b.y;
            float Cx = b.a.x;
            float Cy = b.a.y;
            float Dx = b.b.x;
            float Dy = b.b.y;
            
            float Ix, Iy, Jx, Jy; 
            Ix =Bx - Ax;
            Iy = By - Ay ;

            Jx = Dx - Cx;
            Jy = Dy - Cy;

            float m, k;

            float denominator = (Ix * Jy - Iy * Jx);
            if (denominator == 0) return false;

            m = -(-Ix*Ay+Ix*Cy+Iy*Ax-Iy*Cx)/denominator;
            k = -(Ax*Jy-Cx*Jy-Jx*Ay+Jx*Cy)/denominator;
  
            if (m is > 0 and < 1 && k is > 0 and < 1) return true;
  
            return false;
  
        }
    }
}