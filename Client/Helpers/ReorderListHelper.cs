namespace Client.Helpers;

public static class ReorderListHelper
{
    public static void Reorder<T>(List<T> items, int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex || oldIndex < 0 || oldIndex >= items.Count) return;

        var item = items[oldIndex];
        items.RemoveAt(oldIndex);

        if (newIndex < items.Count)
            items.Insert(newIndex, item);
        else
            items.Add(item);
    }

    public static T? GetBelow<T>(List<T> list, int index)
    {
        if (index + 1 < list.Count)
            return list[index + 1];

        return default;
    }
}
