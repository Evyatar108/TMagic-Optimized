# TMagic-Optimized

## Optimizations
* Replaced linear searches over lists with hash sets.
* Removed many creations of new lists objects when not needed.
* Caching computations when possible:
   * Creating lists objects for enumerable when they are iterated more than once.
   * Caching global getters' computation as a static object if they are practiacally immutable.
   * Replace DistanceTo() calls with DistanceToSquared() to reduce Math.Sqrt() calls.
   * Store indexing of arrays in local objects instead of performing multiple indexing calls.
* Replaced string concatination with an equivalent procedure using StringBuilder and string interpolation.
* Reducing reflection operations when possible by using Traverse of the Harmony library.
* Reduced nested loops when possible, avoiding redundant iterations.
* Replaced some LINQ method calls with for and foreach loops.
* Replaced with ContainsKey()+indexing with a single TryGetValue().
* Replaced AddDistinct() calls on lists with hash sets.
