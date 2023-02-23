using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GrainPath.Application.Entities;

public sealed class AutocompleteIndex
{
    private readonly PruningRadixTrie.PruningRadixTrie _trie = new();

    public void Add(string term, long freq) { _trie.AddTerm(term, freq); }

    public List<string> TopK(string prefix, int count)
    {
        var pairs = _trie.GetTopkTermsForPrefix(prefix, count, out _);
        pairs.Sort((t1, t2) => (int)(t2.termFrequencyCount - t1.termFrequencyCount));
        return pairs.Select((pair) => pair.term).ToList();
    }
}

public sealed class AutocompleteRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int count { get; set; }

    [Required]
    public string index { get; set; }

    [Required]
    public string prefix { get; set; }
}
