using System;
using System.Collections.Generic;

namespace DO_AN
{
    public class AVL
    {
        // =========================================================
        // 1. Thành phần dữ liệu và cấu trúc của node       
        // =========================================================
        private BSTNode root;

        public BSTNode Root
        {
            get
            {
                return root;
            }
        }


        // =========================================================
        // 2. Phương thức công khai: Insert, Delete, Search
        // =========================================================

        public void Insert(Citizen citizen)
        {
            root = InsertRec(root, citizen);
        }

        public void Delete(string citizenID)
        {
            root = DeleteRec(root, citizenID);
        }

        public Citizen Search(string citizenID)
        {
            BSTNode node = Root;

            while (node != null)
            {
                int cmp = string.Compare(citizenID, node.Data.CitizenID);

                if (cmp == 0) return node.Data;
                if (cmp < 0) node = node.Left;
                else node = node.Right;
            }

            return null;
        }
        // =========================================================
        // 3. Logic chèn và xóa node (có cân bằng AVL)
        // =========================================================

        private BSTNode InsertRec(BSTNode node, Citizen citizen)
        {
            if (node == null)
                return new BSTNode(citizen);

            int cmp = string.Compare(citizen.CitizenID, node.Data.CitizenID);

            if (cmp < 0)
                node.Left = InsertRec(node.Left, citizen);
            else if (cmp > 0)
                node.Right = InsertRec(node.Right, citizen);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            if (balance > 1 && node.Left != null &&
                string.Compare(citizen.CitizenID, node.Left.Data.CitizenID) < 0)
                return RightRotate(node);

            if (balance < -1 && node.Right != null &&
                string.Compare(citizen.CitizenID, node.Right.Data.CitizenID) > 0)
                return LeftRotate(node);

            if (balance > 1 && node.Left != null &&
                string.Compare(citizen.CitizenID, node.Left.Data.CitizenID) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            if (balance < -1 && node.Right != null &&
                string.Compare(citizen.CitizenID, node.Right.Data.CitizenID) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        private BSTNode DeleteRec(BSTNode root, string citizenID)
        {
            if (root == null) return null;

            int cmp = string.Compare(citizenID, root.Data.CitizenID);

            if (cmp < 0)
                root.Left = DeleteRec(root.Left, citizenID);
            else if (cmp > 0)
                root.Right = DeleteRec(root.Right, citizenID);
            else
            {
                // ===== TÌM THẤY NODE =====

                // 1. Không có con
                if (root.Left == null && root.Right == null)
                    return null;

                // 2. Có 1 con
                if (root.Left == null)
                    return root.Right;

                if (root.Right == null)
                    return root.Left;

                // 3. Có 2 con → lấy node nhỏ nhất bên phải
                BSTNode minNode = FindMin(root.Right);

                root.Data = minNode.Data;

                root.Right = DeleteRec(root.Right, minNode.Data.CitizenID);
            }

            // ===== 2. UPDATE HEIGHT =====
            root.Height = 1 + Math.Max(GetHeight(root.Left), GetHeight(root.Right));

            // ===== 3. GET BALANCE =====
            int balance = GetBalance(root);

            // ===== 4. ROTATION =====

            // LL
            if (balance > 1 && GetBalance(root.Left) >= 0)
                return RightRotate(root);

            // LR
            if (balance > 1 && GetBalance(root.Left) < 0)
            {
                root.Left = LeftRotate(root.Left);
                return RightRotate(root);
            }

            // RR
            if (balance < -1 && GetBalance(root.Right) <= 0)
                return LeftRotate(root);

            // RL
            if (balance < -1 && GetBalance(root.Right) > 0)
            {
                root.Right = RightRotate(root.Right);
                return LeftRotate(root);
            }

            return root;
        }

        // =========================================================
        // 4. Hàm hỗ trợ: xoay và tính toán chiều cao, cân bằng
        // =========================================================

        private BSTNode RightRotate(BSTNode y)
        {
            BSTNode x = y.Left;
            BSTNode t2 = x.Right;

            x.Right = y;
            y.Left = t2;

            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

            return x;
        }

        private BSTNode LeftRotate(BSTNode x)
        {
            BSTNode y = x.Right;
            BSTNode t2 = y.Left;

            y.Left = x;
            x.Right = t2;

            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

            return y;
        }

        private int GetHeight(BSTNode node)
        {
            if (node == null) return 0;
            return node.Height;
        }

        private int GetBalance(BSTNode node)
        {
            if (node == null) return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        private BSTNode FindMin(BSTNode node)
        {
            while (node.Left != null)
                node = node.Left;

            return node;
        }
        // =========================================================
        // 5. Hàm xuất dữ liệu: Duyệt cây lấy danh sách công dân
        // =========================================================

        public List<Citizen> GetAllCitizens()
        {
            List<Citizen> list = new List<Citizen>();
            InOrder(root, list);
            return list;
        }

        private void InOrder(BSTNode node, List<Citizen> list)
        {
            if (node == null) return;

            // Duyệt bên trái
            InOrder(node.Left, list);

            // Lấy dữ liệu ở giữa (Gốc)
            list.Add(node.Data);

            // Duyệt bên phải
            InOrder(node.Right, list);
        }
    }
}