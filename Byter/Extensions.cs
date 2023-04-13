using Sprache;

namespace Byter;

public static class Extensions
{
    public static Parser<IEnumerable<TItem>> SeparatedBy<TItem, TSeparator>(
        this Parser<TItem> item,
        Parser<TSeparator> separator)
    {
        return input =>
        {
            var result = item(input);
            if (!result.WasSuccessful)
            {
                return Result.Success(Array.Empty<TItem>(), input);
            }

            var items = new List<TItem> { result.Value };
            while (true)
            {
                var separatorResult = separator(result.Remainder);
                if (!separatorResult.WasSuccessful)
                {
                    return Result.Success(items, result.Remainder);
                }

                result = item(separatorResult.Remainder);
                if (!result.WasSuccessful)
                {
                    return Result.Success(items, separatorResult.Remainder);
                }

                items.Add(result.Value);
            }
        };
    }

    public static Parser<None> IgnoreResult<T>(this Parser<T> parser) => parser.Select(_ => new None());
}