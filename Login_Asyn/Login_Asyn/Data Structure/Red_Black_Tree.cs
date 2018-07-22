using Login_Asyn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Red_Black_Tree
{
    public class Node
    {
        public static int RED = 0;
        public static int BLACK = 1;

        public int color { set; get; }
        public ACCOUNT data { set; get; }

        public DateTime key { set; get; }

        public Node pLeft { set; get; }
        public Node pRight { set; get; }
        public Node pParent { set; get; }

        public Node()
        {
            color = RED;
        }
    }

    public class Red_Black
    {
        private int Count;
        private int intHashCode;
        private string strIdentifier;

        private Node rbTree;

        public static Node sentinelNode;

        private Node lastNodeFound;

        private Random rand = new Random();

        public Red_Black()
        {
            strIdentifier = base.ToString() + rand.Next();
            intHashCode = rand.Next();

            // set up the sentinel node. the sentinel node is the key to a successfull
            // implementation and for understanding the red-black tree properties.
            sentinelNode = new Node();
            sentinelNode.pLeft = null;
            sentinelNode.pRight = null;
            sentinelNode.pParent = null;
            sentinelNode.color = Node.BLACK;
            rbTree = sentinelNode;
            lastNodeFound = sentinelNode;
        }

        public Red_Black(string strIdentifier)
        {
            intHashCode = rand.Next();
            this.strIdentifier = strIdentifier;
        }

        ///<summary>
        /// Add
        /// args: ByVal key As IComparable, ByVal data As Object
        /// key is object that implements IComparable interface
        /// performance tip: change to use use int type (such as the hashcode)
        ///</summary>
        public void Add(DateTime key, ACCOUNT data)
        {
            if (key == null || data == null)
                return;
            // traverse tree - find where node belongs
            int result = 0;
            // create new node
            Node node = new Node();
            Node temp = rbTree;             // grab the rbTree node of the tree

            while (temp != sentinelNode)
            {   // find Parent
                node.pParent = temp;
                result = key.CompareTo(temp.key);
                if (result == 0)
                    return;
                if (result > 0)
                    temp = temp.pRight;
                else
                    temp = temp.pLeft;
            }

            // setup node
            node.key = key;
            node.data = data;
            node.pLeft = sentinelNode;
            node.pRight = sentinelNode;

            // insert node into tree starting at parent's location
            if (node.pParent != null)
            {
                result = node.key.CompareTo(node.pParent.key);
                if (result > 0)
                    node.pParent.pRight = node;
                else
                    node.pParent.pLeft = node;
            }
            else
                rbTree = node;					// first node added

            RestoreAfterInsert(node);           // restore red-black properities

            lastNodeFound = node;

            Count = Count + 1;
        }
        ///<summary>
        /// RestoreAfterInsert
        /// Additions to red-black trees usually destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
		private void RestoreAfterInsert(Node x)
        {
            // x and y are used as variable names for brevity, in a more formal
            // implementation, you should probably change the names

            Node y;

            // maintain red-black tree properties after adding x
            while (x != rbTree && x.pParent.color == Node.RED)
            {
                // Parent node is .colored red; 
                if (x.pParent == x.pParent.pParent.pLeft)   // determine traversal path			
                {                                       // is it on the Left or Right subtree?
                    y = x.pParent.pParent.pRight;          // get uncle
                    if (y != null && y.color == Node.RED)
                    {   // uncle is red; change x's Parent and uncle to black
                        x.pParent.color = Node.BLACK;
                        y.color = Node.BLACK;
                        // grandparent must be red. Why? Every red node that is not 
                        // a leaf has only black children 
                        x.pParent.pParent.color = Node.RED;
                        x = x.pParent.pParent;    // continue loop with grandparent
                    }
                    else
                    {
                        // uncle is black; determine if x is greater than Parent
                        if (x == x.pParent.pRight)
                        {   // yes, x is greater than Parent; rotate Left
                            // make x a Left child
                            x = x.pParent;
                            RotateLeft(x);
                        }
                        // no, x is less than Parent
                        x.pParent.color = Node.BLACK;    // make Parent black
                        x.pParent.pParent.color = Node.RED;       // make grandparent black
                        RotateRight(x.pParent.pParent);                   // rotate right
                    }
                }
                else
                {   // x's Parent is on the Right subtree
                    // this code is the same as above with "Left" and "Right" swapped
                    y = x.pParent.pParent.pLeft;
                    if (y != null && y.color == Node.RED)
                    {
                        x.pParent.color = Node.BLACK;
                        y.color = Node.BLACK;
                        x.pParent.pParent.color = Node.RED;
                        x = x.pParent.pParent;
                    }
                    else
                    {
                        if (x == x.pParent.pLeft)
                        {
                            x = x.pParent;
                            RotateRight(x);
                        }
                        x.pParent.color = Node.BLACK;
                        x.pParent.pParent.color = Node.RED;
                        RotateLeft(x.pParent.pParent);
                    }
                }
            }
            rbTree.color = Node.BLACK;      // rbTree should always be black
        }

        ///<summary>
        /// RotateLeft
        /// Rebalance the tree by rotating the nodes to the left
        ///</summary>
        public void RotateLeft(Node x)
        {
            // pushing node x down and to the Left to balance the tree. x's Right child (y)
            // replaces x (since y > x), and y's Left child becomes x's Right child 
            // (since it's < y but > x).

            Node y = x.pRight;           // get x's Right node, this becomes y

            // set x's Right link
            x.pRight = y.pLeft;                   // y's Left child's becomes x's Right child

            // modify parents
            if (y.pLeft != sentinelNode)
                y.pLeft.pParent = x;				// sets y's Left Parent to x

            if (y != sentinelNode)
                y.pParent = x.pParent;            // set y's Parent to x's Parent

            if (x.pParent != null)
            {   // determine which side of it's Parent x was on
                if (x == x.pParent.pLeft)
                    x.pParent.pLeft = y;          // set Left Parent to y
                else
                    x.pParent.pRight = y;         // set Right Parent to y
            }
            else
                rbTree = y;                     // at rbTree, set it to y

            // link x and y 
            y.pLeft = x;                         // put x on y's Left 
            if (x != sentinelNode)                      // set y as x's Parent
                x.pParent = y;
        }
        ///<summary>
        /// RotateRight
        /// Rebalance the tree by rotating the nodes to the right
        ///</summary>
        public void RotateRight(Node x)
        {
            // pushing node x down and to the Right to balance the tree. x's Left child (y)
            // replaces x (since x < y), and y's Right child becomes x's Left child 
            // (since it's < x but > y).

            Node y = x.pLeft;            // get x's Left node, this becomes y

            // set x's Right link
            x.pLeft = y.pRight;                   // y's Right child becomes x's Left child

            // modify parents
            if (y.pRight != sentinelNode)
                y.pRight.pParent = x;				// sets y's Right Parent to x

            if (y != sentinelNode)
                y.pParent = x.pParent;            // set y's Parent to x's Parent

            if (x.pParent != null)               // null=rbTree, could also have used rbTree
            {   // determine which side of it's Parent x was on
                if (x == x.pParent.pRight)
                    x.pParent.pRight = y;         // set Right Parent to y
                else
                    x.pParent.pLeft = y;          // set Left Parent to y
            }
            else
                rbTree = y;                     // at rbTree, set it to y

            // link x and y 
            y.pRight = x;                        // put x on y's Right
            if (x != sentinelNode)              // set y as x's Parent
                x.pParent = y;
        }

        public void Delete(Node z)
        {
            // A node to be deleted will be: 
            //		1. a leaf with no children
            //		2. have one child
            //		3. have two children
            // If the deleted node is red, the red black properties still hold.
            // If the deleted node is black, the tree needs rebalancing

            Node x = new Node();    // work node to contain the replacement node
            Node y;                 // work node 

            // find the replacement node (the successor to x) - the node one with 
            // at *most* one child. 
            if (z.pLeft == sentinelNode || z.pRight == sentinelNode)
                y = z;                      // node has sentinel as a child
            else
            {
                // z has two children, find replacement node which will 
                // be the leftmost node greater than z
                y = z.pRight;                        // traverse right subtree	
                while (y.pLeft != sentinelNode)      // to find next node in sequence
                    y = y.pLeft;
            }

            // at this point, y contains the replacement node. it's content will be copied 
            // to the valules in the node to be deleted

            // x (y's only child) is the node that will be linked to y's old parent. 
            if (y.pLeft != sentinelNode)
                x = y.pLeft;
            else
                x = y.pRight;

            // replace x's parent with y's parent and
            // link x to proper subtree in parent
            // this removes y from the chain
            x.pParent = y.pParent;
            if (y.pParent != null)
                if (y == y.pParent.pLeft)
                    y.pParent.pLeft = x;
                else
                    y.pParent.pRight = x;
            else
                rbTree = x;         // make x the root node

            // copy the values from y (the replacement node) to the node being deleted.
            // note: this effectively deletes the node. 
            if (y != z)
            {
                z.key = y.key;
                z.data = y.data;
            }

            if (y.color == Node.BLACK)
                RestoreAfterDelete(x);

            lastNodeFound = sentinelNode;
        }

        ///<summary>
        /// RestoreAfterDelete
        /// Deletions from red-black trees may destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
		private void RestoreAfterDelete(Node x)
        {
            // maintain Red-Black tree balance after deleting node 			

            Node y;

            while (x != rbTree && x.color == Node.BLACK)
            {
                if (x == x.pParent.pLeft)         // determine sub tree from parent
                {
                    y = x.pParent.pRight;         // y is x's sibling 
                    if (y.color == Node.RED)
                    {   // x is black, y is red - make both black and rotate
                        y.color = Node.BLACK;
                        x.pParent.color = Node.RED;
                        RotateLeft(x.pParent);
                        y = x.pParent.pRight;
                    }
                    if (y.pLeft.color == Node.BLACK &&
                        y.pRight.color == Node.BLACK)
                    {   // children are both black
                        y.color = Node.RED;     // change parent to red
                        x = x.pParent;                   // move up the tree
                    }
                    else
                    {
                        if (y.pRight.color == Node.BLACK)
                        {
                            y.pLeft.color = Node.BLACK;
                            y.color = Node.RED;
                            RotateRight(y);
                            y = x.pParent.pRight;
                        }
                        y.color = x.pParent.color;
                        x.pParent.color = Node.BLACK;
                        y.pRight.color = Node.BLACK;
                        RotateLeft(x.pParent);
                        x = rbTree;
                    }
                }
                else
                {   // right subtree - same as code above with right and left swapped
                    y = x.pParent.pLeft;
                    if (y.color == Node.RED)
                    {
                        y.color = Node.BLACK;
                        x.pParent.color = Node.RED;
                        RotateRight(x.pParent);
                        y = x.pParent.pLeft;
                    }
                    if (y.pRight.color == Node.BLACK &&
                        y.pLeft.color == Node.BLACK)
                    {
                        y.color = Node.RED;
                        x = x.pParent;
                    }
                    else
                    {
                        if (y.pLeft.color == Node.BLACK)
                        {
                            y.pRight.color = Node.BLACK;
                            y.color = Node.RED;
                            RotateLeft(y);
                            y = x.pParent.pLeft;
                        }
                        y.color = x.pParent.color;
                        x.pParent.color = Node.BLACK;
                        y.pLeft.color = Node.BLACK;
                        RotateRight(x.pParent);
                        x = rbTree;
                    }
                }
            }
            x.color = Node.BLACK;
        }

        ///<summary>
		/// GetMinkey
		/// Returns the minimum key value
		///<summary>
		public Node GetMinNode()
        {
            Node treeNode = rbTree;

            if (treeNode == sentinelNode)
                return null;

            // traverse to the extreme left to find the smallest key
            while (treeNode.pLeft != sentinelNode)
                treeNode = treeNode.pLeft;

            lastNodeFound = treeNode;

            return treeNode;

        }

        ///<summary>
		/// Clear
		/// Empties or clears the tree
		///<summary>
		public void Clear()
        {
            rbTree = null;
            Count = 0;
        }
        ///<summary>
        /// Size
        /// returns the size (number of nodes) in the tree
        ///<summary>
        public int Size()
        {
            // number of keys
            return Count;
        }

        public bool isEmpty()
        {
            return rbTree == sentinelNode;
        }


        public Node FindNode(ACCOUNT acc)
        {
            int result;
            Node treeNode = rbTree;     // begin at root

            DateTime key = acc.TimeOut ?? DateTime.Now;
            // traverse tree until node is found
            while (treeNode != sentinelNode)
            {
                TimeSpan diff = key - treeNode.key;
                result = DateTime.Compare(key, treeNode.key);
                if (Math.Abs(diff.TotalMilliseconds) < 1 && treeNode.data.UserName == acc.UserName)
                    result = 0;
                if (result == 0)
                {
                    lastNodeFound = treeNode;
                    return treeNode;
                }
                if (result < 0)
                    treeNode = treeNode.pLeft;
                else
                    treeNode = treeNode.pRight;
            }

            return null;
        }
    }
}