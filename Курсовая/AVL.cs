using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая
{
    class AVL
    {
        public class Node
        {

            public Car car { get; set; }

            int lev = 1;

            Node right = null;
            Node left = null;

            public Node(Car car)
            {
                this.car = car;

            }

            public void set_info(Node node, ref uint countP)
            {
                countP++;

                car.num = node.car.num;
                car.year = node.car.year;
                car.name = node.car.name;
            }

            public string get_num() {return car.num; }
            public int get_year() {return car.year; }
            public string get_name() {return car.name; }


            public Node get_left() {; return left; }

            public Node get_right() {return right; }

            public int get_lev() {return lev; }

            public void set_left(Node node) {left = node; }

            public void set_right(Node node) {right = node; }

            public void set_lev(ref uint countP)
            {
                if (right != null && left != null)
                {
                    lev = Math.Max(get_left().get_lev(), get_right().get_lev()) + 1;
                }

                else if (right != null)
                {
                    lev = right.get_lev() + 1;
                }

                else if (left != null)
                {
                    lev = left.get_lev() + 1;
                }

                else
                {
                    lev = 1;
                }

                countP++;

            }

        }

        Node root = null;

        bool More(string s, string v)
        {
            int s1 = s.Length;
            int v1 = v.Length;
            int j;

            if (s1 > v1)
                j = v1;
            else j = s1;

            for (int i = 0; i < j; i++)
            {

                if (s[i] > v[i])
                    return true;
                if (s[i] < v[i])
                    return false;
            }

            if (s1 < v1)
                return false;
            else
                return true;
        }

        private Node Add(Node node, Node mainnode, ref uint countP)
        {
            if (mainnode == null)
            {
                mainnode = node;
                countP++;
            }
            else
            {
                if (!More(node.get_num(), mainnode.get_num()) && node.get_num() != mainnode.get_num())
                {
                    Node tmp = mainnode.get_left();
                    mainnode.set_left(Add(node, tmp, ref countP));

                    if (level(mainnode.get_right()) - level(mainnode.get_left()) < -1)
                        if (!More(node.get_num(), mainnode.get_left().get_num()) && node.get_num() != mainnode.get_left().get_num())
                            mainnode = rotate_right(mainnode, ref countP);
                        else
                            mainnode = rotate_left_right(mainnode, ref countP);
                }
                else
                {
                    Node tmp = mainnode.get_right();
                    mainnode.set_right(Add(node, tmp, ref countP));

                    if (level(mainnode.get_right()) - level(mainnode.get_left()) > 1)
                        if (More(node.get_num(), mainnode.get_right().get_num()))
                            mainnode = rotate_left(mainnode, ref countP);
                        else
                            mainnode = rotate_right_left(mainnode, ref countP);
                }
                mainnode.set_lev(ref countP);
            }
            return mainnode;
        }

        private Node min_num(Node node, ref uint countP)
        {
            while (node.get_left() != null)
            {
                node = node.get_left();
                countP++;
            }

            return node;
        }

        Node del_node(string num, Node mainnode, ref uint countP)
        {
            if(mainnode != null)
            {
                countP++;

                if (!More(num, mainnode.get_num()) && num != mainnode.get_num())
                {

                    Node tmp = mainnode.get_left();
                    mainnode.set_left(del_node(num, tmp, ref countP));

                    if (level(mainnode.get_right()) - level(mainnode.get_left()) > 1)
                    {
                        countP++;

                        if (mainnode.get_right().get_left() == null ||
                            (mainnode.get_right().get_right() != null &&
                            mainnode.get_right().get_left().get_lev() < mainnode.get_right().get_right().get_lev()))
                        {
                            mainnode = rotate_left(mainnode, ref countP);
                            countP++;
                        }
                        else
                        {
                            mainnode = rotate_right_left(mainnode, ref countP);
                            countP++;
                        }
                    }
                }
                else if (More(num, mainnode.get_num()) && num != mainnode.get_num())
                {

                    Node tmp = mainnode.get_right();
                    mainnode.set_right(del_node(num, tmp, ref countP));

                    if (level(mainnode.get_right()) - level(mainnode.get_left()) < -1)
                    {
                        if (mainnode.get_left().get_right() == null ||
                            (mainnode.get_left().get_left() != null &&
                            mainnode.get_left().get_right().get_lev() < mainnode.get_left().get_left().get_lev()))
                        {
                            mainnode = rotate_right(mainnode, ref countP);
                        }
                        else
                        {
                            mainnode = rotate_left_right(mainnode, ref countP);
                        }
                    }
                }
                else if (mainnode.get_left() == null && mainnode.get_right() == null)
                {
                    mainnode = null;
                    countP++;
                }
                else if (mainnode.get_left() == null)
                {
                    mainnode = mainnode.get_right();
                    countP++;
                }
                else if (mainnode.get_right() == null)
                {
                    mainnode = mainnode.get_left();
                    countP++;
                }
                else
                {
                    countP++;

                    Node tmp = mainnode.get_right();
                    mainnode.set_info(min_num(tmp, ref countP), ref countP);
                    mainnode.set_right(del_node(min_num(tmp, ref countP).get_num(), tmp, ref countP));
                }
                if (mainnode != null)
                    mainnode.set_lev(ref countP);
            }
            return mainnode;
        }

        private bool found_node(string num, Node node, ref uint countP)
        {
            countP++;

            if (node == null)
            {
                countP++;
                return false;
            }

            if (num == node.get_num())
            {
                countP++;
                return true;
            }

            if (!More(num, node.get_num()) && num != node.get_num())
            {
                countP++;
                return found_node(num, node.get_left(), ref countP);
            }
            else
            {
                countP++;
                return found_node(num, node.get_right(), ref countP);
            }
        }

        private Node show_node(string num, Node node, ref uint countP)
        {
            countP++;

            if (node == null)
            {
                return null;
            }

            if (num == node.get_num())
            {
                return node;
            }

            if (!More(num, node.get_num()) && num != node.get_num())
            {
                return show_node(num, node.get_left(), ref countP);
            }
            else
            {
                return show_node(num, node.get_right(), ref countP);
            }
        }

        private Node rotate_right(Node node, ref uint countP)
        {
            countP += 3;

            Node tmp = node.get_left();

            node.set_left(tmp.get_right());
            tmp.set_right(node);

            node.set_lev(ref countP);
            tmp.set_lev(ref countP);

            return tmp;
        }

        private Node rotate_left(Node node, ref uint countP)
        {
            countP += 3;

            Node tmp = node.get_right();

            node.set_right(tmp.get_left());
            tmp.set_left(node);

            node.set_lev(ref countP);
            tmp.set_lev(ref countP);

            return tmp;
        }

        Node rotate_left_right(Node node, ref uint countP)
        {
            Node tmp = node.get_left();
            node.set_left(rotate_left(tmp, ref countP));

            return rotate_right(node, ref countP);
        }

        Node rotate_right_left(Node node, ref uint countP)
        {
            Node tmp = node.get_right();
            node.set_right(rotate_right(tmp, ref countP));

            return rotate_left(node, ref countP);
        }

        int level(Node node)
        {
            return node != null ? node.get_lev() : 0;
        }



        public void AddP(Node node, ref uint countP)
        {
            root = Add(node, root, ref countP);
        }

        public void Del(string num, ref uint countP)
        {
            root = del_node(num, root, ref countP);
        }

        public bool Found(string num, ref uint countP)
        {
            return found_node(num, root, ref countP);
        }
        public Node Show(string num, ref uint countP)
        {
            return show_node(num, root, ref countP);
        }

    }
}
