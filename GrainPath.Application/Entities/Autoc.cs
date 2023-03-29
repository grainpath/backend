using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GrainPath.Application.Entities;

public sealed class AutocRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int count { get; set; }

    [Required]
    [MinLength(1)]
    public string prefix { get; set; }
}

public sealed class AutocItem
{
    [Required]
    public string keyword { get; set; }

    [Required]
    public List<string> features { get; set; }
}

public sealed class AutocIndex
{
    private readonly PruningRadixTrie.PruningRadixTrie _trie = new();
    private readonly Dictionary<string, List<string>> _features = new();

    public void Add(string term, List<string> features, long freq) {
        _features[term] = features;
        _trie.AddTerm(term, freq);
    }

    public List<AutocItem> TopK(string prefix, int count)
    {
        return _trie
            .GetTopkTermsForPrefix(prefix, count, out _)
            .Select((pair) => new AutocItem() { keyword = pair.term, features = _features[pair.term] })
            .ToList();
    }
}
