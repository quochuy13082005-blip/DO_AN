
namespace DO_AN
{
    public class AVLNode
    {
        public Citizen Data; //Dữ liệu của node (thông tin công dân)
        public AVLNode Left; //Con trỏ đến node con bên trái
        public AVLNode Right; //Con trỏ đến node con bên phải
        public int Height; //Chiều cao của node (dùng để cân bằng AVL)

        // Constructor khởi tạo node mới với dữ liệu công dân
        public AVLNode(Citizen citizen)
        {
            Data = citizen;
            Left = null;
            Right = null;
            Height = 1; // Mặc định chiều cao của node mới là 1
        }
    }
}