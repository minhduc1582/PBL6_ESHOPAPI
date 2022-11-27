using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;

namespace eshop_pbl6.Helpers.Products
{
    public class Node{
        public Category data{get;set;}
        public List<Node> child{get;set;} = new List<Node>();
    }
    public static class TreeCategory
    {
        public static Node newNode(Category data)
        {
            Node temp = new Node();
            temp.data = data;
            return temp;
        }
        public static void AddNode(Node node,Category newnode)
        {
            if (node == null)
                return;

            // Standard level order traversal code
            // using queue
            Queue<Node> q = new Queue<Node>(); // Create a queue
            q.Enqueue(node); // Enqueue root
            bool isAddNode = false;
            while (q.Count != 0)
            {
                int n = q.Count;

                // If this node has children
                while (n > 0)
                {
                    // Dequeue an item from queue
                    // and print it
                    Node p = q.Peek();
                    q.Dequeue();
                    if(p.data.Id == newnode.ParentId){
                        p.child.Add(newNode(newnode));
                        isAddNode = true;
                    }

                    // Enqueue all children of
                    // the dequeued item
                    for (int i = 0; i < p.child.Count; i++)
                        q.Enqueue(p.child[i]);
                    n--;
                }

                // Print new line between two levels
            }
            if(isAddNode == false) {
                node.child.Add(newNode(newnode));
            }
        }
    }

}