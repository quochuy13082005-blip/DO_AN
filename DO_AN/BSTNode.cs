
namespace DO_AN
{
    public class BSTNode
    {
        public Citizen Data; //Dữ liệu của node (thông tin công dân)
        public BSTNode Left; //Con trỏ đến node con bên trái
        public BSTNode Right; //Con trỏ đến node con bên phải
        public int Height; //Chiều cao của node (dùng để cân bằng AVL)

        // Constructor khởi tạo node mới với dữ liệu công dân
        public BSTNode(Citizen citizen)
        {
            Data = citizen;
            Left = null;
            Right = null;
            Height = 1; // Mặc định chiều cao của node mới là 1
        }
    }
}