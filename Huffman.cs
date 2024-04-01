using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PriorityQueue<T>
{
    private List<T> data;
    private IComparer<T> comparer;

    public PriorityQueue(IComparer<T> comparer)
    {
        this.data = new List<T>();
        this.comparer = comparer;
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        int ci = data.Count - 1;
        while (ci > 0 && comparer.Compare(data[ci], data[ci - 1]) < 0)
        {
            T tmp = data[ci];
            data[ci] = data[ci - 1];
            data[ci - 1] = tmp;
            ci--;
        }
    }

    public T Dequeue()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("Queue is empty");
        T ret = data[0];
        data.RemoveAt(0);
        return ret;
    }

    public int Count
    {
        get { return data.Count; }
    }

    public T Min
    {
        get { return data[0]; }
    }

}

// A Tree node
public class Node
{
    public char ch;
    public int freq;
    public Node left, right;

    // Constructor
    public Node(char ch, int freq, Node left, Node right)
    {
        this.ch = ch;
        this.freq = freq;
        this.left = left;
        this.right = right;
    }
}

// Comparison object to be used to order the heap
public class Comp : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        // Highest priority item has lowest frequency
        return x.freq - y.freq;
    }
}

class Huffman
{
    // Traverse the Huffman Tree and store Huffman Codes in a dictionary
    public static void Encode(Node root, string str, Dictionary<char, string> huffmanCode)
    {
        if (root == null)
            return;

        // Found a leaf node
        if (root.left == null && root.right == null)
        {
            huffmanCode[root.ch] = str;
        }

        Encode(root.left, str + "0", huffmanCode);
        Encode(root.right, str + "1", huffmanCode);
    }

    // Builds Huffman Tree and decodes given input text
    // Builds Huffman Tree and decodes given input text
    public static void BuildHuffmanTree(string inputFilename, string outputFilename)
    {
        // Read encoded string from input file
        string encodedStr = File.ReadAllText(inputFilename);

        // Count frequency of appearance of each character and store it in a dictionary
        Dictionary<char, int> freq = new Dictionary<char, int>();
        foreach (char ch in encodedStr)
        {
            if (freq.ContainsKey(ch))
                freq[ch]++;
            else
                freq[ch] = 1;
        }

        // Create a priority queue to store live nodes of Huffman tree;
        var pq = new PriorityQueue<Node>(new Comp());

        // Create a leaf node for each character and add it to the priority queue.
        foreach (var pair in freq)
        {
            pq.Enqueue(new Node(pair.Key, pair.Value, null, null));
        }

        // Do till there is more than one node in the queue
        while (pq.Count != 1)
        {
            // Remove the two nodes of highest priority (lowest frequency) from the queue
            Node left = pq.Dequeue();
            Node right = pq.Dequeue();

            // Create a new internal node with these two nodes as children and with frequency equal to the sum
            // of the two nodes' frequencies. Add the new node to the priority queue.
            int sum = left.freq + right.freq;
            Node newNode = new Node('\0', sum, left, right);
            pq.Enqueue(newNode);
        }

        // Root stores pointer to root of Huffman Tree
        Node root = pq.Dequeue();

        // Traverse the Huffman Tree and store Huffman Codes in a dictionary.
        Dictionary<char, string> huffmanCode = new Dictionary<char, string>();
        Encode(root, "", huffmanCode);

        // Encode the input string using Huffman codes
        StringBuilder encodedText = new StringBuilder();
        foreach (char ch in encodedStr)
        {
            encodedText.Append(huffmanCode[ch]);
        }

        // Write the encoded string (as binary) to output file
        File.WriteAllText(outputFilename, encodedText.ToString());
    }

}
