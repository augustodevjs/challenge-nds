using System.Text.Json.Serialization;

namespace Todo.API.Responses;

public abstract class Response
{
    [JsonPropertyOrder(order: 1)]
    public string Title { get; set; } = null!;
    [JsonPropertyOrder(order: 2)]
    public int Status { get; set; }
}