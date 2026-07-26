# Foundational NLP Text Analysis Tool
[![.NET build](https://github.com/Mohmdamirian/Foundational-NLP-Text-Analysis-Tool/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Mohmdamirian/Foundational-NLP-Text-Analysis-Tool/actions/workflows/dotnet.yml)

A C# console application that analyses plain-text documents using a custom
binary search tree (BST). Each unique word is stored with its frequency and the
line numbers on which it appears.

This project was originally developed as part of MSc coursework and has been
refined for portfolio presentation.

## Features

- Reads any UTF-8 `.txt` file
- Extracts words while supporting Unicode letters and internal apostrophes
- Stores word information in a custom BST
- Displays words in alphabetical order
- Identifies the most frequent and longest words
- Finds the frequency and line locations of a search term
- Handles invalid paths, empty files and invalid menu selections

## Example

```text
Text Analysis Tool
Loaded: sample.txt
Words analysed: 53
Unique words: 43

1. Display all words
2. Show most frequent word
3. Show longest word
4. Find the frequency of a word
5. Find the line numbers of a word
6. Exit
```

## Technologies

- C#
- .NET 8
- Regular expressions
- Object-oriented programming
- Binary search trees
- Recursive traversal

The application has no third-party runtime dependencies.

## Getting started

### Prerequisites

Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

### Run the application

Clone the repository and enter its directory:

```bash
git clone https://github.com/Mohmdamirian/Foundational-NLP-Text-Analysis-Tool.git
cd Foundational-NLP-Text-Analysis-Tool
```

Run it with the included sample:

```bash
dotnet run --project src/TextAnalysisTool -- sample-data/sample.txt
```

You can analyse another text file by replacing the final path:

```bash
dotnet run --project src/TextAnalysisTool -- path/to/your-document.txt
```

If no path is supplied, the program prompts for one. Press Enter at the prompt
to use the bundled sample.

## How it works

1. The program reads the selected file one line at a time.
2. A regular expression extracts normalised words from each line.
3. Each word is inserted into the BST.
4. A new word creates a node; a repeated word updates its frequency and line
   locations.
5. In-order traversal returns the stored words alphabetically.

The main classes are:

| Class | Responsibility |
| --- | --- |
| `Program` | File handling, tokenisation and the console interface |
| `BinaryTree` | Word insertion, search and traversal |
| `Node` | Links a `WordInfo` value to left and right child nodes |
| `WordInfo` | Stores a word, its frequency and unique line numbers |

See [Design and Complexity](docs/design-and-complexity.md) for a fuller
technical discussion.

## Complexity

Let `n` be the number of unique words, `h` the tree height and `k` the number
of stored line locations for a particular word.

| Operation | Typical case* | Worst case |
| --- | ---: | ---: |
| Insert or search | `O(log n)` | `O(n)` |
| List all words | `O(n)` | `O(n)` |
| Most frequent or longest word | `O(n)` | `O(n)` |
| Add a unique line number | `O(k)` | `O(k)` |

\*The typical case assumes a reasonably balanced tree. This implementation is
not self-balancing, so insertion order determines `h`.

## Project structure

```text
text-analysis-tool/
├── docs/
│   └── design-and-complexity.md
├── sample-data/
│   └── sample.txt
├── src/
│   └── TextAnalysisTool/
│       ├── BinaryTree.cs
│       ├── Node.cs
│       ├── Program.cs
│       ├── TextAnalysisTool.csproj
│       └── WordInfo.cs
├── .gitattributes
├── .github/workflows/dotnet.yml
├── .gitignore
├── LICENSE
├── README.md
├── TextAnalysisTool.sln
└── original-academic-report.pdf
```

## Design decisions

A custom BST was selected to demonstrate data-structure implementation and to
provide alphabetical traversal without a separate sorting step. A dictionary
would usually provide faster average lookup, while a self-balancing tree would
offer stronger worst-case guarantees.

The application streams the input with `File.ReadLines`, rather than loading
the entire document into memory. Words are compared using ordinal,
case-insensitive rules and stored in lowercase for consistent output.

## Limitations and future improvements

- The BST is not self-balancing and can degrade to a linked-list shape.
- Recursive operations can use substantial stack space on a highly skewed
  tree.
- Line-number de-duplication uses a linked list and therefore requires a
  linear scan.
- A future version could add an AVL or red-black tree, automated tests,
  benchmarking, export to CSV/JSON and stop-word filtering.
  
## Academic report

The [original academic report](original-academic-report.pdf) is included for
project context. The application was subsequently refined for portfolio
presentation, so some implementation details may differ from the submitted
version.

## Data

Only a small original sample is included. The application can analyse any
plain-text document that you have permission to use. The coursework version
was demonstrated with classic literature, but those large source texts are not
bundled in this portfolio repository.

## Author

[Mohammad Amirian](https://github.com/Mohmdamirian)

## Licence

The source code is available under the [MIT Licence](LICENSE).
