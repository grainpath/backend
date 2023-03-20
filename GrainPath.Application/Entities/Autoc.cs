using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GrainPath.Application.Entities;

public sealed class AutocItem
{
    [Required]
    public string keyword { get; set; }

    [Required]
    public List<string> tags { get; set; }
}

public sealed class AutocIndex
{
    private readonly Dictionary<string, List<string>> _tags = new();
    private readonly PruningRadixTrie.PruningRadixTrie _trie = new();

    public void Add(string term, List<string> tags, long freq) {
        _tags[term] = tags;
        _trie.AddTerm(term, freq);
    }

    public List<AutocItem> TopK(string prefix, int count)
    {
        return _trie
            .GetTopkTermsForPrefix(prefix, count, out _)
            .Select((pair) => new AutocItem() { keyword = pair.term, tags = _tags[pair.term] })
            .ToList();
    }
}

public sealed class AutocRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int count { get; set; }

    [Required]
    public string prefix { get; set; }
}
