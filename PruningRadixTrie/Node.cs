/*
 * MIT License
 * 
 * Copyright (c) 2020 Wolf Garbe at https://github.com/wolfgarbe/PruningRadixTrie
 * 
 * Commit: 7459e3c1b2a249ec21d1048f2190181cb65ce501
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Collections.Generic;

namespace PruningRadixTrie
{
    //Trie node class
    public class Node
    {
        public List<(string key, Node node)> Children;

        //Does this node represent the last character in a word? 
        //0: no word; >0: is word (termFrequencyCount)
        public long termFrequencyCount;
        public long termFrequencyCountChildMax;

        public Node(long termfrequencyCount)
        {
            termFrequencyCount = termfrequencyCount;
        }
    }
}
