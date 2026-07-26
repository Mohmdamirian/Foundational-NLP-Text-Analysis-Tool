# Design and Complexity

## Objective

The Text Analysis Tool indexes the words in a plain-text document and supports
frequency, position and ordering queries. Its central purpose is to demonstrate
how a custom data structure can support a practical text-processing task.

## Data-structure selection

Three candidate structures were considered.

| Structure | Strengths | Trade-offs |
| --- | --- | --- |
| Dictionary | Average `O(1)` insertion and lookup; concise implementation | Does not maintain alphabetical order without a separate sort |
| Linked list | Dynamic structure; useful for demonstrating references | Search and ordered insertion require `O(n)` traversal |
| Binary search tree | Search and insertion follow the tree height; in-order traversal is alphabetical | An unbalanced tree can degrade to `O(n)` search and insertion |

A BST was selected because ordered traversal is a core requirement and the
project aims to demonstrate a data structure rather than rely entirely on a
built-in collection. This is an educational design choice: for production use,
a dictionary or a self-balancing tree may be more appropriate.

## Object model

### `WordInfo`

Represents one unique, normalised word. It stores:

- the word in lowercase;
- the total number of occurrences; and
- a linked list of the unique line numbers on which it appears.

Repeated occurrences increment the frequency. A line number is appended only
when it is not already present.

### `Node`

Wraps one `WordInfo` instance and contains nullable references to its left and
right child nodes.

### `BinaryTree`

Owns the root node and implements:

- insertion;
- exact word search;
- alphabetical in-order traversal;
- most-frequent-word discovery; and
- longest-word discovery.

Word comparison is ordinal and case-insensitive. When two words tie for
frequency or length, alphabetical order provides deterministic output.

### `Program`

Coordinates input and output. It resolves the file path, streams lines from
the file, extracts words with a Unicode-aware regular expression, builds the
tree and presents the interactive menu.

The tokeniser recognises sequences of letters and permits internal straight or
curly apostrophes. This means that words such as `don't` and `reader’s` are kept
as single tokens, while punctuation and numbers are excluded.

## Processing flow

1. Resolve and validate the input path.
2. Read the document one line at a time.
3. Extract and normalise every valid word.
4. Insert each word and its one-based line number into the BST.
5. Run user-selected queries against the completed index.

## Complexity analysis

The original coursework focused on insertion. The precise cost depends on tree
height, so it is clearer to express it as `O(h)` before discussing balance.

Let:

- `n` = number of unique words;
- `h` = height of the BST;
- `m` = total extracted word occurrences; and
- `k` = line locations already stored for one word.

### Insertion and search

At each node, the algorithm compares the target word with the current word and
follows exactly one branch. The work is therefore `O(h)`.

- Reasonably balanced tree: `h ≈ log n`, so the cost is `O(log n)`.
- Completely skewed tree: `h = n`, so the cost is `O(n)`.

The tree is not self-balancing. Average-case performance therefore depends on
insertion order and should not be presented as a guarantee.

When a word already exists, checking whether its current line number has
already been stored costs `O(k)` because the implementation uses
`LinkedList.Contains`.

### Building the index

Every extracted occurrence is processed once. Ignoring tokenisation cost, tree
work is:

- approximately `O(m log n)` for a reasonably balanced tree; or
- up to `O(mn)` in a severely skewed worst case.

Line-number checks add costs based on the number of locations stored for each
word.

### Traversal and aggregate queries

In-order traversal visits every node once, so listing all words takes `O(n)`.
Finding the longest or most frequent word also visits every node and takes
`O(n)`.

Recursive traversal uses `O(h)` call-stack space.

### Storage

The tree requires `O(n)` nodes. Line information requires additional space
proportional to the total number of distinct word-line pairs. It is therefore
more precise to describe total storage as `O(n + p)`, where `p` is the number
of stored word-line pairs.

## Engineering improvements from the coursework version

- Removed build output and editor-specific state.
- Removed university identifiers from public-facing material.
- Replaced fixed dataset selection with support for any text path.
- Streams input rather than loading the complete file at once.
- Uses Unicode-aware tokenisation and invariant normalisation.
- Handles missing, unreadable and empty files.
- Enables nullable reference checks without relying on null-forgiving
  assumptions.
- Separates portfolio documentation from the original assessment report.
