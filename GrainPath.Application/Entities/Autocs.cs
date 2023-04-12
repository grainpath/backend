using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GrainPath.Application.Entities;

public sealed class AutocsRequest
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
    public List<string> attributeList { get; set; }
}

public sealed class AutocsIndex
{
    private readonly PruningRadixTrie.PruningRadixTrie _trie = new();
    private readonly Dictionary<string, List<string>> _attributeLists = new();

    public void Add(string term, List<string> attributeList, long freq) {
        _attributeLists[term] = attributeList;
        _trie.AddTerm(term, freq);
    }

    public List<AutocItem> TopK(string prefix, int count)
    {
        return _trie
            .GetTopkTermsForPrefix(prefix, count, out _)
            .Select((pair) => new AutocItem() { keyword = pair.term, attributeList = _attributeLists[pair.term] })
            .ToList();
    }
}

public sealed class AutocsResponse
{
    [Required]
    public List<AutocItem> items { get; set; }
}
