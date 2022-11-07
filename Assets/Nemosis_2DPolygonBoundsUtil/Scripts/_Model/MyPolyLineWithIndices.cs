namespace _Model
{
    public class MyPolyLineWithIndices
    {
        public MyPolyLine MyPolyLine;
        public int indexA;
        public int indexB;


        public MyPolyLineWithIndices(MyPolyLine myPolyLine, int indexA, int indexB)
        {
            MyPolyLine = myPolyLine;
            this.indexA = indexA;
            this.indexB = indexB;
        }
    }
}