using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Приоритетная очередь
public class PriorityQueue<T>
{
    private List<T> data;
    private IComparer<T> comparer;

    // Конструктор
    public PriorityQueue(IComparer<T> comparer)
    {
        this.data = new List<T>();
        this.comparer = comparer;
    }

    // Добавление элемента в очередь
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

    // Удаление и возврат элемента с наивысшим приоритетом
    public T Dequeue()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("Queue is empty");
        T ret = data[0];
        data.RemoveAt(0);
        return ret;
    }

    // Количество элементов в очереди
    public int Count
    {
        get { return data.Count; }
    }

    // Элемент с наивысшим приоритетом
    public T Min
    {
        get { return data[0]; }
    }
}

// Узел дерева
public class Node
{
    public char ch;
    public int freq;
    public Node left, right;

    // Конструктор
    public Node(char ch, int freq, Node left, Node right)
    {
        this.ch = ch;
        this.freq = freq;
        this.left = left;
        this.right = right;
    }
}

// Объект для сравнения узлов дерева
public class Comp : IComparer<Node>
{
    // Сравнение узлов по частоте
    public int Compare(Node x, Node y)
    {
        // Элемент с наивысшим приоритетом имеет наименьшую частоту
        return x.freq - y.freq;
    }
}

// Класс для работы с алгоритмом Хаффмана
class Huffman
{
    // Обход дерева Хаффмана и сохранение кодов Хаффмана в словаре
    public static void Encode(Node root, string str, Dictionary<char, string> huffmanCode)
    {
        if (root == null)
            return;

        // Найден листовой узел
        if (root.left == null && root.right == null)
        {
            huffmanCode[root.ch] = str;
        }

        Encode(root.left, str + "0", huffmanCode);
        Encode(root.right, str + "1", huffmanCode);
    }

    // Построение дерева Хаффмана и декодирование заданной входной строки
    public static void BuildHuffmanTree(string inputFilename, string outputFilename)
    {
        // Чтение закодированной строки из входного файла
        string encodedStr = File.ReadAllText(inputFilename);

        // Подсчет частоты каждого символа и сохранение его в словаре
        Dictionary<char, int> freq = new Dictionary<char, int>();
        foreach (char ch in encodedStr)
        {
            if (freq.ContainsKey(ch))
                freq[ch]++;
            else
                freq[ch] = 1;
        }

        // Создание приоритетной очереди для хранения активных узлов дерева Хаффмана
        var pq = new PriorityQueue<Node>(new Comp());

        // Создание листового узла для каждого символа и добавление его в приоритетную очередь
        foreach (var pair in freq)
        {
            pq.Enqueue(new Node(pair.Key, pair.Value, null, null));
        }

        // Построение дерева Хаффмана, пока в очереди не останется более одного узла
        while (pq.Count != 1)
        {
            // Удаление двух узлов с наивысшим приоритетом (наименьшей частотой) из очереди
            Node left = pq.Dequeue();
            Node right = pq.Dequeue();

            // Создание нового внутреннего узла с этими двумя узлами в качестве дочерних и с частотой, равной сумме
            // частот двух узлов. Добавление нового узла в приоритетную очередь.
            int sum = left.freq + right.freq;
            Node newNode = new Node('\0', sum, left, right);
            pq.Enqueue(newNode);
        }

        // Корень дерева Хаффмана
        Node root = pq.Dequeue();

        // Обход дерева Хаффмана и сохранение кодов Хаффмана в словаре
        Dictionary<char, string> huffmanCode = new Dictionary<char, string>();
        Encode(root, "", huffmanCode);

        // Кодирование входной строки с использованием кодов Хаффмана
        StringBuilder encodedText = new StringBuilder();
        foreach (char ch in encodedStr)
        {
            encodedText.Append(huffmanCode[ch]);
        }

        // Запись закодированной строки (в бинарном формате) в выходной файл
        File.WriteAllText(outputFilename, encodedText.ToString());
    }
}
