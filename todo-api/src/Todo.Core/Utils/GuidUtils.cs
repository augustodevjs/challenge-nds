namespace Todo.Core.Utils;

public static class GuidUtils
{
    public static bool isValidGuid(string? id)
    {
        if (!Guid.TryParse(id, out _)) return false;

        return true;
    }
}