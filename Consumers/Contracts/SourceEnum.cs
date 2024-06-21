using System.ComponentModel;

namespace Domain.Contracts;

public enum SourceEnum
{
    [Description("Producer1")]
    Producer1 = 0,
    [Description("Producer2")]
    Producer2 = 1,
}
