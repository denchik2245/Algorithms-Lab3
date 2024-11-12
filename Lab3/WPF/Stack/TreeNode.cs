public class TreeNode
{
    public char Value; // Значение узла
    public TreeNode Left; // Левый дочерний узел
    public TreeNode Right; // Правый дочерний узел

    public TreeNode(char value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}

public class BinaryTree
{
    public TreeNode Root; // Корень дерева

    public BinaryTree(char rootValue)
    {
        Root = new TreeNode(rootValue);
    }

    // Прямой обход (Корень, Левый, Правый)
    public void PreorderTraversal(TreeNode node)
    {
        if (node == null) return;
        Console.Write(node.Value + " ");
        PreorderTraversal(node.Left);
        PreorderTraversal(node.Right);
    }

    // Симметричный обход (Левый, Корень, Правый)
    public void InorderTraversal(TreeNode node)
    {
        if (node == null) return;
        InorderTraversal(node.Left);
        Console.Write(node.Value + " ");
        InorderTraversal(node.Right);
    }

    // Обратный обход (Левый, Правый, Корень)
    public void PostorderTraversal(TreeNode node)
    {
        if (node == null) return;
        PostorderTraversal(node.Left);
        PostorderTraversal(node.Right);
        Console.Write(node.Value + " ");
    }
}